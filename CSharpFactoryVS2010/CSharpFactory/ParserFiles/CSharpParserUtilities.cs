// ================================================================================================
// CSharpParserUtilities.cs
//
// Created: 2009.04.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;

namespace CSharpFactory.ParserFiles
{
  // ================================================================================================
  /// <summary>
  /// This partial definition adds helper methods to the parser.
  /// </summary>
  // ================================================================================================
  public partial class CSharpSyntaxParser 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Binds left and right operands to a binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void BindBinaryOperator(BinaryOperator oper, BinaryOperatorNode opNode,
                                   Expression unExpr, ExpressionNode unaryNode,
                                   BinaryOperator rightExpr, BinaryOperatorNode rgNode)
    {
      if (rightExpr == null)
      {
        oper.RightOperand = unExpr;
      }
      else
      {
        oper.RightOperand = rightExpr;
        rightExpr.LeftOperand = unExpr;
      }
      oper.Terminate(t);
      // :::
      if (rgNode == null)
      {
        opNode.RightOperand = unaryNode;
      }
      else
      {
        opNode.RightOperand = rgNode;
        rgNode.LeftOperand = unaryNode;
      }
      opNode.Terminate(t);
    }
  }
}