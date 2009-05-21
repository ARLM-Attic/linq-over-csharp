// ================================================================================================
// CSharp9ProjectContentProvider.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents a project provider which reads out project content and settings from a
  /// Visual Studio 2008 project file.
  /// </summary>
  // ================================================================================================
  public class CSharp9ProjectContentProvider: ProjectProviderBase 
  {
    private const string MSBuildSchema = "http://schemas.microsoft.com/developer/msbuild/2003";

    private static readonly XName ItemGroupXName = XName.Get("ItemGroup", MSBuildSchema);
    private static readonly XName ReferenceXName = XName.Get("Reference", MSBuildSchema);
    private static readonly XName CompileXName = XName.Get("Compile", MSBuildSchema);
    private static readonly XName ProjectRefXName = XName.Get("ProjectReference", MSBuildSchema);

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
      var csProj = XDocument.Load(Name);
      if (csProj.Root == null) return;

      var itemGroup = csProj.Root.Elements(ItemGroupXName).Elements();
      foreach (var refElem in itemGroup.Where(r => r.Name.Equals(ReferenceXName)))
      {
        var includeAttr = refElem.Attribute("Include");
        if (includeAttr != null) AddAssemblyReference(includeAttr.Value);
      }

      foreach (var refElem in itemGroup.Where(r => r.Name.Equals(CompileXName)))
      {
        var includeAttr = refElem.Attribute("Include");
        if (includeAttr != null) AddFile(includeAttr.Value);
      }

      foreach (var refElem in itemGroup.Where(r => r.Name.Equals(ProjectRefXName)))
      {
        var includeAttr = refElem.Attribute("Include");
        if (includeAttr != null)
        {
          var referencedProject =
            new CSharp9ProjectContentProvider(Path.Combine(WorkingFolder, includeAttr.Value));
          AddProjectReference(referencedProject, Path.GetFileName(includeAttr.Value));
        }
      }
    }
  }
}