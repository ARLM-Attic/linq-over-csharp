namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when an alias name conflicts with a name declared in a namespace.
  /// </summary>
  // ================================================================================================
  public sealed class AliasNameConflictException : NamespaceOrTypeNameResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AliasNameConflictException"/> class.
    /// </summary>
    /// <param name="namespaceName">The name of the namespace where the conflict occured.</param>
    /// <param name="aliasName">The alias name that cause the conflict.</param>
    // ----------------------------------------------------------------------------------------------
    public AliasNameConflictException(string namespaceName, string aliasName)
    {
      NamespaceName = namespaceName;
      AliasName = aliasName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace where the conflict occured.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NamespaceName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias name that cause the conflict.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string AliasName { get; private set; }
  }
}
