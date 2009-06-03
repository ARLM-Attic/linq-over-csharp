// ================================================================================================
// QueryBodyNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class QueryBodyNode : SyntaxNode<QueryExpressionNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBodyNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public QueryBodyNode(Token start)
      : base(start)
    {
    }
  }
}