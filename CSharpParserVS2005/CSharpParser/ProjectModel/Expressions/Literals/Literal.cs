using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
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
    // --------------------------------------------------------------------------------
    protected Literal(Token token)
      : base(token)
    {
      Name = token.val;
    }
  }
}
