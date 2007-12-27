using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a from clause of a query expression.
  /// </summary>
  // ==================================================================================
  public sealed class FromClause : QueryBodyClause
  {
    #region Private fields

    private TypeReference _Type;
    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new from clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public FromClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the from clause variable, if explicitly defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      internal set { _Type = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the from caluse variable is explicitly typed or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsTyped
    {
      get { return _Type != null; }  
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
}