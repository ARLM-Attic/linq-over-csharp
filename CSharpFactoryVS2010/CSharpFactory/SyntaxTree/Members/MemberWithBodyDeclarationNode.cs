// ================================================================================================
// MemberWithBodyDeclarationNode.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type member declaration with a body.
  /// </summary>
  // ================================================================================================
  public abstract class MemberWithBodyDeclarationNode : MemberWithNameDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberWithBodyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberWithBodyDeclarationNode(Token start)
      : base(start)
    {
    }
  }
}