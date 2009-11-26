namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a type name is expected but a different entity is found.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNameExpectedException : NamespaceOrTypeNameNodeResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameExpectedException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="entityTypeName">The type name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNameExpectedException(string entityName, string entityTypeName)
    {
      EntityName = entityName;
      EntityTypeName = entityTypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string EntityName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type name of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string EntityTypeName { get; private set; }
  }
}
