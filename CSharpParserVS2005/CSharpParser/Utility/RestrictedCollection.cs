using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Collections
{
  // ==================================================================================
  /// <summary>
  /// This type implements a collection that can be cleared and items added, other
  /// operation are only accessible through the IList{T} interface.
  /// </summary>
  /// <typeparam name="T">Item type of the collection</typeparam>
  /// <remarks>
  /// Events can de declared for the Add method.
  /// </remarks>
  // ==================================================================================
  public class RestrictedCollection<T> : IList<T>, IReadOnlySupport
  {
    #region Private fields

    private readonly List<T> _Items = new List<T>();
    private bool _IsReadOnly = false;
    private event EventHandler<ItemedCancelEventArgs<T>> _BeforeAdd;
    private event EventHandler<ItemedEventArgs<T>> _AfterAdd;

    #endregion

    #region Public events

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds or removes event handler for the "BeforeAdd" event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public event EventHandler<ItemedCancelEventArgs<T>> BeforeAdd
    {
      add { _BeforeAdd += value; }
      remove { _BeforeAdd -= value; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds or removes event handler for the "AfterAdd" event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public event EventHandler<ItemedEventArgs<T>> AfterAdd
    {
      add { _AfterAdd += value; }
      remove { _AfterAdd -= value; }
    }
    
    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the list.
    /// </summary>
    /// <param name="item">Item to add to the list.</param>
    // ----------------------------------------------------------------------------------
    public void Add(T item)
    {
      if (_BeforeAdd != null)
      {
        ItemedCancelEventArgs<T> eventArgs = new ItemedCancelEventArgs<T>(item);
        _BeforeAdd(this, eventArgs);
        if (eventArgs.Cancel) return;
      }
      _Items.Add(item);
      if (_AfterAdd != null)
      {
        _AfterAdd(this, new ItemedEventArgs<T>(item));
      }
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
    /// Gets the item at the specified index position.
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
    /// Gets the number of items in the list.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public int Count
    {
      get { return _Items.Count; }
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Inserts the specified item at the given position.
    /// </summary>
    /// <param name="item">Item to insert.</param>
    /// <param name="index">Insertion position.</param>
    // ----------------------------------------------------------------------------------
    void IList<T>.Insert(int index, T item)
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
    void IList<T>.RemoveAt(int index)
    {
      CheckReadOnly();
      _Items.RemoveAt(index);
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
    bool ICollection<T>.Remove(T item)
    {
      CheckReadOnly();
      return _Items.Remove(item);
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
    IEnumerator IEnumerable.GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

    #endregion

    #region IReadOnlySupport implementation

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this collection is readonly or not.
    /// </summary>
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