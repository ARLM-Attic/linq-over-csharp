using System.Collections.Generic;

namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents an abstract tree that can be used for resolving types
  /// within an assembly or within the source code.
  /// </summary>
  // ==================================================================================
  public abstract class TypeResolutionTree : ResolutionNodeBase
  {
    #region Private fields

    // --- Contains a shortcut cache to accelerate nested namespace node access
    private readonly Dictionary<string, NamespaceResolutionNode> _Cache =
      new Dictionary<string, NamespaceResolutionNode>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the resolution tree.
    /// </summary>
    /// <param name="parentNode">Parent node</param>
    /// <param name="name">Resolution tree name</param>
    // --------------------------------------------------------------------------------
    protected TypeResolutionTree(ResolutionNodeBase parentNode, string name)
      : 
      base(parentNode, name)
    {
    }

    #endregion

    #region public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clear all nodes of this hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void Clear()
    {
      base.Clear();
      _Cache.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver node belonging to the specified namespace key.
    /// </summary>
    /// <param name="nsKey">Namespace key</param>
    /// <returns>
    /// The resolver node belonging to the namespace if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public NamespaceResolutionNode this[string nsKey]
    {
      get
      {
        NamespaceResolutionNode result;
        if (_Cache.TryGetValue(nsKey, out result)) return result;
        return null;
      }
    }

    #endregion

    #region Type resolution methods

    #endregion
  }
}