// ================================================================================================
// SizeofOperatorNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This type represents the "sizeof" operator
  /// </summary>
  // ================================================================================================
  public class SizeofOperatorNode : EmbeddedTypeOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public SizeofOperatorNode(Token start) : base(start)
    {
    }
  }
}