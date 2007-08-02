using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "yield return" statement.
  /// </summary>
  // ==================================================================================
  public sealed class YieldReturnStatement : ReturnStatement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "yield return" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public YieldReturnStatement(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }
  }
}