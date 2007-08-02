using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
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
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public ConstStatement(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }
  }
}
