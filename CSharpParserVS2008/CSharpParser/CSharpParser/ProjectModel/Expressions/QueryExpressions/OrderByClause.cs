using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an orderby clause of a query expression.
  /// </summary>
  // ==================================================================================
  public sealed class OrderByClause : QueryBodyClause
  {
    #region Private fields

    private readonly OrderingClauseCollection _Orderings = 
      new OrderingClauseCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new orderby clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public OrderByClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the orderings belonging to the orderby clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public OrderingClauseCollection Orderings
    {
      get { return _Orderings; }
    }

    #endregion
  }
}