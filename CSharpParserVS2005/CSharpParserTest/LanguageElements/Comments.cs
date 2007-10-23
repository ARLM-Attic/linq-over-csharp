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
        Console.WriteLine(member.DocumentationComment.IsWellFormedXml);
      }
    }
  }
}