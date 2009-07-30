using System;
using System.Collections.Generic;

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

      System.Type aliasType = null;

      switch (builtInType)
      {
        case BuiltInType.Bool:
          Name = "bool";
          aliasType = typeof (bool);
          break;
        case BuiltInType.Byte:
          Name = "byte";
          aliasType = typeof(byte);
          break;
        case BuiltInType.Char:
          Name = "char";
          aliasType = typeof(char);
          break;
        case BuiltInType.Decimal:
          Name = "decimal";
          aliasType = typeof(decimal);
          break;
        case BuiltInType.Double:
          Name = "double";
          aliasType = typeof(double);
          break;
        case BuiltInType.Float:
          Name = "float";
          aliasType = typeof(float);
          break;
        case BuiltInType.Int:
          Name = "int";
          aliasType = typeof(int);
          break;
        case BuiltInType.Long:
          Name = "long";
          aliasType = typeof(long);
          break;
        case BuiltInType.Object:
          Name = "object";
          aliasType = typeof(object);
          break;
        case BuiltInType.Sbyte:
          Name = "sbyte";
          aliasType = typeof(sbyte);
          break;
        case BuiltInType.Short:
          Name = "short";
          aliasType = typeof(short);
          break;
        case BuiltInType.String:
          Name = "string";
          aliasType = typeof(string);
          break;
        case BuiltInType.Uint:
          Name = "uint";
          aliasType = typeof(uint);
          break;
        case BuiltInType.Ulong:
          Name = "ulong";
          aliasType = typeof(ulong);
          break;
        case BuiltInType.Ushort:
          Name = "ushort";
          aliasType = typeof(ushort);
          break;
        default:
          throw new ApplicationException(string.Format("Unexpected BuiltInType: '{0}'", builtInType));
      }

      AliasToType = new ReflectedTypeBasedTypeEntityReference(aliasType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the built in type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BuiltInType BuiltInType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the alias type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> AliasToType { get; private set; }

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
    /// If the aliased type is already resolved then returns that type's declaration space.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override DeclarationSpace DeclarationSpace
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.DeclarationSpace
                 : base.DeclarationSpace;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base types.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's base types.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypes
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.BaseTypes
                 : base.BaseTypes;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's members.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<MemberEntity> Members
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.Members
                 : base.Members;
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
