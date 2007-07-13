using System.IO;
using System.Text;
using CSharpParser.ParserFiles.PPExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class PreprocessorTests
  {
    [TestMethod]
    public void BasicPreprocessorTestIsOK()
    {
      Assert.IsTrue(Parse("true"));
      Assert.IsTrue(Parse("false"));
      Assert.IsTrue(Parse("SymbolA"));
      Assert.IsTrue(Parse("!!(!(true))"));
      Assert.IsTrue(Parse("(SymbolA || SymbolB)"));
      Assert.IsTrue(Parse("(SymbolA || SymbolB) != (SymbolC && SymbolD)"));
    }

    [TestMethod]
    public void PreprocessorFails()
    {
      Assert.IsFalse(Parse("true)"));
      Assert.IsFalse(Parse("(false"));
      Assert.IsFalse(Parse("(SymbolA"));
      Assert.IsFalse(Parse("SymbolA)"));
      Assert.IsFalse(Parse("efefe && erd || !sdsd"));

#if  eee
#endif
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses all the files within the project.
    /// </summary>
    /// <returns>
    /// Number of errors.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool Parse(string expression)
    {
      MemoryStream memStream = new MemoryStream(new UTF8Encoding().GetBytes(expression));
      PPScanner scanner = new PPScanner(memStream);
      CSharpPPExprSyntaxParser parser = new CSharpPPExprSyntaxParser(scanner);
      parser.Parse();
      return !parser.ErrorFound;
    }
  }
}