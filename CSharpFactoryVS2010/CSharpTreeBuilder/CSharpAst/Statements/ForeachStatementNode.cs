// ================================================================================================
// ForeachStatementNode.cs
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
  public class ForeachStatementNode : StatementNode, IIdentifierSupport, IParentheses
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ForeachStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ForeachStatementNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

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
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName == null) _TypeName.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the init separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token InToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the Collection expression of this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Collection { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statement to be executed while condition is true.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public StatementNode Statement { get; internal set; }
  }
}