using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a builtin C# type.
  /// </summary>
  /// <remarks>
  /// Builtin types are identified through reserved words, but these reserved words are simply aliases
  /// for predefined types in the System namespace. These are: sbyte, byte, short, ushort, int, uint,
  /// long, ulong, char, float, double, bool, decimal, bool, string, object.
  /// </remarks>
  // ================================================================================================
  public sealed class BuiltInTypeEntity : TypeEntity, IAliasType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BuiltInTypeEntity"/> class.
    /// </summary>
    /// <param name="builtInType">A built in type.</param>
    // ----------------------------------------------------------------------------------------------
    public BuiltInTypeEntity(BuiltInType builtInType)
    {
      BuiltInType = builtInType;

      System.Type aliasedType = null;

      switch (builtInType)
      {
        case BuiltInType.Bool:
          Name = "bool";
          aliasedType = typeof (bool);
          break;
        case BuiltInType.Byte:
          Name = "byte";
          aliasedType = typeof(byte);
          break;
        case BuiltInType.Char:
          Name = "char";
          aliasedType = typeof(char);
          break;
        case BuiltInType.Decimal:
          Name = "decimal";
          aliasedType = typeof(decimal);
          break;
        case BuiltInType.Double:
          Name = "double";
          aliasedType = typeof(double);
          break;
        case BuiltInType.Float:
          Name = "float";
          aliasedType = typeof(float);
          break;
        case BuiltInType.Int:
          Name = "int";
          aliasedType = typeof(int);
          break;
        case BuiltInType.Long:
          Name = "long";
          aliasedType = typeof(long);
          break;
        case BuiltInType.Object:
          Name = "object";
          aliasedType = typeof(object);
          break;
        case BuiltInType.Sbyte:
          Name = "sbyte";
          aliasedType = typeof(sbyte);
          break;
        case BuiltInType.Short:
          Name = "short";
          aliasedType = typeof(short);
          break;
        case BuiltInType.String:
          Name = "string";
          aliasedType = typeof(string);
          break;
        case BuiltInType.Uint:
          Name = "uint";
          aliasedType = typeof(uint);
          break;
        case BuiltInType.Ulong:
          Name = "ulong";
          aliasedType = typeof(ulong);
          break;
        case BuiltInType.Ushort:
          Name = "ushort";
          aliasedType = typeof(ushort);
          break;
        default:
          throw new ApplicationException(string.Format("Unexpected BuiltInType: '{0}'", builtInType));
      }

      AliasedTypeReference = new ReflectedTypeBasedTypeEntityReference(aliasedType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the built in type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BuiltInType BuiltInType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference of the aliased type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> AliasedTypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the aliased type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity AliasedType
    {
      get
      {
        return AliasedTypeReference.TargetEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get
      {
        return (BuiltInType == BuiltInType.Object) || (BuiltInType == BuiltInType.String);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get
      {
        return !IsReferenceType;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a floating point type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsFloatingPointType
    {
      get
      {
        return (BuiltInType == BuiltInType.Float) || (BuiltInType == BuiltInType.Double);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an integral type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsIntegralType
    {
      get
      {
        return (BuiltInType == BuiltInType.Sbyte)
               || (BuiltInType == BuiltInType.Byte)
               || (BuiltInType == BuiltInType.Short)
               || (BuiltInType == BuiltInType.Ushort)
               || (BuiltInType == BuiltInType.Int)
               || (BuiltInType == BuiltInType.Uint)
               || (BuiltInType == BuiltInType.Long)
               || (BuiltInType == BuiltInType.Ulong)
               || (BuiltInType == BuiltInType.Char);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a numeric type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsNumericType
    {
      get
      {
        return (BuiltInType == BuiltInType.Decimal) || IsFloatingPointType || IsIntegralType;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a simple type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsSimpleType
    {
      get
      {
        return (BuiltInType == BuiltInType.Bool) || IsNumericType;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity. 
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's declaration space, 
    /// otherwise returns the base class' empty declaration space.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override DeclarationSpace DeclarationSpace
    {
      get
      {
        return AliasedType == null ? base.DeclarationSpace : AliasedType.DeclarationSpace;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base type references.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's base types,
    /// otherwise returns the base class' empty base type reference list.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypeReferences
    {
      get
      {
        return AliasedType == null ? base.BaseTypeReferences : AliasedType.BaseTypeReferences;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's members,
    /// otherwise returns the base class' empty member list.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<MemberEntity> Members
    {
      get
      {
        return AliasedType == null ? base.Members : AliasedType.Members;
      }
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
      base.AcceptVisitor(visitor);

      visitor.Visit(this);
    }

    #endregion
  }
}
