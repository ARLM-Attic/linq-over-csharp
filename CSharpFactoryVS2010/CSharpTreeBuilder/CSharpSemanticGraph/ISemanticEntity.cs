using System.Collections.ObjectModel;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines of behavior of all semantic entities.
  /// </summary>
  // ================================================================================================
  public interface ISemanticEntity: IGenericCloneSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISemanticEntity Parent { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddChild(ISemanticEntity entity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this node is a (direct or indirect) parent of another node.
    /// </summary>
    /// <param name="node">A semantic node.</param>
    /// <returns>
    /// True if this node is a (direct or indirect) parent of another node, false otherwise.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    bool IsParentOf(ISemanticEntity node);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the semantic graph that this node belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticGraph SemanticGraph { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    void AcceptVisitor(SemanticGraphVisitor visitor);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root namespace of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    RootNamespaceEntity RootNamespace { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the program that this entity belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Program Program { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ReadOnlyCollection<ISyntaxNode> SyntaxNodes { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Associates a syntax node with the entity.
    /// </summary>
    /// <param name="node">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    void AddSyntaxNode(ISyntaxNode node);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reflected metadata (eg. type) that this entity was created from. Can be null.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    object ReflectedMetadata { get;  }
  }
}
