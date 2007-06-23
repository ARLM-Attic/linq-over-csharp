using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Collections
{
  // ====================================================================================
  /// <summary>
  /// This abstract generic class is intended to be the base class for collections that
  /// may access items either by indexes and by string keys.
  /// </summary>
  // ====================================================================================
  [Serializable]
  public abstract class RestrictedIndexedCollection<TValue> : IList<TValue>, IReadOnlySupport
  {
    #region Private fields

    private readonly List<TValue> _Items;
    private readonly Dictionary<string, TValue> _indexedItems;
    protected bool _IsReadOnly;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------
    /// <summary>Creates an empty collection.</summary>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection()
    {
      _Items = new List<TValue>();
      _indexedItems = new Dictionary<string, TValue>();
      _IsReadOnly = false;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>Creates a collection with the specified capacity.</summary>
    /// <param name="capacity">Initial capacity of the collection.</param>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection(int capacity)
    {
      _Items = new List<TValue>(capacity);
      _indexedItems = new Dictionary<string, TValue>(capacity);
      _IsReadOnly = false;
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the count of items in this collection
    /// </summary>
    // ----------------------------------------------------------------------------------
    public int Count
    {
      get { return _Items.Count; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this collection is read-only or not.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool IsReadOnly
    {
      get { return _IsReadOnly; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the item of this collection at the specified index.
    /// </summary>
    /// <param name="index">Zero-based index of the item.</param>
    // ----------------------------------------------------------------------------------
    public TValue this[int index]
    {
      get { return _Items[index]; }
      set
      {
        CheckReadOnly();
        string oldKey = GetKeyOfItem(_Items[index]);
        string newKey = GetKeyOfItem(value);
        _indexedItems.Remove(oldKey);
        _Items[index] = value;
        _indexedItems.Add(newKey, value);
      }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the item of this collection specified by the key given.
    /// </summary>
    /// <param name="key">Key of the item</param>
    /// <returns>Item belongong to the specified key.</returns>
    // ----------------------------------------------------------------------------------
    public TValue this[string key]
    {
      get { return _indexedItems[key]; }
    }

    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the first
    /// occurrence within the entire <b>List</b>.
    /// </summary>
    /// <returns>
    /// The zero-based index of the first occurrence of
    /// <span class="parameter">item</span> within the entire <b>List</b>, if found; otherwise,
    /// –1.
    /// </returns>
    /// <param name="item">
    /// The object to locate in the <strong>List</strong>. The value can be a null
    /// reference (<b>Nothing</b> in Visual Basic) for reference types.
    /// </param>
    // ----------------------------------------------------------------------------------
    public int IndexOf(TValue item)
    {
      return _Items.IndexOf(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>Inserts an element into the <strong>List</strong> at the specified index.</summary>
    /// <param name="index">
    /// 	<para>The zero-based index at which <span class="parameter">item</span> should be
    ///     inserted.</para>
    /// </param>
    /// <param name="item">
    /// 	<para>The object to insert. The value can be a null reference (<b>Nothing</b> in
    ///     Visual Basic) for reference types.</para>
    /// </param>
    // ----------------------------------------------------------------------------------
    void IList<TValue>.Insert(int index, TValue item)
    {
      _indexedItems.Add(GetKeyOfItem(item), item);
      _Items.Insert(index, item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Removes the item at the specified index.
    /// </summary>
    /// <param name="index">Index of item to remove.</param>
    // ----------------------------------------------------------------------------------
    void IList<TValue>.RemoveAt(int index)
    {
      string key = GetKeyOfItem(_Items[index]);
      _indexedItems.Remove(key);
      _Items.RemoveAt(index);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified item to the collection.
    /// </summary>
    /// <param name="item"></param>
    // ----------------------------------------------------------------------------------
    public void Add(TValue item)
    {
      _indexedItems.Add(GetKeyOfItem(item), item);
      _Items.Insert(_Items.Count, item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Clears all items from the collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void Clear()
    {
      _Items.Clear();
      _indexedItems.Clear();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the collection contains the item specified.
    /// </summary>
    /// <param name="item">Item to check for existence.</param>
    /// <returns>True, if the item is in the collection; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------
    public bool Contains(TValue item)
    {
      return _Items.Contains(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the collection contains an item with the specified key.
    /// </summary>
    /// <param name="key">Key of the item to check.</param>
    /// <returns>True, if the item is in the collection; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------
    public bool ContainsKey(string key)
    {
      return _indexedItems.ContainsKey(key);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Tries to obtain the value with the specified key.
    /// </summary>
    /// <param name="key">Key to look up.</param>
    /// <param name="value">Valueif found</param>
    /// <returns>
    /// True, if key found; otherwise, false.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public bool TryGetValue(string key, out TValue value)
    {
      return _indexedItems.TryGetValue(key, out value);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Copies the content of the collection into a native .NET array.
    /// </summary>
    /// <param name="array">Destination array</param>
    /// <param name="arrayIndex">First index</param>
    // ----------------------------------------------------------------------------------
    public void CopyTo(TValue[] array, int arrayIndex)
    {
      _Items.CopyTo(array, arrayIndex);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Removes the specified item from the collection.
    /// </summary>
    /// <param name="item">Item to remove</param>
    /// <returns>True, if the item has been removed; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------
    bool ICollection<TValue>.Remove(TValue item)
    {
      int index = _Items.IndexOf(item);
      if (index < 0) return false;
      (this as IList<TValue>).RemoveAt(index);
      return true;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the enumerator for this collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the enumerator for this collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public IEnumerator GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

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

    #endregion

    #region Abstract methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the key of the specified item.
    /// </summary>
    /// <param name="item">Item used to determine the key.</param>
    // ----------------------------------------------------------------------------------
    protected abstract string GetKeyOfItem(TValue item);

    #endregion

    #region Private methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the collection is read-only.
    /// </summary>
    /// <exception cref="InvalidOperationException">The colletion is read-only.</exception>
    // ----------------------------------------------------------------------------------
    private void CheckReadOnly()
    {
      if (_IsReadOnly)
      {
        throw new ImmutableChangedException();
      }
    }

    #endregion
  }
}