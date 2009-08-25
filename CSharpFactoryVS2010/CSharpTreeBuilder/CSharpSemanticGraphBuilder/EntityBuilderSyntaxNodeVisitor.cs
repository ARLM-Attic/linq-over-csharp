using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a syntax tree visitor, that builds semantic entities from syntax nodes.
  /// </summary>
  // ================================================================================================
  public class EntityBuilderSyntaxNodeVisitor : BlankSyntaxNodeVisitor
  {
    /// <summary>The semantic graph that will hold the built entities.</summary>
    private SemanticGraph _SemanticGraph;

    /// <summary>Error handler object used for reporting compilation messages.</summary>
    private ICompilationErrorHandler _ErrorHandler;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBuilderSyntaxNodeVisitor"/> class.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph that will receive the built entities.</param>
    /// <param name="errorHandler">Error handler object used for reporting compilation messages.</param>
    // ----------------------------------------------------------------------------------------------
    public EntityBuilderSyntaxNodeVisitor(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _SemanticGraph = semanticGraph;
      _ErrorHandler = errorHandler;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a namespace entity from a namespace declaration.
    /// </summary>
    /// <param name="node">A namespace declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(NamespaceDeclarationNode node)
    {
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Looping through every tag in the namespace name
      foreach (var nameTag in node.NameTags)
      {
        // Find out whether a namespace was already declared with this name
        var namespaceEntity = parentEntity.DeclarationSpace.GetSpecificType<NamespaceEntity>(nameTag.Identifier);

        // If no namespace declared with this name, but the name is already taken, then signal error
        if (namespaceEntity == null && parentEntity.DeclarationSpace.IsNameDefined(nameTag.Identifier))
        {
          ErrorDuplicateNameInNamespace(node.StartToken, parentEntity.FullyQualifiedName, nameTag.Identifier);
          return;
        }
        
        // If no such namespace yet then create it
        if (namespaceEntity == null)
        {
          namespaceEntity = new NamespaceEntity(nameTag.Identifier);
          parentEntity.AddChildNamespace(namespaceEntity);
        }

        // Associate the syntax node with the found or the newly created namespace entity
        AssociateSyntaxNodeWithSemanticEntity(node, namespaceEntity);

        // The next parent is the current entity
        parentEntity = namespaceEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an extern alias entity from an extern alias AST node.
    /// </summary>
    /// <param name="node">An extern alias AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ExternAliasNode node)
    {
      // Determine the parent entity of the to-be-created extern alias entity.
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Determine the scope of the node
      var lexicalScope = GetParentSourceRegion(node);

      // Check error CS1537: The using alias 'alias' appeared previously in this namespace
      // _Note: It should say 'extern alias' instead of 'using alias' but that's how csc.exe works.
      if (parentEntity.IsExternAliasAlreadySpecified(node.Identifier, lexicalScope))
      {
        ErrorDuplicateUsingAlias(node.StartToken, node.Identifier);
        return;
      }

      // Create the extern alias entity and add it to the parent namespace (which can be a root namespace as well).
      var externAliasEntity = new ExternAliasEntity(lexicalScope, node);
      parentEntity.AddExternAlias(externAliasEntity);

      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, externAliasEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using namespace entity from a using namespace AST node.
    /// </summary>
    /// <param name="node">A using namespace AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(UsingNamespaceNode node)
    {
      // Determine the parent entity of the to-be-created using entity.
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Determine the scope of the node
      var lexicalScope = GetParentSourceRegion(node);

      // Check warning CS0105: The using directive for 'namespace' appeared previously in this namespace
      var namespaceName = node.NamespaceOrTypeName.TypeTags.ToString();
      if (parentEntity.IsUsingNamespaceAlreadySpecified(namespaceName, lexicalScope))
      {
        _ErrorHandler.Warning("CS0105", node.StartToken,
                              "The using directive for '{0}' appeared previously in this namespace", namespaceName);
        return;
      }

      // Create the using namespace entity and add it to the parent namespace (which can be a root namespace as well).
      var usingNamespaceEntity = new UsingNamespaceEntity(lexicalScope, node.NamespaceOrTypeName);
      parentEntity.AddUsingNamespace(usingNamespaceEntity);
      
      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, usingNamespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using alias entity from a using alias AST node.
    /// </summary>
    /// <param name="node">A using alias AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(UsingAliasNode node)
    {
      // Determine the parent entity of the to-be-created using entity.
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Determine the scope of the node
      var lexicalScope = GetParentSourceRegion(node);

      // Check error CS1537: The using alias 'alias' appeared previously in this namespace
      if (parentEntity.IsUsingAliasAlreadySpecified(node.Alias, lexicalScope)
        || parentEntity.IsExternAliasAlreadySpecified(node.Alias, lexicalScope))
      {
        ErrorDuplicateUsingAlias(node.StartToken, node.Alias);
        return;
      }

      // Create the using alias entity and add it to the parent namespace (which can be a root namespace as well).
      var usingAliasEntity = new UsingAliasEntity(lexicalScope, node.Alias, node.NamespaceOrTypeName);
      parentEntity.AddUsingAlias(usingAliasEntity);

      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, usingAliasEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a class entity from a class declaration.
    /// </summary>
    /// <param name="node">A class declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ClassDeclarationNode node)
    {
      NamespaceOrTypeEntity parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Find out whether a class was already declared with this name
      var classEntity = parentEntity.DeclarationSpace.GetSpecificType<ClassEntity>(node.NameWithGenericDimensions);

      // If no class declared with this name, but the name is already taken, then signal error
      if (classEntity == null && parentEntity.DeclarationSpace.IsNameDefined(node.NameWithGenericDimensions))
      {
        ReportDuplicateNameError(parentEntity,node);
        return;
      }

      if (classEntity == null)
      {
        // Build the new entity
        classEntity = new ClassEntity(node.Name, node.IsPartial);
        AddBaseTypesToTypeEntity(classEntity, node);
        AddTypeParametersToTypeEntity(classEntity, parentEntity, node);
        AddChildTypeToParent(classEntity, parentEntity);
      }
      else
      {
        MergePartialDeclaration(node, classEntity, parentEntity);
      }

      AssociateSyntaxNodeWithSemanticEntity(node, classEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a struct entity from a struct declaration.
    /// </summary>
    /// <param name="node">A struct declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(StructDeclarationNode node)
    {
      NamespaceOrTypeEntity parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Find out whether a struct was already declared with this name
      var structEntity = parentEntity.DeclarationSpace.GetSpecificType<StructEntity>(node.NameWithGenericDimensions);

      // If no struct declared with this name, but the name is already taken, then signal error
      if (structEntity == null && parentEntity.DeclarationSpace.IsNameDefined(node.NameWithGenericDimensions))
      {
        ReportDuplicateNameError(parentEntity, node);
        return;
      }

      if (structEntity == null)
      {
        // Build the new entity
        structEntity = new StructEntity(node.Name, node.IsPartial);
        AddBaseTypesToTypeEntity(structEntity, node);
        AddTypeParametersToTypeEntity(structEntity, parentEntity, node);
        AddChildTypeToParent(structEntity, parentEntity);
      }
      else
      {
        MergePartialDeclaration(node, structEntity, parentEntity);
      }

      AssociateSyntaxNodeWithSemanticEntity(node, structEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an interface entity from an interface declaration.
    /// </summary>
    /// <param name="node">An interface declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(InterfaceDeclarationNode node)
    {
      NamespaceOrTypeEntity parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Find out whether an interface was already declared with this name
      var interfaceEntity = parentEntity.DeclarationSpace.GetSpecificType<InterfaceEntity>(node.NameWithGenericDimensions);

      // If no interface declared with this name, but the name is already taken, then signal error
      if (interfaceEntity == null && parentEntity.DeclarationSpace.IsNameDefined(node.NameWithGenericDimensions))
      {
        ReportDuplicateNameError(parentEntity, node);
        return;
      }

      if (interfaceEntity == null)
      {
        // Build the new entity
        interfaceEntity = new InterfaceEntity(node.Name, node.IsPartial);
        AddBaseTypesToTypeEntity(interfaceEntity, node);
        AddTypeParametersToTypeEntity(interfaceEntity, parentEntity, node);
        AddChildTypeToParent(interfaceEntity, parentEntity);
      }
      else
      {
        MergePartialDeclaration(node, interfaceEntity, parentEntity);
      }

      AssociateSyntaxNodeWithSemanticEntity(node, interfaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an enum entity from an enum declaration.
    /// </summary>
    /// <param name="node">An enum declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(EnumDeclarationNode node)
    {
      NamespaceOrTypeEntity parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Find out whether an enum was already declared with this name
      var enumEntity = parentEntity.DeclarationSpace.GetSpecificType<EnumEntity>(node.NameWithGenericDimensions);

      // If the enum already exists or the name is already taken by an other entity, then signal error
      if (enumEntity != null || parentEntity.DeclarationSpace.IsNameDefined(node.NameWithGenericDimensions))
      {
        ReportDuplicateNameError(parentEntity, node);
        return;
      }

      // Build the new entity
      enumEntity = new EnumEntity(node.Name);
      AddChildTypeToParent(enumEntity, parentEntity);
      AssociateSyntaxNodeWithSemanticEntity(node, enumEntity);

      // Set the underlying type reference.
      if (node.EnumBase != null)
      {
        enumEntity.UnderlyingTypeReference = new TypeNodeBasedTypeEntityReference(node.EnumBase);
      }
      else
      {
        // If no explicit enum-base then int is assumed.
        enumEntity.UnderlyingTypeReference = new ReflectedTypeBasedTypeEntityReference(typeof (int));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a delegate entity from a delegate declaration.
    /// </summary>
    /// <param name="node">A delegate declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(DelegateDeclarationNode node)
    {
      NamespaceOrTypeEntity parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Find out whether a delegate was already declared with this name
      var delegateEntity = parentEntity.DeclarationSpace.GetSpecificType<DelegateEntity>(node.NameWithGenericDimensions);

      // If the delegate already exists or the name is already taken by an other entity, then signal error
      if (delegateEntity != null || parentEntity.DeclarationSpace.IsNameDefined(node.NameWithGenericDimensions))
      {
        ReportDuplicateNameError(parentEntity, node);
        return;
      }

      // Build the new entity
      delegateEntity = new DelegateEntity(node.Name);
      AddTypeParametersToTypeEntity(delegateEntity, parentEntity, node);
      AddChildTypeToParent(delegateEntity, parentEntity);
      AssociateSyntaxNodeWithSemanticEntity(node, delegateEntity);

      // Set the return type.
      if (node.Type != null)
      {
        delegateEntity.ReturnTypeReference = new TypeNodeBasedTypeEntityReference(node.Type);
      }
      else
      {
        throw new ApplicationException("DelegateDeclarationNode.Type should not be null!");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a field entity from a field declaration.
    /// </summary>
    /// <param name="node">A field declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(FieldDeclarationNode node)
    {
      var parentEntity = GetParentEntity<TypeEntity>(node);

      // Looping through every tag in the field declaration
      foreach (var fieldTag in node.FieldTags)
      {
        // Check if this name is already in use in this declaration space
        if (parentEntity.DeclarationSpace.IsNameDefined(fieldTag.Identifier))
        {
          ErrorDuplicateNameInType(node.StartToken, parentEntity.FullyQualifiedName, node.Identifier);
          // Continue with the next field tag.
          continue;
        }

        // Create a semantic entity and add to its parent.
        var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
        var fieldEntity = new FieldEntity(fieldTag.Identifier, true, typeReference, node.IsStatic);
        parentEntity.AddMember(fieldEntity);

        AssociateSyntaxNodeWithSemanticEntity(fieldTag, fieldEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an enum member entity from an enum value AST node.
    /// </summary>
    /// <param name="node">An enum value AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(EnumValueNode node)
    {
      var parentEntity = GetParentEntity<EnumEntity>(node);

      // Check if this name is already in use in this declaration space
      if (parentEntity.DeclarationSpace.IsNameDefined(node.Identifier))
      {
        ErrorDuplicateNameInType(node.StartToken, parentEntity.FullyQualifiedName, node.Identifier);
        return;
      }

      // Create a semantic entity and add to its parent.
      var enumMemberEntity = new EnumMemberEntity(node.Identifier, parentEntity.UnderlyingTypeReference);
      parentEntity.AddMember(enumMemberEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, enumMemberEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a field entity from a field declaration.
    /// </summary>
    /// <param name="node">A field declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(PropertyDeclarationNode node)
    {
      var parentEntity = GetParentEntity<TypeEntity>(node);

      // Check if this name is already in use in this declaration space
      if (parentEntity.DeclarationSpace.IsNameDefined(node.Identifier))
      {
        ErrorDuplicateNameInType(node.StartToken, parentEntity.FullyQualifiedName, node.Identifier);
        return;
      }

      // The property is auto-implemented if both get and set accessors are abstract
      var isAutoImplemented = node.GetAccessor != null && !node.GetAccessor.HasBody
                              && node.SetAccessor != null && !node.SetAccessor.HasBody;

      // Create a semantic entity and add to its parent.
      var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
      var propertyEntity = new PropertyEntity(node.Name, true, typeReference, node.IsStatic, isAutoImplemented);
      parentEntity.AddMember(propertyEntity);
      AssociateSyntaxNodeWithSemanticEntity(node, propertyEntity);

      // Create the accessors and add to the entity
      propertyEntity.GetAccessor = CreateAccessor(node.GetAccessor);
      propertyEntity.SetAccessor = CreateAccessor(node.SetAccessor);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity that will be the parent of a new entity created from a syntax node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>The parent of the entity to be created from the given syntax node.</returns>
    /// <remarks>
    /// Walks the syntax tree up from the given node, looking for a syntax node 
    /// that already has an associated semantic entity.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity GetParentEntity(ISyntaxNode node)
    {
      // Going up in the syntax tree, looking for a node that has a semantic entity
      while (node.Parent.SemanticEntities.Count==0 && !(node.Parent is CompilationUnitNode))
      {
        node = node.Parent;
      }

      // If we reached the compilation unit level, then the entity will be created under the "global" namespace.
      if (node.Parent is CompilationUnitNode)
      {
        return _SemanticGraph.GlobalNamespace;
      }

      // If the found node has only 1 semantic entity associated, then this will be the parent entity.
      var parentEntityCount = node.Parent.SemanticEntities.Count;
      if (parentEntityCount == 1)
      {
        return node.Parent.SemanticEntities[0];
      }

      // If the found node is a NamespaceDeclarationNode, then it can have several entities associated with it
      // (eg. namespace A.B.C where A, B, C are all distinct semantic entities), 
      // then the parent entity is the last in the list.
      if (node.Parent is NamespaceDeclarationNode)
      {
        return node.Parent.SemanticEntities[parentEntityCount - 1];
      }
      
      // All other cases are error.
      throw new ApplicationException("Cannot determine parent entity.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent entity, with a specified expected entity type.
    /// </summary>
    /// <typeparam name="TExpectedEntityType">The expected parent entity type. A subclass of SemanticEntity.</typeparam>
    /// <param name="node">A syntax node.</param>
    /// <returns>The parent entity of the expected type.</returns>
    /// <remarks>If the parent entity found is not of the expected type then an exception is thrown.</remarks>
    // ----------------------------------------------------------------------------------------------
    private TExpectedEntityType GetParentEntity<TExpectedEntityType>(ISyntaxNode node) where TExpectedEntityType : SemanticEntity
    {
      SemanticEntity entity = GetParentEntity(node);

      if (entity is TExpectedEntityType)
      {
        return entity as TExpectedEntityType;
      }

      throw new ApplicationException(
        String.Format("Parent expected to be '{0}' but was '{1}'.", typeof(TExpectedEntityType), entity.GetType()));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Establishis a bi-directional link between an AST (abstract syntax tree) node and an SG (semantic graph) node.
    /// </summary>
    /// <param name="syntaxNode">A syntax tree node.</param>
    /// <param name="semanticEntity">A semantic entity node.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AssociateSyntaxNodeWithSemanticEntity(ISyntaxNode syntaxNode, SemanticEntity semanticEntity)
    {
      semanticEntity.AddSyntaxNode(syntaxNode);
      syntaxNode.SemanticEntities.Add(semanticEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error: The namespace '{0}' already contains a definition for '{1}'
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="namespaceName">The name of the namespace.</param>
    /// <param name="identifier">The name of the identifier.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorDuplicateNameInNamespace(Token errorPoint, string namespaceName, string identifier)
    {
      _ErrorHandler.Error("CS0101", errorPoint,
                          "The namespace '{0}' already contains a definition for '{1}'.",
                          namespaceName, identifier);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error: The type '{0}' already contains a definition for '{1}'
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="typeName">The name of the type.</param>
    /// <param name="identifier">The name of the identifier.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorDuplicateNameInType(Token errorPoint, string typeName, string identifier)
    {
      _ErrorHandler.Error("CS0102", errorPoint, "The type '{0}' already contains a definition for '{1}'.",
                          typeName, identifier);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error: The using alias '{0}' appeared previously in this namespace
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="identifier">The name of the alias.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorDuplicateUsingAlias(Token errorPoint, string identifier)
    {
      _ErrorHandler.Error("CS1537", errorPoint, "The using alias '{0}' appeared previously in this namespace",
                          identifier);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error: Missing partial modifier on declaration of type '{0}' ...
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="identifier">The name of the type.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorMissingPartialModifier(Token errorPoint, string identifier)
    {
      _ErrorHandler.Error("CS0260", errorPoint,
                          "Missing partial modifier on declaration of type '{0}'; another partial declaration of this type exists",
                          identifier);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reports the right error message for a duplicate name.
    /// </summary>
    /// <param name="namespaceOrTypeEntity">The entity where the duplication occured.</param>
    /// <param name="node">The type declaration AST node that caused the duplicaton.</param>
    // ----------------------------------------------------------------------------------------------
    private void ReportDuplicateNameError(NamespaceOrTypeEntity namespaceOrTypeEntity, TypeDeclarationNode node)
    {
      if (namespaceOrTypeEntity is NamespaceEntity)
      {
        ErrorDuplicateNameInNamespace(node.StartToken, namespaceOrTypeEntity.FullyQualifiedName,
                                      node.NameWithGenericDimensions);
      }
      else
      {
        ErrorDuplicateNameInType(node.StartToken, namespaceOrTypeEntity.FullyQualifiedName,
                                 node.NameWithGenericDimensions);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source region of the parent node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>The parent node's source region.</returns>
    // ----------------------------------------------------------------------------------------------
    private static SourceRegion GetParentSourceRegion(ISyntaxNode node)
    {
      if (node.Parent==null)
      {
        throw new ApplicationException("Lexical scope cannot be determined because parent is null.");
      }

      var compilationUnitNode = node.CompilationUnitNode;

      var sourceRegion = new SourceRegion(
        new SourcePoint(compilationUnitNode, node.Parent.StartPosition),
        new SourcePoint(compilationUnitNode, node.Parent.EndPosition));

      return sourceRegion;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Add base types from a type declaration AST node to a type entity.
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    /// <param name="typeNode">A type declaration AST node.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddBaseTypesToTypeEntity(TypeEntity typeEntity, TypeDeclarationNode typeNode)
    {
      foreach (var baseType in typeNode.BaseTypes)
      {
        typeEntity.AddBaseTypeReference(new TypeNodeBasedTypeEntityReference(baseType));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds type parameters to a type entity from its parent and its AST declaration node.
    /// </summary>
    /// <param name="typeEntity">The type entity that will receive the type parameters.</param>
    /// <param name="parentEntity">The (to-be) parent of typeEntity.</param>
    /// <param name="typeNode">An AST node that is the declaration of typeEntity.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddTypeParametersToTypeEntity(
      GenericCapableTypeEntity typeEntity, NamespaceOrTypeEntity parentEntity, TypeDeclarationNode typeNode)
    {
      // First add the (inherited) type parameters from the parent type
      if (parentEntity is GenericCapableTypeEntity)
      {
        foreach (var typeParameterEntity in ((GenericCapableTypeEntity) parentEntity).AllTypeParameters)
        {
          typeEntity.AddTypeParameter(typeParameterEntity);
        }
      }

      // Then add the "own" type parameters
      foreach (var typeParameter in typeNode.TypeParameters)
      {
        var typeParameterEntity = new TypeParameterEntity(typeParameter.Identifier);
        typeEntity.AddTypeParameter(typeParameterEntity);
        AssociateSyntaxNodeWithSemanticEntity(typeParameter, typeParameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type to a parent entity, if possible.
    /// </summary>
    /// <param name="childType">A child type.</param>
    /// <param name="parentEntity">The parent entity</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddChildTypeToParent(TypeEntity childType, NamespaceOrTypeEntity parentEntity)
    {
      // Add the entity to its parent (but only if the parent can have child types)
      if (parentEntity is IHasChildTypes)
      {
        ((IHasChildTypes)parentEntity).AddChildType(childType);
      }
      else
      {
        // This case is not allowed by the syntax analyzer, but we check anyway to be future-proof...
        throw new ApplicationException(string.Format("'{0}' cannot have child types.", parentEntity.GetType()));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Merges a partial type declaration into a type entity.
    /// </summary>
    /// <param name="typeDeclaration">A type declaration AST node.</param>
    /// <param name="typeEntity">A type entity.</param>
    /// <param name="parentEntity">The parent of type entity.</param>
    // ----------------------------------------------------------------------------------------------
    private void MergePartialDeclaration(TypeDeclarationNode typeDeclaration, TypeEntity typeEntity, NamespaceOrTypeEntity parentEntity)
    {
      // If nor the type entity neither this declaration is partial then report duplicate name error
      if (!typeEntity.IsPartial && !typeDeclaration.IsPartial)
      {
        ReportDuplicateNameError(parentEntity, typeDeclaration);
        return;
      }

      // If the entity is partial but this declaration is not (or vica versa) then report missing partial error
      if ((typeEntity.IsPartial && !typeDeclaration.IsPartial)
          || (!typeEntity.IsPartial && typeDeclaration.IsPartial))
      {
        Token typeEntityErrorPoint = null;
        if (typeEntity.SyntaxNodes.Count == 1 && typeEntity.SyntaxNodes[0] != null)
        {
          typeEntityErrorPoint = typeEntity.SyntaxNodes[0].StartToken;
        }

        var errorPoint = typeDeclaration.IsPartial ? typeEntityErrorPoint : typeDeclaration.StartToken;

        ErrorMissingPartialModifier(errorPoint, typeEntity.Name);
        return;
      }

      // This partial type declaration must be merged with the already created type entity.
      // Base class: if present then must be the same --> it will be checked after base type resolution.
      // Base interfaces: union --> duplicates will be eliminated after base type resolution.
      AddBaseTypesToTypeEntity(typeEntity, typeDeclaration);

      // TODO:
      // Type parameters: must be the same (number, name and order)
      // Accesibility modifiers: must be the same
      // If one or more partial declarations of a class include an abstract modifier, the class is considered abstract 
      // If one or more partial declarations of a class include a sealed modifier, the class is considered sealed 
      // Type constraints: must be the same or completely missing
      // When the unsafe modifier is used on a partial type declaration, only that particular part is considered an unsafe context 
      // Attributes: combined
      // Members: union
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an accessor entity from an accessor AST node.
    /// </summary>
    /// <param name="node">An accessor AST node.</param>
    /// <returns>An accessor entity, or null if the AST node was null.</returns>
    // ----------------------------------------------------------------------------------------------
    private static AccessorEntity CreateAccessor(AccessorNode node)
    {
      if (node==null)
      {
        return null;
      }

      var accessor = new AccessorEntity(!node.HasBody);
      AssociateSyntaxNodeWithSemanticEntity(node, accessor);

      return accessor;
    }

    #endregion
  }
}
