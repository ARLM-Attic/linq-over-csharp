using System;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity, based on a type-or-namespace AST node.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNodeBasedTypeEntityReference : SyntaxNodeBasedSemanticEntityReference<TypeEntity, TypeNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeBasedTypeEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeBasedTypeEntityReference(TypeNode syntaxNode)
      : base(syntaxNode)
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
      SemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      return GetTypeEntityByTypeNode(SyntaxNode, context, semanticGraph, errorHandler);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds or constructs the type entity denoted by a type AST node.
    /// </summary>
    /// <param name="typeNode">A type AST node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity GetTypeEntityByTypeNode(TypeNode typeNode, SemanticEntity resolutionContextEntity,
      SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      // First, try to resolve as built-in type.
      TypeEntity typeEntity = null; // FindBuiltInTypeByTypeOrNamespaceNode(typeOrNamespaceNode);

      // If not found, then continue with the resolution
      if (typeEntity == null)
      {
        var namespaceOrTypeNameResolver = new NamespaceOrTypeNameResolver(errorHandler, semanticGraph);
        typeEntity = namespaceOrTypeNameResolver.ResolveToTypeEntity(SyntaxNode.TypeName, resolutionContextEntity);

        //try
        //{
        //  // Try to find the entity by traversing the semantic graph.
        //  foundEntity = FindEntityInSemanticGraph(typeOrNamespaceNode.TypeName.TypeTags, resolutionContextEntity, true, true);
        //}
        //catch (AmbigousReferenceInImportedNamespacesException e)
        //{
        //  _ErrorHandler.Error("CS0104", typeOrNamespaceNode.TypeName.TypeTags.StartToken,
        //                      "'{0}' is an ambiguous reference between '{1}' and '{2}'",
        //                      e.Reference, e.Identifier1, e.Identifier2);
        //  return null;
        //}

        //// If an entity was found, but it's not a TypeEntity, then signal error
        //if (foundEntity != null && !(foundEntity is TypeEntity))
        //{
        //  _ErrorHandler.Error("CS0118", typeOrNamespaceNode.StartToken, "'{0}' is a '{1}' but is used like a type.",
        //                      typeOrNamespaceNode.ToString(), foundEntity.GetType());
        //  return null;
        //}

        //// If couldn't resolve then signal error
        //if (foundEntity == null)
        //{
        //  _ErrorHandler.Error("CS0246", typeOrNamespaceNode.StartToken,
        //                      "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
        //                      typeOrNamespaceNode.ToString());
        //  return null;
        //}

        //// The entity is found, and is a type entity.
        //typeEntity = foundEntity as TypeEntity;
      }

      //// We have to collect all type arguments, because constructed generic types need their parents's type arguments as well.
      //var typeArgumentNodes = GetTypeArgumentNodesFromTypeOrNamespaceNode(typeOrNamespaceNode);

      //// If there are type arguments, but the found entity is not a generic type definition, then it's an error.
      //if (typeArgumentNodes.Count > 0 && !(typeEntity is GenericCapableTypeEntity))
      //{
      //  throw new ApplicationException(string.Format("Expected to find GenericCapableTypeEntity, but found '{0}'.",
      //                                               typeEntity == null ? "(null)" : typeEntity.GetType().ToString()));
      //}

      //// If there are type arguments then we have to create a constructed generic type.
      //if (typeArgumentNodes.Count > 0 && typeEntity is GenericCapableTypeEntity)
      //{
      //  // Resolve all type arguments
      //  var typeArguments = new List<TypeEntity>();
      //  foreach (var typeArgumentSyntaxNode in typeArgumentNodes)
      //  {
      //    var typeArgumentRef = new TypeOrNamespaceNodeBasedTypeEntityReference(typeArgumentSyntaxNode);
      //    var typeArgument = GetTypeEntityByTypeOrNamespaceNode(typeArgumentRef.SyntaxNode, resolutionContextEntity);
      //    if (typeArgument == null)
      //    {
      //      // No need to signal error here, because the GetTypeEntityByTypeOrNamespaceNode method already signaled it, just bail out.
      //      return null;
      //    }
      //    typeArguments.Add(typeArgument);
      //  }

      //  typeEntity = ConstructedTypeHelper.GetConstructedGenericType(typeEntity as GenericCapableTypeEntity, typeArguments);
      //  if (typeEntity == null)
      //  {
      //    throw new ApplicationException("GetConstructedGenericType returned null.");
      //  }
      //}

      //// If the AST node has a nullable type indicating token, then create nullable type.
      //if (typeOrNamespaceNode.NullableToken != null)
      //{
      //  typeEntity = ConstructedTypeHelper.GetConstructedNullableType(typeEntity, _SemanticGraph);
      //}

      //// If the AST node has pointer token(s), then create pointer type(s).
      //bool isFirstStar = true;
      //foreach (var pointerToken in typeOrNamespaceNode.PointerTokens)
      //{
      //  // If it's pointer to unknown type (void*) then the first '*' should be swallowed, because that's part of 'void*'
      //  if (typeEntity is PointerToUnknownTypeEntity && isFirstStar)
      //  {
      //    isFirstStar = false;
      //  }
      //  else
      //  {
      //    typeEntity = ConstructedTypeHelper.GetConstructedPointerType(typeEntity);
      //  }
      //}

      //// If the AST node has rank specifier(s), then create array type(s).
      //foreach (var rankSpecifier in typeOrNamespaceNode.RankSpecifiers)
      //{
      //  typeEntity = ConstructedTypeHelper.GetConstructedArrayType(typeEntity, rankSpecifier.Rank, _SemanticGraph);
      //}

      return typeEntity;
    }

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Extracts the list of type arguments from a type-or-namespace AST node.
    ///// </summary>
    ///// <param name="typeOrNamespaceNode">A type-or-namespace node</param>
    ///// <returns>The list of type arguments.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private static TypeNodeCollection GetTypeArgumentNodesFromTypeOrNamespaceNode(TypeNode typeOrNamespaceNode)
    //{
    //  var typeArgumentNodes = new TypeNodeCollection();

    //  foreach (var typeTag in typeOrNamespaceNode.TypeName.TypeTags)
    //  {
    //    if (typeTag.HasTypeArguments)
    //    {
    //      foreach (var argument in typeTag.Arguments)
    //      {
    //        typeArgumentNodes.Add(argument);
    //      }
    //    }
    //  }

    //  return typeArgumentNodes;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find an entity in the following ways.
    ///// <list type="number">
    ///// <item>Look up in local declaration space.</item>
    ///// <item>Look up in the declaration space of base types, recursively.</item>
    ///// <item>Iterate the following steps for parent types/namespaces, till the root is reached.</item>
    /////   <list type="number">
    /////   <item>Look up in the local declaration space.</item>
    /////   <item>Look up as using alias.</item>
    /////   <item>Look up in using'd namespaces.</item>
    /////   </list>
    ///// </list> 
    ///// </summary>
    ///// <param name="typeTags">The name of the entity to be found.</param>
    ///// <param name="resolutionContextEntity">The entity, where the resolution starts.</param>
    ///// <param name="considerBaseTypes">Look up in base types too.</param>
    ///// <param name="considerUsings">Consider using namespace and using alias directives too.</param>
    ///// <returns>A semantic entity, or null if not found.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private SemanticEntity FindEntityInSemanticGraph(TypeTagNodeCollection typeTags,
    //                                                 NamespaceOrTypeEntity resolutionContextEntity,
    //                                                 bool considerBaseTypes,
    //                                                 bool considerUsings)
    //{
    //  SemanticEntity foundEntity = null;

    //  // Step 1: Look up in local declaration space.
    //  foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<NamespaceOrTypeEntity>(typeTags, resolutionContextEntity);

    //  // Step 2: Look up in the declaration space of base types, recursively.
    //  if (foundEntity == null && considerBaseTypes && resolutionContextEntity is TypeEntity)
    //  {
    //    foundEntity = FindEntityByTraversingBaseClasses(typeTags, resolutionContextEntity as TypeEntity);
    //  }

    //  // If still not found then continue searching one level higher
    //  if (resolutionContextEntity.Parent is NamespaceOrTypeEntity)
    //  {
    //    resolutionContextEntity = resolutionContextEntity.Parent as NamespaceOrTypeEntity;
    //  }

    //  // Loop until the entity is found, or we run out of parent entities
    //  while (foundEntity == null && resolutionContextEntity != null)
    //  {
    //    // Step 3a: Look up in the declaration space of parent type/namespace.
    //    foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<NamespaceOrTypeEntity>(typeTags, resolutionContextEntity);

    //    // Step 3b: Look up as using alias.
    //    if (foundEntity == null && considerUsings && resolutionContextEntity is NamespaceEntity)
    //    {
    //      // TODO: implement this
    //    }

    //    // Step 4: Look up in imported (using) namespaces.
    //    if (foundEntity == null && considerUsings && resolutionContextEntity is NamespaceEntity)
    //    {
    //      foundEntity = FindTypeTagsInImportedNamespaces(typeTags, resolutionContextEntity as NamespaceEntity);
    //    }

    //    // If still not found then continue searching one level higher
    //    resolutionContextEntity = resolutionContextEntity.Parent as NamespaceOrTypeEntity;
    //  }

    //  return foundEntity;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find the entity for TypeTags by looking in imported namespaces.
    ///// </summary>
    ///// <param name="typeTags">The name of the entity to be found.</param>
    ///// <param name="resolutionContextEntity">
    ///// The context of the resolution, a namespace whose usings will be examined.
    ///// </param>
    ///// <returns>A TypeEntity, or null if not found, or more than one found.</returns>
    ///// <remarks>If more than one types are found in the imported namespaces that's an error.</remarks>
    //// ----------------------------------------------------------------------------------------------
    //private TypeEntity FindTypeTagsInImportedNamespaces(TypeTagNodeCollection typeTags,
    //                                                    NamespaceEntity resolutionContextEntity)
    //{
    //  // We have to check whether more than one type can be found because that means error CS0104.
    //  TypeEntity firstFoundEntity = null;
    //  TypeEntity secondFoundEntity = null;

    //  var sourcePoint = new SourcePoint(typeTags.CompilationUnitNode, typeTags.StartPosition);
    //  foreach (var usingNamespaceEntity in resolutionContextEntity.GetUsingNamespacesBySourcePoint(sourcePoint))
    //  {
    //    if (usingNamespaceEntity.ImportedNamespace != null)
    //    {
    //      // Try to find TypeTags in the imported namespace, but only TypeEntities count 
    //      var foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<TypeEntity>(typeTags,
    //                                                                            usingNamespaceEntity.ImportedNamespace);

    //      // Put the 1st and 2nd found entities in the right slot
    //      if (foundEntity != null && firstFoundEntity == null)
    //      {
    //        firstFoundEntity = foundEntity;
    //      }
    //      else if (foundEntity != null && secondFoundEntity == null)
    //      {
    //        secondFoundEntity = foundEntity;
    //      }


    //      // If two entities were found, then signal error and bail out.
    //      if (firstFoundEntity != null && secondFoundEntity != null)
    //      {
    //        throw new AmbigousReferenceInImportedNamespacesException(typeTags.ToString(),
    //                                                                 firstFoundEntity.FullyQualifiedName,
    //                                                                 secondFoundEntity.FullyQualifiedName);
    //      }
    //    }
    //  }

    //  return firstFoundEntity;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find the entity for TypeTags by traversing base class, recursively.
    ///// </summary>
    ///// <param name="typeTags">The name of the entity to be found.</param>
    ///// <param name="resolutionContextEntity">The current context of the resolution. Its base type will be examined first.</param>
    ///// <returns>A semantic entity, or null if not found.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private SemanticEntity FindEntityByTraversingBaseClasses(TypeTagNodeCollection typeTags, TypeEntity resolutionContextEntity)
    //{
    //  SemanticEntity foundEntity = null;

    //  // Loop until the entity is found, or we run out of base classes
    //  while (foundEntity == null && resolutionContextEntity != null)
    //  {
    //    resolutionContextEntity = resolutionContextEntity.BaseType;

    //    if (resolutionContextEntity != null)
    //    {
    //      // Try to resolve the name in the base type's declaration space
    //      foundEntity = FindTypeTagsInDeclarationSpaceHierarchy<TypeEntity>(typeTags, resolutionContextEntity);
    //    }
    //  }

    //  return foundEntity;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find an entity that can be reached from the given resolution context entity
    ///// by matching all type tags in a hierarchy of declaration spaces.
    ///// </summary>
    ///// <typeparam name="TTargetEntityType">Only these kind of entities will be found.</typeparam>
    ///// <param name="typeTags">A multipart name.</param>
    ///// <param name="resolutionContextEntity">The context of the resolution.</param>
    ///// <returns>A TTargetEntityType entity, or null if not found.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private TTargetEntityType FindTypeTagsInDeclarationSpaceHierarchy<TTargetEntityType>(TypeTagNodeCollection typeTags,
    //                                                                                  NamespaceOrTypeEntity resolutionContextEntity)
    //  where TTargetEntityType : SemanticEntity
    //{
    //  SemanticEntity entity = null;

    //  // Match all parts (typeTags) of the name (eg. A.B.C<D> = 3 TypeTags) 
    //  for (int i = 0; i < typeTags.Count; i++)
    //  {
    //    // Look up the TypeTage's name in the current context's declaration space.
    //    entity = FindTypeTagInDeclarationSpace(typeTags[i], resolutionContextEntity.DeclarationSpace);

    //    // If TypeTag was not found, or is not the expected type
    //    // then there's no point to continue matching the next tags, just return null (not found).
    //    if (entity == null || !(entity is TTargetEntityType))
    //    {
    //      return null;
    //    }

    //    // If not all typeTags were matched then we should continue with the next one, 
    //    // but if the current entity is not a NamespaceOrTypeEntity, then it's not possible to continue, so raise an error.
    //    if (i + 1 < typeTags.Count && !(entity is NamespaceOrTypeEntity))
    //    {
    //      throw new ApplicationException(string.Format("Expected a NamespaceOrTypeEntity but found a '{0}'.",
    //                                                   entity.GetType()));
    //    }

    //    // The resolution context of the next tag will be the current entity
    //    resolutionContextEntity = entity as NamespaceOrTypeEntity;
    //  }

    //  return entity as TTargetEntityType;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find a NamespaceOrTypeEntity in a declaration space, that corresponds to a TypeTag syntax node.
    ///// </summary>
    ///// <param name="typeTag">A TypeTag syntax node that contains the name and type arguments to be resolved.</param>
    ///// <param name="declarationSpace">A declaration space.</param>
    ///// <returns>The found NamespaceOrTypeEntity, or null if not found.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private NamespaceOrTypeEntity FindTypeTagInDeclarationSpace(TypeTagNode typeTag, DeclarationSpace declarationSpace)
    //{
    //  // Assemble the name, possibly with type argument specifier (eg. Ident`1)
    //  var name = typeTag.Identifier + (typeTag.HasTypeArguments ? "`" + typeTag.Arguments.Count : "");

    //  // Look up the name
    //  var nameTableEntry = declarationSpace[name];

    //  // If the name is not found, then return null 
    //  if (nameTableEntry == null)
    //  {
    //    return null;
    //  }

    //  // If the name is found, but is not definite, then sign warning, and return null.
    //  if (nameTableEntry.State != NameTableEntryState.Definite)
    //  {
    //    _ErrorHandler.Warning("TBD", typeTag.StartToken, "TypeTag '{0}' is ambigous.", name);
    //    return null;
    //  }

    //  // If the name is found, and definite, but not a NamespaceOrTypeEntity then just return null.
    //  if (!(nameTableEntry.Entity is NamespaceOrTypeEntity))
    //  {
    //    return null;
    //  }

    //  return nameTableEntry.Entity as NamespaceOrTypeEntity;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Tries to find the built-in type (or void*) that is the meaning of a type-or-namespace node.
    ///// </summary>
    ///// <param name="typeOrNamespaceNode">A type-or-namespace node.</param>
    ///// <returns>A TypeEntity, or null if not found.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private TypeEntity FindBuiltInTypeByTypeOrNamespaceNode(TypeNode typeOrNamespaceNode)
    //{
    //  // If the name is not a one-part-long name, then not a builtin type
    //  if (typeOrNamespaceNode.TypeName.TypeTags.Count != 1)
    //  {
    //    return null;
    //  }

    //  TypeEntity typeEntity = null;

    //  string identifier = typeOrNamespaceNode.TypeName.TypeTags[0].Identifier;

    //  // Resolve 'void*'
    //  if (identifier == "void" && typeOrNamespaceNode.PointerTokens.Count > 0)
    //  {
    //    typeEntity = _SemanticGraph.PointerToUnknownType;
    //  }
    //  else
    //  {
    //    // Resolve built-in types
    //    typeEntity = _SemanticGraph.GetBuiltInTypeByName(identifier);
    //  }

    //  return typeEntity;
    //}

    #endregion
  }
}
