// ================================================================================================
// InterfaceDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Represents an interface class declaration.
  /// </summary>
  // ================================================================================================
  public class InterfaceDeclarationNode: TypeWithMembersDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public InterfaceDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }
  }
}