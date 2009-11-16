using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class contains static helper methods to get or create constructed types.
  /// </summary>
  // ================================================================================================
  public static class ConstructedTypeHelper
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a pointer type given its underlying type.
    /// </summary>
    /// <param name="underlyingTypeEntity">The underlying type (eg. 'int' for 'int*')</param>
    /// <returns>A pointer type entity with the given underlying type.</returns>
    /// <remarks>
    /// If the constructed type already exists, then retrieves it, otherwise creates it.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public static PointerToTypeEntity GetConstructedPointerType(TypeEntity underlyingTypeEntity)
    {
      // If the constructed type not exists yet then create it.
      if (underlyingTypeEntity.PointerType == null)
      {
        underlyingTypeEntity.PointerType = new PointerToTypeEntity(underlyingTypeEntity);
      }

      return underlyingTypeEntity.PointerType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an array type given its underlying type.
    /// </summary>
    /// <param name="underlyingTypeEntity">The underlying type (eg. 'int' for 'int[]')</param>
    /// <param name="rank">The rank of the array.</param>
    /// <returns>An array type entity with the given underlying type.</returns>
    /// <remarks>
    /// If the constructed type already exists, then retrieves it, otherwise creates it.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public static ArrayTypeEntity GetConstructedArrayType(TypeEntity underlyingTypeEntity, int rank)
    {
      var arrayType = underlyingTypeEntity.GetArrayTypeByRank(rank);

      // If the constructed type doesn't exist yet then create it.
      if (arrayType == null)
      {
        arrayType = new ArrayTypeEntity(underlyingTypeEntity, rank);
        underlyingTypeEntity.AddArrayType(arrayType);
      }

      return arrayType;
    }
  }
}
