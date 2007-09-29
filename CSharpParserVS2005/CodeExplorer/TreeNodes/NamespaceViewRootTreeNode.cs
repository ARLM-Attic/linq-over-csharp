﻿using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This type represents the root tree node of the file view.
  /// </summary>
  // ====================================================================================
  internal sealed class NamespaceViewRootTreeNode :
    OnDemandTreeNode
  {
    private readonly CompilationUnit _Unit;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new root node using the specified object type and attribute store.
    /// </summary>
    /// <param name="unit">
    /// Compilation unit represented by this tree node.
    /// </param>
    // --------------------------------------------------------------------------------
    public NamespaceViewRootTreeNode(CompilationUnit unit)
      : base("C# Project Files")
    {
      _Unit = unit;
      ImageKey = "CSharpProject";
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
      foreach (Namespace ns in 
        new SortableCollection<Namespace>(_Unit.DeclaredNamespaces).SortBy("Name"))
      {
        Nodes.Add(new NamespaceTreeNode(ns, _Unit));
      }

      List<TypeDeclaration> globalTypes = new List<TypeDeclaration>();
      foreach (TypeDeclaration type in _Unit.DeclaredTypes)
      {
        if (type.EnclosingNamespace == null) globalTypes.Add(type);
      }

      foreach (TypeDeclaration type in 
        new SortableCollection<TypeDeclaration>(globalTypes).SortBy("Name"))
      {
        Nodes.Add(TypeTreeNode.CreateTypeNode(type));
      }
    }
  }
}