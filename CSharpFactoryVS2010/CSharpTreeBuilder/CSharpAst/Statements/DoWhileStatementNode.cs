// ================================================================================================
// DoWhileStatementNode.cs
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
  public class DoWhileStatementNode : StatementNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DoWhileStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public DoWhileStatementNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the while token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token WhileToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition expression of this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Condition { get; internal set; }

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