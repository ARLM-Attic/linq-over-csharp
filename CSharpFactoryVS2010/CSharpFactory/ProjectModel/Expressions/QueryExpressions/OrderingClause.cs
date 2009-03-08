using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an ordering in the query expression's orderby clause.
  /// </summary>
  // ==================================================================================
  public sealed class OrderingClause : LanguageElement
  {
    #region Private fields

    private Expression _Expression;
    private bool _Ascending;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new ordering clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public OrderingClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
      _Ascending = true;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the ordering direction.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool Ascending
    {
      get { return _Ascending; }
      internal set { _Ascending = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression representing the basic set of the from clause. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      internal set { _Expression = value; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a collection ordering clause declarations.
  /// </summary>
  // ==================================================================================
  public sealed class OrderingClauseCollection : ImmutableCollection<OrderingClause>
  {
  }
}