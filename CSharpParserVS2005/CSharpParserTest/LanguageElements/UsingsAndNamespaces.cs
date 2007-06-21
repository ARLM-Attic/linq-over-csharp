using CSharpParser;
using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class UsingsAndNamespaces: ParserTestBed
  {
    [TestMethod]
    public void UsingsAndNameSpacesAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      Assert.IsTrue(InvokeParser(parser));
      ProjectFile file = parser.Files[0];

      // --- Check using clauses in the file
      Assert.AreEqual(file.Usings.Count, 5);
      Assert.IsFalse(file.Usings[0].HasAlias);
      Assert.AreEqual(file.Usings[0].TypeUsed.FullName, "System");
      Assert.IsFalse(file.Usings[1].HasAlias);
      Assert.AreEqual(file.Usings[1].TypeUsed.FullName, "System.Collections.Generic");
      Assert.IsFalse(file.Usings[2].HasAlias);
      Assert.AreEqual(file.Usings[2].TypeUsed.FullName, "System.Text");
      Assert.IsTrue(file.Usings[3].HasAlias);
      Assert.AreEqual(file.Usings[3].Name, "AliasName");
      Assert.AreEqual(file.Usings[3].TypeUsed.FullName, "System.Text.Encoding");
      Assert.IsTrue(file.Usings[4].HasAlias);
      Assert.AreEqual(file.Usings[4].Name, "SecondAlias");
      Assert.AreEqual(file.Usings[4].TypeUsed.FullName, "Microsoft.Win32");

      // --- Check namespaces in the file
      Assert.AreEqual(file.Namespaces.Count, 3);
      Namespace ns1 = file.Namespaces[0];
      Assert.AreEqual(ns1.Name, "CSharpParserTest.TestFiles");
      Assert.AreEqual(ns1.Usings.Count, 3);
      Assert.AreEqual(ns1.NestedNamespaces.Count, 3);

      Namespace ns1a = ns1.NestedNamespaces[0];
      Assert.IsTrue(ns1a.HasParentNamespace);
      Assert.AreEqual(ns1a.ParentNamespace, ns1);
      Assert.AreEqual(ns1a.Name, "Level1");
      Assert.AreEqual(ns1a.Usings.Count, 1);
      Assert.AreEqual(ns1a.Usings[0].TypeUsed.FullName, "System.Xml");
      Assert.IsFalse(ns1a.Usings[0].HasAlias);
      Assert.IsTrue(ns1a.HasNestedNamespace);
      Assert.AreEqual(ns1a.NestedNamespaces.Count, 1);

      Namespace ns1b = ns1.NestedNamespaces[1];
      Assert.IsTrue(ns1b.HasParentNamespace);
      Assert.AreEqual(ns1b.ParentNamespace, ns1);
      Assert.AreEqual(ns1b.Name, "Level1.A");
      Assert.AreEqual(ns1b.Usings.Count, 1);
      Assert.AreEqual(ns1b.NestedNamespaces.Count, 1);
      Assert.AreEqual(ns1b.NestedNamespaces[0].FullName, "CSharpParserTest.TestFiles.Level1.A.Level2.A");

      Namespace ns2 = file.Namespaces[1];
      Assert.AreEqual(ns2.Name, "OtherNameSpace");
      Assert.AreEqual(ns2.Usings.Count, 0);
      Assert.AreEqual(ns2.NestedNamespaces.Count, 1);

      ns1 = file.Namespaces[2];
      Assert.AreEqual(ns1.Name, "CSharpParserTest.TestFiles");
      Assert.AreEqual(ns1.Usings.Count, 2);
      Assert.AreEqual(ns1.NestedNamespaces.Count, 1);

      // --- Check ProjectParser

      Assert.AreEqual(parser.DeclaredNamespaces.Count, 9);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles"].Count, 2);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles.Level1"].Count, 3);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles.Level1.Level2"].Count, 3);
    }
  }
}
