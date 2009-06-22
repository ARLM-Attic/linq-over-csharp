// ================================================================================================
// ConstructorDeclarationNode.cs
//
// Created: 2009.05.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a constructor declaration.
  /// </summary>
  // ================================================================================================
  public class ConstructorDeclarationNode : MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstructorDeclarationNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Colon { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Initializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has initializer.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has initializer; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return Initializer != null; }
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

      if (FormalParameters != null)
      {
        FormalParameters.AcceptVisitor(visitor);
      }

#warning Initializer visiting is missing.

    }

    #endregion
  }
}