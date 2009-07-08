// ================================================================================================
// UnaryExpressionNodeBase.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the abstract base class of expressions with one operand.
  /// </summary>
  // ================================================================================================
  public abstract class UnaryExpressionNodeBase : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryExpressionNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected UnaryExpressionNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the operand of the unary expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Operand { get; internal set; }
  }
}