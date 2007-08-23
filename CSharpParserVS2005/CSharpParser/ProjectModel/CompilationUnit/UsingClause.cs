using System.Collections.Generic;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a using clause that has been declared either in a
  /// file or in a namespace.
  /// </summary>
  // ==================================================================================
  public sealed class UsingClause : LanguageElement, IUsesResolutionContext
  {
    #region Private fields

    private readonly TypeReference _ReferencedName;
    private readonly bool _HasAlias;

    // --- Fields related to name resolution
    private readonly ResolutionNodeList _Resolvers = new ResolutionNodeList();
    private bool _IsResolved;
    private bool _ResolvedToNamespace;
    private ITypeCharacteristics _ResolvingType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new using clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="name">Using alias name, if defined, otherwise null or empty.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="typeUsed">Type reference used by this using clause.</param>
    // --------------------------------------------------------------------------------
    public UsingClause(Token token, CSharpSyntaxParser parser, string name, 
      TypeReference typeUsed): 
      base (token, parser, name)
    {
      _ReferencedName = typeUsed;
      _HasAlias = true;
      if (string.IsNullOrEmpty(name))
      {
        _HasAlias = false;
        Name = typeUsed.FullName;
      }
      _IsResolved = false;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the using clause has an alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasAlias
    {
      get { return _HasAlias; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type used by this directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ReferencedName
    {
      get { return _ReferencedName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of resolvers belonging to this using clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList Resolvers
    {
      get { return _Resolvers; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this using clause has been resolved to t namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool ResolvedToNamespace
    {
      get { return _ResolvedToNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type this using alias has been resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics ResolvingType
    {
      get { return _ResolvingType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the using clause is resolved or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _IsResolved; }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      if (_ReferencedName != null)
      {
        _ReferencedName.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sign that using clause is resolved to a type.
    /// </summary>
    /// <param name="type">Type this using clause is resolved to.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToType(ITypeCharacteristics type)
    {
      _ResolvedToNamespace = false;
      _ResolvingType = type;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sign that using clause is resolved to a namespace.
    /// </summary>
    /// <param name="nsNodes">Namespac resolver nodes.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToNamespace(IEnumerable<NamespaceResolutionNode> nsNodes)
    {
      _ResolvedToNamespace = true;
      _Resolvers.Clear();
      foreach (NamespaceResolutionNode nsNode in nsNodes)
      {
        _Resolvers.Add(nsNode);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that this using clause has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SignResolved()
    {
      _IsResolved = true;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of using clauses.
  /// </summary>
  // ==================================================================================
  public class UsingClauseCollection : RestrictedCollection<UsingClause>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clause having the specified alias name.
    /// </summary>
    /// <param name="key">Alias name.</param>
    /// <returns>
    /// Using clause, if found by the alias name; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public UsingClause this[string key]
    {
      get
      {
        foreach(UsingClause item in this)
          if (item.HasAlias && item.Name == key) return item;
        return null;
      }
    }
  }
}
