// ================================================================================================
// LetClauseNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>This class represents a "let" clause of a query expression.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>'<strong>let</strong>' <em>identifier</em> "<strong>=</strong>"
  ///         <em>ExpressionNode</em></para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             '<strong>let</strong>': <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>identifier</em>: <see cref="IdentifierToken"/><br/>
  ///             "<strong>=</strong>": <see cref="EqualToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="Expression"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class LetClauseNode : QueryBodyClauseNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LetClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LetClauseNode(Token start)
      : base(start)
    {
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
    /// Gets or sets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the "let" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }
  }
}