using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an operator expression.
  /// </summary>
  // ==================================================================================
  public abstract class OperatorExpression : Expression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected OperatorExpression(Token token, CSharpSyntaxParser parser) : 
      base(token, parser)
    {
    }
  }
}
