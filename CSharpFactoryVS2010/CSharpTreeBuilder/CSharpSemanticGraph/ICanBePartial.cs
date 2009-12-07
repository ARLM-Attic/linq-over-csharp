namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by entities that can be declared as partial.
  /// </summary>
  /// <remarks>
  /// A partial entity means that it has to merged from multiple declaration segments.
  /// </remarks>
  // ================================================================================================
  public interface ICanBePartial : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is declared as partial. 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsPartial { get; }
  }
}
