using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type entities.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceOrTypeEntity : SemanticEntity, INamedEntity, IDefinesDeclarationSpace
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceOrTypeEntity(string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }

      Name = name;
      DeclarationSpace = new DeclarationSpace();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual DeclarationSpace DeclarationSpace { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace or type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// Eg. for a class it's the name + number of type params.
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
    /// starting from the root namespace.
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
