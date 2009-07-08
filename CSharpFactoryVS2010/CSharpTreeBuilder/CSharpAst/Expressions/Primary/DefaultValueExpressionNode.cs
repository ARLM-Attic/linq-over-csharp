// ================================================================================================
// DefaultValueExpressionNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a default value expression.
  /// </summary>
  // ================================================================================================
  public sealed class DefaultValueExpressionNode : PrimaryExpressionWithEmbeddedTypeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValueExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public DefaultValueExpressionNode(Token start)
      : base(start)
    {
    }

  }
}