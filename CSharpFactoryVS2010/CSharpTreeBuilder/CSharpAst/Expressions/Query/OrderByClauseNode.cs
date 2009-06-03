// ================================================================================================
// OrderByClauseNode.cs
//
// Created: 2009.06.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class OrderByClauseNode : QueryBodyClauseNode
  {
    public OrderByClauseNode(Token start) : base(start)
    {
    }
  }
}