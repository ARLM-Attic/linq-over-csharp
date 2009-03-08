using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CSharpFactory.CodeExplorer
{
  // ====================================================================================
  /// <summary>
  /// Generic types implementing a list which can be sorted by any property of the
  /// element instances.
  /// </summary>
  /// <typeparam name="TValue">Type of elements in the sortable list.</typeparam>
  // ====================================================================================
  [Serializable]
  public sealed class SortableCollection<TValue> : List<TValue>
  {
    #region Lifecyle methods

    // ------------------------------------------------------------------------------
    /// <summary>
    /// Initialies an empty sortable list.
    /// </summary>
    // ------------------------------------------------------------------------------
    public SortableCollection() { }

    // ------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the sortable list from the specified list.
    /// </summary>
    /// <param name="initialList">Initial values of the sortable list.</param>
    // ------------------------------------------------------------------------------
    public SortableCollection(IEnumerable<TValue> initialList) : base(initialList) { }

    #endregion

    #region SortableCollection's new members

    // ------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves a list where the values are sorted by the specified property value
    /// with ascending order.
    /// </summary>
    /// <param name="propName"></param>
    /// <returns></returns>
    // ------------------------------------------------------------------------------
    public SortableCollection<TValue> SortBy(string propName)
    {
      return SortBy(propName, true);
    }

    // ------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves a list where the values are sorted by the specified property value.
    /// </summary>
    /// <param name="propName">Property name used when sorting.</param>
    /// <param name="order">
    /// True specifies ascending order; false means descending order.</param>
    /// <returns>
    /// An instance of the list where items are physically ordered according to the
    /// value of the specified property.
    /// </returns>
    // ------------------------------------------------------------------------------
    public SortableCollection<TValue> SortBy(string propName, bool order)
    {
      SortedList<object, Collection<int>> tempList =
        new SortedList<object, Collection<int>>();

      // --- Go through each element of the original array, obtain the specified property
      // --- and add the item to the sorted list with the propert value as the key.

      for (int i = 0; i < Count; i++)
      {
        TValue item = this[i];
        object key;
        PropertyInfo propInfo = item.GetType().GetProperty(propName,
          BindingFlags.Instance | BindingFlags.Public);
        if (propInfo != null)
        {
          key = propInfo.GetValue(item, null);
        }
        else
        {
          // --- Try using field instead of a property

          FieldInfo fieldInfo = item.GetType().GetField(propName,
            BindingFlags.Instance | BindingFlags.Public);
          if (fieldInfo == null)
          {
            throw new ArgumentException("The specified property cannot be found.", "propName");
          }
          key = fieldInfo.GetValue(item);
        }

        // --- Add the item to the sorted list

        Collection<int> indexArray;
        if (tempList.ContainsKey(key))
        {
          indexArray = tempList[key];
        }
        else
        {
          indexArray = new Collection<int>();
        }
        indexArray.Add(i);
        tempList[key] = indexArray;
      }

      // --- Create the result List

      SortableCollection<TValue> result = new SortableCollection<TValue>();
      foreach (Collection<int> itemIndexes in tempList.Values)
      {
        foreach (int idx in itemIndexes)
        {
          if (order)
          {
            result.Add(this[idx]);
          }
          else
          {
            result.Insert(0, this[idx]);
          }
        }
      }
      return result;
    }

    #endregion
  }
}
