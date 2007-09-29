using System.Windows.Forms;
using CSharpParser.ProjectModel;
using CSharpParser.CodeExplorer.Entities;
using System.IO;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This class represents a the controller that handles the file tree view of the
  /// CodeExplorer.
  /// </summary>
  // ==================================================================================
  internal sealed class FileTreeController: TreeControllerBase
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of the controller and attaches it to the specifier 
    /// TreeView and PropertyGrid
    /// </summary>
    /// <param name="treeView">Tree view of the controller.</param>
    /// <param name="grid">Property grid of the controller</param>
    // --------------------------------------------------------------------------------
    public FileTreeController(TreeView treeView, PropertyGrid grid):
      base(treeView, grid)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates the root node for this controller containing the file and folder
    /// hierarchy.
    /// </summary>
    /// <returns>Tree node created.</returns>
    // --------------------------------------------------------------------------------
    protected override OnDemandTreeNode CreateRootNode()
    {
 	    return new FileViewRootTreeNode(BuildFolderHierarchy());
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Build the folder and file hierarchy according the source files within the
    /// compliation unit.
    /// </summary>
    /// <returns></returns>
    // --------------------------------------------------------------------------------
    private FolderData BuildFolderHierarchy()
    {
      FolderData rootFolder = new FolderData("Root Project Folder");
      foreach (SourceFile file in CompilationUnit.Files)
      {
        FolderData folder = EnsureFolder(rootFolder, file.Folder);
        folder.Files.Add(file);
      }
      return rootFolder;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains or creates a tree node representing the specified folder.
    /// </summary>
    /// <param name="root">Root folder node</param>
    /// <param name="folderName">Folder to obtain/create.</param>
    /// <returns>
    /// Node represnting the folder.
    /// </returns>
    // --------------------------------------------------------------------------------
    private FolderData EnsureFolder(FolderData root, string folderName)
    {
      if (folderName == WorkingFolder) return root;
      string parentFolder = Path.GetDirectoryName(folderName);
      string subFolder = Path.GetFileName(folderName);

      FolderData parent = EnsureFolder(root, parentFolder);
      FolderData childFound = null;
      foreach (FolderData child in parent.Folders)
      {
        if (child.FolderName == subFolder)
        {
          childFound = child;
          break;
        }
      }
      if (childFound != null) return childFound;
      FolderData newFolder = new FolderData(subFolder);
      parent.Folders.Add(newFolder);
      return newFolder;
    }
  }
}
