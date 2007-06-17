using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a throw statement.
  /// </summary>
  // ==================================================================================
  public sealed class ThrowStatement : Statement
  {
    #region Private fields

    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new throw statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ThrowStatement(Token token)
      : base(token)
    {
    }

    #endregion 

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the label expression belonging to the throw statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    #endregion
  }
}