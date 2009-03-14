// ================================================================================================
// UsingsAndNamespaces.cs
//
// Reviewed: 2009.03.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class UsingsAndNamespaces: ParserTestBed
  {
    [TestMethod]
    public void UsingsAndNameSpacesAreOK()
    {
      var parser = new CompilationUnit(WorkingFolder);
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
      Assert.AreEqual(ns1a.EnclosingNamespace, ns1);
      Assert.AreEqual(ns1a.Name, "Level1");
      Assert.AreEqual(ns1a.Usings.Count, 1);
      Assert.AreEqual(ns1a.Usings[0].Name, "System.Xml");
      Assert.IsFalse(ns1a.Usings[0].HasAlias);
      Assert.IsTrue(ns1a.HasNestedNamespace);
      Assert.AreEqual(ns1a.NestedNamespaces.Count, 1);

      NamespaceFragment ns1b = ns1.NestedNamespaces[1];
      Assert.IsTrue(ns1b.HasParentNamespace);
      Assert.AreEqual(ns1b.EnclosingNamespace, ns1);
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

    [TestMethod]
    public void UsingsAndNameSpacesSyntaxTreeIsOK()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"UsingsAndNamespaces\UsingsAndNamespacesOK.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(parser));

      // --- Check syntax tree
      var sn = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(sn.UsingNodes.Count, 3);
      Assert.AreEqual(sn.UsingWithAliasNodes.Count, 2);

      var typeName = sn.UsingNodes[0].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 1);
      var tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      Assert.IsFalse(tag is TypeTagContinuationNode);

      typeName = sn.UsingNodes[1].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 3);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      Assert.IsFalse(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Collections");
      Assert.IsTrue(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[2];
      Assert.AreEqual(tag.Identifier, "Generic");
      Assert.IsTrue(tag is TypeTagContinuationNode);

      typeName = sn.UsingNodes[2].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 2);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      Assert.IsFalse(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Text");
      Assert.IsTrue(tag is TypeTagContinuationNode);

      Assert.AreEqual(sn.UsingWithAliasNodes[0].Alias, "AliasName");
      typeName = sn.UsingWithAliasNodes[0].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 3);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "System");
      Assert.IsFalse(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Text");
      Assert.IsTrue(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[2];
      Assert.AreEqual(tag.Identifier, "Encoding");
      Assert.IsTrue(tag is TypeTagContinuationNode);

      Assert.AreEqual(sn.UsingWithAliasNodes[1].Alias, "SecondAlias");
      typeName = sn.UsingWithAliasNodes[1].TypeName;
      Assert.IsFalse(typeName.HasQualifier);
      Assert.AreEqual(typeName.TypeTags.Count, 2);
      tag = typeName.TypeTags[0];
      Assert.AreEqual(tag.Identifier, "Microsoft");
      Assert.IsFalse(tag is TypeTagContinuationNode);
      tag = typeName.TypeTags[1];
      Assert.AreEqual(tag.Identifier, "Win32");
      Assert.IsTrue(tag is TypeTagContinuationNode);
    }
  }
}
