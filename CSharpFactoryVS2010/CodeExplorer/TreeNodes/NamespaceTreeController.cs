using System.Windows.Forms;

namespace CSharpFactory.CodeExplorer.TreeNodes
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
    /// <param name="elements">UI elements used by the controller</param>
    // --------------------------------------------------------------------------------
    public NamespaceTreeController(ControlledUIelements elements)
      : base(elements)
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
