using System;
using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class Comments : ParserTestBed
  {
    [TestMethod]
    public void SimpleLineCommentsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\SimpleLineComments.cs");
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Files[0].Comments.Count, 8);
    }

    [TestMethod]
    public void LanguageElementCommentsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\CompoundComments.cs");
      Assert.IsTrue(InvokeParser(parser));

      MultiCommentBlock ci = parser.Files[0].NestedNamespaces[0].Comment as MultiCommentBlock;
      Assert.IsNotNull(ci);
      Assert.AreEqual(ci.Comments[1].Text, " Namespace comment");

      ClassDeclaration cd = parser.DeclaredTypes[0] as ClassDeclaration;
      Assert.IsNotNull(cd);
      ci = cd.Comment as MultiCommentBlock;
      Assert.IsNotNull(ci);
      Assert.IsTrue(ci.Comments[0] is LineComment);
      Assert.IsTrue(ci.Comments[0].Text.StartsWith(" ========"));
      Assert.IsTrue(ci.Comments[1] is XmlCommentLine);
      Assert.IsTrue(ci.Comments[1].Text.StartsWith(" <summary>"));
      Assert.IsTrue(ci.Comments[2] is XmlCommentLine);
      Assert.IsTrue(ci.Comments[2].Text.StartsWith(" This class"));
      Assert.IsTrue(ci.Comments[3] is XmlCommentLine);
      Assert.IsTrue(ci.Comments[3].Text.StartsWith(" </summary>"));
      Assert.IsTrue(ci.Comments[4] is LineComment);
      Assert.IsTrue(ci.Comments[4].Text.StartsWith(" ========"));
      Console.WriteLine(parser.DeclaredTypes[0].DocumentationComment.Text);
      Console.WriteLine(parser.DeclaredTypes[0].DocumentationComment.IsWellFormedXml);
      foreach (MemberDeclaration member in parser.DeclaredTypes[0].Members)
      {
        Console.WriteLine(member.DocumentationComment.Text);
        Console.WriteLine("Summary: {0}", member.DocumentationComment.Summary.Text);
        Console.WriteLine("Summary XML: {0}", member.DocumentationComment.Summary.XmlText);
        Console.WriteLine("Remarks: {0}", member.DocumentationComment.Remarks.Text);
        Console.WriteLine("Remarks XML: {0}", member.DocumentationComment.Remarks.XmlText);
        Console.WriteLine(member.DocumentationComment.IsWellFormedXml);
      }
    }

    [TestMethod]
    public void MissingDocCommentWarns()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\MissingDocComments.cs");
      parser.XmlDocumentFile = "Documents.xml";
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Warnings.Count, 2);
      Assert.AreEqual(parser.Warnings[0].Code, "CS1591");
      Assert.AreEqual(parser.Warnings[1].Code, "CS1591");
    }

    [TestMethod]
    public void MisplacedDocCommentWarns()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\MisplacedDocComments.cs");
      parser.XmlDocumentFile = "Documents.xml";
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Warnings.Count, 1);
      Assert.AreEqual(parser.Warnings[0].Code, "CS1587");
    }

    [TestMethod]
    public void DocCommentParameterWarns()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\DocCommentWarning1.cs");
      parser.XmlDocumentFile = "Documents.xml";
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Warnings.Count, 6);
      Assert.AreEqual(parser.Warnings[0].Code, "CS1571");
      Assert.AreEqual(parser.Warnings[1].Code, "CS1571");
      Assert.AreEqual(parser.Warnings[2].Code, "CS1573");
      Assert.AreEqual(parser.Warnings[3].Code, "CS1572");
      Assert.AreEqual(parser.Warnings[4].Code, "CS1573");
      Assert.AreEqual(parser.Warnings[5].Code, "CS1573");
    }

    [TestMethod]
    public void BadlyFormedDocCommentWarns()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\BadlyFormedDocComment.cs");
      parser.XmlDocumentFile = "Documents.xml";
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Warnings.Count, 4);
      Assert.AreEqual(parser.Warnings[0].Code, "CS1570");
      Assert.AreEqual(parser.Warnings[1].Code, "CS1570");
      Assert.AreEqual(parser.Warnings[2].Code, "CS1570");
      Assert.AreEqual(parser.Warnings[3].Code, "CS1570");
    }

    [TestMethod]
    public void DocCommentTypeParamWarns()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"Comments\DocCommentWarning2.cs");
      parser.XmlDocumentFile = "Documents.xml";
      Assert.IsTrue(InvokeParser(parser));
      Assert.AreEqual(parser.Warnings.Count, 5);
      Assert.AreEqual(parser.Warnings[0].Code, "CS1712");
      Assert.AreEqual(parser.Warnings[1].Code, "CS1711");
      Assert.AreEqual(parser.Warnings[2].Code, "CS1710");
      Assert.AreEqual(parser.Warnings[3].Code, "CS1710");
      Assert.AreEqual(parser.Warnings[4].Code, "CS1711");
    }
  }
}