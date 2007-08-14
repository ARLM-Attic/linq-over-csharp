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
  public sealed class UsingClause : LanguageElement, IResolutionRequired
  {
    #region Private fields

    private readonly TypeReference _AliasedType;

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
      _AliasedType = typeUsed;
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
      get { return _AliasedType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type used by this directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference AliasedType
    {
      get { return _AliasedType; }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      if (_AliasedType != null)
      {
        _AliasedType.ResolveTypeReferences(contextType, contextInstance);
      }
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
  }
}
