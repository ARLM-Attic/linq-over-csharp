namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a namespace-or-type-name is found in more than one imported namespaces.
  /// </summary>
  // ================================================================================================
  public sealed class AmbigousReferenceException : NamespaceOrTypeNameResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbigousReferenceException"/> class.
    /// </summary>
    /// <param name="fullyQualifiedName1">The fully qualified name of the first entity found.</param>
    /// <param name="fullyQualifiedName2">The fully qualified name of the second entity found.</param>
    // ----------------------------------------------------------------------------------------------
    public AmbigousReferenceException(string fullyQualifiedName1, string fullyQualifiedName2)
    {
      FullyQualifiedName1 = fullyQualifiedName1;
      FullyQualifiedName2 = fullyQualifiedName2;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the first entity found.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName1 { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the second entity found.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName2 { get; private set; }
  }
}
