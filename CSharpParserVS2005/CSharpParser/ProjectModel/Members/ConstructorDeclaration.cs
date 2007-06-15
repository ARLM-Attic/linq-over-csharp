using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a constructor declaration.
  /// </summary>
  // ==================================================================================
  public sealed class ConstructorDeclaration : MethodDeclaration
  {
    #region Private fields

    private bool _HasBase;
    private bool _HasThis;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructor declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ConstructorDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constructor has the "base" initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBase
    {
      get { return _HasBase; }
      set { _HasBase = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constructor has the "this" initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasThis
    {
      get { return _HasThis; }
      set { _HasThis = value; }
    }

    #endregion
  }
}
