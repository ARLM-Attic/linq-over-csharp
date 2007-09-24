using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the characteristic of a type that can be either a .NET
  /// type or a type declared in a compilation unit.
  /// </summary>
  // ==================================================================================
  public interface ITypeCharacteristics
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    int GetArrayRank();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics GetElementType();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the types directly nested into this type.
    /// </summary>
    /// <returns>
    /// Dictionary of nested types keyed by the CLR names of the nested types. Empty
    /// dictionary is retrieved if there is no nested type.
    /// </returns>
    // --------------------------------------------------------------------------------
    Dictionary<string, ITypeCharacteristics> GetNestedTypes();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsRuntimeType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    ReferencedUnit DeclaringUnit { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsUnmanagedType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of this type.
    /// </summary>
    /// <remarks>
    /// If there is no explicit base type for this type, a corresponding reference to
    /// System.Object should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics BaseType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics DeclaringType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the type, including the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    string FullName { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool HasElementType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsAbstract { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsArray { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsClass { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsEnum { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsGenericType { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsGenericTypeDefinition { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes an array type from the current type with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of array type to be created</param>
    /// <returns>
    /// Array type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics MakeArrayType(int rank);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics MakePointerType();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    int TypeParameterCount { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsInterface { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsNested { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsNestedAssembly { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsNestedPrivate { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsNestedPublic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsNotPublic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsPointer { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsPrimitive { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsPublic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsSealed { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsStatic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsValueType { get; }

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
    bool IsVisible { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    string SimpleName { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    string Name { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    string Namespace { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object carrying detailed information about this type.
    /// </summary>
    // --------------------------------------------------------------------------------
     object TypeObject { get; }
  }
}
