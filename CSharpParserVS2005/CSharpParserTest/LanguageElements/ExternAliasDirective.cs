using CSharpParser;
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
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveOK.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void ExternAliasSyntaxError()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasDirectiveFailed.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
    }

    [TestMethod]
    public void ExternAliasesCheckedOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile(@"ExternAliasDirective\ExternAliasesChecked.cs");
      Assert.IsTrue(InvokeParser(parser));
      ProjectFile file = parser.Files[0];
      Assert.AreEqual(file.ExternAliases.Count, 3);
      Assert.AreEqual(file.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(file.ExternAliases[1].Name, "Alias2");
      Assert.AreEqual(file.ExternAliases[2].Name, "Alias3");
      Namespace ns = file.Namespaces[0];
      Assert.AreEqual(ns.ExternAliases.Count, 2);
      Assert.AreEqual(ns.ExternAliases[0].Name, "Alias1");
      Assert.AreEqual(ns.ExternAliases[1].Name, "Alias2");
    }
  }
}
