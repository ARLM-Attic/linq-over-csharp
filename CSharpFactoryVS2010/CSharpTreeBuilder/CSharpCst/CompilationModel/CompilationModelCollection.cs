// ================================================================================================
// CompilationModelCollection.cs
//
// Created: 2009.05.10, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of compilation models.
  /// </summary>
  // ================================================================================================
  public sealed class CompilationModelCollection : ImmutableIndexedCollection<CompilationModel>
  {
    protected override string GetKeyOfItem(CompilationModel item)
    {
      return item.Name;
    }
  }
}