// ================================================================================================
// ParenthesisExpressionNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression closed between parentheses.
  /// </summary>
  /// <remarks>
  /// Parentheses are represented by the start and terminating token.
  /// </remarks>
  // ================================================================================================
  public sealed class ParenthesisExpressionNode : EmbeddedExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParenthesisExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public ParenthesisExpressionNode(Token start)
      : base(start)
    {
    }
  }
}