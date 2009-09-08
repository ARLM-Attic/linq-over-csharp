// ================================================================================================
// YieldBreakStatementNode.cs
//
// Created: 2009.06.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a yield break statement.
  /// </summary>
  // ================================================================================================
  public class YieldBreakStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="YieldBreakStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="breakToken">The "break" token.</param>
    // ----------------------------------------------------------------------------------------------
    public YieldBreakStatementNode(Token start, Token breakToken)
      : base(start)
    {
      BreakToken = breakToken;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "break" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token BreakToken { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}