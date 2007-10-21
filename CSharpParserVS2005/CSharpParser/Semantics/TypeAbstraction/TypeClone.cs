using System.Collections.Generic;
using CSharpParser.ProjectModel;
using CSharpParser.Semantics;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents an type that is created from an other by cloning.
  /// </summary>
  // ==================================================================================
  public sealed class TypeClone : ITypeAbstraction
  {
    #region Lifecycle methods

    public TypeClone(ITypeAbstraction origType)
    {

    }

    #endregion

    #region ITypeAbstraction implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if a type is open or not.
    /// </summary>
    /// <remarks>
    /// A type is open, if directly or indireclty references to a type parametes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsOpenType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public int GetArrayRank()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction GetElementType()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets a list representing the generic arguments of a type.
    /// </summary>
    /// <returns>
    /// Arguments of a generic typeor generic type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public List<ITypeAbstraction> GetGenericArguments()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the list of interfaces implemented by this type.
    /// </summary>
    /// <returns>
    /// List ofinterfaces implemented by this type.
    /// </returns>
    /// <remarks>
    /// Retrieves all interfaces implemented by directly or indirectly.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Dictionary<string, ITypeAbstraction> GetInterfaces()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the flag indicating, if the current type is a generic type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericParameter
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the types directly nested into this type.
    /// </summary>
    /// <returns>
    /// Dictionary of nested types keyed by the CLR names of the nested types. Empty
    /// dictionary is retrieved if there is no nested type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public Dictionary<string, ITypeAbstraction> GetNestedTypes()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction GetUnderlyingEnumType()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnmanagedType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the base type of this type.
    /// </summary>
    /// <remarks>
    /// If there is no explicit base type for this type, a corresponding reference to
    /// System.Object should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction BaseType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction DeclaringType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the fully qualified name of the type, including the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Makes an array type from the current type with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of array type to be created</param>
    /// <returns>
    /// Array type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction MakeArrayType(int rank)
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction MakePointerType()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { throw new System.NotImplementedException(); }
    }

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
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public string SimpleName
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ParametrizedName
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { throw new System.NotImplementedException(); }
    }

    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { throw new System.NotImplementedException(); }
    }

    #endregion
  }
}