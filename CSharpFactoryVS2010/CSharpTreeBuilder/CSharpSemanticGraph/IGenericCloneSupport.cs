using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines of behavior of a semantic object that supports generic type parameters.
  /// </summary>
  // ================================================================================================
  public interface IGenericCloneSupport
  { 
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a semantic object cloned from this object using as a generic template.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A semantic object constructed from this object using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    IGenericCloneSupport GetGenericClone(TypeParameterMap typeParameterMap);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this object was cloned from a generic template.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsGenericClone { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the direct generic template of this object. Null if this object was not cloned.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IGenericCloneSupport DirectGenericTemplate { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first generic template in the chain of template->clone relationships,
    /// where none of the type parameters were bound. Null if this object was not cloned.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IGenericCloneSupport OriginalGenericTemplate { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters and type arguments associated with this object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    TypeParameterMap TypeParameterMap { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of the objects cloned from this generic template object 
    /// by replacing type parameters with type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<IGenericCloneSupport> GenericClones { get; }
  }
}
