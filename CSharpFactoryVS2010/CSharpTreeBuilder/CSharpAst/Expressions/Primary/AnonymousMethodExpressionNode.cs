// ================================================================================================
// AnonymousMethodExpressionNode.cs
//
// Created: 2009.05.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an anonymous method expression.
  /// </summary>
  // ================================================================================================
  public sealed class AnonymousMethodExpressionNode : PrimaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AnonymousMethodExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AnonymousMethodExpressionNode(Token start)
      : base(start)
    {
      FormalParameters = new FormalParameterNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parameter list.
    /// </summary>
    /// <value>The parameter list.</value>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNodeCollection FormalParameters { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the body of the anonymous method.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BlockStatementNode Body { get; internal set; }
  }
}