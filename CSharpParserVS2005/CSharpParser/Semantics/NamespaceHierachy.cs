using System.Collections.Generic;
using System.Reflection;
using CSharpParser.Collections;
using CSharpParser.ProjectModel;

namespace CSharpParser
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace hierarchy that ise used when resolving type
  /// names.
  /// </summary>
  /// <remarks>
  /// During the compilation there is a global namespece hierarchy including the 
  /// types and namespaced declared in the source code plus types in assemblies not 
  /// aliased with the "extern alias" directive.
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceHierachy
  {
    #region Private fields

    private readonly string _Name;
    private readonly List<Assembly> _Assemblies = new List<Assembly>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace hierarchy with the specified name.
    /// </summary>
    /// <param name="name">Name of the namespace hierarchy.</param>
    // --------------------------------------------------------------------------------
    public NamespaceHierachy(string name)
    {
      _Name = name;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace hierarchy
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// List of assemblies belonging to the namespace hierarchy
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Assembly> Assemblies
    {
      get { return _Assemblies; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of namespace hierarchy that can be indexed by 
  /// name.
  /// </summary>
  // ==================================================================================
  public sealed class NamespaceHierarchyCollection : RestrictedIndexedCollection<NamespaceHierachy>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">NamespaceHierarchy item.</param>
    /// <returns>
    /// Name of the namespace hierarchy.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(NamespaceHierachy item)
    {
      return item.Name;
    }

    public void Add(ExternAliasResolution aliasRes)
    {
      NamespaceHierachy nsHierarchy;
      if (!TryGetValue(aliasRes.Alias, out nsHierarchy))
      {
        nsHierarchy = new NamespaceHierachy(aliasRes.Alias);
        Add(nsHierarchy);
      }
    }
  }
}
