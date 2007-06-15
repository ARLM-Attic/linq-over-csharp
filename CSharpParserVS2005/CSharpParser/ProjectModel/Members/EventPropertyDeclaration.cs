using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an event property member declaration.
  /// </summary>
  // ==================================================================================
  public class EventPropertyDeclaration : MemberDeclaration
  {
    #region Private fields

    private AccessorDeclaration _Adder;
    private AccessorDeclaration _Remover;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new event property member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public EventPropertyDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has an adder or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasAdder
    {
      get { return _Adder != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a remover or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasRemover
    {
      get { return _Remover != null; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "get" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Adder
    {
      get { return _Adder; }
      set { _Adder = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "set" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Remover
    {
      get { return _Remover; }
      set { _Remover = value; }
    }

    #endregion
  }
}