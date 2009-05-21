// ================================================================================================
// ExpressionInitializerNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
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
      : base(expression == null ? new Token() : expression.StartToken)
      // TODO: Revise initialization
    {
      Expression = expression;
      Terminate(expression == null ? new Token() : expression.TerminatingToken);
      // TODO: Revise termination here
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }
  }
}