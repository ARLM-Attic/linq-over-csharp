using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a member declarator node of type "ident = Expression".
  /// </summary>
  // ================================================================================================
  public class IdentifierMemberDeclaratorNode : MemberDeclaratorNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierMemberDeclaratorNode"/> class.
    /// </summary>
    /// <param name="identifierToken">Identifier token.</param>
    /// <param name="equalToken">Equal sign (=) token.</param>
    /// <param name="expressionNode">An expression node.</param>
    // ----------------------------------------------------------------------------------------------
    public IdentifierMemberDeclaratorNode(Token identifierToken, Token equalToken, ExpressionNode expressionNode)
      : base(identifierToken)
    {
      if (identifierToken==null)
      {
        throw new ArgumentNullException("identifierToken");
      }

      if (equalToken == null)
      {
        throw new ArgumentNullException("equalToken");
      }

      if (expressionNode == null)
      {
        throw new ArgumentNullException("expressionNode");
      }

      IdentifierToken = identifierToken;
      EqualToken = equalToken;
      Expression = expressionNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the equal token.
    /// </summary>
    /// <value>The equal token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression.
    /// </summary>
    /// <value>The expression.</value>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }

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

      if (Expression != null)
      {
        Expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}