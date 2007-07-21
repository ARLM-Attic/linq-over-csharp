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
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void CSLAIsOK()
    {
      CompilationUnit parser = new CompilationUnit(CSLAFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}
