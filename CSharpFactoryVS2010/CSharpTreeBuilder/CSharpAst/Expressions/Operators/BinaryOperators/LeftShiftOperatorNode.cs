// ================================================================================================
// LeftShiftOperatorNode.cs
//
// Created: 2009.04.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a left shift operator node.
  /// </summary>
  // ================================================================================================
  public class LeftShiftOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LeftShiftOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LeftShiftOperatorNode(Token start)
      : base(start)
    {
    }
  }
}