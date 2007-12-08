using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a unary plus operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class UnaryPlusOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public UnaryPlusOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}
