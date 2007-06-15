using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a cast operator declaration.
  /// </summary>
  // ==================================================================================
  public sealed class CastOperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private bool _IsExplicit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new cast operator declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public CastOperatorDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this is an explicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get { return _IsExplicit; }
      set { _IsExplicit = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is an implicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return !_IsExplicit; }
    }

    #endregion
  }
}
