// ================================================================================================
// ReturnStatementNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class ReturnStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ReturnStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expression">The expression belonging to the statement.</param>
    // ----------------------------------------------------------------------------------------------
    public ReturnStatementNode(Token start, ExpressionNode expression)
      : base(start)
    {
      Expression = expression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }

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

      base.AcceptVisitor(visitor);

      if (Expression!=null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}