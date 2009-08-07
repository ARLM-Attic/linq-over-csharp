using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a namespace entity, based on a type-or-namespace AST node.
  /// </summary>
  // ================================================================================================
  public sealed class TypeOrNamespaceNodeBasedNamespaceEntityReference 
    : SyntaxNodeBasedSemanticEntityReference<NamespaceEntity, TypeOrNamespaceNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrNamespaceNodeBasedNamespaceEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNodeBasedNamespaceEntityReference(TypeOrNamespaceNode syntaxNode)
      : base(syntaxNode)
    {
    }
  }
}
