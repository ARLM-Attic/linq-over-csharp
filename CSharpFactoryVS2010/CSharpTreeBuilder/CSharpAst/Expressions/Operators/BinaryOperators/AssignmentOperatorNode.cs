// ================================================================================================
// AssignmentOperatorNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class defines an assignment ("=") operator node.
  /// </summary>
  // ================================================================================================
  public class AssignmentOperatorNode : BinaryOperatorNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignmentOperatorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="second">The second operator token.</param>
    /// <param name="opType">Type of the operator.</param>
    // ----------------------------------------------------------------------------------------------
    public AssignmentOperatorNode(Token start, Token second, AssignmentOperatorType opType)
      : base(start)
    {
      SecondToken = second;
      if (second != null) Terminate(second);
      Operator = opType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AssignmentOperatorType Operator { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second operator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SecondToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the right operand of the binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode RightOperand { get; internal set; }
  }
}