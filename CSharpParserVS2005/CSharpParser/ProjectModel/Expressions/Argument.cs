using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an argument of a method call.
  /// </summary>
  // ==================================================================================
  public sealed class Argument : LanguageElement
  {
    #region Private fields

    private FormalParameterKind _Kind;
    private Expression _Expression;
    
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new argument.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public Argument(Token token)
      : base(token)
    {
      _Kind = FormalParameterKind.In;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the kind of the argument.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterKind Kind
    {
      get { return _Kind; }
      set { _Kind = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the argument expression.
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
