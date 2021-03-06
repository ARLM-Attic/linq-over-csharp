﻿using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an unary operator expression node.
  /// </summary>
  // ================================================================================================
  public class UnaryOperatorExpressionNode : UnaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryOperatorExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="opType">The operator of the unary expression.</param>
    // ----------------------------------------------------------------------------------------------
    public UnaryOperatorExpressionNode(Token start, UnaryOperator opType)
      : base(start)
    {
      Operator = opType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the operator of the unary expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public UnaryOperator Operator { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}