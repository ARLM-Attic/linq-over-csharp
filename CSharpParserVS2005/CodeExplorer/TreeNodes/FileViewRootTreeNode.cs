using CSharpParser.CodeExplorer.Entities;

namespace CSharpParser.CodeExplorer.TreeNodes
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
    private readonly FolderData _RootFolder;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder representing the root working folder of the compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FolderData RootFolder
    {
      get { return _RootFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new root node using the specified object type and attribute store.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FileViewRootTreeNode(FolderData rootFolder)
      : base("C# Project Files")
    {
      ImageKey = "CSharpProject";
      SelectedImageKey = ImageKey;
      _RootFolder = rootFolder;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();
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
  }
}