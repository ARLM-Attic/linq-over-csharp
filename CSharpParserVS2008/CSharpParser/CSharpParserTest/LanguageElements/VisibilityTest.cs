using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class VisibilityTest : ParserTestBed
  {
    [TestMethod]
    public void InvalidAccessCombinationFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessCombination1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0107");
      Assert.AreEqual(parser.Errors[1].Code, "CS0107");
      Assert.AreEqual(parser.Errors[2].Code, "CS0107");
      Assert.AreEqual(parser.Errors[3].Code, "CS0107");
    }

    [TestMethod]
    public void InvalidAccessCombinationFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessCombination2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0107");
      Assert.AreEqual(parser.Errors[1].Code, "CS0107");
      Assert.AreEqual(parser.Errors[2].Code, "CS0107");
      Assert.AreEqual(parser.Errors[3].Code, "CS0107");
    }

    [TestMethod]
    public void InvalidTypeAccessorFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessModifier1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS1527");
      Assert.AreEqual(parser.Errors[1].Code, "CS1527");
      Assert.AreEqual(parser.Errors[2].Code, "CS1527");
      Assert.AreEqual(parser.Errors[3].Code, "CS1527");
    }

    [TestMethod]
    public void InvalidTypeAccessorFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessModifier2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS1527");
      Assert.AreEqual(parser.Errors[1].Code, "CS1527");
      Assert.AreEqual(parser.Errors[2].Code, "CS1527");
      Assert.AreEqual(parser.Errors[3].Code, "CS1527");
    }

    [TestMethod]
    public void InvalidTypeAccessorFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessModifier3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidTypeAccessorFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessModifier4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidTypeAccessorFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Visibility\InvalidAccessModifier5.cs");
      Assert.IsFalse(InvokeParser(parser));
    }
  }
}