using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a join clause of a query expression.
  /// </summary>
  // ==================================================================================
  public sealed class JoinClause : QueryBodyClause
  {
    #region Private fields

    private TypeReference _Type;
    private Expression _InExpression;
    private Expression _OnExpression;
    private Expression _EqualsExpression;
    private string _IntoIdentifier;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new join clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public JoinClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the join clause variable, if explicitly defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      internal set { _Type = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the join caluse variable is explicitly typed or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsTyped
    {
      get { return _Type != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression representing the "in" part of the join clause. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression InExpression
    {
      get { return _InExpression; }
      internal set { _InExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression representing the "on" part of the join clause. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression OnExpression
    {
      get { return _OnExpression; }
      internal set { _OnExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression representing the "equals" part of the join clause. 
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression EqualsExpression
    {
      get { return _EqualsExpression; }
      internal set { _EqualsExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this join clause has an into part or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasInto
    {
      get { return _IntoIdentifier != null; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the "into" identifier.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string IntoIdentifier
    {
      get { return _IntoIdentifier; }
      internal set { _IntoIdentifier = value; }
    }

    #endregion
  }
}