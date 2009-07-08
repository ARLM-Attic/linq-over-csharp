// ================================================================================================
// TypeofExpressionNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the "typeof" expression.
  /// </summary>
  // ================================================================================================
  public sealed class TypeofExpressionNode : PrimaryExpressionWithEmbeddedTypeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeofExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeofExpressionNode(Token start) : base(start)
    {
    }
  }
}