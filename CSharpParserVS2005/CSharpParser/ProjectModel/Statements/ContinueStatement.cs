using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "continue" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ContinueStatement : Statement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "continue" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ContinueStatement(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }
  }
}