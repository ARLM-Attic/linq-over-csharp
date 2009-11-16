using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an expression result classified as a namespace.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceExpressionResult : ExpressionResult
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceExpressionResult"/> class.
    /// </summary>
    /// <param name="namespaceEntity">The namespace associated with this expression.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceExpressionResult(NamespaceEntity namespaceEntity)
    {
      Namespace = namespaceEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace associated with the expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceEntity Namespace { get; private set; }
  }
}
