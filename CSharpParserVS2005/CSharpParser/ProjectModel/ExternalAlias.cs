using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents an external alias declaration belonging to a file or to a
  /// namespace.
  /// </summary>
  // ==================================================================================
  public sealed class ExternalAlias : LanguageElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new external alias declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ExternalAlias(Token token)
      : base(token)
    {
    }
  }
}
