using System;
using CSharpParser;
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
      foreach(CommentInfo c in parser.Files[0].Comments)
      {
        Console.WriteLine("type: {5}, sl:{0} sc:{1}, el:{2}, ec{3}, '{4}'",
                          c.StartLine, c.StartColumn, c.EndLine, c.EndColumn, c.Text,
                          c.GetType().Name);
      }
    }
  }
}