// ================================================================================================
// MemberDeclaratorNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member declarator node.
  /// </summary>
  // ================================================================================================
  public class MemberDeclaratorNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------

    #region DeclaratorKind enum

    /// <summary>
    /// Kind of member declarator
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public enum DeclaratorKind
    {
      /// <summary>
      /// An "ident = expression" form is used.
      /// </summary>
      Expression,

      /// <summary>
      /// A simple name is used.
      /// </summary>
      SimpleName,

      /// <summary>
      /// A member access is used.
      /// </summary>
      MemberAccess
    }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDeclaratorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberDeclaratorNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of declarator used.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclaratorKind Kind { get; internal set; }

    // ----------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    /// <value>The equal token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression (in case of "Expression" and "SimpleName" declarators).
    /// </summary>
    /// <value>The expression.</value>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the dot separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token DotSeparator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type (in case of "MemberAccess" declarator).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    #region IIdentifierSupport Members

    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    /// <value>The identifier name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier
    {
      get { return IdentifierToken != null; }
    }

    #endregion
  }
}