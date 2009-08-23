using System;
using CSharpTreeBuilder.Ast;
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
      SemanticEntity parentEntity = GetParentEntity(node);

      var parentNamespaceEntity = parentEntity as NamespaceEntity;
      if (parentNamespaceEntity == null)
      {
        throw new ApplicationException(String.Format("Parent expected to be NamespaceEntity but was {0}", parentEntity.GetType()));
      }

      // Looping through every tag in the namespace name
      foreach (var nameTag in node.NameTags)
      {
        NamespaceEntity namespaceEntity;

        var nameTableEntry = parentNamespaceEntity.DeclarationSpace[nameTag.Identifier];
        
        // If the name is found then we have to examine some sub-cases
        if (nameTableEntry != null)
        {
          // Let's check whether the name's meaning is definit
          switch (nameTableEntry.State)
          {
            case NameTableEntryState.Definite:
              // If the name is definite, but it's not a namespace, then signal error and get out.
              if (!(nameTableEntry.Entity is NamespaceEntity))
              {
                _ErrorHandler.Error("CS0101", node.StartToken,
                                    "The namespace '{0}' already contains a definition for '{1}'.",
                                    parentNamespaceEntity.FullyQualifiedName, nameTag.Identifier);
                return;
              }

              // If the name is definite, and is a namespace, then this is the one we were looking for.
              namespaceEntity = nameTableEntry.Entity as NamespaceEntity;
              break;

            // If the name is found, but is not definite, then no semantic entity can be created, so let's get out.
            default:
              return;
          }
        }
        else // If the name was not found then we have to create it.
        {
          namespaceEntity = new NamespaceEntity() {Name = nameTag.Identifier};
          parentNamespaceEntity.AddChildNamespace(namespaceEntity);
        }

        AssociateSyntaxNodeWithSemanticEntity(node, namespaceEntity);

        // The next parent is the current entity
        parentNamespaceEntity = namespaceEntity;
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
      SemanticEntity parentEntity = GetParentEntity(node);

      // The parent entity should be a NamespaceEntity.
      if (!(parentEntity is NamespaceEntity))
      {
        throw new ApplicationException(string.Format("Expected NamespaceEntity but received '{0}'.",
                                                     parentEntity.GetType()));
      }

      var parentNamespaceEntity = parentEntity as NamespaceEntity;

      // The parent of AST node should not be null.
      if (node.ParentNode == null)
      {
        throw new ApplicationException("Unexpected null ParentNode.");
      }

      // Find out the compilation unit that the AST node belongs to
      var compilationUnitNode = node.CompilationUnitNode;

      // Spec: "The scope of an extern-alias-directive extends over the using-directives,
      // global-attributes and namespace-member-declarations 
      // of its immediately containing compilation unit or namespace body."
      // But we just simplify it to be the whole parent AST node, 
      // because we'll only use it to resolve names in the namespace-member-declarations anyway.
      var lexicalScope = new SourceRegion(
        new SourcePoint(compilationUnitNode, node.ParentNode.StartPosition),
        new SourcePoint(compilationUnitNode, node.ParentNode.EndPosition));

      // Check error CS1537: The using alias 'alias' appeared previously in this namespace
      // _Note: It should say 'extern alias' instead of 'using alias' but that's how csc.exe works.
      if (parentNamespaceEntity.IsExternAliasAlreadySpecified(node.Identifier, lexicalScope))
      {
        _ErrorHandler.Error("CS1537", node.StartToken, "The using alias '{0}' appeared previously in this namespace",
                            node.Identifier);
        return;
      }

      // Create the extern alias entity and add it to the parent namespace (which can be a root namespace as well).
      var externAliasEntity = new ExternAliasEntity(lexicalScope, node);
      parentNamespaceEntity.AddExternAlias(externAliasEntity);

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
      CreateUsingEntityFromUsingNode<UsingNamespaceNode, UsingNamespaceEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using alias entity from a using alias AST node.
    /// </summary>
    /// <param name="node">A using alias AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(UsingAliasNode node)
    {
      CreateUsingEntityFromUsingNode<UsingAliasNode, UsingAliasEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a using namespace entity or a using alias entity from an AST node.
    /// </summary>
    /// <typeparam name="TUsingNodeType">The type of the AST node. Must be a subclass of UsingNamespaceNode.</typeparam>
    /// <typeparam name="TUsingEntityType">The type of the entity to be created. Must be a subclass of UsingEntity.</typeparam>
    /// <param name="node">An AST node.</param>
    // ----------------------------------------------------------------------------------------------
    private void CreateUsingEntityFromUsingNode<TUsingNodeType, TUsingEntityType>(TUsingNodeType node)
      where TUsingNodeType: UsingNamespaceNode
      where TUsingEntityType: UsingEntity
    {
      // Determine the parent entity of the to-be-created using alias entity.
      SemanticEntity parentEntity = GetParentEntity(node);

      // The parent entity should be a NamespaceEntity.
      if (!(parentEntity is NamespaceEntity))
      {
        throw new ApplicationException(string.Format("Expected NamespaceEntity but received '{0}'.",
                                                     parentEntity.GetType()));
      }

      var parentNamespaceEntity = parentEntity as NamespaceEntity;

      // The parent of AST node should not be null.
      if (node.ParentNode == null)
      {
        throw new ApplicationException("Unexpected null ParentNode.");
      }

      // Find out the compilation unit that the AST node belongs to
      var compilationUnitNode = node.CompilationUnitNode;

      // Spec: "The scope of a using-directive extends over the namespace-member-declarations 
      // of its immediately containing compilation unit or namespace body."
      // But we just simplify it to be the whole parent AST node, 
      // because we'll only use it to resolve names in the namespace-member-declarations anyway.
      var lexicalScope = new SourceRegion(
        new SourcePoint(compilationUnitNode, node.ParentNode.StartPosition),
        new SourcePoint(compilationUnitNode, node.ParentNode.EndPosition));

      UsingEntity usingEntity = null;

      if (node is UsingAliasNode)
      {
        var aliasName = (node as UsingAliasNode).Alias;

        // -- using alias branch

        // Check error CS1537: The using alias 'alias' appeared previously in this namespace
        if (parentNamespaceEntity.IsUsingAliasAlreadySpecified(aliasName, lexicalScope)
          || parentNamespaceEntity.IsExternAliasAlreadySpecified(aliasName, lexicalScope))
        {
          _ErrorHandler.Error("CS1537", node.StartToken, "The using alias '{0}' appeared previously in this namespace",
                              aliasName);
          return;
        }

        // Create the using alias entity and add it to the parent namespace (which can be a root namespace as well).
        var usingAliasEntity = new UsingAliasEntity(lexicalScope, aliasName, node.NamespaceOrTypeName);
        parentNamespaceEntity.AddUsingAlias(usingAliasEntity);
        usingEntity = usingAliasEntity;
      }
      else 
      {
        // -- using namespace branch

        var namespaceName = node.NamespaceOrTypeName.TypeTags.ToString();

        // Check warning CS0105: The using directive for 'namespace' appeared previously in this namespace
        if (parentNamespaceEntity.IsUsingNamespaceAlreadySpecified(namespaceName,lexicalScope))
        {
          _ErrorHandler.Warning("CS0105", node.StartToken,
                                "The using directive for '{0}' appeared previously in this namespace", namespaceName);
          return;
        }

        // Create the using namespace entity and add it to the parent namespace (which can be a root namespace as well).
        var usingNamespaceEntity = new UsingNamespaceEntity(lexicalScope, node.NamespaceOrTypeName);
        parentNamespaceEntity.AddUsingNamespace(usingNamespaceEntity);
        usingEntity = usingNamespaceEntity;
      }

      // Establish to two-way mapping between the AST node and the new semantic entity
      AssociateSyntaxNodeWithSemanticEntity(node, usingEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a class entity from a class declaration.
    /// </summary>
    /// <param name="node">A class declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(ClassDeclarationNode node)
    {
      CreateTypeEntityFromTypeDeclaration<ClassDeclarationNode,ClassEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a struct entity from a struct declaration.
    /// </summary>
    /// <param name="node">A struct declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(StructDeclarationNode node)
    {
      CreateTypeEntityFromTypeDeclaration<StructDeclarationNode, StructEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an interface entity from an interface declaration.
    /// </summary>
    /// <param name="node">An interface declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(InterfaceDeclarationNode node)
    {
      CreateTypeEntityFromTypeDeclaration<InterfaceDeclarationNode, InterfaceEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an enum entity from an enum declaration.
    /// </summary>
    /// <param name="node">An enum declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(EnumDeclarationNode node)
    {
      var enumEntity = CreateTypeEntityFromTypeDeclaration<EnumDeclarationNode, EnumEntity>(node);

      // Set the underlying type reference.
      if (node.EnumBase != null)
      {
        enumEntity.UnderlyingType = new TypeNodeBasedTypeEntityReference(node.EnumBase);
      }
      else
      {
        enumEntity.UnderlyingType = new ReflectedTypeBasedTypeEntityReference(typeof (int));
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
      CreateTypeEntityFromTypeDeclaration<DelegateDeclarationNode, DelegateEntity>(node);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type entity from a type declaration syntax node.
    /// </summary>
    /// <typeparam name="TSyntaxNodeType">The type of the syntax node.</typeparam>
    /// <typeparam name="TSemanticEntityType">The type of the semantic entity to be created.</typeparam>
    /// <param name="node">A syntax node.</param>
    /// <returns>The created type entity, or null if an error occured.</returns>
    // ----------------------------------------------------------------------------------------------
    private TSemanticEntityType CreateTypeEntityFromTypeDeclaration<TSyntaxNodeType, TSemanticEntityType>(TSyntaxNodeType node) 
      where TSyntaxNodeType: TypeDeclarationNode
      where TSemanticEntityType: TypeEntity, new()
    {
      SemanticEntity parentEntity = GetParentEntity(node);

      var parentNamespaceOrTypeEntity = parentEntity as NamespaceOrTypeEntity;
      if (parentNamespaceOrTypeEntity == null)
      {
        throw new ApplicationException(String.Format("Parent expected to be NamespaceOrTypeEntity but was {0}", parentEntity.GetType()));
      }

      TSemanticEntityType typeEntity;

      var nameTableEntry = parentNamespaceOrTypeEntity.DeclarationSpace[node.Name
                              + (node.HasTypeParameters ? "`" + node.TypeParameters.Count : "")];

      // If the name is found then we have to examine some sub-cases
      if (nameTableEntry != null)
      {
        // Let's check whether the name's meaning is definit
        switch (nameTableEntry.State)
        {
          case NameTableEntryState.Definite:
            // If the name is definite, but it's not a type, then signal error and get out.
            if (!(nameTableEntry.Entity is TSemanticEntityType))
            {
              _ErrorHandler.Error("CS0101", node.StartToken,
                                  "The namespace '{0}' already contains a definition for '{1}'.",
                                  parentNamespaceOrTypeEntity.FullyQualifiedName, node.Name);
              return null;
            }

            // If the name is definite, and is the desired type, then this is the one we were looking for.
            typeEntity = nameTableEntry.Entity as TSemanticEntityType;
            // TODO: if partial allowed then deal with it, otherwise signal error
            break;

          // If the name is found, but is not definite, then no semantic entity can be created, so let's get out.
          default:
            _ErrorHandler.Warning("TBD", node.StartToken,
                                "The name '{1}' is ambigous in the context of '{0}'. No entity created.",
                                parentNamespaceOrTypeEntity.FullyQualifiedName, node.Name);
            return null;
        }
      }
      else // If the name was not found then we have to create it.
      {
        // Build the new entity
        typeEntity = new TSemanticEntityType() { Name = node.Name };
        foreach (var baseType in node.BaseTypes)
        {
          typeEntity.AddBaseTypeReference(new TypeNodeBasedTypeEntityReference(baseType));
        }
        if (typeEntity is GenericCapableTypeEntity)
        {
          var genericTypeEntity = typeEntity as GenericCapableTypeEntity;

          // First add the (inherited) type parameters from the parent type
          if (parentEntity is GenericCapableTypeEntity)
          {
            foreach (var typeParameterEntity in ((GenericCapableTypeEntity)parentEntity).AllTypeParameters)
            {
              genericTypeEntity.AddTypeParameter(typeParameterEntity);
            }
          }

          // Then add the "own" type parameters
          foreach (var typeParameter in node.TypeParameters)
          {
            var typeParameterEntity = new TypeParameterEntity(typeParameter.Identifier);
            genericTypeEntity.AddTypeParameter(typeParameterEntity);
            AssociateSyntaxNodeWithSemanticEntity(typeParameter, typeParameterEntity);
          }
        }

        // Add the entity to its parent (but only if the parent can have child types)
        if (parentNamespaceOrTypeEntity is IHasChildTypes)
        {
          ((IHasChildTypes)parentNamespaceOrTypeEntity).AddChildType(typeEntity);
        }
        else
        {
          throw new ApplicationException(String.Format("Type '{0}' cannot declare child type '{1}'.",
                                                       parentNamespaceOrTypeEntity.Name, typeEntity.Name));
        }
      }

      AssociateSyntaxNodeWithSemanticEntity(node, typeEntity);

      return typeEntity as TSemanticEntityType;
    }

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
    /// Creates a field entity from a field declaration.
    /// </summary>
    /// <param name="node">A field declaration syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(FieldDeclarationNode node)
    {
      SemanticEntity parentEntity = GetParentEntity(node);

      var parentTypeEntity = parentEntity as TypeEntity;
      if (parentTypeEntity == null)
      {
        throw new ApplicationException(String.Format("Parent expected to be TypeEntity but was {0}", parentEntity.GetType()));
      }

      // Looping through every tag in the field declaration
      foreach (var fieldTag in node.FieldTags)
      {
        // Check if this name is already in use in this declaration space
        if (parentTypeEntity.DeclarationSpace.IsNameDefined(fieldTag.Identifier))
        {
          _ErrorHandler.Error("CS0102", node.StartToken, "The type '{0}' already contains a definition for '{1}'.",
                              parentTypeEntity.FullyQualifiedName, fieldTag.Identifier);
          
          // Continue with the next field tag.
          continue;
        }

        // Create a semantic entity and add to its parent.
        var fieldEntity = new FieldEntity(fieldTag.Identifier, true,
                                          new TypeNodeBasedTypeEntityReference(node.Type),
                                          node.IsStatic);
        parentTypeEntity.AddMember(fieldEntity);

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
      SemanticEntity parentEntity = GetParentEntity(node);

      var parentEnumEntity = parentEntity as EnumEntity;
      if (parentEnumEntity == null)
      {
        throw new ApplicationException(String.Format("Parent expected to be EnumEntity but was {0}", parentEntity.GetType()));
      }

      // Check if this name is already in use in this declaration space
      if (parentEnumEntity.DeclarationSpace.IsNameDefined(node.Identifier))
      {
        _ErrorHandler.Error("CS0102", node.StartToken, "The type '{0}' already contains a definition for '{1}'.",
                            parentEnumEntity.FullyQualifiedName, node.Identifier);
        return;
      }

      // Create a semantic entity and add to its parent.
      var enumMemberEntity = new EnumMemberEntity(node.Identifier, parentEnumEntity.UnderlyingType);
      parentEnumEntity.AddMember(enumMemberEntity);

      AssociateSyntaxNodeWithSemanticEntity(node, enumMemberEntity);
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
  }
}
