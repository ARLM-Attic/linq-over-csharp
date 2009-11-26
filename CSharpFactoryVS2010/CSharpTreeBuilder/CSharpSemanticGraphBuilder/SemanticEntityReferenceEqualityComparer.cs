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
  public sealed class SemanticEntityReferenceEqualityComparer<TTargetType> : IEqualityComparer<Resolver<TTargetType>>
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
    public bool Equals(Resolver<TTargetType> x, Resolver<TTargetType> y)
    {
      return (x.Target == null || y.Target == null) ? false : x.Target == y.Target;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the hash code of the TargetEntity.
    /// </summary>
    /// <param name="obj">A semantic entity reference.</param>
    /// <returns>The hash code of the TargetEntity.</returns>
    // ----------------------------------------------------------------------------------------------
    public int GetHashCode(Resolver<TTargetType> obj)
    {
      return obj.Target == null ? 0 : obj.Target.GetHashCode();
    }
  }
}
