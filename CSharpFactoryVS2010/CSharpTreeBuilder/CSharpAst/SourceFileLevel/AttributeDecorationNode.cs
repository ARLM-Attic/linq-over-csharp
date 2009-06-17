// ================================================================================================
// AttributeDecorationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents a global or local argument decoration.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>[</strong>" [ <em>identifier</em> "<strong>:</strong>" ]
  ///         <em>AttributeNode</em> { "<strong>,</strong>" <em>AttributeNode</em> } [
  ///         "<strong>,</strong>" ] "<strong>]</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>[</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  ///             [<br/>
  /// 			<em>identifier</em>: <see cref="TargetToken"/><br/>
  ///             "<strong>:</strong>": Stored in the separator token of the next
  ///             <em>AttributeNode</em><br/>
  ///             ]<br/>
  /// 			<em>AttributeNode</em> { "<strong>,</strong>" <em>AttributeNode</em> }:
  ///             <see cref="Attributes"/><br/>
  ///             [ "<strong>,</strong>" ]: <see cref="OrphanSeparator"/><br/>
  ///             "<strong>]</strong>": <see cref="ISyntaxNode.TerminatingToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeDecorationNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeDecorationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNode(Token start)
      : base(start)
    {
      Attributes = new AttributeNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the target token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token TargetToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target information.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Target
    {
      get { return TargetToken == null ? string.Empty : TargetToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has explicit target.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasExplicitTarget { get { return TargetToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeNodeCollection Attributes { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional orphan separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanSeparator { get; internal set; }

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
        TargetToken,
        Attributes,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }
  }
}