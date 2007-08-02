using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "checked" block.
  /// </summary>
  // ==================================================================================
  public sealed class CheckedBlock : BlockStatement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "checked" block declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parent">Parent block of the statement.</param>
    // --------------------------------------------------------------------------------
    public CheckedBlock(Token token, IBlockOwner parent)
      : base(token, parent)
    {
    }
  }
}
