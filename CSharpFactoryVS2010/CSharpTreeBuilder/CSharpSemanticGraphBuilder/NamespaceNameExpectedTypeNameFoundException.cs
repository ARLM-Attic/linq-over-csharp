namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a namespace name is expected but a type name is found.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceNameExpectedTypeNameFoundException : NamespaceOrTypeNameNodeResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceNameExpectedTypeNameFoundException"/> class.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceNameExpectedTypeNameFoundException(string typeName)
    {
      TypeName = typeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string TypeName { get; private set; }
  }
}
