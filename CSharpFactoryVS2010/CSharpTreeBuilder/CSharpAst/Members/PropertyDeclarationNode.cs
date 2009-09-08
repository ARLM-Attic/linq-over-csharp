// ================================================================================================
// PropertyDeclarationNode.cs
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
  public class PropertyDeclarationNode : PropertyDeclarationNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "get" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode GetAccessor
    {
      get { return FindAccessor("get"); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the "set" accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode SetAccessor
    {
      get { return FindAccessor("set"); }
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

      if (Type != null)
      {
        Type.AcceptVisitor(visitor);
      }

      if (GetAccessor!=null)
      {
        GetAccessor.AcceptVisitor(visitor);
      }

      if (SetAccessor != null)
      {
        SetAccessor.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}