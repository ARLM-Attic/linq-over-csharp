using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface is implemented by members that can be overloaded.
  /// </summary>
  // ================================================================================================
  public interface IOverloadableEntity : INamedEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of own type parameters (ie. not inherited from parents).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int OwnTypeParameterCount { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<ParameterEntity> Parameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a parameter to the entity.
    /// </summary>
    /// <param name="parameter">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    void AddParameter(ParameterEntity parameter);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a parameter from the entity.
    /// </summary>
    /// <param name="parameter">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    void RemoveParameter(ParameterEntity parameter);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Signature Signature { get; }
  }
}
