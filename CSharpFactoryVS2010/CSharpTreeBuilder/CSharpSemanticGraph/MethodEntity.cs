using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method.
  /// </summary>
  // ================================================================================================
  public sealed class MethodEntity : FunctionMemberWithBodyEntity, 
    IOverloadableEntity, ICanBePartial, ICanHaveTypeParameters, ICanBeExplicitlyImplementedMember
  {
    #region State

    /// <summary>Backing field for Parameters property.</summary>
    private readonly List<ParameterEntity> _Parameters = new List<ParameterEntity>();

    /// <summary>Backing field for OwnTypeParameters property to disallow direct adding or removing.</summary>
    private readonly List<TypeParameterEntity> _OwnTypeParameters = new List<TypeParameterEntity>();


    /// <summary>Gets a value indicating whether this method was declared as partial.</summary>
    public bool IsPartial { get; private set; }

    /// <summary>Gets the reference to the return type.</summary>
    public Resolver<TypeEntity> ReturnTypeReference { get; private set; }

    /// <summary>
    /// Gets or sets the reference to the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member.
    /// </summary>
    /// <remarks>
    /// The reference points to a TypeEntity rather then an InterfaceEntity, 
    /// because it can be a ConstructedGenericType as well (if the interface is a generic).
    /// </remarks>
    public Resolver<TypeEntity> InterfaceReference { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">A value indicating whether this method is static.</param>
    /// <param name="isPartial">A value indicating whether this method is partial.</param>
    /// <param name="returnTypeReference">Reference to the return type.</param>
    /// <param name="interfaceReference">
    /// A reference to in interface, if the member is explicitly implemented interface member.
    /// Null otherwise.
    /// </param>
    /// <param name="name">The name of the member.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic, 
      bool isPartial, 
      Resolver<TypeEntity> returnTypeReference,
      Resolver<TypeEntity> interfaceReference,
      string name, 
      bool isAbstract)
      : 
      base(isDeclaredInSource, accessibility, name, isAbstract)
    {
      InterfaceReference = interfaceReference;
      IsPartial = isPartial;
      _IsStatic = isStatic;
      ReturnTypeReference = returnTypeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private MethodEntity(MethodEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      IsPartial = template.IsPartial;

      if (template.ReturnTypeReference != null)
      {
        ReturnTypeReference = (Resolver<TypeEntity>)template.ReturnTypeReference.GetGenericClone(typeParameterMap);
      }

      if (template.InterfaceReference != null)
      {
        InterfaceReference = (Resolver<TypeEntity>)template.InterfaceReference.GetGenericClone(typeParameterMap);
      }

      foreach (var parameter in template.Parameters)
      {
        AddParameter((ParameterEntity) parameter.GetGenericClone(typeParameterMap));
      }

      // Note that TypeParameters of the TemplateEntity are NOT added to the constructed method.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new MethodEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is ParameterEntity)
      {
        AddParameter(entity as ParameterEntity);
      }
      else if (entity is TypeParameterEntity)
      {
        AddTypeParameter(entity as TypeParameterEntity);
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the return type. Null if not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ReturnType
    {
      get
      {
        return ReturnTypeReference != null ? ReturnTypeReference.Target : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ParameterEntity> Parameters
    {
      get { return _Parameters; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a parameter.
    /// </summary>
    /// <param name="parameterEntity">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddParameter(ParameterEntity parameterEntity)
    {
      if (parameterEntity != null)
      {
        if (!IsExplicitlyImplemented)
        {
          UnregisterInParentDeclarationSpace();
        }

        parameterEntity.Parent = this;
        _Parameters.Add(parameterEntity);
        _DeclarationSpace.Register(parameterEntity);

        if (!IsExplicitlyImplemented)
        {
          RegisterInParentDeclarationSpace();
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a parameter from the entity.
    /// </summary>
    /// <param name="parameter">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveParameter(ParameterEntity parameter)
    {
      if (parameter != null)
      {
        if (!IsExplicitlyImplemented)
        {
          UnregisterInParentDeclarationSpace();
        }

        _Parameters.Remove(parameter);
        _DeclarationSpace.Unregister(parameter);

        if (!IsExplicitlyImplemented)
        {
          RegisterInParentDeclarationSpace();
        }
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    /// <remarks>
    /// The signature of a method consists of the name of the method, the number of type parameters 
    /// and the type and kind (value, reference, or output) of each of its formal parameters, 
    /// considered in the order left to right.
    /// The signature of a method does not include the return type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public Signature Signature
    {
      get { return new Signature(Name, OwnTypeParameterCount, Parameters); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of all type parameters of this type (parent's + own).
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> AllTypeParameters
    {
      get
      {
        if (IsGenericClone)
        {
          throw new InvalidOperationException("Constructed methods don't have type parameters.");
        }

        if (Parent is GenericCapableTypeEntity)
        {
          return (Parent as GenericCapableTypeEntity).AllTypeParameters.Concat(_OwnTypeParameters);
        }

        return _OwnTypeParameters;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the own type parameters of this type (excluding parents' params). 
    /// Empty list for non-generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> OwnTypeParameters
    {
      get
      {
        if (IsGenericClone)
        {
          throw new InvalidOperationException("Constructed methods don't have type parameters.");
        }

        return _OwnTypeParameters;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of all type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int AllTypeParameterCount
    {
      get
      {
        if (IsGenericClone)
        {
          return (DirectGenericTemplate as MethodEntity).AllTypeParameterCount;
        }

        return AllTypeParameters.Count();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int OwnTypeParameterCount
    {
      get
      {
        if (IsGenericClone)
        {
          return (DirectGenericTemplate as MethodEntity).OwnTypeParameterCount;
        }

        return _OwnTypeParameters.Count;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic method. 
    /// </summary>
    /// <remarks>It's a generic method if it has any type parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsGeneric
    {
      get
      {
        return IsGenericClone
          ? (DirectGenericTemplate as GenericCapableTypeEntity).IsGeneric
          : AllTypeParameterCount > 0;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this method.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      if (IsGenericClone)
      {
        throw new InvalidOperationException("Constructed methods don't have type parameters.");
      }

      if (!IsExplicitlyImplemented)
      {
        UnregisterInParentDeclarationSpace();
      }

      _OwnTypeParameters.Add(typeParameterEntity);
      typeParameterEntity.Parent = this;
      _DeclarationSpace.Register(typeParameterEntity);

      if (!IsExplicitlyImplemented)
      {
        RegisterInParentDeclarationSpace();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a type parameter entity from the type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity to remove.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      if (IsGenericClone)
      {
        throw new InvalidOperationException("Constructed methods don't have type parameters.");
      }

      if (!IsExplicitlyImplemented)
      {
        UnregisterInParentDeclarationSpace();
      }

      _OwnTypeParameters.Remove(typeParameterEntity);
      _DeclarationSpace.Unregister(typeParameterEntity);

      if (!IsExplicitlyImplemented)
      {
        RegisterInParentDeclarationSpace();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an own type parameter by name.
    /// </summary>
    /// <param name="name">The name of the type parameter to be found.</param>
    /// <returns>A type parameter entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterEntity GetOwnTypeParameterByName(string name)
    {
      if (IsGenericClone)
      {
        throw new InvalidOperationException("Constructed methods don't have type parameters.");
      }

      return _DeclarationSpace.GetSingleEntity<TypeParameterEntity>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters and type arguments associated with this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override TypeParameterMap TypeParameterMap
    {
      get
      {
        if (IsGenericClone)
        {
          return base.TypeParameterMap;
        }

        var thisTypeParameterMap = new TypeParameterMap(OwnTypeParameters);

        if (Parent != null && Parent is GenericCapableTypeEntity)
        {
          thisTypeParameterMap = (Parent as GenericCapableTypeEntity).TypeParameterMap.Extend(thisTypeParameterMap);
        }

        return thisTypeParameterMap;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is an explicitly implemented interface member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicitlyImplemented 
    {
      get 
      { 
        return InterfaceReference != null; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the interface entity whose member is explicitly implemented.
    /// Null if this member is not an explicitly implemented interface member 
    /// or if the reference to the interface entity is not yet resolved.
    /// </summary>
    /// <remarks>
    /// The type is a TypeEntity rather then an InterfaceEntity, 
    /// because it can be a ConstructedGenericType as well (if the interface is a generic).
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public InterfaceEntity Interface
    {
      get
      {
        return InterfaceReference != null ? InterfaceReference.Target as InterfaceEntity : null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get { return true; }
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

      foreach (var parameter in Parameters)
      {
        parameter.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
