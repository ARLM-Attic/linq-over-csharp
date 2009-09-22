using System.Reflection;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a C# program: a set of sources (a project) or an assembly.
  /// </summary>
  /// <remarks>
  /// C# programs consist of one or more source files (a project), known formally as compilation units.
  /// When C# programs are compiled, they are physically packaged into assemblies
  /// </remarks>
  // ================================================================================================
  public sealed class Program
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Program"/> type.
    /// </summary>
    /// <param name="sourceProject">The project containing the source of the program.</param>
    /// <param name="targetAssemblyName">The name of the assembly created from the program.</param>
    // ----------------------------------------------------------------------------------------------
    public Program(CSharpProject sourceProject, AssemblyName targetAssemblyName)
    {
      SourceProject = sourceProject;
      TargetAssemblyName = targetAssemblyName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the project containing the source of the program.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CSharpProject SourceProject { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the assembly created from the program.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AssemblyName TargetAssemblyName { get; private set; }
  }
}
