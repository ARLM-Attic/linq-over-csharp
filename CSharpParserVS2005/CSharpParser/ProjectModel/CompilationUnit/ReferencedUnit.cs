using CSharpParser.Collections;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstract compilation reference.
  /// </summary>
  /// <remarks>
  /// Compilation reference is a referenced assembly, a referenced project, or anything
  /// that holds information about types and namespaces external to the compilation
  /// unit.
  /// </remarks>
  // ==================================================================================
  public abstract class ReferencedUnit
  {
    #region Private fields

    private readonly string _Name;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new compilation reference with the specified name.
    /// </summary>
    /// <param name="name">Name of the compilation reference.</param>
    // --------------------------------------------------------------------------------
    protected ReferencedUnit(string name)
    {
      _Name = name;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the compilation reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of compilation references that can be indexed by 
  /// the name of the reference.
  /// </summary>
  // ==================================================================================
  public sealed class ReferencedUnitCollection : RestrictedCollection<ReferencedUnit>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">ReferencedUnit item.</param>
    /// <returns>
    /// Name of the compilation reference.
    /// </returns>
    // --------------------------------------------------------------------------------
    //protected override string GetKeyOfItem(ReferencedUnit item)
    //{
    //  return item.Name;
    //}
  }
}
