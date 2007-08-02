using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "break" statement.
  /// </summary>
  // ==================================================================================
  public sealed class BreakStatement : Statement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "break" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public BreakStatement(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }
  }
}