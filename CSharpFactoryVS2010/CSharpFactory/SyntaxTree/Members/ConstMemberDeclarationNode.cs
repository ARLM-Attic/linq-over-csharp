// ================================================================================================
// ConstMemberDeclarationNode.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a const member declaration node.
  /// </summary>
  // ================================================================================================
  public class ConstMemberDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstMemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberDeclarationNode(Token start)
      : base(start)
    {
      ConstTags = new ConstMemberTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberTagNodeCollection ConstTags { get; private set; }
  }
}