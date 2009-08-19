using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents the "join-into" clause of a query expression.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>'<strong>join</strong>' [ <em>TypeOrNamespaceNode</em> ]
  ///         <em>identifier</em> "<strong>in</strong>" <em>ExpressionNode</em>
  ///         '<strong>on</strong>' <em>ExpressionNode</em> '<strong>equals</strong>'
  ///         <em>ExpressionNode</em> '<strong>into</strong>' <em>intoidentifier</em>
  ///     </para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///       '<strong>join</strong>': <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>TypeOrNamespaceNode</em>: <see cref="JoinClauseNode.Type"/><br/>
  /// 			<em>identifier</em>: <see cref="JoinClauseNode.IdentifierToken"/><br/>
  ///             "<strong>in</strong>": <see cref="JoinClauseNode.InToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="JoinClauseNode.InExpression"/><br/>
  ///             '<strong>on</strong>': <see cref="JoinClauseNode.OnToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="JoinClauseNode.OnExpression"/><br/>
  ///             '<strong>equals</strong>': <see cref="JoinClauseNode.EqualsToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="JoinClauseNode.EqualsExpression"/><br/>
  ///             '<strong>into</strong>': <see cref="IntoToken"/><br/>
  /// 			<em>intoidentifier</em>: <see cref="IntoIdentifierToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class JoinIntoClauseNode : JoinClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinIntoClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public JoinIntoClauseNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "into" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IntoToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "into" identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IntoIdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the "into" identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string IntoIdentifier
    {
      get { return IntoIdentifierToken.Value; }
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

      if (Type != null)
      {
        Type.AcceptVisitor(visitor);
      }

      if (InExpression != null)
      {
        InExpression.AcceptVisitor(visitor);
      }

      if (OnExpression != null)
      {
        OnExpression.AcceptVisitor(visitor);
      }

      if (EqualsExpression != null)
      {
        EqualsExpression.AcceptVisitor(visitor);
      }
    }

    #endregion

  }
}