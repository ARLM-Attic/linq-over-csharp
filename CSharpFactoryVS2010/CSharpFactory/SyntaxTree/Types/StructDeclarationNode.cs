// ================================================================================================
// StructDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a struct declaration.
  /// </summary>
  // ================================================================================================
  public class StructDeclarationNode: TypeWithMembersDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StructDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public StructDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }
  }
}