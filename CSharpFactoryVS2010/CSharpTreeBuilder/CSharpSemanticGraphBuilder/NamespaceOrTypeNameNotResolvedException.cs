namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when the namespace-or-type-name resolution cannot resolve a name.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNotResolvedException : NamespaceOrTypeNameResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNotResolvedException"/> class.
    /// </summary>
    /// <param name="namespaceOrTypeName">The name of the namespace or type.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNotResolvedException(string namespaceOrTypeName)
    {
      NamespaceOrTypeName = namespaceOrTypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NamespaceOrTypeName { get; private set; }
  }
}
