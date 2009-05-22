// ================================================================================================
// NullCoalescingOperatorNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a null coalescing ("??") operator node.
  /// </summary>
  // ================================================================================================
  public class NullCoalescingOperatorNode : BinaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NullCoalescingOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NullCoalescingOperatorNode(Token start)
      : base(start)
    {
    }
  }
}