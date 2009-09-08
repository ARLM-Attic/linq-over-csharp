namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as null.
  /// </summary>
  /// <remarks>
  /// An expression with this classification can be implicitly converted to a reference type 
  /// or nullable type.
  /// </remarks>
  // ================================================================================================
  public sealed class NullExpressionResult : ExpressionResult
  {
  }
}
