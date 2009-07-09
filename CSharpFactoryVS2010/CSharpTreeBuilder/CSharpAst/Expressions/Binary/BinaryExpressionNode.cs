// ================================================================================================
// BinaryExpressionNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents all binary expressions except type testing and assignment expressions.
  /// </summary>
  // ================================================================================================
  public class BinaryExpressionNode : BinaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="opType">The operator type.</param>
    // ----------------------------------------------------------------------------------------------
    public BinaryExpressionNode(Token start, BinaryOperator opType)
      : this(start, null, opType)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="secondToken">The second token.</param>
    /// <param name="opType">Type of the operator.</param>
    // ----------------------------------------------------------------------------------------------
    public BinaryExpressionNode(Token start, Token secondToken, BinaryOperator opType) : 
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
    public BinaryOperator Operator { get; private set; }

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

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      if (LeftOperand != null)
      {
        LeftOperand.AcceptVisitor(visitor);
      }

      if (RightOperand != null)
      {
        RightOperand.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}