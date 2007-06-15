using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "true" literal.
  /// </summary>
  // ==================================================================================
  public sealed class TrueLiteral : BooleanLiteral
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "true" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public TrueLiteral(Token token)
      : base(token)
    {
    }
  }
}