using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a type declaration.
  /// </summary>
  // ==================================================================================
  public abstract class TypeBase : AttributedElement,
    ITypeAbstraction
  {
    #region Private fields

    private List<ITypeAbstraction> _GenericArguments;
    private static readonly Dictionary<string, ITypeAbstraction> NoNestedTypes =
      new Dictionary<string, ITypeAbstraction>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type presentation instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    protected TypeBase(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region ITypeAbstraction methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if a type is open or not.
    /// </summary>
    /// <remarks>
    /// A type is open, if directly or indireclty references to a type parametes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public abstract bool IsOpenType { get; }

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
    public virtual ITypeAbstraction GetElementType()
    {
      return null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list representing the generic arguments of a type.
    /// </summary>
    /// <returns>
    /// Arguments of a generic typeor generic type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public virtual List<ITypeAbstraction> GetGenericArguments()
    {
      if (_GenericArguments == null)
      {
        _GenericArguments = new List<ITypeAbstraction>();
        IEnumerable<ITypeAbstraction> args = GetArguments();
        if (args != null)
        {
          foreach (ITypeAbstraction typeParam in args) 
            _GenericArguments.Add(typeParam);
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
    public abstract Dictionary<string, ITypeAbstraction> GetInterfaces();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating, if the current type is a generic type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsGenericParameter
    {
      get { return false; }
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
    public virtual Dictionary<string, ITypeAbstraction> GetNestedTypes()
    {
      return NoNestedTypes;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual ITypeAbstraction GetUnderlyingEnumType()
    {
      return null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsRuntimeType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual ReferencedUnit DeclaringUnit
    {
      get { return Parser.CompilationUnit.ThisUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsUnmanagedType
    {
      get { return false; }
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
    public virtual ITypeAbstraction BaseType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual ITypeAbstraction DeclaringType
    {
      get { return null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool HasElementType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsAbstract
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsArray
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsClass { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsEnum { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsGenericType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsGenericTypeDefinition
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
    public virtual ITypeAbstraction MakeArrayType(int rank)
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
    public virtual ITypeAbstraction MakePointerType()
    {
      return new PointerType(this);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual int TypeParameterCount
    {
      get { return 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsInterface { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsNested { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsNotPublic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsPointer
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsPublic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsSealed { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsStatic { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract bool IsValueType { get; }

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
    public abstract bool IsVisible { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public abstract string Namespace { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
    {
      get { return GetParametrizedName(this); }
    }

    #endregion

    #region Virtual methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets an enumerable representing the generic type arguments.
    /// </summary>
    /// <returns>Enumerable representing the types.</returns>
    // --------------------------------------------------------------------------------
    protected virtual IEnumerable<ITypeAbstraction> GetArguments()
    {
      return null;  
    }

    #endregion

    #region Static methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the first type is the same as the second one or inherits from the 
    /// second one.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <param name="otherType">Other type used to check.</param>
    /// <returns>
    /// True, if the first type inherits the other type; otherwise, false. If the two
    /// types are equal, this method returns true.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool IsSameOrInheritsFrom(ITypeAbstraction type, 
                                            ITypeAbstraction otherType)
    {
      if (otherType == null) return false;
      while (type != null)
      {
        if (type.FullName == otherType.FullName) return true;
        type = type.BaseType;
      }
      return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root element type of a constructed type.
    /// </summary>
    /// <param name="type">Type to get the root element type for.</param>
    /// <returns>
    /// Root element type.
    /// </returns>
    /// <remarks>
    /// Theroot element type is the type that has no more element types. For example,
    /// in case of byte**[][,,], the root element type is byte.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public static ITypeAbstraction GetRootElementType(ITypeAbstraction type)
    {
      if (type == null) return null;
      while (type.HasElementType) type = type.GetElementType();
      GenericType genType = type as GenericType;
      return genType == null ? type : genType.ConstructingType;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Check is the specified types are the same or not.
    /// </summary>
    /// <param name="type">First type</param>
    /// <param name="otherType">Second type</param>
    /// <returns>
    /// True, if the types are the same; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool IsSame(ITypeAbstraction type, ITypeAbstraction otherType)
    {
      return (type.Namespace == otherType.Namespace &&
        type.ParametrizedName == otherType.ParametrizedName);  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Check is the specified types are the same or not.
    /// </summary>
    /// <param name="type">First type</param>
    /// <param name="otherType">Second type</param>
    /// <returns>
    /// True, if the types are the same; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool IsSame(ITypeAbstraction type, Type otherType)
    {
      return IsSame(type, new NetBinaryType(otherType));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gtes the parametrized name of the specified type.
    /// </summary>
    /// <param name="type">Type specification.</param>
    /// <returns>
    /// String representation of the parametrized name of the type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static string GetParametrizedName(ITypeAbstraction type)
    {
      StringBuilder sb = new StringBuilder(type.Namespace);
      if (!string.IsNullOrEmpty(type.Namespace)) sb.Append(".");
      sb.Append(type.SimpleName);
      if (type.TypeParameterCount > 0)
      {
        sb.Append('<');
        bool isFirst = true;
        foreach (ITypeAbstraction param in type.GetGenericArguments())
        {
          if (!isFirst) sb.Append(", ");
          isFirst = false;
          sb.Append(param.ParametrizedName);
        }
        sb.Append('>');
      }
      return sb.ToString();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the non-nullable equivalent of the specified type.
    /// </summary>
    /// <param name="type">Type to obtaint its non-nullable equivalent for.</param>
    /// <returns>
    /// The inner element type, if the input type is nullable; otherwise the input 
    /// type itself.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static ITypeAbstraction GetNonNullableElement(ITypeAbstraction type)
    {
      NullableType nullable = type as NullableType;
      if (nullable != null || type.FullName == typeof(Nullable<>).FullName)
        return type.GetGenericArguments()[0];
      return type;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of the specified type.
    /// </summary>
    /// <param name="type">Type to obtain base type for.</param>
    /// <returns>
    /// Base type of the type specified.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static ITypeAbstraction GetBaseType(ITypeAbstraction type)
    {
      // --- Native .NET binary types handle type parameter substitution of their own.
      if (type is NetBinaryType) return type.BaseType;
      return type.BaseType;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Substitues type arguments with concrete types in a type template.
    /// </summary>
    /// <param name="typeDef">Declares a generic type and its type arguments.</param>
    /// <param name="typeArgs">
    /// Concrete type arguments that should be substituted in the specified type template
    /// </param>
    /// <param name="typeTemplate">Type template.</param>
    /// <returns></returns>
    /// <remarks>
    /// Concrete type arguments are matched according to the index of the type parameter.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public static ITypeAbstraction SubstituteTypeParameters(ITypeAbstraction typeDef,
      List<ITypeAbstraction> typeArgs, ITypeAbstraction typeTemplate)
    {
      // --- Template is returned if the we do not use a generic type definition.
      if (!typeDef.IsGenericTypeDefinition) return typeTemplate;

      // --- At least as many type arguments should be provided as many type parameters
      // --- are in the definition.
      if (typeArgs.Count < typeDef.TypeParameterCount)
        throw new InvalidOperationException(
          String.Format("{0} type arguments have been provided, but {1} is expected",
          typeArgs.Count, typeDef.TypeParameterCount));

      // --- Create an index addressed by type parameter name.
      Dictionary<string, ITypeAbstraction> paramIndex = 
        new Dictionary<string, ITypeAbstraction>();
      int index = 0;
      foreach (ITypeAbstraction typeParam in typeDef.GetGenericArguments())
      {
        if (!typeParam.IsGenericParameter) continue;
        if (!paramIndex.ContainsKey(typeParam.Name)) 
          paramIndex.Add(typeParam.Name, typeArgs[index]);
        index++;
      }
      return SubstituteTypeParameters(paramIndex, typeTemplate);
    }

    public static ITypeAbstraction SubstituteTypeParameters(
      Dictionary<string, ITypeAbstraction> typeArgs, ITypeAbstraction typeTemplate)
    {
      return typeTemplate;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if there is an implicit conversion from a source type to a destinaion 
    /// type.
    /// </summary>
    /// <param name="fromType">Source type</param>
    /// <param name="toType">Destination type</param>
    /// <returns>
    /// True, if there is an implicit conversion from the source type to the 
    /// destination type; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool HasImplicitConversion(ITypeAbstraction fromType,
      ITypeAbstraction toType)
    {
      // --- Check for implicit identity conversion
      if (IsSame(fromType, toType)) return true;

      // --- Check for implicit numeric conversions
      if (IsSame(fromType, typeof(sbyte)) && HasImplicitFromSByte(toType)) 
        return true;
      if (IsSame(fromType, typeof(byte)) && HasImplicitFromByte(toType))
        return true;
      if (IsSame(fromType, typeof(short)) && HasImplicitFromShort(toType))
        return true;
      if (IsSame(fromType, typeof(ushort)) && HasImplicitFromUShort(toType))
        return true;
      if (IsSame(fromType, typeof(int)) && HasImplicitFromInt(toType))
        return true;
      if (IsSame(fromType, typeof(uint)) && HasImplicitFromUInt(toType))
        return true;
      if (IsSame(fromType, typeof(long)) && HasImplicitFromLong(toType))
        return true;
      if (IsSame(fromType, typeof(ulong)) && HasImplicitFromULong(toType))
        return true;
      if (IsSame(fromType, typeof(char)) && HasImplicitFromChar(toType))
        return true;
      if (IsSame(fromType, typeof(float)) && IsSame(toType, typeof(double)))
        return true;

      // --- Check for implicit reference type to object
      if (!fromType.IsValueType && IsSame(toType, typeof(object))) 
        return true;
      
      // --- Implicit conversion from class to class through inheritance
      if (fromType.IsClass && toType.IsClass && IsSameOrInheritsFrom(fromType, toType)) 
        return true;

      // --- Implicit conversion from interface to interface through inheritance
      if (fromType.IsInterface && toType.IsInterface && 
        IsSameOrInheritsFrom(fromType, toType)) 
        return true;

      // --- No implicit conversion exists
      return false;  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if SByte can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromSByte(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof (short)) ||
        IsSame(type, typeof (int)) ||
        IsSame(type, typeof (long)) ||
        IsSame(type, typeof (float)) ||
        IsSame(type, typeof (double)) ||
        IsSame(type, typeof (decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if Byte can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromByte(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(short)) ||
        IsSame(type, typeof(ushort)) ||
        IsSame(type, typeof(int)) ||
        IsSame(type, typeof(uint)) ||
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(ulong)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if Int16 can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromShort(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(int)) ||
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if UInt16 can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromUShort(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(int)) ||
        IsSame(type, typeof(uint)) ||
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(ulong)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if int can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromInt(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if UInt32 can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromUInt(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(ulong)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if long can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromLong(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if UInt64 can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromULong(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if char can be implicitly converted to the provided type.
    /// </summary>
    /// <param name="type">Destination type.</param>
    /// <returns>
    /// True, if implicit conversion is supported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static bool HasImplicitFromChar(ITypeAbstraction type)
    {
      return
        IsSame(type, typeof(ushort)) ||
        IsSame(type, typeof(int)) ||
        IsSame(type, typeof(uint)) ||
        IsSame(type, typeof(long)) ||
        IsSame(type, typeof(ulong)) ||
        IsSame(type, typeof(float)) ||
        IsSame(type, typeof(double)) ||
        IsSame(type, typeof(decimal));
    }

    #endregion
  }
}
