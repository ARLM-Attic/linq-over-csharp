// ================================================================================================
// EnumDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents an enum declaration.
  /// </summary>
  // ================================================================================================
  public class EnumDeclarationNode : TypeWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumDeclarationNode(Token start, Token name)
      : base(start, name)
    {
      Values = new EnumValueNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of enumeration values.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public EnumValueNodeCollection Values { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the orphan separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanSeparator { get; internal set; }
  }
}