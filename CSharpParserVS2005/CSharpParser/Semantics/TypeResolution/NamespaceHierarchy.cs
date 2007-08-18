using System;
using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace hierarchy that contains one or more 
  /// ResolutionTrees to resolv namespaces and types.
  /// </summary>
  /// <remarks>
  /// During the compilation there is a global namespece hierarchy including the 
  /// types and namespaced declared in the source code plus types in assemblies not 
  /// aliased with the "extern alias" directive.
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceHierarchy
  {
    #region Private fields

    private readonly Dictionary<string, TypeResolutionTree> _ResolutionTrees =
      new Dictionary<string, TypeResolutionTree>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace hierarchy
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy()
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of resolution trees in this namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<TypeResolutionTree> ResolutionTrees
    {
      get { return _ResolutionTrees.Values; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add the specified reolution tree to the hierarchy.
    /// </summary>
    /// <param name="name">Name of the resolution tree</param>
    /// <param name="tree">Tree instance</param>
    // --------------------------------------------------------------------------------
    public void AddTree(string name, TypeResolutionTree tree)
    {
      _ResolutionTrees.Add(name, tree);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clears all items in this namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Clear()
    {
      _ResolutionTrees.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this namespace hierarchy contains the specified tree or not.
    /// </summary>
    /// <param name="key">Key of the tree to check</param>
    /// <returns>
    /// True, if the tree exists within the hierarchy; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool ContainsTree(string key)
    {
      return _ResolutionTrees.ContainsKey(key);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type resolution tree with the specified name. 
    /// </summary>
    /// <param name="key">Name of the type resolution tree</param>
    /// <returns>
    /// Tree instance, if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public TypeResolutionTree this[string key]
    {
      get
      {
        TypeResolutionTree result;
        if (_ResolutionTrees.TryGetValue(key, out result)) return result;
        return null;
      }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Finds the name in any resolution trees within this hierarchy.
    /// </summary>
    /// <param name="type">TypeReference representing the name to find.</param>
    /// <param name="nextPart">Next part of the name that cannot be resolved.</param>
    /// <returns>
    /// List of resolution nodes that could resolve the name as long as
    /// 'nextpart' indicates.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList FindName(TypeReference type, out TypeReference nextPart)
    {
      // --- Init name resolution
      ResolutionNodeList result = new ResolutionNodeList();
      nextPart = type;
      int maxResolutionLength = 0;
      foreach (TypeResolutionTree resTree in _ResolutionTrees.Values)
      {
        ResolutionNodeBase resolvingNode;
        TypeReference carryOnPart;

        // --- Resolve the name in the current tree
        int depth = resTree.FindName(type, out resolvingNode, out carryOnPart);

        // --- This time we could not resolve the name as deep as before, forget
        // --- about this result.
        if (depth < maxResolutionLength) continue;

        // --- This time we resolved the name deeper then before. Forget about
        // --- partial results found before and register only this new result.
        if (depth > maxResolutionLength)
        {
          result.Clear();
          maxResolutionLength = depth;
        }

        // --- Add the result found to the list resolution nodes
        result.Add(resolvingNode);
        nextPart = carryOnPart;
      }
      return result;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the specified namespace into this hierarchy.
    /// </summary>
    /// <param name="nameSpace">Namespace to import.</param>
    // --------------------------------------------------------------------------------
    public void ImportNamespace(string nameSpace)
    {
      foreach (TypeResolutionTree tree in _ResolutionTrees.Values)
      {
        AssemblyResolutionTree asmTree = tree as AssemblyResolutionTree;
        if (asmTree != null) asmTree.ImportNamespace(nameSpace);
      }
    }

    #endregion
  }

}
