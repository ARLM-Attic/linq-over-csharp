using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Members : ParserTestBed
  {
    [TestMethod]
    public void DuplicateConstFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateConst.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
      Assert.AreEqual(parser.Errors[1].Code, "CS0102");
    }

    [TestMethod]
    public void DuplicateCtorFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateCtor.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
      Assert.AreEqual(parser.Errors[1].Code, "CS0111");
    }

    [TestMethod]
    public void DuplicateEventPropFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateEventProp.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
    }

    [TestMethod]
    public void DuplicatePropFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateProp.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
      Assert.AreEqual(parser.Errors[1].Code, "CS0102");
    }

    [TestMethod]
    public void DuplicateIndexerFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateIndexer.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
      Assert.AreEqual(parser.Errors[1].Code, "CS0111");
    }

    [TestMethod]
    public void DuplicateFinalizerFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateFinalizer.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
    }
  }
}