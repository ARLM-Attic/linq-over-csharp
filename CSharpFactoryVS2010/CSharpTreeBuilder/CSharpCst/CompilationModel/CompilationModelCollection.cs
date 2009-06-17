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
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    /// <returns>Key of the specified item.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override string GetKeyOfItem(CompilationModel item)
    {
      return item.Name;
    }
  }
}