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
      var accOp = exprNode as MemberAccessOperatorNodeBase;
      if (accOp != null) return accOp.ScopeOperand.LeftmostExpression();
      var metOp = exprNode as MethodInvocationOperatorNode;
      return metOp != null ? metOp.ScopeOperand.LeftmostExpression() : exprNode;
    }
  }
}