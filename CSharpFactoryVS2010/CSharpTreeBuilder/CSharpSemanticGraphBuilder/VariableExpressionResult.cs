using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a variable.
  /// </summary>
  // ================================================================================================
  public sealed class VariableExpressionResult : TypedExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableExpressionResult"/> class.
    /// </summary>
    /// <param name="variable">The variable associated with this expression.</param>
    // ----------------------------------------------------------------------------------------------
    public VariableExpressionResult(IVariableEntity variable)
      : base(variable.Type)
    {
      Variable = variable;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the variable associated with this expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IVariableEntity Variable { get; private set; }
  }
}
