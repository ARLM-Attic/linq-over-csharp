using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstract statement.
  /// </summary>
  // ==================================================================================
  public abstract class Statement : LanguageElement
  {
    #region Private fields

    private string _Label;
    private BlockStatement _ParentBlock;

    #endregion

    #region Lifecycle methods 

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected Statement(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the label of this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Label
    {
      get { return _Label; }
      set { _Label = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent block of this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public BlockStatement ParentBlock
    {
      get { return _ParentBlock; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this statemenet has a parent or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParent
    {
      get { return _ParentBlock != null; }
    }

    #endregion

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the parent block of this statement.
    /// </summary>
    /// <param name="parent">Parent block.</param>
    // --------------------------------------------------------------------------------
    public void SetParentBlock(BlockStatement parent)
    {
      _ParentBlock = parent;
    }
  }
}
