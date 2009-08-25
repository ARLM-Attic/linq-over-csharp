using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements an equality comparer on semantic entity references.
  /// Two semantic entity references are considered equal if they point to the same entity.
  /// Reference to null target entity does not equal to any other reference.
  /// </summary>
  // ================================================================================================
  public sealed class SemanticEntityReferenceEqualityComparer<TTargetType> : IEqualityComparer<SemanticEntityReference<TTargetType>>
    where TTargetType : SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates equality between two semantic entity references based on the TargetEntity.
    /// </summary>
    /// <param name="x">A semantic entity reference.</param>
    /// <param name="y">A semantic entity reference.</param>
    /// <returns>True if the two semantic entity references point to the same entity.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Equals(SemanticEntityReference<TTargetType> x, SemanticEntityReference<TTargetType> y)
    {
      return (x.TargetEntity == null || y.TargetEntity == null) ? false : x.TargetEntity == y.TargetEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the hash code of the TargetEntity.
    /// </summary>
    /// <param name="obj">A semantic entity reference.</param>
    /// <returns>The hash code of the TargetEntity.</returns>
    // ----------------------------------------------------------------------------------------------
    public int GetHashCode(SemanticEntityReference<TTargetType> obj)
    {
      return obj.TargetEntity == null ? 0 : obj.TargetEntity.GetHashCode();
    }
  }
}
