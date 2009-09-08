// ================================================================================================
// AnonymousObjectCreationExpressionNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an anonymous object creation expression.
  /// </summary>
  // ================================================================================================
  public sealed class AnonymousObjectCreationExpressionNode : PrimaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AnonymousObjectCreationExpressionNode"/> class.
    /// </summary>
    /// <param name="start">The start.</param>
    // ----------------------------------------------------------------------------------------------
    public AnonymousObjectCreationExpressionNode(Token start)
      : base(start)
    {
      Declarators = new MemberDeclaratorNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the opening brace token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declarators of this initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MemberDeclaratorNodeCollection Declarators { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the optional orphan comma token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanComma { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the closing brace token.
    /// </summary>
    /// <value>The open brace.</value>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBrace { get; internal set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this))
      {
        return;
      }

      foreach (var declarator in Declarators)
      {
        declarator.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}