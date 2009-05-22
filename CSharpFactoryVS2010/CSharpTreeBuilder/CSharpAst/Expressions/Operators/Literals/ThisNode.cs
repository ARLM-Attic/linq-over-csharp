// ================================================================================================
// ThisNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a "this" literal node.
  /// </summary>
  // ================================================================================================
  public sealed class ThisNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ThisNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ThisNode(Token start)
      : base(start)
    {
    }
  }
}