using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a syntax node based reference to a semantic entity.
  /// </summary>
  /// <typeparam name="TTargetEntity">The type of the target entity, must be a subclass of SemanticEntity.</typeparam>
  /// <typeparam name="TSyntaxNode">The type of the syntax node, must be a subclass of ISyntaxNode.</typeparam>
  // ================================================================================================
  public abstract class SyntaxNodeBasedSemanticEntityReference<TTargetEntity, TSyntaxNode> : SemanticEntityReference<TTargetEntity>
    where TTargetEntity : class, ISemanticEntity
    where TSyntaxNode : ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNodeBasedSemanticEntityReference{TTargetEntity,TSyntaxNode}"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNodeBasedSemanticEntityReference(TSyntaxNode syntaxNode)
    {
      SyntaxNode = syntaxNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node that represents the referenced entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TSyntaxNode SyntaxNode { get; protected set; }
  }
}
