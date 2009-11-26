using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of expression results that has an instance expression.
  /// </summary>
  // ================================================================================================
  public interface IHasInstanceExpression
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the instance expression associated with this expression result.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ExpressionEntity InstanceExpression { get; }
  }
}
