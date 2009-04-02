using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class GlobalAttributesTest : ParserTestBed
  {
    [TestMethod]
    public void GlobalAttributesAreOK()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(parser));
      var file = parser.SyntaxTree.SourceFileNodes[0];

      // --- Check global attributes in the file
      Assert.AreEqual(file.GlobalAttributes.Count, 5);
      Assert.AreEqual(file.GlobalAttributes[0].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[0].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[1].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[1].Attributes[0].Arguments.Count, 0);
      Assert.AreEqual(file.GlobalAttributes[2].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[2].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[3].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[3].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes.Count, 2);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes[1].Arguments.Count, 1);
    }
  }
}