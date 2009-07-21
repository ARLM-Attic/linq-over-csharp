namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity()
    {
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
    /// Gets the fully qualified name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName 
    { 
      get
      {
        if (Parent is NamespaceOrTypeEntity)
        {
          return Parent is RootNamespaceEntity
            ? string.Format("{0}::{1}", ((RootNamespaceEntity)Parent).DistinctiveName, DistinctiveName)
            : string.Format("{0}.{1}", ((NamespaceOrTypeEntity)Parent).FullyQualifiedName, DistinctiveName);
        }
        return DistinctiveName;
      }
    }
  }
}
