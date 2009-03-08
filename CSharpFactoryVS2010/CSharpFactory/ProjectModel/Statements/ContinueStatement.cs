using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
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
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ContinueStatement(Token token,CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }
  }
}