// ================================================================================================
// ExternAliasNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represnts an "extern alias" node.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>extern</strong>" '<strong>alias</strong>' <em>identifier</em>
  ///         "<strong>;</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>extern</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  ///             '<strong>alias</strong>': <see cref="AliasToken"/><br/>
  /// 			<em>identifier</em>: <see cref="IdentifierToken"/><br/>
  ///             "<strong>;</strong>": <see cref="ISyntaxNode.TerminatingToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class ExternAliasNode : SyntaxNode<NamespaceScopeNode>, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasNode"/> class.
    /// </summary>
    /// <param name="parent">The parent namespace scope node.</param>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNode(NamespaceScopeNode parent, Token start, Token alias, Token identifier,
                           Token terminating) : base(start)
    {
      ParentNode = parent;
      AliasToken = alias;
      IdentifierToken = identifier;
      Terminate(terminating);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token AliasToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.Value; }
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
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        AliasToken,
        MandatoryWhiteSpaceSegment.Default,
        IdentifierToken,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }
}