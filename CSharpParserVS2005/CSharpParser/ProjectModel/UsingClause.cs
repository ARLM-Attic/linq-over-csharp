using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a using clause that has been declared either in a
  /// file or in a namespace.
  /// </summary>
  // ==================================================================================
  public sealed class UsingClause: LanguageElement
  {
    #region Private fields

    private TypeReference _TypeUsed;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new using clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public UsingClause(Token token): base (token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the using clause has an alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasAlias
    {
      get { return Name.Length != 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type used by this directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference TypeUsed
    {
      get { return _TypeUsed; }
      set { _TypeUsed = value; }
    }

    #endregion

    #region Public methods

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of using clauses.
  /// </summary>
  // ==================================================================================
  public class UsingClauseCollection : RestrictedList<UsingClause>
  {
  }
}
