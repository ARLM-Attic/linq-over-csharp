// ================================================================================================
// ElementAccessNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an element access node.
  /// </summary>
  // ================================================================================================
  public sealed class ElementAccessNode : PrimaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expressionNode">The primary expression node</param>
    // ----------------------------------------------------------------------------------------------
    public ElementAccessNode(Token start, ExpressionNode expressionNode)
      : base(start)
    {
      if (expressionNode == null)
      {
        throw new ArgumentNullException("expressionNode");
      }

      if (!(expressionNode is PrimaryExpressionNodeBase))
      {
        throw new ArgumentException("Primary expression expected");
      }

      PrimaryExpression = expressionNode as PrimaryExpressionNodeBase;
      
      Expressions = new ExpressionNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the primary expression part of the element access expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PrimaryExpressionNodeBase PrimaryExpression { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression collection.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNodeCollection Expressions { get; internal set; }

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

      if (PrimaryExpression != null)
      {
        PrimaryExpression.AcceptVisitor(visitor);
      }

      foreach (var expression in Expressions)
      {
        expression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}