using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines of behavior of a semantic entity that supports generic type parameters.
  /// </summary>
  // ================================================================================================
  public interface IGenericSupportingSemanticEntity : ISemanticEntity
  { 
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
    ISemanticEntity GetConstructedEntity(TypeParameterMap typeParameterMap);

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
    ISemanticEntity TemplateEntity { get; }

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
    IEnumerable<ISemanticEntity> ConstructedEntities { get; }
  }
}
