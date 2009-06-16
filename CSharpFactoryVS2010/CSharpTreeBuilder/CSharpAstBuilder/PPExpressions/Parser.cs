
using System;

namespace CSharpTreeBuilder.CSharpAstBuilder.PPExpressions {



// disable warnings about missing XML comments
#pragma warning disable 1591 

// ==================================================================================
/// <summary>
/// This class implements the C# syntax parser functionality.
/// </summary>
// ==================================================================================
public class CSharpPPExprSyntaxParser 
{
  #region These constants represent the grammar elements of the C# syntax.
  
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _false = 2;
	public const int _true = 3;
	public const int _and = 4;
	public const int _eq = 5;
	public const int _lpar = 6;
	public const int _neq = 7;
	public const int _not = 8;
	public const int _or = 9;
	public const int _rpar = 10;
	public const int maxT = 11;

  #endregion

  #region These constants are used within the parser independently of C# syntax.

  /// <summary>Represents the "true" value in the token set table.</summary>
  const bool T = true;

  /// <summary>Represents the "false" value in the token set table.</summary>
  const bool x = false;
	
  /// <summary>
  /// Represents the minimum distance (measured by tokens) between two error points 
  /// that are taken into account as separate errors.
  /// </summary>
  const int MinimumDistanceOfSeparateErrors = 2;

  #endregion

  #region Fields used by the parser

  /// <summary>Scanner used by the parser to obtain tokens.</summary>
  private PPScanner _Scanner;

  /// <summary>Represents the last recognized token.</summary>
  private Token t;
  
  /// <summary>Represents the lookahead token.</summary>
  private Token la;

  /// <summary>Represents the current distance from the last error.</summary>
  public bool ErrorFound;
  
  #endregion

private PPExpression _Expression;

  // --------------------------------------------------------------------------------
  /// <summary>
  /// Gets the expression parsed.
  /// </summary>
  // --------------------------------------------------------------------------------
  public PPExpression Expression
  {
    get { return _Expression; }
  }
    
// ================================================================================
// Scanner description
// ================================================================================



    #region Lifecycle methods
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this parser using the specified scanner, compilation
    /// uint and file.
    /// </summary>
    /// <param name="scanner">The scanner used to scan tokens.</param>
    // --------------------------------------------------------------------------------
  	public CSharpPPExprSyntaxParser(PPScanner scanner) 
  	{
	  	_Scanner = scanner;
  	}
  	
    #endregion

