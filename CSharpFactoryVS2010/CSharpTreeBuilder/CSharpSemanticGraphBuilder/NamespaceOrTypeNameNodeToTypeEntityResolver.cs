using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a NamespaceOrTypeNameNode to a TypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNodeToTypeEntityResolver : NamespaceOrTypeNameNodeResolver<TypeEntity>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeToTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeToTypeEntityResolver(NamespaceOrTypeNameNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translates an exception thrown during namespace-or-type-name resolution to an error that
    /// can be reported to an ICompilationErrorHandler.
    /// </summary>
    /// <param name="e">An exception object.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <remarks>If can't translate the exception then delegates it to its base.</remarks>
    // ----------------------------------------------------------------------------------------------
    protected override void TranslateExceptionToError(ResolverException e, ICompilationErrorHandler errorHandler)
    {
      if (e is TypeNameExpectedException)
      {
        errorHandler.Error("CS0118", SyntaxNode.StartToken, "'{0}' is a '{1}' but is used like a type.",
                           (e as TypeNameExpectedException).EntityName, (e as TypeNameExpectedException).EntityTypeName);
      }
      else
      {
        base.TranslateExceptionToError(e, errorHandler);
      }
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
      // Following resolution as described below, ... 
      NamespaceOrTypeEntity namespaceOrTypeEntity = 
        NamespaceOrTypeNameResolutionAlgorithm.ResolveNamespaceOrTypeNode(SyntaxNode, context, errorHandler);

      // (If the resolution produced a null, then an error was already signaled, so just bail out.)
      if (namespaceOrTypeEntity == null)
      {
        return null;
      }

      // ... the namespace-or-type-name of a type-name must refer to a type, ... 
      if (namespaceOrTypeEntity is TypeEntity)
      {
        return namespaceOrTypeEntity as TypeEntity;
      }

      // ... or otherwise a compile-time error occurs.
      throw new TypeNameExpectedException(SyntaxNode.ToString(), namespaceOrTypeEntity.GetType().ToString());
    }
  }
}
