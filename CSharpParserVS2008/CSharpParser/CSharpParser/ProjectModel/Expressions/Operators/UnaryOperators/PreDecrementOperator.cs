using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a predecrement operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class PreDecrementOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public PreDecrementOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}