// ================================================================================================
// BinaryExpressionNodeBase.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This is the base class for all expressions with left and right operands. 
  /// LeftOperand is always a general expression, but RightOperand's type is defined by descendants.
  /// </summary>
  // ================================================================================================
  public abstract class BinaryExpressionNodeBase : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryExpressionNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected BinaryExpressionNodeBase(Token start)
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
    public BinaryExpressionNodeBase LeftmostExpressionWithMissingLeftOperand
    {
      get
      {
        if (LeftOperand == null) return this;
        
        var childLeftOperand = LeftOperand as BinaryExpressionNodeBase;
        if (childLeftOperand == null)
        {
          throw new InvalidOperationException("Binary operator expected.");
        }
        return childLeftOperand.LeftmostExpressionWithMissingLeftOperand;
      }
    }
  }
}