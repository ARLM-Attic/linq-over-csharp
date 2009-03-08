using System.Collections.Generic;
using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.Entities
{
  // ==================================================================================
  /// <summary>
  /// This class represents folder information for tree nodes representing a folder.
  /// </summary>
  // ==================================================================================
  internal class FolderData
  {
    private readonly string _FolderName;
    private readonly List<FolderData> _Folders = new List<FolderData>();
    private readonly List<SourceFile> _Files = new List<SourceFile>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the folder.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FolderName 
    {
      get { return _FolderName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the subfolders of this folder.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<FolderData> Folders
    {
      get { return _Folders; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets source files in this folder.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<SourceFile> Files
    {
      get { return _Files; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new folder information instance.
    /// </summary>
    /// <param name="folderName">Folder name to display</param>
    // --------------------------------------------------------------------------------
    public FolderData(string folderName)
    {
      _FolderName = folderName;
    }
  }
}
