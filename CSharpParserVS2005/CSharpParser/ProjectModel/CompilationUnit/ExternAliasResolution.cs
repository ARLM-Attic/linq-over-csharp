using System.Reflection;
using CSharpParser.Collections;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type defines an external alias resolution.
  /// </summary>
  // ==================================================================================
  public sealed class ExternAliasResolution
  {
    #region Private fields

    private readonly string _Alias;
    private readonly string _ResourceName;
    private Assembly _Assembly;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new external alias resolution.
    /// </summary>
    /// <param name="_Alias">Alias name (root namespace hierarchy)</param>
    /// <param name="_ResourceName">Name of the resource.</param>
    // --------------------------------------------------------------------------------
    public ExternAliasResolution(string _Alias, string _ResourceName)
    {
      this._Alias = _Alias;
      this._ResourceName = _ResourceName;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Alias
    {
      get { return _Alias; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the corresponding resource name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ResourceName
    {
      get { return _ResourceName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the assembly addressed by this resolution item.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Assembly Assembly
    {
      get
      {
        if (_Assembly == null)
        {
          ReferencedAssembly asmRef;
          int pos = _ResourceName.IndexOf(',');
          if (pos < 0) asmRef = new ReferencedAssembly(_ResourceName);
          else
          {
            asmRef = new ReferencedAssembly(_ResourceName.Substring(0, pos),
                                           _ResourceName.Substring(pos + 1));
          }
          _Assembly = asmRef.Assembly;
        }
        return _Assembly;
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of compilation references.
  /// </summary>
  // ==================================================================================
  public sealed class ExternAliasResolutionCollection : RestrictedCollection<ExternAliasResolution>
  {
  }
}
