using System.Collections.Generic;

namespace CSharpParser.Semantics
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
  public sealed class SourceResolutionTree : TypeResolutionTree
  {
    #region Private fields

    // --- Contains a list of imported assemblies to check before duplicate import
    private readonly List<string> _ImportedAssemblies =
      new List<string>();
    // --- Contains a list of imported namespaces to check before duplicate import
    private readonly List<string> _ImportedNamespaces =
      new List<string>();
    // --- Contains a shortcut cache to accelerate nested namespace node access

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace hierarchy with the specified name.
    /// </summary>
    /// <param name="name">Name of the namespace hierarchy.</param>
    // --------------------------------------------------------------------------------
    public SourceResolutionTree(string name)
      : base(null, name)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of imported assembly names
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ImportedAssemblies
    {
      get { return _ImportedAssemblies; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of imported namespaces
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ImportedNamespaces
    {
      get { return _ImportedNamespaces; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clear all nodes of this hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void Clear()
    {
      base.Clear();
      _ImportedAssemblies.Clear();
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

    #endregion
  }
}