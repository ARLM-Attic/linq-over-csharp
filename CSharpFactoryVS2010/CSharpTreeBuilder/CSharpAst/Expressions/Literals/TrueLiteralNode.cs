// ================================================================================================
// TrueLiteralNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a "true" boolean literal node.
  /// </summary>
  // ================================================================================================
  public sealed class TrueLiteralNode : BooleanLiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanLiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TrueLiteralNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the literal.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool Value { get { return true; } }

  }
}