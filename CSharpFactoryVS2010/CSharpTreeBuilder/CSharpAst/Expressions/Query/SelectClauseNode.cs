// ================================================================================================
// SelectClauseNode.cs
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
  public class SelectClauseNode : QueryBodyClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public SelectClauseNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the "select" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }
  }
}