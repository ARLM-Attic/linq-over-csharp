// ================================================================================================
// ElementInitializerNode.cs
//
// Created: 2009.06.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using System;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents an element initializer node.
  /// </summary>
  // ================================================================================================
  public class ElementInitializerNode : SyntaxNode<ISyntaxNode>
  {
    // Backing field for NonAssignmentExpression
    private ExpressionNode _NonAssignmentExpression;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ElementInitializerNode(Token start)
      : base(start)
    {
      ExpressionList = new ExpressionNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the non-assigment expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode NonAssignmentExpression
    {
      get { return _NonAssignmentExpression; }

      internal set
      {
        if (value is AssignmentExpressionNode)
        {
          throw new ArgumentException("Only non-assignment expression is allowed as an element initializer.");
        }

        _NonAssignmentExpression = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression list.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNodeCollection ExpressionList { get; private set; }
  }
}