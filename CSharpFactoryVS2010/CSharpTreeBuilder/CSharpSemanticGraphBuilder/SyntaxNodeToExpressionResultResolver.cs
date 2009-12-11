using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of resolvers that resolve a syntax node to an ExpressionResult.
  /// </summary>
  // ================================================================================================
  public abstract class SyntaxNodeToExpressionResultResolver<TSyntaxNodeType> : SyntaxNodeResolver<ExpressionResult, TSyntaxNodeType>
    where TSyntaxNodeType : ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeToExpressionResultResolver{TSyntaxNodeType}"/> class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNodeToExpressionResultResolver(TSyntaxNodeType syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeToExpressionResultResolver{TSyntaxNodeType}"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNodeToExpressionResultResolver(SyntaxNodeToExpressionResultResolver<TSyntaxNodeType> template, TypeParameterMap typeParameterMap)
      :base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements a wrapper around the resolution logic that maps type expression results 
    /// using a type parameter map.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override ExpressionResult GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      var result = InternalGetResolvedEntity(context, errorHandler);

      if (IsGenericClone && result is TypeExpressionResult)
      {
        result = new TypeExpressionResult((result as TypeExpressionResult).Type.GetMappedType(TypeParameterMap));
      }

      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the unique resolution logic, not including the type parameter mapping.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected abstract ExpressionResult InternalGetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler);
  }
}
