// ================================================================================================
// UnaryMinusOperatorNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an unary minus operator.
  /// </summary>
  // ================================================================================================
  public sealed class UnaryMinusOperatorNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryMinusOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public UnaryMinusOperatorNode(Token start)
      : base(start)
    {
    }
  }
}