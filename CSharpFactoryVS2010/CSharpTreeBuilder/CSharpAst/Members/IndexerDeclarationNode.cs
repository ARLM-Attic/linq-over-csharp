// ================================================================================================
// IndexerDeclarationNode.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class IndexerDeclarationNode : PropertyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IndexerDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public IndexerDeclarationNode(Token start)
      : base(start)
    {
      FormalParameters = new FormalParameterNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional member name separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token MemberNameSeparator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "this" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ThisToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the node representing formal parameters.
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
      visitor.Visit(this);

      foreach (var attributeDecoration in AttributeDecorations)
      {
        attributeDecoration.AcceptVisitor(visitor);
      }

      if (TypeName!=null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      foreach (var formalParameter in FormalParameters)
      {
        formalParameter.AcceptVisitor(visitor);
      }

      if (GetAccessor!=null)
      {
        GetAccessor.AcceptVisitor(visitor);
      }

      if (SetAccessor!=null)
      {
        SetAccessor.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}