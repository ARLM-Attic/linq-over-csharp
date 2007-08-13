using System;
using System.Collections.Generic;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class contains the result of a name or type resolution.
  /// </summary>
  /// <remarks>
  /// Any resolution may lead to ambiguity. This class is responsible to hold 
  /// references to all items causing ambiguity.
  /// </remarks>
  // ==================================================================================
  public sealed class ResolutionInfo
  {
    #region Private fields

    private readonly List<ResolutionItem> _Items = new List<ResolutionItem>();

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating that this name or type has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _Items.Count != 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating that this name or type has been resolved but is 
    /// ambigous.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAmbigous
    {
      get { return _Items.Count > 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating that this name or type has been successfully resolved 
    /// without any ambiguity.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool SuccessfullyResolved
    {
      get { return _Items.Count == 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the specified resolution item.
    /// </summary>
    /// <param name="index">Index of the item.</param>
    /// <returns>The item at the specified index.</returns>
    // --------------------------------------------------------------------------------
    public ResolutionItem this[int index]
    {
      get { return _Items[index]; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the target type of the successful resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionTarget Target
    {
      get
      {
        if (SuccessfullyResolved)
        {
          return _Items[0].Target;
        }
        throw new InvalidOperationException("This name or type has not been successfully resolved!");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the mode of the successful resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionMode Mode
    {
      get
      {
        if (SuccessfullyResolved)
        {
          return _Items[0].Mode;
        }
        throw new InvalidOperationException("This name or type has not been successfully resolved!");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object of the successfully resolving this name or type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics Resolver
    {
      get
      {
        if (SuccessfullyResolved)
        {
          return _Items[0].Resolver;
        }
        throw new InvalidOperationException("This name or type has not been successfully resolved!");
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the list of resolutions.
    /// </summary>
    /// <param name="item">Item to add to the list.</param>
    // --------------------------------------------------------------------------------
    public void Add(ResolutionItem item)
    {
      _Items.Add(item);
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This structure defines an item as the result of a type or name resolution.
  /// </summary>
  // ==================================================================================
  public struct ResolutionItem
  {
    #region Private fields

    public readonly ResolutionTarget Target;
    public readonly ResolutionMode Mode;
    public readonly ITypeCharacteristics Resolver;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new item describing a name or type resolution.
    /// </summary>
    /// <param name="target">Type of the resolution target.</param>
    /// <param name="mode">Mode of resolution</param>
    /// <param name="resolver">Object resolving the name or type.</param>
    // --------------------------------------------------------------------------------
    public ResolutionItem(ResolutionTarget target, ResolutionMode mode, 
      ITypeCharacteristics resolver)
    {
      Target = target;
      Mode = mode;
      Resolver = resolver;
    }

    #endregion
  }
}
