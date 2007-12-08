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
    private bool _IsResolvedToNamespace;
    private ITypeAbstraction _ResolvingType;
    private string _ResolvingNamespace;

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
    /// Gets the flag indicating if the using clause is resolved or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _IsResolved; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this using clause has been resolved to a namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToNamespace
    {
      get { return _IsResolvedToNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this using clause has been resolved to a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToType
    {
      get { return !_IsResolvedToNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type this using alias has been resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction ResolvingType
    {
      get { return _ResolvingType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace this using has been resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ResolvingNamespace
    {
      get { return _ResolvingNamespace; }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      if (_ReferencedName != null)
      {
        _ReferencedName.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolvers of this using clause according to the specified node list.
    /// </summary>
    /// <param name="resolvers">List of resolver nodes.</param>
    // --------------------------------------------------------------------------------
    public void SetResolvers(ResolutionNodeList resolvers)
    {
      _Resolvers.Clear();
      _Resolvers.Merge(resolvers);
      ResolutionNodeBase resolver = _Resolvers[0];
      if (resolver is NamespaceResolutionNode)
      {
        _IsResolvedToNamespace = true;
        _ResolvingNamespace = resolver.FullName;
      }
      else
      {
        _IsResolvedToNamespace = false;
        _ResolvingType = (resolver as TypeResolutionNode).Resolver;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that this using clause has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SignResolved()
    {
      _IsResolved = _Resolvers.Count > 0;
      Validate(_IsResolved);
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
