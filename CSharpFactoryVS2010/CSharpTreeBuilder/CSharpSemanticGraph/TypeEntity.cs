using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class TypeEntity : NamespaceOrTypeEntity
  {
    /// <summary>Backing field for BaseTypeReferences property.</summary>
    private List<SemanticEntityReference<TypeEntity>> _BaseTypeReferences;

    /// <summary>Backing field for Members property.</summary>
    private readonly List<MemberEntity> _Members;

    /// <summary>Stores array types created from this type. The key is the rank of the array.</summary>
    private readonly Dictionary<int, ArrayTypeEntity> _ArrayTypes;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity(string name)
      : base(name)
    {
      _BaseTypeReferences = new List<SemanticEntityReference<TypeEntity>>();
      _Members = new List<MemberEntity>();
      _ArrayTypes = new Dictionary<int, ArrayTypeEntity>();
      IsPartial = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type was declared as partial.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsReferenceType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsValueType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a pointer type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsPointerType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a class type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsClassType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an interface type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsInterfaceType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a struct type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsStructType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an enum type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsEnumType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a delegate type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsDelegateType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an array type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsArrayType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an alias of another type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsAlias
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypeReferences
    {
      get { return _BaseTypeReferences; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a base type.
    /// </summary>
    /// <param name="typeEntityReference">A type entity reference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddBaseTypeReference(SemanticEntityReference<TypeEntity> typeEntityReference)
    {
      _BaseTypeReferences.Add(typeEntityReference);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Eliminates duplicates in the base type reference list. 
    /// Duplicates are references that point to the same entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void EliminateDuplicateBaseTypeReferences()
    {
      _BaseTypeReferences = BaseTypeReferences.Distinct(new SemanticEntityReferenceEqualityComparer<TypeEntity>()).ToList();
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type entity of this type.
    /// </summary>
    /// <remarks>
    /// It returns an entity, not a reference, so it can be successful only if the base type references are already resolved.
    /// If more than 1 base type reference is resolved to class entity, then null is returned (ambigous).
    /// Always null for interfaces, pointer types, type parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual TypeEntity BaseType
    {
      get
      {
        var baseTypes = (from baseType in BaseTypeReferences
                         where
                           baseType.ResolutionState == ResolutionState.Resolved &&
                           baseType.TargetEntity.IsClassType
                         select baseType.TargetEntity).ToArray();

        return baseTypes.Length == 1 ? baseTypes[0] : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of base types resolved to class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int BaseTypeCount
    {
      get
      {
        return (from baseType in BaseTypeReferences
                where
                  baseType.ResolutionState == ResolutionState.Resolved &&
                  baseType.TargetEntity.IsClassType
                select baseType.TargetEntity).Count();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of base interface entities of this type.
    /// </summary>
    /// <remarks>
    /// It returns the resolved entities, not the references, so it can be successful only if the base type references are already resolved.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<InterfaceEntity> BaseInterfaces
    {
      get
      {
        return (from baseType in BaseTypeReferences
                where baseType.ResolutionState == ResolutionState.Resolved && baseType.TargetEntity is InterfaceEntity
                select baseType.TargetEntity as InterfaceEntity).ToList().AsReadOnly();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of member references.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual IEnumerable<MemberEntity> Members
    {
      get { return _Members; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a member. 
    /// Also sets the member's parent property, and defines member's name in the declaration space.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddMember(MemberEntity memberEntity)
    {
      _Members.Add(memberEntity);
      memberEntity.Parent = this;
      DeclarationSpace.Define(memberEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the nullable type constructed from this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NullableTypeEntity NullableType { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the pointer type constructed from this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PointerToTypeEntity PointerType { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an array type created from this type with the given rank.
    /// </summary>
    /// <param name="rank">The rank of the array to be retreived.</param>
    /// <returns>
    /// An array type created from this type with the given rank. Null if no such array was created so far.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public ArrayTypeEntity GetArrayTypeByRank(int rank)
    {
      return _ArrayTypes.ContainsKey(rank) ? _ArrayTypes[rank] : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an array type to the array type collection of this type.
    /// </summary>
    /// <param name="arrayType">An array type with the this type as underlying type.</param>
    /// <remarks>Throws an error if the array has a different underlying type or 
    /// an array with the same rank already exists in the array collection.</remarks>
    // ----------------------------------------------------------------------------------------------
    public void AddArrayType(ArrayTypeEntity arrayType)
    {
      if (arrayType.UnderlyingType != this)
      {
        throw new ArgumentException(
          string.Format("Only arrays with underlying type '{0}' are accepted, but received underlying type '{1}'.",
                        this.FullyQualifiedName, arrayType.UnderlyingType.FullyQualifiedName), "arrayType");
      }

      if (_ArrayTypes.ContainsKey(arrayType.Rank))
      {
        throw new ApplicationException(string.Format("ArrayType with rank '{0}' was already added.", arrayType.Rank));
      }

      _ArrayTypes.Add(arrayType.Rank, arrayType);
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
      foreach (var member in Members)
      {
        member.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}