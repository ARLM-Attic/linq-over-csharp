// ================================================================================================
// EventArguments.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.ComponentModel;

namespace CSharpTreeBuilder.Collections
{
  // ==================================================================================
  /// <summary>
  /// Represents cancel event arguments for events that deal with an item.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  // ==================================================================================
  public class ItemedCancelEventArgs<T> : CancelEventArgs
  {
    private T _Item;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the specified cancellation flag and the provided item.
    /// </summary>
    /// <param name="cancel">Cancellation flag.</param>
    /// <param name="item">Item used in this event.</param>
    // ----------------------------------------------------------------------------------
    public ItemedCancelEventArgs(bool cancel, T item)
      : base(cancel)
    {
      _Item = item;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the provided item.
    /// </summary>
    /// <param name="item">Item used in this event.</param>
    // ----------------------------------------------------------------------------------
    public ItemedCancelEventArgs(T item)
    {
      _Item = item;
    } 
    
    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the item belonging to the event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public T Item
    {
      get { return _Item; }
      set { _Item = value; }
    }
  }

  // ==================================================================================
  /// <summary>
  /// Represents event arguments for events that deal with an item.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  // ==================================================================================
  public class ItemedEventArgs<T> : EventArgs
  {
    private T _Item;

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the provided item.
    /// </summary>
    /// <param name="item">Item used in this event.</param>
    // ----------------------------------------------------------------------------------
    public ItemedEventArgs(T item)
    {
      _Item = item;
    } 
    
    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the item belonging to the event.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public T Item
    {
      get { return _Item; }
      set { _Item = value; }
    }
  }
}
