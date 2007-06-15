using System;
using System.IO;
using CSharpParser.ParserFiles;
using CSharpParser.ProjectModel;

namespace CSharpParser
{
  // ==================================================================================
  /// <summary>
  /// This class is responsible for parsing all files in a C# project.
  /// </summary>
  // ==================================================================================
  public sealed class ProjectParser
  {
    #region Private Fields

    private ProjectFileCollection _Files = new ProjectFileCollection();
    private string _WorkingFolder = string.Empty;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    // --------------------------------------------------------------------------------
    public ProjectParser(string workingFolder): this(workingFolder, false)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    /// <param name="addCSharpFiles">
    /// If true, all C# files are added to the project.
    /// </param>
    // --------------------------------------------------------------------------------
    public ProjectParser(string workingFolder, bool addCSharpFiles)
    {
      _WorkingFolder = workingFolder;
      if (addCSharpFiles)
      {
        AddAllFilesFrom(_WorkingFolder);
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of files included in the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProjectFileCollection Files
    {
      get { return _Files; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a file to the project.
    /// </summary>
    /// <param name="fileName">File to add to the project.</param>
    // --------------------------------------------------------------------------------
    public void AddFile(string fileName)
    {
      // TODO: Check for duplication
      _Files.Add(new ProjectFile(Path.Combine(_WorkingFolder, fileName)));
    }

    public void AddAllFilesFrom(string path)
    {
      DirectoryInfo dir = new DirectoryInfo(path);
      // --- Add files in this folder
      foreach (FileInfo file in dir.GetFiles("*.cs"))
      {
        _Files.Add(new ProjectFile(file.FullName));
      }
      // --- Add files in subfolders
      foreach (DirectoryInfo subDir in dir.GetDirectories())
      {
        AddAllFilesFrom(subDir.FullName);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses all the files within the project.
    /// </summary>
    /// <returns>
    /// Number of errors.
    /// </returns>
    // --------------------------------------------------------------------------------
    public int Parse()
    {
      int errors = 0;
      foreach (ProjectFile file in _Files)
      {
        Console.WriteLine("Parsing file '{0}'", file.FullName);
        Scanner scanner = new Scanner(File.OpenText(file.FullName).BaseStream);
        Parser parser = new Parser(scanner);
        parser.Project = this;
        parser.File = file;
        parser.Parse();
        errors += parser.errors.count;
      }
      return errors;
    }

    #endregion
  }
}
