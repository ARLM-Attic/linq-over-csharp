using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents an external alias declaration belonging to a file or to a
  /// namespace.
  /// </summary>
  // ==================================================================================
  public sealed class ExternalAlias : LanguageElement
  {
    #region Private fields

    private NamespaceHierarchy _Hierarchy;
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new external alias declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by the comment</param>
    // --------------------------------------------------------------------------------
    public ExternalAlias(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public fields

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the hierarchy belonging to this extern alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy Hierarchy
    {
      get { return _Hierarchy; }
      set { _Hierarchy = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating this extern alias has a hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasHierarchy
    {
      get { return _Hierarchy != null; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of external aliases within a project file.
  /// </summary>
  // ==================================================================================
  public class ExternalAliasCollection : RestrictedCollection<ExternalAlias>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the external alias having the specified alias name.
    /// </summary>
    /// <param name="key">Alias name.</param>
    /// <returns>
    /// External alias, if found by the alias name; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ExternalAlias this[string key]
    {
      get
      {
        foreach (ExternalAlias item in this)
          if (item.Name == key) return item;
        return null;
      }
    }
  }
}
