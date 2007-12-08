using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This node represents a type declaration.
  /// </summary>
  // ====================================================================================
  internal class TypeTreeNode : 
    OnDemandTreeNode,
    IPropertyPanelSupport
  {
    private readonly TypeDeclaration _TypeDeclaration;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration represented by this node.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public TypeDeclaration TypeDeclaration
    {
      get { return _TypeDeclaration; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a tree node representing the specified type declaration.
    /// </summary>
    /// <param name="type">Type declaration represented by this node.</param>
    // ----------------------------------------------------------------------------------
    protected TypeTreeNode(TypeDeclaration type)
      : base(type.ParametrizedName)
    {
      _TypeDeclaration = type;
      if (type is ClassDeclaration) ImageKey = "Class";
      else if (type is StructDeclaration) ImageKey = "Struct";
      else if (type is InterfaceDeclaration) ImageKey = "Interface";
      else if (type is DelegateDeclaration) ImageKey = "Delegate";
      else ImageKey = "Enum";
      SelectedImageKey = ImageKey;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Factory method to create the tree node according to its associated type.
    /// </summary>
    /// <param name="type">Typeto create a node for.</param>
    /// <returns>Newlycreated type node.</returns>
    // ----------------------------------------------------------------------------------
    public static TypeTreeNode CreateTypeNode(TypeDeclaration type)
    {
      EnumDeclaration enumDecl = type as EnumDeclaration;
      if (enumDecl != null) return new EnumTreeNode(enumDecl);
      return new TypeTreeNode(type);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();
      foreach (TypeDeclaration type in TypeDeclaration.NestedTypes)
      {
        Nodes.Add(CreateTypeNode(type));
      }
      foreach (MemberDeclaration member in TypeDeclaration.Members)
      {
        Nodes.Add(new MemberTreeNode(member));
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
      return TypeDeclaration;
    }
  }
}
