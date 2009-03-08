namespace CSharpFactory.CodeExplorer.TreeNodes
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
    /// <param name="elements">UI elements used by the controller</param>
    // --------------------------------------------------------------------------------
    public FileTreeController(ControlledUIelements elements):
      base(elements)
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
 	    return new FileViewRootTreeNode(CompilationUnit);
    }
  }
}
