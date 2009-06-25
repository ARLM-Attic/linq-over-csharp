// ================================================================================================
// BinaryOperatorNodeBase.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  /// <summary>
  /// 
  /// </summary>
  public abstract class BinaryOperatorNodeBase : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected BinaryOperatorNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the left operand of the binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode LeftOperand { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost expression of this expression node with missing operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BinaryOperatorNodeBase LeftmostWithMissingLeftOperand
    {
      get
      {
        if (LeftOperand == null) return this;
        var binOp = LeftOperand as BinaryOperatorNodeBase;
        if (binOp == null)
        {
          throw new InvalidOperationException("Binary operator expected.");
        }
        return binOp.LeftmostWithMissingLeftOperand;
      }
    }
  }
}