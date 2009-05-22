// ================================================================================================
// EmbeddedExpressionNode.cs
//
// Created: 2009.04.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents an expression that has a single embedded expression.
  /// </summary>
  // ================================================================================================
  public abstract class EmbeddedExpressionNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    protected EmbeddedExpressionNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression between parentheses.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }
  }
}