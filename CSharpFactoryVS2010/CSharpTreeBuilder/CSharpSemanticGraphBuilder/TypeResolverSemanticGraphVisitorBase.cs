using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of namespace and type resolver visitors. 
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
    protected readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that this visitor is working on.</summary>
    protected readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeResolverSemanticGraphVisitorBase"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that this visitor is working on.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeResolverSemanticGraphVisitorBase(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }
  }
}
