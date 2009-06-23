// ================================================================================================
// TryStatementNode.cs
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
  public class TryStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TryStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TryStatementNode(Token start)
      : base(start)
    {
      CatchClauses = new CatchClauseNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the try block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode TryBlock { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the catch clauses.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CatchClauseNodeCollection CatchClauses { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the finally token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token FinallyToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the finally block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode FinallyBlock { get; internal set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      base.AcceptVisitor(visitor);

      if (TryBlock != null)
      {
        TryBlock.AcceptVisitor(visitor);
      }

      foreach (var catchClause in CatchClauses)
      {
        catchClause.AcceptVisitor(visitor);
      }

      if (FinallyBlock != null)
      {
        FinallyBlock.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}