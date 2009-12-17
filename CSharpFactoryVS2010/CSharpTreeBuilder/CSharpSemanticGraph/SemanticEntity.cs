using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of all kinds of nodes in the semantic graph (namespace, type, etc.).
  /// </summary>
  // ================================================================================================
  public abstract class SemanticEntity : ISemanticEntity
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
    private readonly Dictionary<TypeParameterMap, IGenericCloneSupport> _GenericClones
      = new Dictionary<TypeParameterMap, IGenericCloneSupport>(new TypeParameterMapEqualityComparer());

    /// <summary>Backing field for Program property.</summary>
    private Program _Program;
    
    /// <summary>Backing field for TypeParameterMap property.</summary>
    protected TypeParameterMap _TypeParameterMap;
    

    /// <summary>Gets or sets the parent of this entity.</summary>
    public ISemanticEntity Parent { get; set; }

    /// <summary>Gets or sets the reflected metadata (eg. type) that this entity was created from.</summary>
    public object ReflectedMetadata { get; set; }

    /// <summary>Gets or sets the generic template of this entity.</summary>
    public IGenericCloneSupport DirectGenericTemplate { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity()
    {
      _TypeParameterMap = new TypeParameterMap();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity(SemanticEntity template, TypeParameterMap typeParameterMap)
    {
      _SemanticGraph = template._SemanticGraph;
      _SyntaxNodes.AddRange(template._SyntaxNodes);
      // ConstructedEntities should not be copied to the clone.
      _Program = template._Program;
      _TypeParameterMap = typeParameterMap;

      Parent = template.Parent;
      ReflectedMetadata = template.ReflectedMetadata;
      DirectGenericTemplate = template;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected virtual SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      throw new ApplicationException("Abstract SemanticEntity cannot be constructed.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a semantic entity constructed from this entity by replacing type parameters 
    /// with type arguments. Gets the entity from cache or creates a new one.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public IGenericCloneSupport GetGenericClone(TypeParameterMap typeParameterMap)
    {
      IGenericCloneSupport constructedEntity;

      if (_GenericClones.ContainsKey(typeParameterMap))
      {
        constructedEntity = _GenericClones[typeParameterMap];
      }
      else
      {
        constructedEntity = ConstructNew(typeParameterMap);

        _GenericClones.Add(typeParameterMap, constructedEntity);
      }

      return constructedEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AddChild(ISemanticEntity entity)
    {
      throw new ApplicationException(string.Format("Unexpected child entity: '{0}' of type: '{1}'", entity, entity.GetType()));
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
    public bool IsParentOf(ISemanticEntity entity)
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
    /// Gets the type parameters and type arguments associated with this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual TypeParameterMap TypeParameterMap 
    {
      get 
      {
        if (!_TypeParameterMap.IsEmpty)
        {
          return _TypeParameterMap;
        }

        if (Parent != null)
        {
          return Parent.TypeParameterMap;
        }

        return TypeParameterMap.Empty;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a constructed entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsGenericClone
    {
      get
      {
        return DirectGenericTemplate != null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first generic template in the chain of template->clone relationships,
    /// where none of the type parameters were bound.
    /// Null if this entity was not constructed from another entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IGenericCloneSupport OriginalGenericTemplate
    {
      get
      {
        return IsGenericClone && DirectGenericTemplate.DirectGenericTemplate != null
                 ? DirectGenericTemplate.OriginalGenericTemplate
                 : DirectGenericTemplate;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the entities constructed from this entity 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<IGenericCloneSupport> GenericClones
    {
      get { return _GenericClones.Values; }
    }

    #region Protected helpers

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers this entity in the parent's declaration space
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected void RegisterInParentDeclarationSpace()
    {
      if (!IsGenericClone && Parent != null && Parent is IDefinesDeclarationSpace)
      {
        (Parent as IDefinesDeclarationSpace).AddDeclaration(this as INamedEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Unregisters this entity in the parent's declaration space
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected void UnregisterInParentDeclarationSpace()
    {
      if (!IsGenericClone && Parent != null && Parent is IDefinesDeclarationSpace)
      {
        (Parent as IDefinesDeclarationSpace).RemoveDeclaration(this as INamedEntity);
      }
    }

    #endregion

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
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
