using System.Collections.Generic;
using System.IO;
using CSharpParser.Collections;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a C# file in the project to compile.
  /// </summary>
  // ==================================================================================
  public sealed class ProjectFile
  {
    #region Private fields

    private string _Name;
    private string _Folder;
    private ProjectParser _ParentProject;
    private ExternalAliasCollection _ExternAliases = new ExternalAliasCollection();
    private UsingClauseCollection _Usings = new UsingClauseCollection();
    private AttributeCollection _GlobalAttributes = new AttributeCollection();
    private List<Namespace> _Namespaces = new List<Namespace>();
    private List<TypeDeclaration> _TypeDeclarations = new List<TypeDeclaration>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new ProjectFile instance
    /// </summary>
    /// <param name="name">Full file name</param>
    /// <param name="parent">Parent project of this file.</param>
    // --------------------------------------------------------------------------------
    public ProjectFile(string name, ProjectParser parent)
    {
      _ParentProject = parent;
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
    public ProjectParser ParentProject
    {
      get { return _ParentProject; }
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
    public List<Namespace> Namespaces
    {
      get { return _Namespaces; }
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

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of project files.
  /// </summary>
  // ==================================================================================
  public class ProjectFileCollection : RestrictedList<ProjectFile>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of project files.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProjectFileCollection()
    {
    }

    #endregion
  }
}
