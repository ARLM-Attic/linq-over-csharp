using System.Text;
using System.Collections;
using System.Collections.Generic;
using CSharpParser.ProjectModel;

using System;

namespace CSharpParser.ParserFiles {


public class Parser {
	const int _EOF = 0;
	const int _ident = 1;
	const int _intCon = 2;
	const int _realCon = 3;
	const int _charCon = 4;
	const int _stringCon = 5;
	const int _abstract = 6;
	const int _as = 7;
	const int _base = 8;
	const int _bool = 9;
	const int _break = 10;
	const int _byte = 11;
	const int _case = 12;
	const int _catch = 13;
	const int _char = 14;
	const int _checked = 15;
	const int _class = 16;
	const int _const = 17;
	const int _continue = 18;
	const int _decimal = 19;
	const int _default = 20;
	const int _delegate = 21;
	const int _do = 22;
	const int _double = 23;
	const int _else = 24;
	const int _enum = 25;
	const int _event = 26;
	const int _explicit = 27;
	const int _extern = 28;
	const int _false = 29;
	const int _finally = 30;
	const int _fixed = 31;
	const int _float = 32;
	const int _for = 33;
	const int _foreach = 34;
	const int _goto = 35;
	const int _if = 36;
	const int _implicit = 37;
	const int _in = 38;
	const int _int = 39;
	const int _interface = 40;
	const int _internal = 41;
	const int _is = 42;
	const int _lock = 43;
	const int _long = 44;
	const int _namespace = 45;
	const int _new = 46;
	const int _null = 47;
	const int _object = 48;
	const int _operator = 49;
	const int _out = 50;
	const int _override = 51;
	const int _params = 52;
	const int _private = 53;
	const int _protected = 54;
	const int _public = 55;
	const int _readonly = 56;
	const int _ref = 57;
	const int _return = 58;
	const int _sbyte = 59;
	const int _sealed = 60;
	const int _short = 61;
	const int _sizeof = 62;
	const int _stackalloc = 63;
	const int _static = 64;
	const int _string = 65;
	const int _struct = 66;
	const int _switch = 67;
	const int _this = 68;
	const int _throw = 69;
	const int _true = 70;
	const int _try = 71;
	const int _typeof = 72;
	const int _uint = 73;
	const int _ulong = 74;
	const int _unchecked = 75;
	const int _unsafe = 76;
	const int _ushort = 77;
	const int _usingKW = 78;
	const int _virtual = 79;
	const int _void = 80;
	const int _volatile = 81;
	const int _while = 82;
	const int _and = 83;
	const int _andassgn = 84;
	const int _assgn = 85;
	const int _colon = 86;
	const int _comma = 87;
	const int _dec = 88;
	const int _divassgn = 89;
	const int _dot = 90;
	const int _dblcolon = 91;
	const int _eq = 92;
	const int _gt = 93;
	const int _gteq = 94;
	const int _inc = 95;
	const int _lbrace = 96;
	const int _lbrack = 97;
	const int _lpar = 98;
	const int _lshassgn = 99;
	const int _lt = 100;
	const int _ltlt = 101;
	const int _minus = 102;
	const int _minusassgn = 103;
	const int _modassgn = 104;
	const int _neq = 105;
	const int _not = 106;
	const int _orassgn = 107;
	const int _plus = 108;
	const int _plusassgn = 109;
	const int _question = 110;
	const int _rbrace = 111;
	const int _rbrack = 112;
	const int _rpar = 113;
	const int _scolon = 114;
	const int _tilde = 115;
	const int _times = 116;
	const int _timesassgn = 117;
	const int _xorassgn = 118;
	const int maxT = 130;
	const int _ppDefine = 131;
	const int _ppUndef = 132;
	const int _ppIf = 133;
	const int _ppElif = 134;
	const int _ppElse = 135;
	const int _ppEndif = 136;
	const int _ppLine = 137;
	const int _ppError = 138;
	const int _ppWarning = 139;
	const int _ppRegion = 140;
	const int _ppEndReg = 141;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

#region Project Model extension

