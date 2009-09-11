using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the simple name resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SimpleNameResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0103: The name 'x' does not exist in the current context
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0103_SimpleNameUndefined()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\CS0103_SimpleNameUndefined.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0103");
      project.Warnings.Count.ShouldEqual(0);
    }
  }
}