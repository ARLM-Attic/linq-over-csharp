namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a statement in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class StatementEntity : SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StatementEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public StatementEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StatementEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    protected StatementEntity(StatementEntity source)
      : base(source)
    {
    }
  }
}
