using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a group by clause of a query expression.
  /// </summary>
  // ==================================================================================
  public sealed class GroupByClause : QueryBodyClause
  {
    #region Private fields

    private Expression _Expression;
    private Expression _ByExpression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new group by clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public GroupByClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression of the group by clause. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      internal set { _Expression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression of the group by clause used for grouping. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression ByExpression
    {
      get { return _ByExpression; }
      set { _ByExpression = value; }
    }

    #endregion
  }
}