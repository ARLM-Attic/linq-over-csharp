using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by semantic entities that can have local variable child entities.
  /// </summary>
  // ================================================================================================
  public interface IHasLocalVariables : ISemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a local variable. 
    /// </summary>
    /// <param name="localVariableEntity">A local variable entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddLocalVariable(LocalVariableEntity localVariableEntity);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of local variables.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<LocalVariableEntity> LocalVariables { get; }
  }
}
