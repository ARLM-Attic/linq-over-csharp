using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This node represents a namespace fragment.
  /// </summary>
  // ====================================================================================
  internal sealed class NamespaceTreeNode :
    OnDemandTreeNode,
    IPropertyPanelSupport
  {
    private readonly CompilationUnit _Unit;
    private readonly Namespace _Namespace;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace fragment represented by this node.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Namespace Namespace
    {
      get { return _Namespace; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace node with the specified namespace instance.
    /// </summary>
    /// <param name="ns">Namespace instance.</param>
    // ----------------------------------------------------------------------------------
    public NamespaceTreeNode(Namespace ns, CompilationUnit unit)
      : base(ns.Name)
    {
      _Namespace = ns;
      _Unit = unit;
      ImageKey = "Namespace";
      SelectedImageKey = ImageKey;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // --------------------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();
      List<TypeDeclaration> topLevelTypes = new List<TypeDeclaration>();
      foreach (TypeDeclaration type in _Unit.DeclaredTypes)
      {
        if (!type.IsNested && type.EnclosingNamespace != null &&
        type.EnclosingNamespace.FullName == Namespace.Name)
        {
          topLevelTypes.Add(type);
        }
      }

      foreach (TypeDeclaration type in 
        new SortableCollection<TypeDeclaration>(topLevelTypes).SortBy("Name"))
      {
        Nodes.Add(TypeTreeNode.CreateTypeNode(type));
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
      return Namespace;
    }
  }
}
