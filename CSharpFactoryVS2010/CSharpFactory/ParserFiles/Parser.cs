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
			ExternAliasDirective(null, SourceFileNode);
		}
		while (la.kind == 78) {
			UsingDirective(null, SourceFileNode);
		}
		while (IsGlobalAttrTarget()) {
			GlobalAttributes();
		}
		while (StartOf(1)) {
			NamespaceMemberDeclaration(null, File, SourceFileNode);
		}
	}

	void ExternAliasDirective(NamespaceFragment parent, NamespaceScopeNode parentNode) {
		Token start;
		Token alias;
		Token identifier;
		
		Expect(28);
		PragmaHandler.SignRealToken(); 
		Token token = t; 
		// :::
		start = t;
		
		Expect(1);
		if (t.val != "alias") Error1003(t, "alias"); 
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
		parentNode.AddExternAlias(start, alias, identifier, t);
		
	}

	void UsingDirective(NamespaceFragment parent, NamespaceScopeNode parentNode) {
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
		if (alias == null)
		  parentNode.AddUsing(start, nsNode, t);
		else
		  parentNode.AddUsingWithAlias(start, alias, eq, nsNode, t);
		
	}

	void GlobalAttributes() {
		AttributeNode attrNode;
		// :::
		AttributeDecorationNode globAttrNode = null;
		
		Expect(98);
		PragmaHandler.SignRealToken();
		// :::
		globAttrNode = new AttributeDecorationNode(t);
		
		Expect(1);
		if (!"assembly".Equals(t.val) && !"module".Equals(t.val)) 
		 Error("UNDEF", la, "Global attribute target specifier \"assembly\" or \"module\" expected");
		string scope = t.val;
		AttributeDeclaration attr;
		// :::
		globAttrNode.IdentifierToken = t;
		
		Expect(87);
		globAttrNode.ColonToken = t; 
		Attribute(out attr, out attrNode);
		attr.Scope = scope; 
		File.GlobalAttributes.Add(attr);
		CurrentElement = attr;
		// :::
		globAttrNode.Attributes.Add(attrNode);
		
		while (NotFinalComma()) {
			Expect(88);
			var separator = t; 
			Attribute(out attr, out attrNode);
			attr.Scope = scope; 
			File.GlobalAttributes.Add(attr);
			CurrentElement = attr;
			// :::
			globAttrNode.Attributes.Add(new AttributeContinuationNode(separator, attrNode));
			
		}
		if (la.kind == 88) {
			Get();
			globAttrNode.ClosingSeparator = t; 
		}
		Expect(113);
		attr.Terminate(t); 
		// :::
		globAttrNode.Terminate(t);
		SourceFileNode.GlobalAttributes.Add(globAttrNode);
		
	}

	void NamespaceMemberDeclaration(NamespaceFragment parent, SourceFile file, 
NamespaceScopeNode parentNode) {
		if (la.kind == 45) {
			Get();
			PragmaHandler.SignRealToken();
			Token startToken = t; 
			// ::: 
			var nsDecl = new NamespaceDeclarationNode(parentNode, t);
			
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
			parentNode.NamespaceDeclarations.Add(nsDecl);
			
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
			var mod = new ModifierNodeCollection();
			var attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			ModifierList(m, mod);
			TypeDeclarationNode typeDecl; 
			TypeDeclaration(attrs, null, m, out td, parentNode, null, out typeDecl);
			if (td != null)
			{
			  if (parent == null) File.AddTypeDeclaration(td);
			  else parent.AddTypeDeclaration(td);
			}
			// :::
			if (typeDecl != null)
			{
			  typeDecl.AttributeDecorations = attrNodes;
			  typeDecl.Modifiers = mod;
			  typeDecl.DeclaringNamespace = parentNode;
			  parentNode.TypeDeclarations.Add(typeDecl);
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
		
		TypeName(out typeRef, out nsNode);
		attr = new AttributeDeclaration(t, this, typeRef); 
		CurrentElement = attr;
		attrNode.TypeName = nsNode;
		
		if (la.kind == 99) {
			AttributeArguments(attr, attrNode);
		}
		attr.Terminate(t); 
	}

	void AttributeDecorations(AttributeCollection attrs, AttributeDecorationNodeCollection attrNodes) {
		AttributeDecorationNode attrNode; 
		while (la.kind == 98) {
			Attributes(attrs, out attrNode);
			attrNodes.Add(attrNode); 
		}
	}

	void ModifierList(Modifiers m, ModifierNodeCollection mods) {
		while (StartOf(3)) {
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
			mods.Add(t); 
		}
	}

	void TypeDeclaration(AttributeCollection attrs, TypeDeclaration parentType, Modifiers m, 
out TypeDeclaration td, NamespaceScopeNode parentNs, TypeDeclarationNode declaringType,
out TypeDeclarationNode typeDecl) {
		td = null;
		bool isPartial = false;
		// :::
		typeDecl = null; 
		
		if (StartOf(4)) {
			if (la.kind == 121) {
				Get();
				isPartial = true; 
			}
			if (la.kind == 16) {
				ClassDeclaration(m, parentType, isPartial, out td, out typeDecl);
			} else if (la.kind == 66) {
				StructDeclaration(m, parentType, isPartial, out td, out typeDecl);
			} else if (la.kind == 40) {
				InterfaceDeclaration(m, parentType, isPartial, out td, out typeDecl);
			} else SynErr(147);
		} else if (la.kind == 25) {
			EnumDeclaration(m, parentType, out td, out typeDecl);
		} else if (la.kind == 21) {
			DelegateDeclaration(m, parentType, out td, out typeDecl);
		} else SynErr(148);
		if (td != null)
		{
		   td.SetModifiers(m.Value); 
		   td.AssignAttributes(attrs);
		   td.Terminate(t);
		}
		// :::
		if (typeDecl != null)
		{
		  typeDecl.IsPartial = isPartial;
		  typeDecl.DeclaringNamespace = parentNs;
		  typeDecl.DeclaringType = declaringType;
		  typeDecl.Terminate(t);
		}
		
	}

	void ClassDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td, out TypeDeclarationNode typeDecl) {
		Expect(16);
		ClassDeclaration cd = new ClassDeclaration(t, this, parentType);
		cd.IsPartial = isPartial;
		td = cd;
		CurrentElement = cd;
		// :::
		var start = t;
		
		Expect(1);
		cd.Name = t.val; 
		// :::
		var classDecl = new ClassDeclarationNode(start, t);
		typeDecl = classDecl;
		
		if (la.kind == 101) {
			TypeParameterList(cd, typeDecl);
		}
		if (la.kind == 87) {
			BaseTypeList(cd, typeDecl);
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			// :::
			TypeParameterConstraintNode constrNode; 
			
			TypeParameterConstraintsClause(out constraint, out constrNode);
			td.AddTypeParameterConstraint(constraint); 
			// :::
			typeDecl.TypeParameterConstraints.Add(constrNode);
			
		}
		ClassBody(td, classDecl);
		typeDecl.Terminate(t); 
		if (la.kind == 115) {
			Get();
			typeDecl.Terminate(t); 
		}
	}

	void StructDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td, out TypeDeclarationNode typeDecl) {
		TypeOrNamespaceNode typeNode; 
		Expect(66);
		StructDeclaration sd = new StructDeclaration(t, this, parentType);
		td = sd;
		CurrentElement = sd;
		sd.IsPartial = isPartial;
		TypeReference typeRef;
		// :::
		var start = t;
		
		Expect(1);
		sd.Name = t.val; 
		// :::
		var structDecl = new StructDeclarationNode(start, t);
		typeDecl = structDecl;
		
		if (la.kind == 101) {
			TypeParameterList(sd, typeDecl);
		}
		if (la.kind == 87) {
			BaseTypeList(sd, typeDecl);
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintNode constrNode; 
			
			TypeParameterConstraintsClause(out constraint, out constrNode);
			td.AddTypeParameterConstraint(constraint); 
			// :::
			typeDecl.TypeParameterConstraints.Add(constrNode);
			
		}
		StructBody(td, structDecl);
		typeDecl.Terminate(t); 
		if (la.kind == 115) {
			Get();
			typeDecl.Terminate(t); 
		}
	}

	void InterfaceDeclaration(Modifiers m, TypeDeclaration parentType, bool isPartial, 
out TypeDeclaration td, out TypeDeclarationNode typeDecl) {
		Expect(40);
		InterfaceDeclaration ifd = new InterfaceDeclaration(t, this, parentType);
		CurrentElement = ifd;
		td = ifd;
		ifd.IsPartial = isPartial;
		var start = t;
		
		Expect(1);
		ifd.Name = t.val; 
		// :::
		var intfDecl = new InterfaceDeclarationNode(start, t);
		typeDecl = intfDecl;
		
		if (la.kind == 101) {
			TypeParameterList(ifd, typeDecl);
		}
		if (la.kind == 87) {
			BaseTypeList(ifd, typeDecl);
		}
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintNode constrNode; 
			
			TypeParameterConstraintsClause(out constraint, out constrNode);
			td.AddTypeParameterConstraint(constraint); 
			// :::
			typeDecl.TypeParameterConstraints.Add(constrNode);
			
		}
		Expect(97);
		intfDecl.OpenBrace = t; 
		while (StartOf(5)) {
			InterfaceMemberDeclaration(ifd, intfDecl);
		}
		Expect(112);
		intfDecl.CloseBrace = t;
		typeDecl.Terminate(t);
		
		if (la.kind == 115) {
			Get();
			typeDecl.Terminate(t); 
		}
	}

	void EnumDeclaration(Modifiers m, TypeDeclaration parentType, out TypeDeclaration td,
out TypeDeclarationNode typeDecl) {
		TypeOrNamespaceNode typeNode; 
		Expect(25);
		EnumDeclaration ed = new EnumDeclaration(t, this, parentType);
		td = ed;
		CurrentElement = ed;
		// :::
		var start = t;
		
		Expect(1);
		ed.Name = t.val;
		var enumDecl = new EnumDeclarationNode(start, t);
		typeDecl = enumDecl;
		
		if (la.kind == 87) {
			Get();
			TypeReference typeRef; 
			if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
				ClassType(out typeRef, out typeNode);
				ed.InterfaceList.Add(typeRef); 
			} else if (StartOf(6)) {
				TypeOrNamespaceNode itypeNode; 
				IntegralType(out typeRef, out itypeNode);
				ed.InterfaceList.Add(typeRef); 
			} else SynErr(149);
		}
		EnumBody(ed, enumDecl);
		ed.Terminate(t); 
		// :::
		typeDecl.Terminate(t);
		
		if (la.kind == 115) {
			Get();
			ed.Terminate(t); 
			// :::
			typeDecl.Terminate(t);
			
		}
	}

	void DelegateDeclaration(Modifiers m, TypeDeclaration parentType, out TypeDeclaration td,
out TypeDeclarationNode typeDecl) {
		Expect(21);
		DelegateDeclaration dd = new DelegateDeclaration(t, this, parentType);
		td = dd;
		CurrentElement = dd;
		TypeReference returnType;
		// :::
		var start = t;
		TypeOrNamespaceNode typeNode;
		
		Type(out returnType, true, out typeNode);
		dd.ReturnType = returnType; 
		Expect(1);
		dd.Name = t.val; 
		typeDecl = new DelegateDeclarationNode(start, t);
		
		if (la.kind == 101) {
			TypeParameterList(dd, typeDecl);
		}
		Expect(99);
		if (StartOf(7)) {
			FormalParameterList(dd.FormalParameters);
		}
		Expect(114);
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintNode constrNode; 
			
			TypeParameterConstraintsClause(out constraint, out constrNode);
			td.AddTypeParameterConstraint(constraint); 
			// :::
			typeDecl.TypeParameterConstraints.Add(constrNode);
			
		}
		Expect(115);
		typeDecl.Terminate(t); 
	}

	void TypeParameterList(ITypeParameterOwner td, ITypeParameterHolder paramNode) {
		TypeParameter tp;
		Token identifier;
		AttributeDecorationNodeCollection attrNodes;
		
		Expect(101);
		if (paramNode != null) paramNode.SetOpenSign(t); 
		TypeParameter(out tp, out attrNodes, out identifier);
		td.AddTypeParameter(tp); 
		// :::
		var typeParam = new TypeParameterNode(identifier, attrNodes);
		if (paramNode != null) paramNode.TypeParameters.Add(typeParam);
		
		while (la.kind == 88) {
			Get();
			var separator= t; 
			TypeParameter(out tp, out attrNodes, out identifier);
			td.AddTypeParameter(tp); 
			// :::
			typeParam = new TypeParameterContinuationNode(separator, identifier, attrNodes);
			if (paramNode != null) paramNode.TypeParameters.Add(typeParam);
			
		}
		Expect(94);
		if (paramNode != null) paramNode.SetCloseSign(t); 
	}

	void BaseTypeList(TypeDeclaration td, TypeDeclarationNode typeDecl) {
		TypeReference typeRef; 
		// :::
		TypeOrNamespaceNode typeNode;
		
		Expect(87);
		ClassType(out typeRef, out typeNode);
		td.InterfaceList.Add(typeRef); 
		// ::: 
		typeDecl.BaseTypes.Add(typeNode);
		
		while (la.kind == 88) {
			Get();
			var separator = t; 
			ClassType(out typeRef, out typeNode);
			td.InterfaceList.Add(typeRef); 
			// :::
			typeDecl.BaseTypes.Add(new TypeOrNamespaceContinuationNode(separator, typeNode));
			
		}
	}

	void TypeParameterConstraintsClause(out TypeParameterConstraint constraint, 
out TypeParameterConstraintNode constrNode) {
		Token start;
		Token identifier;
		
		Expect(125);
		start = t; 
		Expect(1);
		constraint = new TypeParameterConstraint(t, this); 
		constraint.Name = t.val;
		// :::
		identifier = t;
		
		Expect(87);
		TypeReference typeRef; 
		ConstraintElement element = null;
		// :::
		constrNode = new TypeParameterConstraintNode(start, identifier, t);
		ParameterConstraintTagNode tag;
		
		TypeParameterConstraintTag(out element, out tag);
		constraint.AddConstraintElement(element); 
		// ::: 
		constrNode.ConstraintTags.Add(tag);
		
		while (la.kind == 88) {
			Get();
			var separator = t; 
			TypeParameterConstraintTag(out element, out tag);
			constraint.AddConstraintElement(element); 
			// ::: 
			constrNode.ConstraintTags.Add(new ParameterConstraintTagContinuationNode(separator, tag));
			
		}
	}

	void ClassBody(TypeDeclaration td, ClassDeclarationNode typeDecl) {
		AttributeCollection attrs = new AttributeCollection(); 
		Expect(97);
		typeDecl.OpenBrace = t; 
		while (StartOf(8)) {
			attrs = new AttributeCollection(); 
			var attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			Modifiers m = new Modifiers(this); 
			var mod = new ModifierNodeCollection();
			
			ModifierList(m, mod);
			MemberDeclarationNode memNode; 
			ClassMemberDeclaration(attrs, m, td, out memNode);
			if (memNode != null) 
			{
			  memNode.AttributeDecorations = attrNodes;
			  
			  typeDecl.MemberDeclarations.Add(memNode); 
			}
			
		}
		Expect(112);
		typeDecl.CloseBrace = t; 
	}

	void ClassType(out TypeReference typeRef, out TypeOrNamespaceNode typeNode) {
		typeRef = null; 
		// ::: 
		typeNode = null;
		
		if (la.kind == 1) {
			TypeName(out typeRef, out typeNode);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
				typeRef = new TypeReference(t, this, typeof(object)); 
			} else {
				Get();
				typeRef = new TypeReference(t, this, typeof(string)); 
			}
			typeRef.Terminate(t); 
			// :::
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
			
		} else SynErr(150);
	}

	void ClassMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td,
