namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity, IDefinesDeclarationSpace
  {
    /// <summary>Backing field for DeclarationSpace property.</summary>
    private readonly DeclarationSpace _DeclarationSpace;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity()
    {
      _DeclarationSpace = new DeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual DeclarationSpace DeclarationSpace
    {
      get { return _DeclarationSpace; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// Eg. for a class it's the name + number of type params; for methods it's the signature; etc.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual string DistinctiveName
    {
      get { return Name; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the namespace or type, 
    /// that uniquely identifies the namespace or type amongst all others.
    /// </summary>
    /// <remarks>
    /// The fully qualified name of N is the complete hierarchical path of identifiers that lead to N,
    /// starting from the global namespace.
    /// However, in our implementation the root namespace name is also part of the fully qualified name,
    /// to allow storing all namespace and type names in a single collection regardless of the root namespace.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual string FullyQualifiedName 
    { 
      get
      {
        if (Parent is NamespaceOrTypeEntity)
        {
          return Parent is RootNamespaceEntity
            ? DistinctiveName
            : string.Format("{0}.{1}", ((NamespaceOrTypeEntity)Parent).FullyQualifiedName, DistinctiveName);
        }
        return DistinctiveName;
      }
    }
  }
}
