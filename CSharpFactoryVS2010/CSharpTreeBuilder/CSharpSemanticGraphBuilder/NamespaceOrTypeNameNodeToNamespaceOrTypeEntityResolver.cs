using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a NamespaceOrTypeNameNode to a NamespaceOrTypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver 
    : NamespaceOrTypeNameNodeResolver<NamespaceOrTypeEntity>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver(NamespaceOrTypeNameNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver(NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver template, TypeParameterMap typeParameterMap)
      :base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolver.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new resolver constructed from this resolver using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override Resolver<NamespaceOrTypeEntity> ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new NamespaceOrTypeNameNodeToNamespaceOrTypeEntityResolver(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override NamespaceOrTypeEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      if (IsGenericClone)
      {
        throw new ApplicationException("GetResolvedEntity called on generic clone object.");
      }

      return NamespaceOrTypeNameResolutionAlgorithm.ResolveNamespaceOrTypeNode(SyntaxNode, context, errorHandler);
    }
  }
}
