using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity that can be generic (class, struct, interface, delegate).
  /// </summary>
  /// <remarks>
  /// A generic type declaration, by itself, denotes an unbound generic type that is used as a 'blueprint'
  /// to form many different types, by way of applying type arguments.
  /// (Unbound generic type can only be used within a typeof-expression.)
  /// <p>
  /// Concepts:
  /// <list type="bullet">
  /// <item>A constructed type is a generic type with type arguments provided.</item>
  /// <item>An open type is a type that involves type parameters.</item>
  /// <item>A bound type is a closed constructed type.</item>
  /// </list>
  /// In unsafe code, a type-argument may not be a pointer type. 
  /// Each type argument must satisfy any constraints on the corresponding type parameter.
  /// </p>
  /// </remarks>
  // ================================================================================================
  public abstract class GenericCapableTypeEntity : TypeEntity
  {
    /// <summary>Backing field for TypeParameters property to disallow direct adding or removing.</summary>
    private List<TypeParameterEntity> _TypeParameters;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericCapableTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected GenericCapableTypeEntity()
    {
      _TypeParameters = new List<TypeParameterEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of type parameters of this type. 
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> TypeParameters
    {
      get { return _TypeParameters; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// Eg. for a class it's the name + number of type params; for methods it's the signature; etc.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        return _TypeParameters.Count == 0
                 ? Name
                 : string.Format("{0}`{1}", Name, _TypeParameters.Count);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic type. 
    /// </summary>
    /// <remarks>It's a generic type if it has any type parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsGeneric
    {
      get { return _TypeParameters.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      _TypeParameters.Add(typeParameterEntity);
      typeParameterEntity.Parent = this;
      DeclarationSpace.Define(typeParameterEntity);
    }
  }
}
