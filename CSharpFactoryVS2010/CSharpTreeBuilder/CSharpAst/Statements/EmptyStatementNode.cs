// ================================================================================================
// EmptyStatementNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an empty statement.
  /// </summary>
  // ================================================================================================
  public class EmptyStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public EmptyStatementNode(Token start)
      : base(start)
    {
    }
  }
}