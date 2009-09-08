﻿using System;
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
    private readonly SemanticGraph _SemanticGraph;

    /// <summary>Error handler object used for reporting compilation messages.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

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
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(NamespaceDeclarationNode node)
    {
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Looping through every tag in the namespace name
      foreach (var nameTag in node.NameTags)
      {
        // Find out whether this child namespace already exists
        var namespaceEntity = parentEntity.GetChildNamespace(nameTag.Identifier);
        
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

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an extern alias entity from an extern alias AST node.
    /// </summary>
    /// <param name="node">An extern alias AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ExternAliasNode node)
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
        return false;
      }

      // Create the extern alias entity and add it to the parent namespace (which can be a root namespace as well).
      var externAliasEntity = new ExternAliasEntity(lexicalScope, node);
      parentEntity.AddExternAlias(externAliasEntity);

      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, externAliasEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using namespace entity from a using namespace AST node.
    /// </summary>
    /// <param name="node">A using namespace AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(UsingNamespaceNode node)
    {
      // Determine the parent entity of the to-be-created using entity.
      NamespaceEntity parentEntity = GetParentEntity<NamespaceEntity>(node);

      // Determine the scope of the node
      var lexicalScope = GetParentSourceRegion(node);

      // Check warning CS0105: The using directive for 'namespace' appeared previously in this namespace
      var namespaceName = node.NamespaceOrTypeName.TypeTags.ToString();
      if (parentEntity.IsUsingNamespaceAlreadySpecified(namespaceName, lexicalScope))
      {
        ErrorDuplicateUsingNamespace(node.StartToken, namespaceName);
        return false;
      }

      // Create the using namespace entity and add it to the parent namespace (which can be a root namespace as well).
      var usingNamespaceEntity = new UsingNamespaceEntity(lexicalScope, node.NamespaceOrTypeName);
      parentEntity.AddUsingNamespace(usingNamespaceEntity);
      
      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, usingNamespaceEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using alias entity from a using alias AST node.
    /// </summary>
    /// <param name="node">A using alias AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(UsingAliasNode node)
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
        return false;
      }

      // Create the using alias entity and add it to the parent namespace (which can be a root namespace as well).
      var usingAliasEntity = new UsingAliasEntity(lexicalScope, node.Alias, node.NamespaceOrTypeName);
      parentEntity.AddUsingAlias(usingAliasEntity);

      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, usingAliasEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a class entity from a class declaration.
    /// </summary>
    /// <param name="node">A class declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ClassDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Cast the parent to a child-type-capable entity.
      IHasChildTypes childTypeCapableParentEntity = CastToChildTypeCapableEntity(parentEntity);
      
      var classEntity = new ClassEntity(node.Name, node.IsPartial);
      AddBaseTypesToTypeEntity(classEntity, node);
      AddTypeParametersToEntity(classEntity, parentEntity, node.TypeParameters);
      childTypeCapableParentEntity.AddChildType(classEntity);
      
      AssociateSyntaxNodeWithSemanticEntity(node, classEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a struct entity from a struct declaration.
    /// </summary>
    /// <param name="node">A struct declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(StructDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Cast the parent to a child-type-capable entity.
      IHasChildTypes childTypeCapableParentEntity = CastToChildTypeCapableEntity(parentEntity);

      var structEntity = new StructEntity(node.Name, node.IsPartial);
      AddBaseTypesToTypeEntity(structEntity, node);
      AddTypeParametersToEntity(structEntity, parentEntity, node.TypeParameters);
      childTypeCapableParentEntity.AddChildType(structEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, structEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an interface entity from an interface declaration.
    /// </summary>
    /// <param name="node">An interface declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(InterfaceDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Cast the parent to a child-type-capable entity.
      IHasChildTypes childTypeCapableParentEntity = CastToChildTypeCapableEntity(parentEntity);

      var interfaceEntity = new InterfaceEntity(node.Name, node.IsPartial);
      AddBaseTypesToTypeEntity(interfaceEntity, node);
      AddTypeParametersToEntity(interfaceEntity, parentEntity, node.TypeParameters);
      childTypeCapableParentEntity.AddChildType(interfaceEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, interfaceEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an enum entity from an enum declaration.
    /// </summary>
    /// <param name="node">An enum declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(EnumDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Cast the parent to a child-type-capable entity.
      IHasChildTypes childTypeCapableParentEntity = CastToChildTypeCapableEntity(parentEntity);

      // Build the new entity
      var enumEntity = new EnumEntity(node.Name);
      childTypeCapableParentEntity.AddChildType(enumEntity);
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

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a delegate entity from a delegate declaration.
    /// </summary>
    /// <param name="node">A delegate declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(DelegateDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<NamespaceOrTypeEntity>(node);

      // Cast the parent to a child-type-capable entity.
      IHasChildTypes childTypeCapableParentEntity = CastToChildTypeCapableEntity(parentEntity);

      // Build the new entity
      var delegateEntity = new DelegateEntity(node.Name);
      AddTypeParametersToEntity(delegateEntity, parentEntity, node.TypeParameters);
      childTypeCapableParentEntity.AddChildType(delegateEntity);
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

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a field entity from a field declaration.
    /// </summary>
    /// <param name="node">A field declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(FieldDeclarationNode node)
    {
      var parentEntity = GetParentEntity<TypeEntity>(node);

      // Looping through every tag in the field declaration
      foreach (var fieldTag in node.FieldTags)
      {
        // Create a semantic entity and add to its parent.
        var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
        var initializer = CreateInitializer(fieldTag.Initializer);
        var fieldEntity = new FieldEntity(fieldTag.Identifier, true, typeReference, node.IsStatic, initializer);
        parentEntity.AddMember(fieldEntity);

        AssociateSyntaxNodeWithSemanticEntity(fieldTag, fieldEntity);
      }

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an enum member entity from an enum value AST node.
    /// </summary>
    /// <param name="node">An enum value AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(EnumValueNode node)
    {
      var parentEntity = GetParentEntity<EnumEntity>(node);

      // Create a semantic entity and add to its parent.
      var enumMemberEntity = new EnumMemberEntity(node.Identifier, parentEntity.UnderlyingTypeReference,
                                                  node.Expression != null);
      parentEntity.AddMember(enumMemberEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, enumMemberEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a property entity from a property declaration.
    /// </summary>
    /// <param name="node">A property declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(PropertyDeclarationNode node)
    {
      var parentEntity = GetParentEntity<TypeEntity>(node);

      // The property is auto-implemented if both get and set accessors are abstract
      var isAutoImplemented = node.GetAccessor != null && !node.GetAccessor.HasBody
                              && node.SetAccessor != null && !node.SetAccessor.HasBody;

      // Create a semantic entity and add to its parent.
      var interfaceReference = node.InterfaceType != null
                                 ? new NamespaceOrTypeNameNodeBasedTypeEntityReference(node.InterfaceType)
                                 : null;
      var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
      var propertyEntity = new PropertyEntity(node.Name, interfaceReference, true, typeReference, node.IsStatic,
                                              isAutoImplemented);

      parentEntity.AddMember(propertyEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, propertyEntity);

      // Create the accessors and add to the entity
      propertyEntity.GetAccessor = CreateAccessor(node.GetAccessor);
      propertyEntity.SetAccessor = CreateAccessor(node.SetAccessor);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a method entity from a method declaration.
    /// </summary>
    /// <param name="node">A method declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(MethodDeclarationNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<TypeEntity>(node);

      var interfaceReference = node.InterfaceType != null
                                 ? new NamespaceOrTypeNameNodeBasedTypeEntityReference(node.InterfaceType)
                                 : null;
      var isAbstract = (node.Body == null);
      var returnTypeReference = new TypeNodeBasedTypeEntityReference(node.Type);

      var methodEntity = new MethodEntity(node.Name, interfaceReference, true, isAbstract, node.IsPartial, node.IsStatic,
                         returnTypeReference);

      AddTypeParametersToEntity(methodEntity, parentEntity, node.TypeParameters);
      AddParametersToOverloadableEntity(methodEntity, node.FormalParameters);

      parentEntity.AddMember(methodEntity);
      
      AssociateSyntaxNodeWithSemanticEntity(node, methodEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a null literal entity from a null literal AST node.
    /// </summary>
    /// <param name="node">A null literal AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(NullLiteralNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<SemanticEntity>(node);

      var hasExpressions = CastToIHasExpressions(parentEntity);

      var nullLiteralEntity = new NullLiteralExpressionEntity();
      hasExpressions.AddExpression(nullLiteralEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, nullLiteralEntity);

      return true;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity that belongs to the parent of a syntax node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>The parent entity</returns>
    // ----------------------------------------------------------------------------------------------
    private TExpectedEntityType GetParentEntity<TExpectedEntityType>(ISyntaxNode node)
      where TExpectedEntityType : SemanticEntity
    {
      if (node == null)
      {
        throw new ArgumentNullException("node");
      }

      var parentNode = node.Parent;

      // Find the first parent that is either a CompilationUnitNode or has semantic entities associated
      while (parentNode != null 
        && !(parentNode is CompilationUnitNode)
        && (parentNode.SemanticEntities == null || parentNode.SemanticEntities.Count == 0))
      {
        parentNode = parentNode.Parent;
      }

      if (parentNode == null)
      {
        throw new ApplicationException("No parent node found.");
      }

      if (parentNode.SemanticEntities == null)
      {
        throw new ApplicationException("SemanticEntites should not be null.");
      }

      // If we are at the compilation unit level, then the entity will be created under the "global" namespace.
      if (parentNode is CompilationUnitNode)
      {
        return _SemanticGraph.GlobalNamespace as TExpectedEntityType;
      }

      // If the parent node has only 1 semantic entity associated, then this will be the parent entity.
      var semanticEntityCount = parentNode.SemanticEntities.Count;
      if (semanticEntityCount == 1)
      {
        return parentNode.SemanticEntities[0] as TExpectedEntityType;
      }

      // If the parent node is a NamespaceDeclarationNode, then it can have several entities associated with it
      // (eg. namespace A.B.C where A, B, C are all distinct semantic entities), 
      // then the parent entity is the last in the list.
      if (parentNode is NamespaceDeclarationNode && semanticEntityCount >= 1)
      {
        return parentNode.SemanticEntities[semanticEntityCount - 1] as TExpectedEntityType;
      }

      // If the parent node has more than 1 semantic entites, that's an unexpected ambiguity. 
      if (semanticEntityCount > 0)
      {
        throw new ApplicationException(
          string.Format("Unexpected number of semantic entities ('{0}') on node of type: '{1}'",
                        semanticEntityCount, parentNode.GetType()));
      }

      throw new ApplicationException("The parent node has no semantic entity associated.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks whether a namespace-or-type entity can have child types, and returns an IHasChildTypes
    /// interface is possible.
    /// </summary>
    /// <param name="namespaceOrTypeEntity">A namespace or type entity.</param>
    /// <returns>An IHasChildTypes interface.</returns>
    /// <remarks>Throws an AppicationException if the cast was not successful.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static IHasChildTypes CastToChildTypeCapableEntity(NamespaceOrTypeEntity namespaceOrTypeEntity)
    {
      var childTypeCapableEntity = namespaceOrTypeEntity as IHasChildTypes;
      if (childTypeCapableEntity == null)
      {
        // This case is not allowed by the syntax analyzer, but we check anyway to be future-proof...
        throw new ApplicationException(string.Format("'{0}' cannot have child types.", namespaceOrTypeEntity.GetType()));
      }
      return childTypeCapableEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks whether a semantic entity implements the IHasExpressions interface.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>An IHasExpressions interface.</returns>
    /// <remarks>Throws an AppicationException if the cast was not successful.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static IHasExpressions CastToIHasExpressions(SemanticEntity entity)
    {
      var hasExpressions = entity as IHasExpressions;
      if (hasExpressions == null)
      {
        throw new ApplicationException(
          string.Format("Expected a type that has expressions, but found '{0}'.", entity.GetType()));
      }
      return hasExpressions;
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
    /// Adds type parameters to an entity from its parent and its AST declaration node.
    /// </summary>
    /// <param name="typeParameterHolder">An entity that will receive the type parameters.</param>
    /// <param name="parentEntity">The (to-be) parent of entity.</param>
    /// <param name="typeParameterNodes">A collection of type parameter AST nodes.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddTypeParametersToEntity(
      ICanHaveTypeParameters typeParameterHolder, SemanticEntity parentEntity, TypeParameterNodeCollection typeParameterNodes)
    {
      // First add the (inherited) type parameters from the parent type
      if (parentEntity is GenericCapableTypeEntity)
      {
        foreach (var typeParameterEntity in ((GenericCapableTypeEntity) parentEntity).AllTypeParameters)
        {
          typeParameterHolder.AddTypeParameter(typeParameterEntity);
        }
      }

      // Then add the "own" type parameters
      foreach (var typeParameter in typeParameterNodes)
      {
        var typeParameterEntity = new TypeParameterEntity(typeParameter.Identifier);
        typeParameterHolder.AddTypeParameter(typeParameterEntity);
        AssociateSyntaxNodeWithSemanticEntity(typeParameter, typeParameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds parameters to an entity.
    /// </summary>
    /// <param name="overloadableEntity">An overloadable entity that will receive the parameter.</param>
    /// <param name="parameterNodes">A collection of parameter AST nodes.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddParametersToOverloadableEntity(
      IOverloadableEntity overloadableEntity, FormalParameterNodeCollection parameterNodes)
    {
      foreach (var parameter in parameterNodes)
      {
        var typeReference = new TypeNodeBasedTypeEntityReference(parameter.Type);

        var parameterKind = ParameterKind.Value;
        switch (parameter.Modifier)
        {
          case (FormalParameterModifier.In):
            parameterKind = ParameterKind.Value;
            break;
          case (FormalParameterModifier.Out):
            parameterKind = ParameterKind.Output;
            break;
          case (FormalParameterModifier.Ref):
            parameterKind = ParameterKind.Reference;
            break;
        }

        var parameterEntity = new ParameterEntity(parameter.Identifier, typeReference, parameterKind);
        overloadableEntity.AddParameter(parameterEntity);
        AssociateSyntaxNodeWithSemanticEntity(parameter, parameterEntity);
      }
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a variable initializer entity from an AST node.
    /// </summary>
    /// <param name="node">A variable initializer AST node.</param>
    /// <returns>A variable initializer entity.</returns>
    // ----------------------------------------------------------------------------------------------
    private static IVariableInitializer CreateInitializer(VariableInitializerNode node)
    {
      if (node == null)
      {
        return null;
      }

      SemanticEntity result = null;

      if (node is ArrayInitializerNode)
      {
        var arrayInitializer = new ArrayInitializerEntity();

        foreach (var variableInitializer in (node as ArrayInitializerNode).VariableInitializers)
        {
          arrayInitializer.AddVariableInitializer(CreateInitializer(variableInitializer));
        }

        result = arrayInitializer;
      }
      else if (node is ExpressionInitializerNode)
      {
        result = new ScalarInitializerEntity();
      }
      else if (node is StackAllocInitializerNode)
      {
        throw new NotImplementedException("StackAllocInitializerNode-to-entity not yet implemented.");
      }
      else
      {
        throw new ArgumentException(
          string.Format("Unexpected VariableInitializerNode type: '{0}'.", node.GetType()), "node");
      }

      AssociateSyntaxNodeWithSemanticEntity(node, result);

      return result as IVariableInitializer;
    }

    #endregion

    #region Error reporting private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error CS1537: The using alias '{0}' appeared previously in this namespace
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="identifier">The name of the duplicated alias.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorDuplicateUsingAlias(Token errorPoint, string identifier)
    {
      _ErrorHandler.Error("CS1537", errorPoint, "The using alias '{0}' appeared previously in this namespace",
                          identifier);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error CS0105: The using directive for '{0}' appeared previously in this namespace
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    /// <param name="identifier">The name of the duplicated namespace.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorDuplicateUsingNamespace(Token errorPoint, string identifier)
    {
      _ErrorHandler.Warning("CS0105", errorPoint,
                            "The using directive for '{0}' appeared previously in this namespace", identifier);
    }

    #endregion
  }
}
