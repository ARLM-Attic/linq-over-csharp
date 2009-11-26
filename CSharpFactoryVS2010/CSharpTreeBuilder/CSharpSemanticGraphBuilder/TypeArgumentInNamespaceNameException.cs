namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a type argument is found in a namespace name.
  /// </summary>
  // ================================================================================================
  public sealed class TypeArgumentInNamespaceNameException : NamespaceOrTypeNameNodeResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgumentInNamespaceNameException"/> class.
    /// </summary>
    /// <param name="namespaceName">The name of the namespace.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentInNamespaceNameException(string namespaceName)
    {
      NamespaceName = namespaceName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NamespaceName { get; private set; }
  }
}
