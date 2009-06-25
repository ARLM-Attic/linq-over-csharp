// ================================================================================================
// BinaryOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Common root class of all binary operator nodes.
  /// </summary>
  // ================================================================================================
  public class BinaryOperatorNode : BinaryOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BinaryOperatorNode(Token start, BinaryOperatorType opType)
      : this(start, null, opType)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="secondToken">The second token.</param>
    /// <param name="opType">Type of the operator.</param>
    // ----------------------------------------------------------------------------------------------
    public BinaryOperatorNode(Token start, Token secondToken, BinaryOperatorType opType) : 
      base(start)
    {
      Operator = opType;
      SecondToken = secondToken;
      if (secondToken != null) Terminate(secondToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BinaryOperatorType Operator { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second operator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondToken { get; internal set; }

    /// <summary>
    /// Gets the right operand of the binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode RightOperand { get; internal set; }
  }
}