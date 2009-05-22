// ================================================================================================
// OutputItemCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using CSharpFactory.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of items to be written to the output.
  /// </summary>
  // ================================================================================================
  public sealed class OutputItemCollection : ICollection<OutputItem>, IReadOnlySupport
  {
    #region Private fields

    private readonly List<OutputItem> _Items = new List<OutputItem>();

    #endregion

    #region Public members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate 
    /// through the collection.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerator<OutputItem> GetEnumerator()
    {
      return _Items.GetEnumerator();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
    ///  through the collection.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <param name="item">
    /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public void Add(OutputItem item)
    {
      CheckReadOnly();
      _Items.Add(item);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public void Clear()
    {
      CheckReadOnly();
      _Items.Clear();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    /// <returns>
    /// true if <paramref name="item"/> is found in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public bool Contains(OutputItem item)
    {
      return _Items.Contains(item);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements 
    /// copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The 
    /// <see cref="T:System.Array"/> must have zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">
    /// The zero-based index in <paramref name="array"/> at which copying begins.
    /// </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// 	<paramref name="array"/> is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// 	<paramref name="arrayIndex"/> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> 
    /// is equal to or greater than the length of <paramref name="array"/>.
    /// -or-The number of elements in the source
    ///  <see cref="T:System.Collections.Generic.ICollection`1"/> 
    /// is greater than the available space from <paramref name="arrayIndex"/> 
    /// to the end of the destination <paramref name="array"/>.-or-Type 
    /// cannot be cast automatically to the type of the destination <paramref name="array"/>.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public void CopyTo(OutputItem[] array, int arrayIndex)
    {
      _Items.CopyTo(array, arrayIndex);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes the first occurrence of a specific object from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <param name="item">
    /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </param>
    /// <returns>
    /// true if <paramref name="item"/> was successfully removed from the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. 
    /// This method also returns false if <paramref name="item"/> is not found in the original 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">
    /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public bool Remove(OutputItem item)
    {
      CheckReadOnly();
      return _Items.Remove(item);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of elements contained in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </summary>
    /// <value></value>
    /// <returns>The number of elements contained in the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public int Count
    {
      get { return _Items.Count; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> 
    /// is read-only; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool IsReadOnly { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the object's state to read-only.
    /// </summary>
    /// <example>
    /// Once this object has been set to read-only it cannotbe set to changeable
    /// again.
    /// </example>
    /// <seealso cref="IsReadOnly">IsReadOnly Property</seealso>
    // ----------------------------------------------------------------------------------------------
    public void MakeReadOnly()
    {
      IsReadOnly = true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this collection is read only or not.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void CheckReadOnly()
    {
      if (IsReadOnly) throw new NotSupportedException();
    }

    #endregion
  }
}