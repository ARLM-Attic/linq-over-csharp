// ================================================================================================
// BinaryOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Common root class of all binary operator nodes.
  /// </summary>
  // ================================================================================================
  public abstract class BinaryOperatorNode : OperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected BinaryOperatorNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="leftOperand">The left operand.</param>
    // ----------------------------------------------------------------------------------------------
    protected BinaryOperatorNode(Token start, ExpressionNode leftOperand)
      : base(start)
    {
      LeftOperand = leftOperand;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="leftOperand">The left operand.</param>
    /// <param name="rightOperand">The right operand.</param>
    // ----------------------------------------------------------------------------------------------
    protected BinaryOperatorNode(Token start, ExpressionNode leftOperand, ExpressionNode rightOperand)
      : base(start)
    {
      LeftOperand = leftOperand;
      RightOperand = rightOperand;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the left operand of the binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode LeftOperand { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the right operand of the binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode RightOperand { get; internal set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost expression that has a null left operand including this 
    /// operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BinaryOperatorNode LeftmostNonNull
    {
      get
      {
        var current = this;
        do
        {
          if (current.LeftOperand == null) return current;
          var next = current.LeftOperand as BinaryOperatorNode;
          if (next == null) return current;
          current = next;
        } while (true);
      }
    }
  }
}