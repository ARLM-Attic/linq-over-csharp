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
  public sealed class SemanticGraph : IMetadataToEntityMap
  {
    /// <summary>The name of the global namespace.</summary>
    private const string GLOBAL_NAMESPACE_NAME = "global";

    /// <summary>A dictionary of root namespace entities. The key is the name of the root namespace.</summary>
    private Dictionary<string, RootNamespaceEntity> _RootNamespaces;

    /// <summary>A cache that maps metadata objects to semantic entities.</summary>
    private Dictionary<System.Reflection.MemberInfo, SemanticEntity> _MetadataToEntityMap;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticGraph"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticGraph()
    {
      _RootNamespaces = new Dictionary<string, RootNamespaceEntity>();
      _MetadataToEntityMap = new Dictionary<System.Reflection.MemberInfo, SemanticEntity>();

      AddRootNamespace(new RootNamespaceEntity(GLOBAL_NAMESPACE_NAME));
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
    /// Gets a root namespace by name.
    /// </summary>
    /// <param name="rootNamespaceName">A root namespace name.</param>
    /// <returns>A root namespace entity with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity GetRootNamespaceByName(string rootNamespaceName)
    {
      if (_RootNamespaces.ContainsKey(rootNamespaceName))
      {
        return _RootNamespaces[rootNamespaceName];
      }
      return null;
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
    /// Adds an root namespace entity to the semantic graph.
    /// </summary>
    /// <param name="rootNamespaceEntity">A root namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddRootNamespace(RootNamespaceEntity rootNamespaceEntity)
    {
      _RootNamespaces.Add(rootNamespaceEntity.Name, rootNamespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a metadata+entity pair to the mapping cache.
    /// </summary>
    /// <param name="metadata">A metadata object.</param>
    /// <param name="semanticEntity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddMapping(System.Reflection.MemberInfo metadata, SemanticEntity semanticEntity)
    {
      if (!_MetadataToEntityMap.ContainsKey(metadata))
      {
        _MetadataToEntityMap.Add(metadata, semanticEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Indexer operator that returns the semantic entity that is mapped to the supplied metadata object.
    /// </summary>
    /// <param name="metadata">A metadata object.</param>
    /// <returns>The semantic entity mapped to the metadata object. Null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity this[System.Reflection.MemberInfo metadata]
    {
      get
      {
        return _MetadataToEntityMap.ContainsKey(metadata) ? _MetadataToEntityMap[metadata] : null;
      }
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
