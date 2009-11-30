// ================================================================================================
// PrimaryExpressionMemberAccessNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents the member access on a primary expression.
  /// </summary>
  // ================================================================================================
  public sealed class PrimaryExpressionMemberAccessNode : MemberAccessNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryExpressionMemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expressionNode">The primary expression node</param>
    // ----------------------------------------------------------------------------------------------
    public PrimaryExpressionMemberAccessNode(Token start, ExpressionNode expressionNode)
      : base(start)
    {
      if (expressionNode==null)
      {
        throw new ArgumentNullException("expressionNode");
      }

      if (!(expressionNode is PrimaryExpressionNodeBase))
      {
        throw new ArgumentException("Primary expression expected");
      }

      PrimaryExpression = expressionNode as PrimaryExpressionNodeBase;
      PrimaryExpression.ParentNode = this;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the primary expression part of the member access expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PrimaryExpressionNodeBase PrimaryExpression { get; private set; }

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

      if (PrimaryExpression != null)
      {
        PrimaryExpression.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}