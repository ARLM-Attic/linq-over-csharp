// ================================================================================================
// StatementNodeExtensions.cs
//
// Created: 2009.06.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// This class contains extensions for the StatementNode class for testing purposes.
  /// </summary>
  // ================================================================================================
  public static class StatementNodeExtensions
  {
    public static string LeftmostName(this StatementNode stmtNode)
    {
      var expStm = stmtNode as ExpressionStatementNode;
      return expStm != null 
        ? expStm.Expression.LeftmostExpression().StartToken.Value 
        : stmtNode.StartToken.Value;
    }
  }
}