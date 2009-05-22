// ================================================================================================
// DivideOperatorNode.cs
//
// Created: 2009.04.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a division ("/") operator node.
  /// </summary>
  // ================================================================================================
  public class DivideOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DivideOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public DivideOperatorNode(Token start)
      : base(start)
    {
    }
  }
}