using System;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that finds and resolves type reference.
  /// </summary>
  // ================================================================================================
  public class TypeResolverSemanticGraphVisitor : SemanticGraphVisitor
  {
    /// <summary>The project used for reporting compilation messages.</summary>
    private CSharpProject _Project;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="project">The project used for reporting compilation messages.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeResolverSemanticGraphVisitor(CSharpProject project)
    {
      _Project = project;
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
        Resolve<TypeEntity>((NamespaceOrTypeEntity)entity.Parent, typeEntityReference);
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
      if (entity.Type != null)
      {
        Resolve<TypeEntity>((NamespaceOrTypeEntity)entity.Parent, entity.Type);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type entity reference.
    /// </summary>
    /// <typeparam name="TNamespaceOrTypeEntity">The type of entity expected to be found. 
    /// Must be a subclass of NamespaceOrTypeEntity.</typeparam>
    /// <param name="contextEntity">The namespace or type entity which is the starting context of the resolution.</param>
    /// <param name="reference">A namespace or type entity reference to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    private void Resolve<TNamespaceOrTypeEntity>(NamespaceOrTypeEntity contextEntity, NamespaceOrTypeEntityReference reference)
      where TNamespaceOrTypeEntity : NamespaceOrTypeEntity
    {
      // If resolving to a typename, then first try to resolve as built-in type, then bail out if succeeded
      if (typeof(TNamespaceOrTypeEntity) == typeof(TypeEntity) && reference is TypeEntityReference 
        && TryResolveAsBuiltInType((TypeEntityReference)reference))
      {
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

            // If this is a constructed generic type (it has type arguments), then create an entity for it.
            if (typeTagToMatch.HasTypeArguments)
            {
              foundEntity = new ConstructedGenericTypeEntity((GenericCapableTypeEntity)foundEntity, parentEntityForConstructedTypes);
              
              // And also resolve the generic's type arguments.
              foreach (var argument in typeTagToMatch.Arguments)
              {
                var typeArg = new TypeEntityReference(argument);
                ((ConstructedGenericTypeEntity)foundEntity).AddTypeArgument(typeArg);
                Resolve<TypeEntity>(contextEntity, typeArg);
              }
           
              // Note: Possible memory usage optimization: store the constructed types in SemanticGraph's
              // _NamespaceOrTypeEntities collection, avoid creating multiple instances of the same constructed type.
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
          if (foundEntity is TNamespaceOrTypeEntity)
          {
            reference.SetResolved(foundEntity);
          }
          else
          {
            ((ICompilationErrorHandler) _Project).Error( "CS0118", reference.SyntaxNode.StartToken, 
              "'{0}' is a '{1}' but is used like a '{2}'.",
              reference.SyntaxNode.ToString(), foundEntity.GetType(), typeof (TNamespaceOrTypeEntity));

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
      if (reference.ResolutionState==ResolutionState.NotYetResolved && lookupStartingEntity == null)
      {
        ((ICompilationErrorHandler)_Project).Error( "CS0246", reference.SyntaxNode.StartToken, 
          "The type or namespace name '{0}' could not be found (are you missing a using directive or an assembly reference?)",
          reference.SyntaxNode.ToString());

        reference.SetUnresolvable();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolves a type reference to a built-in type.
    /// </summary>
    /// <param name="reference">The type reference to be resolved. Will be set to Resolved state is successful.</param>
    /// <returns>True if succeeded, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool TryResolveAsBuiltInType(TypeEntityReference reference)
    {
      // If the name is not a on-part-long name, then not a builtin type
      if (reference.SyntaxNode.TypeTags.Count != 1)
      {
        return false;
      }

      TypeEntity typeEntity = null;

      switch (reference.SyntaxNode.TypeTags[0].Identifier)
      {
        case "void":
          if (reference.SyntaxNode.PointerTokens.Count>0)
          {
            typeEntity = new PointerToUnknownTypeEntity();
          }
          else
          {
            // TODO: error: void used as type name
          }
          break;
        default:
          break;
      }

      if (typeEntity != null)
      {
        reference.SetResolved(typeEntity);
      }
      return typeEntity != null;
    }
  }
}
