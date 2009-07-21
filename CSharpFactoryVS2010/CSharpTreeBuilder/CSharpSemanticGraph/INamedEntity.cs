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
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// Eg. for a class it's the name + number of type params; for methods it's the signature; etc.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string DistinctiveName { get; }
  }
}