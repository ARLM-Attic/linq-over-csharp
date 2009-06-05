// ================================================================================================
// ForStatementNode.cs
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
  public class ForStatementNode : StatementNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ForStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ForStatementNode(Token start)
      : base(start)
    {
      Iterators = new ForExpressionNodeCollection { ParentNode = this };
      Initializers = new ForExpressionNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of the "for" cycle.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableNode Initializer { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "for" iterators.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForExpressionNodeCollection Initializers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the init separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token InitSeparator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition expression of this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Condition { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ConditionSeparator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "for" iterators.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForExpressionNodeCollection Iterators { get; private set; }

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