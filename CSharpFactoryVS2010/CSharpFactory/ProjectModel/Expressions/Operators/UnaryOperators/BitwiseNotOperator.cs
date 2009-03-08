using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a bitwise not operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class BitwiseNotOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public BitwiseNotOperator(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}