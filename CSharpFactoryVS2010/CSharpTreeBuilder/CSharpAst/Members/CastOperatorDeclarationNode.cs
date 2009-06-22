// ================================================================================================
// CastOperatorDeclarationNode.cs
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
  public class CastOperatorDeclarationNode: MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CastOperatorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public CastOperatorDeclarationNode(Token start) : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this operator is implicit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit 
    {
      get { return StartToken.Value == "implicit"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this operator is explicit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get { return StartToken.Value == "explicit"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "operator" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OperatorToken { get; internal set; }

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

      if (FormalParameters != null)
      {
        FormalParameters.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}