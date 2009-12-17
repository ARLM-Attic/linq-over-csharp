using System;
using System.Linq;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class implements an equality comparer on type parameter map object.
  /// Two type parameter maps are considered equal if they have the same type parameters,
  /// and mapped to the same type arguments.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterMapEqualityComparer : IEqualityComparer<TypeParameterMap>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates equality between two type parameter map objects.
    /// </summary>
    /// <param name="x">A type parameter map object.</param>
    /// <param name="y">A type parameter map object.</param>
    /// <returns>True if the two type parameter map objects are equal.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Equals(TypeParameterMap x, TypeParameterMap y)
    {
      if (x.Count != y.Count)
      {
        return false;
      }

      //// We compare all the key+value pairs in both type parameter maps.
      //// This should be done after ordering the key value pairs by key, but we can assume that equal maps
      //// will always have the keys in the same order, so we can skip the ordering to improve performance.

      var xKeyValuePairs = x.ToList();
      var yKeyValuePairs = y.ToList();

      for (int i = 0; i < xKeyValuePairs.Count; i++)
      {
        if (xKeyValuePairs[i].Key != yKeyValuePairs[i].Key 
          || xKeyValuePairs[i].Value != yKeyValuePairs[i].Value) 
        {
          return false;
        }
      }

      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the hash code of a type parameter map object.
    /// </summary>
    /// <param name="obj">A type parameter map object.</param>
    /// <returns>The hash code of the type parameter map object.</returns>
    // ----------------------------------------------------------------------------------------------
    public int GetHashCode(TypeParameterMap obj)
    {
      int hash = 0;

      foreach (var keyValuePair in obj)
      {
        unchecked
        {
          hash += keyValuePair.Key.GetHashCode() + keyValuePair.Value.GetHashCode();
        }
      }

      return hash;
    }
  }
}
