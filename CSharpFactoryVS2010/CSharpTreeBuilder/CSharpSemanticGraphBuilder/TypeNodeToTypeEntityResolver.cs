using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a TypeNode to a TypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNodeToTypeEntityResolver : SyntaxNodeResolver<TypeEntity, TypeNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeToTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeToTypeEntityResolver(TypeNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      return NamespaceOrTypeNameResolutionAlgorithm.ResolveTypeNode(SyntaxNode, context, errorHandler);
    }

  }
}
