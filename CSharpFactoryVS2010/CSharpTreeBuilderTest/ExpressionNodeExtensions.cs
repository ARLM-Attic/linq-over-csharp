// ================================================================================================
// ExpressionNodeExtensions.cs
//
// Created: 2009.06.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// This class contains extensions for the ExpressionNode class for testing purposes.
  /// </summary>
  // ================================================================================================
  public static class ExpressionNodeExtensions
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost part of this expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static ExpressionNode LeftmostExpression(this ExpressionNode exprNode)
    {
      var unOp = exprNode as UnaryOperatorNode;
      if (unOp != null) return unOp.Operand.LeftmostExpression();
      var binOp = exprNode as BinaryOperatorNode;
      if (binOp != null) return binOp.LeftOperand.LeftmostExpression();
      var accOp = exprNode as PrimaryMemberAccessOperatorNode;
      if (accOp != null) return accOp.PrimaryExpression.LeftmostExpression();
      var metOp = exprNode as InvocationOperatorNode;
      return metOp != null ? metOp.PrimaryExpression.LeftmostExpression() : exprNode;
    }
  }
}