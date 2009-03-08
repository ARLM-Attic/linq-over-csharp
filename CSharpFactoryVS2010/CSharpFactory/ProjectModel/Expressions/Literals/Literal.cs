using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a literal expression.
  /// </summary>
  // ==================================================================================
  public abstract class Literal : Expression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new literal expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected Literal(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }
  }
}
