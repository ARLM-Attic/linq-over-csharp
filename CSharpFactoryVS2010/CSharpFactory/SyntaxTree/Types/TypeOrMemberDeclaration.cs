// ================================================================================================
// TypeOrMemberDeclaration.cs
//
// Created: 2009.04.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the base class of or type and member declaration.
  /// </summary>
  // ================================================================================================
  public abstract class TypeOrMemberDeclaration : AttributedDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrMemberDeclaration"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeOrMemberDeclaration(AttributedDeclarationNode parent, Token start) : 
      base(parent, start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the modifiers of this declaration node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ModifierNodeCollection Modifiers { get; internal set; }
  }
}