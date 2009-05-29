// ================================================================================================
// ParserExtensions.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// This class defines extension methods for a CSharpProject
  /// </summary>
  // ================================================================================================
  internal static class ParserExtensions
  {
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified file to the project.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="fileName">Name of the file.</param>
    // --------------------------------------------------------------------------------------------
    public static void AddFile(this CSharpProject project, string fileName)
    {
      project.ProjectProvider.AddFile(fileName);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified assembly reference to the project.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="fileName">Name of the file.</param>
    // --------------------------------------------------------------------------------------------
    public static void AddAssemblyReference(this CSharpProject project, string fileName)
    {
      project.ProjectProvider.AddAssemblyReference(fileName);
    }
  }
}