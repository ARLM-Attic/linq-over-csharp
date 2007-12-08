using System.Windows.Forms;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This class represents a tree node that loads its children at the first 
  /// expansion of the node.
  /// </summary>
  // ==================================================================================
  internal abstract class OnDemandTreeNode : TreeNode
  {
    private bool _ChildrenLoaded;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a tree node with the specified name.
    /// </summary>
    /// <param name="text">Name of the tree node.</param>
    // --------------------------------------------------------------------------------
    public OnDemandTreeNode(string text)
      : this(text, true)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a tree node with the specified name and on demand behaviour.
    /// </summary>
    /// <param name="text">Name of the tree node.</param>
    /// <param name="onDemandExpand">
    /// True indicates on demnad behaviour. If false, loadeing children must be handled
    /// by the tree node itself.
    /// </param>
    // --------------------------------------------------------------------------------
    public OnDemandTreeNode(string text, bool onDemandExpand): base(text)
    {
      _ChildrenLoaded = !onDemandExpand;
      if (onDemandExpand) CreateEmptyNode();
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// This method can be called when this tree node is about to expand.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    public void ExpandNode()
    {
      if (!_ChildrenLoaded)
      {
        string oldText = Text;
        try
        {
          Text = "Loading...";
          TreeView.Update();
          OnExpandFirstTime();
        }
        finally
        {
          Text = oldText;
        }
      }
      _ChildrenLoaded = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that children are already loaded.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected void SignChildrenLoaded()
    {
      _ChildrenLoaded = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that this node does not have children.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected void SignNoChildren()
    {
      _ChildrenLoaded = true;
      Nodes.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Override this method to change how an empty child node is created.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected virtual void CreateEmptyNode()
    {
      Nodes.Add(new TreeNode("Loading..."));
      Collapse();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Override this method to load the children nodes.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected virtual void OnExpandFirstTime()
    {
      Nodes.Clear();
    }
  }
}
