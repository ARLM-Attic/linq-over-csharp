using System.Windows.Forms;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This class represents a the controller that handles the file tree view of the
  /// CodeExplorer.
  /// </summary>
  // ==================================================================================
  internal sealed class NamespaceTreeController: TreeControllerBase
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of the controller and attaches it to the specifier 
    /// TreeView and PropertyGrid
    /// </summary>
    /// <param name="treeView">Tree view of the controller.</param>
    /// <param name="grid">Property grid of the controller</param>
    // --------------------------------------------------------------------------------
    public NamespaceTreeController(TreeView treeView, PropertyGrid grid)
      : base(treeView, grid)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates the root node of this controller.
    /// </summary>
    /// <returns>Tree node created.</returns>
    // --------------------------------------------------------------------------------
    protected override OnDemandTreeNode CreateRootNode()
    {
      return new NamespaceViewRootTreeNode(CompilationUnit);
    }
  }
}
