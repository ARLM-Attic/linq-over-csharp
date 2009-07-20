using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity
  {
    /// <summary>Backing field for the ChildTypes property.</summary>
    protected List<TypeEntity> _ChildTypes;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity()
    {
      _ChildTypes = new List<TypeEntity>();
      DeclarationSpace = new DeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpace DeclarationSpace { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName 
    { 
      get
      {
        if (Parent != null && Parent is NamespaceOrTypeEntity && !(Parent is RootNamespaceEntity))
        {
          var parentTypeOrNamespaceNode = Parent as NamespaceOrTypeEntity;
          return string.Format("{0}.{1}", parentTypeOrNamespaceNode.FullyQualifiedName, Name);
        }
        return Name;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the namespace or type, prefixed with 'root-namespace-name::'.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FqnWithRoot
    {
      get
      {
        if (Parent != null && Parent is NamespaceOrTypeEntity )
        {
          var parentTypeOrNamespaceNode = Parent as NamespaceOrTypeEntity;
          return parentTypeOrNamespaceNode is RootNamespaceEntity
                   ? string.Format("{0}::{1}", parentTypeOrNamespaceNode.Name, Name)
                   : string.Format("{0}.{1}", parentTypeOrNamespaceNode.FqnWithRoot, Name);
        }
        return Name;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntity> ChildTypes
    {
      get { return _ChildTypes; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list of child types, for unit testing only.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<TypeEntity> ChildTypesForUnitTests
    {
      get { return _ChildTypes; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child type. 
    /// Also sets the child's parent property, and defines child's name in the declaration space.
    /// </summary>
    /// <param name="typeEntity">The child type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AddChildType(TypeEntity typeEntity)
    {
      _ChildTypes.Add(typeEntity);
      typeEntity.Parent = this;
      DeclarationSpace.DefineName(typeEntity.Name, typeEntity);
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
      foreach (var childTypes in _ChildTypes)
      {
        childTypes.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
