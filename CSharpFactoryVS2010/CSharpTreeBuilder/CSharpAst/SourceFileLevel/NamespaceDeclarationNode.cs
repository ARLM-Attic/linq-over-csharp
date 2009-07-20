// ================================================================================================
// NamespaceDeclarationNode.cs
//
// Created: 2009.03.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node represents a namespace declaration.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>namespace</strong>" <em>identifier</em> { "<strong>.</strong>"
  ///         <em>identifier</em> } "<strong>{</strong>" { <em>ExternAliasNode</em> } {
  ///         <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }<br/>
  ///         { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }
  ///         "<strong>}</strong>" [ "<strong>;</strong>" ]</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>namespace</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>identifier</em> { "<strong>.</strong>" <em>identifier</em> }:
  ///             <see cref="NameTags"/><br/>
  ///             "<strong>{</strong>": <see cref="OpenBracket"/><br/>
  ///             { <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>ExternAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }: see
  ///             <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>TypeDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             "<strong>}</strong>": <see cref="CloseBracket"/><br/>
  ///             "<strong>;</strong>": <see cref="ISyntaxNode.TerminatingToken"/> (if
  ///             not defined, <see cref="CloseBracket"/> is the terminating token)
  ///         </para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class NamespaceDeclarationNode : NamespaceScopeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceDeclarationNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceDeclarationNode(NamespaceScopeNode parent, Token start) : base(parent, start)
    {
      NameTags = new NameTagNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the nametags decribing the full name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NameTagNodeCollection NameTags { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing the opening bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBracket { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing the closing bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBracket { get; internal set; }

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
      var result = new OutputSegment(
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        NameTags,
        BraceSegment.OpenType(OpenBracket),
        base.GetOutputSegment(),
        BraceSegment.CloseType(CloseBracket)
        );
      if (TerminatingToken != CloseBracket)
        result.Append(TerminatingToken);
      return result;
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

      // Visit extern alias nodes
      foreach (var externAliasNode in ExternAliasNodes)
      {
        externAliasNode.AcceptVisitor(visitor);
      }

      // Visit using nodes
      foreach (var usingNode in UsingNodes)
      {
        usingNode.AcceptVisitor(visitor);
      }

      // Visit in-scope declaration nodes (namespaces and types)
      foreach (var inScopeDeclaration in InScopeDeclarations)
      {
        inScopeDeclaration.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}