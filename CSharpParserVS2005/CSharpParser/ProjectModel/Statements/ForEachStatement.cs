using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "foreach" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForEachStatement : BlockStatement
  {
    #region Private fields

    private TypeReference _ItemType;
    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "foreach" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ForEachStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of foreach items.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ItemType
    {
      get { return _ItemType; }
      set { _ItemType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the container expression of foreach statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    #endregion
  }
}