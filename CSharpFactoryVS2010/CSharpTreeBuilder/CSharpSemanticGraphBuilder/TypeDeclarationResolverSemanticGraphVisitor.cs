using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that finds and resolves type references
  /// till the type declaration level (eg. base types).
  /// </summary>
  // ================================================================================================
  public sealed class TypeDeclarationResolverSemanticGraphVisitor : TypeResolverSemanticGraphVisitorBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeDeclarationResolverSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="metadataToEntityMap">A cache object for mapping reflected types to semantic entities.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationResolverSemanticGraphVisitor(ICompilationErrorHandler errorHandler, IMetadataToEntityMap metadataToEntityMap)
      :base(errorHandler,metadataToEntityMap)
    {
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
        ResolveTypeEntityReference(typeEntityReference, entity.Parent);
      }
    }
  }
}
