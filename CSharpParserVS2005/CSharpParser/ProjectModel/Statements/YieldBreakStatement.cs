using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "yield break" statement.
  /// </summary>
  // ==================================================================================
  public sealed class YieldBreakStatement : Statement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "yield break" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public YieldBreakStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }
  }
}