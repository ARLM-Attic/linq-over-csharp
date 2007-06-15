using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an array initializer.
  /// </summary>
  // ==================================================================================
  public sealed class ArrayInitializer : Initializer
  {
    #region Private fields

    private List<Initializer> _Initializers = new List<Initializer>();
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new array initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public ArrayInitializer(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of initializers.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Initializer> Initializers
    {
      get { return _Initializers; }
    }

    #endregion
  }
}
