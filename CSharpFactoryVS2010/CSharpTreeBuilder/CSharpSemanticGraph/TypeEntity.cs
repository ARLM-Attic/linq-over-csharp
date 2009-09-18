﻿using System.Collections.Generic;
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
    /// <summary>
    /// Backing field for BaseTypeReferences property.
    /// </summary>
    private List<SemanticEntityReference<TypeEntity>> _BaseTypeReferences;

    /// <summary>
    /// Backing field for Members property.
    /// </summary>
    private readonly List<MemberEntity> _Members;

    /// <summary>
    /// Stores array types created from this type. The key is the rank of the array.
    /// </summary>
    private readonly Dictionary<int, ArrayTypeEntity> _ArrayTypes;

    /// <summary>
    /// Backing field for Accessibility property.
    /// </summary>
    protected AccessibilityKind? _Accessibility;

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
      _BaseTypeReferences = new List<SemanticEntityReference<TypeEntity>>();
      _Members = new List<MemberEntity>();
      _ArrayTypes = new Dictionary<int, ArrayTypeEntity>();
      _Accessibility = accessibility;
      IsNew = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the accessibility of the member.
    /// </summary>
    /// <remarks>If the accessibility was not set then a default is returned 
    /// which depends on the type of the containing type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual AccessibilityKind? Accessibility
    {
      get
      {
        if (_Accessibility != null)
        {
          return _Accessibility;
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

      set
      {
        _Accessibility = value;
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
    /// Gets a value indicating whether this type intentionally hides an inherited member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsNew { get; set; }

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
    public virtual void AddMember(MemberEntity memberEntity)
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
    /// Gets a collection of inherited members by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of members to found.</typeparam>
    /// <param name="name">The name of the members to found.</param>
    /// <returns>A collection of the inherited members. Can be empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetInheritedMembers<TEntityType>(string name) where TEntityType : MemberEntity
    {
      return BaseType.GetMembers<TEntityType>(name).Union(BaseType.GetInheritedMembers<TEntityType>(name));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of members by name.
    /// </summary>
    /// <typeparam name="TEntityType">The type of members to found.</typeparam>
    /// <param name="name">The name of the members to found.</param>
    /// <returns>A collection of the found member. Can be empty.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TEntityType> GetMembers<TEntityType>(string name) where TEntityType : MemberEntity
    {
      return _DeclarationSpace.GetEntities<TEntityType>(name);
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
    public TEntityType GetMember<TEntityType>(string name) where TEntityType : MemberEntity
    {
      if (typeof(TEntityType) == typeof(MethodEntity))
      {
        return GetMethod(new Signature(name, 0, null)) as TEntityType;
      }

      return _DeclarationSpace.GetSingleEntity<TEntityType>(name);
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
      return _DeclarationSpace.GetSingleEntity<MethodEntity>(signature);
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
      where TEntityType : MemberEntity, ICanBeExplicitlyImplementedMember
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