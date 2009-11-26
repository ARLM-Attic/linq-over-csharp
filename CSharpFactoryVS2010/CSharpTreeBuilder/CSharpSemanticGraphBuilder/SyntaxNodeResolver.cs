using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of resolvers where the source object is a syntax node.
  /// </summary>
  /// <typeparam name="TTargetType">The type of the target object. Any class.</typeparam>
  /// <typeparam name="TSyntaxNodeType">The type of the source object. Must be an ISyntaxNode.</typeparam>
  // ================================================================================================
  public abstract class SyntaxNodeResolver<TTargetType, TSyntaxNodeType> : Resolver<TTargetType>
    where TTargetType : class
    where TSyntaxNodeType : ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeResolver{TTargetEntity,TSyntaxNode}"/> class.
    /// </summary>
    /// <param name="syntaxNode">The source object to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNodeResolver(TSyntaxNodeType syntaxNode)
    {
      SyntaxNode = syntaxNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node to be resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TSyntaxNodeType SyntaxNode { get; protected set; }
  }
}
