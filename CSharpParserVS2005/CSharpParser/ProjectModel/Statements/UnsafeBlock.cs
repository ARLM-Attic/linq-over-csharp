using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an "unsafe" block.
  /// </summary>
  // ==================================================================================
  public sealed class UnsafeBlock : BlockStatement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "unsafe" block declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public UnsafeBlock(Token token, IBlockOwner parentBlock)
      : base(token, parentBlock)
    {
    }
  }
}