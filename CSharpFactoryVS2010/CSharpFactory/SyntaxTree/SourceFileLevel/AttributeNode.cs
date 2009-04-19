// ================================================================================================
// AttributeNode.cs
//
// Created: 2009.03.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node represents a global or local argument decoration.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeDecorationNode:
  ///     "[" [ identifier ":" ] AttributeNode { AttributeContinuationNode } [ "," ] "]"
  /// 
  /// The openining bracket is represented by StartToken, the closing bracket with
  /// TerminatingToken.

  /// </remarks>
  // ================================================================================================
  public sealed class AttributeDecorationNode : SyntaxNode, IIdentifierSupport
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
      Attributes = new ImmutableCollection<AttributeNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.val; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has an identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier { get { return IdentifierToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<AttributeNode> Attributes { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional closing separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ClosingSeparator { get; internal set; }
  }

  // ================================================================================================
  /// <summary>
  /// This node represents an attribute with its arguments.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeNode: 
  ///     TypeOrNamespaceNode 
  ///       [ "(" [ AttributeArgumentNode ] { AttributeArgumentContinuationNode }  ")" ]
  /// </remarks>
  // ================================================================================================
  public class AttributeNode : SyntaxNode , IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeNode(Token start)
      : base(start)
    {
      Arguments = new ImmutableCollection<AttributeArgumentNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the attribute.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get { return TerminatingToken; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this attribute defines arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool DefinesArguments
    {
      get { return OpenParenthesis != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the arguments belonging to this attribute.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<AttributeArgumentNode> Arguments { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasArguments
    {
      get { return DefinesArguments && Arguments != null && Arguments.Count > 0; }
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents an attribute continuation node.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeContinuationNode:
  ///     "," AttributeNode
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeContinuationNode : AttributeNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="baseNode">The base node to take the attribute parameters from.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeContinuationNode(Token separator, AttributeNode baseNode)
      : base(separator)
    {
      SeparatorToken = separator;
      TypeName = baseNode.TypeName;
      Arguments = baseNode.Arguments;
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
  /// This class represents an argument of an attribute.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeArgumentNode:
  ///     [ identifier "=" ] ExpressionNode
  /// </remarks>
  // ================================================================================================
  public class AttributeArgumentNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeArgumentNode"/> class.
    /// </summary>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="equal">The equal token.</param>
    /// <param name="expression">The expression node.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeArgumentNode(Token identifier, Token equal, ExpressionNode expression) : 
      base(identifier)
    {
      IdentifierToken = identifier;
      EqualToken = equal;
      Expression = expression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier string.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? String.Empty : IdentifierToken.val; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has an identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier { get { return IdentifierToken != null; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression node of this argument.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents the continuation of an attribute argument.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   AttributeArgumentContinuationNode:
  ///     "," AttributeArgumentNode
  /// </remarks>
  // ================================================================================================
  public sealed class AttributeArgumentContinuationNode : AttributeArgumentNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeArgumentContinuationNode"/> class.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="equal">The equal token.</param>
    /// <param name="expression">The expression node.</param>
    // ----------------------------------------------------------------------------------------------
    public AttributeArgumentContinuationNode(Token separator, Token identifier, Token equal, 
      ExpressionNode expression) : base(identifier, equal, expression)
    {
      StartToken = separator;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of attribute decoration nodes.
  /// </summary>
  // ================================================================================================
  public sealed class AttributeDecorationNodeCollection : 
    ImmutableCollection<AttributeDecorationNode>
  {
  }
}