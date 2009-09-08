// ================================================================================================
// ConditionalExpressionNode.cs
//
// Created: 2009.05.01, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a conditional operator node.
  /// </summary>
  // ================================================================================================
  public class ConditionalExpressionNode : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalExpressionNode"/> class.
    /// </summary>
    /// <param name="condition">Condition expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ConditionalExpressionNode(ExpressionNode condition) : base(condition.StartToken)
    {
      Condition = condition;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Condition { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression when condition is true.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode TrueExpression { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression when condition is false.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode FalseExpression { get; internal set; }

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

      if (Condition!=null)
      {
        Condition.AcceptVisitor(visitor);
      }

      if (TrueExpression != null)
      {
        TrueExpression.AcceptVisitor(visitor);
      }

      if (FalseExpression != null)
      {
        FalseExpression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}