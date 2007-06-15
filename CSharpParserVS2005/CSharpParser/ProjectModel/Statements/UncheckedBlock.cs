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
    // --------------------------------------------------------------------------------
    public UncheckedBlock(Token token)
      : base(token)
    {
    }
  }
}