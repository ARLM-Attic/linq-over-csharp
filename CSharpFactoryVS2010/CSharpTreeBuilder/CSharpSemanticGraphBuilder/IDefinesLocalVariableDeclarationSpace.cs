using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of entities that define a local variable declaration space.
  /// </summary>
  // ================================================================================================
  public interface IDefinesLocalVariableDeclarationSpace : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an entity from the declaration space by name.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <returns>The entity declared with the supplied name or null if no such declaration.</returns>
    // ----------------------------------------------------------------------------------------------
    INamedEntity GetDeclaredEntityByName(string name);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a declaration of an entity.
    /// </summary>
    /// <param name="namedEntity">A named entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddDeclaration(INamedEntity namedEntity);
  }
}
