namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of objects that have a lexical scope.
  /// </summary>
  // ================================================================================================
  public interface IHasLexicalScope
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the region of program text where this object has effect on.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SourceRegion LexicalScope { get; }
  }
}
