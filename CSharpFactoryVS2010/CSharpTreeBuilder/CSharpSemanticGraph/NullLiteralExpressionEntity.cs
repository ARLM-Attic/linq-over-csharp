using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a null literal expression entities.
  /// </summary>
  // ================================================================================================
  public class NullLiteralExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expressions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate()
    {
      Result = new NullExpressionResult();
    }
  }
}
