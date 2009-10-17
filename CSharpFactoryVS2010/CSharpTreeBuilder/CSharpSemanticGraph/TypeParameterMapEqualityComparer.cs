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
      if (x.TypeParameters.Count() != y.TypeParameters.Count())
      {
        return false;
      }

      var xKeysOrdered = x.TypeParameters.OrderBy(typeParameter => typeParameter.ToString()).ToList();
      var yKeysOrdered = y.TypeParameters.OrderBy(typeParameter => typeParameter.ToString()).ToList();

      for (int i = 0; i < xKeysOrdered.Count; i++)
      {
        var key = xKeysOrdered[i];
        if ((key != yKeysOrdered[i]) || (x[key] != y[key]))
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
      return obj.TypeParameters.Count();
    }
  }
}
