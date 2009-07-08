// ================================================================================================
// PrimaryExpressionNodeBase.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Common root class of all primary expression nodes.
  /// </summary>
  // ================================================================================================
  public abstract class PrimaryExpressionNodeBase : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryExpressionNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected PrimaryExpressionNodeBase(Token start)
      : base(start)
    {
    }
  }
}