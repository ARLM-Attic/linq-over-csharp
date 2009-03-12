using CSharpFactory.ProjectContent;
using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  [TestClass]
  public class LargeTestProjects: ParserTestBed
  {
    const string CSharpParserFile = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactory\CSharpFactory.csproj";
    const string NUnitCoreInterfacesFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\LargeTestProjects\NUnit.Core.Interfaces";
    const string NUnitCoreFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\LargeTestProjects\NUnit.Core";
    const string CSLAFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\LargeTestProjects\CSLA";
    const string LinqSamplesFolder = @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\LargeTestProjects\SampleQueries";

    [TestMethod]
    public void NUnitCoreInterfacesIsOK()
    {
      CompilationUnit parser = new CompilationUnit(NUnitCoreInterfacesFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void NUnitCoreIsOK()
    {
      CompilationUnit parser = new CompilationUnit(NUnitCoreFolder, true);
      parser.AddAssemblyReference("NUnit.Core.Interfaces", AsmFolder);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void CSLAIsOK()
    {
      CompilationUnit parser = new CompilationUnit(CSLAFolder, true);
      parser.AddAssemblyReference("System.Configuration");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Design");
      parser.AddAssemblyReference("System.Drawing");
      parser.AddAssemblyReference("System.EnterpriseServices");
      parser.AddAssemblyReference("System.Runtime.Remoting");
      parser.AddAssemblyReference("System.Transactions");
      parser.AddAssemblyReference("System.Web");
      parser.AddAssemblyReference("System.Web.Services");
      parser.AddAssemblyReference("System.Windows.Forms");
      parser.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void CSharpParserIsOK()
    {
      CSharpProjectContent content = new CSharpProjectContent(CSharpParserFile);
      CompilationUnit parser = new CompilationUnit(content);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void LinqSampleAreOK()
    {
      CompilationUnit parser = new CompilationUnit(LinqSamplesFolder, true);
      parser.AddAssemblyReference("System.Core");
      parser.AddAssemblyReference("System.Windows.Forms");
      parser.AddAssemblyReference("System.Xml.Linq");
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}
