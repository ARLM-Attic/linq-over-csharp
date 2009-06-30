// ================================================================================================
// CompilationUnitNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type defines a collection of compilation unit nodes.
  /// </summary>
  // ================================================================================================
  public sealed class CompilationUnitNodeCollection : ImmutableIndexedCollection<CompilationUnitNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    // ----------------------------------------------------------------------------------------------
    protected override string GetKeyOfItem(CompilationUnitNode item)
    {
      return item.FullName;
    }
  }
}