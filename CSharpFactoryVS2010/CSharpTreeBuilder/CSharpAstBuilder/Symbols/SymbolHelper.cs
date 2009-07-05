//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009.07.05. 18:43:14
//
// The template file is BlankSyntaxNodeVisitor.tt 
//

// disable warnings about missing XML comments
#pragma warning disable 1591 

using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Helper class for converting between symbol kind and values.
  /// </summary>
  // ================================================================================================
  public static class SymbolHelper
  {
    // ReSharper disable InconsistentNaming
    // --- Token ID --> Name resolution
    private static readonly Dictionary<int, string> _SymbolsById =
    // ReSharper restore InconsistentNaming
      new Dictionary<int, string>
      {
        { 6, "abstract" },
        { 7, "as" },
        { 8, "base" },
        { 9, "bool" },
        { 10, "break" },
        { 11, "byte" },
        { 12, "case" },
        { 13, "catch" },
        { 14, "char" },
        { 15, "checked" },
        { 16, "class" },
        { 17, "const" },
        { 18, "continue" },
        { 19, "decimal" },
        { 20, "default" },
        { 21, "delegate" },
        { 22, "do" },
        { 23, "double" },
        { 24, "else" },
        { 25, "enum" },
        { 26, "event" },
        { 27, "explicit" },
        { 28, "extern" },
        { 29, "false" },
        { 30, "finally" },
        { 31, "fixed" },
        { 32, "float" },
        { 33, "for" },
        { 34, "foreach" },
        { 35, "goto" },
        { 36, "if" },
        { 37, "implicit" },
        { 38, "in" },
        { 39, "int" },
        { 40, "interface" },
        { 41, "internal" },
        { 42, "is" },
        { 43, "lock" },
        { 44, "long" },
        { 45, "namespace" },
        { 46, "new" },
        { 47, "null" },
        { 48, "object" },
        { 49, "operator" },
        { 50, "out" },
        { 51, "override" },
        { 52, "params" },
        { 53, "private" },
        { 54, "protected" },
        { 55, "public" },
        { 56, "readonly" },
        { 57, "ref" },
        { 58, "return" },
        { 59, "sbyte" },
        { 60, "sealed" },
        { 61, "short" },
        { 62, "sizeof" },
        { 63, "stackalloc" },
        { 64, "static" },
        { 65, "string" },
        { 66, "struct" },
        { 67, "switch" },
        { 68, "this" },
        { 69, "throw" },
        { 70, "true" },
        { 71, "try" },
        { 72, "typeof" },
        { 73, "uint" },
        { 74, "ulong" },
        { 75, "unchecked" },
        { 76, "unsafe" },
        { 77, "ushort" },
        { 78, "using" },
        { 79, "virtual" },
        { 80, "void" },
        { 81, "volatile" },
        { 82, "while" },
        { 83, "&" },
        { 84, "&=" },
        { 85, "=" },
        { 86, ":" },
        { 87, "," },
        { 88, "--" },
        { 89, "/=" },
        { 90, "." },
        { 91, "::" },
        { 92, "==" },
        { 93, ">" },
        { 94, ">=" },
        { 95, "++" },
        { 96, "{" },
        { 97, "[" },
        { 98, "(" },
        { 99, "<<=" },
        { 100, "<" },
        { 101, "<<" },
        { 102, "-" },
        { 103, "-=" },
        { 104, "%=" },
        { 105, "!=" },
        { 106, "!" },
        { 107, "|=" },
        { 108, "+" },
        { 109, "+=" },
        { 110, "?" },
        { 111, "}" },
        { 112, "]" },
        { 113, ")" },
        { 114, ";" },
        { 115, "~" },
        { 116, "*" },
        { 117, "*=" },
        { 118, "^=" },
        { 119, "=>" },
        { 120, "??" },
        { 121, "||" },
        { 122, "&&" },
        { 123, "|" },
        { 124, "^" },
        { 125, "<=" },
        { 126, "/" },
        { 127, "%" },
        { 128, "->" },	
    };  

    // ReSharper disable InconsistentNaming
    // --- Name --> Token ID resolution
    private static readonly Dictionary<string, int> _SymbolsByName = new Dictionary<string, int>();
    // ReSharper restore InconsistentNaming

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets up the helper for token resolution
    /// </summary>
    // --------------------------------------------------------------------------------------------
    static SymbolHelper()
    {
      foreach (var pair in _SymbolsById)
      {
        _SymbolsByName.Add(pair.Value, pair.Key);
      }
    }
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the symbol id by its name.
    /// </summary>
    /// <param name="name">Symbol name.</param>
    /// <returns>ID of the symbol</returns>
    // --------------------------------------------------------------------------------------------
    public static int GetSymbolId(string name)
    {
      return _SymbolsByName[name];
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the symbol by ID.
    /// </summary>
    /// <param name="id">Symbol ID.</param>
    /// <returns>Name of the symbol.</returns>
    // --------------------------------------------------------------------------------------------
    public static string GetSymbolName(int id)
    {
      return _SymbolsById[id];
    }
  }
}

#pragma warning restore 1591