    private ProjectParser _Project;
    private ProjectFile _File;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the instance representing the project being parsed
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProjectParser Project
    {
      get { return _Project; }
      set { _Project = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the instance representing the file being parsed
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProjectFile File
    {
      get { return _File; }
      set { _File = value; }
    }

    #endregion

    #region Error handling

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="file">File that caused the error.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    public void Error(string code, Token errorPoint, ProjectFile file, string description)
    {
      Error error = new Error(code, errorPoint, file.Name, description);
      _Project.Errors.Add(error);
      Error(description);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="file">File that caused the error.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    public void Error(string code, Token errorPoint, ProjectFile file, string description,
      params object[] parameters)
    {
      Error error = new Error(code, errorPoint, file.Name, description, parameters);
      _Project.Errors.Add(error);
      Error(description);
    }

    #endregion

    #region Common parser methods

    /// <summary>List of conditional symbols</summary>
    List<string> _ConditionalSymbols = new List<string>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a list of symbol to the list of existing symbols.
    /// </summary>
    /// <param name="symbols">Array of symbols to add.</param>
    // --------------------------------------------------------------------------------
    public void AddConditionalCompilationSymbols(String[] symbols)
    {
      if (symbols != null)
      {
        for (int i = 0; i < symbols.Length; ++i)
        {
          symbols[i] = symbols[i].Trim();
          if (symbols[i].Length > 0 && !_ConditionalSymbols.Contains(symbols[i]))
          {
            _ConditionalSymbols.Add(symbols[i]);
          }
        }
      }
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
    int EndOf(String symbol, int start, bool whitespaces)
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
    String RemovePreprocessorDirective(String symbol)
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
    /// Adds a conditional directive to the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    void AddConditionalDirective(String symbol)
    {
      symbol = RemovePreprocessorDirective(symbol);
      if (!_ConditionalSymbols.Contains(symbol))
      {
        _ConditionalSymbols.Add(symbol);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Removes a conditional directive from the list of existing conditionals
    /// </summary>
    /// <param name="symbol">Conditional directive</param>
    // --------------------------------------------------------------------------------
    void RemoveConditionalDirective(String symbol)
    {
      _ConditionalSymbols.Remove(RemovePreprocessorDirective(symbol));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified symbol is a conditional symbol or not.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True, if the symbol is a conditional symbol; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsConditionalSymbol(String symbol)
    {
      return _ConditionalSymbols.Contains(RemovePreprocessorDirective(symbol));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the specified conditional symbol within the #if pragma
    /// </summary>
    /// <param name="symbol">Symbol to handle as conditional</param>
    /// <remarks>
    /// Search for the correct alternative and enter. Drop everything before the 
    /// correct alternative.
    /// </remarks>
    // --------------------------------------------------------------------------------
    void IfPragma(String symbol)
    {
      if (!IsConditionalSymbol(symbol))
      {
        int state = 0;
        Token cur = scanner.Scan();

        for (; ; )
        {
          switch (cur.kind)
          {
            case _ppIf: ++state; break;
            case _ppEndif:
              if (state == 0) { return; }
              --state;
              break;
            case _ppElif:
              if (state == 0 && IsConditionalSymbol(cur.val)) { return; }
              break;
            case _ppElse:
              if (state == 0) { return; }
              break;
            case _EOF: Error("Incomplete file."); return;
            default: break;
          }
          cur = scanner.Scan();
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Handles the #elseif or #else pragma.
    /// </summary>
    /// <remarks>
    /// Drops everything until the end of this if, elif, else directive.
    /// </remarks>
    // --------------------------------------------------------------------------------
    void ElifOrElsePragma()
    {
      int state = 0;
      Token cur = scanner.Scan();

      for (; ; )
      {
        switch (cur.kind)
        {
          case _ppIf: ++state; break;
          case _ppEndif:
            if (state == 0) { return; }
            --state;
            break;
          default: break;
        }
        cur = scanner.Scan();
      }
    }

    #endregion

    #region Token sets

    const int maxTerminals = 160;  // set size

    static BitArray NewSet(params int[] values)
    {
      BitArray a = new BitArray(maxTerminals);
      foreach (int item in values) a[item] = true;
      return a;
    }

    /// <summary>Tokens representing unary operators</summary>
    private static BitArray unaryOp =
      NewSet(_plus, _minus, _not, _tilde, _inc, _dec, _true, _false);

    /// <summary>Tokens representing type shortcuts</summary>
    private static BitArray typeKW =
      NewSet(_char, _bool, _object, _string, _sbyte, _byte, _short,
             _ushort, _int, _uint, _long, _ulong, _float, _double, _decimal);

    /// <summary>
    /// Tokens representing unary header tokens
    /// </summary>
    private static BitArray
      unaryHead = NewSet(_plus, _minus, _not, _tilde, _times, _inc, _dec, _and);

    /// <summary>
    /// Tokens representing assignment start tokens
    /// </summary>
    private static BitArray
      assnStartOp = NewSet(_plus, _minus, _not, _tilde, _times);

    /// <summary>Tokens that can follow a cast operation</summary>
    private static BitArray
      castFollower = NewSet(_tilde, _not, _lpar, _ident,
                            /* literals */
                            _intCon, _realCon, _charCon, _stringCon,
                            /* any keyword expect as and is */
                            _abstract, _base, _bool, _break, _byte, _case, _catch,
                            _char, _checked, _class, _const, _continue, _decimal, _default,
                            _delegate, _do, _double, _else, _enum, _event, _explicit,
                            _extern, _false, _finally, _fixed, _float, _for, _foreach,
                            _goto, _if, _implicit, _in, _int, _interface, _internal,
                            _lock, _long, _namespace, _new, _null, _object, _operator,
                            _out, _override, _params, _private, _protected, _public,
                            _readonly, _ref, _return, _sbyte, _sealed, _short, _sizeof,
                            _stackalloc, _static, _string, _struct, _switch, _this, _throw,
                            _true, _try, _typeof, _uint, _ulong, _unchecked, _unsafe,
                            _ushort, _usingKW, _virtual, _void, _volatile, _while
        );

    /// <summary>Tokens that can follow argument lists</summary>
    private static BitArray
      typArgLstFol = NewSet(_lpar, _rpar, _rbrack, _colon, _scolon, _comma, _dot,
                            _question, _eq, _neq);

    /// <summary>Reserved C# keywords</summary>
    private static BitArray
      keyword = NewSet(_abstract, _as, _base, _bool, _break, _byte, _case, _catch,
                       _char, _checked, _class, _const, _continue, _decimal, _default,
                       _delegate, _do, _double, _else, _enum, _event, _explicit,
                       _extern, _false, _finally, _fixed, _float, _for, _foreach,
                       _goto, _if, _implicit, _in, _int, _interface, _internal,
                       _is, _lock, _long, _namespace, _new, _null, _object, _operator,
                       _out, _override, _params, _private, _protected, _public,
                       _readonly, _ref, _return, _sbyte, _sealed, _short, _sizeof,
                       _stackalloc, _static, _string, _struct, _switch, _this, _throw,
                       _true, _try, _typeof, _uint, _ulong, _unchecked, _unsafe,
                       _ushort, _usingKW, _virtual, _void, _volatile, _while);

    /// <summary>Assignment operators</summary>
    private static BitArray
      assgnOps = NewSet(_assgn, _plusassgn, _minusassgn, _timesassgn, _divassgn,
                     _modassgn, _andassgn, _orassgn, _xorassgn, _lshassgn) 
      /* rshassgn: ">" ">="  no whitespace allowed*/
                     ;

    #endregion

    /*---------------------------- auxiliary methods ------------------------*/

    public void Error(string s)
    {
      if (errDist >= minErrDist) errors.SemErr(la.line, la.col, s);
      errDist = 0;
    }

    // Return the n-th token after the current lookahead token
    Token Peek(int n)
    {
      scanner.ResetPeek();
      Token x = la;
      while (n > 0) { x = scanner.Peek(); n--; }
      return x;
    }

    // ident "="
    bool IsAssignment()
    {
      return la.kind == _ident && Peek(1).kind == _assgn;
    }

    /* True, if the comma is not a trailing one, *
     * like the last one in: a, b, c,            */
    bool NotFinalComma()
    {
      int peek = Peek(1).kind;
      return la.kind == _comma && peek != _rbrace && peek != _rbrack;
    }

    /* Checks whether the next sequence of tokens is a qualident *
     * and returns the qualident string                          *
     * !!! Proceeds from current peek position !!!               */
    bool IsQualident(ref Token pt, out string qualident)
    {
      qualident = "";
      if (pt.kind == _ident)
      {
        qualident = pt.val;
        pt = scanner.Peek();
        while (pt.kind == _dot)
        {
          pt = scanner.Peek();
          if (pt.kind != _ident) return false;
          qualident += "." + pt.val;
          pt = scanner.Peek();
        }
        return true;
      }
      else return false;
    }

    bool IsGeneric()
    {
      scanner.ResetPeek();
      Token pt = la;
      if (!IsTypeArgumentList(ref pt))
      {
        return false;
      }
      return typArgLstFol[pt.kind];
    }

    bool IsTypeArgumentList(ref Token pt)
    {
      if (pt.kind == _lt)
      {
        pt = scanner.Peek();
        while (true)
        {
          if (!IsType(ref pt))
          {
            return false;
          }
          if (pt.kind == _gt)
          {
            // list recognized
            pt = scanner.Peek();
            break;
          }
          else if (pt.kind == _comma)
          {
            // another argument
            pt = scanner.Peek();
          }
          else
          {
            // error in type argument list
            return false;
          }
        }
      }
      else
      {
        return false;
      }
      return true;
    }

    // Type
    bool IsType(ref Token pt)
    {
      String dummyId;

      if (typeKW[pt.kind])
      {
        pt = scanner.Peek();
      }
      else if (pt.kind == _void)
      {
        pt = scanner.Peek();
        if (pt.kind != _times)
        {
          return false;
        }
        pt = scanner.Peek();
      }
      else if (pt.kind == _ident)
      {
        pt = scanner.Peek();
        if (pt.kind == _dblcolon || pt.kind == _dot)
        {
          // either namespace alias qualifier "::" or first
          // part of the qualident
          pt = scanner.Peek();
          if (!IsQualident(ref pt, out dummyId))
          {
            return false;
          }
        }
        if (pt.kind == _lt && !IsTypeArgumentList(ref pt))
        {
          return false;
        }
      }
      else
      {
        return false;
      }
      if (pt.kind == _question)
      {
        pt = scanner.Peek();
      }
      return SkipPointerOrDims(ref pt);
    }

    // Type ident
    // (Type can be void*)
    bool IsLocalVarDecl()
    {
      Token pt = la;
      scanner.ResetPeek();
      return IsType(ref pt) && pt.kind == _ident;
    }

    // "[" ("," | "]")
    bool IsDims()
    {
      int peek = Peek(1).kind;
      return la.kind == _lbrack && (peek == _comma || peek == _rbrack);
    }

    // "*" | "[" ("," | "]")
    bool IsPointerOrDims()
    {
      return la.kind == _times || IsDims();
    }

    /* skip: { "[" { "," } "]" | "*" }             */
    /* !!! Proceeds from current peek position !!! */
    bool SkipPointerOrDims(ref Token pt)
    {
      for (; ; )
      {
        if (pt.kind == _lbrack)
        {
          do pt = scanner.Peek();
          while (pt.kind == _comma);
          if (pt.kind != _rbrack) return false;
        }
        else if (pt.kind != _times) break;
        pt = scanner.Peek();
      }
      return true;
    }

    // Is attribute target specifier
    // (ident | keyword) ":"
    bool IsAttrTargSpec()
    {
      return (la.kind == _ident || keyword[la.kind]) && Peek(1).kind == _colon;
    }

    // ident ("," | "=" | ";")
    bool IsFieldDecl()
    {
      int peek = Peek(1).kind;
      return la.kind == _ident &&
             (peek == _comma || peek == _assgn || peek == _scolon);
    }

    bool IsTypeCast()
    {
      if (la.kind != _lpar) { return false; }
      if (IsSimpleTypeCast()) { return true; }
      return GuessTypeCast();
    }

    // "(" typeKW ")"
    bool IsSimpleTypeCast()
    {
      // assert: la.kind == _lpar
      scanner.ResetPeek();
      Token pt1 = scanner.Peek();
      Token pt2 = scanner.Peek();
      return typeKW[pt1.kind] &&
              (pt2.kind == _rpar ||
              (pt2.kind == _question && scanner.Peek().kind == _rpar));
    }

    // "(" Type ")" castFollower
    bool GuessTypeCast()
    {
      // assert: la.kind == _lpar
      scanner.ResetPeek();
      Token pt = scanner.Peek();
      if (!IsType(ref pt))
      {
        return false;
      }
      if (pt.kind != _rpar)
      {
        return false;
      }
      pt = scanner.Peek();
      return castFollower[pt.kind];
    }

    // "[" "assembly"
    bool IsGlobalAttrTarget()
    {
      Token pt = Peek(1);
      return la.kind == _lbrack && pt.kind == _ident && ("assembly".Equals(pt.val) || "module".Equals(pt.val));
    }

    // "extern"
    bool IsExternAliasDirective()
    {
      return la.kind == _extern;
    }

    // true: anyToken"<"
    // no whitespace between the token and the "<" allowed
    // anything else will return false.
    bool IsLtNoWs()
    {
      return (la.kind == _lt) && ((t.pos + t.val.Length) == la.pos);
    }

    bool IsNoSwitchLabelOrRBrace()
    {
      return (la.kind != _case && la.kind != _default && la.kind != _rbrace) ||
             (la.kind == _default && Peek(1).kind != _colon);
    }

    bool IsShift()
    {
      Token pt = Peek(1);
      return (la.kind == _ltlt) ||
             (la.kind == _gt &&
               pt.kind == _gt &&
               (la.pos + la.val.Length == pt.pos)
             );
    }

    // true: TypeArgumentList followed by anything but "("
    bool IsPartOfMemberName()
    {
      scanner.ResetPeek();
      Token pt = la;
      if (!IsTypeArgumentList(ref pt))
      {
        return false;
      }
      return pt.kind != _lpar;
    }

/*------------------------------------------------------------------------*
 *----- SCANNER DESCRIPTION ----------------------------------------------*
 *------------------------------------------------------------------------*/



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }
				if (la.kind == 131) {
				AddConditionalDirective(la.val); 
				}
				if (la.kind == 132) {
				RemoveConditionalDirective(la.val); 
				}
				if (la.kind == 133) {
				IfPragma(la.val); 
				}
				if (la.kind == 134) {
				ElifOrElsePragma(); 
				}
				if (la.kind == 135) {
				ElifOrElsePragma(); 
				}
				if (la.kind == 136) {
				}
				if (la.kind == 137) {
				}
				if (la.kind == 138) {
				}
				if (la.kind == 139) {
				}
				if (la.kind == 140) {
				}
				if (la.kind == 141) {
				}

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}
	
	bool WeakSeparator (int n, int syFol, int repFol) {
		bool[] s = new bool[maxT+1];
		if (la.kind == n) { Get(); return true; }
		else if (StartOf(repFol)) return false;
		else {
			for (int i=0; i <= maxT; i++) {
				s[i] = set[syFol, i] || set[repFol, i] || set[0, i];
			}
			SynErr(n);
			while (!s[la.kind]) Get();
			return StartOf(syFol);
		}
	}
	
	void CS2() {
		while (IsExternAliasDirective()) {
			ExternAliasDirective(null);
		}
		while (la.kind == 78) {
			UsingDirective(null);
		}
		while (IsGlobalAttrTarget()) {
			GlobalAttributes();
		}
		while (StartOf(1)) {
			NamespaceMemberDeclaration(null, _File);
		}
	}

	void ExternAliasDirective(Namespace parent) {
		Expect(28);
		ExternalAlias externAlias = new ExternalAlias(t); 
		Expect(1);
		if (t.val != "alias") 
		 Error("CS1003", t, _File, "Syntax error, 'alias' expected"); 
		
		Expect(1);
		externAlias.Name = t.val;
		if (parent == null) _File.AddExternAlias(externAlias); 
		else parent.AddExternAlias(externAlias); 
		
		Expect(114);
	}

	void UsingDirective(Namespace parent) {
		Expect(78);
		UsingClause uc = new UsingClause(t);
		string name = String.Empty; 
		TypeReference typeUsed;
		
		if (IsAssignment()) {
			Expect(1);
			name = t.val; 
			Expect(85);
		}
		TypeName(out typeUsed);
		Expect(114);
		uc.Name = name;
		uc.TypeUsed = typeUsed;
		if (parent == null) _File.AddUsingClause(uc);
		else parent.AddUsingClause(uc); 
		
	}

	void GlobalAttributes() {
		Expect(97);
		Expect(1);
		if (!"assembly".Equals(t.val) && !"module".Equals(t.val)) 
		 Error("global attribute target specifier \"assembly\" or \"module\" expected");
		string scope = t.val;
		AttributeDeclaration attr;
		
		Expect(86);
		Attribute(out attr);
		attr.Scope = scope; 
		_File.GlobalAttributes.Add(attr);
		
		while (NotFinalComma()) {
			Expect(87);
			Attribute(out attr);
			attr.Scope = scope; 
			_File.GlobalAttributes.Add(attr);
			
		}
		if (la.kind == 87) {
			Get();
		}
		Expect(112);
	}

	void NamespaceMemberDeclaration(Namespace parent, ProjectFile file) {
		if (la.kind == 45) {
			Get();
			Token startToken = t; 
			Expect(1);
			StringBuilder sb = new StringBuilder(t.val); 
			while (la.kind == 90) {
				Get();
				Expect(1);
				sb.Append("."); sb.Append(t.val); 
			}
			Namespace ns = new Namespace(startToken, sb.ToString(), parent, file); 
			Expect(96);
			while (IsExternAliasDirective()) {
				ExternAliasDirective(ns);
			}
			while (la.kind == 78) {
				UsingDirective(ns);
			}
			while (StartOf(1)) {
				NamespaceMemberDeclaration(ns, _File);
			}
			Expect(111);
			if (la.kind == 114) {
				Get();
			}
		} else if (StartOf(2)) {
			Modifiers m = new Modifiers(this); 
			TypeDeclaration td;
			AttributeCollection attrs = new AttributeCollection();
			
			while (la.kind == 97) {
				Attributes(attrs);
			}
			ModifierList(m);
			TypeDeclaration(attrs, m, out td);
			if (td != null)
			{
			  td.AssignAttributes(attrs);
			  if (parent == null) 
			    _File.TypeDeclarations.Add(td);
			  else 
			    parent.TypeDeclarations.Add(td);
			}
			
		} else SynErr(131);
	}

	void TypeName(out TypeReference typeRef) {
		Expect(1);
		typeRef = new TypeReference(t);
		typeRef.Name = t.val; 
		TypeReference nextType = typeRef;
		
		if (la.kind == 91) {
			Get();
			typeRef.IsGlobal = true; 
			Expect(1);
			typeRef.SubType = new TypeReference(t);
			typeRef.SubType.Name = t.val; 
			nextType = typeRef.SubType;
			
		}
		if (la.kind == 100) {
			TypeArgumentList(typeRef.Arguments);
		}
		while (la.kind == 90) {
			Get();
			Expect(1);
			nextType.SubType = new TypeReference(t);
			nextType.SubType.Name = t.val;
			
			if (la.kind == 100) {
				TypeArgumentList(typeRef.Arguments);
			}
			nextType = nextType.SubType; 
		}
	}

	void Attribute(out AttributeDeclaration attr) {
		TypeReference typeRef; 
		TypeName(out typeRef);
		attr = new AttributeDeclaration(t); 
		attr.TypeReference = typeRef;
		
		if (la.kind == 98) {
			AttributeArguments(attr);
		}
	}

	void Attributes(AttributeCollection attrs) {
		string scope = ""; 
		Expect(97);
		if (IsAttrTargSpec()) {
			if (la.kind == 1) {
				Get();
			} else if (StartOf(3)) {
				Keyword();
			} else SynErr(132);
			scope = t.val; 
			Expect(86);
		}
		AttributeDeclaration attr; 
		Attribute(out attr);
		attr.Scope = scope;
		attrs.Add(attr);
		
		while (la.kind == _comma && Peek(1).kind != _rbrack) {
			Expect(87);
			Attribute(out attr);
			attr.Scope = scope;
			attrs.Add(attr);
			
		}
		if (la.kind == 87) {
			Get();
		}
		Expect(112);
	}

	void ModifierList(Modifiers m) {
		while (StartOf(4)) {
			switch (la.kind) {
			case 46: {
				Get();
				m.Add(Modifier.@new); 
				break;
			}
			case 55: {
				Get();
				m.Add(Modifier.@public); 
				break;
			}
			case 54: {
				Get();
				m.Add(Modifier.@protected); 
				break;
			}
			case 41: {
				Get();
				m.Add(Modifier.@internal); 
				break;
			}
			case 53: {
				Get();
				m.Add(Modifier.@private); 
				break;
			}
			case 76: {
				Get();
				m.Add(Modifier.@unsafe); 
				break;
			}
			case 64: {
				Get();
				m.Add(Modifier.@static); 
				break;
			}
			case 56: {
				Get();
				m.Add(Modifier.@readonly); 
				break;
			}
			case 81: {
				Get();
				m.Add(Modifier.@volatile); 
				break;
			}
			case 79: {
				Get();
				m.Add(Modifier.@virtual); 
				break;
			}
			case 60: {
				Get();
				m.Add(Modifier.@sealed); 
				break;
			}
			case 51: {
				Get();
				m.Add(Modifier.@override); 
				break;
			}
			case 6: {
				Get();
				m.Add(Modifier.@abstract); 
				break;
			}
			case 28: {
				Get();
				m.Add(Modifier.@extern); 
				break;
			}
			}
		}
	}

	void TypeDeclaration(AttributeCollection attrs, Modifiers m, out TypeDeclaration td) {
		td = null; 
		if (StartOf(5)) {
			bool isPartial = false; 
			if (la.kind == 119) {
				Get();
				isPartial = true; 
			}
			if (la.kind == 16) {
				ClassDeclaration(m, isPartial, out td);
			} else if (la.kind == 66) {
				StructDeclaration(m, isPartial, out td);
			} else if (la.kind == 40) {
				InterfaceDeclaration(m, isPartial, out td);
			} else SynErr(133);
		} else if (la.kind == 25) {
			EnumDeclaration(m, out td);
		} else if (la.kind == 21) {
			DelegateDeclaration(m, out td);
		} else SynErr(134);
		if (td != null)
		{
		  td.SetModifiers(m.Value); 
		  td.AssignAttributes(attrs);
		}
		
	}

	void ClassDeclaration(Modifiers m, bool isPartial, out TypeDeclaration td) {
		Expect(16);
		m.Check(Modifier.classes); 
		ClassDeclaration cd = new ClassDeclaration(t);
		cd.IsPartial = isPartial;
		td = cd;
		
		Expect(1);
		cd.Name = t.val; 
		if (la.kind == 100) {
			TypeParameterCollection typePars = null; 
			TypeParameterList(out typePars);
			cd.AssignTypeParameters(typePars); 
		}
		if (la.kind == 86) {
			ClassBase(cd);
		}
		while (la.kind == 1) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.ParameterConstraints.Add(constraint); 
		}
		ClassBody(td);
		if (la.kind == 114) {
			Get();
		}
	}

	void StructDeclaration(Modifiers m, bool isPartial, out TypeDeclaration td) {
		Expect(66);
		m.Check(Modifier.nonClassTypes); 
		StructDeclaration sd = new StructDeclaration(t);
		td = sd;
		sd.IsPartial = isPartial;
		TypeReference typeRef;
		
		Expect(1);
		sd.Name = t.val; 
		if (la.kind == 100) {
			TypeParameterCollection typePars = null; 
			TypeParameterList(out typePars);
			sd.AssignTypeParameters(typePars); 
		}
		if (la.kind == 86) {
			Get();
			TypeName(out typeRef);
			sd.BaseTypes.Add(typeRef); 
			while (la.kind == 87) {
				Get();
				TypeName(out typeRef);
				sd.BaseTypes.Add(typeRef); 
			}
		}
		while (la.kind == 1) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.ParameterConstraints.Add(constraint); 
		}
		StructBody(td);
		if (la.kind == 114) {
			Get();
		}
	}

	void InterfaceDeclaration(Modifiers m, bool isPartial, out TypeDeclaration td) {
		Expect(40);
		m.Check(Modifier.nonClassTypes); 
		InterfaceDeclaration ifd = new InterfaceDeclaration(t);
		td = ifd;
		ifd.IsPartial = isPartial;
		
		Expect(1);
		ifd.Name = t.val; 
		if (la.kind == 100) {
			TypeParameterCollection typePars = null; 
			TypeParameterList(out typePars);
			ifd.AssignTypeParameters(typePars); 
		}
		if (la.kind == 86) {
			InterfaceBase(ifd);
		}
		while (la.kind == 1) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.ParameterConstraints.Add(constraint); 
		}
		Expect(96);
		while (StartOf(6)) {
			InterfaceMemberDeclaration(ifd);
		}
		Expect(111);
		if (la.kind == 114) {
			Get();
		}
	}

	void EnumDeclaration(Modifiers m, out TypeDeclaration td) {
		Expect(25);
		m.Check(Modifier.nonClassTypes); 
		EnumDeclaration ed = new EnumDeclaration(t);
		td = ed;
		
		Expect(1);
		ed.Name = t.val; 
		if (la.kind == 86) {
			Get();
			IntegralType();
			TypeReference tr = new TypeReference(t);
			tr.Name = t.val; 
			ed.BaseTypes.Add(tr);
			
		}
		EnumBody(ed);
		if (la.kind == 114) {
			Get();
		}
	}

	void DelegateDeclaration(Modifiers m, out TypeDeclaration td) {
		Expect(21);
		m.Check(Modifier.nonClassTypes); 
		DelegateDeclaration dd = new DelegateDeclaration(t);
		td = dd;
		dd.SetModifiers(m.Value);
		TypeReference returnType;
		
		Type(out returnType, true);
		dd.ReturnType = returnType; 
		Expect(1);
		dd.Name = t.val; 
		if (la.kind == 100) {
			TypeParameterCollection typePars = null; 
			TypeParameterList(out typePars);
			dd.AssignTypeParameters(typePars); 
		}
		Expect(98);
		if (StartOf(7)) {
			FormalParameterList(dd.FormalParameters);
		}
		Expect(113);
		while (la.kind == 1) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.ParameterConstraints.Add(constraint); 
		}
		Expect(114);
	}

	void TypeParameterList(out TypeParameterCollection typePars) {
		typePars = new TypeParameterCollection(); 
		Expect(100);
		TypeParameter tp; 
		TypeParameter(out tp);
		typePars.Add(tp); 
		while (la.kind == 87) {
			Get();
			TypeParameter(out tp);
			typePars.Add(tp); 
		}
		Expect(93);
	}

	void ClassBase(ClassDeclaration cd) {
		Expect(86);
		TypeReference typeRef; 
		ClassType(out typeRef);
		cd.BaseTypes.Add(typeRef); 
		while (la.kind == 87) {
			Get();
			TypeName(out typeRef);
			cd.BaseTypes.Add(typeRef); 
		}
	}

	void TypeParameterConstraintsClause(out TypeParameterConstraint constraint) {
		Expect(1);
		if (t.val != "where") 
		 Error("type parameter constraints clause must start with: where");
		
		Expect(1);
		constraint = new TypeParameterConstraint(t); 
		constraint.Name = t.val;
		
		Expect(86);
		TypeReference typeRef; 
		constraint.ParameterType = ParameterConstraintType.Type;
		
		if (la.kind == 1 || la.kind == 16 || la.kind == 66) {
			if (la.kind == 16 || la.kind == 66) {
				if (la.kind == 16) {
					Get();
					constraint.ParameterType = ParameterConstraintType.Class; 
				} else if (la.kind == 66) {
					Get();
					constraint.ParameterType = ParameterConstraintType.Struct; 
				} else SynErr(135);
				typeRef = new TypeReference(t);
				typeRef.Name = t.val;
				
			} else {
				TypeName(out typeRef);
			}
			constraint.Constraints.Add(typeRef); 
			while (la.kind == _comma && Peek(1).kind != _new) {
				Expect(87);
				TypeName(out typeRef);
				constraint.Constraints.Add(typeRef); 
			}
			if (la.kind == 87) {
				Get();
				Expect(46);
				Expect(98);
				Expect(113);
				constraint.HasNew = true; 
			}
		} else if (la.kind == 46) {
			Get();
			Expect(98);
			Expect(113);
			constraint.HasNew = true; 
		} else SynErr(136);
	}

	void ClassBody(TypeDeclaration td) {
		AttributeCollection attrs = new AttributeCollection(); 
		Expect(96);
		while (StartOf(8)) {
			while (la.kind == 97) {
				Attributes(attrs);
			}
			Modifiers m = new Modifiers(this); 
			ModifierList(m);
			ClassMemberDeclaration(attrs, m, td);
		}
		Expect(111);
	}

	void ClassType(out TypeReference typeRef) {
		typeRef = new TypeReference(t); 
		if (la.kind == 1) {
			TypeName(out typeRef);
		} else if (la.kind == 48) {
			Get();
			typeRef.Name = t.val; 
		} else if (la.kind == 65) {
			Get();
			typeRef.Name = t.val; 
		} else SynErr(137);
	}

	void ClassMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		if (StartOf(9)) {
			StructMemberDeclaration(attrs, m, td);
		} else if (la.kind == 115) {
			Get();
			Expect(1);
			DestructorDeclaration dd = new DestructorDeclaration(t);
			dd.Name = t.val;
			dd.SetModifiers(m.Value);
			dd.AssignAttributes(attrs);
			
			Expect(98);
			Expect(113);
			if (la.kind == 96) {
				Block(dd);
			} else if (la.kind == 114) {
				Get();
			} else SynErr(138);
			td.Members.Add(dd); 
		} else SynErr(139);
	}

	void StructBody(TypeDeclaration td) {
		AttributeCollection attrs = new AttributeCollection(); 
		Expect(96);
		while (StartOf(10)) {
			while (la.kind == 97) {
				Attributes(attrs);
			}
			Modifiers m = new Modifiers(this); 
			ModifierList(m);
			StructMemberDeclaration(attrs, m, td);
		}
		Expect(111);
	}

	void StructMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		TypeReference typeRef; 
		if (la.kind == 17) {
			ConstMemberDeclaration(attrs, m, td);
		} else if (la.kind == 26) {
			EventDeclaration(attrs, m, td);
		} else if (la.kind == _ident && Peek(1).kind == _lpar) {
			ConstructorDeclaration(attrs, m, td);
		} else if (StartOf(11)) {
			Type(out typeRef, true);
			if (la.kind == 49) {
				OperatorDeclaration(attrs, m, typeRef, td);
			} else if (IsFieldDecl()) {
				FieldMemberDeclarators(attrs, m, td, typeRef, false, Modifier.fields);
				Expect(114);
			} else if (la.kind == 1) {
				TypeReference memberRef; 
				MemberName(out memberRef);
				if (la.kind == 96) {
					PropertyDeclaration(attrs, m, typeRef, memberRef, td);
				} else if (la.kind == 90) {
					Get();
					IndexerDeclaration(attrs, m, typeRef, memberRef, td);
				} else if (la.kind == 98 || la.kind == 100) {
					MethodDeclaration(attrs, m, typeRef, memberRef, td, true);
				} else SynErr(140);
			} else if (la.kind == 68) {
				IndexerDeclaration(attrs, m, typeRef, null, td);
			} else SynErr(141);
		} else if (la.kind == 27 || la.kind == 37) {
			CastOperatorDeclaration(attrs, m, td);
		} else if (StartOf(12)) {
			TypeDeclaration nestedType; 
			TypeDeclaration(attrs, m, out nestedType);
			nestedType.ParentType = td; 
		} else SynErr(142);
	}

	void IntegralType() {
		switch (la.kind) {
		case 59: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 61: {
			Get();
			break;
		}
		case 77: {
			Get();
			break;
		}
		case 39: {
			Get();
			break;
		}
		case 73: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 74: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		default: SynErr(143); break;
		}
	}

	void EnumBody(EnumDeclaration ed) {
		Expect(96);
		if (la.kind == 1 || la.kind == 97) {
			EnumMemberDeclaration(ed);
			while (NotFinalComma()) {
				Expect(87);
				EnumMemberDeclaration(ed);
			}
			if (la.kind == 87) {
				Get();
			}
		}
		Expect(111);
	}

	void EnumMemberDeclaration(EnumDeclaration ed) {
		AttributeCollection attrs = new AttributeCollection(); 
		while (la.kind == 97) {
			Attributes(attrs);
		}
		Expect(1);
		EnumValueDeclaration ev = new EnumValueDeclaration(t); 
		ev.Name = t.val;
		Expression expr;
		
		if (la.kind == 85) {
			Get();
			Expression(out expr);
			ev.ValueExpression = expr; 
		}
		ev.AssignAttributes(attrs);
		ed.Values.Add(ev);
		
	}

	void Expression(out Expression expr) {
		expr = null; 
		Expression leftExpr; 
		Unary(out leftExpr);
		if (assgnOps[la.kind] || (la.kind == _gt && Peek(1).kind == _gteq)) {
			AssignmentOperator asgn; 
			AssignmentOperator(out asgn);
			Expression rightExpr; 
			Expression(out rightExpr);
			asgn.RightOperand = rightExpr; 
			asgn.LeftOperand = leftExpr; 
			expr = asgn; 
		} else if (StartOf(13)) {
			BinaryOperator simpleExpr; 
			NullCoalescingExpr(out simpleExpr);
			if (simpleExpr == null) 
			{
			  expr = leftExpr;
			}
			else
			{
			  simpleExpr.LeftMostNonNull.LeftOperand = leftExpr;
			  expr = simpleExpr;
			}
			
			if (la.kind == 110) {
				ConditionalOperator condExpr = new ConditionalOperator(t); 
				condExpr.Condition = simpleExpr; 
				expr = condExpr; 
				Get();
				Expression trueExpr; 
				Expression(out trueExpr);
				condExpr.TrueExpression = trueExpr; 
				Expect(86);
				Expression falseExpr; 
				Expression(out falseExpr);
				condExpr.FalseExpression = falseExpr; 
			}
		} else SynErr(144);
	}

	void Type(out TypeReference typeRef, bool voidAllowed) {
		typeRef = new TypeReference(t); 
		if (StartOf(14)) {
			PrimitiveType();
			typeRef = new TypeReference(t); 
			typeRef.Name = t.val;
			
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef);
		} else if (la.kind == 80) {
			Get();
			typeRef = new TypeReference(t); 
			typeRef.Name = t.val;
			typeRef.Kind = TypeKind.@void; 
			
		} else SynErr(145);
		if (la.kind == 110) {
			Get();
			if (typeRef.Kind == TypeKind.@void) { Error("Unexpected token ?, void must not be nullable."); } 
		}
		PointerOrArray(ref typeRef);
		if (typeRef.Kind == TypeKind.@void && !voidAllowed) { Error("type expected, void found, maybe you mean void*"); } 
	}

	void FormalParameterList(FormalParameterCollection pars) {
		TypeReference typeRef = null; 
		AttributeCollection attrs = new AttributeCollection();
		
		while (la.kind == 97) {
			Attributes(attrs);
		}
		FormalParameter fp = new FormalParameter(t); 
		fp.AssignAttributes(attrs);
		
		if (StartOf(15)) {
			if (la.kind == 50 || la.kind == 57) {
				if (la.kind == 57) {
					Get();
					fp.Kind = FormalParameterKind.Ref; 
				} else {
					Get();
					fp.Kind = FormalParameterKind.Out; 
				}
			}
			Type(out typeRef, false);
			fp.Type = typeRef; 
			Expect(1);
			fp.Name = t.val; 
			fp.Type = typeRef;
			pars.Add(fp);
			
			if (la.kind == 87) {
				Get();
				FormalParameterList(pars);
			}
		} else if (la.kind == 52) {
			Get();
			fp.HasParams = true; 
			Type(out typeRef, false);
			if (typeRef.Kind != TypeKind.array) { Error("params argument must be an array"); } 
			Expect(1);
			fp.Name = t.val; 
		} else SynErr(146);
	}

	void Block(IBlockOwner block) {
		Expect(96);
		while (StartOf(16)) {
			Statement(block);
		}
		Expect(111);
	}

	void ConstMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		m.Check(Modifier.constants); 
		Expect(17);
		TypeReference typeRef; 
		Type(out typeRef, false);
		SingleConstMember(attrs, m, td, typeRef);
		while (la.kind == 87) {
			Get();
			SingleConstMember(attrs, m, td, typeRef);
		}
		Expect(114);
	}

	void EventDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		TypeReference typeRef; 
		Expect(26);
		Type(out typeRef, false);
		if (IsFieldDecl()) {
			FieldMemberDeclarators(attrs, m, td, typeRef, true, Modifier.propEvntMeths);
			Expect(114);
		} else if (la.kind == 1) {
			TypeReference memberRef; 
			TypeName(out memberRef);
			Expect(96);
			EventPropertyDeclaration ep = new EventPropertyDeclaration(t);  
			ep.ResultingType = typeRef; 
			ep.ExplicitName = memberRef; 
			td.Members.Add(ep); 
			EventAccessorDeclarations(ep);
			Expect(111);
		} else SynErr(147);
	}

