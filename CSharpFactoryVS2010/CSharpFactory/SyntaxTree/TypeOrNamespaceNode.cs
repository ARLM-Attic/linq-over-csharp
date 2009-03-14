// ================================================================================================
// TypeOrNamespaceNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
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
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    /// <value>The qualifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; private set; }

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
    public Token QualifierSeparatorToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type tags.
    /// </summary>
    /// <value>The type tags.</value>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<TypeTagNode> TypeTags { get; private set; }

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
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a type tag.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeTagNode:
  ///     identifier [TypeArgumentListNode]
  /// </remarks>
  // ================================================================================================
  public class TypeTagNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(Token start, TypeArgumentListNode argumentListNode)
      : base(start)
    {
      IdentifierToken = start;
      Arguments = argumentListNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    /// <value>The identifier name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Identifier { get { return IdentifierToken.val; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the node providing type arguments.
    /// </summary>
    /// <value>The arguments.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeArgumentListNode Arguments { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasTypeArguments
    {
      get { return Arguments != null && Arguments.Count > 0; }
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a type tag as a continuation ofa type tag list.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeTagContinuationNode:
  ///     "." TypeTagNode
  /// </remarks>
  // ================================================================================================
  public class TypeTagContinuationNode : TypeTagNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagContinuationNode(Token start, Token identifier, 
      TypeArgumentListNode argumentListNode)
      : base(start, argumentListNode)
    {
      if (identifier == null) throw new ArgumentNullException("identifier");
      SeparatorToken = start;
      IdentifierToken = identifier;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the separator token.
    /// </summary>
    /// <value>The separator token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }
  }
}