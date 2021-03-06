﻿using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the namespace and type name resolution logic described in the spec §3.8
  /// and the qualified alias member resolution logic described in §9.7.
  /// </summary>
  /// <remarks>
  /// The logic is implemented in static methods and different parts of it are called from
  /// different resolvers.
  /// </remarks>
  // ================================================================================================
  public static class NamespaceOrTypeNameResolutionAlgorithm
  {    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolve a namespace-or-type-name AST node.
    /// </summary>
    /// <param name="namespaceOrTypeName">The namespace-or-type-name AST node to be resolved.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>A resolved namespace or type entity.</returns>
    // ----------------------------------------------------------------------------------------------
    public static NamespaceOrTypeEntity ResolveNamespaceOrTypeNode(
      NamespaceOrTypeNameNode namespaceOrTypeName, 
      ISemanticEntity resolutionContextEntity, 
      ICompilationErrorHandler errorHandler)
    {
      if (namespaceOrTypeName.TypeTags.Count < 1)
      {
        throw new ArgumentException("Namespace-or-type-name must have at least 1 type tag.");
      }

      NamespaceOrTypeEntity foundEntity = null;
      TypeTagNodeCollection typeTagsToBeResolved = namespaceOrTypeName.TypeTags;
      ISemanticEntity accessingEntity = resolutionContextEntity;

      // If the namespace-or-type-name is a qualified-alias-member its meaning is as described in §9.7.
      if (namespaceOrTypeName.HasQualifier)
      {
        foundEntity = ResolveQualifiedAliasMember(
          namespaceOrTypeName.Qualifier, namespaceOrTypeName.TypeTags[0].Identifier,
          namespaceOrTypeName.TypeTags[0].Arguments, namespaceOrTypeName.SourcePoint,
          resolutionContextEntity, resolutionContextEntity.SemanticGraph.GlobalNamespace, errorHandler);

        // If the qualified-alias-member part of the name could not be resolved, then we can't continue the resolution.
        if (foundEntity == null)
        {
          return null;
        }

        // If the namespace-or-type-name has no more typetag, then we're finished with the resolution
        if (namespaceOrTypeName.TypeTags.Count == 1)
        {
          return foundEntity;
        }

        // Otherwise the resolution must be continued, 
        // - only the remaining typetags need to be resolved,
        // - and the resolution context is the found entity.
        typeTagsToBeResolved = namespaceOrTypeName.TypeTags.GetCopyWithoutFirstTag();
        resolutionContextEntity = foundEntity;
      }

      // Otherwise, a namespace-or-type-name has one of four forms:
      // - I
      // - I<A1, ..., AK>
      // - N.I
      // - N.I<A1, ..., AK>
      // where I is a single identifier, N is a namespace-or-type-name
      // and <A1, ..., AK> is an optional type-argument-list. 
      // When no type-argument-list is specified, consider K to be zero.

      // The meaning of a namespace-or-type-name is determined as follows:
      foundEntity = ResolveTypeTags(typeTagsToBeResolved, resolutionContextEntity, accessingEntity, errorHandler);

      return foundEntity;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the resolution logic of a single-tag namespace-or-type-name.
    ///  - I
    ///  - I{A1, ..., AK}
    /// where I is a single identifier, and {A1, ..., AK} is an optional type-argument-list.
    /// And the name is not a qualified-alias-member.
    /// </summary>
    /// <param name="typeTagNode">A single-tag namespace-or-type-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <param name="accessingEntity">The accessing entity for accessibility checking.</param>
    /// <param name="typeArguments">The collection of already resolved type arguments.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    /// <remarks>
    /// <para>This static method is public because the same algorithm is also used in SimpleNameResolver.</para>
    /// <para>If an error occurs then a NamespaceOrTypeNameResolverException is raised.</para>
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public static NamespaceOrTypeEntity ResolveSingleTypeTagInNamespaces(
      TypeTagNode typeTagNode,
      ISemanticEntity resolutionContextEntity,
      ISemanticEntity accessingEntity,
      List<TypeEntity> typeArguments,
      ICompilationErrorHandler errorHandler)
    {
      // If the previous steps were unsuccessful then, for each namespace N, 
      // starting with the namespace in which the namespace-or-type-name occurs, 
      // continuing with each enclosing namespace (if any), 
      // and ending with the global namespace, the following steps are evaluated until an entity is located:
      var namespaceContext = resolutionContextEntity.GetEnclosing<NamespaceEntity>();

      while (namespaceContext != null)
      {
        // If K is zero ... 
        if (typeTagNode.GenericDimensions == 0)
        {
          // ... and I is the name of a namespace in N, then:
          var foundNamespaceEntity = namespaceContext.GetChildNamespace(typeTagNode.Identifier);

          if (foundNamespaceEntity != null)
          {
            // If the location where the namespace-or-type-name occurs is enclosed by a namespace declaration for N 
            // and the namespace declaration contains an extern-alias-directive or using-alias-directive 
            // that associates the name I with a namespace or type, ...
            if (namespaceContext.GetUsingAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null
              || namespaceContext.GetExternAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null)
            {
              // ... then the namespace-or-type-name is ambiguous and a compile-time error occurs. (CS0576)
              throw new AliasNameConflictException(namespaceContext.FullyQualifiedName, typeTagNode.Identifier);
            }

            // Otherwise, the namespace-or-type-name refers to the namespace named I in N.
            return foundNamespaceEntity;
          }
        }

        // Otherwise, if N contains an accessible type having name I and K type parameters, then:

        // (First we try to find a type with name I and K type parameters, and we'll check the accessibility later.)
        var foundTypeEntity = namespaceContext.GetSingleChildType<TypeEntity>(typeTagNode.Identifier, typeTagNode.GenericDimensions);
        if (foundTypeEntity != null)
        {
          // (Checking whether the found entity is indeed accessible.)
          if (!foundTypeEntity.IsAccessibleBy(accessingEntity))
          {
            throw new EntityIsInaccessibleException(foundTypeEntity);
          }

          // If K is zero and the location where the namespace-or-type-name occurs is enclosed by a namespace declaration for N 
          // and the namespace declaration contains an extern-alias-directive or using-alias-directive 
          // that associates the name I with a namespace or type, then the namespace-or-type-name is ambiguous 
          // and a compile-time error occurs.
          if (typeTagNode.GenericDimensions == 0 &&
            (namespaceContext.GetUsingAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null
              || namespaceContext.GetExternAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null)
            )
          {
            // ... then the namespace-or-type-name is ambiguous and a compile-time error occurs. (CS0576)
            throw new AliasNameConflictException(namespaceContext.FullyQualifiedName, typeTagNode.Identifier);
          }

          // Otherwise, the namespace-or-type-name refers to the type constructed with the given type arguments.
          return GetConstructedTypeEntity(foundTypeEntity, typeArguments);
        }

        // Otherwise, if the location where the namespace-or-type-name occurs is enclosed by a namespace declaration for N:
        //  --> The location is tested in NamespaceEntity.GetUsingAliasByNameAndSourcePoint and GetUsingNamespacesBySourcePoint

        // If K is zero ...
        if (typeTagNode.GenericDimensions == 0)
        {
          // ... and the namespace declaration contains an extern-alias-directive or using-alias-directive 
          // that associates the name I with an imported namespace or type, ...
          var usingAliasEntity = namespaceContext.GetUsingAliasByNameAndSourcePoint(typeTagNode.Identifier,
                                                                                    typeTagNode.SourcePoint);
          if (usingAliasEntity != null
            && usingAliasEntity.NamespaceOrTypeReference != null
            && usingAliasEntity.NamespaceOrTypeReference.Target != null)
          {
            // ... then the namespace-or-type-name refers to that namespace or type.
            return usingAliasEntity.NamespaceOrTypeReference.Target;
          }
        }

        // Otherwise, if the namespaces imported by the using-namespace-directives of the namespace declaration
        // contain ... type having name I and K type parameters, ...
        var typeEntities = FindTypeInImportedNamespaces(typeTagNode, namespaceContext);


        // ... contain exactly one type ...
        if (typeEntities.Count == 1)
        {
          // ... then the namespace-or-type-name refers to that type constructed with the given type arguments.
          return GetConstructedTypeEntity(typeEntities[0], typeArguments);
        }

        // ...  contain more than one type ...
        if (typeEntities.Count > 1)
        {
          // ... then the namespace-or-type-name is ambiguous and an error occurs.
          throw new AmbigousReferenceException(typeTagNode.ToString(), typeEntities[0].FullyQualifiedName, typeEntities[1].FullyQualifiedName);
        }

        // "... continuing with each enclosing namespace (if any), ..."
        namespaceContext = namespaceContext.Parent.GetEnclosing<NamespaceEntity>();
      }

      // Otherwise, the namespace-or-type-name is undefined and a compile-time error occurs. (CS0246)
      throw new NamespaceOrTypeNameNotResolvedException(typeTagNode.ToString());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type argument AST nodes and return a list of type entities.
    /// </summary>
    /// <param name="typeArgumentNodes">A collection if type argument AST nodes.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>A collection of type entities.</returns>
    // ----------------------------------------------------------------------------------------------
    public static List<TypeEntity> ResolveTypeArguments(
      TypeNodeCollection typeArgumentNodes, 
      ISemanticEntity resolutionContextEntity,
      ICompilationErrorHandler errorHandler)
    {
      var typeArguments = new List<TypeEntity>();

      if (typeArgumentNodes != null)
      {
        foreach (var typeArgumentSyntaxNode in typeArgumentNodes)
        {
          var typeArgumentResolver = new TypeNodeToTypeEntityResolver(typeArgumentSyntaxNode);
          typeArgumentResolver.Resolve(resolutionContextEntity, errorHandler);

          if (typeArgumentResolver.ResolutionState != ResolutionState.Resolved)
          {
            // No need to signal error here, because the resolution has already signaled it, just bail out.
            return null;
          }

          typeArguments.Add(typeArgumentResolver.Target);
        }
      }

      return typeArguments;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a constructed type entity based an a type entity and a collection of type arguments.
    /// The type argument list can be empty in which case the type entity itself is returned.
    /// </summary>
    /// <param name="typeEntity">A type entity (the template of the consturcted type).</param>
    /// <param name="typeArguments">A collection of type arguments. Can be empty.</param>
    /// <returns>
    /// The parameter type entity (if there are no type arguments), or a constructed type entity.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeEntity GetConstructedTypeEntity(TypeEntity typeEntity, IEnumerable<TypeEntity> typeArguments)
    {
      if (typeArguments.Count() == 0)
      {
        return typeEntity;
      }

      var genericCapableTypeEntity = typeEntity as GenericCapableTypeEntity;

      // Check for unnecessary type arguments.
      if (genericCapableTypeEntity == null || !genericCapableTypeEntity.IsGeneric)
      {
        // TODO: error CS0308: The non-generic type 'A' cannot be used with type arguments
        return null;
      }

      // Check if the number of type arguments are not right.
      if (genericCapableTypeEntity.IsGeneric && typeArguments.Count() != genericCapableTypeEntity.OwnTypeParameterCount)
      {
        // TODO: error CS0305: Using the generic type 'A<T>' requires '1' type arguments
        return null;
      }

      var newTypeParameterMap = new TypeParameterMap(genericCapableTypeEntity.OwnTypeParameters, typeArguments);
      var fullTypeParameterMap = genericCapableTypeEntity.TypeParameterMap.MapTypeArguments(newTypeParameterMap);

      return genericCapableTypeEntity.GetGenericClone(fullTypeParameterMap) as TypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies qualified alias member resolution logic described in the spec (§9.7 Namespace alias qualifiers).
    /// A qualified-alias-member has one of two forms:
    /// - N::I{A1, ..., AK}, where N and I represent identifiers, and {A1, ..., AK} is a type argument list. (K is always at least one.)
    /// - N::I, where N and I represent identifiers. (In this case, K is considered to be zero.)
    /// </summary>
    /// <param name="qualifier">An identifier, the left-hand side of the qualified alias member.</param>
    /// <param name="identifier">An identifier, the right-hand side of the qualified alias member.</param>
    /// <param name="arguments">The collection of type arguments in the qualified alias member.</param>
    /// <param name="sourcePoint">The source point of the qualified member.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <param name="globalNamespace">The global root namespace.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>A NamespaceOrTypeEntity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    public static NamespaceOrTypeEntity ResolveQualifiedAliasMember(
      string qualifier,
      string identifier,
      TypeNodeCollection arguments,
      SourcePoint sourcePoint,
      ISemanticEntity resolutionContextEntity,
      RootNamespaceEntity globalNamespace,
      ICompilationErrorHandler errorHandler)
    {
      // A qualified-alias-member has one of two forms:
      // - N::I{A1, ..., AK}, where N and I represent identifiers, and {A1, ..., AK} is a type argument list. (K is always at least one.)
      // - N::I, where N and I represent identifiers. (In this case, K is considered to be zero.)
      // Using this notation, the meaning of a qualified-alias-member is determined as follows:

      var N = qualifier;
      var I = identifier;
      var A = arguments;
      var K = arguments == null ? 0 : arguments.Count;

      // Resolve all type argument nodes.
      var typeArguments = ResolveTypeArguments(A, resolutionContextEntity, errorHandler);

      // If N is the identifier global, then the global namespace is searched for I:
      if (N == globalNamespace.Name)
      {
        // If the global namespace contains a namespace named I and K is zero, ...
        if (K == 0)
        {
          var namespaceEntity = globalNamespace.GetChildNamespace(I);
          if (namespaceEntity != null)
          {
            // ... then the qualified-alias-member refers to that namespace.
            return namespaceEntity;
          }
        }

        // Otherwise, if the global namespace contains a non-generic type named I and K is zero, ...
        // Otherwise, if the global namespace contains a type named I that has K type parameters, ... 
        var typeEntity = globalNamespace.GetSingleChildType<TypeEntity>(I, K);
        if (typeEntity != null)
        {
          // ... then the qualified-alias-member refers to that type.
          // ... then the qualified-alias-member refers to that type constructed with the given type arguments. 
          return GetConstructedTypeEntity(typeEntity, typeArguments); ;
        }

        // Otherwise, the qualified-alias-member is undefined and a compile-time error occurs.
        throw new NamespaceOrTypeNameNotResolvedException(N + "::" + I + A.ToString());
      }

      // Otherwise, starting with the namespace declaration immediately containing the qualified-alias-member (if any), 
      var namespaceContext = resolutionContextEntity.GetEnclosing<NamespaceEntity>();
      // continuing with each enclosing namespace declaration (if any), 
      // and ending with the compilation unit containing the qualified-alias-member, ...
      while (namespaceContext != null)
      {
        // ... the following steps are evaluated until an entity is located:

        // If the namespace declaration or compilation unit contains a using-alias-directive ...
        var usingAliasEntity = namespaceContext.GetUsingAliasByNameAndSourcePoint(N, sourcePoint);
        if (usingAliasEntity != null)
        {
          // ... that associates N with a type, ...
          if (usingAliasEntity.AliasedType != null)
          {
            // ... then the qualified-alias-member is undefined and a compile-time error occurs.
            throw new QualifierRefersToType(N);
          }
        }

        // Otherwise, if the namespace declaration or compilation unit 
        // contains an extern-alias-directive or using-alias-directive that associates N with a namespace, then:
        var externAliasEntity = namespaceContext.GetExternAliasByNameAndSourcePoint(N, sourcePoint);
        if ((usingAliasEntity != null && usingAliasEntity.AliasedNamespace != null)
          || (externAliasEntity != null && externAliasEntity.AliasedRootNamespace != null))
        {
          NamespaceEntity qualifierNamespaceEntity =
            (usingAliasEntity != null ? usingAliasEntity.AliasedNamespace : null)
            ?? (externAliasEntity != null ? externAliasEntity.AliasedRootNamespace : null);

          // If the namespace associated with N contains a namespace named I and K is zero, ... 
          if (K == 0)
          {
            var childNamespaceEntity = qualifierNamespaceEntity.GetChildNamespace(I);

            if (childNamespaceEntity != null)
            {
              // ... then the qualified-alias-member refers to that namespace.
              return childNamespaceEntity;
            }
          }

          // Otherwise, if the namespace associated with N contains a non-generic type named I and K is zero, ...
          // Otherwise, if the namespace associated with N contains a type named I that has K type parameters, ...
          var childTypeEntity = qualifierNamespaceEntity.GetSingleChildType<TypeEntity>(I, K);
          if (childTypeEntity != null)
          {
            // ... then the qualified-alias-member refers to that type.
            // ... then the qualified-alias-member refers to that type constructed with the given type arguments.
            return GetConstructedTypeEntity(childTypeEntity, typeArguments); ;
          }

          // Otherwise, the qualified-alias-member is undefined and a compile-time error occurs.
          throw new NamespaceOrTypeNameNotResolvedException(N + "::" + I + A.ToString());
        }

        // "... continuing with each enclosing namespace declaration (if any), ..."
        namespaceContext = namespaceContext.Parent.GetEnclosing<NamespaceEntity>();
      }

      // Otherwise, the qualified-alias-member is undefined and a compile-time error occurs.
      throw new NamespaceOrTypeNameNotResolvedException(N + "::" + I + A.ToString());

      // _Note that using the namespace alias qualifier with an alias that references a type causes a compile-time error. 
      // Also _note that if the identifier N is global, then lookup is performed in the global namespace, 
      // even if there is a using alias associating global with a type or namespace.
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds or constructs the type entity denoted by a type node.
    /// </summary>
    /// <param name="typeNode">A type AST node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeEntity ResolveTypeNode(
      TypeNode typeNode,
      ISemanticEntity resolutionContextEntity,
      ICompilationErrorHandler errorHandler)
    {
      TypeEntity typeEntity = null;

      var semanticGraph = resolutionContextEntity.SemanticGraph;

      // First, try to resolve as built-in type.
      if (typeNode.TypeName.TypeTags.Count == 1)
      {
        typeEntity = ResolveBuiltInTypeName(typeNode.TypeName.TypeTags[0].Identifier, semanticGraph);
      }

      // If not found, then continue with the resolution
      if (typeEntity == null)
      {
        // Resolve the underlying type.
        var namespaceOrTypeNameResolver = new NamespaceOrTypeNameNodeToTypeEntityResolver(typeNode.TypeName);
        namespaceOrTypeNameResolver.Resolve(resolutionContextEntity, errorHandler);
        typeEntity = namespaceOrTypeNameResolver.Target;

        // If no success then just return null. Errors were already signaled in NamespaceOrTypeNameResolver.
        if (typeEntity == null)
        {
          return null;
        }
      }

      // If the AST node has a nullable type indicating token, then create nullable type.
      if (typeNode.NullableToken != null)
      {
        var typeParameterMap = new TypeParameterMap(semanticGraph.NullableGenericTypeDefinition.OwnTypeParameters, new[] { typeEntity });
        typeEntity = semanticGraph.NullableGenericTypeDefinition.GetGenericClone(typeParameterMap) as TypeEntity;
      }

      // If the AST node has pointer token(s), then create pointer type(s).
      foreach (var pointerToken in typeNode.PointerTokens)
      {
        typeEntity = PointerTypeEntity.GetConstructedPointerType(typeEntity);
      }

      // If the AST node has rank specifier(s), then create array type(s).
      foreach (var rankSpecifier in typeNode.RankSpecifiers)
      {
        typeEntity = ArrayTypeEntity.GetConstructedArrayType(typeEntity, rankSpecifier.Rank);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve an identifier to a builtin type.
    /// </summary>
    /// <param name="identifier">An identifier to be resolved to a builtin type.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeEntity ResolveBuiltInTypeName(string identifier, SemanticGraph semanticGraph)
    {
      System.Type type = null;

      switch (identifier)
      {
        case "bool":
          type = typeof(bool);
          break;
        case "byte":
          type = typeof(byte);
          break;
        case "char":
          type = typeof(char);
          break;
        case "decimal":
          type = typeof(decimal);
          break;
        case "double":
          type = typeof(double);
          break;
        case "float":
          type = typeof(float);
          break;
        case "int":
          type = typeof(int);
          break;
        case "long":
          type = typeof(long);
          break;
        case "object":
          type = typeof(object);
          break;
        case "sbyte":
          type = typeof(sbyte);
          break;
        case "short":
          type = typeof(short);
          break;
        case "string":
          type = typeof(string);
          break;
        case "uint":
          type = typeof(uint);
          break;
        case "ulong":
          type = typeof(ulong);
          break;
        case "ushort":
          type = typeof(ushort);
          break;
        case "void":
          type = typeof(void);
          break;
        default:
          // Not a builtin type
          break;
      }

      if (type != null)
      {
        return semanticGraph.GetEntityByMetadataObject(type) as TypeEntity;
      }

      return null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the resolution logic of the following cases.
    ///  - I
    ///  - I{A1, ..., AK}
    ///  - N.I
    ///  - N.I{A1, ..., AK}
    /// where I is a single identifier, N is a namespace-or-type-name 
    /// and {A1, ..., AK} is an optional type-argument-list.
    /// And the name is not a qualified-alias-member.
    /// </summary>
    /// <param name="typeTags">A namespace-or-type-name with any number of type-tags.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <param name="accessingEntity">The accessing entity for accessibility checking.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    /// <remarks>If an error occurs then a NamespaceOrTypeNameResolverException is raised.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static NamespaceOrTypeEntity ResolveTypeTags(
      TypeTagNodeCollection typeTags, 
      ISemanticEntity resolutionContextEntity, 
      ISemanticEntity accessingEntity,
      ICompilationErrorHandler errorHandler)
    {
      NamespaceOrTypeEntity foundEntity = null;

      // If the namespace-or-type-name is of the form I or of the form I<A1, ..., AK>
      if (typeTags.Count == 1)
      {
        foundEntity = ResolveSingleTypeTag(typeTags[0], resolutionContextEntity, accessingEntity, errorHandler);
      }
      else
      {
        // Otherwise, the namespace-or-type-name is of the form N.I or of the form N.I<A1, ..., AK>. 
        // N is first resolved as a namespace-or-type-name.  
        var handleEntity = ResolveTypeTags(typeTags.GetCopyWithoutLastTag(), resolutionContextEntity, accessingEntity, errorHandler);
        var lastTypeTag = typeTags[typeTags.Count - 1];

        // Resolve all type argument nodes.
        var typeArguments = ResolveTypeArguments(lastTypeTag.Arguments, resolutionContextEntity, errorHandler);

        // If the resolution of N is not successful, a compile-time error occurs. 
        if (handleEntity == null)
        {
          throw new NamespaceOrTypeNameNotResolvedException(typeTags.ToString());
        }

        // Otherwise, N.I or N.I<A1, ..., AK> is resolved as follows:

        // If K is zero and N refers to a namespace ... 
        if (lastTypeTag.GenericDimensions == 0 && handleEntity is NamespaceEntity)
        {
          // ... and N contains a nested namespace with name I, ...
          var foundNamespaceEntity = (handleEntity is NamespaceEntity)
            ? (handleEntity as NamespaceEntity).GetChildNamespace(lastTypeTag.Identifier)
            : null;

          if (foundNamespaceEntity != null)
          {
            // ... then the namespace-or-type-name refers to that nested namespace.
            return foundNamespaceEntity;
          }
        }

        // Otherwise, if N refers to a namespace ... 
        if (handleEntity is NamespaceEntity)
        {
          // ... and N contains an accessible type having name I and K type parameters, ...
          
          // (First we try to find a type with name I and K type parameters, and we'll check the accessibility later.)
          var foundTypeEntity = (handleEntity is IHasChildTypes)
            ? (handleEntity as IHasChildTypes).GetSingleChildType<TypeEntity>(lastTypeTag.Identifier, lastTypeTag.GenericDimensions)
            : null;

          if (foundTypeEntity != null)
          {
            // (Checking whether the found entity is indeed accessible.)
            if (!foundTypeEntity.IsAccessibleBy(accessingEntity))
            {
              throw new EntityIsInaccessibleException(foundTypeEntity);
            }

            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments.
            return GetConstructedTypeEntity(foundTypeEntity, typeArguments);
          }
        }

        // Otherwise, if N refers to a (possibly constructed) class or struct type ...
        if (handleEntity is TypeEntity)
        {
          // ... and N or any of its base classes contain a nested accessible type having name I and K type parameters,
          var foundNestesTypeEntity = FindNestedAccessibleTypeInTypeOrBaseTypes(lastTypeTag, handleEntity as TypeEntity, accessingEntity);

          if (foundNestesTypeEntity != null)
          {
            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments. 
            return GetConstructedTypeEntity(foundNestesTypeEntity, typeArguments);
          }
        }

        // Otherwise, N.I is an invalid namespace-or-type-name, and a compile-time error occurs.
        throw new NamespaceOrTypeNameNotResolvedException(typeTags.ToString());
      }

      return foundEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the resolution logic of a single-tag namespace-or-type-name.
    ///  - I
    ///  - I{A1, ..., AK}
    /// where I is a single identifier, and {A1, ..., AK} is an optional type-argument-list.
    /// And the name is not a qualified-alias-member.
    /// </summary>
    /// <param name="typeTagNode">A single-tag namespace-or-type-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <param name="accessingEntity">The accessing entity for accessibility checking.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    /// <remarks>If an error occurs then a NamespaceOrTypeNameResolverException is raised.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static NamespaceOrTypeEntity ResolveSingleTypeTag(
      TypeTagNode typeTagNode, 
      ISemanticEntity resolutionContextEntity, 
      ISemanticEntity accessingEntity,
      ICompilationErrorHandler errorHandler)
    {
      // If K is zero ...
      if (!typeTagNode.HasTypeArguments)
      {
        // ... and the namespace-or-type-name appears within a generic method declaration (§10.6) ...
        var genericMethodEntity = resolutionContextEntity.GetEnclosingGenericMethodDeclaration();
        if (genericMethodEntity != null)
        {
          // ... and if that declaration includes a type parameter (§10.1.3) with name I, ...
          var methodTypeParameter = genericMethodEntity.GetOwnTypeParameterByName(typeTagNode.Identifier);
          if (methodTypeParameter != null)
          {
            // ... then the namespace-or-type-name refers to that type parameter.
            return methodTypeParameter;
          }
        }
      }

      // Resolve all type argument nodes.
      var typeArguments = ResolveTypeArguments(typeTagNode.Arguments, resolutionContextEntity, errorHandler);

      // Otherwise, if the namespace-or-type-name appears within a type declaration, 
      // then for each instance type T (§10.3.1), starting with the instance type of that type declaration
      // and continuing with the instance type of each enclosing class or struct declaration (if any):
      var typeContext = resolutionContextEntity.GetEnclosing<TypeEntity>();

      while (typeContext != null)
      {
        // If K is zero ...
        if (typeTagNode.Arguments.Count == 0)
        {
          // ... and the declaration of T includes a type parameter with name I, ...
          var foundTypeParameterEntity = (typeContext is GenericCapableTypeEntity)
                                           ? (typeContext as GenericCapableTypeEntity).GetOwnTypeParameterByName(typeTagNode.Identifier)
                                           : null;

          if (foundTypeParameterEntity != null)
          {
            // then the namespace-or-type-name refers to that type parameter.
            return foundTypeParameterEntity;
          }
        }

        // Otherwise, if the namespace-or-type-name appears within the body of the type declaration, ...
        // TODO: how to interpret the line above? If implemented with the condition below then it fails to find some base classes
        //if (resolutionContextEntity.IsInTypeDeclarationBody())
        {
          // ... and T or any of its base types contain a nested accessible type having name I and K type parameters, ...
          var foundNestesTypeEntity = FindNestedAccessibleTypeInTypeOrBaseTypes(typeTagNode, typeContext, accessingEntity);

          if (foundNestesTypeEntity != null)
          {
            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments. 
            return GetConstructedTypeEntity(foundNestesTypeEntity, typeArguments);
          }
        }

        // "... and continuing with the instance type of each enclosing class or struct declaration (if any):"
        typeContext = typeContext.Parent.GetEnclosing<TypeEntity>();
      }

      // If not yet resolved then try top resolve in the enclosing namespaces.
      return ResolveSingleTypeTagInNamespaces(typeTagNode, resolutionContextEntity, accessingEntity, typeArguments, errorHandler);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds a nested accessible type by a typeTagNode in a type entity or any of its base types.
    /// </summary>
    /// <param name="typeTagNode">The typeTag to be found.</param>
    /// <param name="contextType">The type where the searching starts.</param>
    /// <param name="accessingEntity">The accessing entity for accessibility checking.</param>
    /// <returns>A TypeEntity with the name and number of type parameters defined in typeTagNode, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity FindNestedAccessibleTypeInTypeOrBaseTypes(
      TypeTagNode typeTagNode, 
      TypeEntity contextType, 
      ISemanticEntity accessingEntity)
    {
      // Spec: If there is more than one such type, the type declared within the more derived type is selected. 
      
      // Supplemental info:
      // The spec is not explicit about it, but csc.exe works like this:
      // if an inaccessible type is found in a more derived type 
      // and an accessible type is found in a less derived type,
      // then the one in the less derived type is selected, obviously, because the other one is inaccessible.
      
      // TODO: If only an inaccessible type is found, 
      // then "CS0122: 'member' is inaccessible due to its protection level" error is reported
      // instead of the usual "CS0246: The type or namespace name '{0}' could not be found" error.

      // Spec: _Note that non-type members (constants, fields, methods, properties, indexers, operators, instance constructors, 
      // destructors, and static constructors) and type members with a different number of type parameters 
      // are ignored when determining the meaning of the namespace-or-type-name.

      // Spec: _Note that if the meaning of N.I is being determined as part of resolving the base class specification of N 
      // then the direct base class of N is considered to be object (§10.1.4.1).
      // TODO: how to interpret and test this?

      TypeEntity accessibleTypeEntity = null;

      // We can stop looking for a type only if an accessible one is found, or we reached the end of the inheritance hierarchy. 
      while (accessibleTypeEntity == null && contextType != null)
      {
        var candidateTypeEntity = (contextType is IHasChildTypes)
          ? (contextType as IHasChildTypes).GetSingleChildType<TypeEntity>(typeTagNode.Identifier, typeTagNode.GenericDimensions)
          : null;

        // It's a match only if it's also accessible.
        if (candidateTypeEntity != null && candidateTypeEntity.IsAccessibleBy(accessingEntity))
        {
          accessibleTypeEntity = candidateTypeEntity;
        }

        contextType = contextType.BaseClass;
      }

      return accessibleTypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds a type specified by a TypeTagNode in imported namespaces.
    /// Can find zero, one, or many matching types.
    /// </summary>
    /// <param name="typeTagNode">A TypeTagNode that specifies the name and number of type arguments of the type to be found.</param>
    /// <param name="namespaceContext">The namespace whose imported namespaces will be searched.</param>
    /// <returns>A (possibly empty) list of type entities found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static IList<TypeEntity> FindTypeInImportedNamespaces(TypeTagNode typeTagNode, NamespaceEntity namespaceContext)
    {
      var typeEntities = new List<TypeEntity>();

      // Get all using namespace directives from namespace-context which has the type-tag in scope.
      var usingNamespaceEntities = namespaceContext.GetUsingNamespacesBySourcePoint(typeTagNode.SourcePoint);

      // Check all imported namespaces.
      foreach (var usingNamespaceEntity in usingNamespaceEntities)
      {
        if (usingNamespaceEntity.ImportedNamespace != null)
        {
          // Find out if the imported namespace contains a type declared with the given name and number of type parameters
          var typeEntity = usingNamespaceEntity.ImportedNamespace.GetSingleChildType<TypeEntity>(typeTagNode.Identifier, typeTagNode.GenericDimensions);

          // If a type is found then add it to the result list.
          if (typeEntity != null)
          {
            typeEntities.Add(typeEntity);
          }
        }
      }

      return typeEntities;
    }

    #endregion
  }
}
