using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class holds all the semantic information about a project 
  /// (a project is a group of source files compiled together and a group of referenced assemblies).
  /// </summary>
  // ================================================================================================
  public sealed class SemanticGraph
  {
    /// <summary>The name of the global namespace.</summary>
    private const string GLOBAL_NAMESPACE_NAME = "global";

    /// <summary>This collection contains all entities that belong to the graph.</summary>
    private readonly HashSet<SemanticEntity> _SemanticEntities;

    /// <summary>A dictionary of root namespace entities. The key is the name of the root namespace.</summary>
    private Dictionary<string, RootNamespaceEntity> _RootNamespaces;

    /// <summary>A dictionary of namespace-or-type entities. The key is: 'root-namespace-name::FQN' .</summary>
    private Dictionary<string, NamespaceOrTypeEntity> _NamespaceOrTypeEntities;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticGraph"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticGraph()
    {
      _SemanticEntities = new HashSet<SemanticEntity>();
      _RootNamespaces = new Dictionary<string, RootNamespaceEntity>();
      _NamespaceOrTypeEntities = new Dictionary<string, NamespaceOrTypeEntity>();

      AddEntity(new RootNamespaceEntity(GLOBAL_NAMESPACE_NAME));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of all semantic entities in the graph.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<SemanticEntity> SemanticEntities
    {
      get { return _SemanticEntities; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an array of semantic entities in the graph, for unit testing purposes only.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity[] SemanticEntitiesForUnitTest
    {
      get
      {
        var semanticEntityArray = new SemanticEntity[_SemanticEntities.Count];
        _SemanticEntities.CopyTo(semanticEntityArray);
        return semanticEntityArray;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of root namespace entities. The key is the namespace name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<RootNamespaceEntity> RootNamespaces
    {
      get { return _RootNamespaces.Values; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global namespace entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity GlobalNamespace
    {
      get { return _RootNamespaces[GLOBAL_NAMESPACE_NAME]; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an entity to the semantic graph.
    /// If the entity is a root namespace entity then it is added to the RootNamespaces collection as well.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddEntity(SemanticEntity entity)
    {
      // If the entity already exists in the graph, that's an error
      if (_SemanticEntities.Contains(entity))
      {
        throw new InvalidOperationException(String.Format("The entity already exists in the semantic graph: {0}", entity));
      }

      // Add entity to the graph
      _SemanticEntities.Add(entity);

      // If it's a root namespace then also add to RootNamespaces dictionary
      if (entity is RootNamespaceEntity)
      {
        var rootNamespaceEntity = entity as RootNamespaceEntity;
        _RootNamespaces.Add(rootNamespaceEntity.Name, rootNamespaceEntity);
      }

      // If it's a namespace or type then also add it to NamespaceOrTypeEntities dictionary
      if (entity is NamespaceOrTypeEntity)
      {
        var namespaceOrTypeEntity = entity as NamespaceOrTypeEntity;
        _NamespaceOrTypeEntities.Add(namespaceOrTypeEntity.FqnWithRoot, namespaceOrTypeEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity by its fully qualified name under a given root namespace.
    /// </summary>
    /// <param name="rootNamespaceName">The name of a root namespace.</param>
    /// <param name="fullyQualifiedName">A fully qualified name.</param>
    /// <returns>A namespace or type entity with the given fully qualified name, or null if not found.</returns>
    /// <remarks>Only namespaces and types have a FQN.</remarks>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity GetEntityByFullyQualifiedName(string rootNamespaceName, string fullyQualifiedName)
    {
      string key = string.Format("{0}::{1}", rootNamespaceName, fullyQualifiedName);
      return _NamespaceOrTypeEntities.ContainsKey(key)
               ? _NamespaceOrTypeEntities[key]
               : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity by its fully qualified name, in the "global" root namespace. 
    /// </summary>
    /// <param name="fullyQualifiedName">A fully qualified name.</param>
    /// <returns>A namespace or type entity with the given fully qualified name, or null if not found.</returns>
    /// <remarks>Only namespaces and types have a FQN.</remarks>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity GetEntityByFullyQualifiedName(string fullyQualifiedName)
    {
      return GetEntityByFullyQualifiedName(GLOBAL_NAMESPACE_NAME, fullyQualifiedName);
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      foreach (var rootNamespace in RootNamespaces)
      {
        rootNamespace.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
