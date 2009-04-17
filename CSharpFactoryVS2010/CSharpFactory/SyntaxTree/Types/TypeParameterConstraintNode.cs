// ================================================================================================
// TypeParameterConstraintNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class defines a syntax node for a type parameter constraint.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeParameterConstraintNode:
  ///     "where" identifier ":" ParameterConstaintNode { ParameterConstraintTagContinuationNode }
  /// </remarks>
  // ================================================================================================
  public sealed class TypeParameterConstraintNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterConstraintNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="colon">The colon token.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintNode(Token start, Token identifier, Token colon)
      : base(start)
    {
      IdentifierToken = identifier;
      ColonToken = colon;
      ConstraintTags = new ParameterConstraintTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter name token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.val;}
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
    /// Gets the ":" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the constraint tags.
    /// </summary>
    /// <value>The constraint tags.</value>
    // ----------------------------------------------------------------------------------------------
    public ParameterConstraintTagNodeCollection ConstraintTags { get; internal set; }
  }

  // ================================================================================================
  /// <summary>
  /// Defines a node for a parameter constraint
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ParameterConstraintTagNode:
  ///     "class" | "struct" | "new" "(" ")" | TypeOrNamespaceNode
  /// </remarks>
  // ================================================================================================
  public class ParameterConstraintTagNode : SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterConstraintTagNode(Token start)
      : base(start)
    {
      ConstraintToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="openPar">The opening parenthesis.</param>
    /// <param name="closePar">The closing parenthesis.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterConstraintTagNode(Token start, Token openPar, Token closePar)
      : base(start)
    {
      ConstraintToken = start;
      OpenParenthesis = openPar;
      CloseParenthesis = closePar;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterConstraintTagNode"/> class.
    /// </summary>
    /// <param name="typeNode">The type node.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterConstraintTagNode(TypeOrNamespaceNode typeNode) : 
      base(typeNode.StartToken)
    {
      ConstraintToken = StartToken;
      TypeName = typeNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constraint token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ConstraintToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "class" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "class" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsClass { get { return ConstraintToken.val == "class"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "struct" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "struct" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsStruct { get { return ConstraintToken.val == "struct"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a "new" constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is "new" constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsNew { get { return ConstraintToken.val == "new"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a type name constraint tag.
    /// </summary>
    /// <value><c>true</c> if this instance is type name constraint tag; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsTypeName { get { return TypeName != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constarint type name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; protected set; }
  }

  // ================================================================================================
  /// <summary>
  /// Defines a continuation for a parameter constraint tag node.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ParameterConstraintTagContinuationNode:
  ///     "," ParameterConstraintTagNode
  /// </remarks>
  // ================================================================================================
  public sealed class ParameterConstraintTagContinuationNode : ParameterConstraintTagNode,
    IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterConstraintTagContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="tag">The tag to clone properties from.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterConstraintTagContinuationNode(Token separator, ParameterConstraintTagNode tag)
      : base(separator)
    {
      SeparatorToken = separator;
      ConstraintToken = tag.ConstraintToken;
      OpenParenthesis = tag.OpenParenthesis;
      CloseParenthesis = tag.CloseParenthesis;
      TypeName = tag.TypeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }   
  }

  // ================================================================================================
  /// <summary>
  /// Defines a collection of ParameterConstraintTagNode instances.
  /// </summary>
  // ================================================================================================
  public sealed class ParameterConstraintTagNodeCollection : 
    ImmutableCollection<ParameterConstraintTagNode>
  {
  }

  // ================================================================================================
  /// <summary>
  /// Defines a collection of TypeParameterConstraint instances.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterConstraintNodeCollection : ImmutableCollection<TypeParameterConstraintNode>
  {
  }
}