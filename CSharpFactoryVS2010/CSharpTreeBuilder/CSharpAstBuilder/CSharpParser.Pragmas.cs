// ================================================================================================
// CSharpParser.Pragmas.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder.PPExpressions;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Pragma handler parts of the AST Builder
  /// </summary>
  // ================================================================================================
  public partial class CSharpParser
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the C# project associated with this parser.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private CSharpProject Project { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Flag indicating if the first real token occurred or not.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private bool _FirstRealTokenOccurred;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Stores state information about #if and related pragmas
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly Stack<IfPragmaState> _IfPragmaStack = new Stack<IfPragmaState>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Stores state information about #region pragmas.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly Stack<RegionPragmaNode> _RegionStack = new Stack<RegionPragmaNode>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs that the first real token occurred in the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void SignRealToken()
    {
      _FirstRealTokenOccurred = true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="pragma">Conditional directive</param>
    // ----------------------------------------------------------------------------------------------
    private void AddConditionalDirective(Token pragma)
    {
      var definePragma = new DefinePragmaNode(pragma);
      if (!RegisterPragma(definePragma)) return;
      if (_FirstRealTokenOccurred)
      {
        Error1032(Lookahead);
        return;
      }
      var symbol = definePragma.PreprocessorSymbol;
      if (!Project.ConditionalSymbols.Contains(symbol)) Project.ConditionalSymbols.Add(symbol);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="pragma">Conditional directive</param>
    // ----------------------------------------------------------------------------------------------
    private void RemoveConditionalDirective(Token pragma)
    {
      var undefPragma = new UndefPragmaNode(pragma);
      if (!RegisterPragma(undefPragma)) return;
      if (_FirstRealTokenOccurred)
      {
        Error1032(Lookahead);
        return;
      }
      Project.ConditionalSymbols.Remove(undefPragma.PreprocessorSymbol);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #if pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void IfPragma(Token pragma)
    {
      var ifPragma = new IfPragmaNode(pragma);
      if (!RegisterPragma(ifPragma)) return;

      // ---Evaluate the pragma, if failed we skip it.
      var evalStatus = EvaluatePragmaCondition(pragma);
      if (evalStatus == PPEvaluationStatus.Failed) return;

      // --- Prepare the pragma status
      var newIfPragma = new IfPragmaState(ifPragma, evalStatus == PPEvaluationStatus.True);
      _IfPragmaStack.Push(newIfPragma);

      // --- Skip the whole #if block, if evaluates to false.
      if (newIfPragma.TrueBlockFound)
      {
        ifPragma.EvaluatesToTrue = true;
      }
      else
      {
        SkipFalseBlock(ifPragma);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #elif pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void ElifPragma(Token pragma)
    {
      var elifPragma = new ElseIfPragmaNode(pragma);
      if (!RegisterPragma(elifPragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      var status = _IfPragmaStack.Peek();
      status.IfPragma.ElseIfPragmas.Add(elifPragma);
      if (!status.TrueBlockFound)
      {
        // --- Evaluate the #elif condition 
        var evalStatus = EvaluatePragmaCondition(pragma);
        status.TrueBlockFound = evalStatus == PPEvaluationStatus.True;

        // --- If this is the true block, we do not skip it.
        if (status.TrueBlockFound)
        {
          elifPragma.EvaluatesToTrue = true;
          return;
        }
      }
      SkipFalseBlock(elifPragma);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #else pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void ElsePragma(Token pragma)
    {
      var elsePragma = new ElsePragmaNode(pragma);
      if (!RegisterPragma(elsePragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      var status = _IfPragmaStack.Peek();
      status.ElseBlockFound = true;
      status.IfPragma.ElsePragma = elsePragma;
      if (status.TrueBlockFound)
      {
        SkipFalseBlock(elsePragma);
      }
      else
      {
        elsePragma.EvaluatesToTrue = true;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #endif pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void EndifPragma(Token pragma)
    {
      var endifPragma = new EndIfPragmaNode(pragma);
      if (!RegisterPragma(endifPragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      var status = _IfPragmaStack.Pop();
      status.IfPragma.EndIfPragma = endifPragma;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #line pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void LinePragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      string linePragmaText = GetPreprocessorText(pragma.val);
      if (linePragmaText.Length > 0)
      {
        int endLine;
        string lineSymbol = GetLiteral(linePragmaText, 0, out endLine);
        int endFile;
        string fileSymbol = GetLiteral(linePragmaText, endLine, out endFile);

        if (lineSymbol == "default" && string.IsNullOrEmpty(fileSymbol))
        {
          ErrorHandler.ResetRedirection();
          return;
        }
        if (HasDigitOnly(lineSymbol))
        {
          if (String.IsNullOrEmpty(fileSymbol))
          {
            ErrorHandler.Redirect(pragma.line, Int32.Parse(lineSymbol), null);
            return;
          }
          if (IsQuotedFileName(fileSymbol))
          {
            ErrorHandler.Redirect(pragma.line, Int32.Parse(lineSymbol),
                                  fileSymbol.Substring(1, fileSymbol.Length - 2));
            return;
          }
        }
      }
      Error1576(pragma);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #error pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void ErrorPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      Error1029(pragma, GetPreprocessorText(pragma.val));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #warning pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void WarningPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      ErrorHandler.Warning("CS1030", pragma, "#warning: " + GetPreprocessorText(pragma.val));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #pragma pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void PragmaPragma(Token pragma)
    {
      RegisterPragma(new PragmaPragmaNode(pragma));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #region pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void RegionPragma(Token pragma)
    {
      var parentRegion = _RegionStack.Count == 0 ? null : _RegionStack.Peek();
      var regionPragma = new RegionPragmaNode(pragma, parentRegion);
      if (!RegisterPragma(regionPragma)) return;
      _RegionStack.Push(regionPragma);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #endregion pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void EndRegionPragma(Token pragma)
    {
      var hostRegion = _RegionStack.Count == 0 ? null : _RegionStack.Peek();
      var endRegion = new EndRegionPragmaNode(pragma, hostRegion);
      if (!RegisterPragma(endRegion)) return;
      if (hostRegion == null)
      {
        Error1028(pragma);
        endRegion.Invalidate();
        return;
      }
      hostRegion.EndRegion = endRegion;
      _RegionStack.Pop();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Registers a pragma with the current source file.
    /// </summary>
    /// <param name="pragma">Syntax node representing the pragma.</param>
    /// <returns>
    /// True, if pragma is successfully registered, false, if pragma is not the first token in the
    /// current line.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    private bool RegisterPragma(PragmaNode pragma)
    {
      CompilationUnitNode.Pragmas.Add(pragma);
      var pragmaOk = CheckTokenIsFirstInLine(pragma.StartToken);
      if (!pragmaOk)
      {
        pragma.Invalidate();
        Error1040(pragma.StartToken);
      }
      return pragmaOk;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the pragma is the first token in the current line.
    /// </summary>
    /// <param name="symbol">Token representing the pragma.</param>
    /// <returns>True, if the pragma is the first token; otherwise, false.</returns>
    // ----------------------------------------------------------------------------------------------
    private bool CheckPragmaIsFirstInLine(Token symbol)
    {
      if (CheckTokenIsFirstInLine(symbol)) return true;
      Error1040(symbol);
      return false;
    }

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
        var status = _IfPragmaStack.Peek();
        if (symbol.kind == ppEndifKind || !status.ElseBlockFound)
          return false;
      }
      Error1028(symbol);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
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
    // ----------------------------------------------------------------------------------------------
    private static string GetPreprocessorText(string symbol)
    {
      var start = 1;
      // skip {ws}
      start = EndOf(symbol, start, true);
      // skip directive  
      start = EndOf(symbol, start, false);
      // skip ws {ws}
      start = EndOf(symbol, start, true);
      // search end of symbol
      var end = symbol.Length - 1;
      return end <= start ? String.Empty : symbol.Substring(start, end - start);
    }


    // ----------------------------------------------------------------------------------------------
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
    // ----------------------------------------------------------------------------------------------
    public static int EndOf(string symbol, int start, bool whitespaces)
    {
      while ((start < symbol.Length) && (Char.IsWhiteSpace(symbol[start]) ^ !whitespaces))
      {
        ++start;
      }
      return start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the specified pragma condition.
    /// </summary>
    /// <param name="pragma">Pragma condition</param>
    /// <returns>Evaluation status</returns>
    // ----------------------------------------------------------------------------------------------
    private PPEvaluationStatus EvaluatePragmaCondition(Token pragma)
    {
      string symbol = GetPreprocessorText(pragma.val);
      PPEvaluationStatus evalStatus = EvaluatePreprocessorExpression(symbol);
      if (evalStatus == PPEvaluationStatus.Failed)
      {
        Error1517(pragma);
      }
      return evalStatus;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the specified preprocessor expression.
    /// </summary>
    /// <param name="preprocessorExpression">Expression to evaluate</param>
    /// <returns>
    /// Result of the preprocessor evaluation
    /// </returns>
    // --------------------------------------------------------------------------------
    public PPEvaluationStatus EvaluatePreprocessorExpression(string preprocessorExpression)
    {
      var memStream =
        new MemoryStream(new UTF8Encoding().GetBytes(preprocessorExpression));
      var scanner = new PPScanner(memStream);
      var parser = new CSharpPPExprSyntaxParser(scanner);
      parser.Parse();
      if (parser.ErrorFound)
      {
        return PPEvaluationStatus.Failed;
      }
      return parser.Expression.Evaluate(Project.ConditionalSymbols)
               ? PPEvaluationStatus.True
               : PPEvaluationStatus.False;
    }


    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Skips a false conditional block.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void SkipFalseBlock(ConditionalPragmaNode pragmaNode)
    {
      Scanner.SkipMode = true;
      try
      {
        var state = 0;
        var cur = Scanner.PeekWithPragma();
        var firstFound = false;
        while(true)
        {
          switch (cur.kind)
          {
            case ppIfKind:
              ++state;
              SetSkipBoundaries(pragmaNode, cur, ref firstFound);
              break;
            case ppEndifKind:
              if (state == 0)
              {
                return;
              }
              --state;
              SetSkipBoundaries(pragmaNode, cur, ref firstFound);
              break;
            case ppElifKind:
              if (state == 0) 
              {
                return;
              }
              SetSkipBoundaries(pragmaNode, cur, ref firstFound);
              break;
            case ppElseKind:
              if (state == 0)
              {
                return;
              }
              SetSkipBoundaries(pragmaNode, cur, ref firstFound);
              break;
            case EOFKind:
              Error("INCFIL", cur, "Incomplete file.");
              return;
            default:
              SetSkipBoundaries(pragmaNode, cur, ref firstFound);
              break;
          }

          // --- Read through the token we peeked
          Scanner.Scan();
          cur = Scanner.PeekWithPragma();
        }
      }
      finally
      {
        Scanner.SkipMode = false;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the skipped token boundaries.
    /// </summary>
    /// <param name="pragmaNode">The pragma node.</param>
    /// <param name="token">The token.</param>
    /// <param name="firstFound">True means the first token has already been found.</param>
    // ----------------------------------------------------------------------------------------------
    private static void SetSkipBoundaries(ConditionalPragmaNode pragmaNode, Token token, 
      ref bool firstFound)
    {
      if (!firstFound)
      {
        pragmaNode.SetFirstSkippedToken(token);
        pragmaNode.SetLastSkippedToken(token);
        firstFound = true;
      }
      else
      {
        pragmaNode.SetLastSkippedToken(token);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the literal string starting at the specified position. Skips whitespaces
    /// before the string. Literal ends at the first whitespace character.
    /// </summary>
    /// <param name="symbol">Original symbol.</param>
    /// <param name="start">Start index of the literal.</param>
    /// <param name="end">Index of the character following the literal.</param>
    /// <returns>Literal string found.</returns>
    // ----------------------------------------------------------------------------------------------
    public static string GetLiteral(string symbol, int start, out int end)
    {
      start = EndOf(symbol, start, true);
      end = EndOf(symbol, start, false);
      return symbol.Substring(start, end - start);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol contains only decimal digits.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol contains only decimal digits; otherwise, false.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static bool HasDigitOnly(string symbol)
    {
      foreach (var c in symbol)
        if (!Char.IsDigit(c)) return false;
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol is a quoted file name.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol is a qouted file name; otherwise, false.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static bool IsQuotedFileName(string symbol)
    {
      if (symbol == null || symbol.Length < 2) return false;
      if (!symbol.StartsWith("\"") || !symbol.EndsWith("\"")) return false;
      foreach (var c in symbol.Substring(1, symbol.Length - 2))
        if (c == '\"') return false;
      return true;
    }
  }
}