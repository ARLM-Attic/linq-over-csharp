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
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(parser));
      SourceFile file = parser.Files[0];

      // --- Check using clauses in the file
      Assert.AreEqual(file.Usings.Count, 5);
      Assert.IsFalse(file.Usings[0].HasAlias);
      Assert.AreEqual(file.Usings[0].Name, "System");
      Assert.IsFalse(file.Usings[1].HasAlias);
      Assert.AreEqual(file.Usings[1].Name, "System.Collections.Generic");
      Assert.IsFalse(file.Usings[2].HasAlias);
      Assert.AreEqual(file.Usings[2].Name, "System.Text");
      Assert.IsTrue(file.Usings[3].HasAlias);
      Assert.AreEqual(file.Usings[3].Name, "AliasName");
      Assert.AreEqual(file.Usings[3].ReferencedName.FullName, "System.Text.Encoding");
      Assert.IsTrue(file.Usings[4].HasAlias);
      Assert.AreEqual(file.Usings[4].Name, "SecondAlias");
      Assert.AreEqual(file.Usings[4].ReferencedName.FullName, "Microsoft.Win32");

      // --- Check namespaces in the file
      Assert.AreEqual(file.NestedNamespaces.Count, 3);
      NamespaceFragment ns1 = file.NestedNamespaces[0];
      Assert.AreEqual(ns1.Name, "CSharpParserTest.TestFiles");
      Assert.AreEqual(ns1.Usings.Count, 3);
      Assert.AreEqual(ns1.NestedNamespaces.Count, 3);

      NamespaceFragment ns1a = ns1.NestedNamespaces[0];
      Assert.IsTrue(ns1a.HasParentNamespace);
      Assert.AreEqual(ns1a.ParentNamespace, ns1);
      Assert.AreEqual(ns1a.Name, "Level1");
      Assert.AreEqual(ns1a.Usings.Count, 1);
      Assert.AreEqual(ns1a.Usings[0].Name, "System.Xml");
      Assert.IsFalse(ns1a.Usings[0].HasAlias);
      Assert.IsTrue(ns1a.HasNestedNamespace);
      Assert.AreEqual(ns1a.NestedNamespaces.Count, 1);

      NamespaceFragment ns1b = ns1.NestedNamespaces[1];
      Assert.IsTrue(ns1b.HasParentNamespace);
      Assert.AreEqual(ns1b.ParentNamespace, ns1);
      Assert.AreEqual(ns1b.Name, "Level1.A");
      Assert.AreEqual(ns1b.Usings.Count, 1);
      Assert.AreEqual(ns1b.NestedNamespaces.Count, 1);
      Assert.AreEqual(ns1b.NestedNamespaces[0].FullName, "CSharpParserTest.TestFiles.Level1.A.Level2.A");

      NamespaceFragment ns2 = file.NestedNamespaces[1];
      Assert.AreEqual(ns2.Name, "OtherNameSpace");
      Assert.AreEqual(ns2.Usings.Count, 0);
      Assert.AreEqual(ns2.NestedNamespaces.Count, 1);

      ns1 = file.NestedNamespaces[2];
      Assert.AreEqual(ns1.Name, "CSharpParserTest.TestFiles");
      Assert.AreEqual(ns1.Usings.Count, 2);
      Assert.AreEqual(ns1.NestedNamespaces.Count, 1);

      // --- Check ProjectParser

      Assert.AreEqual(parser.DeclaredNamespaces.Count, 9);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles"].Fragments.Count, 2);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles.Level1"].Fragments.Count, 3);
      Assert.AreEqual(parser.DeclaredNamespaces["CSharpParserTest.TestFiles.Level1.Level2"].Fragments.Count, 3);
    }
  }
}
