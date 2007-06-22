using CSharpParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  [TestClass]
  public class LargeTestProjects: ParserTestBed
  {
    const string NUnitCoreInterfacesFolder = @"C:\Work\LINQ-CSF\CSharpParser\CSharpParserTest\LargeTestProjects\NUnit.Core.Interfaces";
    const string NUnitCoreFolder = @"C:\Work\LINQ-CSF\CSharpParser\CSharpParserTest\LargeTestProjects\NUnit.Core";
    const string CSLAFolder = @"C:\Work\LINQ-CSF\CSharpParser\CSharpParserTest\LargeTestProjects\CSLA";

    [TestMethod]
    public void NUnitCoreInterfacesIsOK()
    {
      CSharpProject parser = new CSharpProject(NUnitCoreInterfacesFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void NUnitCoreIsOK()
    {
      CSharpProject parser = new CSharpProject(NUnitCoreFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void CSLAIsOK()
    {
      CSharpProject parser = new CSharpProject(CSLAFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}
