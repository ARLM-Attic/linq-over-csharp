// ================================================================================================
// IntoClauseNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class IntoClauseNode : QueryBodyClauseNode, IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IntoClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public IntoClauseNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    /// <value>The identifier name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken.Value; }
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
    /// Gets or sets the query body.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QueryBodyNode QueryBody { get; internal set; }

  }
}