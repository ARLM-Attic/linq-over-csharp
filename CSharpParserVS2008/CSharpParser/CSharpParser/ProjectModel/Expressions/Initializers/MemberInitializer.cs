using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a member initializer.
  /// </summary>
  // ==================================================================================
  public sealed class MemberInitializer : Initializer
  {
    #region Private fields

    private readonly ExpressionInitializer _ValueInitializer;
    private readonly Initializer _ObjectInitializer;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new initializer instance with an expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="expr">Expression element creating this element</param>
    // --------------------------------------------------------------------------------
    public MemberInitializer(Token token, CSharpSyntaxParser parser, Expression expr)
      : base(token, parser)
    {
      _ValueInitializer = new ExpressionInitializer(token, parser, expr);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new initializer instance with an expression.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="init">Compound initializer</param>
    // --------------------------------------------------------------------------------
    public MemberInitializer(Token token, CSharpSyntaxParser parser, Initializer init)
      : base(token, parser)
    {
      _ObjectInitializer = init;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the initializer is a simple valueor not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueInitializer
    {
      get { return _ValueInitializer != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value initializer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ExpressionInitializer ValueInitializer
    {
      get { return _ValueInitializer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object initializer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer ObjectInitializer
    {
      get { return _ObjectInitializer; }
    }

    #endregion
  }
}