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
      CreateTypeEntityFromTypeDeclaration<EnumDeclarationNode, EnumEntity>(node);
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
          typeEntity.AddBaseTypeReference(new TypeOrNamespaceNodeBasedTypeEntityReference(baseType));
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

        // Create a semantic entity, add to its parent, and add to the graph.
        var fieldEntity = new FieldEntity(fieldTag.Identifier, true,
                                          new TypeOrNamespaceNodeBasedTypeEntityReference(node.TypeName),
                                          node.IsStatic);
        parentTypeEntity.AddMember(fieldEntity);

        AssociateSyntaxNodeWithSemanticEntity(fieldTag, fieldEntity);
      }
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
