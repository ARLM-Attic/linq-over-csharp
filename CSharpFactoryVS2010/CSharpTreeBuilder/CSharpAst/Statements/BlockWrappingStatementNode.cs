// ================================================================================================
// BlockWrappingStatementNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a statement that simply wraps a statement block (like "checked", 
  /// "unsafe", etc.)
  /// </summary>
  /// <remarks>This class is used to be a base class for concrete statement node classes.</remarks>
  // ================================================================================================
  public abstract class BlockWrappingStatementNode: StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockWrappingStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="block">The block of statements.</param>
    // ----------------------------------------------------------------------------------------------
    protected BlockWrappingStatementNode(Token start, BlockStatementNode block)
      : base(start)
    {
      Block = block;
      Terminate(block.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the block of statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode Block { get; private set; }
  }
}