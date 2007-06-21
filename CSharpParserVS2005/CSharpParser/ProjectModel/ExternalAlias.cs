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
    // --------------------------------------------------------------------------------
    public ExternalAlias(Token token)
      : base(token)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of external aliases within a project file.
  /// </summary>
  // ==================================================================================
  public class ExternalAliasCollection : RestrictedList<ExternalAlias>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of external aliases.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ExternalAliasCollection()
    {
    }

    #endregion
  }
}
