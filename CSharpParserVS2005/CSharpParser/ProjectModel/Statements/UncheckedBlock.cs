using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an "unchecked" block.
  /// </summary>
  // ==================================================================================
  public sealed class UncheckedBlock : BlockStatement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "unchecked" block declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parent">Parent block of the statement.</param>
    // --------------------------------------------------------------------------------
    public UncheckedBlock(Token token, IBlockOwner parent)
      : base(token, parent)
    {
    }
  }
}