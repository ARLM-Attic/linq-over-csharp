using CSharpFactory.ProjectModel;

namespace CSharpFactory.ProjectContent
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the roles of a project content object thatis able to 
  /// collect C# project information (source files, references, etc.)
  /// </summary>
  // ==================================================================================
  public interface IProjectContentProvider
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    string Name { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    string WorkingFolder { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the projekt information and fills up the compilation unit accordingly.
    /// </summary>
    /// <param name="compilationUnit">CompilationUnit to fill up.</param>
    // --------------------------------------------------------------------------------
    void CollectProjectItems(CompilationUnit compilationUnit);
  }
}
