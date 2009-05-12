// ================================================================================================
// ExpressionNode.cs
//
// Created: 2009.03.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node represents an expression.
  /// </summary>
  // ================================================================================================
  public abstract class ExpressionNode: SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected ExpressionNode(Token start)
      : base(start)
    {
    }
  }
}