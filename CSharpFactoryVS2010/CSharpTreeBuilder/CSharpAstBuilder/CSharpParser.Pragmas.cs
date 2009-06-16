// ================================================================================================
// CSharpParser.Pragmas.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
// ================================================================================================
// CSharpParser.Pragmas.cs
//
// Created: 2009.06.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.IO;
using System.Text;
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
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (_FirstRealTokenOccurred)
      {
        Error1032(Lookahead);
        return;
      }
      var symbol = GetPreprocessorSymbol(pragma.val).Trim();
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
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (_FirstRealTokenOccurred)
      {
        Error1032(Lookahead);
        return;
      }
      Project.ConditionalSymbols.Remove(GetPreprocessorSymbol(pragma.Value).Trim());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #if pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void IfPragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;

      // ---Evaluate the pragma, if failed we skip it.
      var evalStatus = EvaluatePragmaCondition(pragma);
      if (evalStatus == PPEvaluationStatus.Failed) return;

      // --- Prepare the pragma status
      var newIfPragma = new IfPragmaState(pragma)
                          {
                            TrueBlockFound = evalStatus == PPEvaluationStatus.True
                          };
      _IfPragmaStack.Push(newIfPragma);

      // --- Skip the whole #if block, if evaluates to false.
      if (!newIfPragma.TrueBlockFound)
      {
        SkipFalseBlock();
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
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      var status = _IfPragmaStack.Peek();
      if (!status.TrueBlockFound)
      {
        // --- Evaluate the #elif condition 
        var evalStatus = EvaluatePragmaCondition(pragma);
        status.TrueBlockFound = evalStatus == PPEvaluationStatus.True;

        // --- If this is the true block, we do not skip it.
        if (status.TrueBlockFound) return;
      }
      SkipFalseBlock();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #else pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // ----------------------------------------------------------------------------------------------
    public void ElsePragma(Token pragma)
    {
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      var status = _IfPragmaStack.Peek();
      status.ElseBlockFound = true;
      if (status.TrueBlockFound)
      {
        SkipFalseBlock();
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
      if (!CheckPragmaIsFirstInLine(pragma)) return;
      if (CheckUnexpectedPragma(pragma)) return;
      _IfPragmaStack.Pop();
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
    // ----------------------------------------------------------------------------------------------
    private static string GetPreprocessorSymbol(string symbol)
    {
      var start = 1;
      // skip {ws}
      start = EndOf(symbol, start, true);
      // skip directive  
      start = EndOf(symbol, start, false);
      // skip ws {ws}
      start = EndOf(symbol, start, true);
      // search end of symbol
      var end = EndOf(symbol, start, false);
      return symbol.Substring(start, end - start);
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
    private void SkipFalseBlock()
    {
      Scanner.SkipMode = true;
      try
      {
        int state = 0;
        Token cur = Scanner.PeekWithPragma();
        while(true)
        {
          switch (cur.kind)
          {
            case ppIfKind:
              ++state;
              break;
            case ppEndifKind:
              if (state == 0)
              {
                return;
              }
              --state;
              break;
            case ppElifKind:
              if (state == 0) 
              {
                return;
              }
              break;
            case ppElseKind:
              if (state == 0)
              {
                return;
              }
              break;
            case EOFKind:
              Error("INCFIL", cur, "Incomplete file.");
              return;
            default:
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

  }
}