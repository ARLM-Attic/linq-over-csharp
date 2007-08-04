using CSharpParser.Collections;
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
    /// <param name="parser">Parser used by the comment</param>
    // --------------------------------------------------------------------------------
    public ExternalAlias(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of external aliases within a project file.
  /// </summary>
  // ==================================================================================
  public class ExternalAliasCollection : RestrictedCollection<ExternalAlias>
  {
  }
}
