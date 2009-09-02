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
    /// <summary>A list of extern alias entities.</summary>
    private readonly List<ExternAliasEntity> _ExternAliases;

    /// <summary>A list of using alias entities.</summary>
    private readonly List<UsingAliasEntity> _UsingAliases;

    /// <summary>A list of using namespace entities.</summary>
    private readonly List<UsingNamespaceEntity> _UsingNamespaces;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity(string name)
      : base(name)
    {
      _ExternAliases = new List<ExternAliasEntity>();
      _UsingAliases = new List<UsingAliasEntity>();
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
      _DeclarationSpace.Register(namespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child namespace by name. 
    /// </summary>
    /// <param name="name">An identifier.</param>
    /// <returns>A child namespace entity, if one found by name. Null if none or more was found.</returns>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity GetChildNamespace(string name)
    {
      return _DeclarationSpace.GetSingleEntity<NamespaceEntity>(name);
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
      _DeclarationSpace.Register(typeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name and number of type parameters.
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <returns>The type with the given name and number of type parameters, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetChildType(string name, int typeParameterCount)
    {
      return _DeclarationSpace.GetSingleEntity<TypeEntity>(name, typeParameterCount);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a child type by name (assuming no type parameters).
    /// </summary>
    /// <param name="name">The name of the type.</param>
    /// <returns>The type with the given name and no type parameters, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetChildType(string name)
    {
      return GetChildType(name, 0);
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
      _UsingNamespaces.Add(usingNamespaceEntity);
      usingNamespaceEntity.Parent = this;
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
    /// Gets a value indicating whether a using namespace was already specified with the same name
    /// and the same lexical scope.
    /// </summary>
    /// <param name="name">A namespace name in string form.</param>
    /// <param name="lexicalScope">A source region.</param>
    /// <returns>True if a using namespace was already specified with the same name and the same lexical scope. 
    /// False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsUsingNamespaceAlreadySpecified(string name, SourceRegion lexicalScope)
    {
      return (from usingNamespace in _UsingNamespaces
              where usingNamespace.NamespaceName == name && usingNamespace.LexicalScope == lexicalScope
              select usingNamespace).Count() > 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of using alias entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<UsingAliasEntity> UsingAliases
    {
      get { return _UsingAliases; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a using alias entity to this namespace.
    /// </summary>
    /// <param name="usingAliasEntity">A using alias entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddUsingAlias(UsingAliasEntity usingAliasEntity)
    {
      _UsingAliases.Add(usingAliasEntity);
      usingAliasEntity.Parent = this;
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
      return (from usingAlias in UsingAliases
             where usingAlias.Alias == aliasName && usingAlias.LexicalScope.Contains(sourcePoint)
             select usingAlias).FirstOrDefault();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a using alias was already specified with the same alias name
    /// and the same lexical scope.
    /// </summary>
    /// <param name="alias">The alias name.</param>
    /// <param name="lexicalScope">A source region.</param>
    /// <returns>True if a using alias was already specified with the same alias name and the same lexical scope. 
    /// False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsUsingAliasAlreadySpecified(string alias, SourceRegion lexicalScope)
    {
      return (from usingAlias in _UsingAliases
              where usingAlias.Alias == alias && usingAlias.LexicalScope == lexicalScope
              select usingAlias).Count() > 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of extern alias entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ExternAliasEntity> ExternAliases
    {
      get { return _ExternAliases; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an extern alias entity to this namespace.
    /// </summary>
    /// <param name="externAliasEntity">An extern alias entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExternAlias(ExternAliasEntity externAliasEntity)
    {
      _ExternAliases.Add(externAliasEntity);
      externAliasEntity.Parent = this;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an extern alias entity that has the given alias name, 
    /// and can affect the resolution of a name at a given source point.
    /// </summary>
    /// <param name="aliasName">An alias name.</param>
    /// <param name="sourcePoint">A source point.</param>
    /// <returns>An extern alias entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasEntity GetExternAliasByNameAndSourcePoint(string aliasName, SourcePoint sourcePoint)
    {
      return (from externAlias in ExternAliases
              where externAlias.Alias == aliasName && externAlias.LexicalScope.Contains(sourcePoint)
              select externAlias).FirstOrDefault();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether an extern alias was already specified with the same alias name
    /// and the same lexical scope.
    /// </summary>
    /// <param name="alias">The alias name.</param>
    /// <param name="lexicalScope">A source region.</param>
    /// <returns>True if an extern alias was already specified with the same alias name and the same lexical scope. 
    /// False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsExternAliasAlreadySpecified(string alias, SourceRegion lexicalScope)
    {
      return (from externAlias in _ExternAliases
              where externAlias.Alias == alias && externAlias.LexicalScope == lexicalScope
              select externAlias).Count() > 0;
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

      foreach (var externAlias in ExternAliases)
      {
        externAlias.AcceptVisitor(visitor);
      }

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
