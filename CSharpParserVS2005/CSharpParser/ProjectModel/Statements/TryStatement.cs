using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "try...catch...finally" statement.
  /// </summary>
  // ==================================================================================
  public sealed class TryStatement : Statement
  {
    #region Private fields

    private BlockStatement _TryBlock;
    private BlockStatement _FinallyBlock;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "try...catch...finally" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public TryStatement(Token token)
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
    public BlockStatement TryBlock
    {
      get { return _TryBlock; }
      set { _TryBlock = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the statements in the else branch.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement FinallyBlock
    {
      get { return _FinallyBlock; }
      set { _FinallyBlock = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if "else" branch is presented or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasFinally
    {
      get { return _FinallyBlock != null; }
    }

    #endregion
  }
}