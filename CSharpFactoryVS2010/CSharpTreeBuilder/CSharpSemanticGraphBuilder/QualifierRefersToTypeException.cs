namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a qualifier references a type instead of a namespace.
  /// </summary>
  // ================================================================================================
  public sealed class QualifierRefersToType : NamespaceOrTypeNameResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QualifierRefersToType"/> class.
    /// </summary>
    /// <param name="qualifier">The qualifier.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifierRefersToType(string qualifier)
    {
      Qualifier = qualifier;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier { get; private set; }
  }
}
