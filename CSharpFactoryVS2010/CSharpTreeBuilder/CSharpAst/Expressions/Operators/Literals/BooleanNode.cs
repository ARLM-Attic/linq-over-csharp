// ================================================================================================
// BooleanNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a boolean literal node.
  /// </summary>
  // ================================================================================================
  public abstract class BooleanNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected BooleanNode(Token start)
      : base(start)
    {
    }
  }
}