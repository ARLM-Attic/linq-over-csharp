using System.Collections.Generic;
using CSharpParser.ParserFiles;

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
  public sealed class Namespace: LanguageElement
  {
    private string _FullName;
    private Namespace _ParentNamespace;
    private List<ExternalAlias> _ExternAliases = new List<ExternalAlias>();
    private List<Namespace> _NestedNamespaces = new List<Namespace>();
    private List<UsingClause> _Usings = new List<UsingClause>();
    private List<TypeDeclaration> _TypeDeclarations = new List<TypeDeclaration>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace belonging to a file.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public Namespace(Token token): base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get { return _FullName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Namespace ParentNamespace
    {
      get { return _ParentNamespace; }
      set
      {
        _ParentNamespace = value;
        if (_ParentNamespace == null)
        {
          _FullName = Name;
        }
        else
        {
          _FullName = _ParentNamespace._FullName + "." + Name;
          _ParentNamespace.NestedNamespaces.Add(this);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of nested namespaces.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Namespace> NestedNamespaces
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
    public List<UsingClause> Usings
    {
      get { return _Usings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the external aliases used by the namespace
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ExternalAlias> ExternAliases
    {
      get { return _ExternAliases; }
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
  }
}
