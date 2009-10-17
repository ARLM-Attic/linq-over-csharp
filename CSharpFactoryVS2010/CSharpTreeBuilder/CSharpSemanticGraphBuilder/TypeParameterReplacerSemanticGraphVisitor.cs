using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements a semantic graph visitor that replaces type parameters 
  /// with actual type arguments in a subtree of the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterReplacerSemanticGraphVisitor : SemanticGraphVisitor
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Replaces type parameters with type arguments in the specified entity.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override bool Visit(ClassEntity entity)
    {
      var baseTypeReferenceList = entity.BaseTypeReferences.ToList();
      for (int i = 0; i < baseTypeReferenceList.Count; i++)
      {
        baseTypeReferenceList[i] = GetTypeParameterMappedReference(baseTypeReferenceList[i], entity.TypeParameterMap);
      }

      return true;
    }

    #region Private methods

    private SemanticEntityReference<TypeEntity> GetTypeParameterMappedReference(
      SemanticEntityReference<TypeEntity> typeReference, TypeParameterMap typeParameterMap)
    {
      if (typeReference.TargetEntity != null)
      {
        return new DirectSemanticEntityReference<TypeEntity>(typeReference.TargetEntity.ApplyTypeParameterMap(typeParameterMap));
      }

      return typeReference;
    }

    #endregion
  }
}
