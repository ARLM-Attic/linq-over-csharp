using System;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Properties;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace declared in a file.
  /// </summary>
  /// <remarks>
  /// Namespaces can be nested and one file may contain zero, one or more namespace
  /// declarations.
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceFragment: LanguageElement
  {
    #region Private fields

    private readonly NamespaceFragment _ParentNamespace;
    private readonly ProjectFile _ParentFile; 
    private readonly ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private readonly NamespaceFragmentCollection _NestedNamespaces = new NamespaceFragmentCollection();
    private readonly UsingClauseCollection _Usings = new UsingClauseCollection();
    private readonly TypeDeclarationCollection _TypeDeclarations = new TypeDeclarationCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace belonging to a file.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="name">Name of the namespace (relative to the parent).</param>
    /// <param name="parentNamespace">Parent namespace of this namespace.</param>
    /// <param name="parentFile">Parent file defining this namespace.</param>
    // --------------------------------------------------------------------------------
    public NamespaceFragment(Token token, string name, NamespaceFragment parentNamespace, 
      ProjectFile parentFile): base(token, name)
    {
      _ParentNamespace = parentNamespace;

      // --- If this namespace has a parent, this namespace is a nested namespace of the parent.
      if (_ParentNamespace != null)
      {
        (_ParentNamespace.NestedNamespaces as IRestrictedList<NamespaceFragment>).Add(this);
      }

      // --- A namespace must belong to a file.
      if (parentFile == null)
      {
        throw new InvalidOperationException(Resources.ParentFileNotDeclared);
      }
      _ParentFile = parentFile;

      // --- If this is a root namespace in the file, we add it to the namespace list
      // --- of the file.
      if (parentNamespace == null) _ParentFile.Namespaces.Add(this);

      // --- The namespace is added to the list of CSharpProject

      Namespace nameSpace;
      CSharpProject project = _ParentFile.ParentProject;
      if (project.DeclaredNamespaces.TryGetValue(FullName, out nameSpace))
      {
        // --- This namespace has been declared, add the new fragment.
        (nameSpace.Fragments as IRestrictedList<NamespaceFragment>).Add(this);
      }
      else
      {
        // --- This is the first declaration of this namespace.
        Namespace newNamespace = new Namespace(FullName);
        (newNamespace.Fragments as IRestrictedList<NamespaceFragment>).Add(this);
        (project.DeclaredNamespaces as IRestrictedIndexedCollection<Namespace>).
          Add(newNamespace);
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get
      {
        return _ParentNamespace == null 
          ? Name 
          : _ParentNamespace.FullName + "." + Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment ParentNamespace
    {
      get { return _ParentNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of nested namespaces.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragmentCollection NestedNamespaces
    {
      get { return _NestedNamespaces; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this namespace has a parent or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParentNamespace
    {
      get { return _ParentNamespace != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this namespace has nested namespaces or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasNestedNamespace
    {
      get { return _NestedNamespaces.Count > 0;  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses within this namespace declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public UsingClauseCollection Usings
    {
      get { return _Usings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the external aliases used by the namespace
    /// </summary>
    // --------------------------------------------------------------------------------
    public ExternalAliasCollection ExternAliases
    {
      get { return _ExternAliases; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclarationCollection TypeDeclarations
    {
      get { return _TypeDeclarations; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new external alias to this namespace.
    /// </summary>
    /// <param name="item"></param>
    // --------------------------------------------------------------------------------
    public void AddExternAlias(ExternalAlias item)
    {
      (_ExternAliases as IRestrictedList<ExternalAlias>).Add(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new external alias to this namespace.
    /// </summary>
    /// <param name="item"></param>
    // --------------------------------------------------------------------------------
    public void AddUsingClause(UsingClause item)
    {
      (_Usings as IRestrictedList<UsingClause>).Add(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type declaration to this namespace fragment.
    /// </summary>
    /// <param name="item"></param>
    // --------------------------------------------------------------------------------
    public void AddTypeDeclaration(TypeDeclaration item)
    {
      item.Namespace = this;
      (_TypeDeclarations as IRestrictedIndexedCollection<TypeDeclaration>).
        Add(item);
      (_ParentFile.ParentProject.DeclaredTypes
        as IRestrictedIndexedCollection<TypeDeclaration>).Add(item);
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of namespaces.
  /// </summary>
  // ==================================================================================
  public class NamespaceFragmentCollection : RestrictedList<NamespaceFragment>
  {
  }
}
