using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of entities that define a declaration space.
  /// </summary>
  // ================================================================================================
  public interface IDefinesDeclarationSpace : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddDeclaration(INamedEntity namedEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes the declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    void RemoveDeclaration(INamedEntity namedEntity);
  }
}
