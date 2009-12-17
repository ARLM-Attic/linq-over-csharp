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
      var parentEntity = GetSemanticEntity(node.ParentNode);

      // Looping through every tag in the namespace name
      foreach (var nameTag in node.NameTags)
      {
        // Find out whether this child namespace already exists
        var namespaceEntity = (parentEntity as NamespaceEntity).GetChildNamespace(nameTag.Identifier);
        
        // If no such namespace yet then create it
        if (namespaceEntity == null)
        {
          namespaceEntity = new NamespaceEntity(nameTag.Identifier);
          namespaceEntity.Program = _Program;
          parentEntity.AddChild(namespaceEntity);
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
      var parentEntity = GetSemanticEntity(node.ParentNode);
      var lexicalScope = GetParentSourceRegion(node);

      // Check error CS1537: The using alias 'alias' appeared previously in this namespace
      // _Note: It should say 'extern alias' instead of 'using alias' but that's how csc.exe works.
      if ((parentEntity as NamespaceEntity).IsExternAliasAlreadySpecified(node.Identifier, lexicalScope))
      {
        ErrorDuplicateUsingAlias(node.StartToken, node.Identifier);
        return false;
      }

      var newEntity = new ExternAliasEntity(lexicalScope, node);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var parentEntity = GetSemanticEntity(node.ParentNode);
      var lexicalScope = GetParentSourceRegion(node);

      // Check warning CS0105: The using directive for 'namespace' appeared previously in this namespace
      var namespaceName = node.NamespaceOrTypeName.TypeTags.ToString();
      if ((parentEntity as NamespaceEntity).IsUsingNamespaceAlreadySpecified(namespaceName, lexicalScope))
      {
        ErrorDuplicateUsingNamespace(node.StartToken, namespaceName);
        return false;
      }

      var newEntity = new UsingNamespaceEntity(lexicalScope, node.NamespaceOrTypeName);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var parentEntity = GetSemanticEntity(node.ParentNode);
      var lexicalScope = GetParentSourceRegion(node);

      // Check error CS1537: The using alias 'alias' appeared previously in this namespace
      if ((parentEntity as NamespaceEntity).IsUsingAliasAlreadySpecified(node.Alias, lexicalScope)
        || (parentEntity as NamespaceEntity).IsExternAliasAlreadySpecified(node.Alias, lexicalScope))
      {
        ErrorDuplicateUsingAlias(node.StartToken, node.Alias);
        return false;
      }

      var newEntity = new UsingAliasEntity(lexicalScope, node.Alias, node.NamespaceOrTypeName);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new ClassEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (newEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      newEntity.IsStatic = IsStatic(node.Modifiers);
      newEntity.IsAbstract = IsAbstract(node.Modifiers);
      newEntity.IsSealed = IsSealed(node.Modifiers);
      AddBaseTypesToTypeEntity(newEntity, node);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new StructEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (newEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      AddBaseTypesToTypeEntity(newEntity, node);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new InterfaceEntity(GetAccessibility(node.Modifiers), node.Name, node.IsPartial);
      (newEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      AddBaseTypesToTypeEntity(newEntity, node);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new EnumEntity(GetAccessibility(node.Modifiers), node.Name);
      (newEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      newEntity.UnderlyingTypeReference = node.EnumBase == null
                                            ? new ReflectedTypeToTypeEntityResolver(typeof (int)) as Resolver<TypeEntity>
                                            : new TypeNodeToTypeEntityResolver(node.EnumBase);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new DelegateEntity(GetAccessibility(node.Modifiers), node.Name);
      (newEntity as IMemberEntity).IsNew = IsNew(node.Modifiers);
      newEntity.ReturnTypeReference = new TypeNodeToTypeEntityResolver(node.Type);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      foreach (var fieldTag in node.FieldTags)
      {
        var typeReference = new TypeNodeToTypeEntityResolver(node.Type);
        var newEntity = new FieldEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                          IsReadOnly(node.Modifiers), typeReference, fieldTag.Identifier);
        newEntity.IsNew = IsNew(node.Modifiers);

        InsertEntityIntoSemanticGraph(newEntity, fieldTag, node.ParentNode);
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
      // Constant member are created in Visit(ConstTagNode node).
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
      var parentEntity = GetSemanticEntity(node.ParentNode);
      var newEntity = new EnumMemberEntity(true, node.Identifier, (parentEntity as EnumEntity).UnderlyingTypeReference);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      // The property is auto-implemented it's not an interface member
      // and if both get and set accessors are abstract 
      var isAutoImplemented = !(node.ParentNode is InterfaceDeclarationNode)
        && node.GetAccessor != null && !node.GetAccessor.HasBody
        && node.SetAccessor != null && !node.SetAccessor.HasBody;

      var interfaceReference = node.InterfaceType != null
                                 ? new NamespaceOrTypeNameNodeToTypeEntityResolver(node.InterfaceType)
                                 : null;
      var typeReference = new TypeNodeToTypeEntityResolver(node.Type);
      var newEntity = new PropertyEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                              typeReference, interfaceReference, node.Name, isAutoImplemented);
      newEntity.IsNew = IsNew(node.Modifiers);
      newEntity.IsOverride = IsOverride(node.Modifiers);
      newEntity.IsVirtual = IsVirtual(node.Modifiers);
      newEntity.IsSealed = IsSealed(node.Modifiers);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from a syntax node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(AccessorNode node)
    {
      var accessorKind = CSharpSemanticGraph.AccessorKind.Get;
      switch (node.AccessorKind)
      {
        case (Ast.AccessorKind.Get):
          accessorKind = CSharpSemanticGraph.AccessorKind.Get;
          break;
        case (Ast.AccessorKind.Set):
          accessorKind = CSharpSemanticGraph.AccessorKind.Set;
          break;
        case (Ast.AccessorKind.Add):
          accessorKind = CSharpSemanticGraph.AccessorKind.Add;
          break;
        case (Ast.AccessorKind.Remove):
          accessorKind = CSharpSemanticGraph.AccessorKind.Remove;
          break;
      }
      var accessibility = GetAccessibility(node.Modifiers);
      var newEntity = new AccessorEntity(accessorKind, accessibility, !node.HasBody);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var interfaceReference = node.InterfaceType != null
                                 ? new NamespaceOrTypeNameNodeToTypeEntityResolver(node.InterfaceType)
                                 : null;
      var isAbstract = (node.Body == null);
      var returnTypeReference = new TypeNodeToTypeEntityResolver(node.Type);

      var newEntity = new MethodEntity(true, GetAccessibility(node.Modifiers), IsStatic(node.Modifiers),
                                          node.IsPartial, returnTypeReference, interfaceReference, node.Name, isAbstract);

      newEntity.IsNew = IsNew(node.Modifiers);
      newEntity.IsOverride = IsOverride(node.Modifiers);
      newEntity.IsVirtual = IsVirtual(node.Modifiers);
      newEntity.IsSealed = IsSealed(node.Modifiers);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(FormalParameterNode node)
    {
      var parameterKind = ParameterKind.Value;
      switch (node.Modifier)
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
      var typeReference = new TypeNodeToTypeEntityResolver(node.Type);
      var newEntity = new ParameterEntity(node.Identifier, typeReference, parameterKind);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(TypeParameterNode node)
    {
      var typeParameterName = node.Identifier;
      var newEntity = new TypeParameterEntity(typeParameterName);

      // Find the constraints of the type parameter
      var constraints = from typeParameterConstraint in (node.ParentNode as TypeOrMemberDeclarationNode).TypeParameterConstraints
                        where typeParameterConstraint.Identifier == typeParameterName
                        select typeParameterConstraint;

      // Add the constraints to the type parameter entity
      foreach (var constraint in constraints)
      {
        foreach (var constraintTag in constraint.ConstraintTags)
        {
          newEntity.HasDefaultConstructorConstraint = constraintTag.IsNew || constraintTag.IsStruct;
          newEntity.HasReferenceTypeConstraint = constraintTag.IsClass;
          newEntity.HasNonNullableValueTypeConstraint = constraintTag.IsStruct;

          if (constraintTag.IsTypeName)
          {
            newEntity.AddTypeReferenceConstraint(new TypeNodeToTypeEntityResolver(constraintTag.Type));
          }
        }
      }

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a local variable entity.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(LocalVariableTagNode node)
    {
      var localVariableNode = node.ParentNode as LocalVariableNode;
      var typeResolver = new TypeNodeToTypeEntityResolver(localVariableNode.Type);
      var newEntity = new LocalVariableEntity(node.Identifier, typeResolver);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode.Parent);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a local constant entity.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ConstTagNode node)
    {
      // A ConstTagNode can be the child of a ConstDeclarationNode or a ConstStatementNode too,
      // so we have to handle both cases.

      if (node.ParentNode is ConstDeclarationNode)
      {
        var constantDeclarationNode = node.ParentNode as ConstDeclarationNode;
        var typeReference = new TypeNodeToTypeEntityResolver(constantDeclarationNode.Type);
        var newEntity = new ConstantMemberEntity(true, GetAccessibility(node.Modifiers), typeReference, node.Identifier) 
          { IsNew = IsNew(constantDeclarationNode.Modifiers) };

        InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode.Parent);
      }
      else if (node.ParentNode is ConstStatementNode)
      {
        var localConstantNode = node.ParentNode as ConstStatementNode;
        var typeResolver = new TypeNodeToTypeEntityResolver(localConstantNode.Type);
        var newEntity = new LocalConstantEntity(node.Identifier, typeResolver);
        var initializer = new ScalarInitializerEntity();
        newEntity.AddChild(initializer);

        InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      }
      else
      {
        throw new ApplicationException("Unexpected parent node type");
      }

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
      LiteralExpressionEntity newEntity = null;

      if (node is NullLiteralNode)
      {
        newEntity = new NullLiteralExpressionEntity();
      }
      else if (node is BooleanLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(bool)),
          (node as BooleanLiteralNode).Value);
      }
      else if (node is DecimalLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(decimal)),
          (node as DecimalLiteralNode).Value);
      }
      else if (node is Int32LiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(int)),
          (node as Int32LiteralNode).Value);
      }
      else if (node is UInt32LiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(uint)),
          (node as UInt32LiteralNode).Value);
      }
      else if (node is Int64LiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(long)),
          (node as Int64LiteralNode).Value);
      }
      else if (node is UInt64LiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(ulong)),
          (node as UInt64LiteralNode).Value);
      }
      else if (node is CharLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(char)),
          (node as CharLiteralNode).Value);
      }
      else if (node is SingleLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(float)),
          (node as SingleLiteralNode).Value);
      }
      else if (node is DoubleLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(double)),
          (node as DoubleLiteralNode).Value);
      }
      else if (node is StringLiteralNode)
      {
        newEntity = new TypedLiteralExpressionEntity(new ReflectedTypeToTypeEntityResolver(typeof(string)),
          (node as StringLiteralNode).Value);
      }

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new SimpleNameExpressionEntity(new SimpleNameNodeResolver(node));

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(PrimaryExpressionMemberAccessNode node)
    {
      var newEntity = new PrimaryMemberAccessExpressionEntity(new MemberAccessNodeResolver(node));

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(QualifiedAliasMemberAccessNode node)
    {
      var newEntity = new QualifiedAliasMemberAccessExpressionEntity(
        new QualifiedAliasMemberNodeResolver(node.QualifiedAliasMember),
        new MemberAccessNodeResolver(node));

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(PredefinedTypeMemberAccessNode node)
    {
      var newEntity = new PredefinedTypeMemberAccessExpressionEntity(
        node.TypeName.TypeTags[0].Identifier,
        new MemberAccessNodeResolver(node));

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new DefaultValueExpressionEntity(new TypeNodeToTypeEntityResolver(node.Type));

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(InvocationExpressionNode node)
    {
      var newEntity = new InvocationExpressionEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(AssignmentExpressionNode node)
    {
      var newEntity = new AssignmentExpressionEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ArgumentNode node)
    {
      var parameterKind = node.IsRef
                            ? ParameterKind.Reference
                            : node.IsOut
                                ? ParameterKind.Output
                                : ParameterKind.Value;

      var newEntity = new ArgumentEntity(parameterKind);

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
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
      var newEntity = new BlockStatementEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ExpressionStatementNode node)
    {
      var newEntity = new ExpressionStatementEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ReturnStatementNode node)
    {
      var newEntity = new ReturnStatementEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(VariableDeclarationStatementNode node)
    {
      var newEntity = new LocalVariableDeclarationStatementEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ConstStatementNode node)
    {
      var newEntity = new LocalConstantDeclarationStatementEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ExpressionInitializerNode node)
    {
      var newEntity = new ScalarInitializerEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an entity from an AST node.
    /// </summary>
    /// <param name="node">An AST node.</param>
    /// <returns>True if the visitor should continue traversing, false if it should stop.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ArrayInitializerNode node)
    {
      var newEntity = new ArrayInitializerEntity();

      InsertEntityIntoSemanticGraph(newEntity, node, node.ParentNode);
      return true;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Inserts a semantic entity into the semantic graph.
    /// </summary>
    /// <param name="newEntity">The entity to be inserted.</param>
    /// <param name="node">The syntax node that generated to entity.</param>
    /// <param name="parentNode">The syntax node whose corresponding semantic entity 
    /// will be the parent of the new entity.</param>
    // ----------------------------------------------------------------------------------------------
    private void InsertEntityIntoSemanticGraph(ISemanticEntity newEntity, ISyntaxNode node, ISyntaxNode parentNode)
    {
      newEntity.Program = _Program;

      GetSemanticEntity(parentNode).AddChild(newEntity);
      
      AssociateSyntaxNodeWithSemanticEntity(node, newEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a single semantic entity associated with a syntax node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>A single semantic entity.</returns>
    /// <remarks>Throws an exception if can't find any entity or what was found is ambiguous.</remarks>
    // ----------------------------------------------------------------------------------------------
    private ISemanticEntity GetSemanticEntity(ISyntaxNode node)
    {
      if (node == null)
      {
        throw new ArgumentNullException("node");
      }

      // If we are at the compilation unit level, then the corresponding semantic entity is the "global" namespace.
      if (node is CompilationUnitNode)
      {
        return _SemanticGraph.GlobalNamespace;
      }

      var semanticEntityCount = node.SemanticEntities.Count;

      // If the node is a NamespaceDeclarationNode, then it can have several entities associated with it
      // (eg. namespace A.B.C where A, B, C are all distinct semantic entities), 
      // and the corresponding entity is the last in the list.
      if (node is NamespaceDeclarationNode && semanticEntityCount >= 1)
      {
        return node.SemanticEntities[semanticEntityCount - 1];
      }

      // If the node has only 1 semantic entity associated, then this has to be returned.
      if (semanticEntityCount == 1)
      {
        return node.SemanticEntities[0];
      }

      // If the parent node has more than 1 semantic entites, that's an unexpected ambiguity. 
      if (semanticEntityCount > 1)
      {
        throw new ApplicationException(
          string.Format("Unexpected number of semantic entities ('{0}') on node of type: '{1}'",
                        semanticEntityCount, node.GetType()));
      }

      // Otherwise there's no semantic entity, which is an error.
      throw new ApplicationException("No associated semantic entity found.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Establishis a bi-directional link between an AST (abstract syntax tree) node and an SG (semantic graph) node.
    /// </summary>
    /// <param name="syntaxNode">A syntax tree node.</param>
    /// <param name="semanticEntity">A semantic entity node.</param>
    // ----------------------------------------------------------------------------------------------
    private static void AssociateSyntaxNodeWithSemanticEntity(ISyntaxNode syntaxNode, ISemanticEntity semanticEntity)
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
        typeEntity.AddBaseTypeReference(new TypeNodeToTypeEntityResolver(baseType));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "abstract" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "abstract" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool IsAbstract(ModifierNodeCollection modifiers)
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
    private static bool IsSealed(ModifierNodeCollection modifiers)
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
    private static bool IsStatic(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Static));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "readonly" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "readonly" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool IsReadOnly(ModifierNodeCollection modifiers)
    {
      return (modifiers != null && modifiers.Contains(ModifierType.Readonly));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the "new" modifier appears in the modifier list.
    /// </summary>
    /// <param name="modifiers">A collection of modifier AST nodes.</param>
    /// <returns>True if the "new" modifier appears in the modifier list, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool IsNew(ModifierNodeCollection modifiers)
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
    private static bool IsOverride(ModifierNodeCollection modifiers)
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
    private static bool IsVirtual(ModifierNodeCollection modifiers)
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
    private AccessibilityKind? GetAccessibility(ModifierNodeCollection modifiers)
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
