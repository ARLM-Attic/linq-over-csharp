// ================================================================================================
// QueryBodyNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents the body of a query expression.
  /// <code>
  /// query-body:
  ///   query-body-clauses-opt select-or-group-clause query-continuation-opt
  /// </code>
  /// </summary>
  // ================================================================================================
  public class QueryBodyNode : SyntaxNode<QueryExpressionNode>
  {
    // Backing fields for properties.
    private SelectClauseNode _SelectClause;
    private GroupClauseNode _GroupClause;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBodyNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public QueryBodyNode(Token start)
      : base(start)
    {
      BodyClauses = new QueryBodyClauseNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the query body clauses.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QueryBodyClauseNodeCollection BodyClauses { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the select clause node. Only one of select or group clause can be set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SelectClauseNode SelectClause
    {
      get { return _SelectClause; }

      internal set
      {
        if (GroupClause != null)
        {
          throw new InvalidOperationException("Select clause cannot be set, because group clause is already set.");
        }
        _SelectClause = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the group clause node. Only one of select or group clause can be set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public GroupClauseNode GroupClause
    {
      get { return _GroupClause; }

      internal set
      {
        if (SelectClause != null)
        {
          throw new InvalidOperationException("Group clause cannot be set, because select clause is already set.");
        }
        _GroupClause = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional query continuation (aka into clause) node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QueryContinuationNode QueryContinuation { get; internal set; }

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

      foreach (var bodyClause in BodyClauses)
      {
        bodyClause.AcceptVisitor(visitor);
      }

      if (SelectClause != null)
      {
        SelectClause.AcceptVisitor(visitor);
      }

      if (GroupClause != null)
      {
        GroupClause.AcceptVisitor(visitor);
      }

      if (QueryContinuation != null)
      {
        QueryContinuation.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}