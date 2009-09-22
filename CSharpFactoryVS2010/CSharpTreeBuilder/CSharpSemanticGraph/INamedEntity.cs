namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that have a name.
  /// </summary>
  // ================================================================================================
  public interface INamedEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string Name { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the entity (the name prefixed with the parent name).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string FullyQualifiedName { get; }
  }
}