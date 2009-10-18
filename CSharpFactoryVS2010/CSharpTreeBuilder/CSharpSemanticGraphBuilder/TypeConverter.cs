using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the type conversion logic described in the spec (§6).
  /// </summary>
  /// <remarks>
  /// A conversion enables an expression to be treated as being of a particular type. 
  /// A conversion may cause an expression of a given type to be treated as having a different type, 
  /// or it may cause an expression without a type to get a type.
  /// </remarks>
  // ================================================================================================
  public sealed class TypeConverter
  {
    /// <summary>
    /// A lookup table containing a bool value for every pair of BuiltInTypes, indicating whether 
    /// an implicit numeric conversion exists from a type to another.
    /// </summary>
    private static bool[,] _ImplicitNumericConversionTable = InitializeImplicitNumericConversionTable();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an expression can be implicitly converted to a type.
    /// </summary>
    /// <param name="expression">An expression.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the expression can be implicitly converted to the target type, 
    /// false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool ImplicitConversionExists(ExpressionEntity expression, TypeEntity targetType)
    {
      if (expression == null || targetType == null)
      {
        return false;
      }

      var sourceType = expression.Result is TypedExpressionResult
        ? (expression.Result as TypedExpressionResult).Type
        : null;

      return ImplicitConversionExists(sourceType, targetType)
        || ImplicitEnumerationConversionExists(expression, targetType)
        || NullLiteralConversionExists(expression, targetType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a type can be implicitly converted to another type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the source type can be implicitly converted to the target type, 
    /// false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool ImplicitConversionExists(TypeEntity sourceType, TypeEntity targetType)
    {
      if (sourceType == null || targetType == null)
      {
        return false;
      }

      return IdentityConversionExists(sourceType, targetType)
        || ImplicitNumericConversionExists(sourceType, targetType)
        || ImplicitNullableConversionExists(sourceType, targetType)
        || ImplicitReferenceConversionExists(sourceType, targetType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the most encompassed type from a collection of type entities.
    /// </summary>
    /// <param name="typeEntities">A collection of type entities.</param>
    /// <returns>The most encompassed type from the collection. 
    /// Null if the collection is empty or the most encompassed type is ambigous.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetMostEncompassedType(IEnumerable<TypeEntity> typeEntities)
    {
      if (typeEntities == null || typeEntities.Count() == 0)
      {
        return null;
      }

      var mostEncompassedTypes = 
        typeEntities.Where(encompassedCandidate =>
          typeEntities.All(typeEntity =>
            ImplicitConversionExists(encompassedCandidate, typeEntity)));

      return mostEncompassedTypes.Count() == 1 ? mostEncompassedTypes.First() : null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an implicit enumeration conversion exists 
    /// for an expression and a target type.
    /// </summary>
    /// <param name="expression">An expression.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool ImplicitEnumerationConversionExists(ExpressionEntity expression, TypeEntity targetType)
    {
      // An implicit enumeration conversion permits the decimal-integer-literal 0 to be converted
      // to any enum-type and to any nullable-type whose underlying type is an enum-type.
      return IsDecimalIntegerLiteralZero(expression) &&
        (
          targetType is EnumEntity
        ||
          (targetType.IsNullableType && targetType.UnderlyingOfNullableType is EnumEntity)
        );
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a null literal conversion exists 
    /// for an expression and a target type.
    /// </summary>
    /// <param name="expression">An expression.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool NullLiteralConversionExists(ExpressionEntity expression, TypeEntity targetType)
    {
      // An implicit conversion exists from the null literal 
      // to any nullable type and to any reference type.
      return (expression is NullLiteralExpressionEntity) &&
        (targetType.IsNullableType || targetType.IsReferenceType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an identity conversion exists from a type to another type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool IdentityConversionExists(TypeEntity sourceType, TypeEntity targetType)
    {
      return sourceType == targetType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an implicit numeric conversion exists 
    /// from a type to another type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool ImplicitNumericConversionExists(TypeEntity sourceType, TypeEntity targetType)
    {
      if (sourceType.IsBuiltInType && targetType.IsBuiltInType)
      {
        return _ImplicitNumericConversionTable[(int)sourceType.BuiltInTypeValue, (int)targetType.BuiltInTypeValue];
      }

      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an implicit nullable conversion exists 
    /// from a type to another type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool ImplicitNullableConversionExists(TypeEntity sourceType, TypeEntity targetType)
    {
      // Predefined implicit conversions that operate on non-nullable value types 
      // can also be used with nullable forms of those types. 
      // For each of the predefined implicit identity and numeric conversions 
      // that convert from a non-nullable value type S to a non-nullable value type T, 
      // the following implicit nullable conversions exist:
      // - An implicit conversion from S? to T?.
      // - An implicit conversion from S to T?.

      if (targetType.IsNullableType)
      {
        targetType = targetType.UnderlyingOfNullableType;

        if (sourceType.IsNullableType)
        {
          sourceType = sourceType.UnderlyingOfNullableType;
        }

        return IdentityConversionExists(sourceType, targetType) || ImplicitNumericConversionExists(sourceType, targetType);
      }

      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an implicit reference conversion exists 
    /// from a type to another type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>True if the conversion exists, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool ImplicitReferenceConversionExists(TypeEntity sourceType, TypeEntity targetType)
    {
      // The implicit reference conversions are:
      if (sourceType.IsReferenceType)
      {
        // - From any reference-type to object.
        if (targetType.BuiltInTypeValue == BuiltInType.Object)
        {
          return true;
        }

        // - From any class-type S to any class-type T, provided S is derived from T.
        // - From any class-type S to any interface-type T, provided S implements T.
        if (sourceType is ClassEntity)
        {
          return (targetType is ClassEntity && targetType.IsBaseOf(sourceType))
            || (targetType is InterfaceEntity && sourceType.Implements(targetType as InterfaceEntity));
        }

        // - From any interface-type S to any interface-type T, provided S is derived from T.
        if (sourceType is InterfaceEntity)
        {
          return (targetType is InterfaceEntity && sourceType.Implements(targetType as InterfaceEntity));
        }

        // - From an array-type S with an element type SE to an array-type T with an element type TE, 
        //   provided all of the following are true:
        //   - S and T differ only in element type. In other words, S and T have the same number of dimensions.
        //   - Both SE and TE are reference-types.
        //   - An implicit reference conversion exists from SE to TE.
        if (sourceType.IsArrayType && targetType.IsArrayType)
        {
          var sourceArray = sourceType as ArrayTypeEntity;
          var targetArray = targetType as ArrayTypeEntity;

          return (sourceArray.Rank == targetArray.Rank)
            && (sourceArray.UnderlyingType != null) && (sourceArray.UnderlyingType.IsReferenceType)
            && (targetArray.UnderlyingType != null) && (targetArray.UnderlyingType.IsReferenceType)
            && ImplicitReferenceConversionExists(sourceArray.UnderlyingType, targetArray.UnderlyingType);
        }

          // - From any array-type to System.Array and the interfaces it implements.
        if (sourceType.IsArrayType && 
          ((targetType == targetType.SemanticGraph.SystemArray)
          || (targetType is InterfaceEntity && targetType.SemanticGraph.SystemArray.Implements(targetType as InterfaceEntity))))
        {
          return true;
        }

        // - From a single-dimensional array type S[] to System.Collections.Generic.IList<T> and its base interfaces, 
        //   provided that there is an implicit identity or reference conversion from S to T.
        //if (sourceType.IsArrayType && (sourceType as ArrayTypeEntity).Rank == 1)
        //{
        //  if (targetType is ConstructedGenericTypeEntity)
        //  {
        //    var targetConstructedGeneric = targetType as ConstructedGenericTypeEntity;
        //    var ilistEntity = targetType.SemanticGraph.GetEntityByMetadataObject(typeof(IList<>)) as TypeEntity;

        //    if (targetConstructedGeneric.UnderlyingType == ilistEntity
        //      && targetConstructedGeneric.TypeArguments.Count == 1)
        //    {
        //      var sourceElement = (sourceType as ArrayTypeEntity).UnderlyingType;
        //      var targetElement = targetConstructedGeneric.TypeArguments[0];

        //      return ((targetType == ilistEntity) || ilistEntity.Implements(targetType))
        //        && (IdentityConversionExists(sourceElement, targetElement)
        //           || ImplicitReferenceConversionExists(sourceElement, targetElement));
        //    }
        //  }
        //}



        // - From any delegate-type to System.Delegate and the interfaces it implements.
        // - From the null literal to any reference-type --> This is implemented in NullLiteralConversionExists method.
        // - Implicit conversions involving type parameters that are known to be reference types. See §6.1.9 for more details on implicit conversions involving type parameters.
      }

      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an expression is a decimal integer literal 0.
    /// </summary>
    /// <param name="expression">An expression.</param>
    /// <returns>True, if the expression is a decimal integer literal 0, false otherwise.</returns>
    /// <remarks>
    /// This method doesn't implement the spec very precisely, because 
    /// According to the spec, a decimal-integer-literal is a sequence of decimal digits followed by
    /// an optional integer-type-suffix (u, l, ul, etc.) 
    /// So a hexadecimal 0 literal should not be accepted here.
    /// However, we don't keep information in the TypedLiteralExpressionEntity object about 
    /// the original lexical format of the literal, so we accept any 0 value,
    /// even if it was in hexadecimal format in the source.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private bool IsDecimalIntegerLiteralZero(ExpressionEntity expression)
    {
      var literal = expression as TypedLiteralExpressionEntity;
      if (literal != null)
      {
        var type = literal.Type;
        if (type != null)
        {
          if (type.BuiltInTypeValue == BuiltInType.Int
            || type.BuiltInTypeValue == BuiltInType.Uint
            || type.BuiltInTypeValue == BuiltInType.Long
            || type.BuiltInTypeValue == BuiltInType.Ulong
            || type.BuiltInTypeValue == BuiltInType.Decimal
            || type.BuiltInTypeValue == BuiltInType.Float
            || type.BuiltInTypeValue == BuiltInType.Double
            )
          {
            return (literal.Value is int) && (int)literal.Value == 0;
          }
        }
      }

      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Fills in the implicit numeric conversion lookup table.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private static bool[,] InitializeImplicitNumericConversionTable()
    {
      int maxValue = (int)BuiltInType.MaxValue;

      var table = new bool[maxValue,maxValue];

      for (int i = 0; i < maxValue; i++)
      {
        for (int j = 0; i < maxValue; i++)
        {
          table[i, j] = false;
        }
      }

      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Short] = true;
      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Int] = true;
      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Sbyte, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Byte, (int)BuiltInType.Short] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Ushort] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Int] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Uint] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Ulong] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Byte, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Short, (int)BuiltInType.Int] = true;
      table[(int)BuiltInType.Short, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Short, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Short, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Short, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Ushort, (int)BuiltInType.Int] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Uint] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Ulong] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Ushort, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Int, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Int, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Int, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Int, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Uint, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Uint, (int)BuiltInType.Ulong] = true;
      table[(int)BuiltInType.Uint, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Uint, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Uint, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Long, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Long, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Long, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Ulong, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Ulong, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Ulong, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Char, (int)BuiltInType.Ushort] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Int] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Uint] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Long] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Ulong] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Float] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Double] = true;
      table[(int)BuiltInType.Char, (int)BuiltInType.Decimal] = true;

      table[(int)BuiltInType.Float, (int)BuiltInType.Double] = true;

      return table;
    }

    #endregion
  }
}
