// ================================================================================================
// UnaryOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
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
}