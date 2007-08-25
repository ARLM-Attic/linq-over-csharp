using System;
using System.Collections.Generic;
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
  public sealed class NamespaceFragment: LanguageElement, 
    ITypeDeclarationScope,
    IUsesResolutionContext
  {
    #region Private fields

    private readonly NamespaceFragment _ExclosingNamespace;
    private readonly SourceFile _EnclosingFile; 
    private readonly ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private readonly NamespaceFragmentCollection _NestedNamespaces = new NamespaceFragmentCollection();
    private readonly UsingClauseCollection _Usings = new UsingClauseCollection();
    private readonly TypeDeclarationCollection _TypeDeclarations = new TypeDeclarationCollection();

    // --- Fields used for semantics check
    private NamespaceResolutionNode _ResolverNode;

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
      // --- A namespace must belong to a file.
      if (parentFile == null)
        throw new InvalidOperationException(Resources.ParentFileNotDeclared);
      _EnclosingFile = parentFile;

      // --- Store attributes
      Name = name;
      _ExclosingNamespace = parentNamespace;
      CompilationUnit unit = Parser.CompilationUnit;

      if (_ExclosingNamespace != null) 
      {
        // --- This namespace has a parent, this namespace is a nested 
        // --- namespace of the parent.
        _ExclosingNamespace.NestedNamespaces.Add(this);
      }
      else
      {
        // --- This is a root namespace in the file, we add it to the namespace list
        // --- of the file.
        _EnclosingFile.NestedNamespaces.Add(this);
      }

      // --- The namespace is added to the list of CompilationUnit
      Namespace nameSpace;
      if (unit.DeclaredNamespaces.TryGetValue(FullName, out nameSpace))
      {
        // --- This namespace has been declared, add the new fragment.
        nameSpace.Fragments.Add(this);
      }
      else
      {
        // --- This is the first declaration of this namespace.
        Namespace newNamespace = new Namespace(FullName);
        newNamespace.Fragments.Add(this);
        unit.DeclaredNamespaces.Add(newNamespace);
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
        return _ExclosingNamespace == null 
          ? Name 
          : _ExclosingNamespace.FullName + "." + Name;
      }
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
    /// Looks for the specified simple namespace or type name directly in this
    /// type declaration scope.
    /// </summary>
    /// <param name="type">Type reference representing the name to find.</param>
    /// <returns>
    /// Resolution node representing the name found, or null if the name cannot 
    /// be found.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList FindSimpleName(TypeReference type)
    {
      if (_ResolverNode == null) return null;
      ResolutionNodeList results = new ResolutionNodeList();
      ResolutionNodeBase node = _ResolverNode.FindSimpleNamespaceOrType(type);
      if (node != null) results.Add(node);
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
      return SourceFile.ContainsAlias(this, name, out foundInScope);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this namespace has a parent or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParentNamespace
    {
      get { return _ExclosingNamespace != null; }
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
    /// Gets the name of the scope
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ScopeName
    {
      get { return FullName; }
    }

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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver node of this namespace fragment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceResolutionNode ResolverNode
    {
      get { return _ResolverNode; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolver during the semantic parsing.
    /// </summary>
    /// <remarks>
    /// This method can be called only after the full syntax parsing. First sets the
    /// resolver of this namespace then in its types and nested namespaces.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void SetNamespaceResolvers()
    {
      ResolutionNodeBase resolverNode;
      if (_ExclosingNamespace == null)
      {
        // --- This is a top namespace
        resolverNode = Parser.CompilationUnit.SourceResolutionTree;
      }
      else
      {
        // --- This is a nested namespace
        resolverNode = _ExclosingNamespace.ResolverNode;
      }

      // --- Register the namespace
      if (resolverNode != null)
      {
        _ResolverNode = resolverNode.CreateNamespace(Name);
        _ResolverNode.SignDefinedInSource();

        // --- Set resolvers for nested namespaces
        foreach (NamespaceFragment nested in _NestedNamespaces)
        {
          nested.SetNamespaceResolvers();
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolver during the semantic parsing.
    /// </summary>
    /// <remarks>
    /// This method can be called only after the full syntax parsing. First sets the
    /// resolver of this namespace then in its types and nested namespaces.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void SetTypeResolvers()
    {
      ResolutionNodeBase resolverNode;
      if (_ExclosingNamespace == null)
      {
        // --- This is a top namespace
        resolverNode = Parser.CompilationUnit.SourceResolutionTree;
      }
      else
      {
        // --- This is a nested namespace
        resolverNode = _ExclosingNamespace.ResolverNode;
      }

      // --- Register the namespace
      if (resolverNode != null)
      {
        // --- Set resolvers for types declared here
        foreach (TypeDeclaration type in _TypeDeclarations)
        {
          type.SetTypeResolvers();
        }

        // --- Set resolvers for nested namespaces
        foreach (NamespaceFragment nested in _NestedNamespaces)
        {
          nested.SetTypeResolvers();
        }
      }
    }

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
      item.EnclosingNamespace = this;
      _TypeDeclarations.Add(item);
      try
      {
        _EnclosingFile.ParentUnit.DeclaredTypes.Add(item);
      }
      catch (ArgumentException)
      {
        Parser.Error0101(item.Token, Name, item.FullName);
      }
    }

    #endregion

    #region IResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile EnclosingSourceFile
    {
      get { return _EnclosingFile; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment EnclosingNamespace
    {
      get { return _ExclosingNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to iterate from the current scope toward the containing source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<ITypeDeclarationScope> ScopesToOuter
    {
      get { return SourceFile.IterateScopesToOuter(this); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration EnclosingType
    {
      get { throw new NotSupportedException("Namespace has no enclosing type."); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MethodDeclaration EnclosingMethod
    {
      get { throw new NotSupportedException("Namespace has no enclosing method."); }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
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