	void ConstructorDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		m.Check(Modifier.constructors | Modifier.staticConstr); 
		Expect(1);
		ConstructorDeclaration cd = new ConstructorDeclaration(t);
		cd.Name = t.val;
		cd.SetModifiers(m.Value);
		cd.AssignAttributes(attrs);
		
		Expect(98);
		if (StartOf(7)) {
			FormalParameterList(cd.FormalParameters);
		}
		Expect(113);
		if (la.kind == 86) {
			Get();
			if (la.kind == 8) {
				Get();
				cd.HasBase = true; 
			} else if (la.kind == 68) {
				Get();
				cd.HasThis = true; 
			} else SynErr(148);
			Expect(98);
			if (StartOf(17)) {
				Argument(null);
				while (la.kind == 87) {
					Get();
					Argument(null);
				}
			}
			Expect(113);
		}
		if (la.kind == 96) {
			Block(cd);
		} else if (la.kind == 114) {
			Get();
		} else SynErr(149);
		td.Members.Add(cd); 
	}

	void OperatorDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeDeclaration td) {
		m.Check(Modifier.operators);
		m.CheckMust(Modifier.operatorsMust);
		if (typeRef.Kind == TypeKind.@void) { Error("operator not allowed on void"); } 
		OperatorDeclaration od = new OperatorDeclaration(t);
		td.Members.Add(od);
		od.SetModifiers(m.Value);
		od.AssignAttributes(attrs);
		od.ResultingType = typeRef;
		Operator op;
		
		Expect(49);
		OverloadableOp(out op);
		od.Operator = op; 
		Expect(98);
		FormalParameter fp = new FormalParameter(t);
		TypeReference parType;
		   
		Type(out parType, false);
		fp.Type = parType; 
		Expect(1);
		fp.Name = t.val; 
		od.FormalParameters.Add(fp);
		
		if (la.kind == 87) {
			Get();
			fp = new FormalParameter(t); 
			Type(out parType, false);
			fp.Type = parType; 
			Expect(1);
			fp.Name = t.val; 
			od.FormalParameters.Add(fp);
			
			if ((op & Operator.binary) == 0) Error("too many operands for unary operator"); 
		} else if (la.kind == 113) {
			if ((op & Operator.unary) == 0) Error("too few operands for binary operator"); 
		} else SynErr(150);
		Expect(113);
		if (la.kind == 96) {
			Block(od);
		} else if (la.kind == 114) {
			Get();
		} else SynErr(151);
	}

	void FieldMemberDeclarators(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef, bool isEvent, Modifier toCheck) {
		m.Check(toCheck);
		if (typeRef.Kind == TypeKind.@void) { Error("field type must not be void"); } 
		
		SingleFieldMember(attrs, m, td, typeRef, isEvent);
		while (la.kind == 87) {
			Get();
			SingleFieldMember(attrs, m, td, typeRef, isEvent);
		}
	}

	void MemberName(out TypeReference typeRef) {
		Expect(1);
		typeRef = new TypeReference(t);
		typeRef.Name = t.val; 
		TypeReference nextType = typeRef;
		
		if (la.kind == 91) {
			Get();
			typeRef.IsGlobal = true; 
			Expect(1);
			typeRef.SubType = new TypeReference(t);
			typeRef.SubType.Name = t.val; 
			nextType = typeRef.SubType;
			
		}
		if (la.kind == _lt && IsPartOfMemberName()) {
			TypeArgumentList(typeRef.Arguments);
		}
		while (la.kind == _dot && Peek(1).kind == _ident) {
			Expect(90);
			Expect(1);
			nextType.SubType = new TypeReference(t);
			nextType.SubType.Name = t.val;
			
			if (la.kind == _lt && IsPartOfMemberName()) {
				TypeArgumentList(typeRef.Arguments);
			}
		}
	}

	void PropertyDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td) {
		m.Check(Modifier.propEvntMeths);
		if (typeRef.Kind == TypeKind.@void) { Error("property type must not be void"); }
		PropertyDeclaration pd = new PropertyDeclaration(t);
		pd.SetModifiers(m.Value);
		pd.AssignAttributes(attrs);
		pd.ExplicitName = memberRef;
		pd.ResultingType = typeRef;
		
		Expect(96);
		AccessorDeclarations(pd);
		Expect(111);
		td.Members.Add(pd); 
	}

	void IndexerDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td) {
		m.Check(Modifier.indexers);
		if (typeRef.Kind == TypeKind.@void) { Error("indexer type must not be void"); }
		IndexerDeclaration ind = new IndexerDeclaration(t);
		ind.SetModifiers(m.Value);
		ind.AssignAttributes(attrs);
		if (memberRef != null) 
		{
		  ind.ExplicitName = memberRef;
		  ind.Name = memberRef.FullName;
		}
		else
		{
		  ind.Name = "";
		}
		ind.ResultingType = typeRef;
		
		Expect(68);
		Expect(97);
		FormalParameterList(ind.FormalParameters);
		Expect(112);
		Expect(96);
		AccessorDeclarations(ind);
		Expect(111);
		td.Members.Add(ind); 
	}

	void MethodDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td, bool allowBody) {
		m.Check(Modifier.propEvntMeths);
		MethodDeclaration md = new MethodDeclaration(t);
		td.Members.Add(md);
		md.SetModifiers(m.Value);
		md.AssignAttributes(attrs);
		md.ExplicitName = memberRef;
		md.ResultingType = typeRef;
		TypeParameterCollection typePars = null; 
		
		if (la.kind == 100) {
			TypeParameterList(out typePars);
			md.AssignTypeParameters(typePars); 
		}
		Expect(98);
		if (StartOf(7)) {
			FormalParameterList(md.FormalParameters);
		}
		Expect(113);
		while (la.kind == 1) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			md.ParameterConstraints.Add(constraint); 
		}
		if (la.kind == 96) {
			Block(md);
			if (!allowBody || m.Has(Modifier.@abstract)) { Error("Body declaration is not allowed here!"); } 
			md.HasBody = true;
			
		} else if (la.kind == 114) {
			Get();
			md.HasBody = false; 
		} else SynErr(152);
	}

	void CastOperatorDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		m.Check(Modifier.operators);
		m.CheckMust(Modifier.operatorsMust);
		CastOperatorDeclaration cod = new CastOperatorDeclaration(t);
		cod.Name = "";
		cod.SetModifiers(m.Value);
		cod.AssignAttributes(attrs);
		td.Members.Add(cod);
		TypeReference typeRef;
		
		if (la.kind == 37) {
			Get();
		} else if (la.kind == 27) {
			Get();
			cod.IsExplicit = true; 
		} else SynErr(153);
		Expect(49);
		Type(out typeRef, false);
		if (typeRef.Kind == TypeKind.@void) { Error("cast type must not be void"); } 
		cod.ResultingType = typeRef;
		
		Expect(98);
		FormalParameter fp = new FormalParameter(t);
		Type(out typeRef, false);
		fp.Type = typeRef; 
		Expect(1);
		fp.Name = t.val; 
		Expect(113);
		cod.FormalParameters.Add(fp); 
		if (la.kind == 96) {
			Block(cod);
		} else if (la.kind == 114) {
			Get();
		} else SynErr(154);
	}

	void SingleConstMember(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef) {
		
		Expect(1);
		ConstDeclaration cd = new ConstDeclaration(t); 
		cd.AssignAttributes(attrs);
		cd.SetModifiers(m.Value);
		cd.ResultingType = typeRef;
		cd.Name = t.val;
		
		Expect(85);
		td.Members.Add(cd); 
		Expression expr; 
		Expression(out expr);
		cd.Expression = expr; 
	}

	void EventAccessorDeclarations(EventPropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		
		while (la.kind == 97) {
			Attributes(attrs);
		}
		if ("add".Equals(la.val)) {
			Expect(1);
			accessor = prop.Adder = new AccessorDeclaration(t); 
		} else if ("remove".Equals(la.val)) {
			Expect(1);
			accessor = prop.Remover = new AccessorDeclaration(t); 
		} else if (la.kind == 1) {
			Get();
			Error("add or remove expected"); 
		} else SynErr(155);
		Block(accessor);
		accessor.HasBody = true;
		accessor.AssignAttributes(attrs); 
		
		if (la.kind == 1 || la.kind == 97) {
			attrs = new AttributeCollection(); 
			while (la.kind == 97) {
				Attributes(attrs);
			}
			if ("add".Equals(la.val)) {
				Expect(1);
				if (prop.HasAdder) Error("add already declared");  
				accessor = prop.Adder = new AccessorDeclaration(t);
				
			} else if ("remove".Equals(la.val)) {
				Expect(1);
				if (prop.HasRemover) Error("set already declared");  
				accessor = prop.Remover = new AccessorDeclaration(t);
				
			} else if (la.kind == 1) {
				Get();
				Error("add or remove expected"); 
			} else SynErr(156);
			Block(accessor);
			accessor.HasBody = true;
			accessor.AssignAttributes(attrs); 
			
		}
	}

	void Argument(ArgumentList argList) {
		Argument arg = new Argument(t); 
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				arg.Kind = FormalParameterKind.Ref; 
			} else {
				Get();
				arg.Kind = FormalParameterKind.Out; 
			}
		}
		Expression expr; 
		Expression(out expr);
		if (argList != null) argList.Add(arg); 
	}

	void AccessorDeclarations(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		
		while (la.kind == 97) {
			Attributes(attrs);
		}
		Modifiers am = new Modifiers(this); 
		ModifierList(am);
		am.Check(Modifier.accessorsPossib1, Modifier.accessorsPossib2); 
		if ("get".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t); 
		} else if ("set".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t); 
		} else if (la.kind == 1) {
			Get();
			Error("set or get expected"); 
		} else SynErr(157);
		if (la.kind == 96) {
			Block(accessor);
			accessor.HasBody = true; 
		} else if (la.kind == 114) {
			Get();
			accessor.HasBody = false; 
		} else SynErr(158);
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs);
		
		if (StartOf(18)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 97) {
				Attributes(attrs);
			}
			am = new Modifiers(this); 
			ModifierList(am);
			am.Check(Modifier.accessorsPossib1, Modifier.accessorsPossib2); 
			if ("get".Equals(la.val)) {
				Expect(1);
				if (prop.HasGetter) Error("get already declared");  
				accessor = prop.Getter = new AccessorDeclaration(t);
				
			} else if ("set".Equals(la.val)) {
				Expect(1);
				if (prop.HasSetter) Error("set already declared");  
				accessor = prop.Setter = new AccessorDeclaration(t);
				
			} else if (la.kind == 1) {
				Get();
				Error("set or get expected"); 
			} else SynErr(159);
			if (la.kind == 96) {
				Block(accessor);
				accessor.HasBody = true; 
			} else if (la.kind == 114) {
				Get();
				accessor.HasBody = false; 
			} else SynErr(160);
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs);
			
		}
	}

	void OverloadableOp(out Operator op) {
		op = Operator.plus; 
		switch (la.kind) {
		case 108: {
			Get();
			break;
		}
		case 102: {
			Get();
			op = Operator.minus; 
			break;
		}
		case 106: {
			Get();
			op = Operator.not; 
			break;
		}
		case 115: {
			Get();
			op = Operator.tilde; 
			break;
		}
		case 95: {
			Get();
			op = Operator.inc; 
			break;
		}
		case 88: {
			Get();
			op = Operator.dec; 
			break;
		}
		case 70: {
			Get();
			op = Operator.@true; 
			break;
		}
		case 29: {
			Get();
			op = Operator.@false; 
			break;
		}
		case 116: {
			Get();
			op = Operator.times; 
			break;
		}
		case 127: {
			Get();
			op = Operator.div; 
			break;
		}
		case 128: {
			Get();
			op = Operator.mod; 
			break;
		}
		case 83: {
			Get();
			op = Operator.and; 
			break;
		}
		case 124: {
			Get();
			op = Operator.or; 
			break;
		}
		case 125: {
			Get();
			op = Operator.xor; 
			break;
		}
		case 101: {
			Get();
			op = Operator.lshift; 
			break;
		}
		case 92: {
			Get();
			op = Operator.eq; 
			break;
		}
		case 105: {
			Get();
			op = Operator.neq; 
			break;
		}
		case 93: {
			Get();
			op = Operator.gt; 
			if (la.kind == 93) {
				if (la.pos > t.pos+1) Error("no whitespace allowed in right shift operator"); 
				Get();
				op = Operator.rshift; 
			}
			break;
		}
		case 100: {
			Get();
			op = Operator.lt; 
			break;
		}
		case 94: {
			Get();
			op = Operator.gte; 
			break;
		}
		case 126: {
			Get();
			op = Operator.lte; 
			break;
		}
		default: SynErr(161); break;
		}
	}

	void InterfaceBase(InterfaceDeclaration ifd) {
		Expect(86);
		TypeReference typeRef; 
		TypeName(out typeRef);
		ifd.BaseTypes.Add(typeRef); 
		while (la.kind == 87) {
			Get();
			TypeName(out typeRef);
			ifd.BaseTypes.Add(typeRef); 
		}
	}

	void InterfaceMemberDeclaration(InterfaceDeclaration ifd) {
		Modifiers m = new Modifiers(this);
		TypeReference typeRef;
		AttributeCollection attrs = new AttributeCollection();
		
		FormalParameterCollection pars = new FormalParameterCollection(); 
		while (la.kind == 97) {
			Attributes(attrs);
		}
		if (la.kind == 46) {
			Get();
		}
		if (StartOf(11)) {
			Type(out typeRef, true);
			if (la.kind == 1) {
				Get();
				TypeReference memberRef = new TypeReference(t); 
				memberRef.Name = t.val;
				
				if (la.kind == 98 || la.kind == 100) {
					MethodDeclaration(attrs, m, typeRef, memberRef, ifd, false);
				} else if (la.kind == 96) {
					PropertyDeclaration prop = new PropertyDeclaration(t); 
					ifd.Members.Add(prop); 
					prop.ResultingType = typeRef; 
					prop.ExplicitName = memberRef; 
					prop.AssignAttributes(attrs); 
					prop.SetModifiers(m.Value); 
					Get();
					InterfaceAccessors(prop);
					Expect(111);
				} else SynErr(162);
			} else if (la.kind == 68) {
				m.Check(Modifier.indexers);
				if (typeRef.Kind == TypeKind.@void) { Error("indexer type must not be void"); }
				IndexerDeclaration ind = new IndexerDeclaration(t);
				ind.SetModifiers(m.Value);
				ind.AssignAttributes(attrs);
				ind.Name = "";
				ind.ResultingType = typeRef;
				
				Get();
				Expect(97);
				FormalParameterList(ind.FormalParameters);
				Expect(112);
				Expect(96);
				InterfaceAccessors(ind);
				Expect(111);
			} else SynErr(163);
		} else if (la.kind == 26) {
			InterfaceEventDeclaration(attrs, m, ifd);
		} else SynErr(164);
	}

	void InterfaceAccessors(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		
		while (la.kind == 97) {
			Attributes(attrs);
		}
		if ("get".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t); 
		} else if ("set".Equals(la.val)) {
			Expect(1);
			accessor = prop.Setter = new AccessorDeclaration(t); 
		} else if (la.kind == 1) {
			Get();
			Error("set or get expected"); 
		} else SynErr(165);
		Expect(114);
		accessor.AssignAttributes(attrs); 
		if (la.kind == 1 || la.kind == 97) {
			attrs = new AttributeCollection(); 
			while (la.kind == 97) {
				Attributes(attrs);
			}
			if ("get".Equals(la.val)) {
				Expect(1);
				if (prop.HasGetter) Error("get already declared");  
				accessor = prop.Getter = new AccessorDeclaration(t);
				
			} else if ("set".Equals(la.val)) {
				Expect(1);
				if (prop.HasSetter) Error("set already declared");  
				accessor = prop.Setter = new AccessorDeclaration(t);
				
			} else if (la.kind == 1) {
				Get();
				Error("set or get expected"); 
			} else SynErr(166);
			Expect(114);
			accessor.AssignAttributes(attrs); 
		}
	}

	void InterfaceEventDeclaration(AttributeCollection attrs, Modifiers m, InterfaceDeclaration ifd) {
		TypeReference typeRef; 
		Expect(26);
		Type(out typeRef, false);
		Expect(1);
		FieldDeclaration fd = new FieldDeclaration(t); 
		fd.SetModifiers(m.Value);
		fd.AssignAttributes(attrs);
		fd.ResultingType = typeRef;
		fd.Name = t.val;
		fd.IsEvent = true;
		
		Expect(114);
		ifd.Members.Add(fd); 
	}

	void LocalVariableDeclaration(IBlockOwner block) {
		TypeReference typeRef; 
		Type(out typeRef, false);
		LocalVariableDeclarator(block, typeRef);
		while (la.kind == 87) {
			Get();
			LocalVariableDeclarator(block, typeRef);
		}
	}

	void LocalVariableDeclarator(IBlockOwner block, TypeReference typeRef) {
		Expect(1);
		LocalVariableDeclaration loc = new LocalVariableDeclaration(t); 
		loc.Name = t.val; 
		loc.ResultingType = typeRef; 
		if (block != null) block.Statements.Add(loc); 
		if (la.kind == 85) {
			Get();
			if (StartOf(19)) {
				Initializer init; 
				VariableInitializer(out init);
				loc.Initializer = init; 
			} else if (la.kind == 63) {
				Get();
				StackAllocInitializer saIn = new StackAllocInitializer(t); 
				loc.Initializer = saIn; 
				TypeReference tr; 
				Type(out tr, false);
				saIn.Type = tr; 
				Expression expr; 
				Expect(97);
				Expression(out expr);
				saIn.Expression = expr; 
				Expect(112);
			} else SynErr(167);
		}
	}

	void VariableInitializer(out Initializer init) {
		Expression expr; init = null; 
		if (StartOf(20)) {
			Expression(out expr);
			SimpleInitializer sin = new SimpleInitializer(t); 
			sin.Expression = expr; 
			init = sin; 
		} else if (la.kind == 96) {
			ArrayInitializer arrInit; 
			ArrayInitializer(out arrInit);
			init = arrInit;
		} else SynErr(168);
	}

	void ArrayInitializer(out ArrayInitializer init) {
		init = new ArrayInitializer(t); 
		Initializer arrayInit = null; 
		Expect(96);
		if (StartOf(19)) {
			VariableInitializer(out arrayInit);
			init.Initializers.Add(arrayInit); 
			while (NotFinalComma()) {
				Expect(87);
				VariableInitializer(out arrayInit);
				init.Initializers.Add(arrayInit); 
			}
			if (la.kind == 87) {
				Get();
			}
		}
		Expect(111);
	}

	void Keyword() {
		switch (la.kind) {
		case 6: {
			Get();
			break;
		}
		case 7: {
			Get();
			break;
		}
		case 8: {
			Get();
			break;
		}
		case 9: {
			Get();
			break;
		}
		case 10: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 12: {
			Get();
			break;
		}
		case 13: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 15: {
			Get();
			break;
		}
		case 16: {
			Get();
			break;
		}
		case 17: {
			Get();
			break;
		}
		case 18: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			break;
		}
		case 22: {
			Get();
			break;
		}
		case 23: {
			Get();
			break;
		}
		case 24: {
			Get();
			break;
		}
		case 25: {
			Get();
			break;
		}
		case 26: {
			Get();
			break;
		}
		case 27: {
			Get();
			break;
		}
		case 28: {
			Get();
			break;
		}
		case 29: {
			Get();
			break;
		}
		case 30: {
			Get();
			break;
		}
		case 31: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 33: {
			Get();
			break;
		}
		case 34: {
			Get();
			break;
		}
		case 35: {
			Get();
			break;
		}
		case 36: {
			Get();
			break;
		}
		case 37: {
			Get();
			break;
		}
		case 38: {
			Get();
			break;
		}
		case 39: {
			Get();
			break;
		}
		case 40: {
			Get();
			break;
		}
		case 41: {
			Get();
			break;
		}
		case 42: {
			Get();
			break;
		}
		case 43: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 45: {
			Get();
			break;
		}
		case 46: {
			Get();
			break;
		}
		case 47: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		case 49: {
			Get();
			break;
		}
		case 50: {
			Get();
			break;
		}
		case 51: {
			Get();
			break;
		}
		case 52: {
			Get();
			break;
		}
		case 53: {
			Get();
			break;
		}
		case 54: {
			Get();
			break;
		}
		case 55: {
			Get();
			break;
		}
		case 56: {
			Get();
			break;
		}
		case 57: {
			Get();
			break;
		}
		case 58: {
			Get();
			break;
		}
		case 59: {
			Get();
			break;
		}
		case 60: {
			Get();
			break;
		}
		case 61: {
			Get();
			break;
		}
		case 62: {
			Get();
			break;
		}
		case 63: {
			Get();
			break;
		}
		case 64: {
			Get();
			break;
		}
		case 65: {
			Get();
			break;
		}
		case 66: {
			Get();
			break;
		}
		case 67: {
			Get();
			break;
		}
		case 68: {
			Get();
			break;
		}
		case 69: {
			Get();
			break;
		}
		case 70: {
			Get();
			break;
		}
		case 71: {
			Get();
			break;
		}
		case 72: {
			Get();
			break;
		}
		case 73: {
			Get();
			break;
		}
		case 74: {
			Get();
			break;
		}
		case 75: {
			Get();
			break;
		}
		case 76: {
			Get();
			break;
		}
		case 77: {
			Get();
			break;
		}
		case 78: {
			Get();
			break;
		}
		case 79: {
			Get();
			break;
		}
		case 80: {
			Get();
			break;
		}
		case 81: {
			Get();
			break;
		}
		case 82: {
			Get();
			break;
		}
		default: SynErr(169); break;
		}
	}

	void AttributeArguments(AttributeDeclaration attr) {
		AttributeArgument arg; 
		bool nameFound = false; 
		Expect(98);
		if (StartOf(20)) {
			arg = new AttributeArgument(t); 
			if (IsAssignment()) {
				Expect(1);
				arg.Name = t.val; 
				Expect(85);
				nameFound = true; 
			}
			Expression expr; 
			Expression(out expr);
			arg.Expression = expr; 
			attr.Arguments.Add(arg); 
			while (la.kind == 87) {
				Get();
				arg = new AttributeArgument(t); 
				if (IsAssignment()) {
					Expect(1);
					arg.Name = t.val; 
					Expect(85);
					nameFound = true; 
				} else if (StartOf(20)) {
					if (nameFound) Error("no positional argument after named arguments"); 
				} else SynErr(170);
				Expression(out expr);
				arg.Expression = expr; 
				attr.Arguments.Add(arg); 
			}
		}
		Expect(113);
	}

	void PrimitiveType() {
		if (StartOf(21)) {
			IntegralType();
		} else if (la.kind == 32) {
			Get();
		} else if (la.kind == 23) {
			Get();
		} else if (la.kind == 19) {
			Get();
		} else if (la.kind == 9) {
			Get();
		} else SynErr(171);
	}

	void PointerOrArray(ref TypeReference typeRef) {
		while (IsPointerOrDims()) {
			if (la.kind == 116) {
				Get();
				typeRef.Kind = TypeKind.pointer; 
			} else if (la.kind == 97) {
				Get();
				while (la.kind == 87) {
					Get();
				}
				Expect(112);
				typeRef.Kind = TypeKind.array; 
			} else SynErr(172);
		}
	}

	void TypeArgumentList(TypeArgumentCollection args) {
		TypeReference paramType; 
		Expect(100);
		if (StartOf(11)) {
			Type(out paramType, false);
			args.Add(paramType); 
		}
		while (la.kind == 87) {
			Get();
			if (StartOf(11)) {
				Type(out paramType, false);
				args.Add(paramType); 
			}
		}
		Expect(93);
	}

	void Statement(IBlockOwner block) {
		if (la.kind == _ident && Peek(1).kind == _colon) {
			Expect(1);
			Expect(86);
			Statement(block);
		} else if (la.kind == 17) {
			ConstStatement(block);
		} else if (IsLocalVarDecl()) {
			LocalVariableDeclaration(block);
			Expect(114);
		} else if (StartOf(22)) {
			EmbeddedStatement(block);
		} else SynErr(173);
	}

	void ConstStatement(IBlockOwner block) {
		Expect(17);
		TypeReference typeRef; 
		ConstStatement cs = new ConstStatement(t); 
		Type(out typeRef, false);
		Expect(1);
		cs.Name = t.val; 
		Expect(85);
		Expression expr; 
		Expression(out expr);
		cs.Expression = expr; 
		if (block != null) block.Add(cs); 
		while (la.kind == 87) {
			Get();
			cs = new ConstStatement(t); 
			Expect(1);
			cs.Name = t.val; 
			Expect(85);
			Expression(out expr);
			cs.Expression = expr; 
			if (block != null) block.Add(cs); 
		}
		Expect(114);
	}

	void EmbeddedStatement(IBlockOwner block) {
		if (la.kind == 96) {
			Block(block);
		} else if (la.kind == 114) {
			EmptyStatement(block);
		} else if (la.kind == 15) {
			CheckedBlock(block);
		} else if (la.kind == 75) {
			UncheckedBlock(block);
		} else if (la.kind == 76) {
			UnsafeBlock(block);
		} else if (StartOf(20)) {
			StatementExpression(block);
			Expect(114);
		} else if (la.kind == 36) {
			IfStatement(block);
		} else if (la.kind == 67) {
			SwitchStatement(block);
		} else if (la.kind == 82) {
			WhileStatement(block);
		} else if (la.kind == 22) {
			DoWhileStatement(block);
		} else if (la.kind == 33) {
			ForStatement(block);
		} else if (la.kind == 34) {
			ForEachStatement(block);
		} else if (la.kind == 10) {
			BreakStatement(block);
		} else if (la.kind == 18) {
			ContinueStatement(block);
		} else if (la.kind == 35) {
			GotoStatement(block);
		} else if (la.kind == 58) {
			ReturnStatement(block);
		} else if (la.kind == 69) {
			ThrowStatement(block);
		} else if (la.kind == 71) {
			TryFinallyBlock(block);
		} else if (la.kind == 43) {
			LockStatement(block);
		} else if (la.kind == 78) {
			UsingStatement(block);
		} else if (la.kind == 120) {
			Get();
			if (la.kind == 58) {
				YieldReturnStatement(block);
			} else if (la.kind == 10) {
				YieldBreakStatement(block);
			} else SynErr(174);
			Expect(114);
		} else if (la.kind == 31) {
			FixedStatement(block);
		} else SynErr(175);
	}

	void EmptyStatement(IBlockOwner block) {
		Expect(114);
		if (block != null) block.Add(new EmptyStatement(t)); 
	}

	void CheckedBlock(IBlockOwner block) {
		Expect(15);
		CheckedBlock cb = new CheckedBlock(t);
		if (block != null) block.Add(cb);
		
		Block(cb);
	}

	void UncheckedBlock(IBlockOwner block) {
		Expect(75);
		UncheckedBlock ucb = new UncheckedBlock(t);
		if (block != null) block.Add(ucb);
		
		Block(ucb);
	}

	void UnsafeBlock(IBlockOwner block) {
		Expect(76);
		UnsafeBlock usb = new UnsafeBlock(t);
		if (block != null) block.Add(usb);
		
		Block(usb);
	}

	void StatementExpression(IBlockOwner block) {
		bool isAssignment = assnStartOp[la.kind] || IsTypeCast(); 
		Expression expr = null; 
		Unary(out expr);
		ExpressionStatement es = new ExpressionStatement(t); 
		es.Expression = expr; 
		if (StartOf(23)) {
			AssignmentOperator asgn; 
			AssignmentOperator(out asgn);
			es.Expression = asgn; 
			asgn.LeftOperand = expr; 
			Expression rightExpr; 
			Expression(out rightExpr);
			asgn.RightOperand = rightExpr; 
		} else if (la.kind == 87 || la.kind == 113 || la.kind == 114) {
			if (isAssignment) Error("error in assignment."); 
		} else SynErr(176);
		if (block != null) block.Add(es); 
	}

	void IfStatement(IBlockOwner block) {
		Expect(36);
		IfStatement ifs = new IfStatement(t); 
		Expect(98);
		if (block != null) block.Add(ifs); 
		Expression expr; 
		Expression(out expr);
		ifs.Condition = expr; 
		Expect(113);
		ifs.CreateThenBlock(t); 
		EmbeddedStatement(ifs.ThenStatements);
		if (la.kind == 24) {
			Get();
			ifs.CreateElseBlock(t); 
			EmbeddedStatement(ifs.ElseStatements);
		}
	}

	void SwitchStatement(IBlockOwner block) {
		Expect(67);
		SwitchStatement sws = new SwitchStatement(t); 
		Expect(98);
		Expression expr; 
		Expression(out expr);
		sws.Expression = expr; 
		Expect(113);
		Expect(96);
		while (la.kind == 12 || la.kind == 20) {
			SwitchSection(sws);
		}
		Expect(111);
	}

	void WhileStatement(IBlockOwner block) {
		Expect(82);
		WhileStatement whs = new WhileStatement(t); 
		Expect(98);
		if (block != null) block.Add(whs); 
		Expression expr; 
		Expression(out expr);
		whs.Condition = expr; 
		Expect(113);
		EmbeddedStatement(whs);
	}

	void DoWhileStatement(IBlockOwner block) {
		Expect(22);
		DoWhileStatement whs = new DoWhileStatement(t); 
		EmbeddedStatement(whs);
		if (block != null) block.Add(whs); 
		Expect(82);
		Expect(98);
		Expression expr; 
		Expression(out expr);
		whs.Condition = expr; 
		Expect(113);
		Expect(114);
	}

	void ForStatement(IBlockOwner block) {
		Expect(33);
		ForStatement fs = new ForStatement(t); 
		Expect(98);
		if (block != null) block.Add(fs); 
		if (StartOf(24)) {
			fs.CreateInitializerBlock(t); 
			ForInitializer(fs);
		}
		Expect(114);
		if (StartOf(20)) {
			Expression expr; 
			Expression(out expr);
			fs.Condition = expr; 
		}
		Expect(114);
		if (StartOf(20)) {
			ForIterator(fs);
			fs.CreateIteratorBlock(t); 
		}
		Expect(113);
		EmbeddedStatement(fs);
	}

	void ForEachStatement(IBlockOwner block) {
		Expect(34);
		ForEachStatement fes = new ForEachStatement(t); 
		Expect(98);
		if (block != null) block.Add(fes); 
		TypeReference typeRef; 
		Type(out typeRef, false);
		fes.ItemType = typeRef; 
		Expect(1);
		fes.Name = t.val; 
		Expect(38);
		Expression expr; 
		Expression(out expr);
		fes.Expression = expr; 
		Expect(113);
		EmbeddedStatement(fes);
	}

	void BreakStatement(IBlockOwner block) {
		Expect(10);
		Expect(114);
		if (block != null) block.Add(new BreakStatement(t)); 
	}

	void ContinueStatement(IBlockOwner block) {
		Expect(18);
		Expect(114);
		if (block != null) block.Add(new ContinueStatement(t)); 
	}

	void GotoStatement(IBlockOwner block) {
		Expect(35);
		GotoStatement gs = new GotoStatement(t); 
		if (block != null) block.Add(gs); 
		if (la.kind == 1) {
			Get();
			gs.Name = t.val; 
		} else if (la.kind == 12) {
			Get();
			Expression expr; 
			Expression(out expr);
			gs.LabelExpression = expr; 
		} else if (la.kind == 20) {
			Get();
			gs.Name = t.val; 
		} else SynErr(177);
		Expect(114);
	}

	void ReturnStatement(IBlockOwner block) {
		Expect(58);
		ReturnStatement yrs = new ReturnStatement(t); 
		if (StartOf(20)) {
			Expression expr; 
			Expression(out expr);
			yrs.Expression = expr; 
		}
		Expect(114);
		if (block != null) block.Add(yrs); 
	}

	void ThrowStatement(IBlockOwner block) {
		Expect(69);
		ThrowStatement ts = new ThrowStatement(t); 
		if (StartOf(20)) {
			Expression expr; 
			Expression(out expr);
			ts.Expression = expr; 
		}
		Expect(114);
		if (block != null) block.Add(ts); 
	}

	void TryFinallyBlock(IBlockOwner block) {
		Expect(71);
		TryStatement ts = new TryStatement(t); 
		ts.CreateTryBlock(t);
		if (block != null) block.Add(ts);
		
		Block(ts.TryBlock);
		if (la.kind == 13) {
			CatchClauses(ts);
			if (la.kind == 30) {
				Get();
				ts.CreateFinallyBlock(t); 
				Block(ts.FinallyBlock);
			}
		} else if (la.kind == 30) {
			Get();
			ts.CreateFinallyBlock(t); 
			Block(ts.FinallyBlock);
		} else SynErr(178);
	}

	void LockStatement(IBlockOwner block) {
		Expect(43);
		LockStatement ls = new LockStatement(t); 
		if (block != null) block.Add(ls); 
		Expect(98);
		Expression expr; 
		Expression(out expr);
		ls.Expression = expr; 
		Expect(113);
		EmbeddedStatement(ls);
	}

	void UsingStatement(IBlockOwner block) {
		Expect(78);
		UsingStatement us = new UsingStatement(t);
		if (block != null) block.Add(us);
		
		Expect(98);
		if (IsLocalVarDecl()) {
			us.CreateResourceDeclarations(t); 
			LocalVariableDeclaration(us.ResourceDeclarations);
		} else if (StartOf(20)) {
			Expression expr; 
			Expression(out expr);
			us.ResourceExpression = expr; 
		} else SynErr(179);
		Expect(113);
		EmbeddedStatement(us);
	}

	void YieldReturnStatement(IBlockOwner block) {
		Expect(58);
		Expression expr; 
		Expression(out expr);
		YieldReturnStatement yrs = new YieldReturnStatement(t); 
		yrs.Expression = expr; 
		if (block != null) block.Add(yrs); 
	}

	void YieldBreakStatement(IBlockOwner block) {
		Expect(10);
		if (block != null) block.Add(new YieldBreakStatement(t)); 
	}

	void FixedStatement(IBlockOwner block) {
		Expect(31);
		FixedStatement fs = new FixedStatement(t); 
		if (block != null) block.Add(fs); 
		Expect(98);
		TypeReference typeRef; 
		Type(out typeRef, false);
		if (typeRef.Kind != TypeKind.pointer) Error("can only fix pointer types"); 
		ValueAssignmentStatement vas = new ValueAssignmentStatement(t); 
		Expect(1);
		vas.Name = t.val; 
		Expect(85);
		Expression expr; 
		Expression(out expr);
		vas.Expression = expr; 
		fs.Assignments.Add(vas); 
		while (la.kind == 87) {
			Get();
			vas = new ValueAssignmentStatement(t); 
			Expect(1);
			vas.Name = t.val; 
			Expect(85);
			Expression(out expr);
			vas.Expression = expr; 
			fs.Assignments.Add(vas); 
		}
		Expect(113);
		EmbeddedStatement(fs);
	}

	void SwitchSection(SwitchStatement sws) {
		SwitchSection section = sws.CreateSwitchSection(t);
		Expression expr;
		
		SwitchLabel(out expr);
		if (expr == null) section.IsDefault = true; 
		else section.Labels.Add(expr);
		
		while (la.kind == _case || (la.kind == _default && Peek(1).kind == _colon)) {
			SwitchLabel(out expr);
			if (expr == null) section.IsDefault = true; 
			else section.Labels.Add(expr);
			
		}
		Statement(section);
		while (IsNoSwitchLabelOrRBrace()) {
			Statement(section);
		}
	}

	void ForInitializer(ForStatement fs) {
		if (IsLocalVarDecl()) {
			LocalVariableDeclaration(fs.InitializerBlock);
		} else if (StartOf(20)) {
			StatementExpression(fs.InitializerBlock);
			while (la.kind == 87) {
				Get();
				StatementExpression(fs.InitializerBlock);
			}
		} else SynErr(180);
	}

	void ForIterator(ForStatement fs) {
		StatementExpression(fs.IteratorBlock);
		while (la.kind == 87) {
			Get();
			StatementExpression(fs.IteratorBlock);
		}
	}

	void CatchClauses(TryStatement tryStm) {
		Expect(13);
		CatchClause cc = tryStm.CreateCatchClause(t); 
		if (la.kind == 96) {
			Block(cc);
		} else if (la.kind == 98) {
			Get();
			TypeReference typeRef; 
			ClassType(out typeRef);
			cc.ExceptionType = typeRef; 
			if (la.kind == 1) {
				Get();
				cc.Name = t.val; 
			}
			Expect(113);
			Block(cc);
			if (la.kind == 13) {
				CatchClauses(tryStm);
			}
		} else SynErr(181);
	}

	void Unary(out Expression expr) {
		UnaryOperator unOp = null;
		expr = null;
		
		if (unaryHead[la.kind] || IsTypeCast()) {
			switch (la.kind) {
			case 108: {
				Get();
				unOp = new UnaryPlusOperator(t); 
				break;
			}
			case 102: {
				Get();
				unOp = new UnaryMinusOperator(t); 
				break;
			}
			case 106: {
				Get();
				unOp = new NotOperator(t); 
				break;
			}
			case 115: {
				Get();
				unOp = new BitwiseNotOperator(t); 
				break;
			}
			case 95: {
				Get();
				unOp = new PreIncrementOperator(t); 
				break;
			}
			case 88: {
				Get();
				unOp = new PreDecrementOperator(t); 
				break;
			}
			case 116: {
				Get();
				unOp = new PointerOperator(t); 
				break;
			}
			case 83: {
				Get();
				unOp = new ReferenceOperator(t); 
				break;
			}
			case 98: {
				Get();
				TypeReference typeRef; 
				Type(out typeRef, false);
				Expect(113);
				TypeCastOperator tcOp = new TypeCastOperator(t); 
				tcOp.Type = typeRef; 
				unOp = tcOp; 
				break;
			}
			default: SynErr(182); break;
			}
			Expression unaryExpr; 
			Unary(out unaryExpr);
			if (unOp == null) expr = unaryExpr;
			else
			{
			  unOp.Operand = unaryExpr;
			  expr = unOp;
			}
			
		} else if (StartOf(25)) {
			Primary(out expr);
		} else SynErr(183);
	}

	void AssignmentOperator(out AssignmentOperator op) {
		op = null; 
		switch (la.kind) {
		case 85: {
			Get();
			op = new AssignmentOperator(t); 
			break;
		}
		case 109: {
			Get();
			op = new PlusAssignmentOperator(t); 
			break;
		}
		case 103: {
			Get();
			op = new MinusAssignmentOperator(t); 
			break;
		}
		case 117: {
			Get();
			op = new MultiplyAssignmentOperator(t); 
			break;
		}
		case 89: {
			Get();
			op = new DivideAssignmentOperator(t); 
			break;
		}
		case 104: {
			Get();
			op = new ModuloAssignmentOperator(t); 
			break;
		}
		case 84: {
			Get();
			op = new AndAssignmentOperator(t); 
			break;
		}
		case 107: {
			Get();
			op = new OrAssignmentOperator(t); 
			break;
		}
		case 118: {
			Get();
			op = new XorAssignmentOperator(t); 
			break;
		}
		case 99: {
			Get();
			op = new LeftShiftAssignmentOperator(t); 
			break;
		}
		case 93: {
			Get();
			int pos = t.pos; 
			Expect(94);
			if (pos+1 < t.pos) Error("no whitespace allowed in right shift assignment"); 
			op = new RightShiftAssignmentOperator(t); 
			break;
		}
		default: SynErr(184); break;
		}
	}

	void SwitchLabel(out Expression expr) {
		expr = null; 
		if (la.kind == 12) {
			Get();
			Expression(out expr);
			Expect(86);
		} else if (la.kind == 20) {
			Get();
			Expect(86);
		} else SynErr(185);
	}

	void NullCoalescingExpr(out BinaryOperator expr) {
		expr = null; 
		OrExpr(out expr);
		while (la.kind == 121) {
			Get();
			BinaryOperator oper = new NullCoalescingOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			OrExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void OrExpr(out BinaryOperator expr) {
		expr = null; 
		AndExpr(out expr);
		while (la.kind == 122) {
			Get();
			BinaryOperator oper = new OrOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			AndExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void AndExpr(out BinaryOperator expr) {
		expr = null; 
		BitOrExpr(out expr);
		while (la.kind == 123) {
			Get();
			BinaryOperator oper = new AndOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			BitOrExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void BitOrExpr(out BinaryOperator expr) {
		expr = null; 
		BitXorExpr(out expr);
		while (la.kind == 124) {
			Get();
			BinaryOperator oper = new BitwiseOrOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			BitXorExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void BitXorExpr(out BinaryOperator expr) {
		expr = null; 
		BitAndExpr(out expr);
		while (la.kind == 125) {
			Get();
			BinaryOperator oper = new BitwiseXorOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			BitAndExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void BitAndExpr(out BinaryOperator expr) {
		expr = null; 
		EqlExpr(out expr);
		while (la.kind == 83) {
			Get();
			BinaryOperator oper = new BitwiseAndOperator(t); 
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			EqlExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void EqlExpr(out BinaryOperator expr) {
		expr = null; 
		RelExpr(out expr);
		BinaryOperator oper; 
		while (la.kind == 92 || la.kind == 105) {
			if (la.kind == 105) {
				Get();
				oper = new EqualOperator(t); 
			} else {
				Get();
				oper = new NotEqualOperator(t); 
			}
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			RelExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void RelExpr(out BinaryOperator expr) {
		expr = null; 
		ShiftExpr(out expr);
		BinaryOperator oper = null; 
		while (StartOf(26)) {
			if (StartOf(27)) {
				if (la.kind == 100) {
					Get();
					oper = new LessThanOperator(t); 
				} else if (la.kind == 93) {
					Get();
					oper = new GreaterThanOperator(t); 
				} else if (la.kind == 126) {
					Get();
					oper = new LessThanOrEqualOperator(t); 
				} else if (la.kind == 94) {
					Get();
					oper = new GreaterThanOrEqualOperator(t); 
				} else SynErr(186);
				oper.LeftOperand = expr; 
				Expression unExpr; 
				Unary(out unExpr);
				BinaryOperator rightExpr; 
				ShiftExpr(out rightExpr);
				if (rightExpr == null) 
				{
				  oper.RightOperand = unExpr;
				}
				else
				{
				  oper.RightOperand = rightExpr;
				  rightExpr.LeftOperand = unExpr;
				}
				expr = oper;
				
			} else {
				if (la.kind == 42) {
					Get();
					oper = new IsOperator(t); 
				} else if (la.kind == 7) {
					Get();
					oper = new IsOperator(t); 
				} else SynErr(187);
				oper.LeftOperand = expr; 
				TypeReference typeRef; 
				Type(out typeRef, false);
				oper.RightOperand = new TypeOperator(t, typeRef); 
				expr = oper; 
			}
		}
	}

	void ShiftExpr(out BinaryOperator expr) {
		expr = null; 
		AddExpr(out expr);
		BinaryOperator oper = null; 
		while (IsShift()) {
			if (la.kind == 101) {
				Get();
				oper = new LeftShiftOperator(t); 
			} else if (la.kind == 93) {
				Get();
				Expect(93);
				oper = new RightShiftOperator(t); 
			} else SynErr(188);
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			AddExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void AddExpr(out BinaryOperator expr) {
		expr = null; 
		MulExpr(out expr);
		BinaryOperator oper = null; 
		while (la.kind == 102 || la.kind == 108) {
			if (la.kind == 108) {
				Get();
				oper = new AddOperator(t); 
			} else {
				Get();
				oper = new SubtractOperator(t); 
			}
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			BinaryOperator rightExpr; 
			MulExpr(out rightExpr);
			if (rightExpr == null) 
			{
			  oper.RightOperand = unExpr;
			}
			else
			{
			  oper.RightOperand = rightExpr;
			  rightExpr.LeftOperand = unExpr;
			}
			expr = oper;
			
		}
	}

	void MulExpr(out BinaryOperator expr) {
		expr = null;
		BinaryOperator oper = null; 
		
		while (la.kind == 116 || la.kind == 127 || la.kind == 128) {
			if (la.kind == 116) {
				Get();
				oper = new MultiplyOperator(t); 
			} else if (la.kind == 127) {
				Get();
				oper = new DivideOperator(t); 
			} else {
				Get();
				oper = new ModuloOperator(t); 
			}
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			oper.RightOperand = unExpr; 
			expr = oper; 
		}
	}

	void Primary(out Expression expr) {
		TypeReference type = new TypeReference(t); 
		Expression innerExpr = null;
		expr = null;
		
		switch (la.kind) {
		case 2: case 3: case 4: case 5: case 29: case 47: case 70: {
			Literal(out innerExpr);
			break;
		}
		case 98: {
			Get();
			Expression(out innerExpr);
			Expect(113);
			if (innerExpr != null) innerExpr.BracketsUsed = true; 
			break;
		}
		case 9: case 11: case 14: case 19: case 23: case 32: case 39: case 44: case 48: case 59: case 61: case 65: case 73: case 74: case 77: {
			PrimitiveNamedLiteral(out innerExpr);
			break;
		}
		case 1: {
			NamedLiteral(out innerExpr);
			break;
		}
		case 68: {
			Get();
			innerExpr = new ThisLiteral(t); 
			break;
		}
		case 8: {
			Get();
			if (la.kind == 90) {
				BaseNamedLiteral(out expr);
			} else if (la.kind == 97) {
				BaseIndexerOperator(out expr);
			} else SynErr(189);
			break;
		}
		case 46: {
			NewOperator(out innerExpr);
			break;
		}
		case 72: {
			TypeOfOperator(out innerExpr);
			break;
		}
		case 15: {
			CheckedOperator(out innerExpr);
			break;
		}
		case 75: {
			UncheckedOperator(out innerExpr);
			break;
		}
		case 20: {
			DefaultOperator(out innerExpr);
			break;
		}
		case 21: {
			AnonymousDelegate(out innerExpr);
			break;
		}
		case 62: {
			SizeOfOperator(out innerExpr);
			break;
		}
		default: SynErr(190); break;
		}
		Expression curExpr = innerExpr; 
		while (StartOf(28)) {
			switch (la.kind) {
			case 95: {
				Get();
				curExpr = new PostIncrementOperator(t, innerExpr); 
				break;
			}
			case 88: {
				Get();
				curExpr = new PostDecrementOperator(t, innerExpr); 
				break;
			}
			case 129: {
				Get();
				NamedLiteral nl; 
				SimpleNamedLiteral(out nl);
				curExpr = new CTypeMemberAccessOperator(t, innerExpr, nl); 
				break;
			}
			case 90: {
				Get();
				NamedLiteral nl; 
				SimpleNamedLiteral(out nl);
				curExpr = new MemberAccessOperator(t, innerExpr, nl); 
				break;
			}
			case 98: {
				Get();
				ArgumentListOperator alop = new ArgumentListOperator(t, innerExpr); 
				if (StartOf(17)) {
					Argument(alop.Arguments);
					while (la.kind == 87) {
						Get();
						Argument(alop.Arguments);
					}
				}
				Expect(113);
				curExpr = alop; 
				break;
			}
			case 97: {
				ArrayIndexerOperator aiop = new ArrayIndexerOperator(t, innerExpr); 
				ArrayIndexer(aiop);
				curExpr = aiop; 
				break;
			}
			}
		}
		expr = curExpr; 
	}

	void Literal(out Expression value) {
		value = null; 
		switch (la.kind) {
		case 2: {
			Get();
			value = IntegerConstant.Create(t); 
			break;
		}
		case 3: {
			Get();
			value = RealConstant.Create(t); 
			break;
		}
		case 4: {
			Get();
			value = new CharLiteral(t); 
			break;
		}
		case 5: {
			Get();
			value = new StringLiteral(t); 
			break;
		}
		case 70: {
			Get();
			value = new TrueLiteral(t); 
			break;
		}
		case 29: {
			Get();
			value = new FalseLiteral(t); 
			break;
		}
		case 47: {
			Get();
			value = new NullLiteral(t); 
			break;
		}
		default: SynErr(191); break;
		}
	}

	void PrimitiveNamedLiteral(out Expression expr) {
		expr = null; 
		switch (la.kind) {
		case 9: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 23: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 39: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		case 59: {
			Get();
			break;
		}
		case 61: {
			Get();
			break;
		}
		case 65: {
			Get();
			break;
		}
		case 73: {
			Get();
			break;
		}
		case 74: {
			Get();
			break;
		}
		case 77: {
			Get();
			break;
		}
		default: SynErr(192); break;
		}
		PrimitiveNamedLiteral pml = new PrimitiveNamedLiteral(t); 
		expr = pml; 
		pml.Type = new TypeReference(t); 
		Expect(90);
		Expect(1);
		pml.Name = t.val; 
		if (IsGeneric()) {
			TypeArgumentList(pml.TypeArguments);
		}
	}

	void NamedLiteral(out Expression expr) {
		expr = null; 
		Expect(1);
		NamedLiteral nl = new NamedLiteral(t); 
		expr = nl; 
		nl.Name = t.val; 
		if (la.kind == 91) {
			Get();
			nl.IsGlobal = true; 
			Expect(1);
			nl.Name = t.val; 
		}
		if (IsGeneric()) {
			TypeArgumentList(nl.TypeArguments);
		}
	}

	void BaseNamedLiteral(out Expression expr) {
		expr = null; 
		Expect(90);
		Expect(1);
		BaseNamedLiteral bnl = new BaseNamedLiteral(t); 
		bnl.Name = t.val; 
		expr = bnl; 
		if (IsGeneric()) {
			TypeArgumentList(bnl.TypeArguments);
		}
	}

	void BaseIndexerOperator(out Expression expr) {
		Expect(97);
		BaseIndexerOperator bio = new BaseIndexerOperator(t); 
		expr = bio; 
		Expression indexExpr; 
		Expression(out indexExpr);
		bio.Indexes.Add(indexExpr); 
		while (la.kind == 87) {
			Get();
			Expression(out indexExpr);
			bio.Indexes.Add(indexExpr); 
		}
		Expect(112);
	}

	void NewOperator(out Expression expr) {
		ArrayInitializer arrayInit; 
		Expect(46);
		NewOperator nop = new NewOperator(t); 
		expr = nop; 
		TypeReference typeRef; 
		Type(out typeRef, false);
		nop.Type = typeRef; 
		if (la.kind == 98) {
			Get();
			if (StartOf(17)) {
				Argument(nop.Arguments);
				while (la.kind == 87) {
					Get();
					Argument(nop.Arguments);
				}
			}
			Expect(113);
		} else if (la.kind == 97) {
			Get();
			Expression dimExpr; 
			Expression(out dimExpr);
			nop.Dimensions.Add(dimExpr); 
			while (la.kind == 87) {
				Get();
				Expression(out dimExpr);
				nop.Dimensions.Add(dimExpr); 
			}
			Expect(112);
			while (IsDims()) {
				Expect(97);
				nop.RunningDimensions = 1; 
				while (la.kind == 87) {
					Get();
					nop.RunningDimensions++; 
				}
				Expect(112);
			}
			if (la.kind == 96) {
				ArrayInitializer(out arrayInit);
				nop.Initializer = arrayInit; 
			}
		} else if (la.kind == 96) {
			ArrayInitializer(out arrayInit);
			nop.Initializer = arrayInit; 
		} else SynErr(193);
	}

	void TypeOfOperator(out Expression expr) {
		Expect(72);
		TypeOfOperator top = new TypeOfOperator(t); 
		Expect(98);
		expr = top; 
		TypeReference typeRef; 
		Type(out typeRef, true);
		top.Type = typeRef; 
		Expect(113);
	}

	void CheckedOperator(out Expression expr) {
		Expect(15);
		CheckedOperator cop = new CheckedOperator(t); 
		Expect(98);
		expr = cop; 
		Expression innerExpr; 
		Expression(out innerExpr);
		cop.Operand = innerExpr; 
		Expect(113);
	}

	void UncheckedOperator(out Expression expr) {
		Expect(75);
		UncheckedOperator uop = new UncheckedOperator(t); 
		Expect(98);
		expr = uop; 
		Expression innerExpr; 
		Expression(out innerExpr);
		uop.Operand = innerExpr; 
		Expect(113);
	}

	void DefaultOperator(out Expression expr) {
		Expect(20);
		DefaultOperator dop = new DefaultOperator(t); 
		Expect(98);
		expr = dop; 
		Expression innerExpr; 
		Primary(out innerExpr);
		dop.Operand = innerExpr; 
		Expect(113);
	}

	void AnonymousDelegate(out Expression expr) {
		Expect(21);
		AnonymousDelegateOperator adop = new AnonymousDelegateOperator(t); 
		if (la.kind == 98) {
			FormalParameter param; 
			Get();
			if (StartOf(15)) {
				AnonymousMethodParameter(out param);
				adop.FormalParameters.Add(param); 
				while (la.kind == 87) {
					Get();
					AnonymousMethodParameter(out param);
					adop.FormalParameters.Add(param); 
				}
			}
			Expect(113);
		}
		Block(adop);
		expr = adop; 
	}

	void SizeOfOperator(out Expression expr) {
		Expect(62);
		SizeOfOperator sop = new SizeOfOperator(t); 
		Expect(98);
		expr = sop; 
		TypeReference typeRef; 
		Type(out typeRef, true);
		sop.Type = typeRef; 
		Expect(113);
	}

	void SimpleNamedLiteral(out NamedLiteral expr) {
		Expect(1);
		expr = new NamedLiteral(t); 
		if (IsGeneric()) {
			TypeArgumentList(expr.TypeArguments);
		}
	}

	void ArrayIndexer(ArrayIndexerOperator indexer) {
		Expect(97);
		Expression expr; 
		Expression(out expr);
		indexer.Indexers.Add(expr); 
		while (la.kind == 87) {
			Get();
			Expression(out expr);
			indexer.Indexers.Add(expr); 
		}
		Expect(112);
	}

	void AnonymousMethodParameter(out FormalParameter param) {
		param = new FormalParameter(t); 
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				param.Kind = FormalParameterKind.Ref; 
			} else {
				Get();
				param.Kind = FormalParameterKind.Out; 
			}
		}
		TypeReference typeRef; 
		Type(out typeRef, false);
		param.Type = typeRef; 
		Expect(1);
		param.Name = t.val; 
	}

	void SingleFieldMember(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef, bool isEvent) {
		Expect(1);
		FieldDeclaration fd = new FieldDeclaration(t); 
		fd.SetModifiers(m.Value);
		fd.AssignAttributes(attrs);
		fd.ResultingType = typeRef;
		fd.Name = t.val;
		fd.IsEvent = isEvent;
		
		if (la.kind == 85) {
			Get();
			Initializer init; 
			VariableInitializer(out init);
			fd.Initializer = init; 
		}
		td.Members.Add(fd); 
	}

	void TypeParameter(out TypeParameter tp) {
		AttributeCollection attrs = new AttributeCollection(); 
		while (la.kind == 97) {
			Attributes(attrs);
		}
		Expect(1);
		tp = new TypeParameter(t);
		tp.Name = t.val;
		tp.AssignAttributes(attrs);
		
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		CS2();

    Expect(0);
	}
	
	bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,T,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, T,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, x,x,x,x, T,x,x,x, x,T,x,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,T,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, x,x,x,x, T,T,T,x, x,x,x,x, T,T,T,x, x,T,x,x, T,x,T,T, T,T,T,x, T,x,x,x, x,T,T,T, T,T,T,T, T,x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,T,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, T,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,x,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,x,x, x,T,x,x, x,x,x,T, x,x,x,T, T,x,x,T, x,T,x,x, x,x,x,x, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, T,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,T, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
  public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
  
	public void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "intCon expected"; break;
			case 3: s = "realCon expected"; break;
			case 4: s = "charCon expected"; break;
			case 5: s = "stringCon expected"; break;
			case 6: s = "abstract expected"; break;
			case 7: s = "as expected"; break;
			case 8: s = "base expected"; break;
			case 9: s = "bool expected"; break;
			case 10: s = "break expected"; break;
			case 11: s = "byte expected"; break;
			case 12: s = "case expected"; break;
			case 13: s = "catch expected"; break;
			case 14: s = "char expected"; break;
			case 15: s = "checked expected"; break;
			case 16: s = "class expected"; break;
			case 17: s = "const expected"; break;
			case 18: s = "continue expected"; break;
			case 19: s = "decimal expected"; break;
			case 20: s = "default expected"; break;
			case 21: s = "delegate expected"; break;
			case 22: s = "do expected"; break;
			case 23: s = "double expected"; break;
			case 24: s = "else expected"; break;
			case 25: s = "enum expected"; break;
			case 26: s = "event expected"; break;
			case 27: s = "explicit expected"; break;
			case 28: s = "extern expected"; break;
			case 29: s = "false expected"; break;
			case 30: s = "finally expected"; break;
			case 31: s = "fixed expected"; break;
			case 32: s = "float expected"; break;
			case 33: s = "for expected"; break;
			case 34: s = "foreach expected"; break;
			case 35: s = "goto expected"; break;
			case 36: s = "if expected"; break;
			case 37: s = "implicit expected"; break;
			case 38: s = "in expected"; break;
			case 39: s = "int expected"; break;
			case 40: s = "interface expected"; break;
			case 41: s = "internal expected"; break;
			case 42: s = "is expected"; break;
			case 43: s = "lock expected"; break;
			case 44: s = "long expected"; break;
			case 45: s = "namespace expected"; break;
			case 46: s = "new expected"; break;
			case 47: s = "null expected"; break;
			case 48: s = "object expected"; break;
			case 49: s = "operator expected"; break;
			case 50: s = "out expected"; break;
			case 51: s = "override expected"; break;
			case 52: s = "params expected"; break;
			case 53: s = "private expected"; break;
			case 54: s = "protected expected"; break;
			case 55: s = "public expected"; break;
			case 56: s = "readonly expected"; break;
			case 57: s = "ref expected"; break;
			case 58: s = "return expected"; break;
			case 59: s = "sbyte expected"; break;
			case 60: s = "sealed expected"; break;
			case 61: s = "short expected"; break;
			case 62: s = "sizeof expected"; break;
			case 63: s = "stackalloc expected"; break;
			case 64: s = "static expected"; break;
			case 65: s = "string expected"; break;
			case 66: s = "struct expected"; break;
			case 67: s = "switch expected"; break;
			case 68: s = "this expected"; break;
			case 69: s = "throw expected"; break;
			case 70: s = "true expected"; break;
			case 71: s = "try expected"; break;
			case 72: s = "typeof expected"; break;
			case 73: s = "uint expected"; break;
			case 74: s = "ulong expected"; break;
			case 75: s = "unchecked expected"; break;
			case 76: s = "unsafe expected"; break;
			case 77: s = "ushort expected"; break;
			case 78: s = "usingKW expected"; break;
			case 79: s = "virtual expected"; break;
			case 80: s = "void expected"; break;
			case 81: s = "volatile expected"; break;
			case 82: s = "while expected"; break;
			case 83: s = "and expected"; break;
			case 84: s = "andassgn expected"; break;
			case 85: s = "assgn expected"; break;
			case 86: s = "colon expected"; break;
			case 87: s = "comma expected"; break;
			case 88: s = "dec expected"; break;
			case 89: s = "divassgn expected"; break;
			case 90: s = "dot expected"; break;
			case 91: s = "dblcolon expected"; break;
			case 92: s = "eq expected"; break;
			case 93: s = "gt expected"; break;
			case 94: s = "gteq expected"; break;
			case 95: s = "inc expected"; break;
			case 96: s = "lbrace expected"; break;
			case 97: s = "lbrack expected"; break;
			case 98: s = "lpar expected"; break;
			case 99: s = "lshassgn expected"; break;
			case 100: s = "lt expected"; break;
			case 101: s = "ltlt expected"; break;
			case 102: s = "minus expected"; break;
			case 103: s = "minusassgn expected"; break;
			case 104: s = "modassgn expected"; break;
			case 105: s = "neq expected"; break;
			case 106: s = "not expected"; break;
			case 107: s = "orassgn expected"; break;
			case 108: s = "plus expected"; break;
			case 109: s = "plusassgn expected"; break;
			case 110: s = "question expected"; break;
			case 111: s = "rbrace expected"; break;
			case 112: s = "rbrack expected"; break;
			case 113: s = "rpar expected"; break;
			case 114: s = "scolon expected"; break;
			case 115: s = "tilde expected"; break;
			case 116: s = "times expected"; break;
			case 117: s = "timesassgn expected"; break;
			case 118: s = "xorassgn expected"; break;
			case 119: s = "\"partial\" expected"; break;
			case 120: s = "\"yield\" expected"; break;
			case 121: s = "\"??\" expected"; break;
			case 122: s = "\"||\" expected"; break;
			case 123: s = "\"&&\" expected"; break;
			case 124: s = "\"|\" expected"; break;
			case 125: s = "\"^\" expected"; break;
			case 126: s = "\"<=\" expected"; break;
			case 127: s = "\"/\" expected"; break;
			case 128: s = "\"%\" expected"; break;
			case 129: s = "\"->\" expected"; break;
			case 130: s = "??? expected"; break;
			case 131: s = "invalid NamespaceMemberDeclaration"; break;
			case 132: s = "invalid Attributes"; break;
			case 133: s = "invalid TypeDeclaration"; break;
			case 134: s = "invalid TypeDeclaration"; break;
			case 135: s = "invalid TypeParameterConstraintsClause"; break;
			case 136: s = "invalid TypeParameterConstraintsClause"; break;
			case 137: s = "invalid ClassType"; break;
			case 138: s = "invalid ClassMemberDeclaration"; break;
			case 139: s = "invalid ClassMemberDeclaration"; break;
			case 140: s = "invalid StructMemberDeclaration"; break;
			case 141: s = "invalid StructMemberDeclaration"; break;
			case 142: s = "invalid StructMemberDeclaration"; break;
			case 143: s = "invalid IntegralType"; break;
			case 144: s = "invalid Expression"; break;
			case 145: s = "invalid Type"; break;
			case 146: s = "invalid FormalParameterList"; break;
			case 147: s = "invalid EventDeclaration"; break;
			case 148: s = "invalid ConstructorDeclaration"; break;
			case 149: s = "invalid ConstructorDeclaration"; break;
			case 150: s = "invalid OperatorDeclaration"; break;
			case 151: s = "invalid OperatorDeclaration"; break;
			case 152: s = "invalid MethodDeclaration"; break;
			case 153: s = "invalid CastOperatorDeclaration"; break;
			case 154: s = "invalid CastOperatorDeclaration"; break;
			case 155: s = "invalid EventAccessorDeclarations"; break;
			case 156: s = "invalid EventAccessorDeclarations"; break;
			case 157: s = "invalid AccessorDeclarations"; break;
			case 158: s = "invalid AccessorDeclarations"; break;
			case 159: s = "invalid AccessorDeclarations"; break;
			case 160: s = "invalid AccessorDeclarations"; break;
			case 161: s = "invalid OverloadableOp"; break;
			case 162: s = "invalid InterfaceMemberDeclaration"; break;
			case 163: s = "invalid InterfaceMemberDeclaration"; break;
			case 164: s = "invalid InterfaceMemberDeclaration"; break;
			case 165: s = "invalid InterfaceAccessors"; break;
			case 166: s = "invalid InterfaceAccessors"; break;
			case 167: s = "invalid LocalVariableDeclarator"; break;
			case 168: s = "invalid VariableInitializer"; break;
			case 169: s = "invalid Keyword"; break;
			case 170: s = "invalid AttributeArguments"; break;
			case 171: s = "invalid PrimitiveType"; break;
			case 172: s = "invalid PointerOrArray"; break;
			case 173: s = "invalid Statement"; break;
			case 174: s = "invalid EmbeddedStatement"; break;
			case 175: s = "invalid EmbeddedStatement"; break;
			case 176: s = "invalid StatementExpression"; break;
			case 177: s = "invalid GotoStatement"; break;
			case 178: s = "invalid TryFinallyBlock"; break;
			case 179: s = "invalid UsingStatement"; break;
			case 180: s = "invalid ForInitializer"; break;
			case 181: s = "invalid CatchClauses"; break;
			case 182: s = "invalid Unary"; break;
			case 183: s = "invalid Unary"; break;
			case 184: s = "invalid AssignmentOperator"; break;
			case 185: s = "invalid SwitchLabel"; break;
			case 186: s = "invalid RelExpr"; break;
			case 187: s = "invalid RelExpr"; break;
			case 188: s = "invalid ShiftExpr"; break;
			case 189: s = "invalid Primary"; break;
			case 190: s = "invalid Primary"; break;
			case 191: s = "invalid Literal"; break;
			case 192: s = "invalid PrimitiveNamedLiteral"; break;
			case 193: s = "invalid NewOperator"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}

}