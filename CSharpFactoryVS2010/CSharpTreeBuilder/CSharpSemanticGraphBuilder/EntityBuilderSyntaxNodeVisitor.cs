using System;
using System.Linq;
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
    /// <summary>
    /// The program entity representing the source project under processing.
    /// </summary>
    private readonly Program _Program;

    /// <summary>
    /// Error handler object used for reporting compilation messages.
    /// </summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>
    /// The semantic graph that will hold the built entities.
    /// </summary>
    private readonly SemanticGraph _SemanticGraph;
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBuilderSyntaxNodeVisitor"/> class.
    /// </summary>
    /// <param name="project">The project containing the source under processing.</param>
    // ----------------------------------------------------------------------------------------------
    public EntityBuilderSyntaxNodeVisitor(CSharpProject project)
    {
      _Program = new Program(project, null);
      _ErrorHandler = project;
      _SemanticGraph = project.SemanticGraph;
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
      externAliasEntity.Program = _Program;
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
      usingNamespaceEntity.Program = _Program;
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
      usingAliasEntity.Program = _Program;
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
      
      var classEntity = new ClassEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (classEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      classEntity.IsStatic = IsStatic(node.Modifiers);
      classEntity.IsAbstract = IsAbstract(node.Modifiers);
      classEntity.IsSealed = IsSealed(node.Modifiers);
      classEntity.Program = _Program;
      AddBaseTypesToTypeEntity(classEntity, node);
      AddTypeParametersToEntity(classEntity, parentEntity, node.TypeParameters, node.TypeParameterConstraints);
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

      var structEntity = new StructEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (structEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      structEntity.Program = _Program;
      AddBaseTypesToTypeEntity(structEntity, node);
      AddTypeParametersToEntity(structEntity, parentEntity, node.TypeParameters, node.TypeParameterConstraints);
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

      var interfaceEntity = new InterfaceEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (interfaceEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      interfaceEntity.Program = _Program;
      AddBaseTypesToTypeEntity(interfaceEntity, node);
      AddTypeParametersToEntity(interfaceEntity, parentEntity, node.TypeParameters, node.TypeParameterConstraints);
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
      var enumEntity = new EnumEntity(GetAccessibility(node.Modifiers), node.Name);
      (enumEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      enumEntity.Program = _Program;
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
      var delegateEntity = new DelegateEntity(GetAccessibility(node.Modifiers), node.Name);
      (delegateEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      delegateEntity.Program = _Program;
      AddTypeParametersToEntity(delegateEntity, parentEntity, node.TypeParameters, node.TypeParameterConstraints);
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
        var fieldEntity = new FieldEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                          typeReference, fieldTag.Identifier, initializer);
        fieldEntity.IsNew = IsNew(node.Modifiers);
        parentEntity.AddMember(fieldEntity);

        AssociateSyntaxNodeWithSemanticEntity(fieldTag, fieldEntity);
      }

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constant member entity from a const declaration.
    /// </summary>
    /// <param name="node">A const declaration syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ConstDeclarationNode node)
    {
      var parentEntity = GetParentEntity<TypeEntity>(node);

      // Looping through every tag in the field declaration
      foreach (var constTag in node.ConstTags)
      {
        // Create a semantic entity and add to its parent.
        var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
        var constantEntity = new ConstantMemberEntity(true, GetAccessibility(node.Modifiers), typeReference,
                                                      constTag.Identifier);
        constantEntity.IsNew = IsNew(node.Modifiers);
        parentEntity.AddMember(constantEntity);

        AssociateSyntaxNodeWithSemanticEntity(constTag, constantEntity);
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
      var enumMemberEntity = new EnumMemberEntity(true, node.Identifier, parentEntity.UnderlyingTypeReference);
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

      // The property is auto-implemented it's not an interface member
      // and if both get and set accessors are abstract 
      var isAutoImplemented = !parentEntity.IsInterfaceType
        && node.GetAccessor != null && !node.GetAccessor.HasBody
        && node.SetAccessor != null && !node.SetAccessor.HasBody;

      // Create a semantic entity and add to its parent.
      var interfaceReference = node.InterfaceType != null
                                 ? new NamespaceOrTypeNameNodeBasedTypeEntityReference(node.InterfaceType)
                                 : null;
      var typeReference = new TypeNodeBasedTypeEntityReference(node.Type);
      var propertyEntity = new PropertyEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                              typeReference, interfaceReference, node.Name, isAutoImplemented);
      propertyEntity.IsNew = IsNew(node.Modifiers);
      propertyEntity.IsOverride = IsOverride(node.Modifiers);
      propertyEntity.IsVirtual = IsVirtual(node.Modifiers);
      propertyEntity.IsSealed = IsSealed(node.Modifiers);

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

      var methodEntity = new MethodEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                          node.IsPartial, returnTypeReference, interfaceReference, node.Name, isAbstract);

      methodEntity.IsNew = IsNew(node.Modifiers);
      methodEntity.IsOverride = IsOverride(node.Modifiers);
      methodEntity.IsVirtual = IsVirtual(node.Modifiers);
      methodEntity.IsSealed = IsSealed(node.Modifiers);

      AddTypeParametersToEntity(methodEntity, parentEntity, node.TypeParameters, node.TypeParameterConstraints);
      AddParametersToOverloadableEntity(methodEntity, node.FormalParameters);

      parentEntity.AddMember(methodEntity);
      
      AssociateSyntaxNodeWithSemanticEntity(node, methodEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a literal entity from a literal AST node.
    /// </summary>
    /// <param name="node">A literal AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(LiteralNode node)
    {
      // Get the parent entity of the to-be created entity
      var parentEntity = GetParentEntity<SemanticEntity>(node);

      LiteralExpressionEntity literal = null;

      if (node is NullLiteralNode)
      {
        literal  = new NullLiteralExpressionEntity();
      }
      else if (node is BooleanLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(bool)));
      }
      else if (node is DecimalLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(decimal)));
      }
      else if (node is Int32LiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(int)));    
      }
      else if (node is UInt32LiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(uint)));
      }
      else if (node is Int64LiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(long)));
      }
      else if (node is UInt64LiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(ulong)));
      }
      else if (node is CharLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(char)));
      }
      else if (node is SingleLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(float)));
      }
      else if (node is DoubleLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(double)));
      }
      else if (node is StringLiteralNode)
      {
        literal = new TypedLiteralExpressionEntity(new ReflectedTypeBasedTypeEntityReference(typeof(string)));
      }

      var hasExpressions = CastToIHasExpressions(parentEntity);
      hasExpressions.AddExpression(literal);

      AssociateSyntaxNodeWithSemanticEntity(node, literal);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(SimpleNameNode node)
    {
      var parentEntity = GetParentEntity<SemanticEntity>(node);

      var simpleNameEntity = new SimpleNameExpressionEntity(new SimpleNameNodeBasedSemanticEntityReference(node));
      
      var hasExpressions = CastToIHasExpressions(parentEntity);
      hasExpressions.AddExpression(simpleNameEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, simpleNameEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(DefaultValueExpressionNode node)
    {
      var parentEntity = GetParentEntity<SemanticEntity>(node);

      var defaultValueEntity = new DefaultValueExpressionEntity(new TypeNodeBasedTypeEntityReference(node.Type));

      var hasExpressions = CastToIHasExpressions(parentEntity);
      hasExpressions.AddExpression(defaultValueEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, defaultValueEntity);

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(BlockStatementNode node)
    {
      //var parentEntity = GetParentEntity<SemanticEntity>(node);

      //var blockEntity = new BlockEntity();

      //var hasBody = CastToIHasBody(parentEntity);
      //hasBody.Body = blockEntity;

      //AssociateSyntaxNodeWithSemanticEntity(node, blockEntity);

      return false; // for testing only
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
    /// Checks whether a semantic entity implements the IHasBody interface.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>An IHasBody interface.</returns>
    /// <remarks>Throws an AppicationException if the cast was not successful.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static IHasBody CastToIHasBody(SemanticEntity entity)
    {
      var hasBody = entity as IHasBody;
      if (hasBody == null)
      {
        throw new ApplicationException(
          string.Format("Expected a type that has body, but found '{0}'.", entity.GetType()));
      }
      return hasBody;
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
    /// <param name="typeParameterConstraints">A collection of type parameter constraint AST nodes.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AddTypeParametersToEntity(
      ICanHaveTypeParameters typeParameterHolder, 
      SemanticEntity parentEntity, 
      TypeParameterNodeCollection typeParameterNodes,
      TypeParameterConstraintNodeCollection typeParameterConstraints)
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
        var typeParameterName = typeParameter.Identifier;
        var typeParameterEntity = new TypeParameterEntity(typeParameterName);
        typeParameterHolder.AddTypeParameter(typeParameterEntity);
        AssociateSyntaxNodeWithSemanticEntity(typeParameter, typeParameterEntity);

        // Find the constraints of the type parameter
        var constraints = from typeParameterConstraint in typeParameterConstraints
                          where typeParameterConstraint.Identifier == typeParameterName
                          select typeParameterConstraint;

        // Add the constraints to the type parameter entity
        foreach (var constraint in constraints)
        {
          foreach(var constraintTag in constraint.ConstraintTags)
          {
            typeParameterEntity.HasDefaultConstructorConstraint = constraintTag.IsNew || constraintTag.IsStruct;
            typeParameterEntity.HasReferenceTypeConstraint = constraintTag.IsClass;
            typeParameterEntity.HasNonNullableValueTypeConstraint = constraintTag.IsStruct;

            if (constraintTag.IsTypeName)
            {
              typeParameterEntity.AddTypeReferenceConstraint(new TypeNodeBasedTypeEntityReference(constraintTag.Type));
            }
          }
        }
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
    private AccessorEntity CreateAccessor(AccessorNode node)
    {
      if (node == null)
      {
        return null;
      }
      var accessibility = GetAccessibility(node.Modifiers);

      var accessor = new AccessorEntity(accessibility, !node.HasBody);
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
    private static VariableInitializer CreateInitializer(VariableInitializerNode node)
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

      return result as VariableInitializer;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "abstract" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "abstract" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsAbstract(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Abstract));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "sealed" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "sealed" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsSealed(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Sealed));
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "static" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "static" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Static));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "new" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "new" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsNew(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.New));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "override" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "override" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsOverride(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Override));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "virtual" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "virtual" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsVirtual(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Virtual));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessibility defined in a collection of modifier AST nodes.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>The accessibility defined in the collection of modifier AST nodes. 
    /// Null if not defined or ambiguous.</returns>
    /// <remarks>Signals error CS0107 for ambiguous accessibility specifiers.</remarks>
    // ----------------------------------------------------------------------------------------------
    public AccessibilityKind? GetAccessibility(ModifierNodeCollection modifiers)
    {
      AccessibilityKind? accessibility = null;

      if (modifiers != null)
      {
        foreach (var modifier in modifiers)
        {
          switch (modifier.Value)
          {
            case (ModifierType.Private):
              if (accessibility == null)
              {
                accessibility = AccessibilityKind.Private;
              }
              else
              {
                ErrorTooManyProtectionModifier(modifier.StartToken);
                return null;
              }
              break;

            case (ModifierType.Protected):
              if (accessibility == null)
              {
                accessibility = AccessibilityKind.Family;
              }
              else if (accessibility == AccessibilityKind.Assembly)
              {
                accessibility = AccessibilityKind.FamilyOrAssembly;
              }
              else
              {
                ErrorTooManyProtectionModifier(modifier.StartToken);
                return null;
              }
              break;

            case (ModifierType.Internal):
              if (accessibility == null)
              {
                accessibility = AccessibilityKind.Assembly;
              }
              else if (accessibility == AccessibilityKind.Family)
              {
                accessibility = AccessibilityKind.FamilyOrAssembly;
              }
              else
              {
                ErrorTooManyProtectionModifier(modifier.StartToken);
                return null;
              }
              break;

            case (ModifierType.Public):
              if (accessibility == null)
              {
                accessibility = AccessibilityKind.Public;
              }
              else
              {
                ErrorTooManyProtectionModifier(modifier.StartToken);
                return null;
              }
              break;

            default:
              break;
          }
        }
      }

      return accessibility;
    }
    
    #endregion

    #region Error reporting private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signals error CS0107: More than one protection modifier
    /// </summary>
    /// <param name="errorPoint">The token where the error occured.</param>
    // ----------------------------------------------------------------------------------------------
    private void ErrorTooManyProtectionModifier(Token errorPoint)
    {
      _ErrorHandler.Error("CS0107", errorPoint, "More than one protection modifier");
    }

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
