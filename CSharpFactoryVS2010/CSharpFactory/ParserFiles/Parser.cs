using System.Text;
using System.Collections;
using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;

using System;

namespace CSharpFactory.ParserFiles {



// ==================================================================================
/// <summary>
/// This class implements the C# syntax parser functionality.
/// </summary>
// ==================================================================================
public partial class CSharpSyntaxParser 
{
  #region These constants represent the grammar elements of the C# syntax.
  
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _intCon = 2;
	public const int _realCon = 3;
	public const int _charCon = 4;
	public const int _stringCon = 5;
	public const int _abstract = 6;
	public const int _as = 7;
	public const int _base = 8;
	public const int _bool = 9;
	public const int _break = 10;
	public const int _byte = 11;
	public const int _case = 12;
	public const int _catch = 13;
	public const int _char = 14;
	public const int _checked = 15;
	public const int _class = 16;
	public const int _const = 17;
	public const int _continue = 18;
	public const int _decimal = 19;
	public const int _default = 20;
	public const int _delegate = 21;
	public const int _do = 22;
	public const int _double = 23;
	public const int _else = 24;
	public const int _enum = 25;
	public const int _event = 26;
	public const int _explicit = 27;
	public const int _extern = 28;
	public const int _false = 29;
	public const int _finally = 30;
	public const int _fixed = 31;
	public const int _float = 32;
	public const int _for = 33;
	public const int _foreach = 34;
	public const int _goto = 35;
	public const int _if = 36;
	public const int _implicit = 37;
	public const int _in = 38;
	public const int _int = 39;
	public const int _interface = 40;
	public const int _internal = 41;
	public const int _is = 42;
	public const int _lock = 43;
	public const int _long = 44;
	public const int _namespace = 45;
	public const int _new = 46;
	public const int _null = 47;
	public const int _object = 48;
	public const int _operator = 49;
	public const int _out = 50;
	public const int _override = 51;
	public const int _params = 52;
	public const int _private = 53;
	public const int _protected = 54;
	public const int _public = 55;
	public const int _readonly = 56;
	public const int _ref = 57;
	public const int _return = 58;
	public const int _sbyte = 59;
	public const int _sealed = 60;
	public const int _short = 61;
	public const int _sizeof = 62;
	public const int _stackalloc = 63;
	public const int _static = 64;
	public const int _string = 65;
	public const int _struct = 66;
	public const int _switch = 67;
	public const int _this = 68;
	public const int _throw = 69;
	public const int _true = 70;
	public const int _try = 71;
	public const int _typeof = 72;
	public const int _uint = 73;
	public const int _ulong = 74;
	public const int _unchecked = 75;
	public const int _unsafe = 76;
	public const int _ushort = 77;
	public const int _usingKW = 78;
	public const int _var = 79;
	public const int _virtual = 80;
	public const int _void = 81;
	public const int _volatile = 82;
	public const int _while = 83;
	public const int _and = 84;
	public const int _andassgn = 85;
	public const int _assgn = 86;
	public const int _colon = 87;
	public const int _comma = 88;
	public const int _dec = 89;
	public const int _divassgn = 90;
	public const int _dot = 91;
	public const int _dblcolon = 92;
	public const int _eq = 93;
	public const int _gt = 94;
	public const int _gteq = 95;
	public const int _inc = 96;
	public const int _lbrace = 97;
	public const int _lbrack = 98;
	public const int _lpar = 99;
	public const int _lshassgn = 100;
	public const int _lt = 101;
	public const int _ltlt = 102;
	public const int _minus = 103;
	public const int _minusassgn = 104;
	public const int _modassgn = 105;
	public const int _neq = 106;
	public const int _not = 107;
	public const int _orassgn = 108;
	public const int _plus = 109;
	public const int _plusassgn = 110;
	public const int _question = 111;
	public const int _rbrace = 112;
	public const int _rbrack = 113;
	public const int _rpar = 114;
	public const int _scolon = 115;
	public const int _tilde = 116;
	public const int _times = 117;
	public const int _timesassgn = 118;
	public const int _xorassgn = 119;
	public const int _larrow = 120;
	public const int maxT = 145;
	public const int _ppDefine = 146;
	public const int _ppUndef = 147;
	public const int _ppIf = 148;
	public const int _ppElif = 149;
	public const int _ppElse = 150;
	public const int _ppEndif = 151;
	public const int _ppLine = 152;
	public const int _ppError = 153;
	public const int _ppWarning = 154;
	public const int _ppPragma = 155;
	public const int _ppRegion = 156;
	public const int _ppEndReg = 157;
	public const int _cBlockCom = 158;
	public const int _cLineCom = 159;

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

  /// <summary>Represents the last recognized token.</summary>
  private Token t;

  /// <summary>Represents the lookahead token.</summary>
  private Token la;

  /// <summary>Represents the current distance from the last error.</summary>
  private int errDist = MinimumDistanceOfSeparateErrors;

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
        la = Scanner.Scan();
        if (la.kind <= maxT) { ++errDist; break; }
				if (la.kind == 146) {
				PragmaHandler.AddConditionalDirective(la); 
				}
				if (la.kind == 147) {
				PragmaHandler.RemoveConditionalDirective(la); 
				}
				if (la.kind == 148) {
				PragmaHandler.IfPragma(la); 
				}
				if (la.kind == 149) {
				PragmaHandler.ElifPragma(la); 
				}
				if (la.kind == 150) {
				PragmaHandler.ElsePragma(la); 
				}
				if (la.kind == 151) {
				PragmaHandler.EndifPragma(la); 
				}
				if (la.kind == 152) {
				PragmaHandler.LinePragma(la); 
				}
				if (la.kind == 153) {
				PragmaHandler.ErrorPragma(la); 
				}
				if (la.kind == 154) {
				PragmaHandler.WarningPragma(la); 
				}
				if (la.kind == 155) {
				PragmaHandler.PragmaPragma(la); 
				}
				if (la.kind == 156) {
				PragmaHandler.RegionPragma(la); 
				}
				if (la.kind == 157) {
				PragmaHandler.EndregionPragma(la); 
				}
				if (la.kind == 158) {
				CommentHandler.HandleBlockComment(la); 
				}
				if (la.kind == 159) {
				CommentHandler.HandleLineComment(la); 
				}

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
	  
    #region Token sets

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a BitArray with the specified values set.
    /// </summary>
    /// <param name="values">Values to set in the BitArray</param>
    /// <returns></returns>
    // --------------------------------------------------------------------------------
    static BitArray NewSet(params int[] values)
    {
      BitArray a = new BitArray(maxT);
      foreach (int item in values) a[item] = true;
      return a;
    }

    /// <summary>Tokens representing unary operators</summary>
    private static BitArray unaryOp =
      NewSet(_plus, _minus, _not, _tilde, _inc, _dec, _true, _false);

    /// <summary>Tokens representing type shortcuts</summary>
    private static BitArray typeKW =
      NewSet(_char, _bool, _object, _string, _sbyte, _byte, _short,
             _ushort, _int, _uint, _long, _ulong, _float, _double, _decimal, _var);

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
                     _modassgn, _andassgn, _orassgn, _xorassgn, _lshassgn);
                 // rshassgn: ">" ">="  no whitespace allowed

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Return the n-th token after the current lookahead token. 
    /// </summary>
    /// <param name="n">Number of tokens to skip.</param>
    /// <returns>n-th token after the current lookahead token.</returns>
    // --------------------------------------------------------------------------------
    Token Peek(int n)
    {
      Scanner.ResetPeek();
      Token x = la;
      while (n > 0) { x = Scanner.Peek(); n--; }
      return x;
    }

    #endregion
    
    #region Common methods used from outside

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the pragma is the first token in the current line.
    /// </summary>
    /// <param name="symbol">Token representing the pragma.</param>
    /// <returns>True, if the pragma is the first token; otherwise, false.</returns>
    // --------------------------------------------------------------------------------
    public bool CheckTokenIsFirstInLine(Token symbol)
    {
      int oldPos = Scanner.Buffer.Pos;
      bool wsOnly = true;
      for (int i = symbol.col - 1; i >= 1; i--)
      {
        Scanner.Buffer.Pos = symbol.pos - i;
        int ch = Scanner.Buffer.Peek();
        wsOnly &= (ch == ' ' || (ch >= 9 && ch <= 13));
      }
      Scanner.Buffer.Pos = oldPos;
      return wsOnly;
    }

    #endregion

	  #region Parser methods generated by CoCo
	  
	void CS2() {
		while (IsExternAliasDirective()) {
			PragmaHandler.SignRealToken(); 
			ExternAliasDirective(null, null);
		}
		while (la.kind == 78) {
			UsingDirective(null, null);
		}
		while (IsGlobalAttrTarget()) {
			PragmaHandler.SignRealToken(); 
			GlobalAttributes();
		}
		while (StartOf(1)) {
			PragmaHandler.SignRealToken(); 
			NamespaceMemberDeclaration(null, File, null);
		}
	}

	void ExternAliasDirective(NamespaceFragment parent, NamespaceDeclarationNode parentNode) {
		Token start;
		Token alias;
		Token identifier;
		
		Expect(28);
		Token token = t; 
		// :::
		start = t;
		
		Expect(1);
		if (t.val != "alias") 
		 Error1003(la, "alias"); 
		// :::
		alias = t;
		
		Expect(1);
		ExternalAlias externAlias = new ExternalAlias(token, this);
		externAlias.Name = t.val;
		CurrentElement = externAlias;
		if (parent == null) File.ExternAliases.Add(externAlias); 
		else parent.ExternAliases.Add(externAlias); 
		// :::
		identifier = t;
		
		Expect(115);
		externAlias.Terminate(t); 
		// ::: 
		NamespaceScopeNode nsScope = parentNode == null
		  ? (NamespaceScopeNode)SourceFileNode 
		  : (NamespaceScopeNode)parentNode;
		nsScope.AddExternAlias(start, alias, identifier, t);
		
	}

	void UsingDirective(NamespaceFragment parent, NamespaceDeclarationNode parentNode) {
		Token alias = null;
		Token eq = null;
		TypeOrNamespaceNode nsNode = null;
		
		Expect(78);
		Token start = t;
		string name = String.Empty; 
		TypeReference typeUsed = null;
		PragmaHandler.SignRealToken();
		
		if (IsAssignment()) {
			Expect(1);
			name = t.val; 
			// :::
			alias = t;
			
			Expect(86);
			eq = t; 
		}
		TypeName(out typeUsed, out nsNode);
		Expect(115);
		UsingClause uc = new UsingClause(start, this, name, typeUsed);
		CurrentElement = uc;
		if (parent == null) File.Usings.Add(uc);
		else parent.Usings.Add(uc); 
		uc.Terminate(t);
		// :::
		NamespaceScopeNode nsScope = parentNode == null
		  ? (NamespaceScopeNode)SourceFileNode 
		  : (NamespaceScopeNode)parentNode;
		if (alias == null)
		  nsScope.AddUsing(start, nsNode, t);
		else
		  nsScope.AddUsingWithAlias(start, alias, eq, nsNode, t);
		
	}

	void GlobalAttributes() {
		AttributeNode attrNode;
		
		Expect(98);
		Expect(1);
		if (!"assembly".Equals(t.val) && !"module".Equals(t.val)) 
		 Error("UNDEF", la, "Global attribute target specifier \"assembly\" or \"module\" expected");
		string scope = t.val;
		AttributeDeclaration attr;
		
		Expect(87);
		Attribute(out attr, out attrNode);
		attr.Scope = scope; 
		File.GlobalAttributes.Add(attr);
		CurrentElement = attr;
		
		while (NotFinalComma()) {
			Expect(88);
			Attribute(out attr, out attrNode);
			attr.Scope = scope; 
			File.GlobalAttributes.Add(attr);
			CurrentElement = attr;
			
		}
		if (la.kind == 88) {
			Get();
		}
		Expect(113);
		attr.Terminate(t); 
	}

	void NamespaceMemberDeclaration(NamespaceFragment parent, SourceFile file, 
NamespaceDeclarationNode parentNode) {
		if (la.kind == 45) {
			Get();
			Token startToken = t; 
			// ::: 
			var nsDecl = new NamespaceDeclarationNode(t);
			
			Expect(1);
			StringBuilder sb = new StringBuilder(t.val); 
			// :::
			nsDecl.NameTags.Add(t);
			
			while (la.kind == 91) {
				Get();
				var sepToken = t;
				
				Expect(1);
				sb.Append("."); sb.Append(t.val); 
				// :::
				nsDecl.NameTags.Add(sepToken, t);
				
			}
			NamespaceFragment ns = new NamespaceFragment(startToken, this, sb.ToString(), parent, file); 
			CurrentElement = ns;
			// :::
			NamespaceScopeNode nsScope = parentNode == null
			  ? (NamespaceScopeNode)SourceFileNode 
			  : (NamespaceScopeNode)parentNode;
			nsScope.NamespaceDeclarations.Add(nsDecl);
			
			Expect(97);
			nsDecl.OpenBracket = t;
			
			while (IsExternAliasDirective()) {
				ExternAliasDirective(ns, nsDecl);
			}
			while (la.kind == 78) {
				UsingDirective(ns, nsDecl);
			}
			while (StartOf(1)) {
				NamespaceMemberDeclaration(ns, File, nsDecl);
			}
			Expect(112);
			ns.Terminate(t); 
			// :::
			nsDecl.CloseBracket = t;
			nsDecl.Terminate(t);
			
			if (la.kind == 115) {
				Get();
				ns.Terminate(t); 
				// :::
				nsDecl.Terminate(t);
				
			}
		} else if (StartOf(2)) {
			Modifiers m = new Modifiers(this); 
			TypeDeclaration td;
			AttributeCollection attrs = new AttributeCollection();
			// :::
			AttributeDecorationNode attrNode;
			
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			ModifierList(m);
			TypeDeclaration(attrs, null, m, out td);
			if (td != null)
			{
			  if (parent == null) File.AddTypeDeclaration(td);
			  else parent.AddTypeDeclaration(td);
			}
			
		} else SynErr(146);
	}

