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
        Resolve(entity, typeEntityReference);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type entity reference.
    /// </summary>
    /// <typeparam name="TNamespaceOrTypeEntity">The type of entity to be found. 
    /// Must be a subclass of NamespaceOrTypeEntity.</typeparam>
    /// <param name="entity">The namespace or type entity which is the context of the resolution.</param>
    /// <param name="reference">A namespace or type entity reference to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    private void Resolve<TNamespaceOrTypeEntity>(TNamespaceOrTypeEntity entity, NamespaceOrTypeEntityReference reference)
      where TNamespaceOrTypeEntity : NamespaceOrTypeEntity
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

        // If we could match all typetags
        if (matchedTypeTags == reference.SyntaxNode.TypeTags.Count)
        {
          // Then check if the found entity is of the expected type
          if (foundEntity is TNamespaceOrTypeEntity)
          {
            reference.Resolve(foundEntity);
          }
          else
          {
            ((ICompilationErrorHandler) _Project).Error( "CS0118", reference.SyntaxNode.StartToken, 
              "'{0}' is a '{1}' but is used like a '{2}'.",
              reference.SyntaxNode.ToString(), foundEntity.GetType(), typeof (TNamespaceOrTypeEntity));

            reference.Unresolvable();
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

        reference.Unresolvable();
      }
    }
  }
}
