// ================================================================================================
// GroupByClauseNode.cs
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
  public class GroupByClauseNode : QueryBodyClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupByClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public GroupByClauseNode(Token start)
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
  }
}