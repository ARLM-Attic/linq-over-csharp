using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpFactory.ParserFiles.PPExpressions;
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
      PPExpression result;
      Assert.IsTrue(Parse("true", out result));
      Assert.IsTrue(result is PPTrueLiteral);
      Assert.IsTrue(Parse("false", out result));
      Assert.IsTrue(result is PPFalseLiteral);
      Assert.IsTrue(Parse("a", out result));
      Assert.IsTrue(result is PPSymbol);
      Assert.AreEqual((result as PPSymbol).Symbol, "a");

      Assert.IsTrue(Parse("!!(!(true))", out result));
      Assert.IsTrue(result is PPNotOperator);
      Assert.IsTrue((result as PPNotOperator).Operand is PPNotOperator);
      Assert.IsTrue(((result as PPNotOperator).Operand as PPNotOperator).Operand is PPNotOperator) ;
      Assert.IsFalse((result.Evaluate(new List<string>())));

      Assert.IsTrue(Parse("(a || b)", out result));
      Assert.IsTrue(result is PPOrOperator);
      Assert.IsTrue((result as PPOrOperator).LeftOperand is PPSymbol);
      Assert.AreEqual(((result as PPOrOperator).LeftOperand as PPSymbol).Symbol, "a");
      Assert.IsTrue((result as PPOrOperator).RightOperand is PPSymbol);
      Assert.AreEqual(((result as PPOrOperator).RightOperand as PPSymbol).Symbol, "b");
      Assert.IsFalse((result.Evaluate(new List<string>())));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "b" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a", "b" }))));

      Assert.IsTrue(Parse("(a || b) != (c && d)", out result));
      Assert.IsTrue(result is PPNotEqualOperator);
      Assert.IsTrue((result as PPNotEqualOperator).LeftOperand is PPOrOperator);
      Assert.IsTrue((result as PPNotEqualOperator).RightOperand is PPAndOperator);
      Assert.IsTrue(((result as PPNotEqualOperator).LeftOperand as PPOrOperator).LeftOperand is PPSymbol);
      Assert.IsTrue(((result as PPNotEqualOperator).LeftOperand as PPOrOperator).RightOperand is PPSymbol);
      Assert.IsTrue(((result as PPNotEqualOperator).RightOperand as PPAndOperator).LeftOperand is PPSymbol);
      Assert.IsTrue(((result as PPNotEqualOperator).RightOperand as PPAndOperator).RightOperand is PPSymbol);
      Assert.IsFalse((result.Evaluate(new List<string>())));
      Assert.IsFalse((result.Evaluate(new List<string>(new string[] { "X" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "b" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "c", "d" }))));
      
      Assert.IsTrue(Parse("a || b || c", out result));
      Assert.IsTrue(result is PPOrOperator);
      Assert.IsTrue((result as PPOrOperator).LeftOperand is PPOrOperator);
      Assert.IsTrue(((result as PPOrOperator).LeftOperand as PPOrOperator).LeftOperand is PPSymbol);
      Assert.IsTrue(((result as PPOrOperator).LeftOperand as PPOrOperator).RightOperand is PPSymbol);
      Assert.IsTrue((result as PPOrOperator).RightOperand is PPSymbol);
      Assert.IsFalse((result.Evaluate(new List<string>())));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "b" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "c" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a", "c" }))));

      Assert.IsTrue(Parse("a || b && c == d", out result));
      Assert.IsTrue(result is PPEqualOperator);
      Assert.IsTrue((result as PPEqualOperator).LeftOperand is PPAndOperator);
      Assert.IsTrue((result as PPEqualOperator).RightOperand is PPSymbol);
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "a" }))));
      Assert.IsTrue((result.Evaluate(new List<string>(new string[] { "b", "c", "d" }))));

      Assert.IsTrue(Parse("(a || b) && c && (d == e)", out result));
    }

    [TestMethod]
    public void PreprocessorFails()
    {
      PPExpression result;
      Assert.IsFalse(Parse("true)", out result));
      Assert.IsFalse(Parse("(false", out result));
      Assert.IsFalse(Parse("(SymbolA", out result));
      Assert.IsFalse(Parse("SymbolA)", out result));
      Assert.IsFalse(Parse("efefe && erd || !sdsd", out result));
      Assert.IsFalse(Parse("a || b && c || d", out result));
      Assert.IsFalse(Parse("(a || b) && c || (d == e)", out result));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses all the files within the project.
    /// </summary>
    /// <returns>
    /// Number of errors.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool Parse(string expression, out PPExpression result)
    {
      MemoryStream memStream = new MemoryStream(new UTF8Encoding().GetBytes(expression));
      PPScanner scanner = new PPScanner(memStream);
      CSharpPPExprSyntaxParser parser = new CSharpPPExprSyntaxParser(scanner);
      parser.Parse();
      result = parser.Expression;
      return !parser.ErrorFound;
    }
  }
}