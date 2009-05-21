// ================================================================================================
// BlockStatementNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a block statement encapsulating other statements.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   BlockStatementNode:
  ///     "{" { StatementNode } "}"
  /// </remarks>
  // ================================================================================================
  public sealed class BlockStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode(Token start)
      : base(start)
    {
      Statements = new StatementNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements belonging to this block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public StatementNodeCollection Statements { get; private set; }
  }
}