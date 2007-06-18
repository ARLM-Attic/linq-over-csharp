// ====================================================================================
// ImmutableIndexedCollection.cs
//
// Created by: NI, 2006.11.30
// ====================================================================================
using System;

namespace CSharpParser.Collections
{
  // ====================================================================================
  /// <summary>
  /// This class implements an indexed collection that can be set to read-only.
  /// </summary>
  /// <typeparam name="TKey">Type of the key.</typeparam>
  /// <typeparam name="TValue">Type of the item held in the collection.</typeparam>
  /// <remarks>
  /// Use the <see cref="MakeReadOnly"/> method tomake this collection instance read only.
  /// </remarks>
  // ====================================================================================
  [Serializable]
  public abstract class ImmutableIndexedCollection<TKey, TValue> : 
    IndexedCollection<TKey, TValue>, IReadOnlySupport
  {
    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Makes this instance read-only.
    /// </summary>
    /// <remarks>
    /// After calling this method, the collection cannot be made writeable again.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public virtual void MakeReadOnly()
    {
      _IsReadOnly = true;
    }
  }
}
