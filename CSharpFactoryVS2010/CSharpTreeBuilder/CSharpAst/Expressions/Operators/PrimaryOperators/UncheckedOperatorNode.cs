// ================================================================================================
// UncheckedOperatorNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "unchecked" operator.
  /// </summary>
  // ================================================================================================
  public sealed class UncheckedOperatorNode : CheckedOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedOperatorNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public UncheckedOperatorNode(Token start) : base(start)
    {
    }
  }
}