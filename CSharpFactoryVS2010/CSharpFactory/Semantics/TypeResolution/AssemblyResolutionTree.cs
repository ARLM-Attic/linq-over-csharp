using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a resolving unit (a referenced assembly) that contains a
  /// hierarchy of namespaces and types.
  /// </summary>
  /// <remarks>
  /// The parser uses this resolution unit to resolve namespaces and type names.
  /// A namespace hierarchy may contain one or more resolution unit.
  /// </remarks>
  // ==================================================================================
  public sealed class AssemblyResolutionTree : TypeResolutionTree
  {
    #region Private fields

    private readonly Assembly _Assembly;
    // --- Contains a list of imported namespaces to check before duplicate import
    private readonly List<string> _ImportedNamespaces =
      new List<string>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolution unit for the specified assembly.
    /// </summary>
    /// <param name="assembly">
    /// Assembly that contains types used by this resolution unit
    /// </param>
    // --------------------------------------------------------------------------------
    public AssemblyResolutionTree(Assembly assembly)
      : base(null, assembly.FullName)
    {
      _Assembly = assembly;
      CollectNamespaces();
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of imported namespaces
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ImportedNamespaces
    {
      get { return _ImportedNamespaces; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the assembly belonging to this resolution tree.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Assembly Assembly
    {
      get { return _Assembly; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the types in the specified namespace are imported in this hierarchy 
    /// or not.
    /// </summary>
    /// <param name="nsKey">Full namespace name</param>
    /// <returns>
    /// True, if the namespace has already been imported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool IsImported(string nsKey)
    {
      return _ImportedNamespaces.Contains(nsKey);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the types in the specified namespace are imported in this hierarchy 
    /// or not.
    /// </summary>
    /// <param name="nsKey">Full namespace name</param>
    /// <returns>
    /// True, if the namespace has already been imported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public void SignNamespaceIsImported(string nsKey)
    {
      _ImportedNamespaces.Add(nsKey);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Collects namespace information from this resolution unit
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CollectNamespaces()
    {
      // --- Store namespaces already imported to avoid repeated processing
      Dictionary<string, int> nsCache = new Dictionary<string, int>();

      // --- This is the first time we import namespaces from this assembly. We go
      // --- through its types to obtain namespace names.
      foreach (Type type in _Assembly.GetTypes())
      {
        if (!String.IsNullOrEmpty(type.Namespace) && !nsCache.ContainsKey(type.Namespace))
        {
          // --- This namespace is not collected yet from this assembly instance
          CreateNamespace(type.Namespace);
          nsCache.Add(type.Namespace, 0);
        }
      }
    }

    #endregion
  }
}
