using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Collections
{
  // ==================================================================================
  /// <summary>
  /// This interface represents the operations that are allowed through a restricted 
  /// list.
  /// </summary>
  // ==================================================================================
  internal interface IRestrictedList<T> : IEnumerable<T>
  {
    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the list.
    /// </summary>
    /// <param name="item">Item to add to the list.</param>
    // ----------------------------------------------------------------------------------
    void Add(T item);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the index of the specified item.
    /// </summary>
    /// <param name="item">Item to search for.</param>
    /// <returns>
    /// Index of item if found; otherwise, -1.
    /// </returns>
    // ----------------------------------------------------------------------------------
    int IndexOf(T item);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the item at the specified index position.
    /// </summary>
    // ----------------------------------------------------------------------------------
    T this[int index] { get; }

    // ----------------------------------------------------------------------------------
    /// <summary>Checks if the list contains the specified item or not.</summary>
    /// <returns>
    /// 	<strong>True</strong>, if the item found; otherwise,
    /// <strong>false</strong>.
    /// </returns>
    /// <param name="item">Item to search for in the list.</param>
    // ----------------------------------------------------------------------------------
    bool Contains(T item);

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
    void CopyTo(T[] array, int arrayIndex);

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of items in the list.
    /// </summary>
    // ----------------------------------------------------------------------------------
    int Count { get; }
  }

  // ==================================================================================
  /// <summary>
  /// This type implements the IRestricted interface so that through the class only
  /// read-operations are ellowed.
  /// </summary>
  // ==================================================================================
  public class RestrictedList<T> : IRestrictedList<T>
  {
    private List<T> _Items = new List<T>();

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the list.
    /// </summary>
    /// <param name="item">Item to add to the list.</param>
    // ----------------------------------------------------------------------------------
    void IRestrictedList<T>.Add(T item)
    {
      _Items.Add(item);
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
  }
}
