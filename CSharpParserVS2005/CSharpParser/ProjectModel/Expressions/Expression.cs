using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an expression.
  /// </summary>
  // ==================================================================================
  public abstract class Expression : LanguageElement
  {
    #region Privat fields

    private bool _BracketsUsed;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected Expression(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating that expression was defined between brackets.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool BracketsUsed
    {
      get { return _BracketsUsed; }
      set { _BracketsUsed = value; }
    }

    #endregion
  }
}
