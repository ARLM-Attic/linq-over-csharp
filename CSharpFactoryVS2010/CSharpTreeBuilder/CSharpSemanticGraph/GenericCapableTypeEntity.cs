using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    /// <summary>Backing field for AllTypeParameters property to disallow direct adding or removing.</summary>
    private readonly List<TypeParameterEntity> _AllTypeParameters;

    /// <summary>Backing field for ConstructedGenericTypes property to disallow direct adding or removing.</summary>
    private readonly List<ConstructedGenericTypeEntity> _ConstructedGenericTypes;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericCapableTypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected GenericCapableTypeEntity(string name)
      : base(name)
    {
      _AllTypeParameters = new List<TypeParameterEntity>();
      _ConstructedGenericTypes = new List<ConstructedGenericTypeEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of all type parameters of this type (parent's + own).
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> AllTypeParameters
    {
      get { return _AllTypeParameters.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of the own type parameters of this type (excluding parents' params). 
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> OwnTypeParameters
    {
      get
      {
        return (Parent is GenericCapableTypeEntity
                  ? _AllTypeParameters.Skip((Parent as GenericCapableTypeEntity).AllTypeParameters.Count)
                  : _AllTypeParameters)
          .ToList().AsReadOnly();
      }
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
        return OwnTypeParameters.Count == 0
                 ? Name
                 : string.Format("{0}`{1}", Name, OwnTypeParameters.Count);
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
      get { return AllTypeParameters.Count() > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      _AllTypeParameters.Add(typeParameterEntity);

      // If the type parameter is inherited, then the Parent property is already set, and we leave it as is.
      if (typeParameterEntity.Parent == null)
      {
        typeParameterEntity.Parent = this;
      }

      // A type parameter defined in a nested type can hide a type parameter inherited from parent
      var nameTableEntry = DeclarationSpace[typeParameterEntity.Name];

      if (nameTableEntry != null
        && nameTableEntry.State == NameTableEntryState.Definite
        && nameTableEntry.Entity is TypeParameterEntity
        && ((TypeParameterEntity)nameTableEntry.Entity).Parent != this)
      {
        DeclarationSpace.Redefine(typeParameterEntity);
      }
      else
      {
        DeclarationSpace.Define(typeParameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of the types constructed from this generic type definition.
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<ConstructedGenericTypeEntity> ConstructedGenericTypes
    {
      get { return _ConstructedGenericTypes.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an entity to the list of generic types constructed from this generic type definition.
    /// </summary>
    /// <remarks>
    /// Throws InvalidOperationException for non-generic types.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public void AddConstructedGenericType(ConstructedGenericTypeEntity constructedGenericTypeEntity)
    {
      if (!IsGeneric)
      {
        throw new InvalidOperationException(
          string.Format("AddConstructedGenericType cannot be used on nongeneric type '{0}'", FullyQualifiedName));
      }

      if (AllTypeParameters.Count != constructedGenericTypeEntity.TypeArguments.Count)
      {
        throw new ArgumentException(
          string.Format("Expected a type with '{0}' type arguments, but received one with '{1}' type arguments.",
                        AllTypeParameters.Count, constructedGenericTypeEntity.TypeArguments.Count),
          "constructedGenericTypeEntity");
      }

      _ConstructedGenericTypes.Add(constructedGenericTypeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a constructed generic type if it can be found in the ConstructedGenericTypes list.
    /// </summary>
    /// <param name="typeArguments">The list of type arguments.</param>
    /// <returns>A constructed generic type entity if it was found, null otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public ConstructedGenericTypeEntity GetConstructedGenericType(List<TypeEntity> typeArguments)
    {
      // If the number of type arguments don't match the number of type parameters, that's an error.
      if (typeArguments.Count != AllTypeParameters.Count)
      {
        throw new ArgumentException(string.Format("Expected '{0}' type arguments, but received '{1}' type arguments.",
                                                  AllTypeParameters.Count, typeArguments.Count), "typeArguments");
      }

      ConstructedGenericTypeEntity foundEntity = null;

      // Brute-force search: look at all cached constructed generic types and try to match all type arguments
      foreach (var constructedGenericType in ConstructedGenericTypes)
      {
        int matchedTypeArguments = 0;
        foreach (var typeArgument in constructedGenericType.TypeArguments)
        {
          // If no match, then stop matching type arguments, and get to the next type
          if (typeArgument != typeArguments[matchedTypeArguments])
          {
            break;
          }
          matchedTypeArguments++;
        }

        // If all type arguments were matched then we have found the entity, so stop the search
        if (matchedTypeArguments == typeArguments.Count)
        {
          foundEntity = constructedGenericType;
          break;
        }
      }

      return foundEntity;
    }
  }
}
