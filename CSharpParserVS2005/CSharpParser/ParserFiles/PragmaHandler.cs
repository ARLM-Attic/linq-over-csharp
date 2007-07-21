using System;
using System.Collections.Generic;
using CSharpParser.ParserFiles.PPExpressions;
using CSharpParser.ProjectModel;

namespace CSharpParser.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This class describes the processing state of a #if..#elif..#else..#endif
  /// pragma block.
  /// </summary>
  // ==================================================================================
  internal sealed class IfPragmaState
  {
    /// <summary>
    /// Token holding the error/success position information.
    /// </summary>
    public Token PragmaPosition;

    /// <summary>
    /// Flag indicating that the "true" condition block is already found or not.
    /// </summary>
    public bool TrueBlockFound;

    /// <summary>
    /// Flag indicating that the #else block is already found or not.
    /// </summary>
    public bool ElseBlockFound;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new #if pragma state using the specified token.
    /// </summary>
    /// <param name="pragmaPosition"></param>
    // --------------------------------------------------------------------------------
    public IfPragmaState(Token pragmaPosition)
    {
      PragmaPosition = pragmaPosition;
      TrueBlockFound = false;
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class encapsulates the functionality required to handle pragmas.
  /// </summary>
  // ==================================================================================
  internal sealed class PragmaHandler
  {
    #region Private fields

    private readonly CSharpSyntaxParser _Parser;
    private readonly Stack<IfPragmaState> _IfPragmaStack = new Stack<IfPragmaState>();
    private readonly Stack<RegionInfo> _RegionStack = new Stack<RegionInfo>();

    /// <summary>
    /// Flag indicating if the first real token occurred in the stream.
    /// </summary>
    /// <remarks>
    /// Needed to check for #define and #undef directives occure before any real
    /// tokens as stated in S9.5.3.
    /// </remarks>
    private bool _FirstRealTokenOccurred;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of pragma handler that works together with the
    /// specified CSharpSyntaxParser instance.
    /// </summary>
    /// <param name="parser">Parser instance.</param>
    // --------------------------------------------------------------------------------
    public PragmaHandler(CSharpSyntaxParser parser)
    {
      _Parser = parser;
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of open regions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int OpenRegionCount
    {
      get { return _RegionStack.Count; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that the first real token occurred in the source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SignRealToken()
    {
      _FirstRealTokenOccurred = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="pragma">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalDirective(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (_FirstRealTokenOccurred)
      {
        DefineAndUndefError();
        return;
      }
      string symbol = GetPreprocessorSymbol(pragma.val);
      _Parser.CompilationUnit.AddConditionalDirective(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="pragma">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void RemoveConditionalDirective(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (_FirstRealTokenOccurred)
      {
        DefineAndUndefError();
        return;
      }
      _Parser.CompilationUnit.RemoveConditionalDirective(GetPreprocessorSymbol(pragma.val));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #if pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void IfPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;

      // ---Evaluate the pragma, if failed we skip it.
      PPEvaluationStatus evalStatus = EvaluatePragmaCondition(pragma);
      if (evalStatus == PPEvaluationStatus.Failed) return;

      // --- Prepare the pragma status
      IfPragmaState newIfPragma = new IfPragmaState(pragma);
      newIfPragma.TrueBlockFound = evalStatus == PPEvaluationStatus.True;
      _IfPragmaStack.Push(newIfPragma);
      
      // --- Skip the whole #if block, if evaluates to false.
      if (!newIfPragma.TrueBlockFound)
      {
        SkipFalseBlock();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #elif pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void ElifPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      IfPragmaState status = _IfPragmaStack.Peek();
      if (!status.TrueBlockFound)
      {
        // --- Evaluate the #elif condition 
        PPEvaluationStatus evalStatus = EvaluatePragmaCondition(pragma);
        status.TrueBlockFound = evalStatus == PPEvaluationStatus.True;

        // --- If this is the true block, we do not skip it.
        if (status.TrueBlockFound) return;
      }
      SkipFalseBlock();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #else pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void ElsePragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      IfPragmaState status = _IfPragmaStack.Peek();
      status.ElseBlockFound = true;
      if (status.TrueBlockFound)
      {
        SkipFalseBlock();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #endif pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void EndifPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      _IfPragmaStack.Pop();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #line pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void LinePragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      string linePragmaText = GetPreprocessorText(pragma.val);
      if (linePragmaText.Length > 0)
      {
        string lineSymbol;
        string fileSymbol;

        int endLine;
        lineSymbol = GetLiteral(linePragmaText, 0, out endLine);
        int endFile;
        fileSymbol = GetLiteral(linePragmaText, endLine, out endFile);

        if (lineSymbol == "default" && string.IsNullOrEmpty(fileSymbol))
        {
          _Parser.CompilationUnit.ErrorHandler.ResetRedirection();
          return;
        }
        if (HasDigitOnly(lineSymbol))
        {
          if (String.IsNullOrEmpty(fileSymbol))
          {
            _Parser.CompilationUnit.ErrorHandler.Redirect(pragma.line, Int32.Parse(lineSymbol), null);
            return;            
          }
          else if (IsQuotedFileName(fileSymbol))
          {
            _Parser.CompilationUnit.ErrorHandler.Redirect(pragma.line, Int32.Parse(lineSymbol), 
              fileSymbol.Substring(1, fileSymbol.Length - 2));
            return;
          }
        }
      }
      _Parser.CompilationUnit.ErrorHandler.
        Error("CS1576", pragma, "The line number specified for #line directive is missing or invalid.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #error pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void ErrorPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      _Parser.CompilationUnit.ErrorHandler.
        Error("CS1029", pragma, "#error: " + GetPreprocessorText(pragma.val));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #warning pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void WarningPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      _Parser.CompilationUnit.ErrorHandler.
        Warning("CS1030", pragma, "#warning: " + GetPreprocessorText(pragma.val));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #pragma pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void PragmaPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #region pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void RegionPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      RegionInfo regInfo = new RegionInfo();
      regInfo.StartLine = pragma.line;
      regInfo.StartColumn = pragma.col;
      regInfo.StartText = GetPreprocessorText(pragma.val);
      _RegionStack.Push(regInfo);
      _Parser.File.AddRegion(regInfo);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #endregion pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void EndregionPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (_RegionStack.Count == 0)
      {
        ReportUnexpectedDirective();
        return;
      }
      RegionInfo regInfo = _RegionStack.Pop();
      regInfo.EndLine = pragma.line;
      regInfo.EndColumn = pragma.col;
      regInfo.EndText = GetPreprocessorText(pragma.val);
    }

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the pragma is expected in the current #if context or not.
    /// </summary>
    /// <param name="symbol">Token representing the pragma.</param>
    /// <returns>True, if the pragma is unexpected; otherwise, false.</returns>
    // --------------------------------------------------------------------------------
    private bool CheckUnexpectedPragma(Token symbol)
    {
      if (_IfPragmaStack.Count > 0)
      {
        IfPragmaState status = _IfPragmaStack.Peek();
        if (symbol.kind == CSharpSyntaxParser.ppEndifKind || !status.ElseBlockFound)
          return false;
      }
      ReportUnexpectedDirective();
      return true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the pragma is the first token in the current line.
    /// </summary>
    /// <param name="symbol">Token representing the pragma.</param>
    /// <returns>True, if the pragma is the first token; otherwise, false.</returns>
    // --------------------------------------------------------------------------------
    private bool CheckPragmaIsFirstInLine(Token symbol)
    {
      int oldPos = _Parser.Scanner.Buffer.Pos;
      bool wsOnly = true;
      for (int i = symbol.col - 1; i >= 1; i--)
      {
        _Parser.Scanner.Buffer.Pos = symbol.pos - i;
        int ch = _Parser.Scanner.Buffer.Peek();
        wsOnly &= (ch == ' ' || (ch >= 9 && ch <= 13));
      }
      _Parser.Scanner.Buffer.Pos = oldPos;
      if (wsOnly) return true;
      _Parser.Error("CS1040", symbol, "Preprocessor directives must appear as the first non-whitespace character on a line.");
      return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text after the preprocessor directive.
    /// </summary>
    /// <param name="symbol">Symbol representing the preprocessor directive</param>
    /// <returns>
    /// Preprocessor tag
    /// </returns>
    /// <remarks>
    /// input:        "#" {ws} directive ws {ws} {not-newline} {newline}
    /// valid input:  "#" {ws} directive ws {ws} {non-ws} {ws} {newline}
    /// output:       {non-ws}
    /// </remarks>
    // --------------------------------------------------------------------------------
    private string GetPreprocessorText(string symbol)
    {
      int start = 1;
      int end;

      // skip {ws}
      start = EndOf(symbol, start, true);
      // skip directive  
      start = EndOf(symbol, start, false);
      // skip ws {ws}
      start = EndOf(symbol, start, true);
      // search end of symbol
      end = symbol.Length - 1;
      if (end <= start) return String.Empty;
      return symbol.Substring(start, end - start);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first symbol after the preprocessor directive.
    /// </summary>
    /// <param name="symbol">Symbol representing the preprocessor directive</param>
    /// <returns>
    /// Preprocessor tag
    /// </returns>
    /// <remarks>
    /// input:        "#" {ws} directive ws {ws} {not-newline} {newline}
    /// valid input:  "#" {ws} directive ws {ws} {non-ws} {ws} {newline}
    /// output:       {non-ws}
    /// </remarks>
    // --------------------------------------------------------------------------------
    private string GetPreprocessorSymbol(string symbol)
    {
      int start = 1;
      int end;

      // skip {ws}
      start = EndOf(symbol, start, true);
      // skip directive  
      start = EndOf(symbol, start, false);
      // skip ws {ws}
      start = EndOf(symbol, start, true);
      // search end of symbol
      end = EndOf(symbol, start, false);

      return symbol.Substring(start, end - start);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises the CS1028 error.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void ReportUnexpectedDirective()
    {
      _Parser.Error("CS1028", _Parser.Lookahead, "Unexpected preprocessor directive.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Raises the CS1032 error.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void DefineAndUndefError()
    {
      _Parser.Error("CS1032", _Parser.Lookahead, "Cannot define/undefine preprocessor symbols after first token in file.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the specified pragma condition.
    /// </summary>
    /// <param name="pragma">Pragma condition</param>
    /// <returns>Evaluation status</returns>
    // --------------------------------------------------------------------------------
    private PPEvaluationStatus EvaluatePragmaCondition(Token pragma)
    {
      string symbol = GetPreprocessorText(pragma.val);
      PPEvaluationStatus evalStatus = _Parser.CompilationUnit.EvaluatePreprocessorExpression(symbol);
      if (evalStatus == PPEvaluationStatus.Failed)
      {
        _Parser.Error("CS1517", pragma, "Invalid preprocessor expression");
      }
      return evalStatus;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Skips a false conditional block.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void SkipFalseBlock()
    {
      _Parser.Scanner.SkipMode = true;
      try
      {
        int state = 0;
        Token cur = _Parser.Scanner.PeekWithPragma();
        while(true)
        {
          switch (cur.kind)
          {
            case CSharpSyntaxParser.ppIfKind:
              ++state;
              break;
            case CSharpSyntaxParser.ppEndifKind:
              if (state == 0)
              {
                return;
              }
              --state;
              break;
            case CSharpSyntaxParser.ppElifKind:
              if (state == 0) 
              {
                return;
              }
              break;
            case CSharpSyntaxParser.ppElseKind:
              if (state == 0)
              {
                return;
              }
              break;
            case CSharpSyntaxParser.EOFKind:
              _Parser.Error("INCFIL", cur, "Incomplete file.");
              return;
            default:
              break;
          }

          // --- Read through the token we peeked
          _Parser.Scanner.Scan();
          cur = _Parser.Scanner.PeekWithPragma();
        }
      }
      finally
      {
        _Parser.Scanner.SkipMode = false;
      }
    }

    #endregion

    #region Public static methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the end of the whitespaces in the given string.
    /// </summary>
    /// <param name="symbol">Symbol to search for white spaces</param>
    /// <param name="start">Startinf index of the string representing the symbol</param>
    /// <param name="whitespaces">
    /// Flag indicating if we look for whitespace or non-whitespace
    /// </param>
    /// <returns>
    /// The end of the whitespaces in the given string if whitespaces is true;
    /// otherwise returns the end of the non-whitespaces.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static int EndOf(string symbol, int start, bool whitespaces)
    {
      while ((start < symbol.Length) && (Char.IsWhiteSpace(symbol[start]) ^ !whitespaces))
      {
        ++start;
      }

      return start;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the literal string starting at the specified position. Skips whitespaces
    /// before the string. Literal ends at the first whitespace character.
    /// </summary>
    /// <param name="symbol">Original symbol.</param>
    /// <param name="start">Start index of the literal.</param>
    /// <param name="end">Index of the character following the literal.</param>
    /// <returns>Literal string found.</returns>
    // --------------------------------------------------------------------------------
    public static string GetLiteral(string symbol, int start, out int end)
    {
      start = EndOf(symbol, start, true);
      end = EndOf(symbol, start, false);
      return symbol.Substring(start, end - start);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol contains only decimal digits.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol contains only decimal digits; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool HasDigitOnly(string symbol)
    {
      foreach (char c in symbol) 
        if (!Char.IsDigit(c)) return false;
      return true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol is a quoted file name.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol is a qouted file name; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static bool IsQuotedFileName(string symbol)
    {
      if (symbol == null || symbol.Length < 2) return false;
      if (!symbol.StartsWith("\"") || !symbol.EndsWith("\"")) return false;
      foreach (char c in symbol.Substring(1, symbol.Length - 2))
        if (c == '\"') return false;
      return true;
    }

    #endregion
  }
}