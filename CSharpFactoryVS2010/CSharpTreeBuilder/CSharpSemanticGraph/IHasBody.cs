namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that has a body (which is a block statement).
  /// </summary>
  // ================================================================================================
  public interface IHasBody
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    BlockEntity Body { get; }
  }
}