	void TypeName(out TypeReference typeRef, out TypeOrNamespaceNode resultNode) {
		resultNode = null;
		Token qualifier = null;
		Token separator = null;
		Token identifier = null;
		TypeArgumentListNode argList = null;
		
		Expect(1);
		typeRef = new TypeReference(t, this);
		typeRef.Name = t.val; 
		TypeReference nextType = typeRef;
		// :::
		qualifier = t;
		
		if (la.kind == 92) {
			Get();
			typeRef.IsGlobalScope = true; 
			// :::
			separator = t;
			
			Expect(1);
			typeRef.Suffix = new TypeReference(t, this);
			typeRef.Suffix.Name = t.val; 
			nextType = typeRef.Suffix;
			// :::
			identifier = t;
			
		}
		if (separator == null)
		{
		  resultNode = new TypeOrNamespaceNode(qualifier);
		  identifier = qualifier;
		}
		else
		{
		  resultNode = new TypeOrNamespaceNode(qualifier, separator);
		}
		
		if (la.kind == 101) {
			TypeArgumentList(nextType.Arguments, out argList);
		}
		resultNode.AddTypeTag(new TypeTagNode(identifier, argList));
		
		while (la.kind == 91) {
			Get();
			separator = t;
			argList = null;
			
			Expect(1);
			nextType.Suffix = new TypeReference(t, this);
			nextType.Suffix.Name = t.val;
			nextType = nextType.Suffix;
			// :::
			identifier = t;
			
			if (la.kind == 101) {
				TypeArgumentList(nextType.Arguments, out argList);
			}
			resultNode.AddTypeTag(new TypeTagContinuationNode(separator, identifier, argList));
			
		}
	}

	void Attribute(out AttributeDeclaration attr, out AttributeNode attrNode) {
		TypeReference typeRef; 
		// :::
		attrNode = new AttributeNode(t);
		TypeOrNamespaceNode nsNode = null;
		AttributeArgumentsNode args;
		
		TypeName(out typeRef, out nsNode);
		attr = new AttributeDeclaration(t, this, typeRef); 
		CurrentElement = attr;
		attrNode.TypeName = nsNode;
		
		if (la.kind == 99) {
			AttributeArguments(attr, out args);
		}
		attr.Terminate(t); 
		// :::
		attrNode.Terminate(t);
		
	}

	void Attributes(AttributeCollection attrs, out AttributeDecorationNode attrNode) {
		string scope = ""; 
		attrNode = null;
		AttributeNode attributeNode;
		
		Expect(98);
		attrNode = new AttributeDecorationNode(t);
		
		if (IsAttrTargSpec()) {
			if (la.kind == 1) {
				Get();
			} else if (StartOf(3)) {
				Keyword();
			} else SynErr(147);
			scope = t.val; 
			// :::
			attrNode.IdentifierToken = t;
			
			Expect(87);
			attrNode.ColonToken = t; 
		}
		AttributeDeclaration attr; 
		Attribute(out attr, out attributeNode);
		attr.Scope = scope;
		attrs.Add(attr);
		// :::
		attrNode.Attributes.Add(attributeNode);
		
		while (la.kind == _comma && Peek(1).kind != _rbrack) {
			Expect(88);
			var separator = t; 
			Attribute(out attr, out attributeNode);
			attr.Scope = scope;
			attrs.Add(attr);
			// :::
			attrNode.Attributes.Add(new AttributeContinuationNode(separator, attributeNode));
			
		}
		if (la.kind == 88) {
			Get();
			attrNode.ClosingSeparator = t;
			
		}
		Expect(113);
		attrNode.Terminate(t);
		
	}

	void ModifierList(Modifiers m) {
		while (StartOf(4)) {
			switch (la.kind) {
			case 46: {
				Get();
				m.Add(Modifier.@new, t); 
				break;
			}
			case 55: {
				Get();
				m.Add(Modifier.@public, t); 
				break;
			}
			case 54: {
				Get();
				m.Add(Modifier.@protected, t); 
				break;
			}
			case 41: {
				Get();
				m.Add(Modifier.@internal, t); 
				break;
			}
			case 53: {
				Get();
				m.Add(Modifier.@private, t); 
				break;
			}
			case 76: {
				Get();
				m.Add(Modifier.@unsafe, t); 
				break;
			}
			case 64: {
				Get();
				m.Add(Modifier.@static, t); 
				break;
			}
			case 56: {
				Get();
				m.Add(Modifier.@readonly, t); 
				break;
			}
			case 82: {
				Get();
				m.Add(Modifier.@volatile, t); 
				break;
			}
			case 80: {
				Get();
				m.Add(Modifier.@virtual, t); 
				break;
			}
			case 60: {
				Get();
				m.Add(Modifier.@sealed, t); 
				break;
			}
			case 51: {
				Get();
				m.Add(Modifier.@override, t); 
				break;
			}
			case 6: {
				Get();
				m.Add(Modifier.@abstract, t); 
				break;
			}
			case 28: {
				Get();
				m.Add(Modifier.@extern, t); 
				break;
			}
			}
		}
	}

	void TypeDeclaration(AttributeCollection attrs, TypeDeclaration parentType, Modifiers m, 
out TypeDeclaration td) {
		td = null; 
		if (StartOf(5)) {
			bool isPartial = false; 
			if (la.kind == 121) {
				Get();
				isPartial = true; 
			}
			if (la.kind == 16) {
				ClassDeclaration(m, parentType, isPartial, out td);
			} else if (la.kind == 66) {
				StructDeclaration(m, parentType, isPartial, out td);
			} else if (la.kind == 40) {
				InterfaceDeclaration(m, parentType, isPartial, out td);
			} else SynErr(148);
		} else if (la.kind == 25) {
			EnumDeclaration(m, parentType, out td);
		} else if (la.kind == 21) {
			DelegateDeclaration(m, parentType, out td);
		} else SynErr(149);
		if (td != null)
		{
		  td.SetModifiers(m.Value); 
		  td.AssignAttributes(attrs);
		  td.Terminate(t);
		}
		
	}

