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
    /// <param name="namespaceOrTypeName">The name of the namespace or type that caused the error.</param>
    /// <param name="fullyQualifiedName1">The fully qualified name of the first entity found.</param>
    /// <param name="fullyQualifiedName2">The fully qualified name of the second entity found.</param>
    // ----------------------------------------------------------------------------------------------
    public AmbigousReferenceException(string namespaceOrTypeName, string fullyQualifiedName1, string fullyQualifiedName2)
    {
      NamespaceOrTypeName = namespaceOrTypeName;
      FullyQualifiedName1 = fullyQualifiedName1;
      FullyQualifiedName2 = fullyQualifiedName2;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NamespaceOrTypeName { get; private set; }

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
