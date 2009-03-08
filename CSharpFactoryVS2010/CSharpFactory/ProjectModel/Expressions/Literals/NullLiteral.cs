using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "null" literal.
  /// </summary>
  // ==================================================================================
  public sealed class NullLiteral : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "null" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public NullLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}