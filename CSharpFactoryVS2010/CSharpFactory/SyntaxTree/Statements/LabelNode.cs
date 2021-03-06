// ================================================================================================
// LabelNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This node describes a label before a statement.
  /// </summary>
  // ================================================================================================
  public sealed class LabelNode : SyntaxNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelNode"/> class.
    /// </summary>
    /// <param name="identifier">Label identifier token.</param>
    /// <param name="colon">The colon token of the label.</param>
    // ----------------------------------------------------------------------------------------------
    public LabelNode(Token identifier, Token colon)
      : base(identifier)
    {
      IdentifierToken = identifier;
      ColonToken = colon;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.val; }
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
    /// Gets or sets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; private set; }
  }
}