using System.IO;
using CSharpFactory.CodeExplorer.Entities;
using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This type represents the root tree node of the file view.
  /// </summary>
  // ====================================================================================
  internal sealed class FileViewRootTreeNode : 
    OnDemandTreeNode,
    IPropertyPanelSupport
  {
    private readonly CompilationUnit _Unit;
    private readonly FolderData _RootFolder;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new root node using the specified object type and attribute store.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FileViewRootTreeNode(CompilationUnit unit)
      : base(unit.Name + " files")
    {
      ImageKey = "CSharpProject";
      SelectedImageKey = ImageKey;
      _Unit = unit;
      _RootFolder = BuildFolderHierarchy();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit of this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit Unit
    {
      get { return _Unit; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder representing the root working folder of the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FolderData RootFolder
    {
      get { return _RootFolder; }
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();

      // --- Add the list of referenced compilation units to the node
      foreach (ReferencedUnit refUnit in _Unit.ReferencedUnits)
      {
        ReferencedCompilation compilation = refUnit as ReferencedCompilation;
        if (compilation != null)
        {
          Nodes.Add(new FileViewRootTreeNode(compilation.CompilationUnit));
        }
      }

      // --- Add folder and file nodes
      foreach (FolderData folder in RootFolder.Folders)
      {
        Nodes.Add(new FolderTreeNode(folder));
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object providing information about this node.
    /// </summary>
    /// <returns>
    /// Information object
    /// </returns>
    // --------------------------------------------------------------------------------
    public object GetSelectedObject()
    {
      return RootFolder;
    }

    #region Private methods

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
      foreach (SourceFile file in _Unit.Files)
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
      if (folderName == Unit.WorkingFolder) return root;
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

    #endregion
  }
}