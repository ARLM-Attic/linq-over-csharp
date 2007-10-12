using System;
using System.IO;
using System.Xml;
using CSharpParser.ProjectModel;

namespace CSharpParser.ProjectContent
{
  // ==================================================================================
  /// <summary>
  /// This type creates project content from files within a particular folder
  /// and its subfolder.
  /// </summary>
  /// <remarks>
  /// This project content manager type does not add referenced assemblies to the 
  /// project.
  /// </remarks>
  // ==================================================================================
  public class CSharpProjectContent: IProjectContentProvider
  {
    #region Private fields

    private readonly string _WorkingFolder;
    private readonly string _ProjectFile;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the content provider.
    /// </summary>
    /// <param name="projectFile">Name of the project file.</param>
    // --------------------------------------------------------------------------------
    public CSharpProjectContent(string projectFile)
    {
      _ProjectFile = projectFile;
      _WorkingFolder = Path.GetDirectoryName(projectFile);
    }

    #endregion

    #region Public Properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the working folder of the project.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string WorkingFolder
    {
      get { return _WorkingFolder; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the projekt information and fills up the compilation unit accordingly.
    /// </summary>
    /// <param name="compilationUnit">CompilationUnit to fill up.</param>
    // --------------------------------------------------------------------------------
    public void CollectProjectItems(CompilationUnit compilationUnit)
    {
      // --- Open the .csproj file as an XML document
      XmlDocument csProj = new XmlDocument();
      csProj.Load(_ProjectFile);

      // --- Obtain the files from the project
      foreach (XmlNode node in csProj.DocumentElement.ChildNodes)
      {
        if (node.LocalName != "ItemGroup") continue;
        foreach (XmlNode subNode in node.ChildNodes)
        {
          if (subNode.LocalName == "Reference")
          {
            string file = subNode.Attributes["Include"].Value;
            compilationUnit.AddAssemblyReference(file);
          }
          else if (subNode.LocalName == "Compile")
          {
            string file = subNode.Attributes["Include"].Value;
            compilationUnit.AddFile(file);
          }
        }
      }
    }

    #endregion
  }
}