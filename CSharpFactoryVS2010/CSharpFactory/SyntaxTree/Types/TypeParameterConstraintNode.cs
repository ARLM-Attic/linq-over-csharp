// ================================================================================================
// TypeParameterConstraintNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines a syntax node for a type parameter constraint.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   TypeParameterConstraintNode:
  ///     "where" identifier ":" ParameterConstaintNode { TypeParameterConstraintTagContinuationNode }
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
      ConstraintTags = new TypeParameterConstraintTagNodeCollection();
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
    public TypeParameterConstraintTagNodeCollection ConstraintTags { get; internal set; }
  }
}