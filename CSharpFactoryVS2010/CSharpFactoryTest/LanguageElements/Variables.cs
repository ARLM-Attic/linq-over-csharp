using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Variables : ParserTestBed
  {
    [TestMethod]
    public void DuplicateVariableFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0128");
      Assert.AreEqual(parser.Errors[1].Code, "CS0128");
    }

    [TestMethod]
    public void NoDuplicateVariableOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable2.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void DuplicateVariableFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
    }

    [TestMethod]
    public void DuplicateVariableFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
    }

    [TestMethod]
    public void DuplicateVariableFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0128");
      Assert.AreEqual(parser.Errors[1].Code, "CS0128");
    }

    [TestMethod]
    public void DuplicateVariableFails6()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
      Assert.AreEqual(parser.Errors[1].Code, "CS0136");
    }

    [TestMethod]
    public void DuplicateVariableFails7()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
      Assert.AreEqual(parser.Errors[1].Code, "CS0136");
    }

    [TestMethod]
    public void DuplicateVariableFails8()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable8.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
      Assert.AreEqual(parser.Errors[1].Code, "CS0136");
      Assert.AreEqual(parser.Errors[2].Code, "CS0136");
      Assert.AreEqual(parser.Errors[3].Code, "CS0136");
    }

    [TestMethod]
    public void DuplicateVariableFails9()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Variables\DuplicatedVariable9.cs");
      parser.AddAssemblyReference("System.Data");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0136");
      Assert.AreEqual(parser.Errors[1].Code, "CS0136");
      Assert.AreEqual(parser.Errors[2].Code, "CS0136");
      Assert.AreEqual(parser.Errors[3].Code, "CS0136");
    }
  }
}