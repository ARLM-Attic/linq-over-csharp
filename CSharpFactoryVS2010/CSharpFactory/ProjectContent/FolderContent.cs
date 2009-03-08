using System.IO;
using CSharpFactory.ProjectModel;

namespace CSharpFactory.ProjectContent
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
  public class FolderContent : IProjectContentProvider
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
    public FolderContent(string workingFolder)
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
      AddAllFilesFrom(_WorkingFolder, compilationUnit);
    }

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds all .cs file in the specified folder and its subfolders to this project.
    /// </summary>
    /// <param name="path">Path to search for source files.</param>
    /// <param name="unit">Compilation unit to add the source files to.</param>
    // --------------------------------------------------------------------------------
    public void AddAllFilesFrom(string path, CompilationUnit unit)
    {
      DirectoryInfo dir = new DirectoryInfo(path);
      
      // --- Add files in this folder
      foreach (FileInfo file in dir.GetFiles("*.cs"))
      {
        unit.AddFileWithFullName(file.FullName);
      }

      // --- Add files in subfolders
      foreach (DirectoryInfo subDir in dir.GetDirectories())
      {
        AddAllFilesFrom(subDir.FullName, unit);
      }
    }

    #endregion
  }
}
