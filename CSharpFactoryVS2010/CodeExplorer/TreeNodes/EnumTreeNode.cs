using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ==================================================================================
  /// <summary>
  /// This tree node represents an enumerated type definition.
  /// </summary>
  // ==================================================================================
  internal sealed class EnumTreeNode : TypeTreeNode
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new tree node representing the specified enum declaration.
    /// </summary>
    /// <param name="enumDecl">Enum declaration.</param>
    // --------------------------------------------------------------------------------
    public EnumTreeNode(EnumDeclaration enumDecl)
      : base(enumDecl)
    {
      ImageKey = "Enum";
      SelectedImageKey = ImageKey;
      SignNoChildren();
      foreach (EnumValueDeclaration valueDecl in enumDecl.Values)
      {
        Nodes.Add(new EnumValueTreeNode(valueDecl));
      }
    }
  }
}
