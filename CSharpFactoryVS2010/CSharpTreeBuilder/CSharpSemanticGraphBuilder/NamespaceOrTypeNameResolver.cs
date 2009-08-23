using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the namespace and type name resolution logic described in the spec §3.8.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameResolver
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that is the context of the type resolution.</summary>
    private readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameResolver"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that is the context of the type resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameResolver(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the namespace and type name resolution logic to a namespace-or-type-name AST node,
    /// and expects a type entity.
    /// </summary>
    /// <param name="namespaceOrTypeName">An AST node that represents a namespace-or-type-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <returns>The resolved type entity. Null if couldn't be resolved.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ResolveToTypeEntity(NamespaceOrTypeNameNode namespaceOrTypeName, SemanticEntity resolutionContextEntity)
    {
      // Following resolution as described below, ... 
      NamespaceOrTypeEntity namespaceOrTypeEntity = ResolveToNamespaceOrTypeEntity(namespaceOrTypeName, resolutionContextEntity);

      // (If the resolution produced a null, then an error was already signaled, so just bail out.)
      if (namespaceOrTypeEntity == null)
      {
        return null;
      }

      // ... the namespace-or-type-name of a type-name must refer to a type, ... 
      if (namespaceOrTypeEntity is TypeEntity)
      {
        return namespaceOrTypeEntity as TypeEntity;
      }

      // ... or otherwise a compile-time error occurs.
      _ErrorHandler.Error("CS0118", namespaceOrTypeName.StartToken, "'{0}' is a '{1}' but is used like a type.",
                          namespaceOrTypeName.ToString(), namespaceOrTypeEntity.GetType());
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the namespace and type name resolution logic to a namespace-or-type-name AST node,
    /// and expects a namespace entity.
    /// </summary>
    /// <param name="namespaceOrTypeName">An AST node that represents a namespace-or-type-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <returns>The resolved namespace entity. Null if couldn't be resolved.</returns>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity ResolveToNamespaceEntity(NamespaceOrTypeNameNode namespaceOrTypeName, SemanticEntity resolutionContextEntity)
    {
      // No type arguments (§4.4.1) can be present in a namespace-name (only types can have type arguments).
      if (namespaceOrTypeName.TypeTags.Any(typeTag => typeTag.Arguments.Count > 0))
      {
        _ErrorHandler.Error("TBD001", namespaceOrTypeName.StartToken,
                            "No type arguments can be present in a namespace-name ('{0}')",
                            namespaceOrTypeName.ToString());
        return null;
      }

      // Following resolution as described below, ...
      NamespaceOrTypeEntity namespaceOrTypeEntity = ResolveToNamespaceOrTypeEntity(namespaceOrTypeName, resolutionContextEntity);

      // (If the resolution produced a null, then an error was already signaled, so just bail out.)
      if (namespaceOrTypeEntity == null)
      {
        return null;
      }

      // ... the namespace-or-type-name of a namespace-name must refer to a namespace, ...
      if (namespaceOrTypeEntity is NamespaceEntity)
      {
        return namespaceOrTypeEntity as NamespaceEntity;
      }

      // ... or otherwise a compile-time error occurs.
      if (namespaceOrTypeEntity is TypeEntity)
      {
        _ErrorHandler.Error("CS0138", namespaceOrTypeName.StartToken, "'{0}' is a type not a namespace",
                            namespaceOrTypeName.ToString());
        return null;
      }

      throw new ApplicationException("Unexpected case in namespace name resolution.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the namespace and type name resolution logic to a namespace-or-type-name AST node.
    /// </summary>
    /// <param name="namespaceOrTypeName">An AST node that represents a namespace-or-type-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity ResolveToNamespaceOrTypeEntity(NamespaceOrTypeNameNode namespaceOrTypeName, SemanticEntity resolutionContextEntity)
    {
      if (namespaceOrTypeName.TypeTags.Count < 1)
      {
        throw new ArgumentException("Namespace-or-type-name must have at least 1 type tag.", "namespaceOrTypeName");
      }

      NamespaceOrTypeEntity foundEntity = null;

      // If the namespace-or-type-name is a qualified-alias-member its meaning is as described in §9.7.
      if (namespaceOrTypeName.HasQualifier)
      {
        // TODO: resolve qualified-alias-member

        return foundEntity;
      }

      // Otherwise, a namespace-or-type-name has one of four forms:
      // - I
      // - I<A1, ..., AK>
      // - N.I
      // - N.I<A1, ..., AK>
      // where I is a single identifier, N is a namespace-or-type-name
      // and <A1, ..., AK> is an optional type-argument-list. 
      // When no type-argument-list is specified, consider K to be zero.

      try
      {
        // The meaning of a namespace-or-type-name is determined as follows:
        foundEntity = ResolveTypeTags(namespaceOrTypeName.TypeTags, resolutionContextEntity);
      }
      catch(Exception e)
      {
        TranslateExceptionToError(e, _ErrorHandler, namespaceOrTypeName);
      }

      return foundEntity;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translates an exception thrown during namespace-or-type-name resolution to an error that
    /// can be reported to an ICompilationErrorHandler.
    /// </summary>
    /// <param name="e">An exception object.</param>
    /// <param name="errorHandler">An error handler object.</param>
    /// <param name="namespaceOrTypeName">The namespace-or-type-name that caused to error.</param>
    // ----------------------------------------------------------------------------------------------
    private static void TranslateExceptionToError(Exception e, ICompilationErrorHandler errorHandler, NamespaceOrTypeNameNode namespaceOrTypeName)
    {
      if (e is NamespaceOrTypeNameNotResolvedException)
      {
        errorHandler.Error("CS0246", namespaceOrTypeName.StartToken,
                           "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
                           namespaceOrTypeName.ToString());
      }
      else if (e is AliasNameConflictException)
      {
        errorHandler.Error("CS0576", namespaceOrTypeName.StartToken,
                           "Namespace '{0}' contains a definition conflicting with alias '{1}'",
                           (e as AliasNameConflictException).NamespaceName,
                           (e as AliasNameConflictException).AliasName);
      }
      else if (e is AmbigousReferenceException)
      {
        errorHandler.Error("CS0104", namespaceOrTypeName.StartToken,
                           "'{0}' is an ambiguous reference between '{1}' and '{2}'",
                           namespaceOrTypeName.ToString(),
                           (e as AmbigousReferenceException).FullyQualifiedName1,
                           (e as AmbigousReferenceException).FullyQualifiedName2);
      }
      else
      {
        throw new ApplicationException("Could not translate namespace-or-type-name resolution exception.", e);
      }
    }

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
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    /// <remarks>If an error occurs then a NamespaceOrTypeNameResolverException is raised.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static NamespaceOrTypeEntity ResolveTypeTags(TypeTagNodeCollection typeTags, SemanticEntity resolutionContextEntity)
    {
      NamespaceOrTypeEntity foundEntity = null;

      // If the namespace-or-type-name is of the form I or of the form I<A1, ..., AK>
      if (typeTags.Count == 1)
      {
        foundEntity = ResolveSingleTypeTag(typeTags[0], resolutionContextEntity);
      }
      else
      {
        // Otherwise, the namespace-or-type-name is of the form N.I or of the form N.I<A1, ..., AK>. 
        // N is first resolved as a namespace-or-type-name.  
        var handleEntity = ResolveTypeTags(typeTags.GetCopyWithoutLastTag(), resolutionContextEntity);
        var lastTypeTag = typeTags[typeTags.Count - 1];

        // If the resolution of N is not successful, a compile-time error occurs. 
        if (handleEntity == null)
        {
          throw new NamespaceOrTypeNameNotResolvedException();
        }

        // Otherwise, N.I or N.I<A1, ..., AK> is resolved as follows:

        // If K is zero and N refers to a namespace ... 
        if (lastTypeTag.GenericDimensions == 0 && handleEntity is NamespaceEntity)
        {
          // ... and N contains a nested namespace with name I, ...
          var foundNamespaceEntity =
            handleEntity.DeclarationSpace.GetSpecificType<NamespaceEntity>(lastTypeTag.Identifier);
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
          // BUGBUG: accessibility is not checked!
          var foundTypeEntity =
            handleEntity.DeclarationSpace.GetSpecificType<TypeEntity>(lastTypeTag.NameWithGenericDimensions);

          if (foundTypeEntity != null)
          {
            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments.
            //  --> Constructed generic type creation is handled in TypeNodeBasedTypeEntityReference class.
            //  --> Here we just return the resolved generic type definition entity.
            return foundTypeEntity;
          }
        }

        // Otherwise, if N refers to a (possibly constructed) class or struct type ...
        if (handleEntity is TypeEntity)
        {
          // ... and N or any of its base classes contain a nested accessible type having name I and K type parameters,
          // BUGBUG: accessibility is not checked!
          var foundNestesTypeEntity = FindNestedTypeInTypeOrBaseTypes(lastTypeTag, handleEntity as TypeEntity);

          if (foundNestesTypeEntity != null)
          {
            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments. 
            //  --> Constructed generic type creation is handled in TypeNodeBasedTypeEntityReference class.
            //  --> Here we just return the resolved generic type definition entity.
            return foundNestesTypeEntity;
          }
        }

        // Otherwise, N.I is an invalid namespace-or-type-name, and a compile-time error occurs.
        throw new NamespaceOrTypeNameNotResolvedException();
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
    /// <returns>The resolved namespace or type entity. Null if couldn't be resolved.</returns>
    /// <remarks>If an error occurs then a NamespaceOrTypeNameResolverException is raised.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static NamespaceOrTypeEntity ResolveSingleTypeTag(TypeTagNode typeTagNode, SemanticEntity resolutionContextEntity)
    {
      // If K is zero and the namespace-or-type-name appears within a generic method declaration (§10.6) ...
      // ... and if that declaration includes a type parameter (§10.1.3) with name I, 
      // then the namespace-or-type-name refers to that type parameter.

      // TODO: implement this branch

      // Otherwise, if the namespace-or-type-name appears within a type declaration, 
      // then for each instance type T (§10.3.1), starting with the instance type of that type declaration
      // and continuing with the instance type of each enclosing class or struct declaration (if any):
      TypeEntity typeContext = resolutionContextEntity.GetEnclosing<TypeEntity>();

      while (typeContext != null)
      {
        // If K is zero ...
        if (typeTagNode.Arguments.Count == 0)
        {
          // ... and the declaration of T includes a type parameter with name I, ...
          var foundTypeParameterEntity = typeContext.DeclarationSpace.GetSpecificType<TypeParameterEntity>(typeTagNode.Identifier);

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
          // BUGBUG: accessibility is not checked!
          var foundNestesTypeEntity = FindNestedTypeInTypeOrBaseTypes(typeTagNode, typeContext);

          if (foundNestesTypeEntity != null)
          {
            // ... then the namespace-or-type-name refers to that type constructed with the given type arguments. 
            //  --> Constructed generic type creation is handled in TypeNodeBasedTypeEntityReference class.
            //  --> Here we just return the resolved generic type definition entity.
            return foundNestesTypeEntity;
          }
        }

        // "... and continuing with the instance type of each enclosing class or struct declaration (if any):"
        typeContext = typeContext.Parent.GetEnclosing<TypeEntity>();
      }

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
          var foundNamespaceEntity = namespaceContext.DeclarationSpace.GetSpecificType<NamespaceEntity>(typeTagNode.Identifier);

          if (foundNamespaceEntity != null)
          {
            // If the location where the namespace-or-type-name occurs is enclosed by a namespace declaration for N 
            // and the namespace declaration contains an extern-alias-directive or using-alias-directive 
            // that associates the name I with a namespace or type, ...
            if (namespaceContext.GetUsingAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null)

            // TODO: check extern-alias-directive too!
            {
              // ... then the namespace-or-type-name is ambiguous and a compile-time error occurs. (CS0576)
              throw new AliasNameConflictException(namespaceContext.FullyQualifiedName, typeTagNode.Identifier);
            }

            // Otherwise, the namespace-or-type-name refers to the namespace named I in N.
            return foundNamespaceEntity;
          }
        }

        // Otherwise, if N contains an accessible type having name I and K type parameters, then:
        // BUGBUG: accessibility is not checked!
        var foundTypeEntity = namespaceContext.DeclarationSpace.GetSpecificType<TypeEntity>(typeTagNode.NameWithGenericDimensions);

        if (foundTypeEntity != null)
        {
          // If K is zero and the location where the namespace-or-type-name occurs is enclosed by a namespace declaration for N 
          // and the namespace declaration contains an extern-alias-directive or using-alias-directive 
          // that associates the name I with a namespace or type, then the namespace-or-type-name is ambiguous 
          // and a compile-time error occurs.
          if (typeTagNode.GenericDimensions == 0
            && namespaceContext.GetUsingAliasByNameAndSourcePoint(typeTagNode.Identifier, typeTagNode.SourcePoint) != null)

          // TODO: check extern-alias-directive too!
          {
            // ... then the namespace-or-type-name is ambiguous and a compile-time error occurs. (CS0576)
            throw new AliasNameConflictException(namespaceContext.FullyQualifiedName, typeTagNode.Identifier);
          }

          // Otherwise, the namespace-or-type-name refers to the type constructed with the given type arguments.
          //  --> Constructed generic type creation is handled in TypeNodeBasedTypeEntityReference class.
          //  --> Here we just return the resolved generic type definition entity.
          return foundTypeEntity;
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
            && usingAliasEntity.NamespaceOrTypeReference.TargetEntity != null)
          {
            // ... then the namespace-or-type-name refers to that namespace or type.
            return usingAliasEntity.NamespaceOrTypeReference.TargetEntity;
          }
        }

        // Otherwise, if the namespaces imported by the using-namespace-directives of the namespace declaration
        // contain ... type having name I and K type parameters, ...
        var typeEntities = FindTypeInImportedNamespaces(typeTagNode, namespaceContext);


        // ... contain exactly one type ...
        if (typeEntities.Count == 1)
        {
          // ... then the namespace-or-type-name refers to that type constructed with the given type arguments.
          //  --> Constructed generic type creation is handled in TypeNodeBasedTypeEntityReference class.
          //  --> Here we just return the resolved generic type definition entity.
          return typeEntities[0];
        }
        
        // ...  contain more than one type ...
        if (typeEntities.Count > 1)
        {
          // ... then the namespace-or-type-name is ambiguous and an error occurs.
          throw new AmbigousReferenceException(typeEntities[0].FullyQualifiedName, typeEntities[1].FullyQualifiedName);
        }

        // "... continuing with each enclosing namespace (if any), ..."
        namespaceContext = namespaceContext.Parent.GetEnclosing<NamespaceEntity>();
      }

      // Otherwise, the namespace-or-type-name is undefined and a compile-time error occurs. (CS0246)
      throw new NamespaceOrTypeNameNotResolvedException();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds a nested type by a typeTagNode in a type entity or any of its base types.
    /// </summary>
    /// <param name="typeTagNode">The typeTag to be found.</param>
    /// <param name="contextType">The type where the searching starts.</param>
    /// <returns>A TypeEntity with the name and number of type parameters defined in typeTagNode, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity FindNestedTypeInTypeOrBaseTypes(TypeTagNode typeTagNode, TypeEntity contextType)
    {
      // If there is more than one such type, the type declared within the more derived type is selected. 
      // _Note that non-type members (constants, fields, methods, properties, indexers, operators, instance constructors, 
      // destructors, and static constructors) and type members with a different number of type parameters 
      // are ignored when determining the meaning of the namespace-or-type-name.

      // _Note that if the meaning of N.I is being determined as part of resolving the base class specification of N 
      // then the direct base class of N is considered to be object (§10.1.4.1).
      // TODO: how to interpret and test this?

      TypeEntity typeEntity = null;

      while (typeEntity == null && contextType != null)
      {
        typeEntity = contextType.DeclarationSpace.GetSpecificType<TypeEntity>(typeTagNode.NameWithGenericDimensions);

        if (typeEntity == null)
        {
          contextType = contextType.BaseType;
        }
      }

      return typeEntity;
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
          var typeEntity = usingNamespaceEntity.ImportedNamespace.DeclarationSpace.GetSpecificType<TypeEntity>(
            typeTagNode.NameWithGenericDimensions);

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
