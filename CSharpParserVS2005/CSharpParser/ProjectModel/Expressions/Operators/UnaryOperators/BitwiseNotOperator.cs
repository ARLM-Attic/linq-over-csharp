using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
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
    // --------------------------------------------------------------------------------
    public BitwiseNotOperator(Token token)
      : base(token)
    {
    }
  }
}