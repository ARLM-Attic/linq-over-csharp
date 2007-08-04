using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an epmty statement (contains only a ";").
  /// </summary>
  // ==================================================================================
  public sealed class EmptyStatement : Statement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public EmptyStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }
  }
}
