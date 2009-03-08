namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a lambda expression.
  /// </summary>
  // ==================================================================================
  public sealed class QueryBody
  {
    #region Private fields

    private readonly QueryBodyClauseCollection _Clauses = new QueryBodyClauseCollection();
    private SelectClause _Select;
    private GroupByClause _GroupBy;
    private string _IntoIdentifier;
    private QueryBody _ContinuationBody;

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the clauses of the query body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public QueryBodyClauseCollection Clauses
    {
      get { return _Clauses; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the select clause of this query body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SelectClause Select
    {
      get { return _Select; }
      internal set { _Select = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this body has grouping or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGrouping
    {
      get { return _GroupBy != null; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the group by clause of this query body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public GroupByClause GroupBy
    {
      get { return _GroupBy; }
      internal set { _GroupBy = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this body has a continuation or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasContinuation
    {
      get { return _ContinuationBody != null; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "into" identifier of this query body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string IntoIdentifier
    {
      get { return _IntoIdentifier; }
      set { _IntoIdentifier = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the continuation of this query body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public QueryBody ContinuationBody
    {
      get { return _ContinuationBody; }
      set { _ContinuationBody = value; }
    }

    #endregion
  }
}