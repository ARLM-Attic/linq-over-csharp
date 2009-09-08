// ================================================================================================
// AssignmentExpressionNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines an assignment ("=") expression node.
  /// </summary>
  // ================================================================================================
  public class AssignmentExpressionNode : BinaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignmentExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="second">The second operator token.</param>
    /// <param name="opType">Type of the operator.</param>
    // ----------------------------------------------------------------------------------------------
    public AssignmentExpressionNode(Token start, Token second, AssignmentOperator opType)
      : base(start)
    {
      SecondToken = second;
      if (second != null) Terminate(second);
      Operator = opType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AssignmentOperator Operator { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second operator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
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
      if (!visitor.Visit(this)) { return; }

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