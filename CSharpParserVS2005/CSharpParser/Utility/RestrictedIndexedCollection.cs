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
    private readonly Dictionary<string, TValue> _IndexedItems;
    protected bool _IsReadOnly;
    private event EventHandler<ItemedCancelEventArgs<TValue>> _BeforeAdd;
    private event EventHandler<ItemedEventArgs<TValue>> _AfterAdd;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------
    /// <summary>Creates an empty collection.</summary>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection()
    {
      _Items = new List<TValue>();
      _IndexedItems = new Dictionary<string, TValue>();
      _IsReadOnly = false;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>Creates a collection with the specified capacity.</summary>
    /// <param name="capacity">Initial capacity of the collection.</param>
    // ----------------------------------------------------------------------------------
    public RestrictedIndexedCollection(int capacity)
    {
      _Items = new List<TValue>(capacity);
      _IndexedItems = new Dictionary<string, TValue>(capacity);
      _IsReadOnly = false;
    }

    #endregion

    #region Public events

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds or removes event handler for the "BeforeAdd" event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public event EventHandler<ItemedCancelEventArgs<TValue>> BeforeAdd
    {
      add { _BeforeAdd += value; }
      remove { _BeforeAdd -= value; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds or removes event handler for the "AfterAdd" event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public event EventHandler<ItemedEventArgs<TValue>> AfterAdd
    {
      add { _AfterAdd += value; }
      remove { _AfterAdd -= value; }
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
        _IndexedItems.Remove(oldKey);
        _Items[index] = value;
        _IndexedItems.Add(newKey, value);
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
      get { return _IndexedItems[key]; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the keys of this collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public IEnumerable<string> Keys
    {
      get { return _IndexedItems.Keys; }
    }

    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified item to the collection.
    /// </summary>
    /// <param name="item">Item to add to the collection</param>
    // ----------------------------------------------------------------------------------
    public virtual void Add(TValue item)
    {
      if (_BeforeAdd != null)
      {
        ItemedCancelEventArgs<TValue> eventArgs = new ItemedCancelEventArgs<TValue>(item);
        _BeforeAdd(this, eventArgs);
        if (eventArgs.Cancel) return;
      }
      _IndexedItems.Add(GetKeyOfItem(item), item);
      _Items.Insert(_Items.Count, item);
      if (_AfterAdd != null)
      {
        _AfterAdd(this, new ItemedEventArgs<TValue>(item));
      }
    }

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
      _IndexedItems.Add(GetKeyOfItem(item), item);
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
      _IndexedItems.Remove(key);
      _Items.RemoveAt(index);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Clears all items from the collection.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void Clear()
    {
      _Items.Clear();
      _IndexedItems.Clear();
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
      return ContainsKey(GetKeyOfItem(item));
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
      return _IndexedItems.ContainsKey(key);
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
      return _IndexedItems.TryGetValue(key, out value);
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

  // ====================================================================================
  /// <summary>
  /// This class embeds strings to change the standard hash algorithm to use FNV hash 
  /// algorithm (see http://www.isthe.com/chongo/tech/comp/fnv for details)
  /// </summary>
  // ====================================================================================
  public sealed class FnvHashedString
  {
    // --- Constants used by the FNV hash algorithm

    private const UInt32 _OffsetBasis = 2166136261;
    private const UInt32 _FnvPrime = 16777619;

    private readonly string _Value;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates an FNV hashed string.
    /// </summary>
    /// <param name="value">Original string value.</param>
    // ----------------------------------------------------------------------------------
    private FnvHashedString(string value)
    {
      _Value = value;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the original string.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public string Value
    {
      get { return _Value; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks for equality.
    /// </summary>
    /// <param name="obj">Other object to check for equality.</param>
    /// <returns>
    /// True, if the two object values are equal.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public override bool Equals(object obj)
    {
      return _Value.Equals(obj);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates the hash code using the FNV algorithm.
    /// </summary>
    /// <returns>
    /// Hash value of the string.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public override int GetHashCode()
    {
      UInt32 hash = _OffsetBasis;
      for (int i = 0; i < _Value.Length; i++ )
      {
        char c = _Value[i];
        hash *= _FnvPrime;
        hash ^= (byte)c;
        hash *= _FnvPrime;
        hash ^= (byte)(c >> 8);
      }
      return (Int32) hash;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this value.
    /// </summary>
    /// <returns>String representation.</returns>
    // ----------------------------------------------------------------------------------
    public override string ToString()
    {
      return _Value;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Implicit conversion operator from FnvHashedString to string.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <returns>
    /// Converted value.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public static implicit operator string(FnvHashedString value)
    {
      return value.Value;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Implicit conversion operator from string to FnvHashedString.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <returns>
    /// Converted value.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public static implicit operator FnvHashedString(string value)
    {
      return new FnvHashedString(value);
    }
  }
}