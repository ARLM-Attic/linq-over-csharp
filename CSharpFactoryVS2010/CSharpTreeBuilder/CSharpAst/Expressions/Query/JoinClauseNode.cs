// ================================================================================================
// JoinClauseNode.cs
//
// Created: 2009.06.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents the "join" clause of a query expression.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///         '<strong>join</strong>' [ <em>TypeOrNamespaceNode</em> ]
  ///         <em>identifier</em> "<strong>in</strong>" <em>ExpressionNode</em>
  ///         '<strong>on</strong>' <em>ExpressionNode</em> '<strong>equals</strong>'
  ///         <em>ExpressionNode</em> 
  ///     </para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             '<strong>join</strong>': <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>TypeOrNamespaceNode</em>: <see cref="TypeName"/><br/>
  /// 			<em>identifier</em>: <see cref="IdentifierToken"/><br/>
  ///             "<strong>in</strong>": <see cref="InToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="InExpression"/><br/>
  ///             '<strong>on</strong>': <see cref="OnToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="OnExpression"/><br/>
  ///             '<strong>equals</strong>': <see cref="EqualsToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="EqualsExpression"/><br/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class JoinClauseNode : QueryBodyClauseNode, IIdentifierSupport
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public JoinClauseNode(Token start)
      : base(start)
    {
      _TypeName = TypeOrNamespaceNode.CreateEmptyTypeNode(null);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "in" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token InToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "in" expression of the "join" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode InExpression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "on" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OnToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "on" expression of the "join" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode OnExpression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "equals" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualsToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "in" expression of the "join" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode EqualsExpression { get; internal set; }

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

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
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