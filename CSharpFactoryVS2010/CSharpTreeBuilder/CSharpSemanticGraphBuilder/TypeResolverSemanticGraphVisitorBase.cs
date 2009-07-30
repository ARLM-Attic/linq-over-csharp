using System;
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

    /// <summary>A cache object for mapping reflected types to semantic entities.</summary>
    private readonly IMetadataToEntityMap _MetadataToEntityMap;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverSemanticGraphVisitorBase"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="metadataToEntityMap">A cache object for mapping reflected types to semantic entities.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeResolverSemanticGraphVisitorBase(ICompilationErrorHandler errorHandler, IMetadataToEntityMap metadataToEntityMap)
    {
      _ErrorHandler = errorHandler;
      _MetadataToEntityMap = metadataToEntityMap;
    }

    #region Protected methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a type reference.
    /// </summary>
    /// <param name="reference">A reference to a TypeEntity.</param>
    /// <param name="contextEntity">An entity that is the context (or starting point) of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    protected void ResolveTypeEntityReference(SemanticEntityReference<TypeEntity> reference, SemanticEntity contextEntity)
    {
      // If already resolved then bail out.
      if (reference.ResolutionState == ResolutionState.Resolved)
      {
        return;
      }

      if (reference is ReflectedTypeBasedTypeEntityReference)
      {
        ResolveReflectedTypeToTypeEntity(reference as ReflectedTypeBasedTypeEntityReference);
      }
      else if (reference is TypeOrNamespaceNodeBasedTypeEntityReference && contextEntity is NamespaceOrTypeEntity)
      {
        ResolveSyntaxNodeToTypeEntity(reference as TypeOrNamespaceNodeBasedTypeEntityReference, contextEntity as NamespaceOrTypeEntity);
      }
      else
      {
        throw new ApplicationException(string.Format("Unhandled case in ResolverDispatcher, TargetEntity type='{0}'",
                                                     reference.GetType()));
      }
    }
    #endregion

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a System.Type reference to a TypeEntity.
    /// </summary>
    /// <param name="reference">The reference to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    private void ResolveReflectedTypeToTypeEntity(ReflectedTypeBasedTypeEntityReference reference)
    {
      var targetEntity = _MetadataToEntityMap[reference.Metadata];
      if (targetEntity != null && targetEntity is TypeEntity)
      {
        reference.SetResolved(targetEntity as TypeEntity);
      }
      else
      {
        reference.SetUnresolvable();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a type-or-namespace node to a type entity.
    /// </summary>
    /// <param name="reference">The reference to be resolved.</param>
    /// <param name="contextEntity">An entity that is the context (or starting point) of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    private void ResolveSyntaxNodeToTypeEntity(TypeOrNamespaceNodeBasedTypeEntityReference reference, NamespaceOrTypeEntity contextEntity)
    {
      // First, try to resolve as built-in type, and bail out if succeeded.
      TypeEntity typeEntity = ResolveAsBuiltInType(reference);
      if (typeEntity != null)
      {
        reference.SetResolved(typeEntity);
        return;
      }

      SemanticEntity foundEntity = null;

      // Try the resolution in every declaration space starting from contextEntity and proceeding towards the parents
      while (foundEntity == null && contextEntity != null)
      {
        // Try to resolve the name in this context
        foundEntity = ResolveMultipartNameToEntity(reference.SyntaxNode.TypeTags, contextEntity);

        // If not found, then try to resolve in the base classes
        if (foundEntity == null && contextEntity is ClassEntity)
        {
          foundEntity = ResolveMultipartNameToEntityInBaseClass(reference.SyntaxNode.TypeTags, contextEntity as ClassEntity);
        }

        // If nothing was found, then we should continue the lookup one level higher
        if (foundEntity == null)
        {
          // To be able to continue the lookup one level higher, the parent entity must be a NamespaceOrTypeEntity
          contextEntity = contextEntity.Parent as NamespaceOrTypeEntity;
        }
      }

      // If an entity was found, but it's not a TypeEntity, then signal error
      if (foundEntity != null && !(foundEntity is TypeEntity))
      {
        _ErrorHandler.Error("CS0118", reference.SyntaxNode.StartToken, "'{0}' is a '{1}' but is used like a type.",
                            reference.SyntaxNode.ToString(), foundEntity.GetType());

        // We couldn't resolve this reference.
        reference.SetUnresolvable();
      }

      // If the found entity is a type
      if (foundEntity is TypeEntity)
      {
        typeEntity = foundEntity as TypeEntity;

        // Process ?, *, [] parts of the typename, and create constructed (non-generic) types if necessary
        typeEntity = CreateConstructedNonGenericType(typeEntity, reference.SyntaxNode);

        // The reference is successfully resolved.
        reference.SetResolved(typeEntity);
      }

      // If couldn't resolve then signal error
      if (reference.ResolutionState == ResolutionState.NotYetResolved)
      {
        _ErrorHandler.Error("CS0246", reference.SyntaxNode.StartToken,
                            "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
                            reference.SyntaxNode.ToString());

        // We couldn't resolve this reference.
        reference.SetUnresolvable();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a multipart name to a type in the context of the base class, recursively.
    /// </summary>
    /// <param name="typeTags">The multipart name to be resolved.</param>
    /// <param name="contextEntity">The current context of the resolution. Its base class will be examined first.</param>
    /// <returns>The resolved SemanticEntity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity ResolveMultipartNameToEntityInBaseClass(TypeTagNodeCollection typeTags, ClassEntity contextEntity)
    {
      // If the context entity does not have a base class, then bail out.
      if (contextEntity.BaseClassEntity==null)
      {
        return null;
      }

      // Try to resolve the name in the base class
      var foundEntity = ResolveMultipartNameToEntity(typeTags, contextEntity.BaseClassEntity);

      // If succeeded, then return it
      if (foundEntity!=null)
      {
        return foundEntity; 
      }

      // Otherwise, continue with the base class of the base class (recursion)
      return ResolveMultipartNameToEntityInBaseClass(typeTags, contextEntity.BaseClassEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a multipart name (eg. A.B.C) in the context of a given entity.
    /// </summary>
    /// <param name="typeTags">The multipart name.</param>
    /// <param name="contextEntity">The context of the resolution.</param>
    /// <returns>The resolved SemanticEntity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity ResolveMultipartNameToEntity(TypeTagNodeCollection typeTags, NamespaceOrTypeEntity contextEntity)
    {
      if (typeTags == null)
      {
        throw new ArgumentNullException("typeTags");
      }
      if (contextEntity == null)
      {
        throw new ArgumentNullException("contextEntity");
      }

      SemanticEntity entity = null;

      // The first TypeTag must be found in contextEntity's declaration space, the next one in its child, and so on.
      var typeTagContextEntity = contextEntity;

      // Match all parts (typeTags) of the name (eg. A.B.C<D> = 3 TypeTags) 
      for (int i = 0; i < typeTags.Count; i++)
      {
        // Look up the TypeTage's name in the current context's declaration space.
        entity = FindTypeTagInDeclarationSpace(typeTags[i], typeTagContextEntity.DeclarationSpace);

        // If TypeTag was not found, then there's no point to continue matching the next tags, just return null (not found).
        if (entity == null)
        {
          return null;
        }

        // If according to the syntax it's a generic, but the found entity is not, then it's an error
        if (typeTags[i].HasTypeArguments && !(entity is GenericCapableTypeEntity))
        {
          throw new ApplicationException(string.Format("Expected to find GenericCapableTypeEntity, but found '{0}'.",
                                                       entity.GetType()));
        }

        // If according to the syntax it's a generic, and the found entity is also a generic,
        // then create a constructed generic type entity, because that will be the context of the next TypeTag's lookup.
        if (typeTags[i].HasTypeArguments && (entity is GenericCapableTypeEntity))
        {
          entity = CreateConstructedGenericType(entity as GenericCapableTypeEntity,
                                                     typeTagContextEntity,
                                                     typeTags[i].Arguments);
          if (entity == null)
          {
            throw new ApplicationException("CreateConstructedGenericType returned null.");
          }
        }

        // If not all typeTags were matched then we should continue with the next one, 
        // but if the current entity is not a NamespaceOrTypeEntity, then it's not possible to continue, so raise an error.
        if (i+1 < typeTags.Count && !(entity is NamespaceOrTypeEntity))
        {
          throw new ApplicationException(
            string.Format(
              "Cannot continue matching TypeTags, because the current entity is not NamespaceOrTypeEntity but '{0}'.",
              entity.GetType()));
        }

        // The resolution context of the next tag will be the current entity
        typeTagContextEntity = entity as NamespaceOrTypeEntity;
      }

      return entity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find a NamespaceOrTypeEntity in a declaration space, that corresponds to a TypeTag syntax node.
    /// </summary>
    /// <param name="typeTag">A TypeTag syntax node that contains the name and type arguments to be resolved.</param>
    /// <param name="declarationSpace">The declaration space.</param>
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
    /// Resolve a type-or-namespace node based reference as a built-in type,
    /// or a constructed type based on a built-in type, or void*.
    /// </summary>
    /// <param name="reference">A reference to a TypeEntity based on a TypeOrNamespaceNode.</param>
    /// <returns>The resulting TypeEntity. Null if couldn't resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity ResolveAsBuiltInType(TypeOrNamespaceNodeBasedTypeEntityReference reference)
    {
      // If the name is not a one-part-long name, then not a builtin type
      if (reference.SyntaxNode.TypeTags.Count != 1)
      {
        return null;
      }

      TypeEntity typeEntity = null;

      switch (reference.SyntaxNode.TypeTags[0].Identifier)
      {
        case "void":
          if (reference.SyntaxNode.PointerTokens.Count > 0)
          {
            typeEntity = new PointerToUnknownTypeEntity();
          }
          else
          {
            throw new ApplicationException("Unexpected use of 'void'.");
          }
          break;
        case "sbyte":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Sbyte);
          break;
        case "byte":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Byte);
          break;
        case "short":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Short);
          break;
        case "ushort":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Ushort);
          break;
        case "int":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Int);
          break;
        case "uint":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Uint);
          break;
        case "long":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Long);
          break;
        case "ulong":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Ulong);
          break;
        case "char":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Char);
          break;
        case "float":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Float);
          break;
        case "double":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Double);
          break;
        case "bool":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Bool);
          break;
        case "decimal":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Decimal);
          break;
        case "object":
          typeEntity = new BuiltInTypeEntity(BuiltInType.Object);
          break;
        case "string":
          typeEntity = new BuiltInTypeEntity(BuiltInType.String);
          break;
        default:
          // If not a built-in type, that doesn't mean an error, we'll just return null.
          break;
      }

      // If successfully resolved then finish the resolving by creating contructed type if necessary
      if (typeEntity != null)
      {
        // Resolve aliased type too
        if (typeEntity is IAliasType)
        {
          ResolveTypeEntityReference(((IAliasType) typeEntity).AliasToType, null);
        }

        // Create constructed type if necessary
        typeEntity = CreateConstructedNonGenericType(typeEntity, reference.SyntaxNode);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constructed type (except constructed generic type) from a type entity, 
    /// based on syntax node fixtures (?, *, [])
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    /// <param name="syntaxNode">A type or namespace node.</param>
    /// <returns>A type entity constructed from the input type entity.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity CreateConstructedNonGenericType(TypeEntity typeEntity, TypeOrNamespaceNode syntaxNode)
    {
      // Create nullable type if necessary
      if (syntaxNode.NullableToken != null)
      {
        typeEntity = new NullableTypeEntity(typeEntity);
        // Resolve the aliasd type too
        ResolveTypeEntityReference(((IAliasType)typeEntity).AliasToType, null);
      }

      // Create pointer types if necessary
      bool isFirstStar = true;
      foreach (var pointerToken in syntaxNode.PointerTokens)
      {
        // If it's pointer to unknown type (void*) then the first '*' should be swallowed, because that's part of 'void*'
        if (typeEntity is PointerToUnknownTypeEntity && isFirstStar)
        {
          isFirstStar = false;
        }
        else
        {
          typeEntity = new PointerToTypeEntity(typeEntity);
        }
      }

      // Create array types if necessary
      foreach (var rankSpecifier in syntaxNode.RankSpecifiers)
      {
        typeEntity = new ArrayTypeEntity(typeEntity, rankSpecifier.Rank);
      }

      // Note: Possible optimization: store the constructed types in a cache to avoid creating multiple instances of the same type.

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a contructed generic type from a generic entity and the argument syntax nodes. 
    /// Also resolves the arguments.
    /// </summary>
    /// <param name="genericEntity">The generic 'template' entity.</param>
    /// <param name="parentEntity">The parent of the to-be-created constructed generic entity. 
    /// It's not simply the parent of genericEntity, because it can be a construced generic entity.</param>
    /// <param name="typeArguments">The type argument AST nodes.</param>
    /// <returns>The constructed generic entity, with all arguments resolved as well.</returns>
    // ----------------------------------------------------------------------------------------------
    private ConstructedGenericTypeEntity CreateConstructedGenericType(GenericCapableTypeEntity genericEntity,
                                                                      NamespaceOrTypeEntity parentEntity,
                                                                      TypeOrNamespaceNodeCollection typeArguments)
    {
      // Create the constructed generic type
      var constructedGenericType = new ConstructedGenericTypeEntity(genericEntity, parentEntity);

      // Also resolve the generic's type arguments.
      foreach (var argument in typeArguments)
      {
        var typeArg = new TypeOrNamespaceNodeBasedTypeEntityReference(argument);
        constructedGenericType.AddTypeArgument(typeArg);
        ResolveSyntaxNodeToTypeEntity(typeArg, genericEntity);
      }

      // Note: Possible optimization: store the constructed types in a cache to avoid creating multiple instances of the same constructed type.

      return constructedGenericType;
    }

    #endregion
  }
}
