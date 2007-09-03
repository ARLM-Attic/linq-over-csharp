using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class BaseTypes : ParserTestBed
  {
    [TestMethod]
    public void BaseTypeOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesOk1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void BaseTypeOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesOk2.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void BaseTypesFailWithTypeParameter()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0689");
      Assert.AreEqual(parser.Errors[1].Code, "CS0689");
    }

    [TestMethod]
    public void BaseTypesFailWithNamespace()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0118");
    }

    [TestMethod]
    public void BaseTypesFailWithSpecialClasses()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0644");
      Assert.AreEqual(parser.Errors[1].Code, "CS0644");
      Assert.AreEqual(parser.Errors[2].Code, "CS0644");
      Assert.AreEqual(parser.Errors[3].Code, "CS0644");
    }

    [TestMethod]
    public void BaseTypesFailWithSealedTypes()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0509");
      Assert.AreEqual(parser.Errors[1].Code, "CS0509");
    }

    [TestMethod]
    public void BaseTypesFailWithMultipleClasses()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS1721");
      Assert.AreEqual(parser.Errors[1].Code, "CS1721");
    }

    [TestMethod]
    public void BaseTypesFailWithWrongTypeArguments()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0246");
      Assert.AreEqual(parser.Errors[1].Code, "CS0246");
    }

    [TestMethod]
    public void BaseTypesFailWithStruct()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0509");
    }

    [TestMethod]
    public void StructFailsWithNonInterfaces()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail8.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0527");
      Assert.AreEqual(parser.Errors[1].Code, "CS0527");
    }

    [TestMethod]
    public void EnumFailsWithNonIntegrals()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail9.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1008");
    }

    [TestMethod]
    public void InterfaceFailsWithNonInterfaces()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"BaseTypes\BaseTypesFail10.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0527");
      Assert.AreEqual(parser.Errors[1].Code, "CS0527");
    }

  }
}