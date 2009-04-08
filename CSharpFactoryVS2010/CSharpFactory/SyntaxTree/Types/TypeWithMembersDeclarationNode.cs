// ================================================================================================
// TypeWithMembersDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
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

  // ================================================================================================
  /// <summary>
  /// This class represents a common base class for type declarations having members.
  /// </summary>
  // ================================================================================================
  public abstract class TypeWithMembersDeclarationNode: TypeWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeWithMembersDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeWithMembersDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }
  }
}