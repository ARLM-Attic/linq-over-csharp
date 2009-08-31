using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements an equality comparer for signatures, used for determining whether two
  /// signatures can coexist in a declaration space. 
  /// (Ie. it reports equality for signatures that differ solely by ref and out.)
  /// </summary>
  /// <remarks>
  /// <para>
  /// Two signatures are considered equal if they have:
  ///  - the same name,
  ///  - the same number of type parameters,
  ///  - the same type and kind (value, reference, or output) parameters, considered in the order left to right.
  /// However it considers signatures equal if they differ solely in ref and out.
  /// </para>
  /// <para>
  /// If a signature contains unresolved types then it does not equal to any other signature (even itself).
  /// </para>
  /// <para>
  /// this class differs from the <see cref="SignatureEqualityComparerForCompleteMatching"/> class
  /// in the way it compares out and ref parameters.
  /// Members declared in a single declaration space cannot differ in signature solely by ref and out,
  /// so this comparer reports equality for signatures that would be the same 
  /// if all parameters in both methods with out modifiers were changed to ref modifiers. 
  /// </para>
  /// </remarks>
  // ================================================================================================
  public class SignatureEqualityComparerForDeclarationSpace : IEqualityComparer<Signature> 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines the equality of two signatures.
    /// </summary>
    /// <param name="x">A signature.</param>
    /// <param name="y">A signature.</param>
    /// <returns>True if the two signatures are considered equal, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public virtual bool Equals(Signature x, Signature y)
    {
      if (x.Name == null || y.Name == null
        || x.Parameters == null || y.Parameters == null)
      {
        return false;
      }

      return (x.Name == y.Name && x.TypeParameterCount == y.TypeParameterCount && AreEqual(x.Parameters, y.Parameters));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the hash code of the signature.
    /// </summary>
    /// <param name="obj">A signature.</param>
    /// <returns>The hash code of the signature.</returns>
    // ----------------------------------------------------------------------------------------------
    public virtual int GetHashCode(Signature obj)
    {
      return unchecked(obj.Name.GetHashCode() * (obj.TypeParameterCount + 1));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether to parameter list are considered equal.
    /// </summary>
    /// <param name="x">A parameter list.</param>
    /// <param name="y">A parameter list.</param>
    /// <returns>True, if the parameter list are equal, false otherwise.</returns>
    /// <remarks>Parameter lists are considered equal if they differ solely in ref and out.</remarks>
    // ----------------------------------------------------------------------------------------------
    private static bool AreEqual(IEnumerable<ParameterEntity> x, IEnumerable<ParameterEntity> y)
    {
      var xList = x.ToList();
      var yList = y.ToList();

      if (xList.Count != yList.Count)
      {
        return false;
      }

      for (int i = 0; i < xList.Count; i++)
      {
        if (xList[i].TypeReference == null || yList[i].TypeReference == null
          || xList[i].TypeReference.TargetEntity == null || yList[i].TypeReference.TargetEntity == null
          || xList[i].TypeReference.TargetEntity != yList[i].TypeReference.TargetEntity
          || 
            (
              (xList[i].Kind == ParameterKind.Output ? ParameterKind.Reference : xList[i].Kind) 
              !=
              (yList[i].Kind == ParameterKind.Output ? ParameterKind.Reference : yList[i].Kind)
            )
          )
        {
          return false;
        }
      }

      return true;
    }
  }
}
