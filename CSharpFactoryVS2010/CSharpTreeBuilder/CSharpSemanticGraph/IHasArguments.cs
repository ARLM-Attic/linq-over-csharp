using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have arguments as child entities.
  /// </summary>
  // ================================================================================================
  public interface IHasArguments : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an argument entity. 
    /// </summary>
    /// <param name="argumentEntity">An argument entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddArgument(ArgumentEntity argumentEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<ArgumentEntity> Arguments { get; }
  }
}
