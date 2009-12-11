using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves an ExternAliasNod to a RootNamespaceEntity.
  /// </summary>
  // ================================================================================================
  public sealed class ExternAliasNodeToRootNamespaceEntityResolver 
    : SyntaxNodeResolver<RootNamespaceEntity, ExternAliasNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasNodeToRootNamespaceEntityResolver"/> class.
    /// </summary>
    /// <param name="externAliasNode">An extern alias AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNodeToRootNamespaceEntityResolver(ExternAliasNode externAliasNode)
      : base(externAliasNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternAliasNodeToRootNamespaceEntityResolver"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private ExternAliasNodeToRootNamespaceEntityResolver(ExternAliasNodeToRootNamespaceEntityResolver template, TypeParameterMap typeParameterMap)
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
    protected override Resolver<RootNamespaceEntity> ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new ExternAliasNodeToRootNamespaceEntityResolver(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override RootNamespaceEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      if (SyntaxNode.Identifier == context.SemanticGraph.GlobalNamespace.Name)
      {
        errorHandler.Error("CS1681", SyntaxNode.IdentifierToken, "You cannot redefine the global extern alias");
        return null;
      }

      var rootNamespace = context.SemanticGraph.GetRootNamespaceByName(SyntaxNode.Identifier);
      if (rootNamespace != null)
      {
        return rootNamespace;
      }

      errorHandler.Error("CS0430", SyntaxNode.IdentifierToken,
                         "The extern alias '{0}' was not specified in a /reference option", SyntaxNode.Identifier);
      return null;
    }
  }
}
