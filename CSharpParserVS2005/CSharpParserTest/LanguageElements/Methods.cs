using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Methods : ParserTestBed
  {
    [TestMethod]
    public void InvalidMethodModifiersFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\MethodDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0112");
      Assert.AreEqual(parser.Errors[1].Code, "CS0112");
      Assert.AreEqual(parser.Errors[2].Code, "CS0113");
    }

    [TestMethod]
    public void InvalidMethodModifiersFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\MethodDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0112");
      Assert.AreEqual(parser.Errors[1].Code, "CS0503");
      Assert.AreEqual(parser.Errors[2].Code, "CS0502");
      Assert.AreEqual(parser.Errors[3].Code, "CS0238");
      Assert.AreEqual(parser.Errors[4].Code, "CS0180");
    }

    [TestMethod]
    public void InvalidMethodModifiersFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\MethodDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0621");
      Assert.AreEqual(parser.Errors[1].Code, "CS0621");
      Assert.AreEqual(parser.Errors[2].Code, "CS0621");
    }
  }
}