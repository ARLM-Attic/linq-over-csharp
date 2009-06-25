// ================================================================================================
// CheckedOperatorNodeBase.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the base class for the checked and unchecked operators.
  /// </summary>
  // ================================================================================================
  public abstract class CheckedOperatorNodeBase : PrimaryOperatorNode, IParentheses
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedOperatorNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected CheckedOperatorNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression between parentheses.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

    #region IParentheses Members

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
    public Token CloseParenthesis { get; internal set; }

    #endregion
  }
}