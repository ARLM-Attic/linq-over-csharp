// ================================================================================================
// DelegateDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a delegate declaration.
  /// </summary>
  // ================================================================================================
  public class DelegateDeclarationNode : TypeDeclarationNode
  {
    // --- Backing fields
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public DelegateDeclarationNode(Token start, Token name)
      : base(start, name)
    {
      IdentifierToken = name;
      FormalParameters = new FormalParameterNodeCollection() { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode Type
    {
      get { return _Type; }
      internal set
      {
        _Type = value;
        if (_Type != null) _Type.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNodeCollection FormalParameters { get; internal set; }

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

      foreach (var typeParameter in TypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      foreach (var typeParameterConstraint in TypeParameterConstraints)
      {
        typeParameterConstraint.AcceptVisitor(visitor);
      }

      foreach (var formalParameter in FormalParameters)
      {
        formalParameter.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}