// ================================================================================================
// UncheckedStatementNode.cs
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
  public class UncheckedStatementNode : BlockWrappingStatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UncheckedStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="block">The block of statements.</param>
    // ----------------------------------------------------------------------------------------------
    public UncheckedStatementNode(Token start, BlockStatementNode block)
      : base(start, block)
    {
    }
  }
}