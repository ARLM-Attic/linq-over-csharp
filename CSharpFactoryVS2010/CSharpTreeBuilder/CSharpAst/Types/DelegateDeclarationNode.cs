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
    private TypeOrNamespaceNode _TypeName;

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
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the formal parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterListNode FormalParameters { get; internal set; }

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

      foreach (var typeParameter in TypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      foreach (var typeParameterConstraint in TypeParameterConstraints)
      {
        typeParameterConstraint.AcceptVisitor(visitor);
      }

      if(FormalParameters!=null)
      {
        FormalParameters.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}