// ================================================================================================
// OperatorDeclarationNode.cs
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
  public class OperatorDeclarationNode : MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public OperatorDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OperatorToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the token representing the kind of the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token KindToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second token representing the kind of the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondKindToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the kind of the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public OverloadableOperatorType Kind { get; internal set; }

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

      if (FormalParameters!=null)
      {
        FormalParameters.AcceptVisitor(visitor);
      }

      if (Body != null)
      {
        Body.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}