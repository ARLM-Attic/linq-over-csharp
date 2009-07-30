namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that define a declaration space.
  /// </summary>
  // ================================================================================================
  public interface IDefinesDeclarationSpace
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    DeclarationSpace DeclarationSpace{ get; }
  }
}