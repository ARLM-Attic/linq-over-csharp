using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System.Collections.ObjectModel;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type parameter (aka generic parameter) entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterEntity : TypeEntity
  {
    #region State

    /// <summary>Backing field for TypeReferenceConstraints property.</summary>
    private readonly List<SemanticEntityReference<TypeEntity>> _TypeReferenceConstraints
      = new List<SemanticEntityReference<TypeEntity>>();


    /// <summary>Gets a value specifying whether this type parameter has a default constructor ("new()") constraint.</summary>
    public bool HasDefaultConstructorConstraint { get; set; }

    /// <summary>Gets a value specifying whether this type parameter has a "class" constraint.</summary>
    public bool HasReferenceTypeConstraint { get; set; }

    /// <summary>Gets a value specifying whether this type parameter has a "struct" constraint.</summary>
    public bool HasNonNullableValueTypeConstraint { get; set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterEntity(string name)
      : base(null, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are specified with type name.
    /// </summary>
    /// <remarks>
    /// Before type name resolution it is not possible to distinguish 
    /// class types, interface types and type parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<SemanticEntityReference<TypeEntity>> TypeReferenceConstraints
    {
      get { return _TypeReferenceConstraints; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a constraint that is given with a type reference.
    /// </summary>
    /// <param name="typeReference">A type reference.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeReferenceConstraint(SemanticEntityReference<TypeEntity> typeReference)
    {
      if (typeReference != null)
      {
        _TypeReferenceConstraints.Add(typeReference);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to a class type.
    /// </summary>
    /// <remarks>
    /// In a semantically correct program there can be at most 1 class type among the constraints,
    /// but the data model must be able to represent erroneous constraints too.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ClassEntity> ClassTypeConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity is ClassEntity
               select typeReference.TargetEntity as ClassEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the class type constraint.
    /// </summary>
    /// <remarks>
    /// If none or more than one constraint is a class type constraint the returns null.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public ClassEntity ClassTypeConstraint
    {
      get
      {
        var classTypeConstraints = (from typeReference in _TypeReferenceConstraints
                                    where typeReference.ResolutionState == ResolutionState.Resolved
                                          && typeReference.TargetEntity is ClassEntity
                                    select typeReference.TargetEntity as ClassEntity).ToList();

        return classTypeConstraints.Count == 1 ? classTypeConstraints[0] : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to a type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> TypeParameterConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity is TypeParameterEntity
               select typeReference.TargetEntity as TypeParameterEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only list of constraints that are resolved to an interface type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<InterfaceEntity> InterfaceTypeConstraints
    {
      get
      {
        return from typeReference in _TypeReferenceConstraints
               where typeReference.ResolutionState == ResolutionState.Resolved
                     && typeReference.TargetEntity is InterfaceEntity
               select typeReference.TargetEntity as InterfaceEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an unbound generic type 
    /// (ie. a generic type definition with no actual type arguments).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsUnbound
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an open type
    /// (ie. is a type that involves type parameters).
    /// </summary>
    /// <remarks>
    /// A type parameter defines an open type.
    /// An array type is an open type if and only if its element type is an open type.
    /// A constructed type is an open type if and only if one or more of its type arguments is 
    /// an open type. A constructed nested type is an open type if and only if one or more of 
    /// its type arguments or the type arguments of its containing type(s) is an open type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsOpen
    {
      get
      {
        return true;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base class entity of this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override ClassEntity BaseClass
    {
      get
      {
        // The effective base class of a type parameter T is defined as follows:
        
        // If T has no primary constraints or type parameter constraints, its effective base class is object.
        if (ClassTypeConstraint == null && !HasReferenceTypeConstraint && !HasNonNullableValueTypeConstraint
          && TypeParameterConstraints.Count() == 0)
        {
          return SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Object) as ClassEntity;
        }

        // If T has the value type constraint, its effective base class is System.ValueType.
        if (HasNonNullableValueTypeConstraint)
        {
          return SemanticGraph.GetEntityByMetadataObject(typeof(System.ValueType)) as ClassEntity;
        }

        // If T has a class-type constraint C but no type-parameter constraints, its effective base class is C.
        if (ClassTypeConstraint != null && TypeParameterConstraints.Count() == 0)
        {
          return ClassTypeConstraint;
        }

        // If T has no class-type constraint but has one or more type-parameter constraints, 
        if (ClassTypeConstraint == null && TypeParameterConstraints.Count() > 0)
        {
          // its effective base class is the most encompassed type (§6.4.2) 
          // in the set of effective base classes of its type-parameter constraints. 
          // The consistency rules ensure that such a most encompassed type exists.

          var typeConverter = new TypeConverter();
          return typeConverter.GetMostEncompassedType(GetBaseClassesOfTypeParameters().Cast<TypeEntity>()) as ClassEntity;
        }

        // If T has both a class-type constraint and one or more type-parameter constraints, 
        if (ClassTypeConstraint != null && TypeParameterConstraints.Count() > 0)
        {
          // its effective base class is the most encompassed type (§6.4.2)
          // in the set consisting of the class-type constraint of T 
          // and the effective base classes of its type-parameter constraints. 
          // The consistency rules ensure that such a most encompassed type exists.

          var baseClasses = GetBaseClassesOfTypeParameters();
          if (ClassTypeConstraint != null)
          {
            baseClasses.Add(ClassTypeConstraint);
          }

          var typeConverter = new TypeConverter();
          return typeConverter.GetMostEncompassedType(baseClasses.Cast<TypeEntity>()) as ClassEntity;
        }

        // If T has the reference type constraint but no class-type constraints, its effective base class is object.
        if (HasReferenceTypeConstraint && ClassTypeConstraint == null && !HasNonNullableValueTypeConstraint)
        {
          return SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Object) as ClassEntity;
        }

        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of base interface entities of this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override ReadOnlyCollection<InterfaceEntity> BaseInterfaces
    {
      get
      {
        // The effective interface set of a type parameter T is defined as follows:
        // If T has no secondary-constraints, its effective interface set is empty.
        // If T has interface-type constraints but no type-parameter constraints, its effective interface set is its set of interface-type constraints.
        // If T has no interface-type constraints but has type-parameter constraints, its effective interface set is the union of the effective interface sets of its type-parameter constraints.
        // If T has both interface-type constraints and type-parameter constraints, its effective interface set is the union of its set of interface-type constraints and the effective interface sets of its type-parameter constraints.

        return null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the result of mapping this type with a type parameter map.
    /// </summary>
    /// <param name="typeParameterMap">A map of type parameters and corresponding type arguments.</param>
    /// <returns>A TypeEntity, the result of the mapping.</returns>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity GetMappedType(TypeParameterMap typeParameterMap)
    {
      return (typeParameterMap.ContainsTypeParameter(this))
        ? (typeParameterMap[this] ?? this)
        : this;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return (Parent is TypeEntity) && (Parent as TypeEntity).IsUnbound
        ? Parent.ToString() + "." + Name
        : Name;
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion


    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the set of base classes of the type-parameter constraints.
    /// </summary>
    /// <returns>The set of base classes of the type-parameter constraints.</returns>
    // ----------------------------------------------------------------------------------------------
    private HashSet<ClassEntity> GetBaseClassesOfTypeParameters()
    {
      var baseClassesOfTypeParameters = new HashSet<ClassEntity>();

      foreach (var typeParameterEntity in TypeParameterConstraints)
      {
        if (typeParameterEntity.BaseClass != null)
        {
          baseClassesOfTypeParameters.Add(typeParameterEntity.BaseClass);
        }
      }

      return baseClassesOfTypeParameters;
    }

    #endregion
  }
}
