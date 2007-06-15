using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a pointer operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class PointerOperator : UnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public PointerOperator(Token token)
      : base(token)
    {
    }
  }
}