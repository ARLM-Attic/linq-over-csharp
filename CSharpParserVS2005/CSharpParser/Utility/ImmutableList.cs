using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Collections
{
  // ====================================================================================
  /// <summary>
  ///     This generic class represents a list that implements the
  ///     <see cref="IReadOnlySupport"/> interface.
  /// </summary>
  /// <typeparam name="T">List item type</typeparam>
  // ====================================================================================
  public class ImmutableList<T> : IList<T>, IReadOnlySupport
  {
    #region Private fields

    private bool _IsReadOnly;
    private List<T> _Items;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty list.
    /// </summary>
    /// <remarks>
    /// The list is mutable after creation.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ImmutableList()
    {
      _Items = new List<T>();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a list with the specified capacity.
    /// </summary>
    /// <param name="capacity">Initial capacity of the list.</param>
    /// <remarks>
    /// The list is mutable after creation.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ImmutableList(int capacity)
    {
      _Items = new List<T>(capacity);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a list with the specified initial items.
    /// </summary>
    /// <param name="collection">Collection holding the initial items.</param>
    /// <remarks>
    /// The list is mutable after creation.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public ImmutableList(IEnumerable<T> collection)
    {
      _Items = new List<T>(collection);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a list with the specified initial items.
    /// </summary>
    /// <param name="collection">Collection holding the initial items.</param>
    /// <param name="readOnly">
    /// Flag indicating if the collection should be immutable or not.
    /// </param>
    // ----------------------------------------------------------------------------------
    public ImmutableList(IEnumerable<T> collection, bool readOnly)
    {
      _Items = new List<T>(collection);
      _IsReadOnly = readOnly;
    }

    #endregion

    #region IList<T> implementation

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the list.
    /// </summary>
    /// <param name="item">Item to add to the list.</param>
    // ----------------------------------------------------------------------------------
    public void Add(T item)
    {
      CheckReadOnly();
      _Items.Add(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Clears the content of the list.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void Clear()
    {
      CheckReadOnly();
      _Items.Clear();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>Checks if the list contains the specified item or not.</summary>
    /// <returns>
    /// 	<strong>True</strong>, if the item found; otherwise,
    /// <strong>false</strong>.
    /// </returns>
    /// <param name="item">Item to search for in the list.</param>
    // ----------------------------------------------------------------------------------
    public bool Contains(T item)
    {
      return _Items.Contains(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Copies the content of the list into the specified array. Elements are copied
    /// from the index provided.
    /// </summary>
    /// <param name="array">Destination array.</param>
    /// <param name="arrayIndex">
    /// First index in the list that should be copied into the array.
    /// </param>
    // ----------------------------------------------------------------------------------
    public void CopyTo(T[] array, int arrayIndex)
    {
      _Items.CopyTo(array, arrayIndex);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Removes the specified item from the list.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    /// <returns>
    /// True, if the item has been removed from the list; otherwise, false.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public bool Remove(T item)
    {
      CheckReadOnly();
      return _Items.Remove(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of items in the list.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public int Count
    {
      get { return _Items.Count; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the index of the specified item.
    /// </summary>
    /// <param name="item">Item to search for.</param>
    /// <returns>
    /// Index of item if found; otherwise, -1.
    /// </returns>
    // ----------------------------------------------------------------------------------
    public int IndexOf(T item)
    {
      return _Items.IndexOf(item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Inserts the specified item at the given position.
    /// </summary>
    /// <param name="item">Item to insert.</param>
    /// <param name="index">Insertion position.</param>
    // ----------------------------------------------------------------------------------
    public void Insert(int index, T item)
    {
      CheckReadOnly();
      _Items.Insert(index, item);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Removes the index at the specified position.
    /// </summary>
    /// <param name="index">Item index.</param>
    // ----------------------------------------------------------------------------------
    public void RemoveAt(int index)
    {
      CheckReadOnly();
      _Items.RemoveAt(index);
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the item at the specified index position.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public T this[int index]
    {
      get { return _Items[index]; }
      set
      {
        CheckReadOnly();
        _Items[index] = value;
      }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Implements the foreach semantics.
    /// </summary>
    /// <returns>Collection enumerator.</returns>
    // ----------------------------------------------------------------------------------
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Implements the foreach semantics.
    /// </summary>
    /// <returns>Collection enumerator.</returns>
    // ----------------------------------------------------------------------------------
    public IEnumerator GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

    #endregion

    #region IReadOnlySupport Members

    // ----------------------------------------------------------------------------------
    /// <summary>Gets the flag indicating if this list is immutable or not.</summary>
    /// <value>
    /// 	<strong>True</strong>, if the list is immutable; otherwise,
    /// <strong>false</strong>.
    /// </value>
    // ----------------------------------------------------------------------------------
    public bool IsReadOnly
    {
      get { return _IsReadOnly; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Makes this instance read-only.
    /// </summary>
    /// <remarks>
    /// After calling this method, the collection cannot be made writeable again.
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public void MakeReadOnly()
    {
      _IsReadOnly = true;
    }

    #endregion

    #region Private methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the collection is read-only.
    /// </summary>
    /// <exception cref="ImmutableChangedException">The colletion is read-only.</exception>
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
