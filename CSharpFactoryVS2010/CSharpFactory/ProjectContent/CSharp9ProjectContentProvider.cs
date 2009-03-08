// ================================================================================================
// CSharp9ProjectContentProvider.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;
using System.Xml;
using CSharpFactory.SolutionHierarchy;

namespace CSharpFactory.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a project provider which reads out project content and settings from a
  /// Visual Studio 2008 project file.
  /// </summary>
  // ================================================================================================
  public class CSharp9ProjectContentProvider: ProjectProviderBase 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharp9ProjectContentProvider"/> class.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharp9ProjectContentProvider(string fileName)
    {
      Name = fileName;
      WorkingFolder = Path.GetDirectoryName(fileName);
      CollectProjectItems();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the projekt information and fills up the compilation unit accordingly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CollectProjectItems()
    {
      // --- Open the .csproj file as an XML document
      var csProj = new XmlDocument();
      csProj.Load(Name);

      // --- Obtain the files from the project
      foreach (XmlNode node in csProj.DocumentElement.ChildNodes)
      {
        if (node.LocalName != "ItemGroup") continue;
        foreach (XmlNode subNode in node.ChildNodes)
        {
          if (subNode.LocalName == "Reference")
          {
            string file = subNode.Attributes["Include"].Value;
            AddAssemblyReference(file);
          }
          else if (subNode.LocalName == "Compile")
          {
            string file = subNode.Attributes["Include"].Value;
            AddFile(file);
          }
          else if (subNode.LocalName == "ProjectReference")
          {
            string file = subNode.Attributes["Include"].Value;
            var referencedProject =
              new CSharp9ProjectContentProvider(Path.Combine(WorkingFolder, file));
            AddProjectReference(referencedProject, Path.GetFileName(file));
          }
        }
      }
    }
  }
}