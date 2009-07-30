using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that finds and resolves type reference.
  /// </summary>
  // ================================================================================================
  public class TypeResolverSemanticGraphVisitor : SemanticGraphVisitor
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private ICompilationErrorHandler _ErrorHandler;

    /// <summary>A cache object for mapping reflected types to semantic entities.</summary>
    private IMetadataToEntityMap _MetadataToEntityMap;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="metadataToEntityMap">A cache object for mapping reflected types to semantic entities.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeResolverSemanticGraphVisitor(ICompilationErrorHandler errorHandler, IMetadataToEntityMap metadataToEntityMap)
    {
      _ErrorHandler = errorHandler;
      _MetadataToEntityMap = metadataToEntityMap;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a TypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(TypeEntity entity)
    {
      // Resolve base type references
      foreach (var typeEntityReference in entity.BaseTypes)
      {
        ResolverDispatcher(typeEntityReference, entity.Parent);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a FieldEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(FieldEntity entity)
    {
      // Resolve the type of the field
      if (entity.Type != null)
      {
        ResolverDispatcher(entity.Type, entity.Parent);
      }
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Dispatches reference resolution to different resolver routines.
    /// </summary>
    /// <typeparam name="TTargetEntity">The resulting type of the resolution.</typeparam>
    /// <param name="reference">A reference to TTargetEntity.</param>
    /// <param name="contextEntity">An entity that is the context (or starting point) of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    private void ResolverDispatcher<TTargetEntity>(SemanticEntityReference<TTargetEntity> reference, SemanticEntity contextEntity)
      where TTargetEntity : SemanticEntity
    {
      // If already resolved then bail out.
      if (reference.ResolutionState == ResolutionState.Resolved)
      {
        return;
      }

      if (reference is ReflectedTypeBasedTypeEntityReference)
      {
        Resolve(reference as ReflectedTypeBasedTypeEntityReference);
      }
      else if (reference is TypeOrNamespaceNodeBasedTypeEntityReference && contextEntity is NamespaceOrTypeEntity)
      {
        Resolve(reference as TypeOrNamespaceNodeBasedTypeEntityReference, contextEntity as NamespaceOrTypeEntity);
      }
      else
      {
        throw new ApplicationException(string.Format("Unhandled case in ResolverDispatcher, TargetEntity type='{0}'",
                                                     typeof (TTargetEntity)));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a System.Type reference to a TypeEntity.
    /// </summary>
    /// <param name="reference">The reference to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    private void Resolve(ReflectedTypeBasedTypeEntityReference reference)
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
    private void Resolve(TypeOrNamespaceNodeBasedTypeEntityReference reference, NamespaceOrTypeEntity contextEntity)
    {
      // First, try to resolve as built-in type.
      TypeEntity typeEntity = ResolveAsBuiltInType(reference);
      if (typeEntity != null)
      {
        reference.SetResolved(typeEntity);
        return;
      }

      // If it's not a builtin type name then proceed by trying to resolve to declared or constructed types.
      var lookupStartingEntity = contextEntity;

      // Looping till reference is resolved or we run out of the semantic graph
      while (reference.ResolutionState == ResolutionState.NotYetResolved && lookupStartingEntity != null)
      {
        var currentDeclarationSpace = lookupStartingEntity.DeclarationSpace;
        int matchedTypeTags = 0;
        NamespaceOrTypeEntity foundEntity = null;
        NamespaceOrTypeEntity parentEntityForConstructedTypes = lookupStartingEntity;

        // Try to find a match of all parts of the TypeTags list, starting in lookupStartingEntity's declaration space,
        // and continuing in its children's declaration spaces.
        while (matchedTypeTags < reference.SyntaxNode.TypeTags.Count && currentDeclarationSpace != null)
        {
          var typeTagToMatch = reference.SyntaxNode.TypeTags[matchedTypeTags];
          var nameToLookFor = typeTagToMatch.Identifier
                              + (typeTagToMatch.HasTypeArguments ? "`" + typeTagToMatch.Arguments.Count : "");
          var nameTableEntry = currentDeclarationSpace[nameToLookFor];

          // If the name is found 
          if (nameTableEntry != null
              && nameTableEntry.State == NameTableEntryState.Definite
              && nameTableEntry.Entity is NamespaceOrTypeEntity)
          {
            foundEntity = nameTableEntry.Entity as NamespaceOrTypeEntity;

            // If this type has type arguments, then create a constructed generic type entity for it.
            if (typeTagToMatch.HasTypeArguments)
            {
              if (foundEntity is GenericCapableTypeEntity)
              {
                foundEntity = new ConstructedGenericTypeEntity(foundEntity as GenericCapableTypeEntity, parentEntityForConstructedTypes);
              }
              else
              {
                throw new ApplicationException(string.Format("Expected GenericCapableTypeEntity, but found: '{0}'", foundEntity.GetType()));
              }

              // And also resolve the generic's type arguments.
              foreach (var argument in typeTagToMatch.Arguments)
              {
                var typeArg = new TypeOrNamespaceNodeBasedTypeEntityReference(argument);
                ((ConstructedGenericTypeEntity)foundEntity).AddTypeArgument(typeArg);
                Resolve(typeArg, contextEntity);
              }

              // Note: Possible optimization: store the constructed types in a cache to avoid creating multiple instances of the same constructed type.
            }

            // Continue the matching at the next typetag and the next declaration space.
            matchedTypeTags++;
            currentDeclarationSpace = foundEntity.DeclarationSpace;
            parentEntityForConstructedTypes = foundEntity;
          }
          else
          {
            // If no match, then get out of the loop by setting the current declaration space to null
            currentDeclarationSpace = null;
          }
        }

        // If we could match all typetags
        if (matchedTypeTags == reference.SyntaxNode.TypeTags.Count)
        {
          // Then check if the found entity is of the expected type
          if (foundEntity is TypeEntity)
          {
            foundEntity = CreateConstructedType(foundEntity as TypeEntity, reference.SyntaxNode);

            reference.SetResolved(foundEntity as TypeEntity);
          }
          else
          {
            _ErrorHandler.Error("CS0118", reference.SyntaxNode.StartToken,
              "'{0}' is a '{1}' but is used like a type.", reference.SyntaxNode.ToString(), foundEntity.GetType());

            reference.SetUnresolvable();
          }
        }
        else
        {
          // Otherwise start the lookup one level higher
          lookupStartingEntity = lookupStartingEntity.Parent as NamespaceOrTypeEntity;
        }
      }

      // If couldn't resolve then signal error
      if (reference.ResolutionState == ResolutionState.NotYetResolved && lookupStartingEntity == null)
      {
        _ErrorHandler.Error("CS0246", reference.SyntaxNode.StartToken,
          "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
          reference.SyntaxNode.ToString());

        reference.SetUnresolvable();
      }
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
          ResolverDispatcher(((IAliasType) typeEntity).AliasToType, null);
        }

        // Create constructed type if necessary
        typeEntity = CreateConstructedType(typeEntity, reference.SyntaxNode);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a constructed type from a type entity, based on syntax node fixtures (?, *, [])
    /// </summary>
    /// <param name="typeEntity">A type entity.</param>
    /// <param name="syntaxNode">A type or namespace node.</param>
    /// <returns>A type entity constructed from the input type entity.</returns>
    // ----------------------------------------------------------------------------------------------
    private TypeEntity CreateConstructedType(TypeEntity typeEntity, TypeOrNamespaceNode syntaxNode)
    {
      if (syntaxNode.NullableToken != null)
      {
        typeEntity = new NullableTypeEntity(typeEntity);
        // Resolve the aliasd type too
        ResolverDispatcher(((IAliasType)typeEntity).AliasToType, null);
      }

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

      foreach (var rankSpecifier in syntaxNode.RankSpecifiers)
      {
        typeEntity = new ArrayTypeEntity(typeEntity, rankSpecifier.Rank);
      }

      // Note: Possible optimization: store the constructed types in a cache to avoid creating multiple instances of the same type.

      return typeEntity;
    }


    #endregion
  }
}
