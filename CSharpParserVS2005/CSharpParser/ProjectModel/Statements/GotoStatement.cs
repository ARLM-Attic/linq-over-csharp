using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a goto statement.
  /// </summary>
  // ==================================================================================
  public sealed class GotoStatement : Statement
  {
    #region Private fields

    private Expression _LabelExpression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public GotoStatement(Token token)
      : base(token)
    {
    }

    #endregion 

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the label expression belonging to the goto statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression LabelExpression
    {
      get { return _LabelExpression; }
      set { _LabelExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a simple label goto.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSimpleLabel
    {
      get { return _LabelExpression == null && Name != "default";  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is "goto default".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsDefaultLabel
    {
      get { return _LabelExpression == null && Name == "default"; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a "goto case xxx".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsCaseLabel
    {
      get { return _LabelExpression != null; }
    }

    #endregion
  }
}