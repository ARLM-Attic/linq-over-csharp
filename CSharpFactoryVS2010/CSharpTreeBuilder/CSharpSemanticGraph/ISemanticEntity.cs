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
  public interface ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISemanticEntity Parent { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is a (direct or indirect) parent of another entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>
    /// True if this entity is a (direct or indirect) parent of another entity, false otherwise.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    bool IsParentOf(ISemanticEntity entity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root namespace of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    RootNamespaceEntity RootNamespace { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the semantic graph that this entity belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticGraph SemanticGraph { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the program that this entity belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Program Program { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ReadOnlyCollection<ISyntaxNode> SyntaxNodes { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reflected metadata (eg. type) that this entity was created from. Can be null.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    object ReflectedMetadata { get;  }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    void AcceptVisitor(SemanticGraphVisitor visitor);
  }
}
