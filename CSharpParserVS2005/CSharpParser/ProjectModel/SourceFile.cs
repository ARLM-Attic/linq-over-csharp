using System;
using System.IO;
using CSharpParser.Collections;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a C# file in the project to compile.
  /// </summary>
  // ==================================================================================
  public sealed class SourceFile: IResolutionRequired
  {
    #region Private fields

    private readonly string _Name;
    private readonly string _Folder;
    private readonly CompilationUnit _ParentUnit;
    private readonly ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private readonly UsingClauseCollection _Usings = new UsingClauseCollection();
    private readonly AttributeCollection _GlobalAttributes = new AttributeCollection();
    private readonly NamespaceFragmentCollection _Namespaces = new NamespaceFragmentCollection();
    private readonly TypeDeclarationCollection _TypeDeclarations = new TypeDeclarationCollection();
    private readonly RegionInfoCollection _Regions = new RegionInfoCollection();
    private readonly CommentInfoCollection _Comments = new CommentInfoCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new SourceFile instance
    /// </summary>
    /// <param name="name">Full file name</param>
    /// <param name="parent">Parent project of this file.</param>
    // --------------------------------------------------------------------------------
    public SourceFile(string name, CompilationUnit parent)
    {
      _ParentUnit = parent;
      _Name = Path.GetFileName(name);
      _Folder = Path.GetDirectoryName(name);
      _TypeDeclarations.AfterAdd += TypeDeclarationsAfterAdd;
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
    public NamespaceFragmentCollection Namespaces
    {
      get { return _Namespaces; }
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

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds the type declaration not only to the file but also to the project.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    // --------------------------------------------------------------------------------
    void TypeDeclarationsAfterAdd(object sender, ItemedEventArgs<TypeDeclaration> e)
    {
      try
      {
        _ParentUnit.DeclaredTypes.Add(e.Item);
      }
      catch (ArgumentException)
      {
        _ParentUnit.ErrorHandler.Error("CS0101",
          e.Item.Token,
          String.Format("The namespace '{0}' already contains a definition for '{1}'",
          Name, e.Item.FullName));
      }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this source file.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      foreach (UsingClause usingClause in _Usings)
      {
        usingClause.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (AttributeDeclaration attr in _GlobalAttributes)
      {
        attr.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (TypeDeclaration typeDeclaration in _TypeDeclarations)
      {
        typeDeclaration.ResolveTypeReferences(ResolutionContext.SourceFile, this);
      }
      foreach (NamespaceFragment nameSpace in _Namespaces)
      {
        nameSpace.ResolveTypeReferences(ResolutionContext.SourceFile, this);
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of project files.
  /// </summary>
  // ==================================================================================
  public class SourceFileCollection : RestrictedCollection<SourceFile>
  {
  }
}
