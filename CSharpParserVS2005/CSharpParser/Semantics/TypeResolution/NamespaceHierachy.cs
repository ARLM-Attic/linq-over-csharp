namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace hierarchy that ise used when resolving type
  /// names.
  /// </summary>
  /// <remarks>
  /// During the compilation there is a global namespece hierarchy including the 
  /// types and namespaced declared in the source code plus types in assemblies not 
  /// aliased with the "extern alias" directive.
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceHierachy: ResolutionNodeBase
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace hierarchy with the specified name.
    /// </summary>
    /// <param name="name">Name of the namespace hierarchy.</param>
    // --------------------------------------------------------------------------------
    public NamespaceHierachy(string name): base(null, name)
    {
    }

    #endregion

    #region Public properties

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clear all nodes of this hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Clear()
    {
      Children.Clear();
    }

    #endregion
  }
}
