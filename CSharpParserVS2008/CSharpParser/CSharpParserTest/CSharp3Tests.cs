using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class CSharpTests : ParserTestBed
  {
    [TestMethod]
    public void AutoPropertiesAreOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\AutoProperty1.cs");
      Assert.IsTrue(InvokeParser(parser));
      ClassDeclaration cd = parser.DeclaredTypes["AutoProperty"] as ClassDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Properties.Count, 3);
      AccessorDeclaration getAcc = cd.Properties["StringProperty"].Getter;
      Assert.AreEqual(getAcc.Visibility, Visibility.Private);
      Assert.IsFalse(getAcc.HasBody);
    }

    [TestMethod]
    public void VarDeclarationsAreOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddAssemblyReference("System.Data");
      parser.AddFile(@"CSharp3\VarDeclaration1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void VarDeclarationsFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddAssemblyReference("System.Data");
      parser.AddFile(@"CSharp3\VarDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
    }

    [TestMethod]
    public void ImplicitArrayDeclarationsAreOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\ImplicitArrayDecl1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ObjectInitializersAreOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\NewOperator1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ObjectInitializersAreOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\NewOperator2.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ObjectInitializersAreOk3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\NewOperator3.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void AnonymousTypesAreOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\AnonymousType1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void AnonymousTypesAreOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\AnonymousType2.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void PartialMethodsAreOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\PartialMethod1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ExtensionMethodsAreOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"CSharp3\ExtensionMethod1.cs");
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}