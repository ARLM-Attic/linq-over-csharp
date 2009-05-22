// ================================================================================================
// SourceFileNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type defines a collection of source file nodes in the syntax tree.
  /// </summary>
  // ================================================================================================
  public sealed class SourceFileNodeCollection : ImmutableIndexedCollection<SourceFileNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    // ----------------------------------------------------------------------------------------------
    protected override string GetKeyOfItem(SourceFileNode item)
    {
      return item.FullName;
    }
  }
}