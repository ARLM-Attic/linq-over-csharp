// ================================================================================================
// ParenthesisExpressionNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression closed between parentheses.
  /// </summary>
  /// <remarks>
  /// Parentheses are represented by the start and terminating token.
  /// </remarks>
  // ================================================================================================
  public sealed class ParenthesizedExpressionNode : PrimaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParenthesizedExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public ParenthesizedExpressionNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression between parentheses.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

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

      if (Expression!=null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}