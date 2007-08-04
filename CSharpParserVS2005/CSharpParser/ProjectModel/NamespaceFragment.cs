using System;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Properties;
using CSharpParser.Semantics;

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
  public sealed class NamespaceFragment: LanguageElement, IResolutionRequired
  {
    #region Private fields

    private readonly NamespaceFragment _ParentNamespace;
    private readonly SourceFile _ParentFile; 
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
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="name">Name of the namespace (relative to the parent).</param>
    /// <param name="parentNamespace">Parent namespace of this namespace.</param>
    /// <param name="parentFile">Parent file defining this namespace.</param>
    // --------------------------------------------------------------------------------
    public NamespaceFragment(Token token, CSharpSyntaxParser parser, string name, 
      NamespaceFragment parentNamespace, SourceFile parentFile): base(token, parser)
    {
      Name = name;
      _ParentNamespace = parentNamespace;

      // --- If this namespace has a parent, this namespace is a nested namespace of the parent.
      if (_ParentNamespace != null)
      {
        _ParentNamespace.NestedNamespaces.Add(this);
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

      // --- The namespace is added to the list of CompilationUnit

      Namespace nameSpace;
      CompilationUnit project = _ParentFile.ParentUnit;
      if (project.DeclaredNamespaces.TryGetValue(FullName, out nameSpace))
      {
        // --- This namespace has been declared, add the new fragment.
        nameSpace.Fragments.Add(this);
      }
      else
      {
        // --- This is the first declaration of this namespace.
        Namespace newNamespace = new Namespace(FullName);
        newNamespace.Fragments.Add(this);
        project.DeclaredNamespaces.Add(newNamespace);
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the file where the namespace has been declared.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile ParentFile
    {
      get { return _ParentFile; }
    } 
    
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
      _ExternAliases.Add(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new external alias to this namespace.
    /// </summary>
    /// <param name="item"></param>
    // --------------------------------------------------------------------------------
    public void AddUsingClause(UsingClause item)
    {
      _Usings.Add(item);
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
      _TypeDeclarations.Add(item);
      try
      {
        _ParentFile.ParentUnit.DeclaredTypes.Add(item);
      }
      catch (ArgumentException)
      {
        Parser.Error0101(item.Token, Name, item.FullName);
      }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      // --- Resolve using claues
      foreach (UsingClause usingClause in _Usings)
      {
        usingClause.ResolveTypeReferences(contextType, contextInstance);
      }
      // --- Resolve types in this namespace
      foreach (TypeDeclaration type in _TypeDeclarations)
      {
        type.ResolveTypeReferences(contextType, contextInstance);
      }
      // --- Resolve references in nseted namespaces
      foreach (NamespaceFragment nameSpace in _NestedNamespaces)
      {
        nameSpace.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of namespaces.
  /// </summary>
  // ==================================================================================
  public class NamespaceFragmentCollection : RestrictedCollection<NamespaceFragment>
  {
  }
}
