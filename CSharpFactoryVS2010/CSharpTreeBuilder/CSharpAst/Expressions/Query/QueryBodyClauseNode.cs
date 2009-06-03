// ================================================================================================
// QueryBodyClauseNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>
  /// This abstract class in intended to be the base of all types representing query body clauses.
  /// </summary>
  // ================================================================================================
  public abstract class QueryBodyClauseNode : SyntaxNode<QueryExpressionNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBodyClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected QueryBodyClauseNode(Token start)
      : base(start)
    {
    }
  }
}