out MemberDeclarationNode memNode) {
		memNode = null; 
		if (StartOf(9)) {
			StructMemberDeclaration(attrs, m, td, out memNode);
		} else if (la.kind == 116) {
			Get();
			var finNode = new FinalizerDeclarationNode(t);
			memNode = finNode;
			
			Expect(1);
			FinalizerDeclaration dd = new FinalizerDeclaration(t, td);
			CurrentElement = dd;
			dd.Name = t.val;
			dd.SetModifiers(m.Value);
			dd.AssignAttributes(attrs);
			// :::
			finNode.IdentifierToken = t;
			
			Expect(99);
			finNode.OpenParenthesis = t; 
			Expect(114);
			finNode.CloseParenthesis = t; 
			if (la.kind == 97) {
				Block(dd);
			} else if (la.kind == 115) {
				Get();
			} else SynErr(151);
			dd.Terminate(t);
			td.AddMember(dd); 
			// :::
			memNode.Terminate(t);
			
		} else SynErr(152);
	}

	void StructBody(TypeDeclaration td, StructDeclarationNode typeDecl) {
		AttributeCollection attrs = new AttributeCollection(); 
		Expect(97);
		typeDecl.OpenBrace = t; 
		while (StartOf(10)) {
			attrs = new AttributeCollection(); 
			var attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			Modifiers m = new Modifiers(this); 
			// :::
			var mod = new ModifierNodeCollection();
			
			ModifierList(m, mod);
			MemberDeclarationNode memNode; 
			StructMemberDeclaration(attrs, m, td, out memNode);
		}
		Expect(112);
		typeDecl.CloseBrace = t; 
	}

	void StructMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td,
out MemberDeclarationNode memNode) {
		TypeReference typeRef;
		TypeReference memberRef; 
		// :::
		memNode = null;
		
		if (la.kind == 17) {
			ConstMemberDeclaration(attrs, m, td, out memNode);
		} else if (la.kind == 26) {
			EventDeclaration(attrs, m, td, out memNode);
		} else if (la.kind == _ident && Peek(1).kind == _lpar) {
			ConstructorDeclaration(attrs, m, td, out memNode);
		} else if (IsPartialMethod()) {
			Expect(121);
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, true, out typeNode);
			MemberName(out memberRef);
			MethodDeclaration(attrs, m, typeRef, memberRef, td, true);
		} else if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, true, out typeNode);
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
				} else SynErr(153);
			} else if (la.kind == 68) {
				IndexerDeclaration(attrs, m, typeRef, null, td);
			} else SynErr(154);
		} else if (la.kind == 27 || la.kind == 37) {
			CastOperatorDeclaration(attrs, m, td);
		} else if (StartOf(12)) {
			TypeDeclaration nestedType; 
			// :::
			TypeDeclarationNode nestedTypeNode;
			
			TypeDeclaration(attrs, td, m, out nestedType, null, null, out nestedTypeNode);
			td.AddTypeDeclaration(nestedType); 
			// :::
			if (nestedTypeNode != null)
			{
			  // TODO: Handle attributes and modifiers
			  //nestedTypeNode.AttributeDecorations = attrNodes;
			  //nestedTypeNode.Modifiers = mod;
			}
			
		} else SynErr(155);
	}

	void IntegralType(out TypeReference typeRef, out TypeOrNamespaceNode typeNode) {
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
		default: SynErr(156); break;
		}
		typeRef.Terminate(t); 
		// :::
		typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
		
	}

	void EnumBody(EnumDeclaration ed, EnumDeclarationNode typeDecl) {
		Expect(97);
		typeDecl.OpenBrace = t; 
		if (la.kind == 1 || la.kind == 98) {
			EnumMemberDeclaration(ed);
			while (NotFinalComma()) {
				Expect(88);
				while (!(la.kind == 0 || la.kind == 1 || la.kind == 98)) {SynErr(157); Get();}
				EnumMemberDeclaration(ed);
			}
			if (la.kind == 88) {
				Get();
			}
		}
		while (!(la.kind == 0 || la.kind == 112)) {SynErr(158); Get();}
		Expect(112);
		typeDecl.CloseBrace = t; 
	}

	void EnumMemberDeclaration(EnumDeclaration ed) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		ExpressionNode exprNode;
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		Expect(1);
		EnumValueDeclaration ev = new EnumValueDeclaration(t, this); 
		CurrentElement = ev;
		Expression expr;
		
		if (la.kind == 86) {
			Get();
			Expression(out expr, out exprNode);
			ev.ValueExpression = expr; 
		}
		ev.AssignAttributes(attrs);
		ed.Values.Add(ev);
		ev.Terminate(t);
		
	}

	void Expression(out Expression expr, out ExpressionNode exprNode) {
		expr = null; 
		// :::
		exprNode = null;
		Expression leftExpr;
		ExpressionNode leftExprNode;
		BinaryOperatorNode ncsNode = null;
		
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
		} else if (StartOf(13)) {
			Unary(out leftExpr, out leftExprNode);
			if (assgnOps[la.kind] || (la.kind == _gt && Peek(1).kind == _gteq)) {
				AssignmentOperator asgn; 
				AssignmentOperator(out asgn);
				Expression rightExpr; 
				Expression(out rightExpr, out exprNode);
				asgn.RightOperand = rightExpr; 
				asgn.LeftOperand = leftExpr; 
				expr = asgn; 
			} else if (StartOf(14)) {
				BinaryOperator simpleExpr; 
				NullCoalescingExpr(out simpleExpr, out ncsNode);
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
					Get();
					var condExpr = new ConditionalOperator(t, this, expr);
					ConditionalOperatorNode condNode = null;
					if (exprNode != null) condNode = new ConditionalOperatorNode(exprNode);
					expr = condExpr;
					exprNode = condNode;
					Expression trueExpr; 
					ExpressionNode trueNode;
					
					Expression(out trueExpr, out trueNode);
					condExpr.TrueExpression = trueExpr; 
					if (condNode != null) condNode.TrueExpression = trueNode; 
					
					Expect(87);
					Expression falseExpr;
					ExpressionNode falseNode;
					
					Expression(out falseExpr, out falseNode);
					condExpr.FalseExpression = falseExpr;
					if (condNode != null) condNode.FalseExpression = falseNode; 
					condExpr.Terminate(t); 
					if (condNode != null) condNode.Terminate(t);
					
				}
			} else SynErr(159);
		} else SynErr(160);
		if (expr != null) expr.Terminate(t); 
		if (exprNode != null) exprNode.Terminate(t);
		
	}

	void Type(out TypeReference typeRef, bool voidAllowed, out TypeOrNamespaceNode typeNode) {
		typeRef = null;
		// :::
		typeNode = null;
		
		if (StartOf(15)) {
			PrimitiveType(out typeRef, out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef, out typeNode);
		} else if (la.kind == 81) {
			Get();
			typeRef = new TypeReference(t, this); 
			typeRef.Name = t.val;
			typeRef.IsVoid = true; 
			// :::
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
			
		} else SynErr(161);
		if (la.kind == 111) {
			Get();
			typeRef.IsNullable = true; 
			// :::
			typeNode.NullableToken = t;
			
		}
		PointerOrArray(typeRef, typeNode);
		CompilationUnit.AddTypeToFix(typeRef);
		typeRef.Terminate(t); 
		
	}

	void FormalParameterList(FormalParameterCollection pars) {
		TypeReference typeRef = null; 
		AttributeCollection attrs = new AttributeCollection();
		// :::
		ExpressionNode exprNode;
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		FormalParameter fp = new FormalParameter(t, this); 
		fp.AssignAttributes(attrs);
		
		if (StartOf(16)) {
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
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
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
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
			if (!typeRef.IsArray) { Error("UNDEF", la, "params argument must be an array"); } 
			Expect(1);
			fp.Name = t.val; 
			fp.Type = typeRef; 
			pars.Add(fp); 
			fp.Terminate(t);
			
		} else SynErr(162);
	}

	void Block(IBlockOwner block) {
		CurrentElement = block.Owner as LanguageElement; 
		Expect(97);
		while (StartOf(17)) {
			Statement(block);
		}
		Expect(112);
	}

	void ConstMemberDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td,
