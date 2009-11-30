using System;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a QualifiedAliasMemberNode to an ExpressionResult.
  /// </summary>
  /// <remarks>
  /// This class implements the member access resolution logic, as described in the spec §7.5.4
  /// </remarks>
  // ================================================================================================
  public sealed class QualifiedAliasMemberNodeResolver : SyntaxNodeResolver<ExpressionResult, QualifiedAliasMemberNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QualifiedAliasMemberNodeResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberNodeResolver(QualifiedAliasMemberNode syntaxNode)
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
    protected override ExpressionResult GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      ExpressionResult expressionResult = null;

      var namespaceOrTypeEntity = NamespaceOrTypeNameResolutionAlgorithm.ResolveQualifiedAliasMember(
        SyntaxNode.Qualifier, SyntaxNode.Identifier, SyntaxNode.Arguments, SyntaxNode.SourcePoint,
        context, context.RootNamespace, errorHandler);

      if (namespaceOrTypeEntity is NamespaceEntity)
      {
        expressionResult = new NamespaceExpressionResult(namespaceOrTypeEntity as NamespaceEntity);
      }
      else
      {
        expressionResult = new TypeExpressionResult(namespaceOrTypeEntity as TypeEntity);
      }

      return expressionResult;
    }
    
  }
}
