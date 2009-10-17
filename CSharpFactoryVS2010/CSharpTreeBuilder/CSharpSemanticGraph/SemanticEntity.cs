using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.Helpers;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of all kinds of nodes in the semantic graph (namespace, type, etc.).
  /// </summary>
  // ================================================================================================
  public abstract class SemanticEntity : ICloneable
  {
    #region State 

    /// <summary>Backing field for SemanticGraph property.</summary>
    private SemanticGraph _SemanticGraph;

    /// <summary>Backing field for SyntaxNodes property.</summary>
    private readonly List<ISyntaxNode> _SyntaxNodes = new List<ISyntaxNode>();

    /// <summary>
    /// A dictionary holding all the entities constructed from this entity by deep copying
    /// and then replacing type parameters with type arguments. 
    /// The key is a TypeParameterMap object describing all type parameters and the corresponding 
    /// type arguments.
    /// </summary>
    private readonly Dictionary<TypeParameterMap, SemanticEntity> _ConstructedEntities
      = new Dictionary<TypeParameterMap,SemanticEntity>(new TypeParameterMapEqualityComparer());

    /// <summary>Backing field for Program property.</summary>
    private Program _Program;



    /// <summary>Gets or sets the parent of this entity.</summary>
    public SemanticEntity Parent { get; set; }

    /// <summary>Gets or sets the reflected metadata (eg. type) that this entity was created from.</summary>
    public object ReflectedMetadata { get; set; }

    /// <summary>Gets or sets the type parameters and type arguments associated with this entity.</summary>
    public TypeParameterMap TypeParameterMap { get; private set; }

    /// <summary>Gets or sets the generic template of this entity.</summary>
    public SemanticEntity TemplateEntity { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity()
    {
      TypeParameterMap = new TypeParameterMap();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity(SemanticEntity source)
    {
      _SemanticGraph = source._SemanticGraph;
      _SyntaxNodes.AddRange(source._SyntaxNodes);
      // ConstructedEntities should not be copied to the clone.
      _Program = source._Program;

      Parent = source.Parent;
      ReflectedMetadata = source.ReflectedMetadata;
      TypeParameterMap = source.TypeParameterMap;
      TemplateEntity = source.TemplateEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a deep copy of the semantic subtree starting at this entity.
    /// </summary>
    /// <returns>The deep clone of this entity and its semantic subtree.</returns>
    // ----------------------------------------------------------------------------------------------
    public virtual object Clone()
    {
      throw new InvalidOperationException("Abstract class cannot be cloned.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is a (direct or indirect) parent of another entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>
    /// True if this entity is a (direct or indirect) parent of another entity, false otherwise.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsParentOf(SemanticEntity entity)
    {
      if (entity == null)
      {
        return false;
      }

      if (entity == this)
      {
        return true;
      }

      return IsParentOf(entity.Parent);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root namespace of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity RootNamespace
    {
      get 
      {
        if (this is RootNamespaceEntity)
        {
          return this as RootNamespaceEntity;
        }

        if (Parent == null)
        {
          return null;
        }

        return Parent.RootNamespace; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the semantic graph that this entity belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticGraph SemanticGraph
    {
      get
      {
        return _SemanticGraph ?? 
          (RootNamespace == null ? null : RootNamespace.SemanticGraph);
      }
      set
      {
        _SemanticGraph = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the program that this entity belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Program Program
    {
      get
      {
        if (_Program != null)
        {
          return _Program;
        }

        if (Parent != null)
        {
          return Parent.Program;
        }

        throw new InvalidOperationException("Cannot determine the program that this entity belongs to.");
      }

      set
      {
        _Program = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<ISyntaxNode> SyntaxNodes
    {
      get { return _SyntaxNodes.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a syntax node to the SyntaxNodes collection.
    /// </summary>
    /// <param name="syntaxNode">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSyntaxNode(ISyntaxNode syntaxNode)
    {
      _SyntaxNodes.Add(syntaxNode);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the entities constructed from this entity 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<SemanticEntity> ConstructedEntities
    {
      get { return _ConstructedEntities.Values; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a semantic entity by type parameters and type arguments.
    /// </summary>
    /// <param name="typeParameterMap">A type parameter map.</param>
    /// <returns>A constructed semantic entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity GetConstructedEntity(TypeParameterMap typeParameterMap)
    {
      if (_ConstructedEntities.ContainsKey(typeParameterMap))
      {
        return _ConstructedEntities[typeParameterMap];
      }

      var constructedEntity = (SemanticEntity)this.Clone();
      constructedEntity.TypeParameterMap = typeParameterMap;
      constructedEntity.TemplateEntity = this;
      _ConstructedEntities.Add(typeParameterMap, constructedEntity);
      
      return constructedEntity;
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      throw new ApplicationException(string.Format("SemanticEntity.AcceptVisitor called on type: {0}", GetType()));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Visits a collection where the items can change during the visiting. 
    /// Therefore first creates a copy of the collection 
    /// and checks before the visiting of a node if it still has its parent.
    /// </summary>
    /// <typeparam name="T">The type of collection items.</typeparam>
    /// <param name="collection">A collection.</param>
    /// <param name="visitor">A semantic graph visitor object.</param>
    // ----------------------------------------------------------------------------------------------
    protected static void VisitMutableCollection<T>(IEnumerable<T> collection, SemanticGraphVisitor visitor)
      where T : SemanticEntity
    {
      foreach (var item in collection.ToArray())
      {
        if (item.Parent != null)
        {
          item.AcceptVisitor(visitor);
        }
      }
    }

    #endregion
  }
}
