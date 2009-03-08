using CSharpFactory.ProjectModel;
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

    [TestMethod]
    public void IncompatibleAccessModifiers1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType12.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0262");
      Assert.AreEqual(parser.Errors[1].Code, "CS0262");
      Assert.AreEqual(parser.Errors[2].Code, "CS0262");
    }

    [TestMethod]
    public void MixedPartialErrors1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType13.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0262");
      Assert.AreEqual(parser.Errors[1].Code, "CS0261");
      Assert.AreEqual(parser.Errors[2].Code, "CS0261");
    }

    [TestMethod]
    public void DifferentBaseClassesFail1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType14.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0263");
    }

    [TestMethod]
    public void DifferentBaseClassesFail2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType15.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS1721");
      Assert.AreEqual(parser.Errors[1].Code, "CS1721");
      Assert.AreEqual(parser.Errors[2].Code, "CS1721");
      Assert.AreEqual(parser.Errors[3].Code, "CS0263");
      Assert.AreEqual(parser.Errors[4].Code, "CS0263");
      Assert.AreEqual(parser.Errors[5].Code, "CS0263");
    }

    [TestMethod]
    public void PartialStaticModifierOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType16.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsStatic);
      cd = parser.DeclaredTypes["B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsStatic);
      cd = parser.DeclaredTypes["Master+A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsStatic);
      cd = parser.DeclaredTypes["Master+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsStatic);
    }

    [TestMethod]
    public void PartialAbstractModifierOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType17.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsAbstract);
      cd = parser.DeclaredTypes["B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsAbstract);
      cd = parser.DeclaredTypes["Master+A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsAbstract);
      cd = parser.DeclaredTypes["Master+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsAbstract);
    }

    [TestMethod]
    public void PartialSealedModifierOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType18.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsSealed);
      cd = parser.DeclaredTypes["B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsSealed);
      cd = parser.DeclaredTypes["Master+A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsFalse(cd.IsSealed);
      cd = parser.DeclaredTypes["Master+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsSealed);
    }

    [TestMethod]
    public void PartialAttributesOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType19.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Attributes.Count, 2);
      cd = parser.DeclaredTypes["B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Attributes.Count, 2);
      cd = parser.DeclaredTypes["Master+A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Attributes.Count, 1);
      cd = parser.DeclaredTypes["Master+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Attributes.Count, 2);
    }

    [TestMethod]
    public void PartialAccessibilityOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType20.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Visibility, Visibility.Public);
      cd = parser.DeclaredTypes["B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Visibility, Visibility.Internal);
      cd = parser.DeclaredTypes["Master+A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Visibility, Visibility.Private);
      cd = parser.DeclaredTypes["Master+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Visibility, Visibility.Internal);
    }

    [TestMethod]
    public void PartialInPartialTypeOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"PartialTypes\PartialType21.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["A"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsSealed);
      Assert.AreEqual(cd.PartCount, 2);
      cd = parser.DeclaredTypes["A+B"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.IsTrue(cd.IsStatic);
      Assert.AreEqual(cd.PartCount, 2);
      cd = parser.DeclaredTypes["A+B+C"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.IsTrue(cd.IsPartial);
      Assert.AreEqual(cd.Visibility, Visibility.Public);
      Assert.AreEqual(cd.PartCount, 3);
    }
  }
}