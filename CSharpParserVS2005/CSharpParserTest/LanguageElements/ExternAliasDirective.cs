using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class ExternAliasDirective: ParserTestBed
  {
    [TestMethod]
    public void ExternAliasDirectiveOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ExternAliasSyntaxError()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveFailed.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS1003");
      Assert.AreEqual(parser.Errors[1].Code, "CS1003");
      Assert.AreEqual(parser.Errors[2].Code, "SYNERR");
    }

    [TestMethod]
    public void ExternAliasesCheckedOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasesChecked.cs");
      Assert.IsTrue(InvokeParser(parser));
      SourceFile file = parser.Files[0];
      Assert.AreEqual(file.ExternAliases.Count, 3);
      Assert.AreEqual(file.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(file.ExternAliases[1].Name, "Alias2");
      Assert.AreEqual(file.ExternAliases[2].Name, "Alias3");
      NamespaceFragment ns = file.Namespaces[0];
      Assert.AreEqual(ns.ExternAliases.Count, 2);
      Assert.AreEqual(ns.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(ns.ExternAliases[1].Name, "Alias2");
    }
  }
}
