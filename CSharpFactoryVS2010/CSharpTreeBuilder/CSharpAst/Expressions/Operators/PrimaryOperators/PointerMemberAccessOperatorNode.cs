// ================================================================================================
// PointerMemberAccessOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a pointer member access operator ("->").
  /// </summary>
  // ================================================================================================
  public class PointerMemberAccessOperatorNode : MemberAccessOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerMemberAccessOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expressionNode">The primary expression node</param>
    // ----------------------------------------------------------------------------------------------
    public PointerMemberAccessOperatorNode(Token start, ExpressionNode expressionNode)
      : base(start)
    {
      if (expressionNode == null)
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