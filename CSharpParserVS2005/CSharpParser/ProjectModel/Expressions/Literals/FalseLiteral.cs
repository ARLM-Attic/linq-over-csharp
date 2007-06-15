using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "false" literal.
  /// </summary>
  // ==================================================================================
  public sealed class FalseLiteral : BooleanLiteral
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "false" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FalseLiteral(Token token)
      : base(token)
    {
    }
  }
}