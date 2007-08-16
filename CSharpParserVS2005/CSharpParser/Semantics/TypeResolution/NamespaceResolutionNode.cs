using System.Collections.Generic;
using System.Reflection;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class describes a namespace node in the resolution tree.
  /// </summary>
  /// <remarks>
  /// Compound namespaces are represented by cascade namespace nodes. For example,
  /// "System.Collection.Generics" is represented by three nodes: 
  /// "System" --> "Collection" --> "Generics".
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceResolutionNode : ResolutionNodeBase
  {
    #region Private fields

    private readonly Dictionary<string, Assembly> _Resolvers =
      new Dictionary<string, Assembly>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new node with the specified parent and name.
    /// </summary>
    /// <param name="parentNode">Parent node.</param>
    /// <param name="name">Node name.</param>
    /// <remarks>
    /// The parent can be null (the node has no parent), name cannot be null or empty.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public NamespaceResolutionNode(ResolutionNodeBase parentNode, string name)
      : base(parentNode, name)
    {
    }

    #endregion

    #region public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolving reference with the specified name.
    /// </summary>
    /// <param name="key">Reference key</param>
    /// <returns>
    /// Resolving reference if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public Assembly this[string key]
    {
      get
      {
        Assembly result;
        if (_Resolvers.TryGetValue(key, out result)) return result;
        return null;
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a resolver to this node.
    /// </summary>
    /// <param name="key">Name of the resolver</param>
    /// <param name="resolvingAssembly">Resolving assembly instance</param>
    // --------------------------------------------------------------------------------
    public void AddResolver(string key, Assembly resolvingAssembly)
    {
      _Resolvers.Add(key, resolvingAssembly);
    }

    #endregion
  }
}
