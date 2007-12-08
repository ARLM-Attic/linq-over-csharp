using CSharpParser.CodeExplorer.Entities;
using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This tree node type represents a source file folder in the source tree.
  /// </summary>
  // ====================================================================================
  internal sealed class FolderTreeNode : 
    OnDemandTreeNode,
    IPropertyPanelSupport
  {
    private readonly FolderData _Folder;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder represented by this tree node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FolderData Folder
    {
      get { return _Folder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new root node using the specified object type and attribute store.
    /// </summary>
    /// <param name="folder">Folder information.</param>
    // --------------------------------------------------------------------------------
    public FolderTreeNode(FolderData folder)
      : base(folder.FolderName)
    {
      _Folder = folder;
      ImageKey = "FolderClosed";
      SelectedImageKey = "FolderOpen";
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();
      foreach (FolderData subFolder in Folder.Folders)
      {
        Nodes.Add(new FolderTreeNode(subFolder));
      }
      foreach (SourceFile file in Folder.Files)
      {
        Nodes.Add(new FileTreeNode(file));
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
      return Folder;
    }
  }
}
