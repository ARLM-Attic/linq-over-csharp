using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an abstract "base" literal.
  /// </summary>
  // ==================================================================================
  public abstract class BaseLiteral : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "base" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected BaseLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}