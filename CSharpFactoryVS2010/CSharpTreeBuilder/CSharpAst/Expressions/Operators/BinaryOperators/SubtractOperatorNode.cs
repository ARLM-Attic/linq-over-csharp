// ================================================================================================
// SubtractOperatorNode.cs
//
// Created: 2009.04.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a subtraction ("-") operator node.
  /// </summary>
  // ================================================================================================
  public class SubtractOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SubtractOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public SubtractOperatorNode(Token start)
      : base(start)
    {
    }
  }
}