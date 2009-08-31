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
  }
}