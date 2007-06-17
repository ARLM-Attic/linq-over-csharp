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
      ProjectParser parser = new ProjectParser(NUnitCoreInterfacesFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void NUnitCoreIsOK()
    {
      ProjectParser parser = new ProjectParser(NUnitCoreFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void CSLAIsOK()
    {
      ProjectParser parser = new ProjectParser(CSLAFolder, true);
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}
