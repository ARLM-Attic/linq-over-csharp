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
  }
}