	void ClassDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td) {
		Expect(16);
		ClassDeclaration cd = new ClassDeclaration(t, this, parentType);
		cd.IsPartial = isPartial;
		td = cd;
		CurrentElement = cd;
		
		Expect(1);
		cd.Name = t.val; 
		if (la.kind == 101) {
			TypeParameterList(cd);
		}
		if (la.kind == 87) {
			ClassBase(cd);
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.AddTypeParameterConstraint(constraint); 
		}
		ClassBody(td);
		if (la.kind == 115) {
			Get();
		}
	}

	void StructDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td) {
		Expect(66);
		StructDeclaration sd = new StructDeclaration(t, this, parentType);
		td = sd;
		CurrentElement = sd;
		sd.IsPartial = isPartial;
		TypeReference typeRef;
		
		Expect(1);
		sd.Name = t.val; 
		if (la.kind == 101) {
			TypeParameterList(sd);
		}
		if (la.kind == 87) {
			Get();
			ClassType(out typeRef);
			sd.InterfaceList.Add(typeRef); 
			while (la.kind == 88) {
				Get();
				ClassType(out typeRef);
				sd.InterfaceList.Add(typeRef); 
			}
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.AddTypeParameterConstraint(constraint); 
		}
		StructBody(td);
		if (la.kind == 115) {
			Get();
		}
	}

	void InterfaceDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td) {
		Expect(40);
		InterfaceDeclaration ifd = new InterfaceDeclaration(t, this, parentType);
		CurrentElement = ifd;
		td = ifd;
		ifd.IsPartial = isPartial;
		
		Expect(1);
		ifd.Name = t.val; 
		if (la.kind == 101) {
			TypeParameterList(ifd);
		}
		if (la.kind == 87) {
			InterfaceBase(ifd);
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.AddTypeParameterConstraint(constraint); 
		}
		Expect(97);
		while (StartOf(6)) {
			InterfaceMemberDeclaration(ifd);
		}
		Expect(112);
		if (la.kind == 115) {
			Get();
		}
	}

	void EnumDeclaration(Modifiers m, TypeDeclaration parentType, out TypeDeclaration td) {
		Expect(25);
		EnumDeclaration ed = new EnumDeclaration(t, this, parentType);
		td = ed;
		CurrentElement = ed;
		
		Expect(1);
		ed.Name = t.val; 
		if (la.kind == 87) {
			Get();
			TypeReference typeRef; 
			if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
				ClassType(out typeRef);
				ed.InterfaceList.Add(typeRef); 
			} else if (StartOf(7)) {
				IntegralType(out typeRef);
				ed.InterfaceList.Add(typeRef); 
			} else SynErr(150);
		}
		EnumBody(ed);
		ed.Terminate(t); 
		if (la.kind == 115) {
			Get();
			ed.Terminate(t); 
		}
	}

	void DelegateDeclaration(Modifiers m, TypeDeclaration parentType, out TypeDeclaration td) {
		Expect(21);
		DelegateDeclaration dd = new DelegateDeclaration(t, this, parentType);
		td = dd;
		CurrentElement = dd;
		TypeReference returnType;
		
		Type(out returnType, true);
		dd.ReturnType = returnType; 
		Expect(1);
		dd.Name = t.val; 
		if (la.kind == 101) {
			TypeParameterList(dd);
		}
		Expect(99);
		if (StartOf(8)) {
			FormalParameterList(dd.FormalParameters);
		}
		Expect(114);
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			td.AddTypeParameterConstraint(constraint); 
		}
		Expect(115);
	}

	void TypeParameterList(ITypeParameterOwner td) {
		Expect(101);
		TypeParameter tp; 
		TypeParameter(out tp);
		td.AddTypeParameter(tp); 
		while (la.kind == 88) {
			Get();
			TypeParameter(out tp);
			td.AddTypeParameter(tp); 
		}
		Expect(94);
	}

	void ClassBase(ClassDeclaration cd) {
		Expect(87);
		TypeReference typeRef; 
		ClassType(out typeRef);
		cd.InterfaceList.Add(typeRef); 
		while (la.kind == 88) {
			Get();
			ClassType(out typeRef);
			cd.InterfaceList.Add(typeRef); 
		}
	}

	void TypeParameterConstraintsClause(out TypeParameterConstraint constraint) {
		Expect(125);
		Expect(1);
		constraint = new TypeParameterConstraint(t, this); 
		constraint.Name = t.val;
		
		Expect(87);
		TypeReference typeRef; 
		ConstraintElement element = null;
		
		if (la.kind == 16) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.Class); 
		} else if (la.kind == 66) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.Struct); 
		} else if (la.kind == 46) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.New); 
			Expect(99);
			Expect(114);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			Token elemToken = t; 
			ClassType(out typeRef);
			element = new ConstraintElement(elemToken, this, typeRef); 
		} else SynErr(151);
		constraint.AddConstraintElement(element); 
		while (la.kind == 88) {
			Get();
			if (la.kind == 16) {
				Get();
				element = new ConstraintElement(t, this, ConstraintClassification.Class); 
			} else if (la.kind == 66) {
				Get();
				element = new ConstraintElement(t, this, ConstraintClassification.Struct); 
			} else if (la.kind == 46) {
				Get();
				element = new ConstraintElement(t, this, ConstraintClassification.New); 
				Expect(99);
				Expect(114);
			} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
				Token elemToken = t; 
				ClassType(out typeRef);
				element = new ConstraintElement(elemToken, this, typeRef); 
			} else SynErr(152);
			constraint.AddConstraintElement(element); 
		}
	}

	void ClassBody(TypeDeclaration td) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		AttributeDecorationNode attrNode;
		
		Expect(97);
		while (StartOf(9)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			Modifiers m = new Modifiers(this); 
			ModifierList(m);
			ClassMemberDeclaration(attrs, m, td);
		}
		Expect(112);
	}

	void ClassType(out TypeReference typeRef) {
		typeRef = null; 
		if (la.kind == 1) {
			TypeOrNamespaceNode nsNode = null;
			
			TypeName(out typeRef, out nsNode);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
				typeRef = new TypeReference(t, this, typeof(object)); 
			} else {
				Get();
				typeRef = new TypeReference(t, this, typeof(string)); 
			}
			typeRef.Terminate(t); 
		} else SynErr(153);
	}

	void ClassMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		if (StartOf(10)) {
			StructMemberDeclaration(attrs, m, td);
		} else if (la.kind == 116) {
			Get();
			Expect(1);
			FinalizerDeclaration dd = new FinalizerDeclaration(t, td);
			CurrentElement = dd;
			dd.Name = t.val;
			dd.SetModifiers(m.Value);
			dd.AssignAttributes(attrs);
			
			Expect(99);
			Expect(114);
			if (la.kind == 97) {
				Block(dd);
			} else if (la.kind == 115) {
				Get();
			} else SynErr(154);
			dd.Terminate(t);
			td.AddMember(dd); 
			
		} else SynErr(155);
	}

	void StructBody(TypeDeclaration td) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		AttributeDecorationNode attrNode;
		
		Expect(97);
		while (StartOf(11)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			Modifiers m = new Modifiers(this); 
			ModifierList(m);
			StructMemberDeclaration(attrs, m, td);
		}
		Expect(112);
	}

	void StructMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		TypeReference typeRef;
		TypeReference memberRef; 
		
		if (la.kind == 17) {
			ConstMemberDeclaration(attrs, m, td);
		} else if (la.kind == 26) {
			EventDeclaration(attrs, m, td);
		} else if (la.kind == _ident && Peek(1).kind == _lpar) {
			ConstructorDeclaration(attrs, m, td);
		} else if (IsPartialMethod()) {
			Expect(121);
			Type(out typeRef, true);
			MemberName(out memberRef);
			MethodDeclaration(attrs, m, typeRef, memberRef, td, true);
		} else if (StartOf(12)) {
			Type(out typeRef, true);
			if (la.kind == 49) {
				OperatorDeclaration(attrs, m, typeRef, td);
			} else if (IsFieldDecl()) {
				FieldMemberDeclarators(attrs, m, td, typeRef, false, Modifier.fields);
				Expect(115);
			} else if (la.kind == 1) {
				MemberName(out memberRef);
				if (la.kind == 97) {
					PropertyDeclaration(attrs, m, typeRef, memberRef, td);
				} else if (la.kind == 91) {
					Get();
					IndexerDeclaration(attrs, m, typeRef, memberRef, td);
				} else if (la.kind == 99 || la.kind == 101) {
					MethodDeclaration(attrs, m, typeRef, memberRef, td, true);
				} else SynErr(156);
			} else if (la.kind == 68) {
				IndexerDeclaration(attrs, m, typeRef, null, td);
			} else SynErr(157);
		} else if (la.kind == 27 || la.kind == 37) {
			CastOperatorDeclaration(attrs, m, td);
		} else if (StartOf(13)) {
			TypeDeclaration nestedType; 
			TypeDeclaration(attrs, td, m, out nestedType);
			td.AddTypeDeclaration(nestedType); 
		} else SynErr(158);
	}

	void IntegralType(out TypeReference typeRef) {
		typeRef = null; 
		switch (la.kind) {
		case 59: {
			Get();
			typeRef = new TypeReference(t, this, typeof(sbyte)); 
			break;
		}
		case 11: {
			Get();
			typeRef = new TypeReference(t, this, typeof(byte)); 
			break;
		}
		case 61: {
			Get();
			typeRef = new TypeReference(t, this, typeof(short)); 
			break;
		}
		case 77: {
			Get();
			typeRef = new TypeReference(t, this, typeof(ushort)); 
			break;
		}
		case 39: {
			Get();
			typeRef = new TypeReference(t, this, typeof(int)); 
			break;
		}
		case 73: {
			Get();
			typeRef = new TypeReference(t, this, typeof(uint)); 
			break;
		}
		case 44: {
			Get();
			typeRef = new TypeReference(t, this, typeof(long)); 
			break;
		}
		case 74: {
			Get();
			typeRef = new TypeReference(t, this, typeof(ulong)); 
			break;
		}
		case 14: {
			Get();
			typeRef = new TypeReference(t, this, typeof(char)); 
			break;
		}
		default: SynErr(159); break;
		}
		typeRef.Terminate(t); 
	}

	void EnumBody(EnumDeclaration ed) {
		Expect(97);
		if (la.kind == 1 || la.kind == 98) {
			EnumMemberDeclaration(ed);
			while (NotFinalComma()) {
				Expect(88);
				while (!(la.kind == 0 || la.kind == 1 || la.kind == 98)) {SynErr(160); Get();}
				EnumMemberDeclaration(ed);
			}
			if (la.kind == 88) {
				Get();
			}
		}
		while (!(la.kind == 0 || la.kind == 112)) {SynErr(161); Get();}
		Expect(112);
	}

	void EnumMemberDeclaration(EnumDeclaration ed) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		Expect(1);
		EnumValueDeclaration ev = new EnumValueDeclaration(t, this); 
		CurrentElement = ev;
		Expression expr;
		
		if (la.kind == 86) {
			Get();
			Expression(out expr);
			ev.ValueExpression = expr; 
		}
		ev.AssignAttributes(attrs);
		ed.Values.Add(ev);
		ev.Terminate(t);
		
	}

	void Expression(out Expression expr) {
		expr = null; 
		Expression leftExpr; 
		if (IsQueryExpression()) {
			QueryExpression query = new QueryExpression(t, this); 
			FromClause fromClause; 
			FromClause(out fromClause);
			query.From = fromClause; 
			QueryBody(query.Body);
			expr = query; 
		} else if (IsLambda()) {
			LambdaExpression lambda = new LambdaExpression(t, this); 
			LambdaFunctionSignature(lambda);
			Expect(120);
			LambdaFunctionBody(lambda);
			expr = lambda; 
		} else if (StartOf(14)) {
			Unary(out leftExpr);
			if (assgnOps[la.kind] || (la.kind == _gt && Peek(1).kind == _gteq)) {
				AssignmentOperator asgn; 
				AssignmentOperator(out asgn);
				Expression rightExpr; 
				Expression(out rightExpr);
				asgn.RightOperand = rightExpr; 
				asgn.LeftOperand = leftExpr; 
				expr = asgn; 
			} else if (StartOf(15)) {
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
				
				if (la.kind == 111) {
					ConditionalOperator condExpr = new ConditionalOperator(t, this, expr); 
					expr = condExpr; 
					Get();
					Expression trueExpr; 
					Expression(out trueExpr);
					condExpr.TrueExpression = trueExpr; 
					Expect(87);
					Expression falseExpr; 
					Expression(out falseExpr);
					condExpr.FalseExpression = falseExpr; 
					condExpr.Terminate(t); 
				}
			} else SynErr(162);
		} else SynErr(163);
		if (expr != null) expr.Terminate(t); 
	}

	void Type(out TypeReference typeRef, bool voidAllowed) {
		typeRef = null; 
		if (StartOf(16)) {
			PrimitiveType(out typeRef);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef);
		} else if (la.kind == 81) {
			Get();
			typeRef = new TypeReference(t, this); 
			typeRef.Name = t.val;
			typeRef.IsVoid = true; 
			
		} else SynErr(164);
		if (la.kind == 111) {
			Get();
			typeRef.IsNullable = true; 
		}
		PointerOrArray(typeRef);
		CompilationUnit.AddTypeToFix(typeRef); 
		typeRef.Terminate(t); 
	}

	void FormalParameterList(FormalParameterCollection pars) {
		TypeReference typeRef = null; 
		AttributeCollection attrs = new AttributeCollection();
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		FormalParameter fp = new FormalParameter(t, this); 
		fp.AssignAttributes(attrs);
		
		if (StartOf(17)) {
			if (la.kind == 50 || la.kind == 57 || la.kind == 68) {
				if (la.kind == 57) {
					Get();
					fp.Kind = FormalParameterKind.Ref; 
				} else if (la.kind == 50) {
					Get();
					fp.Kind = FormalParameterKind.Out; 
				} else {
					Get();
					fp.Kind = FormalParameterKind.This; 
				}
			}
			Type(out typeRef, false);
			fp.Type = typeRef; 
			Expect(1);
			fp.Name = t.val; 
			fp.Type = typeRef;
			pars.Add(fp);
			fp.Terminate(t);
			
			if (la.kind == 88) {
				Get();
				FormalParameterList(pars);
			}
		} else if (la.kind == 52) {
			Get();
			fp.HasParams = true; 
			Type(out typeRef, false);
			if (!typeRef.IsArray) { Error("UNDEF", la, "params argument must be an array"); } 
			Expect(1);
			fp.Name = t.val; 
			fp.Type = typeRef; 
			pars.Add(fp); 
			fp.Terminate(t);
			
		} else SynErr(165);
	}

	void Block(IBlockOwner block) {
		CurrentElement = block.Owner as LanguageElement; 
		Expect(97);
		while (StartOf(18)) {
			Statement(block);
		}
		Expect(112);
	}

	void ConstMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		Expect(17);
		TypeReference typeRef; 
		Type(out typeRef, false);
		SingleConstMember(attrs, m, td, typeRef);
		while (la.kind == 88) {
			Get();
			SingleConstMember(attrs, m, td, typeRef);
		}
		Expect(115);
	}

	void EventDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		TypeReference typeRef; 
		// :::
		TypeOrNamespaceNode nsNode = null;
		
		Expect(26);
		Type(out typeRef, false);
		if (IsFieldDecl()) {
			FieldMemberDeclarators(attrs, m, td, typeRef, true, Modifier.propEvntMeths);
			Expect(115);
		} else if (la.kind == 1) {
			TypeReference memberRef; 
			TypeName(out memberRef, out nsNode);
			Expect(97);
			EventPropertyDeclaration ep = new EventPropertyDeclaration(t, td);  
			CurrentElement = ep; 
			ep.ResultingType = typeRef; 
			ep.ExplicitName = memberRef; 
			td.AddMember(ep); 
			EventAccessorDeclarations(ep);
			Expect(112);
			ep.Terminate(t); 
		} else SynErr(166);
	}

	void ConstructorDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		Expect(1);
		ConstructorDeclaration cd = new ConstructorDeclaration(t, td);
		CurrentElement = cd;
		cd.SetModifiers(m.Value);
		cd.AssignAttributes(attrs);
		
		Expect(99);
		if (StartOf(8)) {
			FormalParameterList(cd.FormalParameters);
		}
		Expect(114);
		if (la.kind == 87) {
			Get();
			if (la.kind == 8) {
				Get();
				cd.HasBase = true; 
			} else if (la.kind == 68) {
				Get();
				cd.HasThis = true; 
			} else SynErr(167);
			Expect(99);
			if (StartOf(19)) {
				Argument(cd.BaseArguments);
				while (la.kind == 88) {
					Get();
					Argument(cd.BaseArguments);
				}
			}
			Expect(114);
		}
		if (la.kind == 97) {
			Block(cd);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(168);
		td.AddMember(cd); 
		cd.Terminate(t);
		
	}

	void MemberName(out TypeReference typeRef) {
		TypeArgumentListNode argList = null;
		
		Expect(1);
		typeRef = new TypeReference(t, this);
		typeRef.Name = t.val; 
		TypeReference nextType = typeRef;
		
		if (la.kind == 92) {
			Get();
			typeRef.IsGlobalScope = true; 
			Expect(1);
			typeRef.Suffix = new TypeReference(t, this);
			typeRef.Suffix.Name = t.val; 
			nextType = typeRef.Suffix;
			
		}
		if (la.kind == _lt && IsPartOfMemberName()) {
			TypeArgumentList(typeRef.Arguments, out argList);
		}
		while (la.kind == _dot && Peek(1).kind == _ident) {
			Expect(91);
			Expect(1);
			nextType.Suffix = new TypeReference(t, this);
			nextType.Suffix.Name = t.val;
			nextType = nextType.Suffix;
			
			if (la.kind == _lt && IsPartOfMemberName()) {
				TypeArgumentList(typeRef.Arguments, out argList);
			}
		}
	}

	void MethodDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td, bool allowBody) {
		MethodDeclaration md = new MethodDeclaration(t, td);
		CurrentElement = md;
		md.SetModifiers(m.Value);
		md.AssignAttributes(attrs);
		md.ExplicitName = memberRef;
		md.ResultingType = typeRef;
		
		if (la.kind == 101) {
			TypeParameterList(md);
		}
		Expect(99);
		if (StartOf(8)) {
			FormalParameterList(md.FormalParameters);
		}
		Expect(114);
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintsClause(out constraint);
			md.AddTypeParameterConstraint(constraint); 
		}
		if (la.kind == 97) {
			Block(md);
			if (!allowBody || m.Has(Modifier.@abstract)) { Error("UNDEF", la, "Body declaration is not allowed here!"); } 
			md.HasBody = true;
			
		} else if (la.kind == 115) {
			Get();
			md.HasBody = false; 
		} else SynErr(169);
		td.AddMember(md); 
		md.Terminate(t);
		
	}

	void OperatorDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeDeclaration td) {
		OperatorDeclaration od = new OperatorDeclaration(t, td);
		CurrentElement = od;
		od.SetModifiers(m.Value);
		od.AssignAttributes(attrs);
		od.ResultingType = typeRef;
		Operator op;
		
		Expect(49);
		OverloadableOp(out op);
		od.Operator = op; 
		Expect(99);
		od.Name = op.ToString(); 
		if (StartOf(8)) {
			FormalParameterList(od.FormalParameters);
		}
		Expect(114);
		if (la.kind == 97) {
			Block(od);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(170);
		td.AddMember(od); 
		od.Terminate(t);
		
	}

	void FieldMemberDeclarators(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef, bool isEvent, Modifier toCheck) {
		SingleFieldMember(attrs, m, td, typeRef, isEvent);
		while (la.kind == 88) {
			Get();
			SingleFieldMember(attrs, m, td, typeRef, isEvent);
		}
	}

	void PropertyDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td) {
		PropertyDeclaration pd = new PropertyDeclaration(t, td);
		CurrentElement = pd;
		pd.SetModifiers(m.Value);
		pd.AssignAttributes(attrs);
		pd.ExplicitName = memberRef;
		pd.ResultingType = typeRef;
		
		Expect(97);
		AccessorDeclarations(pd);
		Expect(112);
		td.AddMember(pd); 
		pd.Terminate(t);
		
	}

	void IndexerDeclaration(AttributeCollection attrs, Modifiers m, TypeReference typeRef, 
TypeReference memberRef, TypeDeclaration td) {
		IndexerDeclaration ind = new IndexerDeclaration(t, td);
		CurrentElement = ind;
		ind.SetModifiers(m.Value);
		ind.AssignAttributes(attrs);
		if (memberRef != null) 
		{
		  ind.ExplicitName = memberRef;
		  ind.Name = memberRef.FullName;
		}
		else
		{
		  ind.Name = "this";
		}
		ind.ResultingType = typeRef;
		
		Expect(68);
		Expect(98);
		if (StartOf(8)) {
			FormalParameterList(ind.FormalParameters);
		}
		Expect(113);
		Expect(97);
		AccessorDeclarations(ind);
		Expect(112);
		td.AddMember(ind); 
		ind.Terminate(t);
		
	}

	void CastOperatorDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td) {
		CastOperatorDeclaration cod = new CastOperatorDeclaration(t, td);
		CurrentElement = cod;
		cod.SetModifiers(m.Value);
		cod.AssignAttributes(attrs);
		TypeReference typeRef;
		
		if (la.kind == 37) {
			Get();
		} else if (la.kind == 27) {
			Get();
			cod.IsExplicit = true; 
		} else SynErr(171);
		Expect(49);
		Type(out typeRef, false);
		cod.ResultingType = typeRef;
		cod.Name = typeRef.TailName;
		
		Expect(99);
		if (StartOf(8)) {
			FormalParameterList(cod.FormalParameters);
		}
		Expect(114);
		if (la.kind == 97) {
			Block(cod);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(172);
		td.AddMember(cod); 
		cod.Terminate(t);
		
	}

	void SingleConstMember(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef) {
		
		Expect(1);
		ConstDeclaration cd = new ConstDeclaration(t, td); 
		CurrentElement = cd;
		cd.AssignAttributes(attrs);
		cd.SetModifiers(m.Value);
		cd.ResultingType = typeRef;
		cd.Name = t.val;
		
		Expect(86);
		td.AddMember(cd); 
		Expression expr; 
		Expression(out expr);
		cd.Expression = expr; 
		cd.Terminate(t);
		
	}

	void EventAccessorDeclarations(EventPropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		Modifiers am = new Modifiers(this); 
		ModifierList(am);
		if ("add".Equals(la.val)) {
			Expect(1);
			accessor = prop.Adder = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if ("remove".Equals(la.val)) {
			Expect(1);
			accessor = prop.Remover = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if (la.kind == 1) {
			Get();
			Error("UNDEF", la, "add or remove expected"); 
		} else SynErr(173);
		Block(accessor);
		accessor.Terminate(t);
		accessor.HasBody = true;
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs); 
		
		if (StartOf(20)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			am = new Modifiers(this); 
			ModifierList(am);
			if ("add".Equals(la.val)) {
				Expect(1);
				if (prop.HasAdder) Error("UNDEF", la, "add already declared");  
				accessor = prop.Adder = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if ("remove".Equals(la.val)) {
				Expect(1);
				if (prop.HasRemover) Error("UNDEF", la, "set already declared");  
				accessor = prop.Remover = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if (la.kind == 1) {
				Get();
				Error("UNDEF", la, "add or remove expected"); 
			} else SynErr(174);
			Block(accessor);
			accessor.Terminate(t);
			accessor.HasBody = true;
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs); 
			
		}
	}

	void Argument(ArgumentList argList) {
		Argument arg = new Argument(t, this); 
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
		arg.Expression = expr; 
		if (argList != null) argList.Add(arg); 
		arg.Terminate(t);
		
	}

	void AccessorDeclarations(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		Modifiers am = new Modifiers(this); 
		ModifierList(am);
		if ("get".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if ("set".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if (la.kind == 1) {
			Get();
			Error("UNDEF", la, "set or get expected"); 
		} else SynErr(175);
		if (la.kind == 97) {
			Block(accessor);
			accessor.HasBody = true; 
		} else if (la.kind == 115) {
			Get();
			accessor.HasBody = false; 
		} else SynErr(176);
		accessor.Terminate(t);
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs);
		
		if (StartOf(20)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			am = new Modifiers(this); 
			ModifierList(am);
			if ("get".Equals(la.val)) {
				Expect(1);
				if (prop.HasGetter) Error("UNDEF", la, "get already declared");  
				accessor = prop.Getter = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if ("set".Equals(la.val)) {
				Expect(1);
				if (prop.HasSetter) Error("UNDEF", la, "set already declared");  
				accessor = prop.Setter = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if (la.kind == 1) {
				Get();
				Error("UNDEF", la, "set or get expected"); 
			} else SynErr(177);
			if (la.kind == 97) {
				Block(accessor);
				accessor.HasBody = true; 
			} else if (la.kind == 115) {
				Get();
				accessor.HasBody = false; 
			} else SynErr(178);
			accessor.Terminate(t);
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs);
			
		}
	}

	void OverloadableOp(out Operator op) {
		op = Operator.Plus; 
		switch (la.kind) {
		case 109: {
			Get();
			break;
		}
		case 103: {
			Get();
			op = Operator.Minus; 
			break;
		}
		case 107: {
			Get();
			op = Operator.Not; 
			break;
		}
		case 116: {
			Get();
			op = Operator.BitwiseNot; 
			break;
		}
		case 96: {
			Get();
			op = Operator.Increment; 
			break;
		}
		case 89: {
			Get();
			op = Operator.Decrement; 
			break;
		}
		case 70: {
			Get();
			op = Operator.True; 
			break;
		}
		case 29: {
			Get();
			op = Operator.False; 
			break;
		}
		case 117: {
			Get();
			op = Operator.Multiply; 
			break;
		}
		case 142: {
			Get();
			op = Operator.Divide; 
			break;
		}
		case 143: {
			Get();
			op = Operator.Modulus; 
			break;
		}
		case 84: {
			Get();
			op = Operator.BitwiseAnd; 
			break;
		}
		case 139: {
			Get();
			op = Operator.BitwiseOr; 
			break;
		}
		case 140: {
			Get();
			op = Operator.BitwiseXor; 
			break;
		}
		case 102: {
			Get();
			op = Operator.LeftShift; 
			break;
		}
		case 93: {
			Get();
			op = Operator.Equal; 
			break;
		}
		case 106: {
			Get();
			op = Operator.NotEqual; 
			break;
		}
		case 94: {
			Get();
			op = Operator.GreaterThan; 
			if (la.kind == 94) {
				if (la.pos > t.pos+1) Error("UNDEF", la, "no whitespace allowed in right shift operator"); 
				Get();
				op = Operator.RightShift; 
			}
			break;
		}
		case 101: {
			Get();
			op = Operator.LessThan; 
			break;
		}
		case 95: {
			Get();
			op = Operator.GreaterThanOrEqual; 
			break;
		}
		case 141: {
			Get();
			op = Operator.LessThanOrEqual; 
			break;
		}
		default: SynErr(179); break;
		}
	}

	void InterfaceBase(InterfaceDeclaration ifd) {
		Expect(87);
		TypeReference typeRef; 
		ClassType(out typeRef);
		ifd.InterfaceList.Add(typeRef); 
		while (la.kind == 88) {
			Get();
			ClassType(out typeRef);
			ifd.InterfaceList.Add(typeRef); 
		}
	}

	void InterfaceMemberDeclaration(InterfaceDeclaration ifd) {
		Modifiers m = new Modifiers(this);
		TypeReference typeRef;
		AttributeCollection attrs = new AttributeCollection();
		// :::
		AttributeDecorationNode attrNode;
		
		FormalParameterCollection pars = new FormalParameterCollection(); 
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		ModifierList(m);
		if (StartOf(12)) {
			Type(out typeRef, true);
			if (la.kind == 1) {
				Get();
				TypeReference memberRef = new TypeReference(t, this); 
				memberRef.Name = t.val;
				
				if (la.kind == 99 || la.kind == 101) {
					MethodDeclaration(attrs, m, typeRef, memberRef, ifd, false);
				} else if (la.kind == 97) {
					PropertyDeclaration prop = new PropertyDeclaration(t, ifd); 
					CurrentElement = prop; 
					ifd.AddMember(prop); 
					prop.ResultingType = typeRef; 
					prop.ExplicitName = memberRef; 
					prop.AssignAttributes(attrs); 
					prop.SetModifiers(m.Value); 
					Get();
					InterfaceAccessors(prop);
					Expect(112);
					prop.Terminate(t); 
				} else SynErr(180);
			} else if (la.kind == 68) {
				IndexerDeclaration ind = new IndexerDeclaration(t, ifd);
				CurrentElement =ind;
				ind.SetModifiers(m.Value);
				ind.AssignAttributes(attrs);
				ind.Name = "";
				ind.ResultingType = typeRef;
				
				Get();
				Expect(98);
				if (StartOf(8)) {
					FormalParameterList(ind.FormalParameters);
				}
				Expect(113);
				Expect(97);
				InterfaceAccessors(ind);
				Expect(112);
				ind.Terminate(t); 
			} else SynErr(181);
		} else if (la.kind == 26) {
			InterfaceEventDeclaration(attrs, m, ifd);
		} else SynErr(182);
	}

	void InterfaceAccessors(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		Modifiers am = new Modifiers(this); 
		ModifierList(am);
		if ("get".Equals(la.val)) {
			Expect(1);
			accessor = prop.Getter = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if ("set".Equals(la.val)) {
			Expect(1);
			accessor = prop.Setter = new AccessorDeclaration(t, prop.DeclaringType, prop); 
			CurrentElement = accessor; 
		} else if (la.kind == 1) {
			Get();
			Error("UNDEF", la, "set or get expected"); 
		} else SynErr(183);
		Expect(115);
		accessor.Terminate(t);
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs); 
		
		if (StartOf(20)) {
			attrs = new AttributeCollection(); 
			while (la.kind == 98) {
				Attributes(attrs, out attrNode);
			}
			am = new Modifiers(this); 
			ModifierList(am);
			if ("get".Equals(la.val)) {
				Expect(1);
				if (prop.HasGetter) Error("UNDEF", la, "get already declared");  
				accessor = prop.Getter = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if ("set".Equals(la.val)) {
				Expect(1);
				if (prop.HasSetter) Error("UNDEF", la, "set already declared");  
				accessor = prop.Setter = new AccessorDeclaration(t, prop.DeclaringType, prop);
				CurrentElement = accessor;
				
			} else if (la.kind == 1) {
				Get();
				Error("UNDEF", la, "set or get expected"); 
			} else SynErr(184);
			Expect(115);
			accessor.Terminate(t);
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs); 
			
		}
	}

	void InterfaceEventDeclaration(AttributeCollection attrs, Modifiers m, InterfaceDeclaration ifd) {
		TypeReference typeRef; 
		Expect(26);
		Type(out typeRef, false);
		Expect(1);
		FieldDeclaration fd = new FieldDeclaration(t, ifd); 
		CurrentElement = fd;
		fd.SetModifiers(m.Value);
		fd.AssignAttributes(attrs);
		fd.ResultingType = typeRef;
		fd.Name = t.val;
		fd.IsEvent = true;
		
		Expect(115);
		ifd.AddMember(fd); 
		fd.Terminate(t);
		
	}

	void LocalVariableDeclaration(IBlockOwner block) {
		TypeReference typeRef = null; 
		bool isImplicit = false; 
		
		if (StartOf(12)) {
			Type(out typeRef, false);
		} else if (la.kind == 79) {
			Get();
			isImplicit = true; 
		} else SynErr(185);
		LocalVariableDeclarator(block, typeRef, isImplicit);
		while (la.kind == 88) {
			Get();
			LocalVariableDeclarator(block, typeRef, isImplicit);
		}
	}

	void LocalVariableDeclarator(IBlockOwner block, TypeReference typeRef, bool isImplicit) {
		Expect(1);
		LocalVariableDeclaration loc = new LocalVariableDeclaration(t, this, block); 
		CurrentElement = loc; 
		loc.Name = t.val; 
		loc.Variable.ResultingType = typeRef; 
		loc.Variable.IsImplicit = isImplicit; 
		if (block != null) block.Statements.Add(loc); 
		if (la.kind == 86) {
			Get();
			if (StartOf(21)) {
				Initializer init; 
				VariableInitializer(out init);
				loc.Variable.Initializer = init; 
			} else if (la.kind == 63) {
				Get();
				StackAllocInitializer saIn = new StackAllocInitializer(t, this); 
				loc.Variable.Initializer = saIn; 
				TypeReference tr; 
				Type(out tr, false);
				saIn.Type = tr; 
				Expression expr; 
				Expect(98);
				Expression(out expr);
				saIn.Expression = expr; 
				Expect(113);
				saIn.Terminate(t); 
			} else SynErr(186);
		}
		block.Add(loc.Variable); 
		loc.Terminate(t);
		
	}

	void VariableInitializer(out Initializer init) {
		Expression expr; init = null; 
		if (StartOf(22)) {
			Expression(out expr);
			ExpressionInitializer expIn = new ExpressionInitializer(t, this, expr); 
			init = expIn; expIn.Terminate(t); 
		} else if (la.kind == 97) {
			ArrayInitializer arrInit; 
			ArrayInitializer(out arrInit);
			init = arrInit; 
		} else SynErr(187);
	}

	void ArrayInitializer(out ArrayInitializer init) {
		init = new ArrayInitializer(t, this); 
		Initializer arrayInit = null; 
		Expect(97);
		if (StartOf(21)) {
			VariableInitializer(out arrayInit);
			init.Initializers.Add(arrayInit); 
			while (NotFinalComma()) {
				Expect(88);
				VariableInitializer(out arrayInit);
				init.Initializers.Add(arrayInit); 
			}
			if (la.kind == 88) {
				Get();
			}
		}
		Expect(112);
		init.Terminate(t); 
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
		case 83: {
			Get();
			break;
		}
		default: SynErr(188); break;
		}
	}

	void AttributeArguments(AttributeDeclaration attr, out AttributeArgumentsNode argsNode) {
		AttributeArgument arg; 
		bool nameFound = false; 
		Expect(99);
		argsNode = new AttributeArgumentsNode(t); 
		if (StartOf(22)) {
			arg = new AttributeArgument(t,this); 
			if (IsAssignment()) {
				Expect(1);
				arg.Name = t.val; 
				Expect(86);
				nameFound = true; 
				
			}
			Expression expr; 
			Expression(out expr);
			arg.Expression = expr; 
			attr.Arguments.Add(arg); 
			arg.Terminate(t);
			
			while (la.kind == 88) {
				Get();
				arg = new AttributeArgument(t, this); 
				if (IsAssignment()) {
					Expect(1);
					arg.Name = t.val; 
					Expect(86);
					nameFound = true; 
				} else if (StartOf(22)) {
					if (nameFound) Error("UNDEF", la, "no positional argument after named arguments"); 
				} else SynErr(189);
				Expression(out expr);
				arg.Expression = expr; 
				attr.Arguments.Add(arg); 
				arg.Terminate(t);
				
			}
		}
		Expect(114);
		argsNode.Terminate(t); 
	}

	void PrimitiveType(out TypeReference typeRef) {
		typeRef = null; 
		if (StartOf(7)) {
			IntegralType(out typeRef);
		} else if (StartOf(23)) {
			if (la.kind == 32) {
				Get();
				typeRef = new TypeReference(t, this, typeof(float)); 
			} else if (la.kind == 23) {
				Get();
				typeRef = new TypeReference(t, this, typeof(double)); 
			} else if (la.kind == 19) {
				Get();
				typeRef = new TypeReference(t, this, typeof(decimal)); 
			} else {
				Get();
				typeRef = new TypeReference(t, this, typeof(bool)); 
			}
			typeRef.Terminate(t); 
		} else SynErr(190);
	}

	void PointerOrArray(TypeReference typeRef) {
		while (IsPointerOrDims()) {
			if (la.kind == 117) {
				Get();
				typeRef.TypeModifiers.Add(new PointerModifier()); 
			} else if (la.kind == 98) {
				Get();
				int rank = 1; 
				while (la.kind == 88) {
					Get();
					rank++; 
				}
				Expect(113);
				typeRef.TypeModifiers.Add(new ArrayModifier(rank)); 
			} else SynErr(191);
		}
	}

	void NonArrayType(out TypeReference typeRef) {
		typeRef = null; 
		if (StartOf(16)) {
			PrimitiveType(out typeRef);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef);
		} else SynErr(192);
		if (la.kind == 111) {
			Get();
			typeRef.IsNullable = true; 
		}
		if (la.kind == 117) {
			Get();
			typeRef.TypeModifiers.Add(new PointerModifier()); 
			CompilationUnit.AddTypeToFix(typeRef); 
		}
		typeRef.Terminate(t); 
	}

	void TypeInRelExpr(out TypeReference typeRef, bool voidAllowed) {
		typeRef = null; 
		if (StartOf(16)) {
			PrimitiveType(out typeRef);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef);
		} else if (la.kind == 81) {
			Get();
			typeRef = new TypeReference(t, this); 
			typeRef.Name = t.val;
			typeRef.IsVoid = true; 
			
		} else SynErr(193);
		if (IsNullableTypeMark()) {
			Expect(111);
		}
		PointerOrArray(typeRef);
		CompilationUnit.AddTypeToFix(typeRef); 
		typeRef.Terminate(t); 
	}

	void PredefinedType(out TypeReference typeRef) {
		typeRef = null; 
		if (StartOf(16)) {
			PrimitiveType(out typeRef);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
				typeRef = new TypeReference(t, this, typeof(object)); 
			} else {
				Get();
				typeRef = new TypeReference(t, this, typeof(string)); 
			}
			typeRef.Terminate(t); 
		} else SynErr(194);
	}

	void TypeArgumentList(TypeReferenceCollection args, out TypeArgumentListNode argList) {
		TypeReference paramType; 
		argList = null;
		
		Expect(101);
		paramType = TypeReference.EmptyType; 
		if (StartOf(12)) {
			Type(out paramType, false);
		}
		args.Add(paramType); 
		while (la.kind == 88) {
			Get();
			paramType = TypeReference.EmptyType; 
			if (StartOf(12)) {
				Type(out paramType, false);
			}
			args.Add(paramType); 
		}
		Expect(94);
	}

	void Statement(IBlockOwner block) {
		if (la.kind == _ident && Peek(1).kind == _colon) {
			Expect(1);
			Expect(87);
			Statement(block);
		} else if (la.kind == 17) {
			ConstStatement(block);
		} else if (IsLocalVarDecl()) {
			LocalVariableDeclaration(block);
			Expect(115);
		} else if (StartOf(24)) {
			EmbeddedStatement(block);
		} else SynErr(195);
	}

	void ConstStatement(IBlockOwner block) {
		Expect(17);
		TypeReference typeRef; 
		ConstStatement cs = new ConstStatement(t, this, block); 
		CurrentElement = cs; 
		Type(out typeRef, false);
		Expect(1);
		cs.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr);
		cs.Expression = expr; 
		if (block != null) block.Add(cs); 
		cs.Terminate(t);
		
		while (la.kind == 88) {
			Get();
			cs = new ConstStatement(t, this, block); 
			Expect(1);
			cs.Name = t.val; 
			Expect(86);
			Expression(out expr);
			cs.Expression = expr; 
			if (block != null) block.Add(cs); 
			cs.Terminate(t);
			
		}
		Expect(115);
	}

	void EmbeddedStatement(IBlockOwner block) {
		if (la.kind == 97) {
			BlockStatement embedded = new BlockStatement(t, this, block); 
			Block(embedded);
			if (block != null) block.Add(embedded); 
		} else if (la.kind == 115) {
			EmptyStatement(block);
		} else if (la.kind == 15) {
			CheckedBlock(block);
		} else if (la.kind == 75) {
			UncheckedBlock(block);
		} else if (la.kind == 76) {
			UnsafeBlock(block);
		} else if (StartOf(14)) {
			StatementExpression(block);
			Expect(115);
		} else if (la.kind == 36) {
			IfStatement(block);
		} else if (la.kind == 67) {
			SwitchStatement(block);
		} else if (la.kind == 83) {
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
		} else if (la.kind == 122) {
			Get();
			if (la.kind == 58) {
				YieldReturnStatement(block);
			} else if (la.kind == 10) {
				YieldBreakStatement(block);
			} else SynErr(196);
			Expect(115);
		} else if (la.kind == 31) {
			FixedStatement(block);
		} else SynErr(197);
	}

	void EmptyStatement(IBlockOwner block) {
		Expect(115);
		EmptyStatement es = new EmptyStatement(t, this, block); 
		CurrentElement = es; 
		if (block != null) block.Add(es); 
	}

	void CheckedBlock(IBlockOwner block) {
		Expect(15);
		CheckedBlock cb = new CheckedBlock(t, this, block);
		CurrentElement = cb;
		if (block != null) block.Add(cb);
		
		Block(cb);
		cb.Terminate(t); 
	}

	void UncheckedBlock(IBlockOwner block) {
		Expect(75);
		UncheckedBlock ucb = new UncheckedBlock(t, this, block);
		CurrentElement = ucb;
		if (block != null) block.Add(ucb);
		
		Block(ucb);
		ucb.Terminate(t); 
	}

	void UnsafeBlock(IBlockOwner block) {
		Expect(76);
		UnsafeBlock usb = new UnsafeBlock(t, this, block);
		CurrentElement = usb;
		if (block != null) block.Add(usb);
		
		Block(usb);
		usb.Terminate(t); 
	}

	void StatementExpression(IBlockOwner block) {
		bool isAssignment = assnStartOp[la.kind] || IsTypeCast(); 
		Expression expr = null; 
		Unary(out expr);
		ExpressionStatement es = new ExpressionStatement(t, this, block); 
		CurrentElement = es; 
		es.Expression = expr; 
		if (StartOf(25)) {
			AssignmentOperator asgn; 
			AssignmentOperator(out asgn);
			es.Expression = asgn; 
			asgn.LeftOperand = expr; 
			Expression rightExpr; 
			Expression(out rightExpr);
			asgn.RightOperand = rightExpr; 
		} else if (la.kind == 88 || la.kind == 114 || la.kind == 115) {
			if (isAssignment) Error("UNDEF", la, "error in assignment."); 
		} else SynErr(198);
		if (block != null) block.Add(es); 
		es.Terminate(t); 
	}

	void IfStatement(IBlockOwner block) {
		Expect(36);
		IfStatement ifs = new IfStatement(t, this, block); 
		CurrentElement = ifs; 
		Expect(99);
		if (block != null) block.Add(ifs); 
		Expression expr; 
		Expression(out expr);
		ifs.Condition = expr; 
		Expect(114);
		ifs.CreateThenBlock(t); 
		EmbeddedStatement(ifs.ThenStatements);
		ifs.ThenStatements.Terminate(t); 
		if (la.kind == 24) {
			Get();
			ifs.CreateElseBlock(t); 
			EmbeddedStatement(ifs.ElseStatements);
			ifs.ElseStatements.Terminate(t); 
		}
		ifs.Terminate(t); 
	}

	void SwitchStatement(IBlockOwner block) {
		Expect(67);
		SwitchStatement sws = new SwitchStatement(t, this, block); 
		CurrentElement = sws; 
		Expect(99);
		Expression expr; 
		Expression(out expr);
		sws.Expression = expr; 
		Expect(114);
		Expect(97);
		while (la.kind == 12 || la.kind == 20) {
			SwitchSection(sws);
		}
		Expect(112);
		if (block != null) block.Add(sws); 
		sws.Terminate(t);
		
	}

	void WhileStatement(IBlockOwner block) {
		Expect(83);
		WhileStatement whs = new WhileStatement(t, this, block); 
		CurrentElement = whs; 
		Expect(99);
		if (block != null) block.Add(whs); 
		Expression expr; 
		Expression(out expr);
		whs.Condition = expr; 
		Expect(114);
		EmbeddedStatement(whs);
		whs.Terminate(t); 
	}

	void DoWhileStatement(IBlockOwner block) {
		Expect(22);
		DoWhileStatement whs = new DoWhileStatement(t, this, block); 
		CurrentElement = whs; 
		EmbeddedStatement(whs);
		if (block != null) block.Add(whs); 
		Expect(83);
		Expect(99);
		Expression expr; 
		Expression(out expr);
		whs.Condition = expr; 
		Expect(114);
		Expect(115);
		whs.Terminate(t); 
	}

	void ForStatement(IBlockOwner block) {
		Expect(33);
		ForStatement fs = new ForStatement(t, this, block); 
		CurrentElement = fs; 
		Expect(99);
		if (block != null) block.Add(fs); 
		if (StartOf(26)) {
			fs.CreateInitializerBlock(t); 
			ForInitializer(fs);
		}
		Expect(115);
		if (StartOf(22)) {
			Expression expr; 
			Expression(out expr);
			fs.Condition = expr; 
		}
		Expect(115);
		if (StartOf(14)) {
			ForIterator(fs);
			fs.CreateIteratorBlock(t); 
		}
		Expect(114);
		EmbeddedStatement(fs);
		fs.Terminate(t); 
	}

	void ForEachStatement(IBlockOwner block) {
		Expect(34);
		ForEachStatement fes = new ForEachStatement(t, this, block); 
		CurrentElement = fes; 
		Expect(99);
		if (block != null) block.Add(fes); 
		TypeReference typeRef; 
		if (StartOf(12)) {
			Type(out typeRef, false);
			fes.Variable.ResultingType = typeRef; 
		} else if (la.kind == 79) {
			Get();
			fes.Variable.IsImplicit = true; 
		} else SynErr(199);
		Expect(1);
		fes.Variable.Name = t.val; 
		Expect(38);
		Expression expr; 
		Expression(out expr);
		fes.Expression = expr; 
		Expect(114);
		fes.Add(fes.Variable); 
		EmbeddedStatement(fes);
		fes.Terminate(t); 
	}

	void BreakStatement(IBlockOwner block) {
		Expect(10);
		Expect(115);
		BreakStatement bs = new BreakStatement(t, this, block); 
		CurrentElement = bs; 
		if (block != null) block.Add(bs); 
		bs.Terminate(t); 
	}

	void ContinueStatement(IBlockOwner block) {
		Expect(18);
		Expect(115);
		ContinueStatement cs = new ContinueStatement(t, this, block); 
		CurrentElement = cs; 
		if (block != null) block.Add(cs); 
		cs.Terminate(t); 
	}

	void GotoStatement(IBlockOwner block) {
		Expect(35);
		GotoStatement gs = new GotoStatement(t, this, block); 
		CurrentElement = gs; 
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
		} else SynErr(200);
		Expect(115);
		gs.Terminate(t); 
	}

	void ReturnStatement(IBlockOwner block) {
		Expect(58);
		ReturnStatement yrs = new ReturnStatement(t, this, block); 
		CurrentElement = yrs; 
		if (StartOf(22)) {
			Expression expr; 
			Expression(out expr);
			yrs.Expression = expr; 
		}
		Expect(115);
		if (block != null) block.Add(yrs); 
		yrs.Terminate(t); 
	}

	void ThrowStatement(IBlockOwner block) {
		Expect(69);
		ThrowStatement ts = new ThrowStatement(t, this, block); 
		CurrentElement = ts; 
		if (StartOf(22)) {
			Expression expr; 
			Expression(out expr);
			ts.Expression = expr; 
		}
		Expect(115);
		if (block != null) block.Add(ts); 
		ts.Terminate(t); 
	}

	void TryFinallyBlock(IBlockOwner block) {
		Expect(71);
		TryStatement ts = new TryStatement(t, this, block); 
		CurrentElement = ts;
		ts.CreateTryBlock(t);
		if (block != null) block.Add(ts);
		
		Block(ts.TryBlock);
		if (la.kind == 13) {
			CatchClauses(ts);
			if (la.kind == 30) {
				Get();
				ts.CreateFinallyBlock(t); 
				Block(ts.FinallyBlock);
				ts.FinallyBlock.Terminate(t); 
			}
		} else if (la.kind == 30) {
			Get();
			ts.CreateFinallyBlock(t); 
			Block(ts.FinallyBlock);
			ts.FinallyBlock.Terminate(t); 
		} else SynErr(201);
		ts.Terminate(t); 
	}

	void LockStatement(IBlockOwner block) {
		Expect(43);
		LockStatement ls = new LockStatement(t, this, block); 
		CurrentElement = ls; 
		if (block != null) block.Add(ls); 
		Expect(99);
		Expression expr; 
		Expression(out expr);
		ls.Expression = expr; 
		Expect(114);
		EmbeddedStatement(ls);
		ls.Terminate(t); 
	}

	void UsingStatement(IBlockOwner block) {
		Expect(78);
		UsingStatement us = new UsingStatement(t, this, block);
		CurrentElement = us;
		if (block != null) block.Add(us);
		
		Expect(99);
		if (IsLocalVarDecl()) {
			LocalVariableDeclaration(us);
		} else if (StartOf(22)) {
			Expression expr; 
			Expression(out expr);
			us.ResourceExpression = expr; 
		} else SynErr(202);
		Expect(114);
		EmbeddedStatement(us);
		us.Terminate(t); 
	}

	void YieldReturnStatement(IBlockOwner block) {
		Expect(58);
		Expression expr; 
		Expression(out expr);
		YieldReturnStatement yrs = new YieldReturnStatement(t, this, block); 
		CurrentElement = yrs; 
		yrs.Expression = expr; 
		if (block != null) block.Add(yrs); 
		yrs.Terminate(t); 
	}

	void YieldBreakStatement(IBlockOwner block) {
		Expect(10);
		YieldBreakStatement ybs = new YieldBreakStatement(t, this, block); 
		CurrentElement = ybs; 
		if (block != null) block.Add(ybs); 
		ybs.Terminate(t); 
	}

	void FixedStatement(IBlockOwner block) {
		Expect(31);
		FixedStatement fs = new FixedStatement(t, this, block); 
		CurrentElement = fs; 
		if (block != null) block.Add(fs); 
		Expect(99);
		TypeReference typeRef; 
		Type(out typeRef, false);
		if (!typeRef.IsPointer) Error("UNDEF", la, "can only fix pointer types"); 
		ValueAssignmentStatement vas = new ValueAssignmentStatement(t, this, block); 
		CurrentElement = vas; 
		Expect(1);
		vas.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr);
		vas.Expression = expr; 
		fs.Assignments.Add(vas); 
		while (la.kind == 88) {
			Get();
			vas = new ValueAssignmentStatement(t, this, block); 
			CurrentElement = vas; 
			Expect(1);
			vas.Name = t.val; 
			Expect(86);
			Expression(out expr);
			vas.Expression = expr; 
			fs.Assignments.Add(vas); 
		}
		Expect(114);
		EmbeddedStatement(fs);
		fs.Terminate(t); 
	}

	void SwitchSection(SwitchStatement sws) {
		SwitchSection section = sws.CreateSwitchSection(t);
		CurrentElement = section;
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
		sws.Terminate(t); 
	}

	void ForInitializer(ForStatement fs) {
		if (IsLocalVarDecl()) {
			LocalVariableDeclaration(fs);
		} else if (StartOf(14)) {
			StatementExpression(fs.InitializerBlock);
			while (la.kind == 88) {
				Get();
				StatementExpression(fs.InitializerBlock);
			}
		} else SynErr(203);
	}

	void ForIterator(ForStatement fs) {
		StatementExpression(fs.IteratorBlock);
		while (la.kind == 88) {
			Get();
			StatementExpression(fs.IteratorBlock);
		}
	}

	void CatchClauses(TryStatement tryStm) {
		Expect(13);
		CatchClause cc = tryStm.CreateCatchClause(t); 
		CurrentElement = cc; 
		if (la.kind == 97) {
			Block(cc);
		} else if (la.kind == 99) {
			Get();
			TypeReference typeRef; 
			ClassType(out typeRef);
			cc.ExceptionType = typeRef; 
			if (la.kind == 1) {
				Get();
				cc.Name = t.val; 
				cc.CreateInstanceVariable(typeRef, t.val); 
			}
			Expect(114);
			Block(cc);
			cc.Terminate(t); 
			if (la.kind == 13) {
				CatchClauses(tryStm);
			}
		} else SynErr(204);
	}

	void Unary(out Expression expr) {
		UnaryOperator unOp = null;
		expr = null;
		
		if (unaryHead[la.kind] || IsTypeCast()) {
			switch (la.kind) {
			case 109: {
				Get();
				unOp = new UnaryPlusOperator(t, this); 
				break;
			}
			case 103: {
				Get();
				unOp = new UnaryMinusOperator(t, this); 
				break;
			}
			case 107: {
				Get();
				unOp = new NotOperator(t, this); 
				break;
			}
			case 116: {
				Get();
				unOp = new BitwiseNotOperator(t, this); 
				break;
			}
			case 96: {
				Get();
				unOp = new PreIncrementOperator(t, this); 
				break;
			}
			case 89: {
				Get();
				unOp = new PreDecrementOperator(t, this); 
				break;
			}
			case 117: {
				Get();
				unOp = new PointerOperator(t, this); 
				break;
			}
			case 84: {
				Get();
				unOp = new ReferenceOperator(t, this); 
				break;
			}
			case 99: {
				Get();
				TypeReference typeRef; 
				TypeCastOperator tcOp = new TypeCastOperator(t, this); 
				Type(out typeRef, false);
				Expect(114);
				tcOp.Type = typeRef; 
				unOp = tcOp; 
				break;
			}
			default: SynErr(205); break;
			}
			Expression unaryExpr; 
			Unary(out unaryExpr);
			if (unOp == null) expr = unaryExpr;
			else
			{
			  unOp.Operand = unaryExpr;
			  expr = unOp;
			}
			unOp.Terminate(t);
			
		} else if (StartOf(27)) {
			Primary(out expr);
		} else SynErr(206);
	}

	void AssignmentOperator(out AssignmentOperator op) {
		op = null; 
		switch (la.kind) {
		case 86: {
			Get();
			op = new AssignmentOperator(t, this); 
			break;
		}
		case 110: {
			Get();
			op = new PlusAssignmentOperator(t, this); 
			break;
		}
		case 104: {
			Get();
			op = new MinusAssignmentOperator(t, this); 
			break;
		}
		case 118: {
			Get();
			op = new MultiplyAssignmentOperator(t, this); 
			break;
		}
		case 90: {
			Get();
			op = new DivideAssignmentOperator(t, this); 
			break;
		}
		case 105: {
			Get();
			op = new ModuloAssignmentOperator(t, this); 
			break;
		}
		case 85: {
			Get();
			op = new AndAssignmentOperator(t, this); 
			break;
		}
		case 108: {
			Get();
			op = new OrAssignmentOperator(t, this); 
			break;
		}
		case 119: {
			Get();
			op = new XorAssignmentOperator(t, this); 
			break;
		}
		case 100: {
			Get();
			op = new LeftShiftAssignmentOperator(t, this); 
			break;
		}
		case 94: {
			Get();
			int pos = t.pos; 
			Expect(95);
			if (pos+1 < t.pos) Error("UNDEF", la, "no whitespace allowed in right shift assignment"); 
			op = new RightShiftAssignmentOperator(t, this); 
			break;
		}
		default: SynErr(207); break;
		}
	}

	void SwitchLabel(out Expression expr) {
		expr = null; 
		if (la.kind == 12) {
			Get();
			Expression(out expr);
			Expect(87);
		} else if (la.kind == 20) {
			Get();
			Expect(87);
		} else SynErr(208);
	}

	void LambdaFunctionSignature(LambdaExpression lambda) {
		if (la.kind == _ident) {
			Expect(1);
		} else if (la.kind == 99) {
			Get();
			if (IsExplicitLambdaParameter(la)) {
				ExplicitLambdaParameterList(lambda);
			} else if (la.kind != _rpar) {
				ImplicitLambdaParameterList(lambda);
			} else if (la.kind == 114) {
			} else SynErr(209);
			Expect(114);
		} else SynErr(210);
	}

	void ExplicitLambdaParameterList(LambdaExpression lambda) {
		FormalParameter fp = new FormalParameter(t, this); 
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				fp.Kind = FormalParameterKind.Ref; 
			} else {
				Get();
				fp.Kind = FormalParameterKind.Out; 
			}
		}
		TypeReference typeRef; 
		Type(out typeRef, false);
		fp.Type = typeRef; 
		Expect(1);
		fp.Name = t.val; 
		lambda.FormalParameters.Add(fp); 
		if (la.kind == 88) {
			Get();
			ExplicitLambdaParameterList(lambda);
		}
	}

	void ImplicitLambdaParameterList(LambdaExpression lambda) {
		Expect(1);
		FormalParameter fp = new FormalParameter(t, this); 
		fp.Name = t.val; 
		lambda.FormalParameters.Add(fp); 
		if (la.kind == 88) {
			Get();
			ImplicitLambdaParameterList(lambda);
		}
	}

	void LambdaFunctionBody(LambdaExpression lambda) {
		if (StartOf(22)) {
			Expression expr; 
			Expression(out expr);
			lambda.Expression = expr; 
		} else if (la.kind == 97) {
			Block(lambda);
		} else SynErr(211);
	}

	void FromClause(out FromClause fromClause) {
		Expect(123);
		Token typeToken = la; 
		Token start = t; 
		TypeReference typeRef; 
		fromClause = new FromClause(start, this); 
		if (IsType(ref typeToken) && typeToken.val != "in") {
			Type(out typeRef, false);
			fromClause.Type = typeRef; 
		} else if (la.kind == 1) {
		} else SynErr(212);
		Expect(1);
		fromClause.Name = t.val; 
		Expect(38);
		Expression expr; 
		Expression(out expr);
		fromClause.Expression = expr; 
		fromClause.Terminate(t); 
	}

	void QueryBody(QueryBody body) {
		while (StartOf(28)) {
			QueryBodyClause(body);
		}
		if (la.kind == 133) {
			SelectClause(body);
		} else if (la.kind == 134) {
			GroupClause(body);
		} else SynErr(213);
		if (la.kind == 129) {
			QueryContinuation(body);
		}
	}

	void QueryBodyClause(QueryBody body) {
		if (la.kind == 123) {
			FromClause fromClause; 
			FromClause(out fromClause);
			body.Clauses.Add(fromClause); 
		} else if (la.kind == 124) {
			LetClause letClause; 
			LetClause(out letClause);
			body.Clauses.Add(letClause); 
		} else if (la.kind == 125) {
			WhereClause whereClause; 
			WhereClause(out whereClause);
			body.Clauses.Add(whereClause); 
		} else if (la.kind == 126) {
			JoinClause joinClause; 
			JoinClause(out joinClause);
			body.Clauses.Add(joinClause); 
		} else if (la.kind == 130) {
			OrderByClause obClause; 
			OrderByClause(out obClause);
			body.Clauses.Add(obClause); 
		} else SynErr(214);
	}

	void SelectClause(QueryBody body) {
		Expect(133);
		Expression expr; 
		Expression(out expr);
		body.Select = new SelectClause(t, this); 
		body.Select.Expression = expr; 
		body.Select.Terminate(t); 
	}

	void GroupClause(QueryBody body) {
		Expect(134);
		Expression grExpr; 
		Expression(out grExpr);
		body.GroupBy = new GroupByClause(t, this); 
		body.GroupBy.Expression = grExpr; 
		Expect(135);
		Expression byExpr; 
		Expression(out byExpr);
		body.GroupBy.ByExpression = byExpr; 
		body.GroupBy.Terminate(t); 
	}

	void QueryContinuation(QueryBody body) {
		Expect(129);
		Expect(1);
		body.IntoIdentifier = t.val; 
		body.ContinuationBody = new QueryBody(); 
		QueryBody(body.ContinuationBody);
	}

	void LetClause(out LetClause letClause) {
		Expect(124);
		letClause = new LetClause(t, this); 
		Expect(1);
		letClause.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr);
		letClause.Expression = expr; 
		letClause.Terminate(t); 
	}

	void WhereClause(out WhereClause whereClause) {
		Expect(125);
		whereClause = new WhereClause(t, this); 
		Expression expr; 
		Expression(out expr);
		whereClause.Expression = expr; 
		whereClause.Terminate(t); 
	}

	void JoinClause(out JoinClause joinClause) {
		Expect(126);
		Token typeToken = la; 
		TypeReference typeRef; 
		joinClause = new JoinClause(t, this); 
		if (IsType(ref typeToken) && typeToken.val != "in") {
			Type(out typeRef, false);
			joinClause.Type = typeRef; 
		} else if (la.kind == 1) {
		} else SynErr(215);
		Expect(1);
		joinClause.Name = t.val; 
		Expect(38);
		Expression inExpr; 
		Expression(out inExpr);
		joinClause.InExpression = inExpr; 
		Expect(127);
		Expression onExpr; 
		Expression(out onExpr);
		joinClause.OnExpression = onExpr; 
		Expect(128);
		Expression eqExpr; 
		Expression(out eqExpr);
		joinClause.EqualsExpression = eqExpr; 
		if (la.kind == 129) {
			Get();
			Expect(1);
			joinClause.IntoIdentifier = t.val; 
		}
		joinClause.Terminate(t); 
	}

	void OrderByClause(out OrderByClause obClause) {
		Expect(130);
		obClause = new OrderByClause(t, this); 
		OrderingClause(obClause);
		while (la.kind == 88) {
			Get();
			OrderingClause(obClause);
		}
		obClause.Terminate(t); 
	}

	void OrderingClause(OrderByClause obClause) {
		Expression expr; 
		Expression(out expr);
		OrderingClause oc = new OrderingClause(t, this); 
		if (la.kind == 131 || la.kind == 132) {
			if (la.kind == 131) {
				Get();
				oc.Ascending = true; 
			} else {
				Get();
				oc.Ascending = false; 
			}
		}
		oc.Expression = expr; 
		oc.Terminate(t); 
	}

	void NullCoalescingExpr(out BinaryOperator expr) {
		expr = null; 
		OrExpr(out expr);
		while (la.kind == 136) {
			Get();
			BinaryOperator oper = new NullCoalescingOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void OrExpr(out BinaryOperator expr) {
		expr = null; 
		AndExpr(out expr);
		while (la.kind == 137) {
			Get();
			BinaryOperator oper = new OrOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void AndExpr(out BinaryOperator expr) {
		expr = null; 
		BitOrExpr(out expr);
		while (la.kind == 138) {
			Get();
			BinaryOperator oper = new AndOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void BitOrExpr(out BinaryOperator expr) {
		expr = null; 
		BitXorExpr(out expr);
		while (la.kind == 139) {
			Get();
			BinaryOperator oper = new BitwiseOrOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void BitXorExpr(out BinaryOperator expr) {
		expr = null; 
		BitAndExpr(out expr);
		while (la.kind == 140) {
			Get();
			BinaryOperator oper = new BitwiseXorOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void BitAndExpr(out BinaryOperator expr) {
		expr = null; 
		EqlExpr(out expr);
		while (la.kind == 84) {
			Get();
			BinaryOperator oper = new BitwiseAndOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void EqlExpr(out BinaryOperator expr) {
		expr = null; 
		RelExpr(out expr);
		BinaryOperator oper; 
		while (la.kind == 93 || la.kind == 106) {
			if (la.kind == 106) {
				Get();
				oper = new EqualOperator(t, this); 
			} else {
				Get();
				oper = new NotEqualOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void RelExpr(out BinaryOperator expr) {
		expr = null; BinaryOperator oper = null; 
		ShiftExpr(out expr);
		while (StartOf(29)) {
			if (StartOf(30)) {
				if (la.kind == 101) {
					Get();
					oper = new LessThanOperator(t, this); 
				} else if (la.kind == 94) {
					Get();
					oper = new GreaterThanOperator(t, this); 
				} else if (la.kind == 141) {
					Get();
					oper = new LessThanOrEqualOperator(t, this); 
				} else if (la.kind == 95) {
					Get();
					oper = new GreaterThanOrEqualOperator(t, this); 
				} else SynErr(216);
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
				oper.Terminate(t);
				
			} else {
				if (la.kind == 42) {
					Get();
					oper = new IsOperator(t, this); 
				} else if (la.kind == 7) {
					Get();
					oper = new IsOperator(t, this); 
				} else SynErr(217);
				oper.LeftOperand = expr; 
				TypeReference typeRef; 
				TypeInRelExpr(out typeRef, false);
				oper.RightOperand = new TypeOperator(t, typeRef); 
				expr = oper; 
				oper.Terminate(t); 
			}
		}
	}

	void ShiftExpr(out BinaryOperator expr) {
		expr = null; 
		AddExpr(out expr);
		BinaryOperator oper = null; 
		while (IsShift()) {
			if (la.kind == 102) {
				Get();
				oper = new LeftShiftOperator(t, this); 
			} else if (la.kind == 94) {
				Get();
				Expect(94);
				oper = new RightShiftOperator(t, this); 
			} else SynErr(218);
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
			oper.Terminate(t);
			
		}
	}

	void AddExpr(out BinaryOperator expr) {
		expr = null; 
		MulExpr(out expr);
		BinaryOperator oper = null; 
		while (la.kind == 103 || la.kind == 109) {
			if (la.kind == 109) {
				Get();
				oper = new AddOperator(t, this); 
			} else {
				Get();
				oper = new SubtractOperator(t, this); 
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
			oper.Terminate(t);
			
		}
	}

	void MulExpr(out BinaryOperator expr) {
		expr = null;
		BinaryOperator oper = null; 
		
		while (la.kind == 117 || la.kind == 142 || la.kind == 143) {
			if (la.kind == 117) {
				Get();
				oper = new MultiplyOperator(t, this); 
			} else if (la.kind == 142) {
				Get();
				oper = new DivideOperator(t, this); 
			} else {
				Get();
				oper = new ModuloOperator(t, this); 
			}
			oper.LeftOperand = expr; 
			Expression unExpr; 
			Unary(out unExpr);
			oper.RightOperand = unExpr; 
			expr = oper; 
			oper.Terminate(t); 
		}
	}

	void Primary(out Expression expr) {
		Expression innerExpr = null;
		expr = null;
		
		switch (la.kind) {
		case 2: case 3: case 4: case 5: case 29: case 47: case 70: {
			Literal(out innerExpr);
			break;
		}
		case 99: {
			Get();
			Expression(out innerExpr);
			Expect(114);
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
			innerExpr = new ThisLiteral(t, this); 
			break;
		}
		case 8: {
			Get();
			if (la.kind == 91) {
				BaseNamedLiteral(out expr);
			} else if (la.kind == 98) {
				BaseIndexerOperator(out expr);
			} else SynErr(219);
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
		default: SynErr(220); break;
		}
		Expression curExpr = innerExpr; 
		while (StartOf(31)) {
			switch (la.kind) {
			case 96: {
				Get();
				curExpr = new PostIncrementOperator(t, this, innerExpr); 
				break;
			}
			case 89: {
				Get();
				curExpr = new PostDecrementOperator(t, this, innerExpr); 
				break;
			}
			case 144: {
				Get();
				NamedLiteral nl; 
				SimpleNamedLiteral(out nl);
				curExpr = new CTypeMemberAccessOperator(t, innerExpr, nl); 
				break;
			}
			case 91: {
				Get();
				NamedLiteral nl; 
				SimpleNamedLiteral(out nl);
				curExpr = new MemberAccessOperator(t, innerExpr, nl); 
				break;
			}
			case 99: {
				Get();
				ArgumentListOperator alop = new ArgumentListOperator(t, this, innerExpr); 
				if (StartOf(19)) {
					Argument(alop.Arguments);
					while (la.kind == 88) {
						Get();
						Argument(alop.Arguments);
					}
				}
				Expect(114);
				curExpr = alop; 
				break;
			}
			case 98: {
				ArrayIndexerOperator aiop = new ArrayIndexerOperator(t, this, innerExpr); 
				ArrayIndexer(aiop);
				curExpr = aiop; 
				break;
			}
			}
			curExpr.Terminate(t); 
		}
		expr = curExpr; 
	}

	void Literal(out Expression value) {
		value = null; 
		switch (la.kind) {
		case 2: {
			Get();
			value = IntegerConstant.Create(t, this); 
			break;
		}
		case 3: {
			Get();
			value = RealConstant.Create(t, this); 
			break;
		}
		case 4: {
			Get();
			value = new CharLiteral(t, this); 
			break;
		}
		case 5: {
			Get();
			value = new StringLiteral(t, this); 
			break;
		}
		case 70: {
			Get();
			value = new TrueLiteral(t, this); 
			break;
		}
		case 29: {
			Get();
			value = new FalseLiteral(t, this); 
			break;
		}
		case 47: {
			Get();
			value = new NullLiteral(t, this); 
			break;
		}
		default: SynErr(221); break;
		}
	}

	void PrimitiveNamedLiteral(out Expression expr) {
		expr = null; 
		PrimitiveNamedLiteral pml = null; 
		switch (la.kind) {
		case 9: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(bool)); 
			break;
		}
		case 11: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(byte)); 
			break;
		}
		case 14: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(char)); 
			break;
		}
		case 19: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(decimal)); 
			break;
		}
		case 23: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(double)); 
			break;
		}
		case 32: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(float)); 
			break;
		}
		case 39: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(int)); 
			break;
		}
		case 44: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(long)); 
			break;
		}
		case 48: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(object)); 
			break;
		}
		case 59: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(sbyte)); 
			break;
		}
		case 61: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(short)); 
			break;
		}
		case 65: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(string)); 
			break;
		}
		case 73: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(uint)); 
			break;
		}
		case 74: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(ulong)); 
			break;
		}
		case 77: {
			Get();
			pml = new PrimitiveNamedLiteral(t, this, typeof(ushort)); 
			break;
		}
		default: SynErr(222); break;
		}
		expr = pml; 
		Expect(91);
		Expect(1);
		pml.Name = t.val; 
		pml.Terminate(t); 
	}

	void NamedLiteral(out Expression expr) {
		expr = null; 
		TypeArgumentListNode argList = null;
		
		Expect(1);
		NamedLiteral nl = new NamedLiteral(t, this); 
		expr = nl; 
		nl.Name = t.val; 
		if (la.kind == 92) {
			Get();
			nl.IsGlobalScope = true; 
			Expect(1);
			nl.Name = t.val; 
		}
		if (IsGeneric()) {
			TypeArgumentList(nl.TypeArguments, out argList);
		}
		nl.Terminate(t); 
	}

	void BaseNamedLiteral(out Expression expr) {
		expr = null; 
		TypeArgumentListNode argList = null;
		
		Expect(91);
		Expect(1);
		BaseNamedLiteral bnl = new BaseNamedLiteral(t, this); 
		bnl.Name = t.val; 
		expr = bnl; 
		if (IsGeneric()) {
			TypeArgumentList(bnl.TypeArguments, out argList);
		}
		bnl.Terminate(t); 
	}

	void BaseIndexerOperator(out Expression expr) {
		Expect(98);
		BaseIndexerOperator bio = new BaseIndexerOperator(t, this); 
		expr = bio; 
		Expression indexExpr; 
		Expression(out indexExpr);
		bio.Indexes.Add(indexExpr); 
		while (la.kind == 88) {
			Get();
			Expression(out indexExpr);
			bio.Indexes.Add(indexExpr); 
		}
		Expect(113);
		bio.Terminate(t); 
	}

	void NewOperator(out Expression expr) {
		Expect(46);
		NewOperator nop = new NewOperator(t, this); 
		expr = nop; 
		TypeReference typeRef; 
		if (la.kind == 97) {
			AnonymousObjectInitializer(nop);
		} else if (StartOf(32)) {
			NonArrayType(out typeRef);
			NewOperatorWithType(nop, typeRef);
		} else if (la.kind == 98) {
			ImplicitArrayCreation(nop);
			nop.Kind = NewOperatorKind.UntypedArrayInitialization; 
		} else SynErr(223);
	}

	void TypeOfOperator(out Expression expr) {
		Expect(72);
		TypeOfOperator top = new TypeOfOperator(t, this); 
		Expect(99);
		expr = top; 
		TypeReference typeRef; 
		Type(out typeRef, true);
		top.Type = typeRef; 
		Expect(114);
		top.Terminate(t); 
	}

	void CheckedOperator(out Expression expr) {
		Expect(15);
		CheckedOperator cop = new CheckedOperator(t, this); 
		Expect(99);
		expr = cop; 
		Expression innerExpr; 
		Expression(out innerExpr);
		cop.Operand = innerExpr; 
		Expect(114);
		cop.Terminate(t); 
	}

	void UncheckedOperator(out Expression expr) {
		Expect(75);
		UncheckedOperator uop = new UncheckedOperator(t, this); 
		Expect(99);
		expr = uop; 
		Expression innerExpr; 
		Expression(out innerExpr);
		uop.Operand = innerExpr; 
		Expect(114);
		uop.Terminate(t); 
	}

	void DefaultOperator(out Expression expr) {
		Expect(20);
		DefaultOperator dop = new DefaultOperator(t, this); 
		Expect(99);
		expr = dop; 
		Expression innerExpr; 
		Primary(out innerExpr);
		dop.Operand = innerExpr; 
		Expect(114);
		dop.Terminate(t); 
	}

	void AnonymousDelegate(out Expression expr) {
		Expect(21);
		AnonymousDelegateOperator adop = new AnonymousDelegateOperator(t, this); 
		CurrentElement = adop; 
		if (la.kind == 99) {
			FormalParameter param; 
			Get();
			if (StartOf(33)) {
				AnonymousMethodParameter(out param);
				adop.FormalParameters.Add(param); 
				while (la.kind == 88) {
					Get();
					AnonymousMethodParameter(out param);
					adop.FormalParameters.Add(param); 
				}
			}
			Expect(114);
		}
		Block(adop);
		expr = adop; 
		adop.Terminate(t); 
	}

	void SizeOfOperator(out Expression expr) {
		Expect(62);
		SizeOfOperator sop = new SizeOfOperator(t, this); 
		Expect(99);
		expr = sop; 
		TypeReference typeRef; 
		Type(out typeRef, true);
		sop.Type = typeRef; 
		Expect(114);
		sop.Terminate(t); 
	}

	void SimpleNamedLiteral(out NamedLiteral expr) {
		TypeArgumentListNode argList = null;
		
		Expect(1);
		expr = new NamedLiteral(t, this); 
		if (IsGeneric()) {
			TypeArgumentList(expr.TypeArguments, out argList);
		}
		expr.Terminate(t); 
	}

	void ArrayIndexer(ArrayIndexerOperator indexer) {
		Expect(98);
		Expression expr; 
		Expression(out expr);
		indexer.Indexers.Add(expr); 
		while (la.kind == 88) {
			Get();
			Expression(out expr);
			indexer.Indexers.Add(expr); 
		}
		Expect(113);
	}

	void AnonymousObjectInitializer(NewOperator nop) {
		Expect(97);
		MemberDeclaratorList mInitList; 
		MemberDeclaratorList(out mInitList);
		if (la.kind == 88) {
			Get();
		}
		Expect(112);
	}

	void NewOperatorWithType(NewOperator nop, TypeReference typeRef) {
		ArrayInitializer arrayInit;
		nop.Type = typeRef; 
		
		if (la.kind == 99) {
			Get();
			if (StartOf(19)) {
				Argument(nop.Arguments);
				while (la.kind == 88) {
					Get();
					Argument(nop.Arguments);
				}
			}
			Expect(114);
			if (la.kind == 97) {
				Initializer init; 
				ObjectOrCollectionInitializer(out init);
				nop.Initializer = init; 
			}
		} else if (la.kind == 97) {
			Initializer init; 
			ObjectOrCollectionInitializer(out init);
			nop.Initializer = init; 
		} else if (IsDims()) {
			nop.Kind = NewOperatorKind.TypedArrayInitialization; 
			Expect(98);
			nop.RunningDimensions = 1; 
			while (la.kind == 88) {
				Get();
				nop.RunningDimensions++; 
			}
			Expect(113);
			ArrayInitializer(out arrayInit);
			nop.Initializer = arrayInit; 
		} else if (la.kind == 98) {
			Get();
			Expression dimExpr; 
			Expression(out dimExpr);
			nop.Dimensions.Add(dimExpr); 
			while (la.kind == 88) {
				Get();
				Expression(out dimExpr);
				nop.Dimensions.Add(dimExpr); 
			}
			Expect(113);
			while (IsDims()) {
				Expect(98);
				nop.RunningDimensions = 1; 
				while (la.kind == 88) {
					Get();
					nop.RunningDimensions++; 
				}
				Expect(113);
			}
			nop.Kind = NewOperatorKind.TypedArrayCreation; 
			if (la.kind == 97) {
				ArrayInitializer(out arrayInit);
				nop.Initializer = arrayInit; 
				nop.Kind = NewOperatorKind.TypedArrayInitialization; 
			}
		} else SynErr(224);
		nop.Terminate(t); 
	}

	void ImplicitArrayCreation(NewOperator nop) {
		ArrayInitializer arrayInit;
		nop.IsImplicitArray = true;
		
		Expect(98);
		nop.RunningDimensions = 1; 
		while (la.kind == 88) {
			Get();
			nop.RunningDimensions++; 
		}
		Expect(113);
		if (la.kind == 97) {
			ArrayInitializer(out arrayInit);
			nop.Initializer = arrayInit; 
		}
	}

	void MemberDeclaratorList(out MemberDeclaratorList initList) {
		initList = new MemberDeclaratorList(t, this);
		MemberDeclarator mInit;
		
		MemberDeclarator(out mInit);
		initList.Initializers.Add(mInit); 
		while (NotFinalComma()) {
			Expect(88);
			MemberDeclarator(out mInit);
			initList.Initializers.Add(mInit); 
		}
		initList.Terminate(t); 
	}

	void MemberDeclarator(out MemberDeclarator init) {
		init = null; 
		Expression expr = null;
		
		if (IsMemberInitializer()) {
			Expect(1);
			Token start = t; 
			Expect(86);
			Expression(out expr);
			init = new MemberDeclarator(start, this, expr, start.val); 
		} else if (StartOf(27)) {
			Token start = la; 
			Primary(out expr);
			init = new MemberDeclarator(start, this, expr, start.val, true); 
		} else if (StartOf(34)) {
			Token start = la; 
			TypeReference typeRef;
			
			PredefinedType(out typeRef);
			Expect(91);
			Expect(1);
			init = new MemberDeclarator(start, this, typeRef, start.val); 
		} else SynErr(225);
	}

	void ObjectOrCollectionInitializer(out Initializer init) {
		Expect(97);
		init = null; 
		if (IsEmptyMemberInitializer()) {
			Expect(112);
		} else if (IsMemberInitializer()) {
			MemberInitializerList mInitList; 
			MemberInitializerList(out mInitList);
			init = mInitList; 
		} else if (StartOf(21)) {
			CollectionInitializer(out init);
		} else SynErr(226);
		Expect(112);
	}

	void MemberInitializerList(out MemberInitializerList initList) {
		initList = new MemberInitializerList(t, this);
		MemberInitializer mInit;
		
		MemberInitializer(out mInit);
		initList.Initializers.Add(mInit); 
		while (NotFinalComma()) {
			Expect(88);
			MemberInitializer(out mInit);
			initList.Initializers.Add(mInit); 
		}
		if (la.kind == 88) {
			Get();
		}
		initList.Terminate(t); 
	}

	void CollectionInitializer(out Initializer init) {
		CollectionInitializer collInit = new CollectionInitializer(t, this); 
		init = collInit;
		Initializer elInit;
		
		ElementInitializerList(out elInit);
		collInit.Initializers.Add(elInit); 
		init.Terminate(t); 
	}

	void ElementInitializerList(out Initializer init) {
		CollectionInitializer collInit = new CollectionInitializer(t, this); 
		init = collInit;
		Initializer elementInit;
		
		ElementInitializer(out elementInit);
		collInit.Initializers.Add(elementInit); 
		while (NotFinalComma()) {
			Expect(88);
			ElementInitializer(out elementInit);
			collInit.Initializers.Add(elementInit); 
		}
		if (la.kind == 88) {
			Get();
		}
		init.Terminate(t); 
	}

	void ElementInitializer(out Initializer init) {
		Expression expr; init = null; 
		if (IsValueInitializer()) {
			Expression(out expr);
			init = new ExpressionInitializer(t, this, expr); 
		} else if (la.kind == 97) {
			ExpressionListInitializer listInit = new ExpressionListInitializer(t, this); 
			init = listInit; 
			Get();
			Expression(out expr);
			listInit.Initializers.Add(new ExpressionInitializer(t, this, expr)); 
			while (la.kind == 88) {
				Get();
				Expression(out expr);
				listInit.Initializers.Add(new ExpressionInitializer(t, this, expr)); 
			}
			Expect(112);
		} else SynErr(227);
		if (init != null) init.Terminate(t); 
	}

	void MemberInitializer(out MemberInitializer init) {
		Expect(1);
		Token startToken = t; 
		Expect(86);
		Expression expr; 
		init = null; 
		if (IsValueInitializer()) {
			Expression(out expr);
			init = new MemberInitializer(startToken, this, expr); 
		} else if (la.kind == 97) {
			Initializer compoundInit; 
			ObjectOrCollectionInitializer(out compoundInit);
			init = new MemberInitializer(startToken, this, compoundInit); 
		} else SynErr(228);
		init.Terminate(t); 
	}

	void AnonymousMethodParameter(out FormalParameter param) {
		param = new FormalParameter(t, this); 
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
		param.Terminate(t); 
	}

	void SingleFieldMember(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef, bool isEvent) {
		Expect(1);
		FieldDeclaration fd = new FieldDeclaration(t, td); 
		CurrentElement = fd;
		fd.SetModifiers(m.Value);
		fd.AssignAttributes(attrs);
		fd.ResultingType = typeRef;
		fd.Name = t.val;
		fd.IsEvent = isEvent;
		
		if (la.kind == 86) {
			Get();
			Initializer init; 
			VariableInitializer(out init);
			fd.Initializer = init; 
		}
		td.AddMember(fd); 
		fd.Terminate(t); 
	}

	void TypeParameter(out TypeParameter tp) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		AttributeDecorationNode attrNode;
		
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
		}
		Expect(1);
		tp = new TypeParameter(t, this);
		tp.Name = t.val;
		tp.AssignAttributes(attrs);
		
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
		CS2();

      Expect(0);
      if (PragmaHandler.OpenRegionCount > 0)
      {
        Error1038(la);
      }
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
		{T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,T, x,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,x,x, x,x,x,x, x,T,T,x, T,T,x,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, T,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, T,x,x,x, x,T,T,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, x,x,x,x, T,x,x,x, x,T,x,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,T,x, x,x,x,x, x,T,T,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,x, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, T,x,x,x, x,T,T,T, x,x,x,x, x,T,T,T, x,x,T,x, x,T,x,T, T,T,T,T, x,T,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, T,x,x,x, x,T,T,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,T,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,T, T,x,x,x, x,T,x,x, x,x,x,x, T,T,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,T, T,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,T,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,x,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,T, T,x,x,x, x,T,x,x, x,x,x,x, T,T,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,T, T,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,T,x, x,x,T,x, x,x,x,x, T,x,x,x, T,T,x,x, T,x,T,x, x,x,x,x, x,x,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,T, x,T,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,T, x,x,x,x, T,x,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x}

	  };

	  #region Syntax error handling
	  
  	void SynErr (int n) 
  	{
		  if (errDist >= MinimumDistanceOfSeparateErrors)
  		{
    		string s;
		    switch (n) 
		    {
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
			case 79: s = "var expected"; break;
			case 80: s = "virtual expected"; break;
			case 81: s = "void expected"; break;
			case 82: s = "volatile expected"; break;
			case 83: s = "while expected"; break;
			case 84: s = "and expected"; break;
			case 85: s = "andassgn expected"; break;
			case 86: s = "assgn expected"; break;
			case 87: s = "colon expected"; break;
			case 88: s = "comma expected"; break;
			case 89: s = "dec expected"; break;
			case 90: s = "divassgn expected"; break;
			case 91: s = "dot expected"; break;
			case 92: s = "dblcolon expected"; break;
			case 93: s = "eq expected"; break;
			case 94: s = "gt expected"; break;
			case 95: s = "gteq expected"; break;
			case 96: s = "inc expected"; break;
			case 97: s = "lbrace expected"; break;
			case 98: s = "lbrack expected"; break;
			case 99: s = "lpar expected"; break;
			case 100: s = "lshassgn expected"; break;
			case 101: s = "lt expected"; break;
			case 102: s = "ltlt expected"; break;
			case 103: s = "minus expected"; break;
			case 104: s = "minusassgn expected"; break;
			case 105: s = "modassgn expected"; break;
			case 106: s = "neq expected"; break;
			case 107: s = "not expected"; break;
			case 108: s = "orassgn expected"; break;
			case 109: s = "plus expected"; break;
			case 110: s = "plusassgn expected"; break;
			case 111: s = "question expected"; break;
			case 112: s = "rbrace expected"; break;
			case 113: s = "rbrack expected"; break;
			case 114: s = "rpar expected"; break;
			case 115: s = "scolon expected"; break;
			case 116: s = "tilde expected"; break;
			case 117: s = "times expected"; break;
			case 118: s = "timesassgn expected"; break;
			case 119: s = "xorassgn expected"; break;
			case 120: s = "larrow expected"; break;
			case 121: s = "\"partial\" expected"; break;
			case 122: s = "\"yield\" expected"; break;
			case 123: s = "\"from\" expected"; break;
			case 124: s = "\"let\" expected"; break;
			case 125: s = "\"where\" expected"; break;
			case 126: s = "\"join\" expected"; break;
			case 127: s = "\"on\" expected"; break;
			case 128: s = "\"equals\" expected"; break;
			case 129: s = "\"into\" expected"; break;
			case 130: s = "\"orderby\" expected"; break;
			case 131: s = "\"ascending\" expected"; break;
			case 132: s = "\"descending\" expected"; break;
			case 133: s = "\"select\" expected"; break;
			case 134: s = "\"group\" expected"; break;
			case 135: s = "\"by\" expected"; break;
			case 136: s = "\"??\" expected"; break;
			case 137: s = "\"||\" expected"; break;
			case 138: s = "\"&&\" expected"; break;
			case 139: s = "\"|\" expected"; break;
			case 140: s = "\"^\" expected"; break;
			case 141: s = "\"<=\" expected"; break;
			case 142: s = "\"/\" expected"; break;
			case 143: s = "\"%\" expected"; break;
			case 144: s = "\"->\" expected"; break;
			case 145: s = "??? expected"; break;
			case 146: s = "invalid NamespaceMemberDeclaration"; break;
			case 147: s = "invalid Attributes"; break;
			case 148: s = "invalid TypeDeclaration"; break;
			case 149: s = "invalid TypeDeclaration"; break;
			case 150: s = "invalid EnumDeclaration"; break;
			case 151: s = "invalid TypeParameterConstraintsClause"; break;
			case 152: s = "invalid TypeParameterConstraintsClause"; break;
			case 153: s = "invalid ClassType"; break;
			case 154: s = "invalid ClassMemberDeclaration"; break;
			case 155: s = "invalid ClassMemberDeclaration"; break;
			case 156: s = "invalid StructMemberDeclaration"; break;
			case 157: s = "invalid StructMemberDeclaration"; break;
			case 158: s = "invalid StructMemberDeclaration"; break;
			case 159: s = "invalid IntegralType"; break;
			case 160: s = "this symbol not expected in EnumBody"; break;
			case 161: s = "this symbol not expected in EnumBody"; break;
			case 162: s = "invalid Expression"; break;
			case 163: s = "invalid Expression"; break;
			case 164: s = "invalid Type"; break;
			case 165: s = "invalid FormalParameterList"; break;
			case 166: s = "invalid EventDeclaration"; break;
			case 167: s = "invalid ConstructorDeclaration"; break;
			case 168: s = "invalid ConstructorDeclaration"; break;
			case 169: s = "invalid MethodDeclaration"; break;
			case 170: s = "invalid OperatorDeclaration"; break;
			case 171: s = "invalid CastOperatorDeclaration"; break;
			case 172: s = "invalid CastOperatorDeclaration"; break;
			case 173: s = "invalid EventAccessorDeclarations"; break;
			case 174: s = "invalid EventAccessorDeclarations"; break;
			case 175: s = "invalid AccessorDeclarations"; break;
			case 176: s = "invalid AccessorDeclarations"; break;
			case 177: s = "invalid AccessorDeclarations"; break;
			case 178: s = "invalid AccessorDeclarations"; break;
			case 179: s = "invalid OverloadableOp"; break;
			case 180: s = "invalid InterfaceMemberDeclaration"; break;
			case 181: s = "invalid InterfaceMemberDeclaration"; break;
			case 182: s = "invalid InterfaceMemberDeclaration"; break;
			case 183: s = "invalid InterfaceAccessors"; break;
			case 184: s = "invalid InterfaceAccessors"; break;
			case 185: s = "invalid LocalVariableDeclaration"; break;
			case 186: s = "invalid LocalVariableDeclarator"; break;
			case 187: s = "invalid VariableInitializer"; break;
			case 188: s = "invalid Keyword"; break;
			case 189: s = "invalid AttributeArguments"; break;
			case 190: s = "invalid PrimitiveType"; break;
			case 191: s = "invalid PointerOrArray"; break;
			case 192: s = "invalid NonArrayType"; break;
			case 193: s = "invalid TypeInRelExpr"; break;
			case 194: s = "invalid PredefinedType"; break;
			case 195: s = "invalid Statement"; break;
			case 196: s = "invalid EmbeddedStatement"; break;
			case 197: s = "invalid EmbeddedStatement"; break;
			case 198: s = "invalid StatementExpression"; break;
			case 199: s = "invalid ForEachStatement"; break;
			case 200: s = "invalid GotoStatement"; break;
			case 201: s = "invalid TryFinallyBlock"; break;
			case 202: s = "invalid UsingStatement"; break;
			case 203: s = "invalid ForInitializer"; break;
			case 204: s = "invalid CatchClauses"; break;
			case 205: s = "invalid Unary"; break;
			case 206: s = "invalid Unary"; break;
			case 207: s = "invalid AssignmentOperator"; break;
			case 208: s = "invalid SwitchLabel"; break;
			case 209: s = "invalid LambdaFunctionSignature"; break;
			case 210: s = "invalid LambdaFunctionSignature"; break;
			case 211: s = "invalid LambdaFunctionBody"; break;
			case 212: s = "invalid FromClause"; break;
			case 213: s = "invalid QueryBody"; break;
			case 214: s = "invalid QueryBodyClause"; break;
			case 215: s = "invalid JoinClause"; break;
			case 216: s = "invalid RelExpr"; break;
			case 217: s = "invalid RelExpr"; break;
			case 218: s = "invalid ShiftExpr"; break;
			case 219: s = "invalid Primary"; break;
			case 220: s = "invalid Primary"; break;
			case 221: s = "invalid Literal"; break;
			case 222: s = "invalid PrimitiveNamedLiteral"; break;
			case 223: s = "invalid NewOperator"; break;
			case 224: s = "invalid NewOperatorWithType"; break;
			case 225: s = "invalid MemberDeclarator"; break;
			case 226: s = "invalid ObjectOrCollectionInitializer"; break;
			case 227: s = "invalid ElementInitializer"; break;
			case 228: s = "invalid MemberInitializer"; break;

  			  default: s = "error " + n; break;
	  	  }
        CompilationUnit.ErrorHandler.Error("SYNERR", la, s, null);
	  	}
		  errDist = 0;
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
}