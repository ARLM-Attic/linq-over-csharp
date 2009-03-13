using CSharpFactory.Collections;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class stores information about a logical namespace.
  /// </summary>
  /// <remarks>
  /// A logical namespace can contain one or more namespace fragments. Each namespace 
  /// declaration in a file is a separate namespace fragment with types declared 
  /// within.
  /// </remarks>
  // ==================================================================================
  public sealed class Namespace
  {
    #region Private fields

    private readonly string _Name;
    private readonly NamespaceFragmentCollection _Fragments = new NamespaceFragmentCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace instance with an empty list of fragments.
    /// </summary>
    /// <param name="name"></param>
    // --------------------------------------------------------------------------------
    public Namespace(string name)
    {
      _Name = name;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the namespace. This is the full name of the namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of physical fragments belonging to this namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragmentCollection Fragments
    {
      get { return _Fragments; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a list of namespaces that can be indexed by the full name
  /// of the namespace.
  /// </summary>
  // ==================================================================================
  public sealed class NamespaceCollection : ImmutableIndexedCollection<Namespace>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">Namespace item.</param>
    /// <returns>
    /// Full name of the namespace.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(Namespace item)
    {
      return item.Name;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if there is any namespace partially specified by the key.
    /// </summary>
    /// <param name="key">Namespace partial key</param>
    /// <param name="parent">Parent namespace</param>
    /// <returns>
    /// True if the key is contained fully or partially.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool ContainsPartially(string key, out string parent)
    {
      parent = string.Empty;
      if (ContainsKey(key)) return true;
      string start = key + ".";
      string part = "." + start;
      foreach (string otherKey in Keys)
      {
        if (otherKey.StartsWith(start)) return true;
        int pos = otherKey.IndexOf(part);
        if (pos > 0)
        {
          parent = otherKey.Substring(0, pos);
          return true;
        }
      }
      return false;
    }
  }
}
