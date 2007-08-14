using System;
using System.Reflection;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents a .NET binary type described by a System.Type instance.
  /// </summary>
  /// <remarks>This class implements the ITypeCharactersitic interface.</remarks>
  // ==================================================================================
  public sealed class NetBinaryType : ITypeCharacteristics, IEquatable<NetBinaryType>
  {
    #region Private fields

    private readonly Type _TypeObject;
    private readonly CompilationReference _AssemblyRef;
    private readonly ITypeCharacteristics _BaseType;
    private readonly ITypeCharacteristics _DeclaringType;

    #endregion

    #region Static public members

    /// <summary>Represents the System.Object type</summary>
    public static NetBinaryType Object = new NetBinaryType(typeof(Object));

    /// <summary>Represents the System.Boolean type</summary>
    public static NetBinaryType Boolean = new NetBinaryType(typeof(Boolean));

    /// <summary>Represents the System.Byte type</summary>
    public static NetBinaryType Byte = new NetBinaryType(typeof(Byte));

    /// <summary>Represents the System.SByte type</summary>
    public static NetBinaryType SByte = new NetBinaryType(typeof(SByte));

    /// <summary>Represents the System.Char type</summary>
    public static NetBinaryType Char = new NetBinaryType(typeof(Char));

    /// <summary>Represents the System.Decimal type</summary>
    public static NetBinaryType Decimal = new NetBinaryType(typeof(Decimal));

    /// <summary>Represents the System.Double type</summary>
    public static NetBinaryType Double = new NetBinaryType(typeof(Double));

    /// <summary>Represents the System.Singe type</summary>
    public static NetBinaryType Single = new NetBinaryType(typeof(Single));

    /// <summary>Represents the System.Int16 type</summary>
    public static NetBinaryType Int16 = new NetBinaryType(typeof(Int16));

    /// <summary>Represents the System.Int32 type</summary>
    public static NetBinaryType Int32 = new NetBinaryType(typeof(Int32));

    /// <summary>Represents the System.Int64 type</summary>
    public static NetBinaryType Int64 = new NetBinaryType(typeof(Int64));

    /// <summary>Represents the System.UInt16 type</summary>
    public static NetBinaryType UInt16 = new NetBinaryType(typeof(UInt16));

    /// <summary>Represents the System.Int32 type</summary>
    public static NetBinaryType UInt32 = new NetBinaryType(typeof(UInt32));

    /// <summary>Represents the System.Int64 type</summary>
    public static NetBinaryType UInt64 = new NetBinaryType(typeof(UInt64));

    /// <summary>Represents the System.String type</summary>
    public static NetBinaryType String = new NetBinaryType(typeof(String));

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
      _AssemblyRef = new AssemblyReference(typeObject.Assembly);
      _BaseType = typeObject.BaseType == null
                    ? Object
                    : new NetBinaryType(typeObject.BaseType);
      _DeclaringType = typeObject.DeclaringType == null
                         ? null
                         : new NetBinaryType(typeObject.DeclaringType);
    }

    #endregion

    #region ITypeCharacteristics implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationReference Compilation
    {
      get { return _AssemblyRef; }
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
    public ITypeCharacteristics BaseType
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
    public ITypeCharacteristics DeclaringType
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
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedAssembly
    {
      get { return _TypeObject.IsNestedAssembly; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPrivate
    {
      get { return _TypeObject.IsNestedPrivate; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return _TypeObject.IsNestedPublic; }
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
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPrimitive
    {
      get { return _TypeObject.IsPrimitive; }
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
    /// Gets a MemberTypes value indicating that this member is a type or a nested 
    /// type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MemberTypes MemberType
    {
      get { return _TypeObject.MemberType; }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object carrying detailed information about this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public object TypeObject
    {
      get { return _TypeObject;  }
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

    #endregion
  }
}