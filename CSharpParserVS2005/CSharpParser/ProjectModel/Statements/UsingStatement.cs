using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "using" statement.
  /// </summary>
  // ==================================================================================
  public sealed class UsingStatement : BlockStatement
  {
    #region Private fields

    private Expression _ResourceExpression;
    private BlockStatement _ResourceDeclarations;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "using" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public UsingStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resource acquisition expression belonging to the "using".
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression ResourceExpression
    {
      get { return _ResourceExpression; }
      set { _ResourceExpression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resource declaration block belonging to the "using".
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ResourceDeclarations
    {
      get { return _ResourceDeclarations; }
      set { _ResourceDeclarations = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the "using" has a resource declaration block.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasResourceDeclaration
    {
      get { return _ResourceDeclarations != null; }
    }

    #endregion
  }
}