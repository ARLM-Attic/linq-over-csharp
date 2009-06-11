// ================================================================================================
// TypeOrNamespaceNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node describes a type or namespace reference.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>[ <em>qualifier</em> "<strong>::</strong>"] <em>TypeTagNode</em> {
  ///         "<strong>.</strong>" <em>TypeTagNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  /// 			<em>qualifier</em>: an item in <see cref="TypeTags"/><br/>
  ///             "<strong>::</strong>" <em>TypeTagNode</em>: an item in <see cref="TypeTags"/><br/>
  ///             "<strong>.</strong>" <em>TypeTagNode</em>: an item in <see cref="TypeTags"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeOrNamespaceNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode(Token identifier) : base(identifier)
    {
      TypeTags = new TypeTagNodeCollection { ParentNode = this };
      TypeModifiers = new TypeModifierNodeCollection { ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode(Token separator, Token identifier)
      : this(identifier)
    {
      SeparatorToken = separator;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty <see cref="TypeOrNamespaceNode"/> object with no token yet.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode()
      : this(null)
    {}

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has qualifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has qualifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasQualifier
    {
      get { return TypeTags.Count >= 2 && TypeTags[1].SeparatorToken.Value == "::"; }
    }

    // ---------------------------8-------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    /// <value>The qualifier.</value>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return HasQualifier ? TypeTags[1].Identifier : string.Empty; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type tags.
    /// </summary>
    /// <value>The type tags.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNodeCollection TypeTags { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the nullable token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token NullableToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is nullable.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is nullable; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsNullable
    {
      get { return NullableToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type modifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeModifierNodeCollection TypeModifiers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create type of namespace node from the specified token used for a simple type.
    /// </summary>
    /// <param name="t">The token for the simple type.</param>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode CreateTypeNode(Token t)
    {
      var result = new TypeOrNamespaceNode(t);
      result.TypeTags.Add(new TypeTagNode(t, null));
      result.Terminate(t);
      return result;
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
        TypeTags
        );
    }
  }
}