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
    // Backing field for QualifierToken property
    private Token _QualifierToken;

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
    /// Gets or sets the qualifier token.
    /// </summary>
    /// <value>The qualifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken
    {
      get
      {
        return _QualifierToken;
      }
      set
      {
        _QualifierToken = value;
        SetSeparatorTokens();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier string.
    /// </summary>
    /// <value>The qualifier string.</value>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? null : QualifierToken.Value; }
    }

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
      get { return QualifierToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an empty type name. (used in unbound generic types)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return TypeTags == null || TypeTags.Count == 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type tags.
    /// </summary>
    /// <value>The type tags.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNodeCollection TypeTags { get; protected set; }

#warning TypeTags collection should not be publicly modified, only through AddTypeTag method, otherwise separatot tokens are not guaranteed to be set correctly

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type tag to the TypeTags collection, and sets the separator tokens.
    /// </summary>
    /// <param name="typeTagNode">A type tag.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeTag(TypeTagNode typeTagNode)
    {
      TypeTags.Add(null, typeTagNode);
      SetSeparatorTokens();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the separator tokens to null, double-colon or dot, 
    /// according to the number of type tags, and whether a qualifier exists.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void SetSeparatorTokens()
    {
      for (int i = 0; i < TypeTags.Count; i++)
      {
        if (i == 0)
        {
          TypeTags[i].SeparatorToken = (HasQualifier ? Token.DoubleColon : null);
        }
        else
        {
          TypeTags[i].SeparatorToken = Token.Dot;
        }
      }
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
    /// Create type or namespace node from the specified token used for a simple type.
    /// </summary>
    /// <param name="t">The token for the simple type.</param>
    /// <returns>A new TypeOrNamespaceNode</returns>
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
    /// Gets a special TypeOrNamespaceNode that represents a missing type name. Used for unbound types.
    /// </summary>
    /// <param name="separatorToken">Optional separator token.</param>
    /// <returns>A new TypeOrNamespaceNode with null identifier token.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeOrNamespaceNode CreateEmptyTypeNode(Token separatorToken)
    {
      return new TypeOrNamespaceNode(separatorToken, null);
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
        SeparatorToken,
        SeparatorToken.IsComma() ? SpaceAfterSegment.AfterComma() : null,
        QualifierToken,
        TypeTags
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
            
      foreach (var tag in TypeTags)
      {
        tag.AcceptVisitor(visitor);
      }

      foreach (var modifier in TypeModifiers)
      {
        modifier.AcceptVisitor(visitor);
      }

#warning Seems like ArrayModifierNodes are missing!
    }

    #endregion
  }
}
