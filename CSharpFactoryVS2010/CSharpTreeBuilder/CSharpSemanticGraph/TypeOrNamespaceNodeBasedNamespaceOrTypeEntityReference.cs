using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a namespace or type entity, based on a type-or-namespace AST node.
  /// </summary>
  // ================================================================================================
  public sealed class TypeOrNamespaceNodeBasedNamespaceOrTypeEntityReference 
    : SyntaxNodeBasedSemanticEntityReference<NamespaceOrTypeEntity, TypeOrNamespaceNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNodeBasedNamespaceOrTypeEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNodeBasedNamespaceOrTypeEntityReference(TypeOrNamespaceNode syntaxNode)
      : base(syntaxNode)
    {
    }
  }
}
