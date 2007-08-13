namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a reference to a compiation unit.
  /// </summary>
  // ==================================================================================
  public sealed class CompilationUnitReference : CompilationReference
  {
    #region Private fields

    private readonly CompilationUnit _CompilationUnit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the specified compilation unit and name.
    /// </summary>
    /// <param name="unit">Compilation unit instance</param>
    /// <param name="name">Name</param>
    // --------------------------------------------------------------------------------
    public CompilationUnitReference(CompilationUnit unit, string name)
      : base(name)
    {
      _CompilationUnit = unit;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit referenced by this instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit CompilationUnit
    {
      get { return _CompilationUnit; }
    }

    #endregion
  }
}