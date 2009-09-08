// ================================================================================================
// EventPropertyDeclarationNode.cs
//
// Created: 2009.05.17, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a property declaration node.
  /// </summary>
  // ================================================================================================
  public class EventPropertyDeclarationNode : PropertyDeclarationNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EventPropertyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public EventPropertyDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "add" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode AddAccessor
    {
      get { return FindAccessor("add"); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "remove" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode RemoveAccessor
    {
      get { return FindAccessor("remove"); }
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
      if (!visitor.Visit(this)) { return; }

      foreach (var attributeDecoration in AttributeDecorations)
      {
        attributeDecoration.AcceptVisitor(visitor);
      }

      if (Type!=null)
      {
        Type.AcceptVisitor(visitor);
      }

      if (AddAccessor!=null)
      {
        AddAccessor.AcceptVisitor(visitor);
      }

      if (RemoveAccessor != null)
      {
        RemoveAccessor.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}