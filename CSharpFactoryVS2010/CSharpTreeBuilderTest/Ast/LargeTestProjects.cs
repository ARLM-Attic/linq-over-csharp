// ================================================================================================
// LargeTestProjects.cs
//
// Created: 2009.06.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CSharpParserTest
{
  [TestClass]
  public class LargeTestProjects: ParserTestBed
  {
    private readonly string CSharpParserFile = Path.Combine(WorkingFolder, @"..\..\CSharpTreeBuilder\CSharpTreeBuilder.csproj");
    private readonly string NUnitCoreInterfacesFolder = Path.Combine(WorkingFolder, @"LargeTestProjects\NUnit.Core.Interfaces");
    private readonly string NUnitCoreFolder = Path.Combine(WorkingFolder, @"LargeTestProjects\NUnit.Core");
    private readonly string CSLAFolder = Path.Combine(WorkingFolder, @"LargeTestProjects\CSLA");

    [TestMethod]
    public void NUnitCoreInterfacesIsOk()
    {
      var project = new CSharpProject(NUnitCoreInterfacesFolder, true);
      Assert.IsTrue(InvokeParser(project, true, false));
    }

    [TestMethod]
    public void NUnitCoreIsOk()
    {
      var project = new CSharpProject(NUnitCoreFolder, true);
      project.AddAssemblyReference("NUnit.Core.Interfaces");
      Assert.IsTrue(InvokeParser(project, true, false));
    }

    [TestMethod]
    public void CslaisOk()
    {
      var project = new CSharpProject(CSLAFolder, true);
      project.AddAssemblyReference("System.Configuration");
      project.AddAssemblyReference("System.Data");
      project.AddAssemblyReference("System.Design");
      project.AddAssemblyReference("System.Drawing");
      project.AddAssemblyReference("System.EnterpriseServices");
      project.AddAssemblyReference("System.Runtime.Remoting");
      project.AddAssemblyReference("System.Transactions");
      project.AddAssemblyReference("System.Web");
      project.AddAssemblyReference("System.Web.Services");
      project.AddAssemblyReference("System.Windows.Forms");
      project.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(project, true, false));
    }

    [TestMethod]
    public void CSharpParserIsOk()
    {
      var content = new CSharp9ProjectContentProvider(CSharpParserFile);
      var project = new CSharpProject(content);
      Assert.IsTrue(InvokeParser(project, true, false));
    }
  }
}
