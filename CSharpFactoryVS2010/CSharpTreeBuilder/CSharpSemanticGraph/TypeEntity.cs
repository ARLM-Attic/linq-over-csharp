using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.Helpers;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class TypeEntity : NamespaceOrTypeEntity, IMemberEntity
  {
    #region State

    /// <summary>Backing field for BaseTypeReferences property.</summary>
    private List<SemanticEntityReference<TypeEntity>> _BaseTypeReferences = new List<SemanticEntityReference<TypeEntity>>();

    /// <summary>Backing field for Members property.</summary>
    private readonly List<IMemberEntity> _Members = new List<IMemberEntity>();

    /// <summary>
    /// A flag for lazy importing reflected members (expensive operation).
    /// True if the reflected members were already imported, false otherwise.
    /// </summary>
    private bool _ReflectedMembersImported;

    /// <summary>Stores array types created from this type. The key is the rank of the array.</summary>
    private readonly Dictionary<int, ArrayTypeEntity> _ArrayTypes = new Dictionary<int,ArrayTypeEntity>();

    /// <summary>Backing field for IsNew property.</summary>
    private bool _IsNew;


    /// <summary>Gets or sets the factory object needed for lazy importing reflected members.</summary>
    public MetadataImporterSemanticEntityFactory MetadataImporterFactory { get; set; }

    /// <summary>Gets or sets the BuiltInType value of this type entity.</summary>
    public BuiltInType? BuiltInTypeValue { get; set; }

    /// <summary>Gets or sets the declared accessibility of the entity.</summary>
    public AccessibilityKind? DeclaredAccessibility { get; set; }

    /// <summary>Gets or sets the pointer type constructed from this type.</summary>
    public PointerToTypeEntity PointerType { get; set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity(AccessibilityKind? accessibility, string name)
      : base(name)
    {
      DeclaredAccessibility = accessibility;

      _ReflectedMembersImported = false;
      (this as IMemberEntity).IsNew = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity(TypeEntity source)
      : base(source)
    {
      _BaseTypeReferences.AddRange(source._BaseTypeReferences);
      _ReflectedMembersImported = source._ReflectedMembersImported;
      _IsNew = source._IsNew;
      MetadataImporterFactory = source.MetadataImporterFactory;
      BuiltInTypeValue = source.BuiltInTypeValue;
      DeclaredAccessibility = source.DeclaredAccessibility;
      
      // Source members should be retrieved through the Members property (not the _Members field)
      // because that also handles lazy importing,
      // and cloned members should be added through the AddMember method,
      // because that also handles registering into the declaration space.
      foreach (var member in source.Members)
      {
        AddMember((IMemberEntity)member.Clone());
      }
      
      // ArrayTypes should not be cloned.
      // PointerType should not be cloned.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a built in type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsBuiltInType
    {
      get { return BuiltInTypeValue != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an integral type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsIntegralType
    {
      get
      {
        return 
          (BuiltInTypeValue == BuiltInType.Sbyte
          || BuiltInTypeValue == BuiltInType.Byte
          || BuiltInTypeValue == BuiltInType.Short
          || BuiltInTypeValue == BuiltInType.Ushort
          || BuiltInTypeValue == BuiltInType.Int
          || BuiltInTypeValue == BuiltInType.Uint
          || BuiltInTypeValue == BuiltInType.Long
          || BuiltInTypeValue == BuiltInType.Ulong
          || BuiltInTypeValue == BuiltInType.Char);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a floating point type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsFloatingPointType
    {
      get
      {
        return (BuiltInTypeValue == BuiltInType.Float || BuiltInTypeValue == BuiltInType.Double);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a numeric type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsNumericType
    {
      get
      {
        return (BuiltInTypeValue == BuiltInType.Decimal)
          || IsIntegralType
          || IsFloatingPointType;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a simple type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsSimpleType
    {
      get
      {
        return (BuiltInTypeValue == BuiltInType.Bool)
          || IsNumericType;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a nullable type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsNullableType
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of a nullable type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual TypeEntity UnderlyingOfNullableType
    {
      get { return null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the effective accessibility of the entity.
    /// </summary>
    /// <remarks>
    /// If there's no declared accessibility then returns the default accessibility.
    /// If the default cannot be determined (eg. no parent entity) then returns null.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public AccessibilityKind? EffectiveAccessibility
    {
      get
      {
        if (DeclaredAccessibility != null)
        {
          return DeclaredAccessibility;
        }

        // If no declared accessibility then the default has to be returned,
        // which is based on the type of the containing type.
        if (Parent != null)
        {
          // Types declared in compilation units or namespaces default to internal declared accessibility.
          if (Parent is NamespaceEntity)
          {
            return AccessibilityKind.Assembly;
          }

          // Nested types can only be declared in class or struct where the default accessibility is private.
          if (Parent is TypeEntity)
          {
            return AccessibilityKind.Private;
          }
        }

        // If there's no parent, then a default accessibility cannot be determined.
        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is accessible by another entity.
    /// </summary>
    /// <param name="accessingEntity">The accessing entity.</param>
    /// <returns>True if the parameter entity can access this entity, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsAccessibleBy(SemanticEntity accessingEntity)
    {
      // First check the accessibility of the parent type (if any)
      var parentType = Parent as TypeEntity;
      
      if (IsNestedType && !parentType.IsAccessibleBy(accessingEntity))
      {
        return false;
      }
      
      // Then check the accessibility of this entity.
      switch (EffectiveAccessibility)
      {
        case AccessibilityKind.Public:
          return true;

        case AccessibilityKind.Assembly:
          return accessingEntity.Program == this.Program;

        case AccessibilityKind.FamilyOrAssembly:
          return accessingEntity.Program == this.Program || parentType.ContainsInFamily(accessingEntity);

        case AccessibilityKind.Family:
          return parentType.ContainsInFamily(accessingEntity);

        case AccessibilityKind.Private:
          return parentType.IsParentOf(accessingEntity);

        default:
          throw new ApplicationException("Effective accessibility is undefined.");
      }
    }

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
    /// Gets a value indicating whether this type is declared in the body of a type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsNestedType
    {
      get 
      { 
        return (this.Parent != null) && (this.Parent is TypeEntity); 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this type intentionally hides an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IMemberEntity.IsNew 
    {
      get { return _IsNew; }
      set { _IsNew = value; } 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>Nested types are not invocable.</remarks>
    // ----------------------------------------------------------------------------------------------
    bool IMemberEntity.IsInvocable
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
                         select baseType.TargetEntity).ToList();
        
        if (baseTypes.Count == 1)
        { 
          return baseTypes[0]; 
        }

        // If the base type is not found, then return the default base type.
        // Interfaces and System.Object don't have a base type. All other types have.
        if (!IsInterfaceType && BuiltInTypeValue != BuiltInType.Object)
        {
          if (IsEnumType)
          {
            return SemanticGraph.GetEntityByMetadataObject(typeof(System.Enum)) as TypeEntity;
          }
          if (IsStructType)
          {
            return SemanticGraph.GetEntityByMetadataObject(typeof(System.ValueType)) as TypeEntity;
          }
          if (IsDelegateType)
          {
            return SemanticGraph.GetEntityByMetadataObject(typeof(System.MulticastDelegate)) as TypeEntity;
          }
          if (IsClassType)
          {
            return SemanticGraph.GetEntityByMetadataObject(typeof(System.Object)) as TypeEntity;
          }
        }

        return null;
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
    public virtual ReadOnlyCollection<TypeEntity> BaseInterfaces
    {
      get
      {
        return (from baseType in BaseTypeReferences
                where baseType.ResolutionState == ResolutionState.Resolved && baseType.TargetEntity.IsInterfaceType
                select baseType.TargetEntity).ToList().AsReadOnly();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual IEnumerable<IMemberEntity> Members
    {
      get 
      {
        LazyImportReflectedMembers();

        return _Members; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a member. 
    /// Also sets the member's parent property, and defines the member's name in the declaration space.
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AddMember(IMemberEntity memberEntity)
    {
      _Members.Add(memberEntity);
      memberEntity.Parent = this;

      // Register into the declaration space only if it's not an explicitly implemented interface member
      if (!(memberEntity is ICanBeExplicitlyImplementedMember)
        || !(memberEntity as ICanBeExplicitlyImplementedMember).IsExplicitlyImplemented)
      {
        _DeclarationSpace.Register(memberEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a member. 
    /// </summary>
    /// <param name="memberEntity">The member entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveMember(IMemberEntity memberEntity)
    {
      _Members.Remove(memberEntity);
      memberEntity.Parent = null;
      _DeclarationSpace.Unregister(memberEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a member by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of member to found.</typeparam>
    /// <param name="name">The name of the member to found.</param>
    /// <returns>The found member, or null if not found.</returns>
    /// <remarks>If getting a method then assumes zero type parameters and zero parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetMember<TEntityType>(string name)
      where TEntityType : class, IMemberEntity
    {
      LazyImportReflectedMembers();

      if (typeof(TEntityType) == typeof(MethodEntity))
      {
        return GetMethod(new Signature(name, 0, null)) as TEntityType;
      }

      return _DeclarationSpace.GetSingleEntity<TEntityType>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of members by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of members to found.</typeparam>
    /// <param name="name">The name of the members to found.</param>
    /// <returns>A collection of the found members. Can be empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetMembers<TEntityType>(string name)
      where TEntityType : IMemberEntity
    {
      LazyImportReflectedMembers();

      return _DeclarationSpace.GetEntities<TEntityType>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a method by signature.
    /// </summary>
    /// <param name="signature">A signature.</param>
    /// <returns>The found method, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity GetMethod(Signature signature)
    {
      LazyImportReflectedMembers();
      
      return _DeclarationSpace.GetSingleEntity<MethodEntity>(signature);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a method by signature components (name, number of type parameters, parameters).
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="typeParameterCount">The number of type parameters of the method.</param>
    /// <param name="parameterFilters">Any number of parameter filters (ie. type + kind pairs).</param>
    /// <returns>The found method, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity GetMethod(string name, int typeParameterCount, params ParameterFilter[] parameterFilters)
    {
      var parameters = new List<ParameterEntity>();
      
      if (parameterFilters != null)
      {
        foreach (var parameterFilter in parameterFilters)
        {
          var typeReference = new DirectSemanticEntityReference<TypeEntity>(parameterFilter.Type);
          parameters.Add(new ParameterEntity("irrelevant name", typeReference, parameterFilter.Kind));
        }
      }
      
      return GetMethod(new Signature(name, typeParameterCount, parameters));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an explicitly implemented interface member by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of member to found.</typeparam>
    /// <param name="name">The name of the member to found.</param>
    /// <param name="implementedInterface">The interface whose method is implemented.</param>
    /// <returns>The found member, or null if not found.</returns>
    /// <remarks>
    /// Throws AmbiguousDeclarationsException if there are more then one matching entities.
    /// If getting a method then assumes zero type parameters and zero parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public TEntityType GetMember<TEntityType>(string name, TypeEntity implementedInterface)
      where TEntityType : NonTypeMemberEntity, ICanBeExplicitlyImplementedMember
    {
      if (typeof(TEntityType) == typeof(MethodEntity))
      {
        return GetMethod(new Signature(name, 0, null), implementedInterface) as TEntityType;
      }

      var members = (from member in Members
                     where (member is TEntityType)
                           && (member as TEntityType).Interface == implementedInterface
                           && member.Name == name
                     select member as TEntityType).ToList();

      if (members.Count > 1)
      {
        throw new AmbiguousDeclarationsException(members.Cast<INamedEntity>());
      }

      return members.FirstOrDefault();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an explicitly implemented interface method by signature.
    /// </summary>
    /// <param name="signature">A signature.</param>
    /// <param name="implementedInterface">The interface whose method is implemented.</param>
    /// <returns>The found method, or null if not found.</returns>
    /// <remarks>
    /// Throws AmbiguousDeclarationsException if there are more then one matching entities.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity GetMethod(Signature signature, TypeEntity implementedInterface)
    {
      var comparer = new SignatureEqualityComparerForCompleteMatching();

      var methods = (from member in Members
                     where (member is MethodEntity)
                           && (member as MethodEntity).Interface == implementedInterface
                           && comparer.Equals((member as MethodEntity).Signature, signature)
                     select member as MethodEntity).ToList();

      if (methods.Count>1)
      {
        throw new AmbiguousDeclarationsException(methods.Cast<INamedEntity>());
      }

      return methods.FirstOrDefault();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of accessible members by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of members to found.</typeparam>
    /// <param name="name">The name of the members to found.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <returns>A collection of accessible members with a given name. Can be empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetAccessibleMembers<TEntityType>(string name, SemanticEntity accessingEntity)
      where TEntityType : IMemberEntity
    {
      var result = new HashSet<TEntityType>();

      foreach (var entity in GetMembers<TEntityType>(name))
      {
        if (entity.IsAccessibleBy(accessingEntity))
        {
          result.Add(entity);
        }
      }

      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of accessible inherited members by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of members to found.</typeparam>
    /// <param name="name">The name of the members to found.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <returns>A collection of accessible inherited members with a given name. Can be empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetAccessibleInheritedMembers<TEntityType>(
      string name, SemanticEntity accessingEntity)
      where TEntityType : IMemberEntity
    {
      return (BaseType == null)
        ? new List<TEntityType>()
        : BaseType.GetAccessibleMembers<TEntityType>(name, accessingEntity)
          .Union(BaseType.GetAccessibleInheritedMembers<TEntityType>(name, accessingEntity));
    }

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity or any of its base types contain another entity 
    /// (directly or indirectly).
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>
    /// True if this entity or any of its base types contain the parameter entity 
    /// (directly or indirectly), false otherwise.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public bool ContainsInFamily(SemanticEntity entity)
    {
      if (entity == null)
      {
        return false;
      }

      if (entity == this 
        || (entity is TypeEntity && this.IsBaseOf(entity as TypeEntity)))
      {
        return true;
      }

      return ContainsInFamily(entity.Parent);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a (direct or indirect) base type of another type.
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    /// <returns>True if this type is a (direct or indirect) base type of the parameter type, 
    /// false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsBaseOf(TypeEntity typeEntity)
    {
      if (typeEntity == null || typeEntity.BaseType == null)
      {
        return false;
      }

      if (typeEntity.BaseType == this)
      {
        return true;
      }

      return this.IsBaseOf(typeEntity.BaseType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type implements an interface.
    /// </summary>
    /// <param name="typeEntity">An interface entity (can be constructed generic type too).</param>
    /// <returns>True if this type implements the interface entity, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Implements(TypeEntity typeEntity)
    {
      if (typeEntity == null || !typeEntity.IsInterfaceType)
      {
        return false;
      }

      if (BaseInterfaces.Contains(typeEntity))
      {
        return true;
      }

      return (BaseType != null && BaseType.Implements(typeEntity))
        || (BaseInterfaces != null && BaseInterfaces.Any(x => x.Implements(typeEntity)));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies a type parameter map to this type. Replaces all occurrencies of all type parameters 
    /// found in the map with the corresponding type argument.
    /// </summary>
    /// <param name="typeParameterMap">A type parameter map.</param>
    /// <returns>The type that this type is mapped to.</returns>
    // ----------------------------------------------------------------------------------------------
    public virtual TypeEntity ApplyTypeParameterMap(TypeParameterMap typeParameterMap)
    {
      return this;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Imports reflected members if needed. 
    /// That is, if this type was created from metadata and members are not yet imported.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void LazyImportReflectedMembers()
    {
      // Don't have to import if already did or the type is not created from metadata.
      if (_ReflectedMembersImported || ReflectedMetadata == null)
      {
        return;
      }

      if (MetadataImporterFactory == null)
      {
        throw new ApplicationException("No MetadataImporterFactory supplied for lazy importing members.");
      }

      if (!(ReflectedMetadata is Type))
      {
        throw new ApplicationException("ReflectedMetadat does not contain a Type object.");
      }

      MetadataImporterFactory.ImportMembersIntoSemanticGraph(ReflectedMetadata as Type, this);

      _ReflectedMembersImported = true;
    }

    #endregion

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      // Visit members only if those are not subject of lazy importing.
      if (_ReflectedMembersImported || ReflectedMetadata == null)
      {
        foreach (var member in Members)
        {
          (member as SemanticEntity).AcceptVisitor(visitor);
        }
      }
    }

    #endregion
  }
}