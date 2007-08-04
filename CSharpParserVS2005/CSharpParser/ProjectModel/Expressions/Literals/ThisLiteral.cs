using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "this" literal.
  /// </summary>
  // ==================================================================================
  public sealed class ThisLiteral : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "this" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public ThisLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}