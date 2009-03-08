using CSharpFactory.ProjectModel;

namespace CSharpFactory.ProjectWriter
{
  // ==================================================================================
  /// <summary>
  /// This class is responsible for writing out a parsed and/or refactored compilation
  /// unit
  /// </summary>
  // ==================================================================================
  public sealed class CompilationUnitWriter
  {
    #region Private fields

    private readonly ProjectModel.CompilationUnit _CompilationUnit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationUnitWriter"/> class.
    /// </summary>
    /// <param name="compilationUnit">The compilation unit.</param>
    // --------------------------------------------------------------------------------
    public CompilationUnitWriter(ProjectModel.CompilationUnit compilationUnit)
    {
      _CompilationUnit = compilationUnit;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit.
    /// </summary>
    /// <value>The compilation unit.</value>
    // --------------------------------------------------------------------------------
    public CompilationUnit CompilationUnit
    {
      get { return _CompilationUnit; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Writes out the code represented by the compilation unit.
    /// </summary>
    /// <param name="rootFolder">The root folder of project files</param>
    /// <param name="options">Options determining how the code should be formatted</param>
    // --------------------------------------------------------------------------------
    public void WriteOutCode(string rootFolder, CodeFormattingOptions options)
    {
      
    }

    #endregion
  }
}
