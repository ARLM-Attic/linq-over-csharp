using System;
using System.Collections.Generic;
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
  public sealed class Namespace: LanguageElement
  {
    private Namespace _ParentNamespace;
    private ProjectFile _ParentFile; 
    private ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private NamespaceCollection _NestedNamespaces = new NamespaceCollection();
    private List<UsingClause> _Usings = new List<UsingClause>();
    private List<TypeDeclaration> _TypeDeclarations = new List<TypeDeclaration>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace belonging to a file.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="name">Name of the namespace (relative to the parent).</param>
    /// <param name="parentNamespace">Parent namespace of this namespace.</param>
    /// <param name="parentFile">Parent file defining this namespace.</param>
    // --------------------------------------------------------------------------------
    public Namespace(Token token, string name, Namespace parentNamespace, 
      ProjectFile parentFile): base(token)
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

      // --- The namespace is added to the list of ProjectParser

      NamespaceCollection fragments;
      ProjectParser parser = _ParentFile.ParentProject;
      if (parser.DeclaredNamespaces.TryGetValue(FullName, out fragments))
      {
        // --- This namespace has been declared, add the new fragment.
        fragments.Add(this);
      }
      else
      {
        // --- This is the first declaration of this namespace.
        NamespaceCollection newFragment = new NamespaceCollection();
        newFragment.Add(this);
        parser.DeclaredNamespaces.Add(FullName, newFragment);
      }
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
    public Namespace ParentNamespace
    {
      get { return _ParentNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of nested namespaces.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceCollection NestedNamespaces
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
    public ExternalAliasCollection ExternAliases
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

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of namespaces.
  /// </summary>
  // ==================================================================================
  public class NamespaceCollection : ImmutableList<Namespace>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of namespaces.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceCollection()
    {
    }

    #endregion
  }
}
