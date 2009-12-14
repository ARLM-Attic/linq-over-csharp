using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have local constant child entities.
  /// </summary>
  // ================================================================================================
  public interface IHasLocalConstants : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a local constant. 
    /// </summary>
    /// <param name="localConstantEntity">A local constant entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddLocalConstant(LocalConstantEntity localConstantEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of local constants.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<LocalConstantEntity> LocalConstants { get; }
  }
}
