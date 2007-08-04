using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a named literal.
  /// </summary>
  // ==================================================================================
  public sealed class NamedLiteral : BaseNamedLiteral
  {
    #region Private fields

    private bool _IsGlobal;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new named literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public NamedLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
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

    #endregion
  }
}