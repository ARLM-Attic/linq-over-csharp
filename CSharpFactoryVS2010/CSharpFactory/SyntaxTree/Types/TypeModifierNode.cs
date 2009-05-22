// ================================================================================================
// TypeModifierNode.cs
//
// Created: 2009.04.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a type modifier (pointer or array modifier).
  /// </summary>
  // ================================================================================================
  public abstract class TypeModifierNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeModifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeModifierNode(Token start)
      : base(start)
    {
    }
  }
}