out MemberDeclarationNode memNode) {
		memNode = null; 
		Expect(17);
		TypeReference typeRef; 
		// :::
		var constNode = new ConstMemberDeclarationNode(t);
		memNode = constNode;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeRef, false, out typeNode);
		memNode.TypeName = typeNode; 
		ConstMemberTagNode tagNode;
		
		SingleConstMember(attrs, m, td, typeRef, out tagNode);
		constNode.ConstTags.Add(tagNode); 
		while (la.kind == 88) {
			Get();
			var separator = t; 
			SingleConstMember(attrs, m, td, typeRef, out tagNode);
			constNode.ConstTags.Add(new ConstMemberContinuationTagNode(separator, tagNode)); 
		}
		Expect(115);
		memNode.Terminate(t); 
	}

	void EventDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
out MemberDeclarationNode memNode) {
		TypeReference typeRef; 
		// :::
		TypeOrNamespaceNode nsNode = null;
		memNode = null;
		
		Expect(26);
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
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
		} else SynErr(163);
	}

	void ConstructorDeclaration(AttributeCollection attrs, Modifiers m, TypeDeclaration td,
out MemberDeclarationNode memNode) {
		memNode = null; 
		Expect(1);
		ConstructorDeclaration cd = new ConstructorDeclaration(t, td);
		CurrentElement = cd;
		cd.SetModifiers(m.Value);
		cd.AssignAttributes(attrs);
		
		Expect(99);
		if (StartOf(7)) {
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
			} else SynErr(164);
			Expect(99);
			CurrentArgumentList(cd.BaseArguments, null);
			Expect(114);
		}
		if (la.kind == 97) {
			Block(cd);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(165);
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
			TypeParameterList(md, null);
		}
		Expect(99);
		if (StartOf(7)) {
			FormalParameterList(md.FormalParameters);
		}
		Expect(114);
		while (la.kind == 125) {
			TypeParameterConstraint constraint; 
			TypeParameterConstraintNode constrNode; 
			
			TypeParameterConstraintsClause(out constraint, out constrNode);
			md.AddTypeParameterConstraint(constraint); 
		}
		if (la.kind == 97) {
			Block(md);
			if (!allowBody || m.Has(Modifier.@abstract)) { Error("UNDEF", la, "Body declaration is not allowed here!"); } 
			md.HasBody = true;
			
		} else if (la.kind == 115) {
			Get();
			md.HasBody = false; 
		} else SynErr(166);
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
		if (StartOf(7)) {
			FormalParameterList(od.FormalParameters);
		}
		Expect(114);
		if (la.kind == 97) {
			Block(od);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(167);
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
		if (StartOf(7)) {
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
		} else SynErr(168);
		Expect(49);
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
		cod.ResultingType = typeRef;
		cod.Name = typeRef.TailName;
		
		Expect(99);
		if (StartOf(7)) {
			FormalParameterList(cod.FormalParameters);
		}
		Expect(114);
		if (la.kind == 97) {
			Block(cod);
		} else if (la.kind == 115) {
			Get();
		} else SynErr(169);
		td.AddMember(cod); 
		cod.Terminate(t);
		
	}

	void SingleConstMember(AttributeCollection attrs, Modifiers m, TypeDeclaration td, 
TypeReference typeRef, out ConstMemberTagNode tagNode) {
		Expect(1);
		ConstDeclaration cd = new ConstDeclaration(t, td); 
		CurrentElement = cd;
		cd.AssignAttributes(attrs);
		cd.SetModifiers(m.Value);
		cd.ResultingType = typeRef;
		cd.Name = t.val;
		// :::
		tagNode = new ConstMemberTagNode(t);
		
		Expect(86);
		td.AddMember(cd);
		Expression expr;
		// :::
		tagNode.EqualToken = t;
		ExpressionNode exprNode;
		
		Expression(out expr, out exprNode);
		cd.Expression = expr; 
		cd.Terminate(t);
		// :::
		tagNode.Expression = exprNode;
		tagNode.Terminate(t);
		
	}

	void EventAccessorDeclarations(EventPropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		Modifiers am = new Modifiers(this); 
		// :::
		var mod = new ModifierNodeCollection();
		
		ModifierList(am, mod);
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
		} else SynErr(170);
		Block(accessor);
		accessor.Terminate(t);
		accessor.HasBody = true;
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs); 
		
		if (StartOf(18)) {
			attrs = new AttributeCollection(); 
			attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			am = new Modifiers(this); 
			// :::
			mod = new ModifierNodeCollection();
			
			ModifierList(am, mod);
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
			} else SynErr(171);
			Block(accessor);
			accessor.Terminate(t);
			accessor.HasBody = true;
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs); 
			
		}
	}

	void CurrentArgumentList(ArgumentList argList, ArgumentNodeCollection argNodes) {
		ExpressionNode exprNode; 
		Argument arg = new Argument(t, this);
		Token argKind = null;
		Token separator = null;
		
		if (StartOf(19)) {
			if (la.kind == 50 || la.kind == 57) {
				if (la.kind == 57) {
					Get();
					arg.Kind = FormalParameterKind.Ref; 
					argKind = t;
					
				} else {
					Get();
					arg.Kind = FormalParameterKind.Out; 
					argKind = t;
					
				}
			}
			Expression expr; 
			Expression(out expr, out exprNode);
			arg.Expression = expr;
			if (argList != null) argList.Add(arg); 
			arg.Terminate(t);
			if (argNodes != null)
			{
			  var argNode = new ArgumentNode(argKind == null ? 
			    (exprNode == null ? t : exprNode.StartToken) : argKind);
			  argNode.KindToken = argKind;
			  argNode.Expression = exprNode;
			  argNode.Terminate(t);
			  argNodes.Add(argNode);
			}
			
			while (la.kind == 88) {
				Get();
				separator = t; 
				if (la.kind == 50 || la.kind == 57) {
					if (la.kind == 57) {
						Get();
						arg.Kind = FormalParameterKind.Ref; 
						argKind = t;
						
					} else {
						Get();
						arg.Kind = FormalParameterKind.Out; 
						argKind = t;
						
					}
				}
				Expression(out expr, out exprNode);
				arg.Expression = expr;
				if (argList != null) argList.Add(arg); 
				arg.Terminate(t);
				if (argNodes != null)
				{
				  var argNode = new ArgumentContinuationNode(separator);
				  argNode.KindToken = argKind;
				  argNode.Expression = exprNode;
				  argNode.Terminate(t);
				  argNodes.Add(argNode);
				}
				
			}
		}
	}

	void AccessorDeclarations(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		Modifiers am = new Modifiers(this); 
		// :::
		var mod = new ModifierNodeCollection();
		
		ModifierList(am, mod);
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
		} else SynErr(172);
		if (la.kind == 97) {
			Block(accessor);
			accessor.HasBody = true; 
		} else if (la.kind == 115) {
			Get();
			accessor.HasBody = false; 
		} else SynErr(173);
		accessor.Terminate(t);
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs);
		
		if (StartOf(18)) {
			attrs = new AttributeCollection(); 
			attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			am = new Modifiers(this); 
			mod = new ModifierNodeCollection();
			
			ModifierList(am, mod);
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
			} else SynErr(174);
			if (la.kind == 97) {
				Block(accessor);
				accessor.HasBody = true; 
			} else if (la.kind == 115) {
				Get();
				accessor.HasBody = false; 
			} else SynErr(175);
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
		default: SynErr(176); break;
		}
	}

	void InterfaceMemberDeclaration(InterfaceDeclaration ifd, InterfaceDeclarationNode typeDecl) {
		var m = new Modifiers(this);
		TypeReference typeRef;
		var attrs = new AttributeCollection();
		var pars = new FormalParameterCollection();
		// :::
		var mod = new ModifierNodeCollection();
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		ModifierList(m, mod);
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, true, out typeNode);
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
				} else SynErr(177);
			} else if (la.kind == 68) {
				IndexerDeclaration ind = new IndexerDeclaration(t, ifd);
				CurrentElement =ind;
				ind.SetModifiers(m.Value);
				ind.AssignAttributes(attrs);
				ind.Name = "";
				ind.ResultingType = typeRef;
				
				Get();
				Expect(98);
				if (StartOf(7)) {
					FormalParameterList(ind.FormalParameters);
				}
				Expect(113);
				Expect(97);
				InterfaceAccessors(ind);
				Expect(112);
				ind.Terminate(t); 
			} else SynErr(178);
		} else if (la.kind == 26) {
			InterfaceEventDeclaration(attrs, m, ifd);
		} else SynErr(179);
	}

	void InterfaceAccessors(PropertyDeclaration prop) {
		AttributeCollection attrs = new AttributeCollection();
		AccessorDeclaration accessor = null;
		// :::
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		Modifiers am = new Modifiers(this); 
		var mod = new ModifierNodeCollection();
		
		ModifierList(am, mod);
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
		} else SynErr(180);
		Expect(115);
		accessor.Terminate(t);
		accessor.SetModifiers(am.Value); 
		accessor.AssignAttributes(attrs); 
		
		if (StartOf(18)) {
			attrs = new AttributeCollection(); 
			attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrs, attrNodes);
			am = new Modifiers(this); 
			mod = new ModifierNodeCollection();
			
			ModifierList(am, mod);
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
			} else SynErr(181);
			Expect(115);
			accessor.Terminate(t);
			accessor.SetModifiers(am.Value); 
			accessor.AssignAttributes(attrs); 
			
		}
	}

	void InterfaceEventDeclaration(AttributeCollection attrs, Modifiers m, InterfaceDeclaration ifd) {
		TypeReference typeRef; 
		Expect(26);
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
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
		
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
		} else if (la.kind == 79) {
			Get();
			isImplicit = true; 
		} else SynErr(182);
		LocalVariableDeclarator(block, typeRef, isImplicit);
		while (la.kind == 88) {
			Get();
			LocalVariableDeclarator(block, typeRef, isImplicit);
		}
	}

	void LocalVariableDeclarator(IBlockOwner block, TypeReference typeRef, bool isImplicit) {
		ExpressionNode exprNode; 
		Expect(1);
		LocalVariableDeclaration loc = new LocalVariableDeclaration(t, this, block); 
		CurrentElement = loc; 
		loc.Name = t.val; 
		loc.Variable.ResultingType = typeRef; 
		loc.Variable.IsImplicit = isImplicit; 
		if (block != null) block.Statements.Add(loc); 
		if (la.kind == 86) {
			Get();
			if (StartOf(20)) {
				Initializer init; 
				VariableInitializer(out init);
				loc.Variable.Initializer = init; 
			} else if (la.kind == 63) {
				Get();
				StackAllocInitializer saIn = new StackAllocInitializer(t, this); 
				loc.Variable.Initializer = saIn; 
				TypeReference tr; 
				TypeOrNamespaceNode typeNode; 
				Type(out tr, false, out typeNode);
				saIn.Type = tr; 
				Expression expr; 
				Expect(98);
				Expression(out expr, out exprNode);
				saIn.Expression = expr; 
				Expect(113);
				saIn.Terminate(t); 
			} else SynErr(183);
		}
		block.Add(loc.Variable); 
		loc.Terminate(t);
		
	}

	void VariableInitializer(out Initializer init) {
		Expression expr; init = null; 
		// :::
		ExpressionNode exprNode;
		
		if (StartOf(21)) {
			Expression(out expr, out exprNode);
			ExpressionInitializer expIn = new ExpressionInitializer(t, this, expr); 
			init = expIn; expIn.Terminate(t); 
		} else if (la.kind == 97) {
			ArrayInitializer arrInit; 
			ArrayInitializer(out arrInit);
			init = arrInit; 
		} else SynErr(184);
	}

	void ArrayInitializer(out ArrayInitializer init) {
		init = new ArrayInitializer(t, this); 
		Initializer arrayInit = null; 
		Expect(97);
		if (StartOf(20)) {
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

	void Attributes(AttributeCollection attrs, out AttributeDecorationNode attrNode) {
		string scope = ""; 
		attrNode = null;
		AttributeNode attributeNode;
		
		Expect(98);
		attrNode = new AttributeDecorationNode(t); 
		if (IsAttrTargSpec()) {
			if (la.kind == 1) {
				Get();
			} else if (StartOf(22)) {
				Keyword();
			} else SynErr(185);
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
		attr.Terminate(t);
		// :::
		attrNode.Terminate(t);
		
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
		default: SynErr(186); break;
		}
	}

	void AttributeArguments(AttributeDeclaration attr, AttributeNode argsNode) {
		AttributeArgument arg; 
		Token identifier = null;
		Token equal = null;
		ExpressionNode exprNode;
		bool nameFound = false;
		
		Expect(99);
		argsNode.OpenParenthesis = t; 
		if (StartOf(21)) {
			arg = new AttributeArgument(t, this); 
			if (IsAssignment()) {
				Expect(1);
				arg.Name = t.val;
				// :::
				identifier = t;
				
				Expect(86);
				nameFound = true; 
				// :::
				equal = t;
				
			}
			Expression expr; 
			Expression(out expr, out exprNode);
			arg.Expression = expr;
			attr.Arguments.Add(arg); 
			arg.Terminate(t);
			// :::
			var argNode = new AttributeArgumentNode(identifier, equal, exprNode); 
			argNode.Terminate(t);
			argsNode.Arguments.Add(argNode);
			
			while (la.kind == 88) {
				Get();
				arg = new AttributeArgument(t, this); 
				// :::
				var separator = t;
				
				if (IsAssignment()) {
					Expect(1);
					arg.Name = t.val; 
					// :::
					identifier = t;
					
					Expect(86);
					nameFound = true; 
					// :::
					equal = t;
					
				} else if (StartOf(21)) {
					if (nameFound) Error("UNDEF", la, "no positional argument after named arguments"); 
				} else SynErr(187);
				Expression(out expr, out exprNode);
				arg.Expression = expr; 
				attr.Arguments.Add(arg); 
				arg.Terminate(t);
				// :::
				var argcNode = new AttributeArgumentContinuationNode(separator, identifier, equal, exprNode); 
				argcNode.Terminate(t);
				argsNode.Arguments.Add(argcNode);
				
			}
		}
		Expect(114);
		argsNode.Terminate(t); 
	}

	void PrimitiveType(out TypeReference typeRef, out TypeOrNamespaceNode typeNode) {
		typeRef = null; 
		// :::
		typeNode = null;
		
		if (StartOf(6)) {
			IntegralType(out typeRef, out typeNode);
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
			// :::
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
			
		} else SynErr(188);
	}

	void PointerOrArray(TypeReference typeRef, TypeOrNamespaceNode typeNode) {
		while (IsPointerOrDims()) {
			if (la.kind == 117) {
				Get();
				typeRef.TypeModifiers.Add(new PointerModifier());
				// :::
				if (typeNode != null) typeNode.TypeModifiers.Add(new PointerModifierNode(t)); 
				
			} else if (la.kind == 98) {
				Get();
				int rank = 1; 
				// :::
				var arrNode = new ArrayModifierNode(t);
				
				while (la.kind == 88) {
					Get();
					rank++; 
					// :::
					arrNode.AddSeparator(t);
					
				}
				Expect(113);
				typeRef.TypeModifiers.Add(new ArrayModifier(rank)); 
				// :::
				arrNode.Terminate(t);
				
			} else SynErr(189);
		}
	}

	void NonArrayType(out TypeReference typeRef, out TypeOrNamespaceNode typeNode) {
		typeRef = null;
		// :::
		typeNode = null;
		
		if (StartOf(15)) {
			PrimitiveType(out typeRef, out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef, out typeNode);
		} else SynErr(190);
		if (la.kind == 111) {
			Get();
			typeRef.IsNullable = true;
			// :::
			typeNode.NullableToken = t;
			
		}
		if (la.kind == 117) {
			Get();
			typeRef.TypeModifiers.Add(new PointerModifier());
			CompilationUnit.AddTypeToFix(typeRef); 
			// :::
			typeNode.TypeModifiers.Add(new PointerModifierNode(t));
			
		}
		typeRef.Terminate(t); 
		// :::
		typeNode.Terminate(t);
		
	}

	void TypeInRelExpr(out TypeReference typeRef, out TypeOrNamespaceNode typeNode, bool voidAllowed) {
		typeRef = null;
		// :::
		typeNode = null; 
		
		if (StartOf(15)) {
			PrimitiveType(out typeRef, out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeRef, out typeNode);
		} else if (la.kind == 81) {
			Get();
			typeRef = new TypeReference(t, this); 
			typeRef.Name = t.val;
			typeRef.IsVoid = true; 
			// :::
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
			
		} else SynErr(191);
		if (IsNullableTypeMark()) {
			Expect(111);
			typeNode.NullableToken = t; 
		}
		PointerOrArray(typeRef, typeNode);
		CompilationUnit.AddTypeToFix(typeRef);
		typeRef.Terminate(t); 
		
	}

	void PredefinedType(out TypeReference typeRef, out TypeOrNamespaceNode typeNode) {
		typeRef = null; 
		typeNode = null;
		
		if (StartOf(15)) {
			PrimitiveType(out typeRef, out typeNode);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
				typeRef = new TypeReference(t, this, typeof(object)); 
			} else {
				Get();
				typeRef = new TypeReference(t, this, typeof(string)); 
			}
			typeRef.Terminate(t); 
			// :::
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
			
		} else SynErr(192);
	}

	void TypeArgumentList(TypeReferenceCollection args, out TypeArgumentListNode argList) {
		TypeReference paramType; 
		argList = null;
		
		Expect(101);
		paramType = TypeReference.EmptyType;
		argList = new TypeArgumentListNode(t);
		
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out paramType, false, out typeNode);
		}
		args.Add(paramType); 
		while (la.kind == 88) {
			Get();
			paramType = TypeReference.EmptyType; 
			if (StartOf(11)) {
				TypeOrNamespaceNode typeNode; 
				Type(out paramType, false, out typeNode);
			}
			args.Add(paramType); 
		}
		Expect(94);
		argList.Terminate(t); 
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
		} else SynErr(193);
	}

	void ConstStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(17);
		TypeReference typeRef; 
		ConstStatement cs = new ConstStatement(t, this, block); 
		CurrentElement = cs; 
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
		Expect(1);
		cs.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr, out exprNode);
		cs.Expression = expr; 
		if (block != null) block.Add(cs); 
		cs.Terminate(t);
		
		while (la.kind == 88) {
			Get();
			cs = new ConstStatement(t, this, block); 
			Expect(1);
			cs.Name = t.val; 
			Expect(86);
			Expression(out expr, out exprNode);
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
		} else if (StartOf(13)) {
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
			} else SynErr(194);
			Expect(115);
		} else if (la.kind == 31) {
			FixedStatement(block);
		} else SynErr(195);
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
		ExpressionNode exprNode;
		bool isAssignment = assnStartOp[la.kind] || IsTypeCast(); 
		Expression expr = null; 
		ExpressionNode unaryNode;
		
		Unary(out expr, out unaryNode);
		ExpressionStatement es = new ExpressionStatement(t, this, block); 
		CurrentElement = es; 
		es.Expression = expr; 
		if (StartOf(25)) {
			AssignmentOperator asgn; 
			AssignmentOperator(out asgn);
			es.Expression = asgn; 
			asgn.LeftOperand = expr; 
			Expression rightExpr; 
			Expression(out rightExpr, out exprNode);
			asgn.RightOperand = rightExpr; 
		} else if (la.kind == 88 || la.kind == 114 || la.kind == 115) {
			if (isAssignment) Error("UNDEF", la, "error in assignment."); 
		} else SynErr(196);
		if (block != null) block.Add(es); 
		es.Terminate(t); 
	}

	void IfStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(36);
		IfStatement ifs = new IfStatement(t, this, block); 
		CurrentElement = ifs; 
		Expect(99);
		if (block != null) block.Add(ifs); 
		Expression expr; 
		Expression(out expr, out exprNode);
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
		ExpressionNode exprNode; 
		Expect(67);
		SwitchStatement sws = new SwitchStatement(t, this, block); 
		CurrentElement = sws; 
		Expect(99);
		Expression expr; 
		Expression(out expr, out exprNode);
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
		ExpressionNode exprNode; 
		Expect(83);
		WhileStatement whs = new WhileStatement(t, this, block); 
		CurrentElement = whs; 
		Expect(99);
		if (block != null) block.Add(whs); 
		Expression expr; 
		Expression(out expr, out exprNode);
		whs.Condition = expr; 
		Expect(114);
		EmbeddedStatement(whs);
		whs.Terminate(t); 
	}

	void DoWhileStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(22);
		DoWhileStatement whs = new DoWhileStatement(t, this, block); 
		CurrentElement = whs; 
		EmbeddedStatement(whs);
		if (block != null) block.Add(whs); 
		Expect(83);
		Expect(99);
		Expression expr; 
		Expression(out expr, out exprNode);
		whs.Condition = expr; 
		Expect(114);
		Expect(115);
		whs.Terminate(t); 
	}

	void ForStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
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
		if (StartOf(21)) {
			Expression expr; 
			Expression(out expr, out exprNode);
			fs.Condition = expr; 
		}
		Expect(115);
		if (StartOf(13)) {
			ForIterator(fs);
			fs.CreateIteratorBlock(t); 
		}
		Expect(114);
		EmbeddedStatement(fs);
		fs.Terminate(t); 
	}

	void ForEachStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(34);
		ForEachStatement fes = new ForEachStatement(t, this, block); 
		CurrentElement = fes; 
		Expect(99);
		if (block != null) block.Add(fes); 
		TypeReference typeRef; 
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
			fes.Variable.ResultingType = typeRef; 
		} else if (la.kind == 79) {
			Get();
			fes.Variable.IsImplicit = true; 
		} else SynErr(197);
		Expect(1);
		fes.Variable.Name = t.val; 
		Expect(38);
		Expression expr; 
		Expression(out expr, out exprNode);
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
		ExpressionNode exprNode; 
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
			Expression(out expr, out exprNode);
			gs.LabelExpression = expr; 
		} else if (la.kind == 20) {
			Get();
			gs.Name = t.val; 
		} else SynErr(198);
		Expect(115);
		gs.Terminate(t); 
	}

	void ReturnStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(58);
		ReturnStatement yrs = new ReturnStatement(t, this, block); 
		CurrentElement = yrs; 
		if (StartOf(21)) {
			Expression expr; 
			Expression(out expr, out exprNode);
			yrs.Expression = expr; 
		}
		Expect(115);
		if (block != null) block.Add(yrs); 
		yrs.Terminate(t); 
	}

	void ThrowStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(69);
		ThrowStatement ts = new ThrowStatement(t, this, block); 
		CurrentElement = ts; 
		if (StartOf(21)) {
			Expression expr; 
			Expression(out expr, out exprNode);
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
		} else SynErr(199);
		ts.Terminate(t); 
	}

	void LockStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(43);
		LockStatement ls = new LockStatement(t, this, block); 
		CurrentElement = ls; 
		if (block != null) block.Add(ls); 
		Expect(99);
		Expression expr; 
		Expression(out expr, out exprNode);
		ls.Expression = expr; 
		Expect(114);
		EmbeddedStatement(ls);
		ls.Terminate(t); 
	}

	void UsingStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(78);
		UsingStatement us = new UsingStatement(t, this, block);
		CurrentElement = us;
		if (block != null) block.Add(us);
		
		Expect(99);
		if (IsLocalVarDecl()) {
			LocalVariableDeclaration(us);
		} else if (StartOf(21)) {
			Expression expr; 
			Expression(out expr, out exprNode);
			us.ResourceExpression = expr; 
		} else SynErr(200);
		Expect(114);
		EmbeddedStatement(us);
		us.Terminate(t); 
	}

	void YieldReturnStatement(IBlockOwner block) {
		ExpressionNode exprNode; 
		Expect(58);
		Expression expr; 
		Expression(out expr, out exprNode);
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
		ExpressionNode exprNode; 
		Expect(31);
		FixedStatement fs = new FixedStatement(t, this, block); 
		CurrentElement = fs; 
		if (block != null) block.Add(fs); 
		Expect(99);
		TypeReference typeRef; 
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
		if (!typeRef.IsPointer) Error("UNDEF", la, "can only fix pointer types"); 
		ValueAssignmentStatement vas = new ValueAssignmentStatement(t, this, block); 
		CurrentElement = vas; 
		Expect(1);
		vas.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr, out exprNode);
		vas.Expression = expr; 
		fs.Assignments.Add(vas); 
		while (la.kind == 88) {
			Get();
			vas = new ValueAssignmentStatement(t, this, block); 
			CurrentElement = vas; 
			Expect(1);
			vas.Name = t.val; 
			Expect(86);
			Expression(out expr, out exprNode);
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
		} else if (StartOf(13)) {
			StatementExpression(fs.InitializerBlock);
			while (la.kind == 88) {
				Get();
				StatementExpression(fs.InitializerBlock);
			}
		} else SynErr(201);
	}

	void ForIterator(ForStatement fs) {
		StatementExpression(fs.IteratorBlock);
		while (la.kind == 88) {
			Get();
			StatementExpression(fs.IteratorBlock);
		}
	}

	void CatchClauses(TryStatement tryStm) {
		TypeOrNamespaceNode typeNode; 
		Expect(13);
		CatchClause cc = tryStm.CreateCatchClause(t); 
		CurrentElement = cc; 
		if (la.kind == 97) {
			Block(cc);
		} else if (la.kind == 99) {
			Get();
			TypeReference typeRef; 
			ClassType(out typeRef, out typeNode);
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
		} else SynErr(202);
	}

	void Unary(out Expression expr, out ExpressionNode exprNode) {
		UnaryOperator unOp = null;
		expr = null;
		// :::
		exprNode = null;
		UnaryOperatorNode unaryOp = null;
		
		if (unaryHead[la.kind] || IsTypeCast()) {
			switch (la.kind) {
			case 109: {
				Get();
				unOp = new UnaryPlusOperator(t, this); 
				unaryOp = new UnaryPlusOperatorNode(t);
				
				break;
			}
			case 103: {
				Get();
				unOp = new UnaryMinusOperator(t, this); 
				unaryOp = new UnaryMinusOperatorNode(t);
				
				break;
			}
			case 107: {
				Get();
				unOp = new NotOperator(t, this); 
				unaryOp = new UnaryNotOperatorNode(t);
				
				break;
			}
			case 116: {
				Get();
				unOp = new BitwiseNotOperator(t, this); 
				unaryOp = new BitwiseNotOperatorNode(t);
				
				break;
			}
			case 96: {
				Get();
				unOp = new PreIncrementOperator(t, this); 
				unaryOp = new PreIncrementOperatorNode(t);
				
				break;
			}
			case 89: {
				Get();
				unOp = new PreDecrementOperator(t, this);
				unaryOp = new PreDecrementOperatorNode(t);
				
				break;
			}
			case 117: {
				Get();
				unOp = new PointerOperator(t, this); 
				unaryOp = new PointerOperatorNode(t);
				
				break;
			}
			case 84: {
				Get();
				unOp = new ReferenceOperator(t, this); 
				unaryOp = new ReferenceOperatorNode(t);
				
				break;
			}
			case 99: {
				Get();
				TypeReference typeRef;
				TypeCastOperator tcOp = new TypeCastOperator(t, this);
				// :::
				TypeOrNamespaceNode typeNode; 
				var tcNode = new TypecastOperatorNode(t);
				unaryOp = tcNode;
				
				Type(out typeRef, false, out typeNode);
				tcNode.TypeName = typeNode; 
				Expect(114);
				tcOp.Type = typeRef;
				unOp = tcOp; 
				
				break;
			}
			default: SynErr(203); break;
			}
			Expression unaryExpr; 
			// :::
			ExpressionNode unaryNode;
			
			Unary(out unaryExpr, out unaryNode);
			if (unOp == null) expr = unaryExpr;
			else
			{
			  unOp.Operand = unaryExpr;
			  expr = unOp;
			}
			unOp.Terminate(t);
			// :::
			if (unaryOp == null) exprNode = unaryNode;
			else
			{
			  unaryOp.Operand = unaryNode;
			  exprNode = unaryOp;
			}
			unaryOp.Terminate(t);
			
		} else if (StartOf(27)) {
			Primary(out expr, out exprNode);
		} else SynErr(204);
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
		default: SynErr(205); break;
		}
	}

	void SwitchLabel(out Expression expr) {
		expr = null; 
		ExpressionNode exprNode;
		
		if (la.kind == 12) {
			Get();
			Expression(out expr, out exprNode);
			Expect(87);
		} else if (la.kind == 20) {
			Get();
			Expect(87);
		} else SynErr(206);
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
			} else SynErr(207);
			Expect(114);
		} else SynErr(208);
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
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
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
		ExpressionNode exprNode; 
		if (StartOf(21)) {
			Expression expr; 
			Expression(out expr, out exprNode);
			lambda.Expression = expr; 
		} else if (la.kind == 97) {
			Block(lambda);
		} else SynErr(209);
	}

	void FromClause(out FromClause fromClause) {
		ExpressionNode exprNode; 
		Expect(123);
		Token typeToken = la; 
		Token start = t; 
		TypeReference typeRef; 
		fromClause = new FromClause(start, this); 
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
			fromClause.Type = typeRef; 
		} else if (la.kind == 1) {
		} else SynErr(210);
		Expect(1);
		fromClause.Name = t.val; 
		Expect(38);
		Expression expr; 
		Expression(out expr, out exprNode);
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
		} else SynErr(211);
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
		} else SynErr(212);
	}

	void SelectClause(QueryBody body) {
		ExpressionNode exprNode; 
		Expect(133);
		Expression expr; 
		Expression(out expr, out exprNode);
		body.Select = new SelectClause(t, this); 
		body.Select.Expression = expr; 
		body.Select.Terminate(t); 
	}

	void GroupClause(QueryBody body) {
		ExpressionNode exprNode; 
		Expect(134);
		Expression grExpr; 
		Expression(out grExpr, out exprNode);
		body.GroupBy = new GroupByClause(t, this); 
		body.GroupBy.Expression = grExpr; 
		Expect(135);
		Expression byExpr; 
		Expression(out byExpr, out exprNode);
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
		ExpressionNode exprNode; 
		Expect(124);
		letClause = new LetClause(t, this); 
		Expect(1);
		letClause.Name = t.val; 
		Expect(86);
		Expression expr; 
		Expression(out expr, out exprNode);
		letClause.Expression = expr; 
		letClause.Terminate(t); 
	}

	void WhereClause(out WhereClause whereClause) {
		ExpressionNode exprNode; 
		Expect(125);
		whereClause = new WhereClause(t, this); 
		Expression expr; 
		Expression(out expr, out exprNode);
		whereClause.Expression = expr; 
		whereClause.Terminate(t); 
	}

	void JoinClause(out JoinClause joinClause) {
		ExpressionNode exprNode; 
		Expect(126);
		Token typeToken = la; 
		TypeReference typeRef; 
		joinClause = new JoinClause(t, this); 
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeOrNamespaceNode typeNode; 
			Type(out typeRef, false, out typeNode);
			joinClause.Type = typeRef; 
		} else if (la.kind == 1) {
		} else SynErr(213);
		Expect(1);
		joinClause.Name = t.val; 
		Expect(38);
		Expression inExpr; 
		Expression(out inExpr, out exprNode);
		joinClause.InExpression = inExpr; 
		Expect(127);
		Expression onExpr; 
		Expression(out onExpr, out exprNode);
		joinClause.OnExpression = onExpr; 
		Expect(128);
		Expression eqExpr; 
		Expression(out eqExpr, out exprNode);
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
		ExpressionNode exprNode;
		
		Expression(out expr, out exprNode);
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

	void NullCoalescingExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		OrExpr(out expr, out exprNode);
		while (la.kind == 136) {
			Get();
			var oper = new NullCoalescingOperator(t, this); 
			var opNode = new NullCoalescingOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			OrExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void OrExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		AndExpr(out expr, out exprNode);
		while (la.kind == 137) {
			Get();
			var oper = new OrOperator(t, this); 
			var opNode = new LogicalOrOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			AndExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void AndExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		BitOrExpr(out expr, out exprNode);
		while (la.kind == 138) {
			Get();
			var oper = new AndOperator(t, this); 
			var opNode = new LogicalAndOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			BitOrExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void BitOrExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		BitXorExpr(out expr, out exprNode);
		while (la.kind == 139) {
			Get();
			var oper = new BitwiseOrOperator(t, this); 
			var opNode = new BitwiseOrOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			BitXorExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void BitXorExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		BitAndExpr(out expr, out exprNode);
		while (la.kind == 140) {
			Get();
			var oper = new BitwiseXorOperator(t, this); 
			var opNode = new BitwiseXorOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			BitAndExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void BitAndExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		EqlExpr(out expr, out exprNode);
		while (la.kind == 84) {
			Get();
			var oper = new BitwiseAndOperator(t, this); 
			var opNode = new BitwiseAndOperatorNode(t);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			EqlExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void EqlExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		RelExpr(out expr, out exprNode);
		BinaryOperator oper = null; 
		BinaryOperatorNode opNode = null;
		
		while (la.kind == 93 || la.kind == 106) {
			if (la.kind == 106) {
				Get();
				oper = new EqualOperator(t, this); 
				opNode = new EqualOperatorNode(t);
				
			} else {
				Get();
				oper = new NotEqualOperator(t, this); 
				opNode = new EqualOperatorNode(t);
				
			}
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			RelExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void RelExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		ShiftExpr(out expr, out exprNode);
		BinaryOperator oper = null; 
		BinaryOperatorNode opNode = null;
		
		while (StartOf(29)) {
			if (StartOf(30)) {
				if (la.kind == 101) {
					Get();
					oper = new LessThanOperator(t, this); 
					opNode = new LessThanOperatorNode(t);
					
				} else if (la.kind == 94) {
					Get();
					oper = new GreaterThanOperator(t, this); 
					opNode = new GreaterThanOperatorNode(t);
					
				} else if (la.kind == 141) {
					Get();
					oper = new LessThanOrEqualOperator(t, this); 
					opNode = new LessThanOrEqualOperatorNode(t);
					
				} else if (la.kind == 95) {
					Get();
					oper = new GreaterThanOrEqualOperator(t, this); 
					opNode = new GreaterThanOrEqualOperatorNode(t);
					
				} else SynErr(214);
				oper.LeftOperand = expr;
				opNode.LeftOperand = exprNode;
				Expression unExpr; 
				ExpressionNode unaryNode;
				
				Unary(out unExpr, out unaryNode);
				BinaryOperator rightExpr; 
				BinaryOperatorNode rgNode;
				
				ShiftExpr(out rightExpr, out rgNode);
				BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
				expr = oper;
				exprNode = opNode;
				
			} else {
				if (la.kind == 42) {
					Get();
					oper = new IsOperator(t, this); 
					opNode = new IsOperatorNode(t);
					
				} else if (la.kind == 7) {
					Get();
					oper = new AsOperator(t, this); 
					opNode = new AsOperatorNode(t);
					
				} else SynErr(215);
				oper.LeftOperand = expr;
				TypeReference typeRef; 
				TypeOrNamespaceNode typeNode;
				
				TypeInRelExpr(out typeRef, out typeNode, false);
				oper.RightOperand = new TypeOperator(t, typeRef);
				opNode.RightOperand = new TypeOperatorNode(typeNode);
				expr = oper;
				exprNode = opNode;
				oper.Terminate(t);
				opNode.Terminate(t); 
				
			}
		}
	}

	void ShiftExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		Token start;
		
		AddExpr(out expr, out exprNode);
		BinaryOperator oper = null; 
		BinaryOperatorNode opNode = null;
		
		while (IsShift()) {
			if (la.kind == 102) {
				Get();
				oper = new LeftShiftOperator(t, this); 
				opNode = new LeftShiftOperatorNode(t);
				
			} else if (la.kind == 94) {
				Get();
				start = t; 
				Expect(94);
				oper = new RightShiftOperator(t, this); 
				opNode = new RightShiftOperatorNode(start, t);
				
			} else SynErr(216);
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			AddExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void AddExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null; 
		exprNode = null;
		
		MulExpr(out expr, out exprNode);
		BinaryOperator oper = null; 
		BinaryOperatorNode opNode = null;
		
		while (la.kind == 103 || la.kind == 109) {
			if (la.kind == 109) {
				Get();
				oper = new AddOperator(t, this); 
				opNode = new AddOperatorNode(t);
				
			} else {
				Get();
				oper = new SubtractOperator(t, this); 
				opNode = new SubtractOperatorNode(t);
				
			}
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			BinaryOperator rightExpr; 
			BinaryOperatorNode rgNode;
			
			MulExpr(out rightExpr, out rgNode);
			BindBinaryOperator(oper, opNode, unExpr, unaryNode, rightExpr, rgNode);
			expr = oper;
			exprNode = opNode;
			
		}
	}

	void MulExpr(out BinaryOperator expr, out BinaryOperatorNode exprNode) {
		expr = null;
		BinaryOperator oper = null; 
		exprNode = null;
		BinaryOperatorNode opNode = null;
		
		while (la.kind == 117 || la.kind == 142 || la.kind == 143) {
			if (la.kind == 117) {
				Get();
				oper = new MultiplyOperator(t, this); 
				opNode = new MultiplyOperatorNode(t);
				
			} else if (la.kind == 142) {
				Get();
				oper = new DivideOperator(t, this); 
				opNode = new DivideOperatorNode(t);
				
			} else {
				Get();
				oper = new ModuloOperator(t, this); 
				opNode = new ModuloOperatorNode(t);
				
			}
			oper.LeftOperand = expr;
			opNode.LeftOperand = exprNode;
			Expression unExpr; 
			ExpressionNode unaryNode;
			
			Unary(out unExpr, out unaryNode);
			oper.RightOperand = unExpr;
			expr = oper;
			oper.Terminate(t);
			opNode.RightOperand = unaryNode;
			exprNode = opNode;
			opNode.Terminate(t);
			
		}
	}

	void Primary(out Expression expr, out ExpressionNode exprNode) {
		Expression innerExpr = null;
		expr = null;
		// :::
		ExpressionNode innerNode = null;
		exprNode = null;
		
		switch (la.kind) {
		case 2: case 3: case 4: case 5: case 29: case 47: case 70: {
			Literal(out innerExpr, out innerNode);
			break;
		}
		case 99: {
			Get();
			var pExprNode = new ParenthesisExpressionNode(t); 
			
			Expression(out innerExpr, out innerNode);
			pExprNode.Expression = innerNode;
			innerNode = pExprNode;
			
			Expect(114);
			if (innerExpr != null) innerExpr.BracketsUsed = true; 
			pExprNode.Terminate(t);
			
			break;
		}
		case 9: case 11: case 14: case 19: case 23: case 32: case 39: case 44: case 48: case 59: case 61: case 65: case 73: case 74: case 77: {
			PrimitiveNamedLiteral(out innerExpr, out innerNode);
			break;
		}
		case 1: {
			NamedLiteral(out innerExpr, out innerNode);
			break;
		}
		case 68: {
			Get();
			innerExpr = new ThisLiteral(t, this); 
			innerNode = new ThisNode(t);
			
			break;
		}
		case 8: {
			Get();
			innerExpr = new BaseLiteral(t, this); 
			innerNode = new BaseNode(t);
			
			break;
		}
		case 46: {
			NewOperator(out innerExpr);
			break;
		}
		case 72: {
			TypeOfOperator(out innerExpr, out innerNode);
			break;
		}
		case 15: {
			CheckedOperator(out innerExpr, out innerNode);
			break;
		}
		case 75: {
			UncheckedOperator(out innerExpr, out innerNode);
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
			SizeOfOperator(out innerExpr, out innerNode);
			break;
		}
		default: SynErr(217); break;
		}
		Expression curExpr = innerExpr; 
		var curExprNode = innerNode;
		
		while (StartOf(31)) {
			switch (la.kind) {
			case 96: {
				Get();
				curExpr = new PostIncrementOperator(t, this, innerExpr); 
				var incNode = new PostIncrementOperatorNode(t);
				incNode.Operand = curExprNode;
				curExprNode = incNode;
				
				break;
			}
			case 89: {
				Get();
				curExpr = new PostDecrementOperator(t, this, innerExpr); 
				var decNode = new PostDecrementOperatorNode(t);
				decNode.Operand = curExprNode;
				curExprNode = decNode;
				
				break;
			}
			case 144: {
				Get();
				NamedLiteral nl; 
				SimpleNameNode snlNode;
				var ctypeNode = new CTypeMemberAccessOperatorNode(t);
				
				SimpleNamedLiteral(out nl, out snlNode);
				curExpr = new CTypeMemberAccessOperator(t, innerExpr, nl); 
				ctypeNode.ScopeOperand = curExprNode;
				ctypeNode.MemberName = snlNode;
				curExprNode = ctypeNode;
				
				break;
			}
			case 91: {
				Get();
				NamedLiteral nl; 
				SimpleNameNode snlNode;
				var maNode = new MemberAccessOperatorNode(t);
				
				SimpleNamedLiteral(out nl, out snlNode);
				curExpr = new MemberAccessOperator(t, innerExpr, nl); 
				maNode.ScopeOperand = curExprNode;
				maNode.MemberName = snlNode;
				curExprNode = maNode;
				
				break;
			}
			case 99: {
				Get();
				ArgumentListOperator alop = new ArgumentListOperator(t, this, innerExpr); 
				var invNode = new MethodInvocationOperatorNode(t);
				invNode.ScopeOperand = curExprNode;
				
				CurrentArgumentList(alop.Arguments, invNode.Arguments);
				Expect(114);
				curExpr = alop; 
				invNode.Terminate(t);
				curExprNode = invNode;
				
				break;
			}
			case 98: {
				Get();
				ArrayIndexerOperator aiop = new ArrayIndexerOperator(t, this, innerExpr); 
				var indNode = new ArrayIndexerInvocationOperatorNode(t);
				indNode.ScopeOperand = curExprNode;
				
				ArrayIndexer(aiop, indNode.Arguments);
				curExpr = aiop; 
				Expect(113);
				indNode.Terminate(t); 
				break;
			}
			}
			curExpr.Terminate(t); 
		}
		expr = curExpr; 
		exprNode = curExprNode;
		
	}

	void Literal(out Expression value, out ExpressionNode valNode) {
		value = null; 
		// :::
		valNode = null;
		
		switch (la.kind) {
		case 2: {
			Get();
			value = IntegerConstant.Create(t, this); 
			valNode = IntegerConstantNode.Create(t);
			
			break;
		}
		case 3: {
			Get();
			value = RealConstant.Create(t, this); 
			valNode = RealConstantNode.Create(t);
			
			break;
		}
		case 4: {
			Get();
			value = new CharLiteral(t, this); 
			valNode = new CharNode(t);
			
			break;
		}
		case 5: {
			Get();
			value = new StringLiteral(t, this); 
			valNode = new StringNode(t);
			
			break;
		}
		case 70: {
			Get();
			value = new TrueLiteral(t, this); 
			valNode = new TrueNode(t);
			
			break;
		}
		case 29: {
			Get();
			value = new FalseLiteral(t, this); 
			valNode = new FalseNode(t);
			
			break;
		}
		case 47: {
			Get();
			value = new NullLiteral(t, this); 
			valNode = new NullNode(t);
			
			break;
		}
		default: SynErr(218); break;
		}
	}

	void PrimitiveNamedLiteral(out Expression expr, out ExpressionNode exprNode) {
		expr = null; 
		exprNode = null;
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
		default: SynErr(219); break;
		}
		expr = pml; 
		var pnNode = new PrimitiveNamedNode(t);
		exprNode = pnNode;
		
		Expect(91);
		pnNode.SeparatorToken = t; 
		Expect(1);
		pml.Name = t.val;
		pml.Terminate(t);
		// :::
		pnNode.IdentifierToken = t;
		pnNode.Terminate(t);
		
	}

	void NamedLiteral(out Expression expr, out ExpressionNode exprNode) {
		expr = null; 
		// :::
		exprNode = null;
		
		Expect(1);
		NamedLiteral nl = new NamedLiteral(t, this); 
		expr = nl;
		nl.Name = t.val;
		// :::
		var nlNode = new ScopedNameNode(t);
		exprNode = nlNode;
		
		if (la.kind == 92) {
			Get();
			nl.IsGlobalScope = true; 
			nlNode.QualifierSeparatorToken = t;
			nlNode.QualifierToken = nlNode.IdentifierToken;
			
			Expect(1);
			nl.Name = t.val; 
			nlNode.IdentifierToken = t;
			
		}
		TypeArgumentListNode argList; 
		if (IsGeneric()) {
			TypeArgumentList(nl.TypeArguments, out argList);
			nlNode.Arguments = argList; 
		}
		nl.Terminate(t); 
		// :::
		nlNode.Terminate(t);
		
	}

	void NewOperator(out Expression expr) {
		Expect(46);
		NewOperator nop = new NewOperator(t, this); 
		expr = nop; 
		TypeReference typeRef; 
		if (la.kind == 97) {
			AnonymousObjectInitializer(nop);
		} else if (StartOf(32)) {
			TypeOrNamespaceNode typeNode; 
			NonArrayType(out typeRef, out typeNode);
			NewOperatorWithType(nop, typeRef);
		} else if (la.kind == 98) {
			ImplicitArrayCreation(nop);
			nop.Kind = NewOperatorKind.UntypedArrayInitialization; 
		} else SynErr(220);
	}

	void TypeOfOperator(out Expression expr, out ExpressionNode exprNode) {
		Expect(72);
		TypeOfOperator top = new TypeOfOperator(t, this); 
		var topNode = new TypeofOperatorNode(t);
		exprNode = topNode;
		
		Expect(99);
		expr = top; 
		topNode.OpenParenthesis = t;
		TypeReference typeRef; 
		TypeOrNamespaceNode typeNode;
		
		Type(out typeRef, true, out typeNode);
		top.Type = typeRef; 
		topNode.TypeName = typeNode;
		
		Expect(114);
		top.Terminate(t); 
		topNode.CloseParenthesis = t;
		exprNode.Terminate(t);
		
	}

	void CheckedOperator(out Expression expr, out ExpressionNode exprNode) {
		Expect(15);
		CheckedOperator cop = new CheckedOperator(t, this); 
		var copNode = new CheckedOperatorNode(t);
		exprNode = copNode;
		
		Expect(99);
		expr = cop; 
		copNode.OpenParenthesis = t;
		Expression innerExpr;
		ExpressionNode innerNode;
		
		Expression(out innerExpr, out innerNode);
		cop.Operand = innerExpr; 
		copNode.Expression = innerNode;
		
		Expect(114);
		cop.Terminate(t); 
		copNode.CloseParenthesis = t;
		exprNode.Terminate(t);
		
	}

	void UncheckedOperator(out Expression expr, out ExpressionNode exprNode) {
		Expect(75);
		UncheckedOperator uop = new UncheckedOperator(t, this); 
		var uopNode = new UncheckedOperatorNode(t);
		exprNode = uopNode;
		
		Expect(99);
		expr = uop;
		uopNode.OpenParenthesis = t;
		Expression innerExpr; 
		ExpressionNode innerNode;
		
		Expression(out innerExpr, out innerNode);
		uop.Operand = innerExpr; 
		uopNode.Expression = innerNode;
		
		Expect(114);
		uop.Terminate(t); 
		uopNode.CloseParenthesis = t;
		exprNode.Terminate(t);
		
	}

	void DefaultOperator(out Expression expr) {
		Expect(20);
		DefaultOperator dop = new DefaultOperator(t, this); 
		Expect(99);
		expr = dop; 
		Expression innerExpr; 
		ExpressionNode primNode;
		
		Primary(out innerExpr, out primNode);
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

	void SizeOfOperator(out Expression expr, out ExpressionNode exprNode) {
		Expect(62);
		SizeOfOperator sop = new SizeOfOperator(t, this); 
		var sopNode = new SizeofOperatorNode(t);
		exprNode = sopNode;
		
		Expect(99);
		expr = sop; 
		sopNode.OpenParenthesis = t;
		TypeReference typeRef; 
		TypeOrNamespaceNode typeNode;
		
		Type(out typeRef, true, out typeNode);
		sop.Type = typeRef; 
		sopNode.TypeName = typeNode;
		
		Expect(114);
		sop.Terminate(t); 
		sopNode.CloseParenthesis = t;
		exprNode.Terminate(t);
		
	}

	void SimpleNamedLiteral(out NamedLiteral expr, out SimpleNameNode snlNode) {
		Expect(1);
		expr = new NamedLiteral(t, this); 
		snlNode = new SimpleNameNode(t);
		
		TypeArgumentListNode argList; 
		if (IsGeneric()) {
			TypeArgumentList(expr.TypeArguments, out argList);
			snlNode.Arguments = argList; 
		}
		expr.Terminate(t); 
		snlNode.Terminate(t);
		
	}

	void ArrayIndexer(ArrayIndexerOperator indexer, ArgumentNodeCollection argNodes) {
		Expression expr;
		ExpressionNode exprNode; 
		Argument arg = new Argument(t, this);
		Token separator = null;
		
		Expression(out expr, out exprNode);
		indexer.Indexers.Add(expr); 
		// :::
		if (argNodes != null)
		{
		  var argNode = new ArgumentNode(exprNode == null ? t : exprNode.StartToken);
		  argNode.Expression = exprNode;
		  argNode.Terminate(t);
		  argNodes.Add(argNode);
		}
		
		while (la.kind == 88) {
			Get();
			separator = t; 
			Expression(out expr, out exprNode);
			indexer.Indexers.Add(expr); 
			// :::
			if (argNodes != null)
			{
			  var argNode = new ArgumentContinuationNode(separator);
			  argNode.Expression = exprNode;
			  argNode.Terminate(t);
			  argNodes.Add(argNode);
			}
			
		}
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
		ExpressionNode exprNode;
		
		if (la.kind == 99) {
			Get();
			CurrentArgumentList(nop.Arguments, null);
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
			Expression(out dimExpr, out exprNode);
			nop.Dimensions.Add(dimExpr); 
			while (la.kind == 88) {
				Get();
				Expression(out dimExpr, out exprNode);
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
		} else SynErr(221);
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
		ExpressionNode exprNode;
		
		if (IsMemberInitializer()) {
			Expect(1);
			Token start = t; 
			Expect(86);
			Expression(out expr, out exprNode);
			init = new MemberDeclarator(start, this, expr, start.val); 
		} else if (StartOf(27)) {
			Token start = la; 
			ExpressionNode primNode;
			
			Primary(out expr, out primNode);
			init = new MemberDeclarator(start, this, expr, start.val, true); 
		} else if (StartOf(34)) {
			Token start = la; 
			TypeReference typeRef;
			TypeOrNamespaceNode typeNode;
			
			PredefinedType(out typeRef, out typeNode);
			Expect(91);
			Expect(1);
			init = new MemberDeclarator(start, this, typeRef, start.val); 
		} else SynErr(222);
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
		} else if (StartOf(20)) {
			CollectionInitializer(out init);
		} else SynErr(223);
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
		ExpressionNode exprNode;
		
		if (IsValueInitializer()) {
			Expression(out expr, out exprNode);
			init = new ExpressionInitializer(t, this, expr); 
		} else if (la.kind == 97) {
			ExpressionListInitializer listInit = new ExpressionListInitializer(t, this); 
			init = listInit; 
			Get();
			Expression(out expr, out exprNode);
			listInit.Initializers.Add(new ExpressionInitializer(t, this, expr)); 
			while (la.kind == 88) {
				Get();
				Expression(out expr, out exprNode);
				listInit.Initializers.Add(new ExpressionInitializer(t, this, expr)); 
			}
			Expect(112);
		} else SynErr(224);
		if (init != null) init.Terminate(t); 
	}

	void MemberInitializer(out MemberInitializer init) {
		ExpressionNode exprNode; 
		Expect(1);
		Token startToken = t; 
		Expect(86);
		Expression expr; 
		init = null; 
		if (IsValueInitializer()) {
			Expression(out expr, out exprNode);
			init = new MemberInitializer(startToken, this, expr); 
		} else if (la.kind == 97) {
			Initializer compoundInit; 
			ObjectOrCollectionInitializer(out compoundInit);
			init = new MemberInitializer(startToken, this, compoundInit); 
		} else SynErr(225);
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
		TypeOrNamespaceNode typeNode; 
		Type(out typeRef, false, out typeNode);
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

	void TypeParameter(out TypeParameter tp, out AttributeDecorationNodeCollection attrNodes,
out Token identifier) {
		AttributeCollection attrs = new AttributeCollection(); 
		// :::
		attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrs, attrNodes);
		Expect(1);
		tp = new TypeParameter(t, this);
		tp.Name = t.val;
		tp.AssignAttributes(attrs);
		// :::
		identifier = t;
		
	}

	void TypeParameterConstraintTag(out ConstraintElement element, out ParameterConstraintTagNode tag) {
		element = null;
		tag = null;
		
		if (la.kind == 16) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.Class); 
			// :::
			tag = new ParameterConstraintTagNode(t);       
			
		} else if (la.kind == 66) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.Struct); 
			// :::
			tag = new ParameterConstraintTagNode(t);       
			
		} else if (la.kind == 46) {
			Get();
			element = new ConstraintElement(t, this, ConstraintClassification.New); 
			var start = t;
			
			Expect(99);
			var openPar = t; 
			Expect(114);
			tag = new ParameterConstraintTagNode(start, openPar, t); 
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			Token elemToken = t; 
			TypeReference typeRef; 
			// :::
			TypeOrNamespaceNode typeNode;
			
			ClassType(out typeRef, out typeNode);
			element = new ConstraintElement(elemToken, this, typeRef); 
			// :::
			tag = new ParameterConstraintTagNode(typeNode);       
			
		} else SynErr(226);
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
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,T,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,x,x,x, T,x,x,T, x,x,x,T, x,x,x,T, x,T,x,x, x,x,x,x, T,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
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
			case 147: s = "invalid TypeDeclaration"; break;
			case 148: s = "invalid TypeDeclaration"; break;
			case 149: s = "invalid EnumDeclaration"; break;
			case 150: s = "invalid ClassType"; break;
			case 151: s = "invalid ClassMemberDeclaration"; break;
			case 152: s = "invalid ClassMemberDeclaration"; break;
			case 153: s = "invalid StructMemberDeclaration"; break;
			case 154: s = "invalid StructMemberDeclaration"; break;
			case 155: s = "invalid StructMemberDeclaration"; break;
			case 156: s = "invalid IntegralType"; break;
			case 157: s = "this symbol not expected in EnumBody"; break;
			case 158: s = "this symbol not expected in EnumBody"; break;
			case 159: s = "invalid Expression"; break;
			case 160: s = "invalid Expression"; break;
			case 161: s = "invalid Type"; break;
			case 162: s = "invalid FormalParameterList"; break;
			case 163: s = "invalid EventDeclaration"; break;
			case 164: s = "invalid ConstructorDeclaration"; break;
			case 165: s = "invalid ConstructorDeclaration"; break;
			case 166: s = "invalid MethodDeclaration"; break;
			case 167: s = "invalid OperatorDeclaration"; break;
			case 168: s = "invalid CastOperatorDeclaration"; break;
			case 169: s = "invalid CastOperatorDeclaration"; break;
			case 170: s = "invalid EventAccessorDeclarations"; break;
			case 171: s = "invalid EventAccessorDeclarations"; break;
			case 172: s = "invalid AccessorDeclarations"; break;
			case 173: s = "invalid AccessorDeclarations"; break;
			case 174: s = "invalid AccessorDeclarations"; break;
			case 175: s = "invalid AccessorDeclarations"; break;
			case 176: s = "invalid OverloadableOp"; break;
			case 177: s = "invalid InterfaceMemberDeclaration"; break;
			case 178: s = "invalid InterfaceMemberDeclaration"; break;
			case 179: s = "invalid InterfaceMemberDeclaration"; break;
			case 180: s = "invalid InterfaceAccessors"; break;
			case 181: s = "invalid InterfaceAccessors"; break;
			case 182: s = "invalid LocalVariableDeclaration"; break;
			case 183: s = "invalid LocalVariableDeclarator"; break;
			case 184: s = "invalid VariableInitializer"; break;
			case 185: s = "invalid Attributes"; break;
			case 186: s = "invalid Keyword"; break;
			case 187: s = "invalid AttributeArguments"; break;
			case 188: s = "invalid PrimitiveType"; break;
			case 189: s = "invalid PointerOrArray"; break;
			case 190: s = "invalid NonArrayType"; break;
			case 191: s = "invalid TypeInRelExpr"; break;
			case 192: s = "invalid PredefinedType"; break;
			case 193: s = "invalid Statement"; break;
			case 194: s = "invalid EmbeddedStatement"; break;
			case 195: s = "invalid EmbeddedStatement"; break;
			case 196: s = "invalid StatementExpression"; break;
			case 197: s = "invalid ForEachStatement"; break;
			case 198: s = "invalid GotoStatement"; break;
			case 199: s = "invalid TryFinallyBlock"; break;
			case 200: s = "invalid UsingStatement"; break;
			case 201: s = "invalid ForInitializer"; break;
			case 202: s = "invalid CatchClauses"; break;
			case 203: s = "invalid Unary"; break;
			case 204: s = "invalid Unary"; break;
			case 205: s = "invalid AssignmentOperator"; break;
			case 206: s = "invalid SwitchLabel"; break;
			case 207: s = "invalid LambdaFunctionSignature"; break;
			case 208: s = "invalid LambdaFunctionSignature"; break;
			case 209: s = "invalid LambdaFunctionBody"; break;
			case 210: s = "invalid FromClause"; break;
			case 211: s = "invalid QueryBody"; break;
			case 212: s = "invalid QueryBodyClause"; break;
			case 213: s = "invalid JoinClause"; break;
			case 214: s = "invalid RelExpr"; break;
			case 215: s = "invalid RelExpr"; break;
			case 216: s = "invalid ShiftExpr"; break;
			case 217: s = "invalid Primary"; break;
			case 218: s = "invalid Literal"; break;
			case 219: s = "invalid PrimitiveNamedLiteral"; break;
			case 220: s = "invalid NewOperator"; break;
			case 221: s = "invalid NewOperatorWithType"; break;
			case 222: s = "invalid MemberDeclarator"; break;
			case 223: s = "invalid ObjectOrCollectionInitializer"; break;
			case 224: s = "invalid ElementInitializer"; break;
			case 225: s = "invalid MemberInitializer"; break;
			case 226: s = "invalid TypeParameterConstraintTag"; break;

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