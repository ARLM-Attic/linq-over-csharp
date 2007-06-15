using System.Collections.Generic;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a collection of project files.
  /// </summary>
  // ==================================================================================
  public class ProjectFileCollection : List<ProjectFile>
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new empty collection of project files.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProjectFileCollection()
    {
    }

    #endregion
  }
}
