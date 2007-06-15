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
    // --------------------------------------------------------------------------------
    public EmptyStatement(Token token)
      : base(token)
    {
    }
  }
}
