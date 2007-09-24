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

    [TestMethod]
    public void DuplicateMethodFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateMethod.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
      Assert.AreEqual(parser.Errors[1].Code, "CS0111");
      Assert.AreEqual(parser.Errors[2].Code, "CS0111");
      Assert.AreEqual(parser.Errors[3].Code, "CS0111");
    }

    [TestMethod]
    public void DuplicateOperatorFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateOperator.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0111");
    }

    [TestMethod]
    public void DuplicateCastOperatorFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateCastOperator.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0557");
      Assert.AreEqual(parser.Errors[1].Code, "CS0557");
    }

    [TestMethod]
    public void DuplicateFieldFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\DuplicateField.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
      Assert.AreEqual(parser.Errors[1].Code, "CS0102");
    }

    [TestMethod]
    public void FieldDeclarationFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0681");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0678");
      Assert.AreEqual(parser.Errors[4].Code, "CS0678");
    }

    [TestMethod]
    public void FieldAccessibilityOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration2.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration4.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityOk3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration5.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

  }
}