// ================================================================================================
// TypeOrNamespaceNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node describes a type reference.
  /// </summary>
  // ================================================================================================
  public class TypeNode : SyntaxNode<ISyntaxNode>
  {
    /// <summary>Backing field for TypeName property.</summary>
    private NamespaceOrTypeNameNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNode"/> class.
    /// </summary>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNode(Token identifier) : base(identifier)
    {
      RankSpecifiers = new RankSpecifierNodeCollection {ParentNode = this};
      PointerTokens = new ImmutableCollection<Token>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNode(Token separator, Token identifier)
      : this(identifier)
    {
      SeparatorToken = separator;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNode TypeName
    {
      get
      {
        return _TypeName;
      }

      internal set
      {
        if (value != null)
        {
          value.ParentNode = this;
        }
        _TypeName = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an empty type name. (used in unbound generic types)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return TypeName == null ? true : TypeName.IsEmpty; }
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
    /// Gets the array rank specifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RankSpecifierNodeCollection RankSpecifiers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type has array rank specifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsArray 
    {
      get { return RankSpecifiers.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the pointer tokens.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<Token> PointerTokens { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type has pointer specifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return PointerTokens.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of pointer specifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int PointerSpecifierCount
    {
      get { return PointerTokens.Count; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create type or namespace node from the specified token used for a simple type.
    /// </summary>
    /// <param name="t">The token for the simple type.</param>
    /// <returns>A new TypeNode</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeNode CreateTypeNode(Token t)
    {
      var result = new TypeNode(t);
      result.TypeName = new NamespaceOrTypeNameNode(t);
      result.TypeName.TypeTags.Add(new TypeTagNode(t, null));
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
    public static TypeNode CreateEmptyTypeNode(Token separatorToken)
    {
      return new TypeNode(separatorToken, null);
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
        TypeName
        );
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var result = new StringBuilder();

      if (TypeName != null)
      {
        result.Append(TypeName.ToString());
      }

      if (NullableToken!=null)
      {
        result.Append('?');
      }

      foreach (var pointerToken in PointerTokens)
      {
        result.Append('*');
      }

      foreach (var rankSpecifier in RankSpecifiers)
      {
        result.Append(rankSpecifier.ToString());
      }

      return result.Length == 0 ? GetType().ToString() : result.ToString();
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
      if (!visitor.Visit(this)) { return; }

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      foreach (var rankSpecifier in RankSpecifiers)
      {
        rankSpecifier.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
