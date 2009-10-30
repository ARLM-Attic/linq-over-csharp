using System.Collections.Generic;
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
    SemanticEntity Parent { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity is a (direct or indirect) parent of another entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <returns>
    /// True if this entity is a (direct or indirect) parent of another entity, false otherwise.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    bool IsParentOf(SemanticEntity entity);

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
    SemanticGraph SemanticGraph { get; set; }

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
    /// Adds a syntax node to the SyntaxNodes collection.
    /// </summary>
    /// <param name="syntaxNode">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    void AddSyntaxNode(ISyntaxNode syntaxNode);
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reflected metadata (eg. type) that this entity was created from. Can be null.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    object ReflectedMetadata { get; set; }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a semantic entity constructed from this entity by replacing type parameters 
    /// with type arguments.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    SemanticEntity GetConstructedEntity(TypeParameterMap typeParameterMap);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a constructed entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsConstructed { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the generic template of this entity. 
    /// Null if this entity was not constructed from another entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticEntity TemplateEntity { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters and type arguments associated with this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    TypeParameterMap TypeParameterMap { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the entities constructed from this entity 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<SemanticEntity> ConstructedEntities { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    void AcceptVisitor(SemanticGraphVisitor visitor);

  }
}
