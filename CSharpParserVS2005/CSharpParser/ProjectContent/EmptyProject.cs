using CSharpParser.ProjectModel;

namespace CSharpParser.ProjectContent
{
  // ==================================================================================
  /// <summary>
  /// This type creates project content from files within a particular folder
  /// and its subfolder.
  /// </summary>
  /// <remarks>
  /// This project content manager type does not add referenced assemblies to the 
  /// project.
  /// </remarks>
  // ==================================================================================
  public class EmptyProject : IProjectContentProvider
  {
    #region Private fields

    private readonly string _WorkingFolder;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance using the specified working directory.
    /// </summary>
    /// <param name="workingFolder">Working directory.</param>
    /// <remarks>
    /// The project content is created from the .cs files in the working directory
    /// and its nested folders.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public EmptyProject(string workingFolder)
    {
      _WorkingFolder = workingFolder;
    }

    #endregion

    #region IProjectContentProvider implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _WorkingFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string WorkingFolder
    {
      get { return _WorkingFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the projekt information and fills up the compilation unit accordingly.
    /// </summary>
    /// <param name="compilationUnit">CompilationUnit to fill up.</param>
    // --------------------------------------------------------------------------------
    public virtual void CollectProjectItems(CompilationUnit compilationUnit)
    {
    }

    #endregion
  }
}