using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Collections
{
  // ====================================================================================
  /// <summary>
  /// This interface defines the behaviour of indexed collections
  /// </summary>
  // ====================================================================================
  public interface IRestrictedIndexedCollection<TValue>
  {
    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the count of items in this collection
    /// </summary>
    // ----------------------------------------------------------------------------------
    int Count { get; }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the item of this collection at the specified index.
    /// </summary>
    /// <param name="index">Zero-based index of the item.</param>
    // ----------------------------------------------------------------------------------
    TValue this[int index] { get; }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the item of this collection specified by the key given.
    /// </summary>
    /// <param name="key">Key of the item</param>
    /// <returns>Item belongong to the specified key.</returns>
    // ----------------------------------------------------------------------------------
    TValue this[string key] { get; }

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
    int IndexOf(TValue item);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified item to the collection.
    /// </summary>
    /// <param name="item"></param>
    // ----------------------------------------------------------------------------------
    void Add(TValue item);

    /// <summary>
    /// Clears all items from the collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    void Clear();

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the collection contains the item specified.
    /// </summary>
    /// <param name="item">Item to check for existence.</param>
    /// <returns>True, if the item is in the collection; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------
    bool Contains(TValue item);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the collection contains an item with the specified key.
    /// </summary>
    /// <param name="key">Key of the item to check.</param>
    /// <returns>True, if the item is in the collection; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------
    bool ContainsKey(string key);

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
    bool TryGetValue(string key, out TValue value);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Copies the content of the collection into a native .NET array.
    /// </summary>
    /// <param name="array">Destination array</param>
    /// <param name="arrayIndex">First index</param>
    // ----------------------------------------------------------------------------------
    void CopyTo(TValue[] array, int arrayIndex);
  }

  // ====================================================================================
  /// <summary>
  /// This abstract generic class is intended to be the base class for collections that
  /// may access items either by indexes and by keys.
  /// </summary>
  // ====================================================================================
  [Serializable]
  public abstract class RestrictedIndexedCollection<TValue> : 
    IRestrictedIndexedCollection<TValue>, IEnumerable<TValue>
  {
    #region Private fields

    private readonly List<TValue> _Items;
    private readonly Dictionary<string, TValue> _indexedItems;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------
    /// <summary>Creates an empty collection.</summary>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection()
    {
      _Items = new List<TValue>();
      _indexedItems = new Dictionary<string, TValue>();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>Creates a collection with the specified capacity.</summary>
    /// <param name="capacity">Initial capacity of the collection.</param>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection(int capacity)
    {
      _Items = new List<TValue>(capacity);
      _indexedItems = new Dictionary<string, TValue>(capacity);
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
    /// Gets or sets the item of this collection at the specified index.
    /// </summary>
    /// <param name="index">Zero-based index of the item.</param>
    // ----------------------------------------------------------------------------------
    public TValue this[int index]
    {
      get { return _Items[index]; }
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
    /// <summary>
    /// Adds the specified item to the collection.
    /// </summary>
    /// <param name="item"></param>
    // ----------------------------------------------------------------------------------
    void IRestrictedIndexedCollection<TValue>.Add(TValue item)
    {
      _indexedItems.Add(GetKeyOfItem(item), item);
      _Items.Insert(_Items.Count, item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Clears all items from the collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    void IRestrictedIndexedCollection<TValue>.Clear()
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
  }
}