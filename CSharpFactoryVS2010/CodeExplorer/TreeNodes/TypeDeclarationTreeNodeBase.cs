using CSharpFactory.ProjectModel;
using CSharpFactory.Semantics;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This abstract class represents a C# type declaration scope that is either a source
  /// file or a namespace fragment.
  /// </summary>
  // ====================================================================================
  internal abstract class TypeDeclarationTreeNodeBase : OnDemandTreeNode
  {
    private readonly ITypeDeclarationScope _Scope;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object representing the scope.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public ITypeDeclarationScope Scope
    {
      get { return _Scope; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new tree node with the specified scope amnd name.
    /// </summary>
    /// <param name="scope">Scope to use</param>
    /// <param name="name">Name of the node</param>
    // ----------------------------------------------------------------------------------
    public TypeDeclarationTreeNodeBase(ITypeDeclarationScope scope, string name)
      : base(name)
    {
      _Scope = scope;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Loads the object types when the tree node is expanded at the first time.
    /// </summary>
    // ----------------------------------------------------------------------------------
    protected override void OnExpandFirstTime()
    {
      base.OnExpandFirstTime();
      foreach (NamespaceFragment subNamespace in Scope.NestedNamespaces)
      {
        Nodes.Add(new NamespaceFragmentTreeNode(subNamespace));
      }
      foreach (TypeDeclaration type in Scope.TypeDeclarations)
      {
        Nodes.Add(TypeTreeNode.CreateTypeNode(type));
      }
    }
  }
}
