// ================================================================================================
// EnumDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents an enum declaration.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>enum</strong>" <em>identifier</em> [ "<strong>:</strong>"
  ///         <em>TypeOrNamespaceNode</em> ] "<strong>{</strong>" [ <em>EnumValueNode</em> {
  ///         "<strong>,</strong>" <em>EnumValueNode</em> } [ "<strong>,</strong>" ] ]
  ///         "<strong>}</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>enum</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>identifier: <see cref="TypeOrMemberDeclarationNode.IdentifierToken"/></em><br/>
  ///             "<strong>:</strong>": <see cref="TypeDeclarationNode.ColonToken"/><br/>
  /// 			<em>TypeOrNamespaceNode:</em>
  /// 			<see cref="TypeDeclarationNode.BaseTypes"/><br/>
  ///             "<strong>{</strong>": <see cref="TypeWithBodyDeclarationNode.OpenBrace"/><br/>
  ///             { <em>EnumValueNode</em> }: <see cref="Values"/><br/>
  ///             "<strong>,</strong>": <see cref="OrphanSeparator"/><br/>
  ///             "<strong>}</strong>": <see cref="ISyntaxNode.TerminatingToken"/>,
  ///             <see cref="TypeWithBodyDeclarationNode.CloseBrace"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class EnumDeclarationNode : TypeWithBodyDeclarationNode
  {
    // --- Backing fields
    private TypeOrNamespaceNode _EnumBase;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumDeclarationNode(Token start, Token name)
      : base(start, name)
    {
      Values = new EnumValueNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional enum base type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode EnumBase
    {
      get { return _EnumBase; }
      internal set
      {
        _EnumBase = value;
        if (_EnumBase != null) _EnumBase.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of enumeration values.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public EnumValueNodeCollection Values { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the orphan separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanSeparator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segments representing the type declaration body.
    /// </summary>
    /// <returns>
    /// Output segments representing the type declaration body.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override OutputSegment GetBodySegments()
    {
      return new OutputSegment(
        Values,
        OrphanSeparator
        );
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

      if (EnumBase != null)
      {
        EnumBase.AcceptVisitor(visitor);
      }

      foreach (var value in Values)
      {
        value.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}