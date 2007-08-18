using CSharpParser;
using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  [TestClass]
  public class LargeTestProjects: ParserTestBed
  {
    const string NUnitCoreInterfacesFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\NUnit.Core.Interfaces";
    const string NUnitCoreFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\NUnit.Core";
    const string CSLAFolder = @"C:\Work\LINQOverCSharp\CSharpParserVS2005\CSharpParserTest\LargeTestProjects\CSLA";

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
  }
}
