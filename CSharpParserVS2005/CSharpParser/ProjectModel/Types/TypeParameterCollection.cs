using System.Collections.Generic;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a collection of type parameters.
  /// </summary>
  // ==================================================================================
  public class TypeParameterCollection : List<TypeParameter>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of project files.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameterCollection()
    {
    }

    #endregion
  }
}