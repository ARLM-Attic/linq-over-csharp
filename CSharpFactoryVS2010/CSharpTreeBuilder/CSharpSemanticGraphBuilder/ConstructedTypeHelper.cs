using System.Collections.Generic;
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
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>An array type entity with the given underlying type.</returns>
    /// <remarks>
    /// If the constructed type already exists, then retrieves it, otherwise creates it.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public static ArrayTypeEntity GetConstructedArrayType(TypeEntity underlyingTypeEntity, int rank, SemanticGraph semanticGraph)
    {
      var arrayType = underlyingTypeEntity.GetArrayTypeByRank(rank);

      // If the constructed type not exists yet then create it.
      if (arrayType == null)
      {
        arrayType = new ArrayTypeEntity(underlyingTypeEntity, rank);
        underlyingTypeEntity.AddArrayType(arrayType);
      }

      return arrayType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a contructed generic type given its type definition entity and the type arguments.
    /// </summary>
    /// <param name="genericTypeDefinition">The generic type definition entity.</param>
    /// <param name="typeArguments">A list of type arguments.</param>
    /// <returns>A constructed generic entity.</returns>
    // ----------------------------------------------------------------------------------------------
    public static ConstructedGenericTypeEntity GetConstructedGenericType(
      GenericCapableTypeEntity genericTypeDefinition, List<TypeEntity> typeArguments)
    {
      // Check whether this particular constructed generic type was already created.
      var constructedGenericType = genericTypeDefinition.GetConstructedGenericType(typeArguments);

      // If not yet exists then create it
      if (constructedGenericType == null)
      {
        // Create the constructed generic type
        constructedGenericType = new ConstructedGenericTypeEntity(genericTypeDefinition, typeArguments);

        // Add the constructed type to its type definition
        genericTypeDefinition.AddConstructedGenericType(constructedGenericType);
      }

      return constructedGenericType;
    }
  }
}
