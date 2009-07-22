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

    /// <summary>The project used for reporting compilation messages.</summary>
    private CSharpProject _Project;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBuilderSyntaxNodeVisitor"/> class.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph that will receive the built entities.</param>
    /// <param name="project">The project used for reporting compilation messages.</param>
    // ----------------------------------------------------------------------------------------------
    public EntityBuilderSyntaxNodeVisitor(SemanticGraph semanticGraph, CSharpProject project)
    {
      _SemanticGraph = semanticGraph;
      _Project = project;
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
                                     ? parentNamespaceEntity.FullyQualifiedName + "::"+ nameTag.Identifier
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
        if (entity is GenericCapableTypeEntity)
        {
          foreach (var typeParameter in node.TypeParameters)
          {
            var typeParameterEntity = new TypeParameterEntity(typeParameter.Identifier);
            ((GenericCapableTypeEntity)entity).AddTypeParameter(typeParameterEntity);
            _SemanticGraph.AddEntity(typeParameterEntity);
          }
        }

        // Add the entity to its parent
        if (parentNamespaceOrTypeEntity is IHasChildTypes)
        {
          ((IHasChildTypes)parentNamespaceOrTypeEntity).AddChildType(entity);
          _SemanticGraph.AddEntity(entity);
        }
        else
        {
          throw new ApplicationException(String.Format("Type '{0}' cannot not declare child type '{1}'.",
            parentNamespaceOrTypeEntity.Name, entity.Name));
        }
      }

      AssociateSyntaxNodeWithSemanticEntity(node, entity);
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
          ((ICompilationErrorHandler) _Project).Error( "CS0102", node.StartToken, 
            "The type '{0}' already contains a definition for '{1}'.",
            parentTypeEntity.FullyQualifiedName, fieldTag.Identifier);
          
          // Continue with the next field tag.
          continue;
        }

        // Create a semantic entity, add to its parent, and add to the graph.
        var fieldEntity = new FieldEntity(fieldTag.Identifier, true, new TypeEntityReference(node.TypeName), node.IsStatic);
        parentTypeEntity.AddMember(fieldEntity);
        _SemanticGraph.AddEntity(fieldEntity);

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
      semanticEntity.SyntaxNodes.Add(syntaxNode);
      syntaxNode.SemanticEntities.Add(semanticEntity);
    }
  }
}
