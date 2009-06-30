// ================================================================================================
// UnaryOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the abstract base class of expressions with one operand.
  /// </summary>
  // ================================================================================================
  public abstract class UnaryOperatorNode : OperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected UnaryOperatorNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="operand">The operand of the unary expression.</param>
    // ----------------------------------------------------------------------------------------------
    protected UnaryOperatorNode(Token start, ExpressionNode operand)
      : base(start)
    {
      Operand = operand;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the operand of the unary expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Operand { get; internal set; }
  }
}