// ================================================================================================
// AddOperatorNode.cs
//
// Created: 2009.04.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines an addition ("+") operator node.
  /// </summary>
  // ================================================================================================
  public class AddOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AddOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AddOperatorNode(Token start)
      : base(start)
    {
    }
  }
}