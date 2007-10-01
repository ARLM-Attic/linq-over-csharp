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

    [TestMethod]
    public void OverloadOutRefFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\MethodDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0663");
      Assert.AreEqual(parser.Errors[1].Code, "CS0663");
      Assert.AreEqual(parser.Errors[2].Code, "CS0663");
    }

    [TestMethod]
    public void InvalidCtorFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\CtorDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidCtorFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\CtorDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0663");
    }

    [TestMethod]
    public void InvalidCtorFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\CtorDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
      Assert.AreEqual(parser.Errors[1].Code, "CS0111");
    }

    [TestMethod]
    public void InvalidCtorFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\CtorDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0132");
    }

    [TestMethod]
    public void InvalidCtorFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\CtorDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0515");
      Assert.AreEqual(parser.Errors[1].Code, "CS0515");
      Assert.AreEqual(parser.Errors[2].Code, "CS0515");
      Assert.AreEqual(parser.Errors[3].Code, "CS0515");
      Assert.AreEqual(parser.Errors[4].Code, "CS0515");
    }

    [TestMethod]
    public void InvalidFinalizerFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\FinalizerDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 8);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
      Assert.AreEqual(parser.Errors[7].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidFinalizerFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\FinalizerDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
    }

    [TestMethod]
    public void ReservedNameFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\ReservedName1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
      Assert.AreEqual(parser.Errors[1].Code, "CS0111");
      Assert.AreEqual(parser.Errors[2].Code, "CS0111");
      Assert.AreEqual(parser.Errors[3].Code, "CS0111");
    }
  }
}