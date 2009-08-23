// ================================================================================================
// EmptyContentProvider.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a project provider which is bound to a specified working directory but 
  /// containing no source files initially.
  /// </summary>
  // ================================================================================================
  public class EmptyContentProvider: ProjectProviderBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharp9ProjectContentProvider"/> class.
    /// </summary>
    /// <param name="path">Name of the file.</param>
    // ----------------------------------------------------------------------------------------------
    public EmptyContentProvider(string path)
    {
      Name = path;
      WorkingFolder = path;
      //AddAssemblyReference("mscorlib");
      //AddAssemblyReference("System");
    }
  }
}