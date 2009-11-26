using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a method group.
  /// </summary>
  // ================================================================================================
  public sealed class MethodGroupExpressionResult : ExpressionResult, IHasInstanceExpression
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodGroupExpressionResult"/> class.
    /// </summary>
    /// <param name="methodGroup">The method group associated with this expression.</param>
    /// <param name="instanceExpression">The instance expression associated with this entity.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodGroupExpressionResult(MethodGroup methodGroup, ExpressionEntity instanceExpression)
    {
      MethodGroup = methodGroup;
      InstanceExpression = instanceExpression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method group associated with the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MethodGroup MethodGroup { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the instance expression associated with this expression result.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity InstanceExpression { get; private set; }

  }
}
