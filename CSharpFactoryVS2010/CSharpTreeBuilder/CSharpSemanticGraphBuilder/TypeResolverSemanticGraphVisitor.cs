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
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves type references in a TypeEntity node.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Visit(TypeEntity entity)
    {
      foreach (var typeEntityReference in entity.BaseTypes)
      {
        Resolve(entity, typeEntityReference);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type entity reference.
    /// </summary>
    /// <param name="entity">The namespace or type entity which is the context of the resolution.</param>
    /// <param name="reference">A namespace or type entity reference to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    private void Resolve(NamespaceOrTypeEntity entity, NamespaceOrTypeEntityReference reference)
    {
      var lookupStartingEntity = entity.Parent as NamespaceOrTypeEntity;

      // Looping till reference is resolved or we run out of the semantic graph
      while (reference.ResolutionState == ResolutionState.NotYetResolved && lookupStartingEntity != null)
      {
        var currentDeclarationSpace = lookupStartingEntity.DeclarationSpace;
        int matchedTypeTags = 0;
        NamespaceOrTypeEntity foundEntity = null;

        // Try to find a match of all parts of the TypeTags list, starting in lookupStartingEntity's declaration space,
        // and continuing in its embedded declaration spaces
        while (matchedTypeTags < reference.SyntaxNode.TypeTags.Count && currentDeclarationSpace != null)
        {
          var nameTableEntry = currentDeclarationSpace[reference.SyntaxNode.TypeTags[matchedTypeTags].Identifier];

          // If the name is found then continue matching the remaining typetags
          if (nameTableEntry != null
              && nameTableEntry.State == NameTableEntryState.Definite
              && nameTableEntry.Entity is NamespaceOrTypeEntity)
          {
            // Continue the matching at the next typetag and the next nested declaration space
            foundEntity = nameTableEntry.Entity as NamespaceOrTypeEntity;
            matchedTypeTags++;
            currentDeclarationSpace = foundEntity.DeclarationSpace;
          }
          else
          {
            // If no match, then get out of the loop by setting the current declaration space to null
            currentDeclarationSpace = null;
          }
        }

        // If we could match all typetags then the reference is resolved
        if (matchedTypeTags == reference.SyntaxNode.TypeTags.Count)
        {
          reference.Resolve(foundEntity);
        }
        else
        {
          // Otherwise start the lookup one level higher
          lookupStartingEntity = lookupStartingEntity.Parent as NamespaceOrTypeEntity;
        }
      }

      if (lookupStartingEntity == null)
      {
        reference.Unresolvable();
      }
    }
  }
}
