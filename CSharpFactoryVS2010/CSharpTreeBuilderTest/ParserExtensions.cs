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

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified assembly reference to the project, with an alias name.
    /// </summary>
    /// <param name="project">The project.</param>
    /// <param name="alias">The alias name.</param>
    /// <param name="fileName">Name of the file.</param>
    // --------------------------------------------------------------------------------------------
    public static void AddAliasedAssemblyReference(this CSharpProject project, string alias, string fileName)
    {
      var path = fileName.Substring(0, fileName.LastIndexOf('\\'));
      var fileNameWithoutPath = fileName.Remove(0, path.Length + 1);
      project.ProjectProvider.AddAliasedReference(alias, fileNameWithoutPath, path);
    }
  }
}