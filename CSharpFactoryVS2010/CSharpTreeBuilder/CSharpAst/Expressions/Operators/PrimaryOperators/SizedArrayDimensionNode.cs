// ================================================================================================
// SizedArrayDimensionNode.cs
//
// Created: 2009.06.09, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represens a specifier for sized array dimensions.
  /// </summary>
  // ================================================================================================
  public class SizedArrayDimensionNode :
    SyntaxNodeCollection<ExpressionNode, ArrayCreationExpressionNode>
  {
  }
}