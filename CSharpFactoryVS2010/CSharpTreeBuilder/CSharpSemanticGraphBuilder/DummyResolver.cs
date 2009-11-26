using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a dummy resolver where the target object is already known 
  /// but has to be wrapped in a resolver object. Used mostly for testing.
  /// </summary>
  /// <typeparam name="TTargetType">The type of the target object. Any class.</typeparam>
  // ================================================================================================
  public sealed class DummyResolver<TTargetType> : Resolver<TTargetType>
    where TTargetType : class
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DummyResolver{TTargetEntity}"/> class.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    // ----------------------------------------------------------------------------------------------
    public DummyResolver(TTargetType targetObject)
    {
      SetResolved(targetObject);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error reporting.</param>
    /// <returns>The resolved object, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TTargetType GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      return null;
    }
  }
}
