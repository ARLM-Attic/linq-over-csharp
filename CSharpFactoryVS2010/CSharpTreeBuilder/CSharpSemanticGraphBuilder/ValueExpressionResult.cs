using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a value.
  /// </summary>
  // ================================================================================================
  public sealed class ValueExpressionResult : TypedExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueExpressionResult"/> class.
    /// </summary>
    /// <param name="typeEntity">The type associated with the expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ValueExpressionResult(TypeEntity typeEntity)
      : base(typeEntity)
    {
    }
  }
}
