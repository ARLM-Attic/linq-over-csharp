// ================================================================================================
// FolderContentProvider.cs
//
// Created: 2009.03.12, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;
using CSharpFactory.SolutionHierarchy;

namespace CSharpFactory.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a project provider which reads out project content from a folder. This
  /// provider cannot have information about referenced assemblies, thos information should be set
  /// when the source files are filled up.
  /// </summary>
  // ================================================================================================
  public class FolderContentProvider: ProjectProviderBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharp9ProjectContentProvider"/> class.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    // ----------------------------------------------------------------------------------------------
    public FolderContentProvider(string fileName)
    {
      Name = fileName;
      WorkingFolder = Path.GetDirectoryName(fileName);
      CollectProjectItems();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the project information and fills up the compilation unit accordingly.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void CollectProjectItems()
    {
      AddAllFilesFrom(WorkingFolder);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds all .cs file in the specified folder and its subfolders to this project.
    /// </summary>
    /// <param name="path">Path to search for source files.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddAllFilesFrom(string path)
    {
      var dir = new DirectoryInfo(path);
      
      // --- Add files in this folder
      foreach (var file in dir.GetFiles("*.cs"))
      {
        AddFile(file.FullName);
      }

      // --- Add files in subfolders
      foreach (var subDir in dir.GetDirectories())
      {
        AddAllFilesFrom(subDir.FullName);
      }
    }
  }
}