using System;
using CSharpParser.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  /// <summary>
  /// Summary description for StringHelperTest
  /// </summary>
  [TestClass]
  public class StringHelperTest
  {
    [TestMethod]
    public void StringLiteralsAreOK()
    {
      string csharp = "Hello, world";
      string normal = "Hello, world";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "\\\\Hello, world";
      normal = "\\Hello, world";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "\\\\\\a\\b\\'\\\"\\0\\f\\n\\r\\t\\v";
      normal = "\\\a\b\'\"\0\f\n\r\t\v";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "Hello\\x1\\x12\\x123\\x1234";
      normal = "Hello\x1\x12\x123\x1234";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "Hello\\x1\\x12\\x123\\x123456";
      normal = "Hello\x1\x12\x123\x123456";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "\\u1234\\u2345\\u3456";
      normal = "\u1234\u2345\u3456";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
      csharp = "\\U00001234\\U00002345\\U00003456";
      normal = "\U00001234\U00002345\U00003456";
      Assert.AreEqual(StringHelper.StringFromCSharpLiteral(csharp), normal);
    }

    [TestMethod]
    public void StringLiteralExceptionsAreOK()
    {
      int count = 0;
      string csharp = "\\";
      try {StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }
      
      csharp = "\\q";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\x";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\u123";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }
      Assert.AreEqual(count, 4);

      csharp = "\\U123345";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\U00012345";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\U00002345";
      try { StringHelper.StringFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }
      Assert.AreEqual(count, 6);
    }

    [TestMethod]
    public void CharLiteralsAreOK()
    {
      string csharp = "\\x020";
      char normal = '\x020';
      Assert.AreEqual(StringHelper.CharFromCSharpLiteral(csharp), normal);
      csharp = "\\n";
      normal = '\n';
      Assert.AreEqual(StringHelper.CharFromCSharpLiteral(csharp), normal);
    }

    [TestMethod]
    public void CharLiteralExceptionsAreOK()
    {
      int count = 0;
      string csharp = "\\";
      try { StringHelper.CharFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\q";
      try { StringHelper.CharFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\x";
      try { StringHelper.CharFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      csharp = "\\x020ABC";
      try { StringHelper.CharFromCSharpLiteral(csharp); }
      catch (ArgumentException) { count++; }

      Assert.AreEqual(count, 4);
    }

    [TestMethod]
    public void VerbatimLiteralsAreOK()
    {
      string csharp = @"Hello, world";
      string normal = "Hello, world";
      Assert.AreEqual(StringHelper.StringFromVerbatimLiteral(csharp), normal);
      csharp = "\"\"Hello, world\"\"";
      normal = @"""Hello, world""";
      Assert.AreEqual(StringHelper.StringFromVerbatimLiteral(csharp), normal);
    }
  }
}
