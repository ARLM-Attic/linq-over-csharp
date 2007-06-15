using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a property member declaration.
  /// </summary>
  // ==================================================================================
  public class PropertyDeclaration : MemberDeclaration
  {
    #region Private fields

    private AccessorDeclaration _Getter;
    private AccessorDeclaration _Setter;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new property member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public PropertyDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a getter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasGetter
    {
      get { return _Getter != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a setter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSetter
    {
      get { return _Setter != null; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "get" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Getter
    {
      get { return _Getter; }
      set { _Getter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "set" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Setter
    {
      get { return _Setter; }
      set { _Setter = value; }
    }

    #endregion
  }
}
