// ================================================================================================
// BitwiseXorOperatorNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a bitwise xor ("^") operator node.
  /// </summary>
  // ================================================================================================
  public class BitwiseXorOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BitwiseXorOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BitwiseXorOperatorNode(Token start)
      : base(start)
    {
    }
  }
}