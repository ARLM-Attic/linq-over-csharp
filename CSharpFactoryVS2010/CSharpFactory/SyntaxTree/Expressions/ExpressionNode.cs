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
    protected ExpressionNode(Token start) : base(start)
    {
    }
  }
}