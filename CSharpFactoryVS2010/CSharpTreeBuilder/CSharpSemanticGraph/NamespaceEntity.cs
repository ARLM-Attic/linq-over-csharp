using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a namespace node in the semantic graph.
  /// </summary>
  // ================================================================================================
  public class NamespaceEntity : NamespaceOrTypeEntity, IHasChildTypes
  {
    /// <summary>A dictionary that stores using alias entities. The key is the alias name.</summary>
    private Dictionary<string, UsingAliasEntity> _UsingAliases;

    /// <summary>A list of using namespace entities.</summary>
    private List<UsingNamespaceEntity> _UsingNamespaces;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity()
    {
      _UsingAliases = new Dictionary<string, UsingAliasEntity>();
      _UsingNamespaces = new List<UsingNamespaceEntity>();

      ChildNamespaces = new List<NamespaceEntity>();
      ChildTypes = new List<TypeEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this namespace is explicitly defined in code 
    /// (not inferred from assembly metadata).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get
      {
        return SyntaxNodes.Count > 0;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child namespaces.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<NamespaceEntity> ChildNamespaces { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child namespaces.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<TypeEntity> ChildTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child namespace. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="namespaceEntity">The child namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildNamespace(NamespaceEntity namespaceEntity)
    {
      ChildNamespaces.Add(namespaceEntity);
      namespaceEntity.Parent = this;
      DeclarationSpace.Define(namespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child namespace by name. 
    /// </summary>
    /// <param name="name">An identifier.</param>
    /// <returns>A child namespace entity, if one found by name. Null if none or more was found.</returns>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity GetChildNamespaceByName(string name)
    {
      var resultSet = from childNamespace in ChildNamespaces 
                      where childNamespace.Name == name 
                      select childNamespace;
      if (resultSet.Count()==1)
      {
        return resultSet.First();
      }
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddChildType(TypeEntity typeEntity)
    {
      ChildTypes.Add(typeEntity);
      typeEntity.Parent = this;
      DeclarationSpace.Define(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name. 
    /// </summary>
    /// <param name="distinctiveName">A distinctive name.</param>
    /// <returns>The type with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetChildTypeByDistinctiveName(string distinctiveName)
    {
      if (DeclarationSpace.IsNameDefined(distinctiveName))
      {
        var nameTableEntry = DeclarationSpace[distinctiveName];
        if (nameTableEntry.State == NameTableEntryState.Definite && nameTableEntry.Entity is TypeEntity)
        {
          return nameTableEntry.Entity as TypeEntity;
        }
      }
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of using namespaces entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<UsingNamespaceEntity> UsingNamespaces
    {
      get { return _UsingNamespaces; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a using namespace entity to this namespace.
    /// </summary>
    /// <param name="usingNamespaceEntity">A using namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddUsingNamespace(UsingNamespaceEntity usingNamespaceEntity)
    {
      if (_UsingNamespaces.Contains(usingNamespaceEntity))
      {
        throw new ApplicationException(string.Format(
                                         "The using directive for '{0}' appeared previously in this namespace.",
                                         usingNamespaceEntity.NamespaceReference.SyntaxNode.TypeTags));
      }

      _UsingNamespaces.Add(usingNamespaceEntity);
      usingNamespaceEntity.Parent = this;

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a given namespace name 
    /// was already specified in this namespace as a using namespace directive.
    /// </summary>
    /// <param name="namespaceName">A namespace name as string.</param>
    /// <returns>True if the namespace name was already specified, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsUsingNamespaceNameAlreadySpecified(string namespaceName)
    {
      return (from usingNamespace in _UsingNamespaces
              where usingNamespace.NamespaceReference.SyntaxNode.TypeTags.ToString() == namespaceName
              select usingNamespace).Count() > 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the iterate-only collection of those using namespace entities 
    /// that can affect the resolution of a name at a given source point.
    /// </summary>
    /// <param name="sourcePoint">A source point.</param>
    /// <returns>A iterate-only collection of using namespace entities.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<UsingNamespaceEntity> GetUsingNamespacesBySourcePoint(SourcePoint sourcePoint)
    {
      return from usingNamespace in _UsingNamespaces
             where usingNamespace.LexicalScope.Contains(sourcePoint)
             select usingNamespace;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of using alias entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<UsingAliasEntity> UsingAliases
    {
      get { return _UsingAliases.Values; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a using alias entity to this namespace.
    /// </summary>
    /// <param name="usingAliasEntity">A using alias entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddUsingAlias(UsingAliasEntity usingAliasEntity)
    {
      if (_UsingAliases.ContainsKey(usingAliasEntity.Alias))
      {
        throw new ApplicationException(string.Format("The using alias '{0}' appeared previously in this namespace",
                                                     usingAliasEntity.Alias));
      }

      _UsingAliases.Add(usingAliasEntity.Alias, usingAliasEntity);
      usingAliasEntity.Parent = this;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a given name was already defined as a using alias name.
    /// </summary>
    /// <param name="aliasName">An alias name.</param>
    /// <returns>True if the name was already defined as a using alias name, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsUsingAliasNameDefined(string aliasName)
    {
      return _UsingAliases.ContainsKey(aliasName);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a using alias entity that has the given alias name, 
    /// and can affect the resolution of a name at a given source point.
    /// </summary>
    /// <param name="aliasName">An alias name.</param>
    /// <param name="sourcePoint">A source point.</param>
    /// <returns>A using alias entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingAliasEntity GetUsingAliasByNameAndSourcePoint(string aliasName, SourcePoint sourcePoint)
    {
      if (_UsingAliases.ContainsKey(aliasName))
      {
        var usingAliasEntity = _UsingAliases[aliasName];
        if (usingAliasEntity.LexicalScope.Contains(sourcePoint))
        {
          return usingAliasEntity;
        }
      }
      return null;
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);

      foreach (var usingNamespace in UsingNamespaces)
      {
        usingNamespace.AcceptVisitor(visitor);
      }

      foreach (var usingAlias in UsingAliases)
      {
        usingAlias.AcceptVisitor(visitor);
      }

      foreach (var childNamespace in ChildNamespaces)
      {
        childNamespace.AcceptVisitor(visitor);
      }

      foreach (var childTypes in ChildTypes)
      {
        childTypes.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
