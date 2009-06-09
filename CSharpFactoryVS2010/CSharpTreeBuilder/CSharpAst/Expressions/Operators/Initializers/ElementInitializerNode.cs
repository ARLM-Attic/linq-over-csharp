// ================================================================================================
// ElementInitializerNode.cs
//
// Created: 2009.06.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class ElementInitializerNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ElementInitializerNode(Token start)
      : base(start)
    {
      ExpressionList = new InitializerExpressionNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression list.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public InitializerExpressionNodeCollection ExpressionList { get; private set; }
  }
}