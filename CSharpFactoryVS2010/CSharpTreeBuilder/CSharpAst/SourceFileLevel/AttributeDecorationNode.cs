// ================================================================================================
// AttributeDecorationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

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
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.Value; }
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
}