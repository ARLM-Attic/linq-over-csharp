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
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public TrueLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}