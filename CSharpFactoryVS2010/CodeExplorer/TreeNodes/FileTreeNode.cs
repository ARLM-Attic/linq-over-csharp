using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This class represents a C# source file node.
  /// </summary>
  // ====================================================================================
  internal sealed class FileTreeNode : 
    TypeDeclarationTreeNodeBase,
    IPropertyPanelSupport
  {
    private readonly SourceFile _File;
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file represented by this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile File
    {
      get { return _File; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new tree node representing the specified source file.
    /// </summary>
    /// <param name="file">Source file instance.</param>
    // --------------------------------------------------------------------------------
    public FileTreeNode(SourceFile file) : base(file, file.Name)
    {
      _File = file;
      ImageKey = "CSharpFile";
      SelectedImageKey = ImageKey;
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
      return File;
    }
  }
}
