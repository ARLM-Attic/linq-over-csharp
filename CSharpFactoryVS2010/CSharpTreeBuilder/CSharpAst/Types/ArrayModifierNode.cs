// ================================================================================================
// ArrayModifierNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a pointer modifier.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ArrayModifierNode:
  ///     "[" { "," } "]"
  /// </remarks>
  // ================================================================================================
  public sealed class ArrayModifierNode : TypeModifierNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayModifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayModifierNode(Token start)
      : base(start)
    {
      Separators = new ImmutableCollection<Token>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of separators.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<Token> Separators { get; private set; }

    // ----------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Rank
    {
      get { return Separators.Count + 1; }
    }

    /// <summary>
    /// Adds a new separator token.
    /// </summary>
    /// <param name="t">The token to add.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSeparator(Token t)
    {
      Separators.Add(t);
    }
  }
}