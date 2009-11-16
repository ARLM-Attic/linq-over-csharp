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
    /// Returns a semantic entity cloned from this entity using as a generic template.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    IGenericSupportingSemanticEntity GetGenericClone(TypeParameterMap typeParameterMap);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this entity was cloned from a generic template.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool HasGenericTemplate { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the direct generic template of this entity. 
    /// Null if this entity was not constructed from another entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IGenericSupportingSemanticEntity DirectGenericTemplate { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first generic template in the chain of template->clone relationships,
    /// where none of the type parameters were bound.
    /// Null if this entity was not constructed from another entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IGenericSupportingSemanticEntity UnboundGenericTemplate { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters and type arguments associated with this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    TypeParameterMap TypeParameterMap { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the entities cloned from this generic template entity 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<IGenericSupportingSemanticEntity> GenericCloneEntities { get; }
  }
}
