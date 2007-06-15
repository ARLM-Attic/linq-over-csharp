using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstact initializer.
  /// </summary>
  // ==================================================================================
  public abstract class Initializer : LanguageElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected Initializer(Token token)
      : base(token)
    {
    }
  }
}
