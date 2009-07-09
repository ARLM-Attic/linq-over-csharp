// ================================================================================================
// GroupClauseNode.cs
//
// Created: 2009.06.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class GroupClauseNode : QueryBodyClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public GroupClauseNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "group" expression of the "group by" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode GroupExpression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "by" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ByToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "by" expression of the "group by" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode ByExpression { get; internal set; }

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

      if (GroupExpression != null)
      {
        GroupExpression.AcceptVisitor(visitor);
      }

      if (ByExpression != null)
      {
        ByExpression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}