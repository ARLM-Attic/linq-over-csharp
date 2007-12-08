using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class reresents a list of resolution nodes.
  /// </summary>
  // ==================================================================================
  public sealed class ResolutionNodeList : IList<ResolutionNodeBase>
  {
    #region Private fields

    private readonly List<ResolutionNodeBase> _Nodes = new List<ResolutionNodeBase>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty resolution node list.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList()
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the list with the specified nodes.
    /// </summary>
    /// <param name="items">Collection containing the nodes.</param>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList(IEnumerable<ResolutionNodeBase> items)
    {
      Merge(items);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the list with the trees of the specified hierarchy.
    /// </summary>
    /// <param name="hierarchy">Hierarchy with trees.</param>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList(NamespaceHierarchy hierarchy)
    {
      Merge(hierarchy);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the list is empty or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return _Nodes.Count == 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the list contains exactly one element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnambigous
    {
      get { return _Nodes.Count == 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Merge the specified list to this list.
    /// </summary>
    /// <param name="list">List to merge to this list</param>
    // --------------------------------------------------------------------------------
    public void Merge(IEnumerable<ResolutionNodeBase> list)
    {
      foreach (ResolutionNodeBase item in list) Add(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Merge the specified hierarchy to this list.
    /// </summary>
    /// <param name="hierarchy">Hierarchy to merge to this list</param>
    // --------------------------------------------------------------------------------
    public void Merge(NamespaceHierarchy hierarchy)
    {
      foreach (TypeResolutionTree tree in hierarchy.ResolutionTrees.Values) Add(tree);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the nodes defining namespace resolutions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<NamespaceResolutionNode> GetNamespaceResolutions()
    {
      List<NamespaceResolutionNode> results =
        new List<NamespaceResolutionNode>();
      foreach (ResolutionNodeBase item in _Nodes)
      {
        NamespaceResolutionNode node = item as NamespaceResolutionNode;
        if (node != null) results.Add(node);
      }
      return results;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the nodes defining type resolutions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeResolutionNode> GetTypeResolutions()
    {
      List<TypeResolutionNode> results =
        new List<TypeResolutionNode>();
      foreach (ResolutionNodeBase item in _Nodes)
      {
        TypeResolutionNode node = item as TypeResolutionNode;
        if (node != null) results.Add(node);
      }
      return results;
    }

    #endregion

    #region IList<ResolutioNode> implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Determines the index of a specific item in the 
    /// <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </summary>
    /// <returns>
    /// The index of item if found in the list; otherwise, -1.
    /// </returns>
    /// <param name="item">
    /// The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </param>
    // --------------------------------------------------------------------------------
    public int IndexOf(ResolutionNodeBase item)
    {
      return _Nodes.IndexOf(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/>
    /// at the specified index.
    /// </summary>
    /// <param name="item">
    /// The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </param>
    /// <param name="index">
    /// The zero-based index at which item should be inserted.
    /// </param>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// index is not a valid index in the 
    /// <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </exception>
    // --------------------------------------------------------------------------------
    public void Insert(int index, ResolutionNodeBase item)
    {
      _Nodes.Insert(index, item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the 
    /// specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// index is not a valid index in the 
    /// <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </exception>
    // --------------------------------------------------------------------------------
    public void RemoveAt(int index)
    {
      _Nodes.RemoveAt(index);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <returns>
    /// The element at the specified index.
    /// </returns>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// index is not a valid index in the 
    /// <see cref="T:System.Collections.Generic.IList`1"/>.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> 
    /// is read-only.
    /// </exception>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase this[int index]
    {
      get { return _Nodes[index]; }
      set { _Nodes[index] = value ; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <param name="item">The object to add to the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    // --------------------------------------------------------------------------------
    public void Add(ResolutionNodeBase item)
    {
      _Nodes.Add(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes all items from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. 
    /// </exception>
    // --------------------------------------------------------------------------------
    public void Clear()
    {
      _Nodes.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/>
    /// contains a specific value.
    /// </summary>
    /// <returns>
    /// true if item is found in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
    /// </returns>
    /// <param name="item">
    /// The object to locate in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    // --------------------------------------------------------------------------------
    public bool Contains(ResolutionNodeBase item)
    {
      return _Nodes.Contains(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Copies the elements of the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/> to an 
    /// <see cref="T:System.Array"/>, starting at a particular 
    /// <see cref="T:System.Array"/> index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional <see cref="T:System.Array"/> that is the destination of 
    /// the elements copied from 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>. The 
    /// <see cref="T:System.Array"/> must have zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">
    /// The zero-based index in array at which copying begins.
    /// </param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// arrayIndex is less than 0.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// array is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    /// array is multidimensional.-or-arrayIndex is equal to or greater than the 
    /// length of array.-or-The number of elements in the source 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than 
    /// the available space from arrayIndex to the end of the destination array.
    /// -or-Type T cannot be cast automatically to the type of the destination array.
    /// </exception>
    // --------------------------------------------------------------------------------
    public void CopyTo(ResolutionNodeBase[] array, int arrayIndex)
    {
      _Nodes.CopyTo(array, arrayIndex);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes the first occurrence of a specific object from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"></see>.
    /// </summary>
    /// <returns>
    /// true if item was successfully removed from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. 
    /// This method also returns false if item is not found in the original 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    /// <param name="item">
    /// The object to remove from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    // --------------------------------------------------------------------------------
    public bool Remove(ResolutionNodeBase item)
    {
      return _Nodes.Remove(item);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of elements contained in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <returns>
    /// The number of elements contained in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    // --------------------------------------------------------------------------------
    public int Count
    {
      get { return _Nodes.Count; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </summary>
    /// <returns>
    /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is 
    /// read-only; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool IsReadOnly
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used 
    /// to iterate through the collection.
    /// </returns>
    // --------------------------------------------------------------------------------
    IEnumerator<ResolutionNodeBase> IEnumerable<ResolutionNodeBase>.GetEnumerator()
    {
      return _Nodes.GetEnumerator();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used 
    /// to iterate through the collection.
    /// </returns>
    // --------------------------------------------------------------------------------
    public IEnumerator GetEnumerator()
    {
      return _Nodes.GetEnumerator();
    }

    #endregion
  }
}