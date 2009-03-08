using CSharpFactory.ProjectModel;
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
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityOk3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration5.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityOk4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration6.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
      Assert.AreEqual(parser.Errors[3].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration8.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration9.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration10.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityOk5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration11.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityFails6()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration12.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
      Assert.AreEqual(parser.Errors[3].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityFails7()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration13.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0060");
      Assert.AreEqual(parser.Errors[1].Code, "CS0060");
      Assert.AreEqual(parser.Errors[2].Code, "CS0060");
    }

    [TestMethod]
    public void FieldAccessibilityOk6()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration14.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityOk7()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration15.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void FieldAccessibilityOk8()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration16.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void InvalidVolatileFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration17.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0677");
      Assert.AreEqual(parser.Errors[1].Code, "CS0677");
      Assert.AreEqual(parser.Errors[2].Code, "CS0677");
      Assert.AreEqual(parser.Errors[3].Code, "CS0677");
      Assert.AreEqual(parser.Errors[4].Code, "CS0677");
    }

    [TestMethod]
    public void InvalidVolatileFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration18.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0677");
      Assert.AreEqual(parser.Errors[1].Code, "CS0677");
      Assert.AreEqual(parser.Errors[2].Code, "CS0677");
    }

    [TestMethod]
    public void StructFieldInitializerFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration19.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0573");
      Assert.AreEqual(parser.Errors[1].Code, "CS0573");
    }

    [TestMethod]
    public void StaticFieldTypeFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration20.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0723");
    }

    [TestMethod]
    public void FieldCannotBeVoid()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\FieldDeclaration21.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0670");
    }

    [TestMethod]
    public void InvalidConstModifierFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\ConstDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0681");
    }

    [TestMethod]
    public void InvalidConstTypesFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Members\ConstDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0283");
    }
  }
}