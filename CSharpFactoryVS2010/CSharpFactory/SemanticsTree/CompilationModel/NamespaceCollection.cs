// ================================================================================================
// NamespaceCollection.cs
//
// Created: 2009.05.10, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of <see cref="NamespaceItem"/> instances.
  /// </summary>
  /// <remarks>
  /// This collection can be indexed with the name of the namespace.
  /// </remarks>
  // ================================================================================================
  public sealed class NamespaceCollection : ImmutableIndexedCollection<NamespaceItem>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    /// <returns>The name of the namespace item.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override string GetKeyOfItem(NamespaceItem item)
    {
      return item.Name;
    }
  }
}