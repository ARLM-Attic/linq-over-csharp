// ================================================================================================
// MemberDeclarationNode.cs
//
// Created: 2009.04.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is an abstraction root for all type member declarations.
  /// </summary>
  // ================================================================================================
  public abstract class MemberDeclarationNode : TypeOrMemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberDeclarationNode(Token start)
      : base(start)
    {
    }
  }
}