// ================================================================================================
// UsingClause.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;
using System.Linq;

namespace CSharpFactory.ProjectModel
{
  // ================================================================================================
  /// <summary>
  /// This class represents a using clause that has been declared either in a
  /// file or in a namespace.
  /// </summary>
  // ================================================================================================
  public sealed class UsingClause : LanguageElement, IUsesResolutionContext
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new using clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="name">Using alias name, if defined, otherwise null or empty.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="typeUsed">Type reference used by this using clause.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingClause(Token token, CSharpSyntaxParser parser, string name, 
      TypeReference typeUsed): 
      base (token, parser, name)
    {
      Resolvers = new ResolutionNodeList();
      ReferencedName = typeUsed;
      HasAlias = true;
      if (string.IsNullOrEmpty(name))
      {
        HasAlias = false;
        Name = typeUsed.FullName;
      }
      IsResolved = false;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the using clause has an alias.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasAlias { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type used by this directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ReferencedName { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of resolvers belonging to this using clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList Resolvers { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the using clause is resolved or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this using clause has been resolved to a namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToNamespace { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this using clause has been resolved to a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToType
    {
      get { return !IsResolvedToNamespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type this using alias has been resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction ResolvingType { get; private set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace this using has been resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ResolvingNamespace { get; private set; }

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
      if (ReferencedName != null)
      {
        ReferencedName.ResolveTypeReferences(contextType, declarationScope, parameterScope);
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
      Resolvers.Clear();
      Resolvers.Merge(resolvers);
      ResolutionNodeBase resolver = Resolvers[0];
      if (resolver is NamespaceResolutionNode)
      {
        IsResolvedToNamespace = true;
        ResolvingNamespace = resolver.FullName;
      }
      else
      {
        IsResolvedToNamespace = false;
        ResolvingType = (resolver as TypeResolutionNode).Resolver;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that this using clause has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SignResolved()
    {
      IsResolved = Resolvers.Count > 0;
      Validate(IsResolved);
    }

    #endregion
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of using clauses.
  /// </summary>
  // ================================================================================================
  public class UsingClauseCollection : ImmutableCollection<UsingClause>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clause having the specified alias name.
    /// </summary>
    /// <param name="key">AliasToken name.</param>
    /// <returns>
    /// Using clause, if found by the alias name; otherwise, null.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public UsingClause this[string key]
    {
      get { return this.FirstOrDefault(item => item.HasAlias && item.Name == key); }
    }
  }
}
