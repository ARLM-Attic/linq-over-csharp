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
    #region Private fields

    private readonly ITypeAbstraction _OrigType;
    private readonly List<ITypeAbstraction> _GenericArguments = new List<ITypeAbstraction>();
    private readonly Dictionary<string, ITypeAbstraction> _Interfaces = 
      new Dictionary<string, ITypeAbstraction>();
    private readonly Dictionary<string, ITypeAbstraction> _NestedTypes =
      new Dictionary<string, ITypeAbstraction>();
    private readonly ITypeAbstraction _BaseType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new clone of the specified type.
    /// </summary>
    /// <param name="origType">Original type to clone.</param>
    // --------------------------------------------------------------------------------
    public TypeClone(ITypeAbstraction origType)
    {
      _OrigType = origType;
      foreach (ITypeAbstraction genArg in origType.GetGenericArguments())
        _GenericArguments.Add(genArg);
      foreach (string key in origType.GetInterfaces().Keys)
        _Interfaces.Add(key, origType.GetInterfaces()[key]);
      foreach (string key in origType.GetNestedTypes().Keys)
        _NestedTypes.Add(key, origType.GetNestedTypes()[key]);
      if (origType.BaseType != null)
        _BaseType = new TypeClone(origType.BaseType);
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
      get { return _OrigType.IsOpenType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public int GetArrayRank()
    {
      return _OrigType.GetArrayRank();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction GetElementType()
    {
      return _OrigType.GetElementType();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list representing the generic arguments of a type.
    /// </summary>
    /// <returns>
    /// Arguments of a generic typeor generic type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public List<ITypeAbstraction> GetGenericArguments()
    {
      return _GenericArguments;
    }

    // --------------------------------------------------------------------------------
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
      return _Interfaces;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating, if the current type is a generic type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericParameter
    {
      get { return _OrigType.IsGenericParameter; }
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
    public Dictionary<string, ITypeAbstraction> GetNestedTypes()
    {
      return _NestedTypes;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction GetUnderlyingEnumType()
    {
      return _OrigType.GetUnderlyingEnumType();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { return _OrigType.IsRuntimeType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { return _OrigType.DeclaringUnit; }
    }

    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnmanagedType
    {
      get { return _OrigType.IsUnmanagedType; }
    }

    // --------------------------------------------------------------------------------
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
      get { return _BaseType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction DeclaringType
    {
      get { return _OrigType.DeclaringType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the type, including the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get { return _OrigType.FullName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { return _OrigType.HasElementType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return _OrigType.IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { return _OrigType.IsArray; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return _OrigType.IsClass; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { return _OrigType.IsEnum; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return _OrigType.IsGenericType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return _OrigType.IsGenericTypeDefinition; }
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
    public ITypeAbstraction MakeArrayType(int rank)
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
    public ITypeAbstraction MakePointerType()
    {
      return new PointerType(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return _GenericArguments.Count; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return _OrigType.IsInterface; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return _OrigType.IsNested; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return _OrigType.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return _OrigType.IsPointer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return _OrigType.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return _OrigType.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _OrigType.IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { return _OrigType.IsValueType; }
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
      get { return _OrigType.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public string SimpleName
    {
      get { return _OrigType.SimpleName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ParametrizedName
    {
      get { return TypeBase.GetParametrizedName(this); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _OrigType.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { return _OrigType.Namespace; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Substitutes the specified generic argument of this type with the specified
    /// formal type parameter
    /// </summary>
    /// <param name="index">Zero-based ordinal number of the type parameter.</param>
    /// <param name="type">Formal parameter value.</param>
    // --------------------------------------------------------------------------------
    public void SubstituteArgument(int index, ITypeAbstraction type)
    {
      _GenericArguments[index] = type;
    }

    #endregion
  }
}