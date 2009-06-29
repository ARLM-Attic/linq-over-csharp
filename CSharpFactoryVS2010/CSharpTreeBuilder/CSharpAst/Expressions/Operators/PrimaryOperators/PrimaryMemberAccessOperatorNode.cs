// ================================================================================================
// MemberAccessOperatorNode.cs
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
  public class PrimaryMemberAccessOperatorNode : MemberAccessOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryMemberAccessOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expressionNode">The primary expression node</param>
    // ----------------------------------------------------------------------------------------------
    public PrimaryMemberAccessOperatorNode(Token start, ExpressionNode expressionNode)
      : base(start)
    {
      if (expressionNode==null)
      {
        throw new ArgumentNullException("expressionNode");
      }

      if (!(expressionNode is PrimaryOperatorNode))
      {
        throw new ArgumentException("Primary expression expected");
      }

      PrimaryExpression = expressionNode as PrimaryOperatorNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the primary expression part of the member access expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PrimaryOperatorNode PrimaryExpression { get; private set; }
  }
}