// ================================================================================================
// ConditionalOperatorNode.cs
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
  public class ConditionalOperatorNode : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryOperatorNode"/> class.
    /// </summary>
    /// <param name="condition">Condition expression.</param>
    // ----------------------------------------------------------------------------------------------
    public ConditionalOperatorNode(ExpressionNode condition) : base(condition.StartToken)
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
  }
}