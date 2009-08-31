using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a complete matching equality comparer for signatures.
  /// Ie. signatures that differ solely in ref and out are also considered different.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Two signatures are considered equal if they have:
  ///  - the same name,
  ///  - the same number of type parameters,
  ///  - the same type and kind (value, reference, or output) parameters, considered in the order left to right.
  /// </para>
  /// <para>
  /// If a signature contains unresolved types then it does not equal to any other signature (even itself).
  /// </para>
  /// <para>
  /// This class build on the behaviour of the <see cref="SignatureEqualityComparerForDeclarationSpace"/> class,
  /// but extends it by considering signatures different even if they differ solely in ref and out.
  /// This comparer is used for every purpose other than declaration spaces (e.g., hiding or overriding).
  /// </para>
  /// </remarks>
  // ================================================================================================
  public class SignatureEqualityComparerForCompleteMatching : SignatureEqualityComparerForDeclarationSpace
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines the equality of two signatures.
    /// </summary>
    /// <param name="x">A signature.</param>
    /// <param name="y">A signature.</param>
    /// <returns>True if the two signatures are considered equal, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Equals(Signature x, Signature y)
    {
      return base.Equals(x,y) && AllParameterKindsMatch(x.Parameters, y.Parameters);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether all parameter kinds are equal in two parameter lists.
    /// </summary>
    /// <param name="x">A parameter list.</param>
    /// <param name="y">A parameter list.</param>
    /// <returns>True, if both parameter list have the same parameter kinds, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool AllParameterKindsMatch(IEnumerable<ParameterEntity> x, IEnumerable<ParameterEntity> y)
    {
      var xList = x.ToList();
      var yList = y.ToList();

      if (xList.Count != yList.Count)
      {
        return false;
      }

      for (int i = 0; i < xList.Count; i++)
      {
        if (xList[i].Kind != yList[i].Kind)
        {
          return false;
        }
      }

      return true;
    }
  }
}
