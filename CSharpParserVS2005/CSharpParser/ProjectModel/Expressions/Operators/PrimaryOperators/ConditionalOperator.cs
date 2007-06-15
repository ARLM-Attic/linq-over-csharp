using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a conditional "..? .. : .." operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class ConditionalOperator : PrimaryOperator
  {
    #region Private fields

    private Expression _Condition;
    private Expression _TrueExpression;
    private Expression _FalseExpression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ConditionalOperator(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the condition expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the true branch expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression TrueExpression
    {
      get { return _TrueExpression; }
      set { _TrueExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the false branch expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression FalseExpression
    {
      get { return _FalseExpression; }
      set { _FalseExpression = value; }
    }

    #endregion
  }
}