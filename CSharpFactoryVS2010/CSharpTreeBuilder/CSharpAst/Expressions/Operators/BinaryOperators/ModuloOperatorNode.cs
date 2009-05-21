// ================================================================================================
// ModuloOperatorNode.cs
//
// Created: 2009.04.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a modulo ("%") operator node.
  /// </summary>
  // ================================================================================================
  public class ModuloOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuloOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ModuloOperatorNode(Token start)
      : base(start)
    {
    }
  }
}