using System;
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

    private readonly TypeReference _TypeUsed;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new using clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="name">Using alias name, if defined, otherwise null or empty.</param>
    /// <param name="typeUsed">Type reference used by this using clause.</param>
    // --------------------------------------------------------------------------------
    public UsingClause(Token token, string name, TypeReference typeUsed): 
      base (token, name)
    {
      _TypeUsed = typeUsed;
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
      get { return !String.IsNullOrEmpty(Name); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type used by this directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference TypeUsed
    {
      get { return _TypeUsed; }
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
      if (_TypeUsed == null) return;
      if (Name == String.Empty)
      {
        TypeReference currentType = _TypeUsed;
        while (currentType != null)
        {
          // --- No alias is used, so the type is resolved.
          currentType.ResolutionInfo.Add(
            new ResolutionItem(ResolutionTarget.Namespace, ResolutionMode.SourceType, currentType));
          currentType = currentType.SubType;
#if DIAGNOSTICS
          TypeReference.ResolutionCounter++;
#endif
        }
      }
      else
      {
        _TypeUsed.ResolveTypeReferences(contextType, contextInstance);
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
