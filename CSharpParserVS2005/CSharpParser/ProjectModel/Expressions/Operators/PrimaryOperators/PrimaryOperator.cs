using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a primary operator
  /// </summary>
  /// <remark>
  /// A primary operator does not have a traditional operand.
  /// </remark>
  // ==================================================================================
  public abstract class PrimaryOperator : OperatorExpression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected PrimaryOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}