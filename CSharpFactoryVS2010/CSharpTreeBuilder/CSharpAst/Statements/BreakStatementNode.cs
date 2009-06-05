// ================================================================================================
// BreakStatementNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a break statement.
  /// </summary>
  // ================================================================================================
  public class BreakStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BreakStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BreakStatementNode(Token start)
      : base(start)
    {
    }
  }
}