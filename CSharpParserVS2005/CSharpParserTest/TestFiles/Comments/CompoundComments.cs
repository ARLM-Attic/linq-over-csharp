using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// ==================================================================================
// Namespace comment
// ==================================================================================
namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is responsible for parsing all files in a C# project.
  /// </summary>
  // ==================================================================================
  public sealed class CompilationUnit
  {
    #region Private Fields

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a project parser using the specified working folder.
    /// </summary>
    /// <param name="workingFolder">Folder used as the working folder</param>
    // --------------------------------------------------------------------------------
    public CompilationUnit(string workingFolder): this(workingFolder, false)
    {
    }

    // --------------------------------------------------------------------------------
    /** <summary>
      * Creates a project parser using the specified working folder.
      * </summary>
      * <param name="workingFolder">Folder used as the working folder</param>
      * <param name="addCSharpFiles">
      * If true, all C# files are added to the project.
      * </param>
    */
    // --------------------------------------------------------------------------------
    public CompilationUnit(string workingFolder, bool addCSharpFiles)
    {
    }

    #endregion
  }
}
