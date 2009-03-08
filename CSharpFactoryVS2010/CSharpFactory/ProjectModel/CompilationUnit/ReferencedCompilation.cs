// ================================================================================================
// ReferencedCompilation.cs
//
// Reviewed: 2009.03.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.SolutionHierarchy;

namespace CSharpFactory.ProjectModel
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
    /// <param name="unit">Compilation unit instance</param>
    /// <param name="name">Name</param>
    // ----------------------------------------------------------------------------------------------
    public ReferencedCompilation(CompilationUnit unit, string name)
      : base(name)
    {
      CompilationUnit = unit;
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
    public CompilationUnit CompilationUnit { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the project provider referenced by this instance.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ProjectProviderBase ProjectProvider { get; private set; }
  }
}