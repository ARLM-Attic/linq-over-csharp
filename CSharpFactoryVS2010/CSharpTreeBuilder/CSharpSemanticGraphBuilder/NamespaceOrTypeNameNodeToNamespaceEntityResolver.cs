using System;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a NamespaceOrTypeNameNode to a NamespaceEntity.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeNameNodeToNamespaceEntityResolver : NamespaceOrTypeNameNodeResolver<NamespaceEntity>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeNameNodeToNamespaceEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNodeToNamespaceEntityResolver(NamespaceOrTypeNameNode syntaxNode)
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
      var errorToken = SyntaxNode.StartToken;

      if (e is TypeArgumentInNamespaceNameException)
      {
        errorHandler.Error("TBD001", errorToken,
                           "No type arguments can be present in a namespace-name ('{0}')",
                           (e as TypeArgumentInNamespaceNameException).NamespaceName);
      }
      else if (e is NamespaceNameExpectedTypeNameFoundException)
      {
        errorHandler.Error("CS0138", errorToken, "'{0}' is a type not a namespace",
                           (e as NamespaceNameExpectedTypeNameFoundException).TypeName);
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
    protected override NamespaceEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      // No type arguments (§4.4.1) can be present in a namespace-name (only types can have type arguments).
      if (SyntaxNode.TypeTags.Any(typeTag => typeTag.Arguments.Count > 0))
      {
        throw new TypeArgumentInNamespaceNameException(SyntaxNode.ToString());
      }

      // Following resolution as described below, ...
      NamespaceOrTypeEntity namespaceOrTypeEntity = ResolveNamespaceOrTypeNode(SyntaxNode, context, errorHandler);

      // (If the resolution produced a null, then an error was already signaled, so just bail out.)
      if (namespaceOrTypeEntity == null)
      {
        return null;
      }

      // ... the namespace-or-type-name of a namespace-name must refer to a namespace, ...
      if (namespaceOrTypeEntity is NamespaceEntity)
      {
        return namespaceOrTypeEntity as NamespaceEntity;
      }

      // ... or otherwise a compile-time error occurs.
      if (namespaceOrTypeEntity is TypeEntity)
      {
        throw new NamespaceNameExpectedTypeNameFoundException(SyntaxNode.ToString());
      }

      throw new ApplicationException("Unexpected case in namespace name resolution.");
    }
  }
}
