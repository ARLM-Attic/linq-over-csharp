using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a lambda expression.
  /// </summary>
  // ==================================================================================
  public sealed class QueryExpression : Expression
  {
    #region Private fields

    private FromClause _From;
    private QueryBody _Body = new QueryBody();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new query expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public QueryExpression(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the starting from clause of this query expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FromClause From
    {
      get { return _From; }
      internal set { _From = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body part of the query
    /// </summary>
    // --------------------------------------------------------------------------------
    public QueryBody Body
    {
      get { return _Body; }
    }

    #endregion
  }
}