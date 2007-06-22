using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a named literal.
  /// </summary>
  // ==================================================================================
  public sealed class NamedLiteral : Literal
  {
    #region Private fields

    private bool _IsGlobal;
    private TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new named literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public NamedLiteral(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this name is global or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGlobal
    {
      get { return _IsGlobal; }
      set { _IsGlobal = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type arguments of the primitive method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReferenceCollection TypeArguments
    {
      get { return _TypeArguments; }
    }

    #endregion

  }
}