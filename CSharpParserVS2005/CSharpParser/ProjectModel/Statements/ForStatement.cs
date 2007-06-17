using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "for" statement.
  /// </summary>
  // ==================================================================================
  public sealed class ForStatement : BlockStatement
  {
    #region Private fields

    private BlockStatement _InitializerBlock;
    private Expression _Condition;
    private BlockStatement _IteratorBlock;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "for" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ForStatement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the initializer block of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement InitializerBlock
    {
      get { return _InitializerBlock; }
      set { _InitializerBlock = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the condition expression of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Condition
    {
      get { return _Condition; }
      set { _Condition = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gest or sets the iterator block of the for statements.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement IteratorBlock
    {
      get { return _IteratorBlock; }
      set { _IteratorBlock = value; }
    }

    #endregion
  }
}