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

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      foreach (var attributeDecoration in AttributeDecorations)
      {
        attributeDecoration.AcceptVisitor(visitor);
      }

      if (Type != null)
      {
        Type.AcceptVisitor(visitor);
      }

      foreach (var constTag in ConstTags)
      {
        constTag.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}