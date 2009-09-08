using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that resolves expressions.
  /// </summary>
  // ================================================================================================
  public sealed class ExpressionEvaluatorSemanticGraphVisitor : SemanticGraphVisitor
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves expressions in a semantic entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ScalarInitializerEntity entity)
    {
      if (entity.Expression != null)
      {
        entity.Expression.Evaluate();
      }

      return true;
    }
  }
}
