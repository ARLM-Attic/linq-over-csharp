// ================================================================================================
// QueryExpressions.cs
//
// Created: 2009.06.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class QueryExpressions : ParserTestBed
  {
    [TestMethod]
    public void QuerySampleIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SampleQueries\LinqSamples.cs");
      project.AddFile(@"SampleQueries\ObjectDumper.cs");
      project.AddFile(@"SampleQueries\SampleHarness.cs");
      project.AddAssemblyReference("System.Core");
      project.AddAssemblyReference("System.Windows.Forms");
      project.AddAssemblyReference("System.Xml.Linq");
      Assert.IsTrue(InvokeParser(project, true, false));
    }
  }
}