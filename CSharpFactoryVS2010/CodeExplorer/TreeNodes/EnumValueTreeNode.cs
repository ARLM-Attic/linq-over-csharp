using System.Windows.Forms;
using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This tree node represents an enumeration value definition.
  /// </summary>
  // ==================================================================================
  internal sealed class EnumValueTreeNode : TreeNode, IPropertyPanelSupport
  {
    private readonly EnumValueDeclaration _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value represented by this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public EnumValueDeclaration Value
    {
      get { return _Value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Cretaes a new tree node representing the specified value.
    /// </summary>
    /// <param name="value">Enumeration value to represent.</param>
    // --------------------------------------------------------------------------------
    public EnumValueTreeNode(EnumValueDeclaration value)
      : base(value.Name)
    {
      ImageKey = "EnumValue";
      SelectedImageKey = ImageKey;
      _Value = value;
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
      return Value;
    }
  }
}
