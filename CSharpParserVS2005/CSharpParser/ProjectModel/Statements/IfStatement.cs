using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an "if" statement.
  /// </summary>
  // ==================================================================================
  public sealed class IfStatement : Statement
  {
    #region Private fields

    private BlockStatement _ThenStatements;
    private BlockStatement _ElseStatements;
    private Expression _Condition;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "if" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public IfStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the then branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ThenStatements
    {
      get { return _ThenStatements; }
      set { _ThenStatements = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the else branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ElseStatements
    {
      get { return _ElseStatements; }
      set { _ElseStatements = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if "else" branch is presented or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElse
    {
      get { return _ElseStatements != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition of the statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    #endregion
  }
}