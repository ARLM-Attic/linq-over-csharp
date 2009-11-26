namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a simple name cannot be resolved.
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameUndefinedException : NamespaceOrTypeNameNodeResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameUndefinedException"/> class.
    /// </summary>
    /// <param name="simpleName">The simple name.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameUndefinedException(string simpleName)
    {
      SimpleName = simpleName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string SimpleName { get; private set; }
  }
}
