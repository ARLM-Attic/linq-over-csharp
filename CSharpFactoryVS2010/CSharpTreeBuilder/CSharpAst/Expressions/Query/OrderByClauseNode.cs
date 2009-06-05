// ================================================================================================
// OrderByClauseNode.cs
//
// Created: 2009.06.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class OrderByClauseNode : QueryBodyClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderByClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public OrderByClauseNode(Token start)
      : base(start)
    {
      Orderings = new OrderingClauseNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of ordering items.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public OrderingClauseNodeCollection Orderings { get; private set; }
  }
}