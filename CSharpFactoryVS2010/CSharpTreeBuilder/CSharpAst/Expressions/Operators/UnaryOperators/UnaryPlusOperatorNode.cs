// ================================================================================================
// UnaryPlusOperatorNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an unary plus operator.
  /// </summary>
  // ================================================================================================
  public sealed class UnaryPlusOperatorNode : UnaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryPlusOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public UnaryPlusOperatorNode(Token start)
      : base(start)
    {
    }
  }
}