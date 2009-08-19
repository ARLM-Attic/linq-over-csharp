using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type resolver visitors, 
  /// and contains the common name resolution logic.
  /// </summary>
  /// <remarks>
  /// Type resolution cannot be performed in 1 pass, at least 2 is necessary.
  /// <para>(1) Resolve type references till the type declaration level.</para>
  /// <para>(2) Resolve type references in type bodies.</para>
  /// The second pass builds on the first, because it also searches in the resolved base classes.
  /// </remarks>
  // ================================================================================================
  public abstract class TypeResolverSemanticGraphVisitorBase : SemanticGraphVisitor
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that this visitor is working on.</summary>
    private readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverSemanticGraphVisitorBase"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeResolverSemanticGraphVisitorBase(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    #region Protected methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a type reference.
    /// </summary>
    /// <param name="reference">A reference to a TypeEntity to be resolved.</param>
    /// <param name="resolutionContextEntity">An entity that is the context (or starting point) of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    protected void ResolveTypeEntityReference(SemanticEntityReference<TypeEntity> reference, SemanticEntity resolutionContextEntity)
    {
      // If already resolved then bail out.
      if (reference.ResolutionState == ResolutionState.Resolved)
      {
        return;
      }

      TypeEntity resolvedEntity;

      // Invoke the proper resolver method
      if (reference is ReflectedTypeBasedTypeEntityReference)
      {
        resolvedEntity = GetTypeEntityByReflectedType(((ReflectedTypeBasedTypeEntityReference)reference).Metadata);
      }
      else if (reference is TypeOrNamespaceNodeBasedTypeEntityReference && resolutionContextEntity is NamespaceOrTypeEntity)
      {
        resolvedEntity = GetTypeEntityByTypeOrNamespaceNode(((TypeOrNamespaceNodeBasedTypeEntityReference)reference).SyntaxNode, 
                                                             resolutionContextEntity as NamespaceOrTypeEntity);
      }
      else
      {
        throw new ApplicationException(string.Format("Unhandled case in ResolveTypeEntityReference, TargetEntity type='{0}'",
                                                     reference.GetType()));
      }

      // Set the reference to the correct state
      if (resolvedEntity != null)
      {
        reference.SetResolved(resolvedEntity);
      }
      else
      {
        reference.SetUnresolvable();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace reference.
    /// </summary>
    /// <param name="reference">A reference to a NamespaceEntity to be resolved.</param>
    /// <param name="resolutionContextEntity">An entity that is the context (or starting point) of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    protected void ResolveNamespaceEntityReference(SemanticEntityReference<NamespaceEntity> reference, SemanticEntity resolutionContextEntity)
    {
      // If already resolved then bail out.
      if (reference.ResolutionState == ResolutionState.Resolved)
      {
        return;
      }

      NamespaceEntity resolvedEntity;

      if (reference is TypeOrNamespaceNodeBasedNamespaceEntityReference && resolutionContextEntity is NamespaceEntity)
      {
        resolvedEntity = GetNamespaceEntityByTypeOrNamespaceNode(
          ((TypeOrNamespaceNodeBasedNamespaceEntityReference) reference).SyntaxNode,
          resolutionContextEntity as NamespaceEntity);
      }
      else
      {
        throw new ApplicationException(string.Format("Unhandled case in ResolveNamespaceEntityReference, TargetEntity type='{0}'",
                                                     reference.GetType()));
      }

      // Set the reference to the correct state
      if (resolvedEntity != null)
      {
        reference.SetResolved(resolvedEntity);
      }
      else
      {
        reference.SetUnresolvable();
      }
    }

    #endregion

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the TypeEntity that belongs to a System.Type object.
    /// </summary>
    /// <param name="type">A System.Type object.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity GetTypeEntityByReflectedType(System.Type type)
    {
      TypeEntity typeEntity = null;

      // If it's not a constructed generic type, then find its corresponding entity
      if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        // If it's a constructed generic type, then find its type definition's entity
        var genericTypeDefinition = type.GetGenericTypeDefinition();
        var typeDefinitionEntity = _SemanticGraph.GetEntityByMetadataObject(genericTypeDefinition);

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

        // Resolve type arguments
        var argumentEntities = new List<TypeEntity>();
        foreach (var genericArgument in type.GetGenericArguments())
        {
          var argumentEntity = GetTypeEntityByReflectedType(genericArgument);
          if (argumentEntity==null)
          {
            throw new ApplicationException(string.Format("Could not resolve type '{0}' to entity.",
                                                         genericArgument.Name));
          }
          argumentEntities.Add(argumentEntity);
        }

        // Get the constructed type
        typeEntity = GetConstructedGenericType(typeDefinitionEntity as GenericCapableTypeEntity, argumentEntities);
      }
      else
      {
        var foundEntity = _SemanticGraph.GetEntityByMetadataObject(type);

        if (foundEntity != null && !(foundEntity is TypeEntity))
        {
          throw new ApplicationException(
            string.Format("Expected to find a TypeEntity for type '{0}', but found a '{1}'.", type.Name,
                          foundEntity.GetType()));
        }
        typeEntity = foundEntity as TypeEntity;
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds a namespace entity denoted by a type-or-namespace node.
    /// </summary>
    /// <param name="typeOrNamespaceNode">A type-or-namespace node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <returns>A NamespaceEntity or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private NamespaceEntity GetNamespaceEntityByTypeOrNamespaceNode(NamespaceOrTypeNameNode typeOrNamespaceNode, 
                                                                    NamespaceEntity resolutionContextEntity)
    {
      // Try to find the entity by traversing the semantic graph.
      SemanticEntity foundEntity = FindEntityInSemanticGraph(typeOrNamespaceNode.TypeTags, resolutionContextEntity,
                                                             false, false);

      // If an entity was found, but it's not a NamespaceEntity, then signal error
      if (foundEntity != null && !(foundEntity is NamespaceEntity))
      {
        if (foundEntity is TypeEntity)
        {
          _ErrorHandler.Error("CS0138", typeOrNamespaceNode.StartToken,
                              "A using namespace directive can only be applied to namespaces; '{0}' is a type not a namespace",
                              typeOrNamespaceNode.ToString());
        }
        else
        {
          throw new ApplicationException(string.Format("Unexpected type of entity found: '{0}'.", foundEntity.GetType()));
        }
        return null;
      }

      // If couldn't resolve then signal error
      if (foundEntity == null)
      {
        _ErrorHandler.Error("CS0246", typeOrNamespaceNode.StartToken,
                            "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
                            typeOrNamespaceNode.ToString());
        return null;
      }

      // The entity is found, and is a namespace entity.
      return foundEntity as NamespaceEntity;      
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the type entity denoted by a type-or-namespace node.
    /// </summary>
    /// <param name="typeOrNamespaceNode">A type-or-namespace node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity GetTypeEntityByTypeOrNamespaceNode(TypeNode typeOrNamespaceNode,
                                                          NamespaceOrTypeEntity resolutionContextEntity)
    {
      // First, try to resolve as built-in type.
      TypeEntity typeEntity = FindBuiltInTypeByTypeOrNamespaceNode(typeOrNamespaceNode);

      // If not found, then continue with the resolution
      if (typeEntity == null)
      {
        SemanticEntity foundEntity = null;

        try
        {
          // Try to find the entity by traversing the semantic graph.
          foundEntity = FindEntityInSemanticGraph(typeOrNamespaceNode.TypeName.TypeTags, resolutionContextEntity, true, true);
        }
        catch (AmbigousReferenceInImportedNamespacesException e)
        {
          _ErrorHandler.Error("CS0104", typeOrNamespaceNode.TypeName.TypeTags.StartToken,
                              "'{0}' is an ambiguous reference between '{1}' and '{2}'",
                              e.Reference, e.Identifier1, e.Identifier2);
          return null;
        }

        // If an entity was found, but it's not a TypeEntity, then signal error
        if (foundEntity != null && !(foundEntity is TypeEntity))
        {
          _ErrorHandler.Error("CS0118", typeOrNamespaceNode.StartToken, "'{0}' is a '{1}' but is used like a type.",
                              typeOrNamespaceNode.ToString(), foundEntity.GetType());
          return null;
        }

        // If couldn't resolve then signal error
        if (foundEntity == null)
        {
          _ErrorHandler.Error("CS0246", typeOrNamespaceNode.StartToken,
                              "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
                              typeOrNamespaceNode.ToString());
          return null;
        }

        // The entity is found, and is a type entity.
        typeEntity = foundEntity as TypeEntity;
      }
      
      // We have to collect all type arguments, because constructed generic types need their parents's type arguments as well.
      var typeArgumentNodes = GetTypeArgumentNodesFromTypeOrNamespaceNode(typeOrNamespaceNode);

      // If there are type arguments, but the found entity is not a generic type definition, then it's an error.
      if (typeArgumentNodes.Count > 0 && !(typeEntity is GenericCapableTypeEntity))
      {
        throw new ApplicationException(string.Format("Expected to find GenericCapableTypeEntity, but found '{0}'.",
                                                     typeEntity == null ? "(null)" : typeEntity.GetType().ToString()));
      }

      // If there are type arguments then we have to create a constructed generic type.
      if (typeArgumentNodes.Count > 0 && typeEntity is GenericCapableTypeEntity)
      {
        // Resolve all type arguments
        var typeArguments = new List<TypeEntity>();
        foreach (var typeArgumentSyntaxNode in typeArgumentNodes)
        {
          var typeArgumentRef = new TypeOrNamespaceNodeBasedTypeEntityReference(typeArgumentSyntaxNode);
          var typeArgument = GetTypeEntityByTypeOrNamespaceNode(typeArgumentRef.SyntaxNode, resolutionContextEntity);
          if (typeArgument == null)
          {
            // No need to signal error here, because the GetTypeEntityByTypeOrNamespaceNode method already signaled it, just bail out.
            return null;
          }
          typeArguments.Add(typeArgument);
        }

        typeEntity = GetConstructedGenericType(typeEntity as GenericCapableTypeEntity, typeArguments);
        if (typeEntity == null)
        {
          throw new ApplicationException("GetConstructedGenericType returned null.");
        }
      }

      // If the AST node has a nullable type indicating token, then create nullable type.
      if (typeOrNamespaceNode.NullableToken != null)
      {
        typeEntity = GetConstructedNullableType(typeEntity);
      }

      // If the AST node has pointer token(s), then create pointer type(s).
      bool isFirstStar = true;
      foreach (var pointerToken in typeOrNamespaceNode.PointerTokens)
      {
        // If it's pointer to unknown type (void*) then the first '*' should be swallowed, because that's part of 'void*'
        if (typeEntity is PointerToUnknownTypeEntity && isFirstStar)
        {
          isFirstStar = false;
        }
        else
        {
          typeEntity = GetConstructedPointerType(typeEntity);
        }
      }

      // If the AST node has rank specifier(s), then create array type(s).
      foreach (var rankSpecifier in typeOrNamespaceNode.RankSpecifiers)
      {
        typeEntity = GetConstructedArrayType(typeEntity, rankSpecifier.Rank);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Extracts the list of type arguments from a type-or-namespace AST node.
    /// </summary>
    /// <param name="typeOrNamespaceNode">A type-or-namespace node</param>
    /// <returns>The list of type arguments.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeNodeCollection GetTypeArgumentNodesFromTypeOrNamespaceNode(TypeNode typeOrNamespaceNode)
    {
      var typeArgumentNodes = new TypeNodeCollection();

      foreach (var typeTag in typeOrNamespaceNode.TypeName.TypeTags)
      {
        if (typeTag.HasTypeArguments)
        {
          foreach (var argument in typeTag.Arguments)
          {
            typeArgumentNodes.Add(argument);
          }
        }
      }

      return typeArgumentNodes;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find an entity in the following ways.
    /// <list type="number">
    /// <item>Look up in local declaration space.</item>
    /// <item>Look up in the declaration space of base types, recursively.</item>
    /// <item>Iterate the following steps for parent types/namespaces, till the root is reached.</item>
    ///   <list type="number">
    ///   <item>Look up in the local declaration space.</item>
    ///   <item>Look up as using alias.</item>
    ///   <item>Look up in using'd namespaces.</item>
    ///   </list>
    /// </list> 
    /// </summary>
    /// <param name="typeTags">The name of the entity to be found.</param>
    /// <param name="resolutionContextEntity">The entity, where the resolution starts.</param>
    /// <param name="considerBaseTypes">Look up in base types too.</param>
    /// <param name="considerUsings">Consider using namespace and using alias directives too.</param>
    /// <returns>A semantic entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity FindEntityInSemanticGraph(TypeTagNodeCollection typeTags,
                                                     NamespaceOrTypeEntity resolutionContextEntity,
                                                     bool considerBaseTypes,
                                                     bool considerUsings)
    {
      SemanticEntity foundEntity = null;

      // Step 1: Look up in local declaration space.
      foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<NamespaceOrTypeEntity>(typeTags, resolutionContextEntity);

      // Step 2: Look up in the declaration space of base types, recursively.
      if (foundEntity == null && considerBaseTypes && resolutionContextEntity is TypeEntity)
      {
        foundEntity = FindEntityByTraversingBaseClasses(typeTags, resolutionContextEntity as TypeEntity);
      }

      // If still not found then continue searching one level higher
      if (resolutionContextEntity.Parent is NamespaceOrTypeEntity)
      {
        resolutionContextEntity = resolutionContextEntity.Parent as NamespaceOrTypeEntity;
      }

      // Loop until the entity is found, or we run out of parent entities
      while (foundEntity == null && resolutionContextEntity != null)
      {
        // Step 3a: Look up in the declaration space of parent type/namespace.
        foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<NamespaceOrTypeEntity>(typeTags, resolutionContextEntity);

        // Step 3b: Look up as using alias.
        if (foundEntity == null && considerUsings && resolutionContextEntity is NamespaceEntity)
        {
          // TODO: implement this
        }

        // Step 4: Look up in imported (using) namespaces.
        if (foundEntity == null && considerUsings && resolutionContextEntity is NamespaceEntity)
        {
          foundEntity = FindTypeTagsInImportedNamespaces(typeTags, resolutionContextEntity as NamespaceEntity);
        }

        // If still not found then continue searching one level higher
        resolutionContextEntity = resolutionContextEntity.Parent as NamespaceOrTypeEntity;
      }

      return foundEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find the entity for TypeTags by looking in imported namespaces.
    /// </summary>
    /// <param name="typeTags">The name of the entity to be found.</param>
    /// <param name="resolutionContextEntity">
    /// The context of the resolution, a namespace whose usings will be examined.
    /// </param>
    /// <returns>A TypeEntity, or null if not found, or more than one found.</returns>
    /// <remarks>If more than one types are found in the imported namespaces that's an error.</remarks>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity FindTypeTagsInImportedNamespaces(TypeTagNodeCollection typeTags,
                                                        NamespaceEntity resolutionContextEntity)
    {
      // We have to check whether more than one type can be found because that means error CS0104.
      TypeEntity firstFoundEntity = null;
      TypeEntity secondFoundEntity = null;

      var sourcePoint = new SourcePoint(typeTags.CompilationUnitNode, typeTags.StartPosition);
      foreach (var usingNamespaceEntity in resolutionContextEntity.GetUsingNamespacesBySourcePoint(sourcePoint))
      {
        if (usingNamespaceEntity.ImportedNamespace != null)
        {
          // Try to find TypeTags in the imported namespace, but only TypeEntities count 
          var foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<TypeEntity>(typeTags,
                                                                                usingNamespaceEntity.ImportedNamespace);

          // Put the 1st and 2nd found entities in the right slot
          if (foundEntity != null && firstFoundEntity == null)
          {
            firstFoundEntity = foundEntity;
          }
          else if (foundEntity != null && secondFoundEntity == null)
          {
            secondFoundEntity = foundEntity;
          }


          // If two entities were found, then signal error and bail out.
          if (firstFoundEntity != null && secondFoundEntity != null)
          {
            throw new AmbigousReferenceInImportedNamespacesException(typeTags.ToString(),
                                                                     firstFoundEntity.FullyQualifiedName,
                                                                     secondFoundEntity.FullyQualifiedName);
          }
        }
      }

      return firstFoundEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find the entity for TypeTags by traversing base class, recursively.
    /// </summary>
    /// <param name="typeTags">The name of the entity to be found.</param>
    /// <param name="resolutionContextEntity">The current context of the resolution. Its base type will be examined first.</param>
    /// <returns>A semantic entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity FindEntityByTraversingBaseClasses(TypeTagNodeCollection typeTags, TypeEntity resolutionContextEntity)
    {
      SemanticEntity foundEntity = null;

      // Loop until the entity is found, or we run out of base classes
      while (foundEntity == null && resolutionContextEntity != null)
      {
        resolutionContextEntity = resolutionContextEntity.BaseType;

        if (resolutionContextEntity != null)
        {
          // Try to resolve the name in the base type's declaration space
          foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<TypeEntity>(typeTags, resolutionContextEntity);
        }
      }

      return foundEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find an entity that can be reached from the given resolution context entity
    /// by matching all type tags in a hierarchy of declaration spaces.
    /// </summary>
    /// <typeparam name="TTargetEntityType">Only these kind of entities will be found.</typeparam>
    /// <param name="typeTags">A multipart name.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <returns>A TTargetEntityType entity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private TTargetEntityType FindTypeTagsInDeclarationSpaceHierarchy<TTargetEntityType>(TypeTagNodeCollection typeTags,
                                                                                      NamespaceOrTypeEntity resolutionContextEntity)
      where TTargetEntityType : SemanticEntity
    {
      SemanticEntity entity = null;

      // Match all parts (typeTags) of the name (eg. A.B.C<D> = 3 TypeTags) 
      for (int i = 0; i < typeTags.Count; i++)
      {
        // Look up the TypeTage's name in the current context's declaration space.
        entity = FindTypeTagInDeclarationSpace(typeTags[i], resolutionContextEntity.DeclarationSpace);

        // If TypeTag was not found, or is not the expected type
        // then there's no point to continue matching the next tags, just return null (not found).
        if (entity == null || !(entity is TTargetEntityType))
        {
          return null;
        }

        // If not all typeTags were matched then we should continue with the next one, 
        // but if the current entity is not a NamespaceOrTypeEntity, then it's not possible to continue, so raise an error.
        if (i + 1 < typeTags.Count && !(entity is NamespaceOrTypeEntity))
        {
          throw new ApplicationException(string.Format("Expected a NamespaceOrTypeEntity but found a '{0}'.",
                                                       entity.GetType()));
        }

        // The resolution context of the next tag will be the current entity
        resolutionContextEntity = entity as NamespaceOrTypeEntity;
      }

      return entity as TTargetEntityType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find a NamespaceOrTypeEntity in a declaration space, that corresponds to a TypeTag syntax node.
    /// </summary>
    /// <param name="typeTag">A TypeTag syntax node that contains the name and type arguments to be resolved.</param>
    /// <param name="declarationSpace">A declaration space.</param>
    /// <returns>The found NamespaceOrTypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private NamespaceOrTypeEntity FindTypeTagInDeclarationSpace(TypeTagNode typeTag, DeclarationSpace declarationSpace)
    {
      // Assemble the name, possibly with type argument specifier (eg. Ident`1)
      var name = typeTag.Identifier + (typeTag.HasTypeArguments ? "`" + typeTag.Arguments.Count : "");

      // Look up the name
      var nameTableEntry = declarationSpace[name];

      // If the name is not found, then return null 
      if (nameTableEntry == null)
      {
        return null;
      }

      // If the name is found, but is not definite, then sign warning, and return null.
      if (nameTableEntry.State != NameTableEntryState.Definite)
      {
        _ErrorHandler.Warning("TBD", typeTag.StartToken, "TypeTag '{0}' is ambigous.", name);
        return null;
      }

      // If the name is found, and definite, but not a NamespaceOrTypeEntity then just return null.
      if (!(nameTableEntry.Entity is NamespaceOrTypeEntity))
      {
        return null;
      }

      return nameTableEntry.Entity as NamespaceOrTypeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find the built-in type (or void*) that is the meaning of a type-or-namespace node.
    /// </summary>
    /// <param name="typeOrNamespaceNode">A type-or-namespace node.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity FindBuiltInTypeByTypeOrNamespaceNode(TypeNode typeOrNamespaceNode)
    {
      // If the name is not a one-part-long name, then not a builtin type
      if (typeOrNamespaceNode.TypeName.TypeTags.Count != 1)
      {
        return null;
      }

      TypeEntity typeEntity = null;

      string identifier = typeOrNamespaceNode.TypeName.TypeTags[0].Identifier;

      // Resolve 'void*'
      if (identifier == "void" && typeOrNamespaceNode.PointerTokens.Count > 0)
      {
        typeEntity = _SemanticGraph.PointerToUnknownType;
      }
      else
      {
        // Resolve built-in types
        typeEntity = _SemanticGraph.GetBuiltInTypeByName(identifier);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a nullable type given its underlying type.
    /// </summary>
    /// <param name="underlyingTypeEntity">The underlying type (eg. 'int' for 'int?')</param>
    /// <returns>A nullable type entity with the given underlying type.</returns>
    /// <remarks>
    /// If the constructed type already exists, then retrieves it, otherwise creates it.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private NullableTypeEntity GetConstructedNullableType(TypeEntity underlyingTypeEntity)
    {
      // If the constructed type not exists yet then create it.
      if (underlyingTypeEntity.NullableType == null)
      {
        // The aliased type is a constructed generic type created from System.Nullable`1 and the underlying type
        var aliasedType = new ConstructedGenericTypeEntity(_SemanticGraph.NullableGenericTypeDefinition,
                                                           new List<TypeEntity> {underlyingTypeEntity});
        underlyingTypeEntity.NullableType = new NullableTypeEntity(underlyingTypeEntity, aliasedType);
      }

      return underlyingTypeEntity.NullableType;
    }

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
    private PointerToTypeEntity GetConstructedPointerType(TypeEntity underlyingTypeEntity)
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
    private ArrayTypeEntity GetConstructedArrayType(TypeEntity underlyingTypeEntity, int rank)
    {
      var arrayType = underlyingTypeEntity.GetArrayTypeByRank(rank);

      // If the constructed type not exists yet then create it.
      if (arrayType == null)
      {
        arrayType = new ArrayTypeEntity(underlyingTypeEntity, rank, _SemanticGraph.SystemArray);
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
    private static ConstructedGenericTypeEntity GetConstructedGenericType(GenericCapableTypeEntity genericTypeDefinition,
                                                                          List<TypeEntity> typeArguments)
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

    #endregion
  }
}
