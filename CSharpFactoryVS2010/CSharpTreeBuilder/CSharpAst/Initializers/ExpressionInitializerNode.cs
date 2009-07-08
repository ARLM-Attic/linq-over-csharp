// ================================================================================================
// ExpressionInitializerNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression initializer.
  /// </summary>
  // ================================================================================================
  public sealed class ExpressionInitializerNode : VariableInitializerNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionInitializerNode"/> class.
    /// </summary>
    /// <param name="expression">Initializer expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ExpressionInitializerNode(ExpressionNode expression)
      : base(expression.StartToken)
    {
      Expression = expression;
      Terminate(expression.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }
  }
}