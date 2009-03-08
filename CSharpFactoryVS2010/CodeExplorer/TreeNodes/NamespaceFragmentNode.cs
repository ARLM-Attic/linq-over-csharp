using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This node represents a namespace fragment.
  /// </summary>
  // ====================================================================================
  internal sealed class NamespaceFragmentTreeNode : 
    TypeDeclarationTreeNodeBase,
    IPropertyPanelSupport
  {
    private readonly ProjectModel.NamespaceFragment _NamespaceFragment;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace fragment represented by this node.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public ProjectModel.NamespaceFragment NamespaceFragment
    {
      get { return _NamespaceFragment; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace fragment node with the specified namespace instance.
    /// </summary>
    /// <param name="nsFragment">Namespace fragment instance.</param>
    // ----------------------------------------------------------------------------------
    public NamespaceFragmentTreeNode(NamespaceFragment nsFragment)
      : base(nsFragment, nsFragment.Name)
    {
      _NamespaceFragment = nsFragment;
      ImageKey = "Namespace";
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
      return NamespaceFragment;
    }
  }
}
