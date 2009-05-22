// ================================================================================================
// GreaterThanOrEqualOperatorNode.cs
//
// Created: 2009.04.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a greater than or equal operator node.
  /// </summary>
  // ================================================================================================
  public class GreaterThanOrEqualOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GreaterThanOrEqualOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public GreaterThanOrEqualOperatorNode(Token start)
      : base(start)
    {
    }
  }
}