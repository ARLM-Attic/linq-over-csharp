using System.Collections.Generic;

namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace hierarchy.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The namespace hierarchy is the top level of the resolution hierarchy. During 
  /// the compilation there is a global namespace hierarchy including the 
  /// types and namespaced declared in the source code plus types in assemblies not 
  /// aliased with the "extern alias" directive.
  /// </para>
  /// <para>
  /// A namespace hierarchy containts one or more type resolution tree representing
  /// a referenced assembly or the source code.
  /// </para>
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
    /// Gets the resolution trees belonging to this namespace hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Dictionary<string, TypeResolutionTree> ResolutionTrees
    {
      get { return _ResolutionTrees; }
    } 
    
    #endregion
  }

}
