// ================================================================================================
// InvocationExpressionNode.cs
//
// Created: 2009.04.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method invocation operator node.
  /// </summary>
  // ================================================================================================
  public sealed class InvocationExpressionNode : PrimaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="InvocationExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="expressionNode">The primary expression node</param>
    // ----------------------------------------------------------------------------------------------
    public InvocationExpressionNode(Token start, ExpressionNode expressionNode)
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
      PrimaryExpression.ParentNode = this;

      Arguments = new ArgumentNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the primary expression part.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PrimaryExpressionNodeBase PrimaryExpression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments belonging to the invocation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNodeCollection Arguments { get; private set; }

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

      if (PrimaryExpression!=null)
      {
        PrimaryExpression.AcceptVisitor(visitor);
      }

      foreach (var argument in Arguments)
      {
        argument.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}