using System;
using System.Collections.Generic;
using System.Reflection;
using CSharpFactory.ProjectModel;

namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents a .NET binary type described by a System.Type instance.
  /// </summary>
  /// <remarks>This class implements the ITypeCharactersitic interface.</remarks>
  // ==================================================================================
  public sealed class NetBinaryType : ITypeAbstraction, IEquatable<NetBinaryType>
  {
    #region Private fields

    private readonly Type _TypeObject;
    private readonly ReferencedUnit _AssemblyRef;
    private readonly ITypeAbstraction _BaseType;
    private readonly ITypeAbstraction _DeclaringType;
    private readonly ITypeAbstraction _ElementType;
    private List<ITypeAbstraction> _GenericArguments;
    private Dictionary<string, ITypeAbstraction> _NestedTypeDictionary;
    private Dictionary<string, ITypeAbstraction> _InterfacesDictionary;

    #endregion

    #region Static public members

    /// <summary>Represents the System.Object type</summary>
    public static readonly NetBinaryType Object = new NetBinaryType(typeof(Object));

    /// <summary>Represents the System.ValueType type</summary>
    public static readonly NetBinaryType ValueType = new NetBinaryType(typeof(ValueType));

    /// <summary>Represents the System.Boolean type</summary>
    public static readonly NetBinaryType Boolean = new NetBinaryType(typeof(Boolean));

    /// <summary>Represents the System.Byte type</summary>
    public static readonly NetBinaryType Byte = new NetBinaryType(typeof(Byte));

    /// <summary>Represents the System.SByte type</summary>
    public static readonly NetBinaryType SByte = new NetBinaryType(typeof(SByte));

    /// <summary>Represents the System.Char type</summary>
    public static readonly NetBinaryType Char = new NetBinaryType(typeof(Char));

    /// <summary>Represents the System.Decimal type</summary>
    public static readonly NetBinaryType Decimal = new NetBinaryType(typeof(Decimal));

    /// <summary>Represents the System.Double type</summary>
    public static NetBinaryType Double = new NetBinaryType(typeof(Double));

    /// <summary>Represents the System.Singe type</summary>
    public static readonly NetBinaryType Single = new NetBinaryType(typeof(Single));

    /// <summary>Represents the System.Int16 type</summary>
    public static readonly NetBinaryType Int16 = new NetBinaryType(typeof(Int16));

    /// <summary>Represents the System.Int32 type</summary>
    public static readonly NetBinaryType Int32 = new NetBinaryType(typeof(Int32));

    /// <summary>Represents the System.Int64 type</summary>
    public static readonly NetBinaryType Int64 = new NetBinaryType(typeof(Int64));

    /// <summary>Represents the System.UInt16 type</summary>
    public static readonly NetBinaryType UInt16 = new NetBinaryType(typeof(UInt16));

    /// <summary>Represents the System.Int32 type</summary>
    public static readonly NetBinaryType UInt32 = new NetBinaryType(typeof(UInt32));

    /// <summary>Represents the System.Int64 type</summary>
    public static readonly NetBinaryType UInt64 = new NetBinaryType(typeof(UInt64));

    /// <summary>Represents the System.String type</summary>
    public static readonly NetBinaryType String = new NetBinaryType(typeof(String));

    /// <summary>Represents the INullable{T} type</summary>
    public static readonly NetBinaryType Nullable = new NetBinaryType(typeof(Nullable<>));

    /// <summary>Represents an empty array of types.</summary>
    public static readonly List<ITypeAbstraction> EmptyTypes = 
      new List<ITypeAbstraction>();

    /// <summary>Represents an empty dictionary of types.</summary>
    public static readonly Dictionary<string, ITypeAbstraction> EmptyTypeDictionary =
       new Dictionary<string, ITypeAbstraction>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this class to represent the specified .NET binary
    /// type.
    /// </summary>
    /// <param name="typeObject">.NET binary type represented by this instance.</param>
    // --------------------------------------------------------------------------------
    public NetBinaryType(Type typeObject)
    {
      if (typeObject == null)
      {
        throw new ArgumentNullException();
      }
      _TypeObject = typeObject;
      _AssemblyRef = new ReferencedAssembly(typeObject.Assembly);
      _BaseType = typeObject.BaseType == null
                    ? Object
                    : new NetBinaryType(typeObject.BaseType);
      _DeclaringType = typeObject.DeclaringType == null
                         ? null
                         : new NetBinaryType(typeObject.DeclaringType);
      if (typeObject.HasElementType)
      {
        _ElementType = new NetBinaryType(typeObject.GetElementType());
      }
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
      get { return IsGenericTypeDefinition; }
    }

    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public int GetArrayRank()
    {
      return _TypeObject.GetArrayRank();
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
      return _ElementType;
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
      if (_GenericArguments == null)
      {
        _GenericArguments = new List<ITypeAbstraction>();
        Type[] genericArguments = _TypeObject.GetGenericArguments();
        foreach (Type type in genericArguments)
        {
          _GenericArguments.Add(new NetBinaryType(type));
        }
      }
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
      if (_InterfacesDictionary == null)
      {
        _InterfacesDictionary = new Dictionary<string, ITypeAbstraction>();
        Type[] interfaces = _TypeObject.GetInterfaces();
        foreach (Type type in interfaces)
        {
          NetBinaryType nbType = new NetBinaryType(type);
          _InterfacesDictionary.Add(nbType.ParametrizedName, nbType);
        }
      }
      return _InterfacesDictionary;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating, if the current type is a generic type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericParameter
    {
      get { return _TypeObject.IsGenericParameter; }
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
      if (_NestedTypeDictionary == null)
      {
        Type[] nestedTypes =
          _TypeObject.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
        _NestedTypeDictionary = new Dictionary<string, ITypeAbstraction>();
        foreach(Type type in nestedTypes)
          _NestedTypeDictionary.Add(type.Name, new NetBinaryType(type));
      }
      return _NestedTypeDictionary;
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
      if (!IsEnum)
        throw new InvalidOperationException("Underlying type is not an enum.");
      return new NetBinaryType(Enum.GetUnderlyingType(_TypeObject));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    /// <remarks>Always returns true.</remarks>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { return _AssemblyRef; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnmanagedType
    {
      get
      {
        if (IsPointer || IsEnum) return true;
        if (IsInterface || IsClass || IsGenericType) return false;
        // --- Check for native unmanaged types
        if (_TypeObject == typeof (bool) ||
          _TypeObject == typeof (byte) ||
          _TypeObject == typeof (sbyte) ||
          _TypeObject == typeof (short) ||
          _TypeObject == typeof (ushort) ||
          _TypeObject == typeof (int) ||
          _TypeObject == typeof (uint) ||
          _TypeObject == typeof (long) ||
          _TypeObject == typeof (ulong) ||
          _TypeObject == typeof (char) ||
          _TypeObject == typeof (float) ||
          _TypeObject == typeof (double) ||
          _TypeObject == typeof (decimal)) 
          return true;

        // --- At this point we have a non-generic struct
        foreach (FieldInfo fi in _TypeObject.GetFields(
          BindingFlags.Public|BindingFlags.NonPublic|
          BindingFlags.Instance|BindingFlags.Static))
        {
          if (!new NetBinaryType(fi.FieldType).IsUnmanagedType) return false;
        }
        return true;
      }
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
      get { return _DeclaringType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the type, including the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get { return _TypeObject.FullName; }
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
      get { return _TypeObject.HasElementType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return _TypeObject.IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { return _TypeObject.IsArray; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return _TypeObject.IsClass; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { return _TypeObject.IsEnum; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return _TypeObject.IsGenericType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return _TypeObject.IsGenericTypeDefinition; }
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
      if (rank == 1) return new NetBinaryType(_TypeObject.MakeArrayType());
      return new NetBinaryType(_TypeObject.MakeArrayType(rank));
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
      return new NetBinaryType(_TypeObject.MakePointerType());
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return _TypeObject.GetGenericArguments().Length; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return _TypeObject.IsInterface; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return _TypeObject.IsNested; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return _TypeObject.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return _TypeObject.IsPointer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return _TypeObject.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return _TypeObject.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _TypeObject.IsAbstract && _TypeObject.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { return _TypeObject.IsValueType; }
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
      get { return _TypeObject.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public string SimpleName
    {
      get
      {
        int pos = _TypeObject.Name.IndexOf('`');
        if (pos < 0) return _TypeObject.Name;
        return _TypeObject.Name.Substring(0, pos);
      }
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
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _TypeObject.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { return _TypeObject.Namespace; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if two NetBinaryType instances refer to the same type.
    /// </summary>
    /// <param name="obj">Object to check the equality with this instance.</param>
    /// <returns>
    /// True, if the two objects are equal; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Equals(object obj)
    {
      NetBinaryType type = obj as NetBinaryType;
      if (type == null) return false;
      return _TypeObject.Equals(type._TypeObject);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if two NetBinaryType instances refer to the same type.
    /// </summary>
    /// <param name="other">Object to check the equality with this instance.</param>
    /// <returns>
    /// True, if the two objects are equal; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool Equals(NetBinaryType other)
    {
      return _TypeObject.Equals(other._TypeObject);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the hash code of the aggregated type instance.
    /// </summary>
    /// <returns>Hash code of this instance.</returns>
    // --------------------------------------------------------------------------------
    public override int GetHashCode()
    {
      return _TypeObject.GetHashCode();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this type.
    /// </summary>
    /// <returns></returns>
    // --------------------------------------------------------------------------------
    public override string ToString()
    {
      return FullName;
    }

    #endregion
  }
}