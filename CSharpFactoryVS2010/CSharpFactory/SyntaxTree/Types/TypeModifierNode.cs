// ================================================================================================
// TypeModifierNode.cs
//
// Created: 2009.04.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
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
    /// <summary>
    /// Adds a new separator token.
    /// </summary>
    /// <param name="t">The token to add.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSeparator(Token t)
    {
      Separators.Add(t);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Rank
    {
      get { return Separators.Count + 1; }
    }
  }
}