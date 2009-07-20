using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBuilderSyntaxNodeVisitor"/> class.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph that will receive the built entities.</param>
    // ----------------------------------------------------------------------------------------------
    public EntityBuilderSyntaxNodeVisitor(SemanticGraph semanticGraph)
    {
      _SemanticGraph = semanticGraph;
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
        // Determine FQN that is the key for finding the entity in the semantic graph.
        string fullyQualifiedName = parentNamespaceEntity is RootNamespaceEntity
                                     ? nameTag.Identifier
                                     : parentNamespaceEntity.FullyQualifiedName + "." + nameTag.Identifier;

        // Find out whether the namespace entity already exists in the graph.
        var namespaceEntity = _SemanticGraph.GetEntityByFullyQualifiedName(fullyQualifiedName) as NamespaceEntity;

        // If no namespace entity found then create it and add it to the graph
        if (namespaceEntity == null)
        {
          namespaceEntity = new NamespaceEntity() {Name = nameTag.Identifier};
          parentNamespaceEntity.AddChildNamespace(namespaceEntity);
          _SemanticGraph.AddEntity(namespaceEntity);
        }

        // Set a bidirectional link between AST and SG node
        namespaceEntity.SyntaxNodes.Add(node);
        node.SemanticEntities.Add(namespaceEntity);

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
    /// Creates a type entity from a type declaration syntax node.
    /// </summary>
    /// <typeparam name="TSyntaxNodeType">The type of the syntax node.</typeparam>
    /// <typeparam name="TSemanticEntityType">The type of the semantic entity.</typeparam>
    /// <param name="node">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    private void CreateTypeEntityFromTypeDeclaration<TSyntaxNodeType, TSemanticEntityType>(TSyntaxNodeType node) 
      where TSyntaxNodeType: TypeDeclarationNode
      where TSemanticEntityType: TypeEntity, new()
    {
      SemanticEntity parentEntity = GetParentEntity(node);

      var parentNamespaceOrTypeEntity = parentEntity as NamespaceOrTypeEntity;
      if (parentNamespaceOrTypeEntity == null)
      {
        throw new ApplicationException(String.Format("Parent expected to be NamespaceOrTypeEntity but was {0}", parentEntity.GetType()));
      }

      // Determine FQN that is the key for finding the entity in the semantic graph.
      string fullyQualifiedName = parentNamespaceOrTypeEntity is RootNamespaceEntity
                                   ? node.Name
                                   : parentNamespaceOrTypeEntity.FullyQualifiedName + "." + node.Name;

      // Find out whether the entity already exists in the graph.
      var entity = _SemanticGraph.GetEntityByFullyQualifiedName(fullyQualifiedName) as TypeEntity;

      // If it already exists, that's an error (unless partial)
      if (entity != null)
      {
        // TODO: signal error or deal with partials
      }
      else
      {
        // Build the new entity
        entity = new TSemanticEntityType() {Name = node.Name};
        foreach (var baseType in node.BaseTypes)
        {
          entity.BaseTypes.Add(new NamespaceOrTypeEntityReference(baseType));
        }

        parentNamespaceOrTypeEntity.AddChildType(entity);
        _SemanticGraph.AddEntity(entity);
      }

      // Set a bidirectional link between AST and SG node
      entity.SyntaxNodes.Add(node);
      node.SemanticEntities.Add(entity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity associated with the parent of a syntax node. 
    /// This will be the parent of the entity created from this syntax node.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    /// <returns>The parent of the entity to be created from the given syntax node.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity GetParentEntity(ISyntaxNode node)
    {
      // If the syntax node is at the compilation unit level, then the entity will be created under the "global" namespace.
      if (node.Parent is CompilationUnitNode) { return _SemanticGraph.GlobalNamespace; }

      // If the parent of this syntax node has only 1 entity associated with it, then it will be the parent entity.
      var parentEntityCount = node.Parent.SemanticEntities.Count;
      if (parentEntityCount == 1) { return node.Parent.SemanticEntities[0]; }

      // If the parent of this syntax node is a NamespaceDeclarationNode, then it can have several entities associated with it
      // (eg. namespace A.B.C where A, B, C are all NamespaceEntity objects), then the parent entity is the last in the list.
      if (node.Parent is NamespaceDeclarationNode) { return node.Parent.SemanticEntities[parentEntityCount - 1]; }
      
      // All other cases are error.
      throw new ApplicationException("Cannot determine parent entity.");
    }
  }
}
