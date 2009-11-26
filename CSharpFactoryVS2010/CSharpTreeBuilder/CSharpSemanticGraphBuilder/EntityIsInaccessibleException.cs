using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when the namespace-or-type-name resolution finds a member 
  /// that is inaccessible due to its protection level.
  /// </summary>
  // ================================================================================================
  public sealed class EntityIsInaccessibleException : NamespaceOrTypeNameNodeResolverException
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityIsInaccessibleException"/> class.
    /// </summary>
    /// <param name="inaccessibleEntity">The inaccessible entity.</param>
    // ----------------------------------------------------------------------------------------------
    public EntityIsInaccessibleException(IHasAccessibility inaccessibleEntity)
    {
      InaccessibleEntity = inaccessibleEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the inaccessible entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IHasAccessibility InaccessibleEntity { get; private set; }
  }
}
