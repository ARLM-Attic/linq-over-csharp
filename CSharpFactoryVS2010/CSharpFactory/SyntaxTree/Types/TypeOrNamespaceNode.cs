// ================================================================================================
// TypeOrNamespaceNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node describes a type or namespace reference.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeOrNamespaceNode:
  ///     [Qualifier "::"] TypeTagNode { TypeTagContinuationNode }
  /// </remarks>
  // ================================================================================================
  public class TypeOrNamespaceNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode(Token start) : base(start)
    {
      TypeTags = new ImmutableCollection<TypeTagNode>();
      TypeModifiers = new ImmutableCollection<TypeModifierNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNode"/> class.
    /// </summary>
    /// <param name="qualifier">The qualifier.</param>
    /// <param name="separator">The separator.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode(Token qualifier, Token separator)
      : base(qualifier)
    {
      QualifierToken = qualifier;
      QualifierSeparatorToken = separator;
      TypeTags = new ImmutableCollection<TypeTagNode>();
      TypeModifiers = new ImmutableCollection<TypeModifierNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    /// <value>The qualifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has qualifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has qualifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasQualifier { get { return QualifierToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier.
    /// </summary>
    /// <value>The qualifier.</value>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? string.Empty : QualifierToken.val; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierSeparatorToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type tags.
    /// </summary>
    /// <value>The type tags.</value>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<TypeTagNode> TypeTags { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the type tag.
    /// </summary>
    /// <param name="tag">The tag.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeTag(TypeTagNode tag)
    {
      TypeTags.Add(tag);
    }

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
      result.AddTypeTag(new TypeTagNode(t, null));
      result.Terminate(t);
      return result;
    }

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
    public bool IsNullable { get { return NullableToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type modifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<TypeModifierNode> TypeModifiers { get; private set; }

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
      return HasQualifier
        ? new OutputSegment(QualifierToken, QualifierSeparatorToken)
        : new OutputSegment(TypeTags);
    }
  }
}