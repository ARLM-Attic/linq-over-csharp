// ================================================================================================
// TypeWithBodyDeclarationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents type declarations having bodies.
  /// </summary>
  // ================================================================================================
  public abstract class TypeWithBodyDeclarationNode : TypeDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeWithBodyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeWithBodyDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening brace token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing brace token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBrace { get; internal set; }
  }
}