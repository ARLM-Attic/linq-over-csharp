using System;
using System.Collections.Generic;
using CSharpParser.ParserFiles.PPExpressions;

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

    private readonly Stack<IfPragmaState> _IfPragmaStack = new Stack<IfPragmaState>();
    private readonly CSharpSyntaxParser _Parser;

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
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalDirective(String symbol)
    {
      if (_FirstRealTokenOccurred)
      {
        DefineAndUndefError();
        return;
      }
      symbol = RemovePreprocessorDirective(symbol);
      _Parser.CompilationUnit.AddConditionalDirective(symbol);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    public void RemoveConditionalDirective(String symbol)
    {
      if (_FirstRealTokenOccurred)
      {
        DefineAndUndefError();
        return;
      }
      _Parser.CompilationUnit.RemoveConditionalDirective(RemovePreprocessorDirective(symbol));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #if pragma.
    /// </summary>
    /// <param name="pragma">Pragma expression</param>
    // --------------------------------------------------------------------------------
    public void IfPragma(Token pragma)
    {
      // ---Evaluate the pragam, if failed we skip it.
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
      if (CheckUnexpectedPragma(pragma)) return;
      _IfPragmaStack.Pop();
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
      _Parser.Error("CS1028", symbol, "Unexpected preprocessor directive.");
      return true;
    }

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
    private int EndOf(string symbol, int start, bool whitespaces)
    {
      while ((start < symbol.Length) && (Char.IsWhiteSpace(symbol[start]) ^ !whitespaces))
      {
        ++start;
      }

      return start;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes preprocessor directive.
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
    private string RemovePreprocessorDirective(string symbol)
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
      string symbol = RemovePreprocessorDirective(pragma.val);
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
  }
}