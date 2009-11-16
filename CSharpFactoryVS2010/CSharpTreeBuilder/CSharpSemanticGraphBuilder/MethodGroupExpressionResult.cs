namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a method group.
  /// </summary>
  // ================================================================================================
  public sealed class MethodGroupExpressionResult : ExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodGroupExpressionResult"/> class.
    /// </summary>
    /// <param name="methodGroup">The method group associated with this expression.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodGroupExpressionResult(MethodGroup methodGroup)
    {
      MethodGroup = methodGroup;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method group associated with the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MethodGroup MethodGroup { get; private set; }
  }
}
