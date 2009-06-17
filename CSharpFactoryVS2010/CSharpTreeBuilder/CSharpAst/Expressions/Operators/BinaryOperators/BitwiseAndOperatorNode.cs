// ================================================================================================
// BitwiseAndOperatorNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a bitwise and operator node.
  /// </summary>
  // ================================================================================================
  public class BitwiseAndOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BitwiseAndOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BitwiseAndOperatorNode(Token start)
      : base(start)
    {
    }
  }
}