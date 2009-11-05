using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity based on a System.Type object.
  /// </summary>
  // ================================================================================================
  public sealed class ReflectedTypeBasedTypeEntityReference: MetadataBasedSemanticEntityReference<TypeEntity, System.Type>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectedTypeBasedTypeEntityReference"/> class.
    /// </summary>
    /// <param name="metadata">A metadata object that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public ReflectedTypeBasedTypeEntityReference(System.Type metadata)
      : base(metadata)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(
      ISemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      return GetTypeEntityByReflectedType(Metadata, semanticGraph);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetTypeEntityByReflectedType(System.Type type, SemanticGraph semanticGraph)
    {
      TypeEntity typeEntity = null;

      // Is it a constructed generic type? If so then the arguments must be resolved too.
      if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        typeEntity = GetConstructedGenericTypeEntity(type, semanticGraph);
      }
      else if (type.IsArray)
      {
        typeEntity = GetArrayTypeEntity(type, semanticGraph);
      }
      else if (type.IsByRef)
      {
        typeEntity = GetReferenceTypeEntity(type, semanticGraph);
      }
      else
      {
        typeEntity = GetSimpleTypeEntity(type, semanticGraph);
      }

      if (typeEntity==null)
      {
        throw new ApplicationException("Unresolved reflected type.");  
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in case of a constructed generic type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetConstructedGenericTypeEntity(System.Type type, SemanticGraph semanticGraph)
    {
      // If it's a constructed generic type, then lets find its type definition's entity.
      var genericTypeDefinition = type.GetGenericTypeDefinition();
      var typeDefinitionEntity = semanticGraph.GetEntityByMetadataObject(genericTypeDefinition);

      if (typeDefinitionEntity == null)
      {
        throw new ApplicationException(string.Format("Semantic entity not found for type '{0}'.",
                                                     genericTypeDefinition.Name));
      }
      if (!(typeDefinitionEntity is GenericCapableTypeEntity))
      {
        throw new ApplicationException(string.Format("Expected to find generic capable type entity, but found '{0}'.",
                                                     typeDefinitionEntity.GetType()));
      }

      // Resolve type arguments.
      var argumentEntities = new List<TypeEntity>();
      foreach (var genericArgument in type.GetGenericArguments())
      {
        // The type argument can be any type
        var argumentEntity = GetTypeEntityByReflectedType(genericArgument, semanticGraph);
        if (argumentEntity == null)
        {
          throw new ApplicationException(string.Format("Could not resolve type '{0}' to entity.",
                                                       genericArgument.Name));
        }
        argumentEntities.Add(argumentEntity);
      }

      // Return the constructed type.
      return ConstructedTypeHelper.GetConstructedGenericType(typeDefinitionEntity as GenericCapableTypeEntity, argumentEntities);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of a simple type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetSimpleTypeEntity(System.Type type, SemanticGraph semanticGraph)
    {
      var foundEntity = semanticGraph.GetEntityByMetadataObject(type);

      if (foundEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve type '{0}' to entity.", type.Name));
      }

      if (foundEntity != null && !(foundEntity is TypeEntity))
      {
        throw new ApplicationException(
          string.Format("Expected to find a TypeEntity for type '{0}', but found a '{1}'.", type.Name,
                        foundEntity.GetType()));
      }

      return foundEntity as TypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of an array type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetArrayTypeEntity(System.Type type, SemanticGraph semanticGraph)
    {
      var underlyingTypeEntity = GetTypeEntityByReflectedType(type.GetElementType(), semanticGraph);

      if (underlyingTypeEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve underlying type of array '{0}'.", type.Name));
      }

      return ConstructedTypeHelper.GetConstructedArrayType(underlyingTypeEntity, type.GetArrayRank());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of a reference type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetReferenceTypeEntity(System.Type type, SemanticGraph semanticGraph)
    {
      var underlyingTypeEntity = GetTypeEntityByReflectedType(type.GetElementType(), semanticGraph);

      if (underlyingTypeEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve underlying type of reference '{0}'.", type.Name));
      }

      // TODO: is it correct to return the underlying type in case of a reference type?
      return underlyingTypeEntity;
    }
  }
}
