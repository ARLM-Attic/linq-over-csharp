using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a System.Type object to a TypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class ReflectedTypeToTypeEntityResolver : MetadataResolver<TypeEntity, System.Type>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectedTypeToTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="metadata">The source object to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public ReflectedTypeToTypeEntityResolver(System.Type metadata)
      : base(metadata)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      return GetTypeEntityByReflectedType(Metadata, context);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetTypeEntityByReflectedType(System.Type type, ISemanticEntity contextEntity)
    {
      TypeEntity typeEntity = null;

      if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        typeEntity = GetConstructedGenericTypeEntity(type, contextEntity, type.GetGenericArguments());
      }
      else if (type.IsArray)
      {
        typeEntity = GetArrayTypeEntity(type, contextEntity);
      }
      else if (type.IsByRef)
      {
        typeEntity = GetReferenceTypeEntity(type, contextEntity);
      }
      else if (type.IsPointer)
      {
        typeEntity = GetPointerTypeEntity(type, contextEntity);
      }
      else
      {
        typeEntity = GetSimpleTypeEntity(type, contextEntity);
      }

      if (typeEntity == null)
      {
        throw new ApplicationException("Unresolved reflected type.");
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity that will be used as a generic template for creating a generic type entity.
    /// </summary>
    /// <param name="type">A reflected type whose generic template entity will be determined.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <param name="genericArguments">The arguments of the constructed type.</param>
    /// <returns>A semantic entity that will be used as a generic template to create the entity 
    /// that correspondes to the supplied reflected type object.</returns>
    // ----------------------------------------------------------------------------------------------
    private static GenericCapableTypeEntity GetGenericTemplateEntity(
      System.Type type, 
      ISemanticEntity contextEntity, 
      IEnumerable<Type> genericArguments)
    {
      GenericCapableTypeEntity typeDefinitionEntity = null;
      var declaringType = type.DeclaringType;

      // If the parent (declaring) type is also a generic, then we go into a recursion 
      if (declaringType != null && declaringType.IsGenericType)
      {
        var parentGenericArguments = genericArguments.Take(declaringType.GetGenericArguments().Length);
        var constructedParent = GetConstructedGenericTypeEntity(declaringType, contextEntity, parentGenericArguments);

        var childTypeCapableParent = constructedParent as ChildTypeCapableTypeEntity;
        if (childTypeCapableParent == null)
        {
          throw new ApplicationException(string.Format("Parent of '{0}' is not child type capable.", type));
        }

        var childTypeParameterCount = type.GetGenericArguments().Length - declaringType.GetGenericArguments().Length;
        var childTypeNameBacktickIndex = type.Name.IndexOf('`');
        var childTypeName = childTypeNameBacktickIndex >= 0 ? type.Name.Substring(0, childTypeNameBacktickIndex) : type.Name;
        typeDefinitionEntity = childTypeCapableParent.GetSingleChildType<GenericCapableTypeEntity>(childTypeName, childTypeParameterCount);
      }
      else
      {
        typeDefinitionEntity = contextEntity.SemanticGraph.GetEntityByMetadataObject(type.GetGenericTypeDefinition())
          as GenericCapableTypeEntity;
      }

      return typeDefinitionEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the GenericCapableTypeEntity that belongs to a System.Type object in case of a constructed generic type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <param name="genericArguments">The arguments of the constructed type.</param>
    /// <returns>A GenericCapableTypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static GenericCapableTypeEntity GetConstructedGenericTypeEntity(
      System.Type type, 
      ISemanticEntity contextEntity, 
      IEnumerable<Type> genericArguments)
    {
      GenericCapableTypeEntity constructedGenericTypeEntity = null;

      var typeDefinitionEntity = GetGenericTemplateEntity(type, contextEntity, genericArguments);
      if (typeDefinitionEntity == null)
      {
        throw new ApplicationException(string.Format("Type definition entity not found for type '{0}'.", type));
      }

      var ownTypeParameterCount = typeDefinitionEntity.OwnTypeParameterCount;
      var parentTypeParameterCount = typeDefinitionEntity.AllTypeParameterCount - ownTypeParameterCount;

      if (ownTypeParameterCount > 0)
      {
        // Let's determine the type argument entities.
        var argumentEntities = new List<TypeEntity>();

        foreach (var genericArgument in genericArguments)
        {
          // Let's resolve the type argument to an entity
          var argumentEntity = GetTypeEntityByReflectedType(genericArgument, contextEntity);
          if (argumentEntity == null)
          {
            throw new ApplicationException(string.Format("Could not resolve type '{0}' to entity.",
                                                         genericArgument.Name));
          }

          argumentEntities.Add(argumentEntity);
        }

        var typeParameterMap = new TypeParameterMap(typeDefinitionEntity.OwnTypeParameters, argumentEntities.Skip(parentTypeParameterCount));

        constructedGenericTypeEntity = typeDefinitionEntity.GetGenericClone(typeParameterMap) as GenericCapableTypeEntity;
      }
      else
      {
        constructedGenericTypeEntity = typeDefinitionEntity;
      }

      return constructedGenericTypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of a simple type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetSimpleTypeEntity(System.Type type, ISemanticEntity contextEntity)
    {
      var foundEntity = contextEntity.SemanticGraph.GetEntityByMetadataObject(type);

      if (foundEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve type '{0}' to entity.", type.Name));
      }

      var foundTypeEntity = foundEntity as TypeEntity;

      if (foundTypeEntity == null)
      {
        throw new ApplicationException(
          string.Format("Expected to find a TypeEntity for type '{0}', but found a '{1}'.", type.Name,
                        foundEntity.GetType()));
      }

      return foundTypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of an array type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetArrayTypeEntity(System.Type type, ISemanticEntity contextEntity)
    {
      var underlyingTypeEntity = GetTypeEntityByReflectedType(type.GetElementType(), contextEntity);

      if (underlyingTypeEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve underlying type of array '{0}'.", type.Name));
      }

      return ArrayTypeEntity.GetConstructedArrayType(underlyingTypeEntity, type.GetArrayRank());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of a pointer type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetPointerTypeEntity(System.Type type, ISemanticEntity contextEntity)
    {
      var underlyingTypeEntity = GetTypeEntityByReflectedType(type.GetElementType(), contextEntity);

      if (underlyingTypeEntity == null)
      {
        throw new ApplicationException(
          string.Format("Could not resolve underlying type of pointer '{0}'.", type.Name));
      }

      return PointerTypeEntity.GetConstructedPointerType(underlyingTypeEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object in the case of a reference type.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <param name="contextEntity">The context of the type resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetReferenceTypeEntity(System.Type type, ISemanticEntity contextEntity)
    {
      var underlyingTypeEntity = GetTypeEntityByReflectedType(type.GetElementType(), contextEntity);

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
