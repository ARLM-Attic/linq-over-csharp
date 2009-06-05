// ================================================================================================
// CheckedStatementNode.cs
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
  public class CheckedStatementNode : BlockWrappingStatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="block">The block of statements.</param>
    // ----------------------------------------------------------------------------------------------
    public CheckedStatementNode(Token start, BlockStatementNode block)
      : base(start, block)
    {
    }
  }
}