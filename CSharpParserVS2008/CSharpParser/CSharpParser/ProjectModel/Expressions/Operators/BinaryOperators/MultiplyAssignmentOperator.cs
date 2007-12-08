using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents an "*=" assignemnt operator
  /// </summary>
  // ==================================================================================
  public sealed class MultiplyAssignmentOperator : AssignmentOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public MultiplyAssignmentOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    // --------------------------------------------------------------------------------
    public MultiplyAssignmentOperator(Token token, Expression leftOperand) : 
      base(token, leftOperand)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator using the specified left and right operand.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="leftOperand">Left operand of the operator</param>
    /// <param name="rightOperand">Right operand of the operator</param>
    // --------------------------------------------------------------------------------
    public MultiplyAssignmentOperator(Token token, Expression leftOperand, Expression rightOperand)
      : base(token, leftOperand, rightOperand)
    {
    }
  }
}