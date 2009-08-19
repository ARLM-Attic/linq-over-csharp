// ================================================================================================
// TypeTestingExpressionNode.cs
//
// Created: 2009.06.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type tesing expression node (as, is).
  /// </summary>
  // ================================================================================================
  public class TypeTestingExpressionNode : BinaryExpressionNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTestingExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="opType">The type of the typetesting operator.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTestingExpressionNode(Token start, TypeTestingOperator opType) : 
      base(start)
    {
      Operator = opType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the type testing operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeTestingOperator Operator { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the right operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode RightOperand { get; internal set; }

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

      if (LeftOperand != null)
      {
        LeftOperand.AcceptVisitor(visitor);
      }

      if (RightOperand != null)
      {
        RightOperand.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}