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

    /// <summary>A dictionary of namespace-or-type entities. 
    /// The key is the fully qualified name (including 'root-namespace-name::' too.</summary>
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
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <remarks>
    /// Root namespace entities are also added to the _RootNamespaces collection.
    /// Namespace and type entities are also added to the _NamespaceOrTypeEntities collection
    /// (except TypeParameters which are not visible outside of the defining type).
    /// </remarks>
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

      // If it's a namespace or type (but not a type parameter) then also add it to NamespaceOrTypeEntities dictionary
      if (entity is NamespaceOrTypeEntity && !(entity is TypeParameterEntity))
      {
        var namespaceOrTypeEntity = entity as NamespaceOrTypeEntity;
        _NamespaceOrTypeEntities.Add(namespaceOrTypeEntity.FullyQualifiedName, namespaceOrTypeEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity by its fully qualified name under a given root namespace.
    /// </summary>
    /// <param name="fullyQualifiedName">A fully qualified name.</param>
    /// <returns>A namespace or type entity with the given fully qualified name, or null if not found.</returns>
    /// <remarks>Only namespaces and types have a FQN.</remarks>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity GetEntityByFullyQualifiedName(string fullyQualifiedName)
    {
      return _NamespaceOrTypeEntities.ContainsKey(fullyQualifiedName)
               ? _NamespaceOrTypeEntities[fullyQualifiedName]
               : null;
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
