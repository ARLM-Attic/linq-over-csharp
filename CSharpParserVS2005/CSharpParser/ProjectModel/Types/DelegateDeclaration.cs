using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a delegate type declaration.
  /// </summary>
  // ==================================================================================
  public sealed class DelegateDeclaration : TypeDeclaration
  {
    #region Private fields

    private TypeReference _ReturnType;
    private FormalParameterCollection _FormalParameters = new FormalParameterCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new delegate type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    public DelegateDeclaration(Token token, Parser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type returned by this delegate.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ReturnType
    {
      get { return _ReturnType; }
      set { _ReturnType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the formal parameters of this delegate.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterCollection FormalParameters
    {
      get { return _FormalParameters; }
    }

    #endregion
  }
}
