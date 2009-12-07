namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that has a body (which is a block statement).
  /// </summary>
  // ================================================================================================
  public interface IHasBody : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    BlockStatementEntity Body { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the owner of the body is abstract (ie. the body is null).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsAbstract { get; }
  }
}
