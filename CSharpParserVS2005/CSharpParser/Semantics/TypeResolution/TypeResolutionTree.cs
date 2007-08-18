using System.Collections.Generic;

namespace CSharpParser.Semantics
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
    public virtual void Clear()
    {
      _Cache.Clear();
      Children.Clear();
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Registers a namespace node.
    /// </summary>
    /// <param name="nameSpace">Namespace to register.</param>
    /// <param name="node">The node representing the namespace.</param>
    /// <param name="conflictingNode">Node causing collision.</param>
    /// <returns>
    /// True, if the node successfully registered; otherwise, false. False actually 
    /// means that there is a name collision with a type name.
    /// </returns>
    /// <remarks>
    /// If the namespace node is already registered, does not create any new node. If
    /// any namespace node is missing, this methods creates that node.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool RegisterNamespace(string nameSpace, out NamespaceResolutionNode node,
                                           out ResolutionNodeBase conflictingNode)
    {
      bool result = base.RegisterNamespace(nameSpace, out node, out conflictingNode);
      if (result && !_Cache.ContainsKey(nameSpace))
      {
        _Cache.Add(nameSpace, node);
      }
      return result;
    }

    #endregion
  }
}