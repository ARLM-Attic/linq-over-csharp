using System;
using System.Collections.Generic;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents an implicit namespace that has not been explicitly 
  /// declared withtin the source code.
  /// </summary>
  /// <remarks>
  /// <para>
  /// If you declare namespaces in your source code, for example A, A.B.C there can be 
  /// implicit namespaces In our case A.B is an implicit namespace that may contain
  /// other types (in referenced assemblies). A.B is the enclosing namespace of A.B.C
  /// even if it is not declared explicitly.
  /// </para>
  /// </remarks>
  // ==================================================================================
  public class ImplicitNamespace : ITypeDeclarationScope
  {
    #region Private fields

    private readonly UsingClauseCollection _Usings = new UsingClauseCollection();
    private readonly ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private readonly string _Name;
    private readonly CompilationUnit _ParentUnit;
    private readonly List<TypeDeclaration> _TypeDeclarations = new List<TypeDeclaration>();

    #endregion

    #region Lifecycle method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of an implicit namespace.
    /// </summary>
    /// <param name="name">Name of the namespace scope.</param>
    /// <param name="parentUnit">Compilation unit of this implicit </param>
    // --------------------------------------------------------------------------------
    public ImplicitNamespace(string name, CompilationUnit parentUnit)
    {
      _Name = name;
      _ParentUnit = parentUnit;
    }

    #endregion

    #region ITypeDeclarationScope implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the scope
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ScopeName
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the external aliases of the declaration unit
    /// </summary>
    // --------------------------------------------------------------------------------
    public ExternalAliasCollection ExternAliases
    {
      get { return _ExternAliases; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of using clauses in this declaration unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public UsingClauseCollection Usings
    {
      get { return _Usings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of namespace declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragmentCollection NestedNamespaces
    {
      get
      {
        throw new NotSupportedException("Implicit namespace does not support NestedNamespaces.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeDeclaration> TypeDeclarations
    {
      get { return _TypeDeclarations; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile EnclosingSourceFile
    {
      get
      {
        throw new NotSupportedException("Implicit namespace does not support NestedNamespaces.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment EnclosingNamespace
    {
      get
      {
        throw new NotSupportedException("Implicit namespace does not support NestedNamespaces.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment DeclaringNamespace
    {
      get
      {
        throw new NotSupportedException("Implicit namespace does not support NestedNamespaces.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to iterate from the current scope toward the containing source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<ITypeDeclarationScope> ScopesToOuter
    {
      get
      {
        throw new NotSupportedException("Implicit namespace does not support NestedNamespaces.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Looks for the specified simple namespace or type name directly in this
    /// type declaration scope.
    /// </summary>
    /// <param name="type">Type reference representing the name to find.</param>
    /// <returns>
    /// Resolution nodes representing the name found, or null if the name cannot 
    /// be found.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList FindSimpleName(TypeReference type)
    {
      ResolutionNodeList results = new ResolutionNodeList();

      // --- Go through all resolution trees in the global hierarchy and look for
      // --- the name in them.
      foreach (TypeResolutionTree tree in
        _ParentUnit.GlobalHierarchy.ResolutionTrees.Values)
      {
        ResolutionNodeBase nsNode = tree.FindCompoundChild(_Name);
        if (nsNode != null)
        {
          // --- Namespace found, look for the child
          ResolutionNodeBase nameNode = nsNode.FindSimpleNamespaceOrType(type);
          if (nameNode != null) results.Add(nameNode);
        }
      }
      return results;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this declaration scope contains an extern-alias or a using-alias
    /// declaration with the specified name.
    /// </summary>
    /// <param name="name">Alias name to check.</param>
    /// <param name="foundInScope">Scope where the alias has been found.</param>
    /// <returns>
    /// True, if the this scope contains the alias; otherwise, false. If alias found, 
    /// foundInScope contains that scope declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool ContainsAlias(string name, out ITypeDeclarationScope foundInScope)
    {
      foundInScope = null;
      return false;
    }

    #endregion
  }
}
