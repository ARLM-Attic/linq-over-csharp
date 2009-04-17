// ================================================================================================
// OperatorNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Common root class of all operator nodes.
  /// </summary>
  // ================================================================================================
  public abstract class OperatorNode : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected OperatorNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// Common root class of all unary operator nodes.
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
    /// <param name="operand">The operand of the unary operator.</param>
    // ----------------------------------------------------------------------------------------------
    protected UnaryOperatorNode(Token start, ExpressionNode operand)
      : base(start)
    {
      Operand = operand;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the operand of the unary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Operand { get; internal set; }
  }

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
  }

  // ================================================================================================
  /// <summary>
  /// Common root class of all primary operator nodes.
  /// </summary>
  // ================================================================================================
  public abstract class PrimaryOperatorNode : OperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected PrimaryOperatorNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// Common root class of all literal nodes.
  /// </summary>
  // ================================================================================================
  public abstract class LiteralNode : OperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected LiteralNode(Token start)
      : base(start)
    {
    }
  }
}