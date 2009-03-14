using System;
using System.Collections.Generic;
using System.IO;
using CSharpFactory.Collections;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a C# file in the project to compile.
  /// </summary>
  // ==================================================================================
  public sealed class SourceFile: 
    ITypeDeclarationScope,
    IUsesResolutionContext
  {
    #region Private fields

    private readonly string _Name;
    private readonly string _Folder;
    private readonly CompilationUnit _ParentUnit;
    private readonly ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private readonly UsingClauseCollection _Usings = new UsingClauseCollection();
    private readonly AttributeCollection _GlobalAttributes = new AttributeCollection();
    private readonly NamespaceFragmentCollection _Namespaces = new NamespaceFragmentCollection();
    private readonly List<TypeDeclaration> _TypeDeclarations = new List<TypeDeclaration>();
    private readonly RegionInfoCollection _Regions = new RegionInfoCollection();
    private readonly CommentInfoCollection _Comments = new CommentInfoCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new SourceFile instance
    /// </summary>
    /// <param name="name">Full file name</param>
    /// <param name="parent">ParentBlock project of this file.</param>
    // --------------------------------------------------------------------------------
    public SourceFile(string name, CompilationUnit parent)
    {
      _ParentUnit = parent;
      _Name = Path.GetFileName(name);
      _Folder = Path.GetDirectoryName(name);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent project of this file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit ParentUnit
    {
      get { return _ParentUnit; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the file
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder of the file
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Folder
    {
      get { return _Folder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the scope
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ScopeName
    {
      get { return CompilationUnit.GlobalHierarchyName; }
    }

    /// <summary>
    /// Gets the external aliases of the file
    /// </summary>
    // --------------------------------------------------------------------------------
    public ExternalAliasCollection ExternAliases
    {
      get { return _ExternAliases; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get { return Path.Combine(_Folder, _Name); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of using clauses in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public UsingClauseCollection Usings
    {
      get { return _Usings; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of global attribute declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public AttributeCollection GlobalAttributes
    {
      get { return _GlobalAttributes; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of namespace declarations in this project file
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragmentCollection NestedNamespaces
    {
      get { return _Namespaces; }
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
      ResolutionNodeList results = new ResolutionNodeList();
      foreach (TypeResolutionTree tree in 
        _ParentUnit.GlobalHierarchy.ResolutionTrees.Values)
      {
        ResolutionNodeBase node = tree.FindSimpleNamespaceOrType(type);
        if (node != null) results.Add(node);
      }
      return results;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this declaration scope contains an extern-alias or a using-alias
    /// declaration with the specified name.
    /// </summary>
    /// <param name="name">AliasToken name to check.</param>
    /// <param name="foundInScope">Scope where the alias has been found.</param>
    /// <returns>
    /// True, if the this scope contains the alias; otherwise, false. If alias found, 
    /// foundInScope contains that scope declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool ContainsAlias(string name, out ITypeDeclarationScope foundInScope)
    {
      return ContainsAlias(this, name, out foundInScope);
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
    /// Gets the #region..#endregion information about this source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public RegionInfoCollection Regions
    {
      get { return _Regions; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of comment information of this source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfoCollection Comments
    {
      get { return _Comments; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new region to this source file.
    /// </summary>
    /// <param name="region">#region information.</param>
    // --------------------------------------------------------------------------------
    public void AddRegion(RegionInfo region)
    {
      _Regions.Add(region);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new comment to this source file.
    /// </summary>
    /// <param name="comment">Comment information.</param>
    // --------------------------------------------------------------------------------
    public void AddComment(CommentInfo comment)
    {
      _Comments.Add(comment);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type declaration to the container of types declared in this source 
    /// file.
    /// </summary>
    /// <param name="type">Type to add</param>
    /// <remarks>
    /// Type declaration is added to the type container of the compilation unit.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void AddTypeDeclaration(TypeDeclaration type)
    {
      _TypeDeclarations.Add(type);
      _ParentUnit.AddTypeDeclaration(type);
      type.SetSourceFile(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the scope within the file using the specified namespace.
    /// </summary>
    /// <param name="ns">Namespace within the file.</param>
    /// <returns>
    /// The namespace scope, if that is not null; otherwise the file scope.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeDeclarationScope NamespaceScope(ITypeDeclarationScope ns)
    {
      return ns ?? (ITypeDeclarationScope)this;
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this source file.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      foreach (AttributeDeclaration attr in _GlobalAttributes)
      {
        attr.ResolveTypeReferences(ResolutionContext.SourceFile, this, parameterScope);
      }
      foreach (TypeDeclaration typeDeclaration in _TypeDeclarations)
      {
        typeDeclaration.ResolveTypeReferences(ResolutionContext.SourceFile, this, parameterScope);
      }
      foreach (NamespaceFragment nameSpace in _Namespaces)
      {
        nameSpace.ResolveTypeReferences(ResolutionContext.SourceFile, this, parameterScope);
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
      get { return this; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    /// <remarks>
    /// We return null to sign that the enclosing namespace of a source file is
    /// the global namespace.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public NamespaceFragment EnclosingNamespace
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment DeclaringNamespace
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator to iterate from the current scope toward the containing source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<ITypeDeclarationScope> ScopesToOuter
    {
      get { return IterateScopesToOuter(this); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration EnclosingType
    {
      get { throw new NotSupportedException("Source file has no enclosing type."); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MethodDeclaration EnclosingMethod
    {
      get { throw new NotSupportedException("Source file has no enclosing method."); }
    }

    #endregion

    #region Iterators

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterates through the enclosing namespaces to this source file using the 
    /// specified inner namespace as a starting point.
    /// </summary>
    /// <param name="ns">Namespace to start from.</param>
    /// <returns>Current enclosing declaration scope.</returns>
    // --------------------------------------------------------------------------------
    public IEnumerable<ITypeDeclarationScope> GetScopesToOuter(NamespaceFragment ns)
    {
      while (ns != null)
      {
        // --- Retrieve the namespace fragment itself
        yield return ns;

        // --- Create implicit namespaces
        string[] nameParts = ns.Name.Split('.');
        Stack<string> names= new Stack<string>();
        string name = ns.EnclosingNamespace == null 
          ? String.Empty 
          : ns.EnclosingNamespace.FullName;
        for (int i = 0; i < nameParts.Length - 1; i++)
        {
          if (name != String.Empty) name += ".";
          name += nameParts[i];
          names.Push(name);
        }
        while (names.Count > 0)
        {
          yield return new ImplicitNamespace(names.Pop(), _ParentUnit);
        }

        // --- Move to the parent fragment
        ns = ns.EnclosingNamespace;
      }
      yield return this;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterates through the enclosing namespaces to this source file using the 
    /// specified inner namespace as a starting point.
    /// </summary>
    /// <param name="innerScope">Inner declaration scope to start from.</param>
    /// <returns>Current enclosing declaration scope.</returns>
    // --------------------------------------------------------------------------------
    public static IEnumerable<ITypeDeclarationScope> IterateScopesToOuter(
      ITypeDeclarationScope innerScope)
    {
      if (innerScope == null) yield break;
      ITypeDeclarationScope fileScope = innerScope.EnclosingSourceFile;
      if (innerScope != fileScope)
      {
        while (innerScope != null)
        {
          yield return innerScope;
          innerScope = innerScope.EnclosingNamespace;
        }
      }
      if (fileScope != null) yield return fileScope;
    }

    #endregion

    #region Static helper methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified declaration scope contains an extern-alias or a 
    /// using-alias declaration with the specified name.
    /// </summary>
    /// <param name="innerScope">Scope to check.</param>
    /// <param name="name">AliasToken name to check.</param>
    /// <param name="foundInScope">Scope where the alias has been found.</param>
    /// <returns>
    /// True, if the this scope contains the alias; otherwise, false. If alias found, 
    /// foundInScope contains that scope declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool ContainsAlias(ITypeDeclarationScope innerScope, string name,
      out ITypeDeclarationScope foundInScope)
    {
      foundInScope = null;
      foreach (ITypeDeclarationScope scope in innerScope.ScopesToOuter)
      {
        if (scope.Usings[name] != null || scope.ExternAliases[name] != null)
        {
          foundInScope = scope;
          return true;
        }
      }
      return false;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of project files.
  /// </summary>
  // ==================================================================================
  public class SourceFileCollection : ImmutableCollection<SourceFile>
  {
  }
}
