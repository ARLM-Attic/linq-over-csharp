using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  public abstract class GenericCapableTypeEntity : TypeEntity, ICanHaveTypeParameters
  {
    #region State

    /// <summary>Backing field for OwnTypeParameters property to disallow direct adding or removing.</summary>
    private readonly List<TypeParameterEntity> _OwnTypeParameters = new List<TypeParameterEntity>();

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericCapableTypeEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected GenericCapableTypeEntity(AccessibilityKind? accessibility, string name)
      : base(accessibility, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericCapableTypeEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected GenericCapableTypeEntity(GenericCapableTypeEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      // Note that TypeParameters of the TemplateEntity are NOT added to the constructed type.
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
          throw new InvalidOperationException("Constructed types don't have type parameters.");
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
          return (DirectGenericTemplate as GenericCapableTypeEntity).OwnTypeParameters;
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
          return (DirectGenericTemplate as GenericCapableTypeEntity).AllTypeParameterCount;
        }

        return AllTypeParameters.Count(); 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters (not including parents' type parameters).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int OwnTypeParameterCount
    {
      get 
      {
        if (IsGenericClone)
        {
          return (DirectGenericTemplate as GenericCapableTypeEntity).OwnTypeParameterCount;
        }

        return _OwnTypeParameters.Count; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      if (IsGenericClone)
      {
        throw new InvalidOperationException("Constructed types don't have type parameters.");
      }

      _OwnTypeParameters.Add(typeParameterEntity);
      typeParameterEntity.Parent = this;
      _DeclarationSpace.Register(typeParameterEntity);
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
        throw new InvalidOperationException("Constructed types don't have type parameters.");
      }

      if (typeParameterEntity != null)
      {
        _OwnTypeParameters.Remove(typeParameterEntity);
        _DeclarationSpace.Unregister(typeParameterEntity);
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
        throw new InvalidOperationException("Constructed types don't have type parameters.");
      }

      return _DeclarationSpace.GetSingleEntity<TypeParameterEntity>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic type (ie. has type parameters).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsGeneric
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
    /// Gets a value indicating whether this is an unbound generic type 
    /// (ie. a generic type definition with no actual type arguments).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsUnboundGeneric
    {
      get 
      {
        return IsGeneric && DirectGenericTemplate == null;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an open type
    /// (ie. is a type that involves type parameters).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsOpen
    {
      get
      {
        return TypeParameterMap.TypeArguments.Any(typeArgument => typeArgument == null || typeArgument.IsOpen);
      }
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
    /// Returns the result of mapping this type with a type parameter map.
    /// </summary>
    /// <param name="typeParameterMap">A map of type parameters and corresponding type arguments.</param>
    /// <returns>A TypeEntity, the result of the mapping.</returns>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity GetMappedType(TypeParameterMap typeParameterMap)
    {
      return IsGenericClone 
        ? DirectGenericTemplate.GetGenericClone(TypeParameterMap.MapTypeArguments(typeParameterMap)) as TypeEntity
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
      var stringBuilder = new StringBuilder();

      if (Parent != null)
      {
        stringBuilder.Append(Parent.ToString());
      }

      if (Parent is RootNamespaceEntity)
      {
        stringBuilder.Append("::");
      }
      else if (Parent is NamespaceEntity)
      {
        stringBuilder.Append('.');
      }
      else if (Parent is TypeEntity)
      {
        stringBuilder.Append('+');
      }

      stringBuilder.Append(Name);

      if (OwnTypeParameterCount > 0)
      {
        stringBuilder.Append('`');
        stringBuilder.Append(OwnTypeParameterCount);

        // Show the type arguments only for bound types.
        if (!IsUnboundGeneric)
        {
          stringBuilder.Append('[');

          bool isFirst = true;
          foreach (var typeParameter in OwnTypeParameters)
          {
            if (isFirst)
            {
              isFirst = false;
            }
            else
            {
              stringBuilder.Append(',');
            }

            var typeArgument = TypeParameterMap[typeParameter];

            stringBuilder.Append(typeArgument == null ? "null" : typeArgument.ToString());
          }

          stringBuilder.Append(']');
        }
      }

      return stringBuilder.ToString();
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

      foreach (var typeParameter in OwnTypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
