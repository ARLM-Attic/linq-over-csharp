using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "const" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ConstStatement : ValueAssignmentStatement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "const" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ConstStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }
  }
}
