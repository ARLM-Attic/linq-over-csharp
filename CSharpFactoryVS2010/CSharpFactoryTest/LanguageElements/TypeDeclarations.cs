using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class TypeDeclarations : ParserTestBed
  {
    [TestMethod]
    public void NewOnNotnestedClassFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedStructFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"StructDeclaration\StructDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedDelegateFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"DelegateDeclaration\DelegateDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedEnumFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedInterfaceFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"InterfaceDeclaration\InterfaceDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void SealedOrStaticAbstractFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0418");
      Assert.AreEqual(parser.Errors[1].Code, "CS0418");
      Assert.AreEqual(parser.Errors[2].Code, "CS0418");
      Assert.AreEqual(parser.Errors[3].Code, "CS0418");
    }

    [TestMethod]
    public void SealedAndStaticClassFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0441");
      Assert.AreEqual(parser.Errors[1].Code, "CS0441");
    }

    [TestMethod]
    public void InvalidClassModifiersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidStructModifiersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"StructDeclaration\StructDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidInterfaceModifiersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"InterfaceDeclaration\InterfaceDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidDelegateModifiersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"DelegateDeclaration\DelegateDeclaration1.cs");
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
    public void InvalidEnumModifiersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void MemberWithEnclosingNameFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 9);
      Assert.AreEqual(parser.Errors[0].Code, "CS0542");
      Assert.AreEqual(parser.Errors[1].Code, "CS0542");
      Assert.AreEqual(parser.Errors[2].Code, "CS0542");
      Assert.AreEqual(parser.Errors[3].Code, "CS0542");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
      Assert.AreEqual(parser.Errors[5].Code, "CS0101");
      Assert.AreEqual(parser.Errors[6].Code, "CS0101");
      Assert.AreEqual(parser.Errors[7].Code, "CS0542");
      Assert.AreEqual(parser.Errors[8].Code, "CS0542");
    }

    [TestMethod]
    public void TypeParameterWithInvalidNameFails()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
      Assert.AreEqual(parser.Errors[1].Code, "CS0694");
      Assert.AreEqual(parser.Errors[2].Code, "CS0102");
    }
  }
}