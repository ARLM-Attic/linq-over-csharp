// ================================================================================================
// PointerModifierNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This type represents a pointer modifier.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   PointerModifierNode:
  ///     "*"
  /// </remarks>
  // ================================================================================================
  public sealed class PointerModifierNode : TypeModifierNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerModifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PointerModifierNode(Token start)
      : base(start)
    {
    }
  }
}