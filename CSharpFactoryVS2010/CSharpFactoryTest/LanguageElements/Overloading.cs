using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Overloading : ParserTestBed
  {
    [TestMethod]
    public void MethodOverloadOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Overloading\Overloading1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}