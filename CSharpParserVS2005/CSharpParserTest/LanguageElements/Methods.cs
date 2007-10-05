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

    [TestMethod]
    public void InvalidPropertyModifiersFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0112");
      Assert.AreEqual(parser.Errors[1].Code, "CS0112");
      Assert.AreEqual(parser.Errors[2].Code, "CS0113");
    }

    [TestMethod]
    public void InvalidPropertyModifiersFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0112");
      Assert.AreEqual(parser.Errors[1].Code, "CS0503");
      Assert.AreEqual(parser.Errors[2].Code, "CS0502");
      Assert.AreEqual(parser.Errors[3].Code, "CS0238");
      Assert.AreEqual(parser.Errors[4].Code, "CS0180");
    }

    [TestMethod]
    public void InvalidPropertyModifiersFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0621");
      Assert.AreEqual(parser.Errors[1].Code, "CS0621");
      Assert.AreEqual(parser.Errors[2].Code, "CS0621");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 18);
      for (int i = 0; i < 18; i++)
      {
        Assert.AreEqual(parser.Errors[i].Code, "CS0106");
      }
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0275");
      Assert.AreEqual(parser.Errors[1].Code, "CS0275");
      Assert.AreEqual(parser.Errors[2].Code, "CS0275");
      Assert.AreEqual(parser.Errors[3].Code, "CS0275");
      Assert.AreEqual(parser.Errors[4].Code, "CS0275");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0276");
      Assert.AreEqual(parser.Errors[1].Code, "CS0276");
      Assert.AreEqual(parser.Errors[2].Code, "CS0274");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0273");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration8.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0273");
      Assert.AreEqual(parser.Errors[1].Code, "CS0273");
      Assert.AreEqual(parser.Errors[2].Code, "CS0273");
      Assert.AreEqual(parser.Errors[3].Code, "CS0273");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails6()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration9.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0273");
      Assert.AreEqual(parser.Errors[1].Code, "CS0273");
      Assert.AreEqual(parser.Errors[2].Code, "CS0273");
      Assert.AreEqual(parser.Errors[3].Code, "CS0273");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails7()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration10.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0273");
      Assert.AreEqual(parser.Errors[1].Code, "CS0273");
    }

    [TestMethod]
    public void InvalidAccessorModifiersFails8()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration11.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0273");
      Assert.AreEqual(parser.Errors[1].Code, "CS0273");
      Assert.AreEqual(parser.Errors[2].Code, "CS0273");
      Assert.AreEqual(parser.Errors[3].Code, "CS0273");
      Assert.AreEqual(parser.Errors[4].Code, "CS0273");
    }

    [TestMethod]
    public void PropertyCannotReturnVoid()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\PropertyDeclaration12.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0547");
    }

    [TestMethod]
    public void InvalidIndexerModifiersFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\IndexerDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0113");
    }

    [TestMethod]
    public void InvalidIndexerModifiersFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\IndexerDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0503");
      Assert.AreEqual(parser.Errors[2].Code, "CS0502");
      Assert.AreEqual(parser.Errors[3].Code, "CS0238");
      Assert.AreEqual(parser.Errors[4].Code, "CS0180");
    }

    [TestMethod]
    public void InvalidIndexerModifiersFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\IndexerDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0621");
      Assert.AreEqual(parser.Errors[1].Code, "CS0621");
      Assert.AreEqual(parser.Errors[2].Code, "CS0621");
    }

    [TestMethod]
    public void InvalidIndexerModifiersFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\IndexerDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0631");
      Assert.AreEqual(parser.Errors[1].Code, "CS0631");
      Assert.AreEqual(parser.Errors[2].Code, "CS1551");
    }

    [TestMethod]
    public void IndexerCannotReturnVoid()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\IndexerDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0620");
    }

    [TestMethod]
    public void OperatorCannotReturnVoid()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\OperatorDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0590");
    }

    [TestMethod]
    public void OperatorDeclarationFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\OperatorDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 9);
      Assert.AreEqual(parser.Errors[0].Code, "CS0558");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0558");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0558");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0558");
      Assert.AreEqual(parser.Errors[7].Code, "CS0106");
      Assert.AreEqual(parser.Errors[8].Code, "CS0558");
    }

    [TestMethod]
    public void OperatorDeclarationFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\OperatorDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 9);
      Assert.AreEqual(parser.Errors[0].Code, "CS0558");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0558");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0558");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0558");
      Assert.AreEqual(parser.Errors[7].Code, "CS0106");
      Assert.AreEqual(parser.Errors[8].Code, "CS0558");
    }

    [TestMethod]
    public void OperatorDeclarationFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\OperatorDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0631");
      Assert.AreEqual(parser.Errors[1].Code, "CS0631");
      Assert.AreEqual(parser.Errors[2].Code, "CS0631");
    }

    [TestMethod]
    public void OperatorDeclarationFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Methods\OperatorDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS1535");
      Assert.AreEqual(parser.Errors[1].Code, "CS1534");
      Assert.AreEqual(parser.Errors[2].Code, "CS1534");
    }
  }
}