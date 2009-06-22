// ================================================================================================
// InterfaceEventDeclarationNode.cs
//
// Created: 2009.06.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class InterfaceEventDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InterfaceEventDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public InterfaceEventDeclarationNode(Token start)
      : base(start)
    {
    }

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

      if (TypeName!=null)
      {
        TypeName.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}