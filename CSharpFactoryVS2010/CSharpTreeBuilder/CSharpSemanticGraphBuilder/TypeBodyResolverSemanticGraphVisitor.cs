using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that finds and resolves type references in type bodies.
  /// </summary>
  // ================================================================================================
  public sealed class TypeBodyResolverSemanticGraphVisitor : TypeResolverSemanticGraphVisitorBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeBodyResolverSemanticGraphVisitor"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="metadataToEntityMap">A cache object for mapping reflected types to semantic entities.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeBodyResolverSemanticGraphVisitor(ICompilationErrorHandler errorHandler, IMetadataToEntityMap metadataToEntityMap)
      :base(errorHandler,metadataToEntityMap)
    {
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
        ResolveTypeEntityReference(entity.Type, entity.Parent);
      }
    }
  }
}
