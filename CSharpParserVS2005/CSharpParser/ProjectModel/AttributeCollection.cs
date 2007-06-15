using System.Collections.Generic;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a collection of attribute declarations.
  /// </summary>
  // ==================================================================================
  public class AttributeCollection : List<AttributeDeclaration>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of project files.
    /// </summary>
    // --------------------------------------------------------------------------------
    public AttributeCollection()
    {
    }

    #endregion
  }
}