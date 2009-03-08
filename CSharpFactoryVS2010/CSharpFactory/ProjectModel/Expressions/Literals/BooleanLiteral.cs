using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a boolean literal.
  /// </summary>
  // ==================================================================================
  public abstract class BooleanLiteral : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new boolean literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected BooleanLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}