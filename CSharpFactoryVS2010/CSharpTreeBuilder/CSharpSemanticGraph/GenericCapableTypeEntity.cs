using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    protected GenericCapableTypeEntity(GenericCapableTypeEntity source)
      : base(source)
    {
      foreach (var typeParameter in source._OwnTypeParameters)
      {
        AddTypeParameter((TypeParameterEntity)typeParameter.Clone());
      }
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
      get { return _OwnTypeParameters; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of all type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int AllTypeParameterCount
    {
      get { return AllTypeParameters.Count(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters (not including parents' type parameters).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int OwnTypeParameterCount
    {
      get { return _OwnTypeParameters.Count; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic type. 
    /// </summary>
    /// <remarks>It's a generic type if it has any type parameters (inherited or own).</remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsGeneric
    {
      get { return AllTypeParameters.Count() > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an unbound generic type 
    /// (ie. a generic type definition with no actual type arguments).
    /// </summary>
    /// <remarks>
    /// An unbound type refers to the entity declared by a type declaration. 
    /// An unbound generic type is not itself a type, and cannot be used as the type of a variable, 
    /// argument or return value, or as a base type. The only construct in which 
    /// an unbound generic type can be referenced is the typeof expression.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsUnbound
    {
      get { return TypeParameterMap.TypeArguments.All(typeEntity => typeEntity == null); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a constructed type
    /// (ie. a type that includes at least one type argument).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsConstructed
    {
      get { return !IsUnbound; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this type.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
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
      return _DeclarationSpace.GetSingleEntity<TypeParameterEntity>(name);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var stringBuilder = new StringBuilder(base.ToString());

      if (OwnTypeParameterCount > 0)
      {
        stringBuilder.Append('`');
        stringBuilder.Append(OwnTypeParameterCount.ToString());
      }

      if (IsConstructed)
      {
        stringBuilder.Append('[');

        bool isFirst = true;

        foreach (var typeArgument in TypeParameterMap.TypeArguments)
        {
          if (isFirst)
          {
            isFirst = false;
          }
          else
          {
            stringBuilder.Append(',');
          }

          stringBuilder.Append(typeArgument == null ? "(null)" : typeArgument.ToString());
        }

        stringBuilder.Append(']');
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
      foreach (var typeParameter in OwnTypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
