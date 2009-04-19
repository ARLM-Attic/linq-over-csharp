// ================================================================================================
// CheckedOperatorNodes.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the base class for the checked and unchecked operators.
  /// </summary>
  // ================================================================================================
  public abstract class CheckedOperatorNodeBase : EmbeddedExpressionNode, IParentheses
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
  }

  // ================================================================================================
  /// <summary>
  /// This class represents the "checked" operator.
  /// </summary>
  // ================================================================================================
  public sealed class CheckedOperatorNode : CheckedOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public CheckedOperatorNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// This class represents the "unchecked" operator.
  /// </summary>
  // ================================================================================================
  public sealed class UncheckedOperatorNode : CheckedOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckedOperatorNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public UncheckedOperatorNode(Token start) : base(start)
    {
    }
  }
}