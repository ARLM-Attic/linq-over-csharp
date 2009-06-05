// ================================================================================================
// YieldReturnStatementNode.cs
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
  public class YieldReturnStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="YieldReturnStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expression">The expression belonging to the statement.</param>
    // ----------------------------------------------------------------------------------------------
    public YieldReturnStatementNode(Token start, ExpressionNode expression)
      : base(start)
    {
      Expression = expression;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; private set; }
  }
}