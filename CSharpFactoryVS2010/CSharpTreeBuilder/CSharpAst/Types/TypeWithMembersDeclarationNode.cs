// ================================================================================================
// TypeWithMembersDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
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