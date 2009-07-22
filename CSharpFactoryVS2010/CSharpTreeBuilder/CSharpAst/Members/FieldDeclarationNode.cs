// ================================================================================================
// FieldDeclarationNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field member declaration
  /// </summary>
  // ================================================================================================
  public class FieldDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldDeclarationNode(Token start)
      : base(start)
    {
      FieldTags = new FieldTagNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is event field.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is event field; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsEventField
    {
      get { return StartToken.Value == "event"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the field tags belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FieldTagNodeCollection FieldTags { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this field is declared with the static modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return (Modifiers != null && Modifiers.Contains(ModifierType.Static)); }
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

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      foreach (var fieldTag in FieldTags)
      {
        fieldTag.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}