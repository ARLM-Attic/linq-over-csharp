// ================================================================================================
// ReferencedCompliation.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This type represents a reference to a compilation unit.
  /// </summary>
  // ================================================================================================
  public sealed class ReferencedCompilation : ReferencedUnit
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the specified compilation unit and name.
    /// </summary>
    /// <param name="project">Compilation unit instance</param>
    /// <param name="name">Name</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedCompilation(Project project, string name)
      : base(name)
    {
      Project = project;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ReferencedCompilation"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="projectProvider">The project provider.</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedCompilation(ProjectProviderBase projectProvider, string name)
      : base(name)
    {
      ProjectProvider = projectProvider;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit referenced by this instance.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Project Project { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the project provider referenced by this instance.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ProjectProviderBase ProjectProvider { get; private set; }
  }
}