	  #region Methods referenced when CoCo generates the parser code
	  
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next token from the input stream.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method handles all the pragmas (like #region, #if, #endif, etc.) and 
    /// positions to the next token accordingly. Sets the private member "t" to the 
    /// current token, and member "la" to the next token.
    /// </para>
    /// <para>
    /// Line and block comments are handled by the scanner and not the parser.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
	  void Get () 
  	{
	  	for (;;) 
	  	{
        t = la;
        la = _Scanner.Scan();
        if (la.kind <= maxT) break;

			  la = t;
      }
    }
	
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Expects the next token to be as defined by tokenKind.
    /// </summary>
    /// <param name="tokenKind">Expected kind of token.</param>
    /// <remarks>
    /// If the token does not match with the kind specified, raises a syntax error.
    /// </remarks>
    // --------------------------------------------------------------------------------
    void Expect(int tokenKind)
    {
      if (la.kind == tokenKind) Get();
      else
      {
        SynErr(tokenKind);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the lookahead token can be a start for the specified tokenKind.
    /// </summary>
    /// <param name="tokenKind">Kind of token to examine.</param>
    /// <returns>
    /// True, if the lookahead token can be a strat for tokenKind; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool StartOf(int tokenKind)
    {
      return _StartupSet[tokenKind, la.kind];
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Expects the next token to be as defined by tokenKind. If the expected token is
    /// not found, steps forward in the input stream the find the token with type
    /// of "follow".
    /// </summary>
    /// <param name="tokenKind">Expected kind of token.</param>
    /// <param name="follow">Kind of week token to follow the parsing from.</param>
    /// <remarks>
    /// If the token does not match with the kind specified, raises a syntax error.
    /// </remarks>
    // --------------------------------------------------------------------------------
    void ExpectWeak(int tokenKind, int follow)
    {
      if (la.kind == tokenKind) Get();
      else
      {
        SynErr(tokenKind);
        while (!StartOf(follow)) Get();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Searches for a week separator.
    /// </summary>
    /// <param name="tokenKind"></param>
    /// <param name="syFol"></param>
    /// <param name="repFol"></param>
    /// <returns></returns>
    // --------------------------------------------------------------------------------
    bool WeakSeparator(int tokenKind, int syFol, int repFol)
    {
      bool[] s = new bool[maxT + 1];
      if (la.kind == tokenKind) { Get(); return true; }
      else if (StartOf(repFol)) return false;
      else
      {
        for (int i = 0; i <= maxT; i++)
        {
          s[i] = _StartupSet[syFol, i] || _StartupSet[repFol, i] || _StartupSet[0, i];
        }
        SynErr(tokenKind);
        while (!s[la.kind]) Get();
        return StartOf(syFol);
      }
    }
	
	  #endregion
	  
	  #region Parser methods generated by CoCo
	  
	void CS2PPExpr() {
		PPExpression(out _Expression);
	}

	void PPExpression(out PPExpression expr) {
		expr = null; 
		Unary(out expr);
		PPBinaryOperator oper = null; 
		PPExpression rightExpr; 
		while (la.kind == 9) {
			Get();
			oper = new PPOrOperator(); 
			oper.LeftOperand = expr; 
			Unary(out rightExpr);
			oper.RightOperand = rightExpr; 
			expr = oper; 
		}
		while (la.kind == 4) {
			Get();
			oper = new PPAndOperator(); 
			oper.LeftOperand = expr; 
			Unary(out rightExpr);
			oper.RightOperand = rightExpr; 
			expr = oper; 
		}
		while (la.kind == 5 || la.kind == 7) {
			if (la.kind == 5) {
				Get();
				oper = new PPEqualOperator(); 
			} else {
				Get();
				oper = new PPNotEqualOperator(); 
			}
			oper.LeftOperand = expr; 
			Unary(out rightExpr);
			oper.RightOperand = rightExpr; 
			expr = oper; 
		}
	}

	void Unary(out PPExpression expr) {
		expr = null; 
		if (StartOf(1)) {
			Primary(out expr);
		} else if (la.kind == 8) {
			Get();
			PPExpression operand; 
			Unary(out operand);
			expr = new PPNotOperator(operand); 
		} else SynErr(12);
	}

	void Primary(out PPExpression expr) {
		expr = null; 
		if (la.kind == 3) {
			Get();
			expr = new PPTrueLiteral(); 
		} else if (la.kind == 2) {
			Get();
			expr = new PPFalseLiteral(); 
		} else if (la.kind == 1) {
			Get();
			expr = new PPSymbol(t.val); 
		} else if (la.kind == 6) {
			Get();
			PPExpression(out expr);
			Expect(10);
		} else SynErr(13);
	}


    #endregion

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Starts parsing and parses the whole input accordingly to the specified language
    /// (C#) syntax.
    /// </summary>
    /// <remarks>
    /// Source should be parsed fully: the file must be ended when parsing is ready.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void Parse()
    {
		  la = new Token();
		  la.val = "";		
		  Get();
		CS2PPExpr();

      Expect(0);
  	}
	
    // --------------------------------------------------------------------------------
    /// <summary>This array represent the matrix of startup tokens.</summary>
    /// <remarks>
    /// In the cell of this matrix there is an "x" representing false and 
    /// "T" representing true. A cells contains true, if the token kind represented by
    /// the column (second dimension) can be directly followed by the token kind
    /// represented by the row (first dimension).
    /// </remarks>
    // --------------------------------------------------------------------------------
	  private bool[,] _StartupSet = 
    {
		{T,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, x,x,T,x, x,x,x,x, x}

	  };

	  #region Syntax error handling
	  
  	void SynErr (int n) 
  	{
  		string s;
      switch (n) 
	    {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "false expected"; break;
			case 3: s = "true expected"; break;
			case 4: s = "and expected"; break;
			case 5: s = "eq expected"; break;
			case 6: s = "lpar expected"; break;
			case 7: s = "neq expected"; break;
			case 8: s = "not expected"; break;
			case 9: s = "or expected"; break;
			case 10: s = "rpar expected"; break;
			case 11: s = "??? expected"; break;
			case 12: s = "invalid Unary"; break;
			case 13: s = "invalid Primary"; break;

  		  default: s = "error " + n; break;
	 	  }
      ErrorFound = true;
	  }

	  #endregion
}

  // ==================================================================================
  /// <summary>
  /// This class represents a fatal error occuring during the parse operation.
  /// </summary>
  // ==================================================================================
  public class FatalError : Exception
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this error.
    /// </summary>
    /// <param name="message">Error message.</param>
    // --------------------------------------------------------------------------------
    public FatalError(string message) : base(message) { }
  }
  
  #pragma warning restore 1591

}