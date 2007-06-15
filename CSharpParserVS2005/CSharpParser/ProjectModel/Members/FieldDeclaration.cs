using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a field member declaration.
  /// </summary>
  // ==================================================================================
  public class FieldDeclaration : MemberDeclaration
  {
    #region Private fields

    private Initializer _Initializer;
    private bool _IsEvent;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new field member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FieldDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has an initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return _Initializer != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field is an event field or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEvent
    {
      get { return _IsEvent; }
      set { _IsEvent = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of this field.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer
    {
      get { return _Initializer; }
      set { _Initializer = value; }
    }

    #endregion
  }
}
