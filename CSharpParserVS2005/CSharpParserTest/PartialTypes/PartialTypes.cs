using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class PartialTypes : ParserTestBed
  {
    [TestMethod]
    public void MultipleClassesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MultipleStructsFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MultipleInterfacesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MultipleEnumsFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MultipleDelegatesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MultipleTypesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
      Assert.AreEqual(parser.Errors[3].Code, "CS0101");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
    }

    [TestMethod]
    public void MixedPartialTypesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0260");
      Assert.AreEqual(parser.Errors[1].Code, "CS0260");
      Assert.AreEqual(parser.Errors[2].Code, "CS0261");
      Assert.AreEqual(parser.Errors[3].Code, "CS0260");
      Assert.AreEqual(parser.Errors[4].Code, "CS0261");
      Assert.AreEqual(parser.Errors[5].Code, "CS0260");
    }

    [TestMethod]
    public void MultipleClassesOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType8.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void MultipleStructsOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType9.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void MultipleInterfacesOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType10.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void MultipleDelegatesOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType11.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

  }
}