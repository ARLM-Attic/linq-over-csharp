// ================================================================================================
// UsingAliasNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using clause with an alias.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>using</strong>" <em>alias</em> "<strong>=</strong>"
  ///         <em>TypeOrNamespaceNode</em> "<strong>;</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>using</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>alias</em>: <see cref="AliasToken"/><br/>
  ///             "<strong>=</strong>": <see cref="EqualToken"/><br/>
  /// 			<em>TypeOrNamespaceNode</em>: <see cref="UsingNamespaceNode.TypeName"/><br/>
  ///             "<strong>;</strong>": <see cref="UsingNamespaceNode.TerminatingToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class UsingAliasNode : UsingNamespaceNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingAliasNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingAliasNode(NamespaceScopeNode parent, Token start, Token alias, Token equalToken,
                              TypeOrNamespaceNode typeName, Token terminating)
      : base(parent, start, typeName, terminating)
    {
      AliasToken = alias;
      EqualToken = equalToken;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new blank <see cref="UsingAliasNode"/> object 
    /// with no parent, no alias and empty namespace name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public UsingAliasNode()
      : this(null, Token.Using, null, Token.Equal, null, Token.Semicolon)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the alias token.
    /// </summary>
    /// <value>The alias token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token AliasToken { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias.
    /// </summary>
    /// <value>The alias.</value>
    // ----------------------------------------------------------------------------------------------
    public string Alias
    {
      get { return AliasToken.Value; }
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
        SpaceAroundSegment.AssignmentOp(EqualToken),
        TypeName,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }
}