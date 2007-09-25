using System;
using System.Collections.Generic;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a type constructed from a declaration in the source code.
  /// </summary>
  /// <remarks>
  /// Types are constructed by using the "*" pointer construction or the
  /// "[]" or "[,...,]" array construction.
  /// </remarks>
  // ==================================================================================
  public abstract class ConstructedType : ITypeCharacteristics
  {
    #region Private fields

    private readonly ITypeCharacteristics _ElementType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constructed type from the specified element type.
    /// </summary>
    /// <param name="elementType">Element type instance.</param>
    // --------------------------------------------------------------------------------
    protected ConstructedType(ITypeCharacteristics elementType)
    {
      _ElementType = elementType;
    }

    #endregion

    #region ITypeCharacteristics Implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public virtual int GetArrayRank()
    {
      throw new ArgumentException("Type is not an array.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics GetElementType()
    {
      return _ElementType;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the types directly nested into this type.
    /// </summary>
    /// <returns>
    /// Dictionary of nested types keyed by the CLR names of the nested types. Empty
    /// dictionary is retrieved if there is no nested type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public Dictionary<string, ITypeCharacteristics> GetNestedTypes()
    {
      return new Dictionary<string, ITypeCharacteristics>();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics GetUnderlyingEnumType()
    {
      throw new InvalidOperationException("Underlying type is not an enum.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { return _ElementType.IsRuntimeType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { return _ElementType.DeclaringUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsUnmanagedType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of this type.
    /// </summary>
    /// <remarks>
    /// Constructed types never have base type
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics BaseType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// Constructed types never have declaring type
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics DeclaringType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the type, including the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get
      {
        return string.IsNullOrEmpty(Namespace) ? Name : Namespace + "." + Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    /// <remarks>
    /// Constructed types always have element type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    /// <remarks>
    /// Constructed types are never abstract.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsArray { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    /// <remarks>
    /// Constructed types are never classes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    /// <remarks>
    /// Constructed types are never enums.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    /// <remarks>
    /// Constructed types are never generic.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    /// <remarks>
    /// Constructed types are never generic definitions.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes an array type from the current type with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of array type to be created</param>
    /// <returns>
    /// Array type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakeArrayType(int rank)
    {
      return new ArrayType(this, rank);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakePointerType()
    {
      return new PointerType(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    /// <remarks>
    /// Constructed types never have type parameters.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    /// <remarks>
    /// Constructed types are never interfaces.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    /// <remarks>
    /// Constructed types are never nested.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedAssembly
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPrivate
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return _ElementType.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsPointer { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPrimitive
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return _ElementType.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    /// <remarks>
    /// Constructed types are always sealed.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type can be accessed by code outside the 
    /// assembly.
    /// </summary>
    /// <value>
    /// true if the current Type is a public type or a public nested type such that 
    /// all the enclosing types are public; otherwise, false.
    /// </value>
    /// <remarks>
    /// Use this property to determine whether a type is part of the public 
    /// interface of a component assembly.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsVisible
    {
      get { return _ElementType.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public string SimpleName
    {
      get { return _ElementType.SimpleName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract string Name { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { return _ElementType.Namespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object carrying detailed information about this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual object TypeObject
    {
      get { return this; }
    }

    #endregion
  }
}
