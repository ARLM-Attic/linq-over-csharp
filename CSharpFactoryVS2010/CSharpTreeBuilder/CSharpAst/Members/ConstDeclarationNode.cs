// ================================================================================================
// ConstDeclarationNode.cs
//
// Created: 2009.04.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a const member declaration node.
  /// </summary>
  // ================================================================================================
  public class ConstDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstDeclarationNode(Token start)
      : base(start)
    {
      ConstTags = new ConstTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstTagNodeCollection ConstTags { get; private set; }
  }
}