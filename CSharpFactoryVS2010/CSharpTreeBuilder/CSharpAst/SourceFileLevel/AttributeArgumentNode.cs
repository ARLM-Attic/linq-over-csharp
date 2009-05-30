// ================================================================================================
// AttributeArgumentNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
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
  public class AttributeArgumentNode : SyntaxNode<AttributeNode>, IIdentifierSupport
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
      Terminate(Expression.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the equal token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token EqualToken { get; private set; }

    /// <summary>
    /// Gets the expression node of this argument.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }

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
      get { return IdentifierToken == null ? String.Empty : IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has an identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier
    {
      get { return IdentifierToken != null; }
    }
  }
}