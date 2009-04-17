using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an abstract "base" literal.
  /// </summary>
  // ==================================================================================
  public class BaseLiteral : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "base" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public BaseLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}