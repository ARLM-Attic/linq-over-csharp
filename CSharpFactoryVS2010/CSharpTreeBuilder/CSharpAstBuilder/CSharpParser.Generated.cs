using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder {



// disable warnings about missing XML comments
#pragma warning disable 1591 

// ==================================================================================
/// <summary>
/// This class implements the C# syntax parser functionality.
/// </summary>
// ==================================================================================
public partial class CSharpParser 
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
	public const int _virtual = 79;
	public const int _void = 80;
	public const int _volatile = 81;
	public const int _while = 82;
	public const int _and = 83;
	public const int _andassgn = 84;
	public const int _assgn = 85;
	public const int _colon = 86;
	public const int _comma = 87;
	public const int _dec = 88;
	public const int _divassgn = 89;
	public const int _dot = 90;
	public const int _dblcolon = 91;
	public const int _eq = 92;
	public const int _gt = 93;
	public const int _gteq = 94;
	public const int _inc = 95;
	public const int _lbrace = 96;
	public const int _lbrack = 97;
	public const int _lpar = 98;
	public const int _lshassgn = 99;
	public const int _lt = 100;
	public const int _ltlt = 101;
	public const int _minus = 102;
	public const int _minusassgn = 103;
	public const int _modassgn = 104;
	public const int _neq = 105;
	public const int _not = 106;
	public const int _orassgn = 107;
	public const int _plus = 108;
	public const int _plusassgn = 109;
	public const int _question = 110;
	public const int _rbrace = 111;
	public const int _rbrack = 112;
	public const int _rpar = 113;
	public const int _scolon = 114;
	public const int _tilde = 115;
	public const int _times = 116;
	public const int _timesassgn = 117;
	public const int _xorassgn = 118;
	public const int _larrow = 119;
	public const int maxT = 135;
	public const int _ppDefine = 136;
	public const int _ppUndef = 137;
	public const int _ppIf = 138;
	public const int _ppElif = 139;
	public const int _ppElse = 140;
	public const int _ppEndif = 141;
	public const int _ppLine = 142;
	public const int _ppError = 143;
	public const int _ppWarning = 144;
	public const int _ppPragma = 145;
	public const int _ppRegion = 146;
	public const int _ppEndReg = 147;
	public const int _cBlockCom = 148;
	public const int _cLineCom = 149;

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
				if (la.kind == 136) {
				AddConditionalDirective(la); 
				}
				if (la.kind == 137) {
				RemoveConditionalDirective(la); 
				}
				if (la.kind == 138) {
				IfPragma(la); 
				}
				if (la.kind == 139) {
				ElifPragma(la); 
				}
				if (la.kind == 140) {
				ElsePragma(la); 
				}
				if (la.kind == 141) {
				EndifPragma(la); 
				}
				if (la.kind == 142) {
				LinePragma(la); 
				}
				if (la.kind == 143) {
				ErrorPragma(la); 
				}
				if (la.kind == 144) {
				WarningPragma(la); 
				}
				if (la.kind == 145) {
				PragmaPragma(la); 
				}
				if (la.kind == 146) {
				RegionPragma(la); 
				}
				if (la.kind == 147) {
				EndRegionPragma(la); 
				}
				if (la.kind == 148) {
				HandleBlockComment(la); 
				}
				if (la.kind == 149) {
				HandleLineComment(la); 
				}

			  la = t;
      }
    }
	
	  #region Parser methods generated by CoCo
	  
	void CS3() {
		while (IsExternAliasDirective()) {
			ExternAliasDirective(SourceFileNode);
		}
		while (la.kind == 78) {
			UsingDirective(SourceFileNode);
		}
		while (IsGlobalAttrTarget()) {
			GlobalAttributes();
		}
		while (StartOf(1)) {
			NamespaceMemberDeclaration(SourceFileNode);
		}
	}

	void ExternAliasDirective(NamespaceScopeNode parentNode) {
		Token start;
		Token alias;
		Token identifier;
		
		Expect(28);
		SignRealToken(); 
		start = t;
		
		Expect(1);
		if (t.val != "alias") Error1003(t, "alias"); 
		alias = t;
		
		Expect(1);
		identifier = t; 
		Expect(114);
		var eaNode = parentNode.AddExternAlias(start, alias, identifier, t); 
		SetCommentOwner(eaNode);
		
	}

	void UsingDirective(NamespaceScopeNode parentNode) {
		Token alias = null;
		Token eq = null;
		TypeOrNamespaceNode nsNode = null;
		
		Expect(78);
		SignRealToken();
		Token start = t;
		
		if (IsAssignment()) {
			Expect(1);
			alias = t; 
			Expect(85);
			eq = t; 
		}
		TypeName(out nsNode);
		Expect(114);
		var node = (alias == null)
		 ? parentNode.AddUsing(start, nsNode, t)
		 : parentNode.AddUsingWithAlias(start, alias, eq, nsNode, t);
		SetCommentOwner(node);
		Terminate(node);
		
	}

	void GlobalAttributes() {
		AttributeDecorationNode globAttrNode = null; 
		Expect(97);
		SignRealToken();
		globAttrNode = new AttributeDecorationNode(t);
		
		Expect(1);
		globAttrNode.TargetToken = t; 
		Expect(86);
		var separator = t; 
		AttributeNode attrNode; 
		Attribute(out attrNode);
		globAttrNode.Attributes.Add(separator, attrNode); 
		while (NotFinalComma()) {
			Expect(87);
			separator = t; 
			Attribute(out attrNode);
			globAttrNode.Attributes.Add(separator, attrNode); 
		}
		if (la.kind == 87) {
			Get();
			globAttrNode.OrphanSeparator = t; 
		}
		Expect(112);
		Terminate(globAttrNode);
		SourceFileNode.GlobalAttributes.Add(globAttrNode);
		
	}

	void NamespaceMemberDeclaration(NamespaceScopeNode parentNode) {
		if (la.kind == 45) {
			Get();
			SignRealToken();
			Token startToken = t; 
			var nsDecl = new NamespaceDeclarationNode(parentNode, t);
			SetCommentOwner(nsDecl);
			
			Expect(1);
			nsDecl.NameTags.Add(t); 
			while (la.kind == 90) {
				Get();
				var sepToken = t; 
				Expect(1);
				nsDecl.NameTags.Add(sepToken, t); 
			}
			Expect(96);
			nsDecl.OpenBracket = t; 
			while (IsExternAliasDirective()) {
				ExternAliasDirective(nsDecl);
			}
			while (la.kind == 78) {
				UsingDirective(nsDecl);
			}
			while (StartOf(1)) {
				NamespaceMemberDeclaration(nsDecl);
			}
			Expect(111);
			nsDecl.CloseBracket = t;
			Terminate(nsDecl);
			
			if (la.kind == 114) {
				Get();
				Terminate(nsDecl); 
			}
			parentNode.NamespaceDeclarations.Add(nsDecl);
			parentNode.InScopeDeclarations.Add(nsDecl);
			
		} else if (StartOf(2)) {
			var mod = new ModifierNodeCollection();
			var attrNodes = new AttributeDecorationNodeCollection();
			
			AttributeDecorations(attrNodes);
			ModifierList(mod);
			TypeDeclarationNode typeDecl; 
			TypeDeclaration(null, out typeDecl);
			typeDecl.AttributeDecorations = attrNodes;
			typeDecl.Modifiers = mod;
			typeDecl.DeclaringNamespace = parentNode;
			parentNode.TypeDeclarations.Add(typeDecl);
			parentNode.InScopeDeclarations.Add(typeDecl);
			
		} else SynErr(136);
	}

	void TypeName(out TypeOrNamespaceNode resultNode) {
		resultNode = null;
		Token separator = null;
		Token identifier = null;
		TypeOrNamespaceNodeCollection argList = null;
		
		Expect(1);
		resultNode = new TypeOrNamespaceNode(t);
		SetCommentOwner(resultNode);
		identifier = t; 
		
		if (la.kind == 91) {
			Get();
			resultNode.QualifierToken = identifier;
			separator = t; 
			
			Expect(1);
			identifier = t; 
		}
		if (la.kind == 100) {
			TypeArgumentList(out argList);
		}
		var tagNode = new TypeTagNode(identifier, argList);
		SetCommentOwner(tagNode);
		resultNode.TypeTags.Add(separator, tagNode); 
		
		while (la.kind == 90) {
			Get();
			separator = t;
			argList = null;
			
			Expect(1);
			identifier = t; 
			if (la.kind == 100) {
				TypeArgumentList(out argList);
			}
			tagNode = new TypeTagNode(identifier, argList);
			SetCommentOwner(tagNode);
			resultNode.TypeTags.Add(separator, tagNode); 
			
		}
		Terminate(resultNode); 
	}

	void Attribute(out AttributeNode attrNode) {
		attrNode = new AttributeNode(la);
		SetCommentOwner(attrNode);
		TypeOrNamespaceNode nsNode = null;
		
		TypeName(out nsNode);
		attrNode.TypeName = nsNode; 
		if (la.kind == 98) {
			AttributeArguments(attrNode);
		}
		Terminate(attrNode); 
	}

	void AttributeDecorations(AttributeDecorationNodeCollection attrNodes) {
		AttributeDecorationNode attrNode; 
		while (la.kind == 97) {
			Attributes(out attrNode);
			attrNodes.Add(attrNode); 
		}
	}

	void ModifierList(ModifierNodeCollection mods) {
		while (StartOf(3)) {
			switch (la.kind) {
			case 46: {
				Get();
				break;
			}
			case 55: {
				Get();
				break;
			}
			case 54: {
				Get();
				break;
			}
			case 41: {
				Get();
				break;
			}
			case 53: {
				Get();
				break;
			}
			case 76: {
				Get();
				break;
			}
			case 64: {
				Get();
				break;
			}
			case 56: {
				Get();
				break;
			}
			case 81: {
				Get();
				break;
			}
			case 79: {
				Get();
				break;
			}
			case 60: {
				Get();
				break;
			}
			case 51: {
				Get();
				break;
			}
			case 6: {
				Get();
				break;
			}
			case 28: {
				Get();
				break;
			}
			}
			mods.Add(t); 
		}
	}

	void TypeDeclaration(TypeDeclarationNode declaringType, out TypeDeclarationNode typeDecl) {
		typeDecl = null; 
		Token partialToken = null;
		
		if (StartOf(4)) {
			if (la.kind == 120) {
				Get();
				partialToken = t; 
			}
			if (la.kind == 16) {
				ClassDeclaration(out typeDecl);
			} else if (la.kind == 66) {
				StructDeclaration(out typeDecl);
			} else if (la.kind == 40) {
				InterfaceDeclaration(out typeDecl);
			} else SynErr(137);
		} else if (la.kind == 25) {
			EnumDeclaration(out typeDecl);
		} else if (la.kind == 21) {
			DelegateDeclaration(out typeDecl);
		} else SynErr(138);
		if (typeDecl != null)
		{
		  typeDecl.PartialToken = partialToken;
		  typeDecl.DeclaringType = declaringType;
		  Terminate(typeDecl);
		}
		
	}

	void ClassDeclaration(out TypeDeclarationNode typeDecl) {
		Expect(16);
		var start = t; 
		Expect(1);
		var classDecl = new ClassDeclarationNode(start, t);
		SetCommentOwner(classDecl);
		typeDecl = classDecl;
		
		if (la.kind == 100) {
			TypeParameterList(typeDecl);
		}
		if (la.kind == 86) {
			BaseTypeList(typeDecl);
		}
		while (la.kind == 122) {
			TypeParameterConstraintNode constrNode; 
			TypeParameterConstraintsClause(out constrNode);
			typeDecl.TypeParameterConstraints.Add(constrNode); 
		}
		ClassBody(classDecl);
		Terminate(typeDecl); 
		if (la.kind == 114) {
			Get();
			Terminate(typeDecl); 
		}
	}

	void StructDeclaration(out TypeDeclarationNode typeDecl) {
		Expect(66);
		var start = t; 
		Expect(1);
		var structDecl = new StructDeclarationNode(start, t);
		SetCommentOwner(structDecl);
		typeDecl = structDecl;
		
		if (la.kind == 100) {
			TypeParameterList(typeDecl);
		}
		if (la.kind == 86) {
			BaseTypeList(typeDecl);
		}
		while (la.kind == 122) {
			TypeParameterConstraintNode constrNode; 
			TypeParameterConstraintsClause(out constrNode);
			typeDecl.TypeParameterConstraints.Add(constrNode); 
		}
		StructBody(structDecl);
		Terminate(typeDecl); 
		if (la.kind == 114) {
			Get();
			Terminate(typeDecl); 
		}
	}

	void InterfaceDeclaration(out TypeDeclarationNode typeDecl) {
		Expect(40);
		var start = t; 
		Expect(1);
		var intfDecl = new InterfaceDeclarationNode(start, t);
		SetCommentOwner(intfDecl);
		typeDecl = intfDecl;
		
		if (la.kind == 100) {
			TypeParameterList(typeDecl);
		}
		if (la.kind == 86) {
			BaseTypeList(typeDecl);
		}
		while (la.kind == 122) {
			TypeParameterConstraintNode constrNode; 
			TypeParameterConstraintsClause(out constrNode);
			typeDecl.TypeParameterConstraints.Add(constrNode); 
		}
		Expect(96);
		intfDecl.OpenBrace = t; 
		while (StartOf(5)) {
			InterfaceMemberDeclaration(intfDecl);
		}
		Expect(111);
		intfDecl.CloseBrace = t;
		Terminate(typeDecl);
		
		if (la.kind == 114) {
			Get();
			Terminate(typeDecl); 
		}
	}

	void EnumDeclaration(out TypeDeclarationNode typeDecl) {
		Expect(25);
		var start = t; 
		Expect(1);
		var enumDecl = new EnumDeclarationNode(start, t);
		SetCommentOwner(enumDecl);
		typeDecl = enumDecl;
		
		if (la.kind == 86) {
			Get();
			TypeOrNamespaceNode typeNode = null; 
			if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
				ClassType(out typeNode);
			} else if (StartOf(6)) {
				IntegralType(out typeNode);
			} else SynErr(139);
			enumDecl.EnumBase = typeNode; 
		}
		EnumBody(enumDecl);
		Terminate(typeDecl); 
		if (la.kind == 114) {
			Get();
			Terminate(typeDecl); 
		}
	}

	void DelegateDeclaration(out TypeDeclarationNode typeDecl) {
		Expect(21);
		var start = t;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeNode);
		Expect(1);
		var ddNode = new DelegateDeclarationNode(start, t);
		SetCommentOwner(ddNode);
		typeDecl = ddNode;
		ddNode.TypeName = typeNode;
		
		if (la.kind == 100) {
			TypeParameterList(typeDecl);
		}
		Expect(98);
		ddNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(ddNode.FormalParameters);
		}
		Expect(113);
		Terminate(ddNode.FormalParameters); 
		while (la.kind == 122) {
			TypeParameterConstraintNode constrNode; 
			TypeParameterConstraintsClause(out constrNode);
			typeDecl.TypeParameterConstraints.Add(constrNode); 
		}
		Expect(114);
		Terminate(typeDecl); 
	}

	void TypeParameterList(ITypeParameterHolder paramNode) {
		Token identifier;
		AttributeDecorationNodeCollection attrNodes;
		
		Expect(100);
		Start(paramNode.TypeParameters); 
		TypeParameter(out attrNodes, out identifier);
		var newParNode = new TypeParameterNode(identifier, attrNodes);
		SetCommentOwner(newParNode);
		paramNode.TypeParameters.Add(newParNode); 
		
		while (la.kind == 87) {
			Get();
			var separator= t; 
			TypeParameter(out attrNodes, out identifier);
			newParNode = new TypeParameterNode(identifier, attrNodes);
			SetCommentOwner(newParNode);
			paramNode.TypeParameters.Add(separator, newParNode); 
			
		}
		Expect(93);
		Terminate(paramNode.TypeParameters); 
	}

	void BaseTypeList(TypeDeclarationNode typeDecl) {
		TypeOrNamespaceNode typeNode; 
		Expect(86);
		typeDecl.ColonToken = t; 
		ClassType(out typeNode);
		typeDecl.BaseTypes.Add(typeNode);
		SetCommentOwner(typeNode);
		
		while (la.kind == 87) {
			Get();
			var separator = t; 
			ClassType(out typeNode);
			typeNode.SeparatorToken = separator;
			typeDecl.BaseTypes.Add(typeNode); 
			SetCommentOwner(typeNode);
			
		}
	}

	void TypeParameterConstraintsClause(out TypeParameterConstraintNode constrNode) {
		Token start;
		Token identifier;
		
		Expect(122);
		start = t; 
		Expect(1);
		identifier = t; 
		Expect(86);
		constrNode = new TypeParameterConstraintNode(start, identifier, t);
		SetCommentOwner(constrNode);
		TypeParameterConstraintTagNode tag;
		
		TypeParameterConstraintTag(out tag);
		constrNode.ConstraintTags.Add(tag); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			TypeParameterConstraintTag(out tag);
			constrNode.ConstraintTags.Add(separator, tag); 
		}
		Terminate(constrNode); 
	}

	void ClassBody(ClassDeclarationNode typeDecl) {
		Expect(96);
		typeDecl.OpenBrace = t; 
		while (StartOf(8)) {
			var attrNodes = new AttributeDecorationNodeCollection(); 
			AttributeDecorations(attrNodes);
			var mod = new ModifierNodeCollection(); 
			ModifierList(mod);
			MemberDeclarationNode memNode; 
			ClassMemberDeclaration(attrNodes, mod, typeDecl, out memNode);
			if (memNode != null) 
			{
			  memNode.AttributeDecorations = attrNodes;
			  memNode.Modifiers = mod;
			  typeDecl.MemberDeclarations.Add(memNode); 
			}
			
		}
		Expect(111);
		typeDecl.CloseBrace = t; 
	}

	void ClassType(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (la.kind == 1) {
			TypeName(out typeNode);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
			} else {
				Get();
			}
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else SynErr(140);
	}

	void ClassMemberDeclaration(AttributeDecorationNodeCollection attrNodes, ModifierNodeCollection mod,
TypeDeclarationNode typeDecl, out MemberDeclarationNode memNode) {
		memNode = null; 
		if (StartOf(9)) {
			StructMemberDeclaration(attrNodes, mod, typeDecl, out memNode);
		} else if (la.kind == 115) {
			Get();
			var finNode = new FinalizerDeclarationNode(t);
			SetCommentOwner(finNode);
			memNode = finNode;
			
			Expect(1);
			finNode.IdentifierToken = t; 
			Expect(98);
			finNode.FormalParameters = new FormalParameterListNode(t); 
			Expect(113);
			Terminate(finNode.FormalParameters); 
			if (la.kind == 96) {
				BlockStatementNode blockNode; 
				Block(out blockNode);
				finNode.Body = blockNode; 
			} else if (la.kind == 114) {
				Get();
				finNode.ClosingSemicolon = t; 
			} else SynErr(141);
			Terminate(memNode); 
		} else SynErr(142);
	}

	void StructBody(StructDeclarationNode typeDecl) {
		Expect(96);
		typeDecl.OpenBrace = t; 
		while (StartOf(10)) {
			var attrNodes = new AttributeDecorationNodeCollection(); 
			AttributeDecorations(attrNodes);
			var mod = new ModifierNodeCollection(); 
			ModifierList(mod);
			MemberDeclarationNode memNode; 
			StructMemberDeclaration(attrNodes, mod, typeDecl, out memNode);
			if (memNode != null) 
			{
			  memNode.AttributeDecorations = attrNodes;
			  memNode.Modifiers = mod;
			  typeDecl.MemberDeclarations.Add(memNode); 
			}
			
		}
		Expect(111);
		typeDecl.CloseBrace = t; 
	}

	void StructMemberDeclaration(AttributeDecorationNodeCollection attrNodes, ModifierNodeCollection mod,
TypeDeclarationNode typeDecl, out MemberDeclarationNode memNode) {
		memNode = null; 
		if (la.kind == 17) {
			ConstMemberDeclaration(out memNode);
		} else if (la.kind == 26) {
			EventDeclaration(out memNode);
		} else if (la.kind == _ident && Peek(1).kind == _lpar) {
			ConstructorDeclaration(out memNode);
		} else if (IsPartialMethod()) {
			Expect(120);
			var partialToken = t;
			TypeOrNamespaceNode typeNode; 
			
			Type(out typeNode);
			TypeOrNamespaceNode nameNode; 
			MemberName(out nameNode);
			var metNode = new MethodDeclarationNode(partialToken);
			SetCommentOwner(metNode);
			metNode.TypeName = typeNode;
			metNode.MemberName = nameNode;
			memNode = metNode;
			
			MethodDeclaration(metNode);
		} else if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeNode);
			if (la.kind == 49) {
				var opNode = new OperatorDeclarationNode(typeNode.StartToken); 
				SetCommentOwner(opNode);
				opNode.TypeName = typeNode;
				memNode = opNode;
				
				OperatorDeclaration(opNode);
			} else if (IsFieldDecl()) {
				var fiNode = new FieldDeclarationNode(typeNode.StartToken); 
				SetCommentOwner(fiNode);
				fiNode.TypeName = typeNode;
				memNode = fiNode;
				
				FieldMemberDeclarators(fiNode);
				Expect(114);
				Terminate(memNode); 
			} else if (la.kind == 1) {
				TypeOrNamespaceNode nameNode; 
				MemberName(out nameNode);
				if (la.kind == 96) {
					var propNode = new PropertyDeclarationNode(typeNode.StartToken);
					SetCommentOwner(propNode);
					propNode.TypeName = typeNode; 
					propNode.MemberName = nameNode;
					memNode = propNode;
					
					PropertyDeclaration(propNode);
				} else if (la.kind == 90) {
					var indNode = new IndexerDeclarationNode(typeNode.StartToken);
					SetCommentOwner(indNode);
					indNode.TypeName = typeNode;
					indNode.MemberName = nameNode;
					memNode = indNode;
					
					Get();
					indNode.MemberNameSeparator = t; 
					IndexerDeclaration(indNode);
				} else if (la.kind == 98 || la.kind == 100) {
					var metNode = new MethodDeclarationNode(typeNode.StartToken);
					SetCommentOwner(metNode);
					metNode.TypeName = typeNode;
					metNode.MemberName = nameNode;
					memNode = metNode;
					
					MethodDeclaration(metNode);
				} else SynErr(143);
			} else if (la.kind == 68) {
				var indNode = new IndexerDeclarationNode(typeNode.StartToken);
				SetCommentOwner(indNode);
				indNode.TypeName = typeNode;
				memNode = indNode;
				
				IndexerDeclaration(indNode);
			} else SynErr(144);
		} else if (la.kind == 27 || la.kind == 37) {
			CastOperatorDeclaration(out memNode);
		} else if (StartOf(12)) {
			TypeDeclarationNode nestedTypeNode; 
			TypeDeclaration(typeDecl, out nestedTypeNode);
			nestedTypeNode.AttributeDecorations = attrNodes;
			nestedTypeNode.Modifiers = mod;
			nestedTypeNode.DeclaringNamespace = typeDecl.DeclaringNamespace;
			nestedTypeNode.DeclaringType = typeDecl;
			typeDecl.NestedDeclarations.Add(nestedTypeNode);
			typeDecl.NestedTypes.Add(nestedTypeNode);
			
		} else SynErr(145);
	}

	void IntegralType(out TypeOrNamespaceNode typeNode) {
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
		default: SynErr(146); break;
		}
		typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
	}

	void EnumBody(EnumDeclarationNode typeDecl) {
		Expect(96);
		typeDecl.OpenBrace = t; 
		if (la.kind == 1 || la.kind == 97) {
			EnumValueNode valNode; 
			EnumMemberDeclaration(out valNode);
			typeDecl.Values.Add(valNode); 
			while (NotFinalComma()) {
				Expect(87);
				var separator = t; 
				while (!(la.kind == 0 || la.kind == 1 || la.kind == 97)) {SynErr(147); Get();}
				EnumMemberDeclaration(out valNode);
				typeDecl.Values.Add(separator, valNode); 
			}
			if (la.kind == 87) {
				Get();
				typeDecl.OrphanSeparator = t; 
			}
		}
		while (!(la.kind == 0 || la.kind == 111)) {SynErr(148); Get();}
		Expect(111);
		typeDecl.CloseBrace = t; 
		Terminate(typeDecl);
		
	}

	void EnumMemberDeclaration(out EnumValueNode valNode) {
		valNode = null;
		var attrNodes = new AttributeDecorationNodeCollection();
		
		AttributeDecorations(attrNodes);
		Expect(1);
		valNode = new EnumValueNode(t);
		SetCommentOwner(valNode);
		valNode.AttributeDecorations = attrNodes;
		
		if (la.kind == 85) {
			Get();
			valNode.EqualToken = t;
			ExpressionNode exprNode;
			
			Expression(out exprNode);
			valNode.Expression = exprNode; 
		}
		Terminate(valNode); 
	}

	void Expression(out ExpressionNode exprNode) {
		exprNode = null;
		ExpressionNode leftExprNode;
		
		if (IsQueryExpression()) {
			var qNode = new QueryExpressionNode(la);    
			SetCommentOwner(qNode);
			FromClauseNode fromNode; 
			exprNode = qNode;
			
			FromClause(out fromNode);
			qNode.FromClause = fromNode; 
			QueryBodyNode bodyNode;
			
			QueryBody(out bodyNode);
		} else if (IsLambda()) {
			var lambdaNode = new LambdaExpressionNode(t);
			SetCommentOwner(lambdaNode);
			exprNode = lambdaNode;
			
			LambdaFunctionSignature(lambdaNode);
			Expect(119);
			lambdaNode.LambdaToken = t; 
			LambdaFunctionBody(lambdaNode);
			Terminate(lambdaNode); 
		} else if (StartOf(13)) {
			Unary(out leftExprNode);
			if (assgnOps[la.kind] || (la.kind == _gt && Peek(1).kind == _gteq)) {
				AssignmentOperatorNode asgnNode; 
				AssignmentOperator(out asgnNode);
				ExpressionNode rightExprNode; 
				Expression(out rightExprNode);
				asgnNode.RightOperand = rightExprNode;
				asgnNode.LeftOperand = leftExprNode;
				exprNode = asgnNode;
				
			} else if (StartOf(14)) {
				BinaryOperatorNodeBase ncNode; 
				NullCoalescingExpr(out ncNode);
				if (ncNode == null)
				{
				  exprNode = leftExprNode;
				}
				else
				{
				  ncNode.LeftmostWithMissingLeftOperand.LeftOperand = leftExprNode;
				  exprNode = ncNode;
				}
				
				if (la.kind == 110) {
					Get();
					ConditionalOperatorNode condNode = null;
					condNode = new ConditionalOperatorNode(exprNode);
					SetCommentOwner(condNode);
					exprNode = condNode;
					ExpressionNode trueNode;
					
					Expression(out trueNode);
					if (condNode != null) condNode.TrueExpression = trueNode; 
					Expect(86);
					ExpressionNode falseNode; 
					Expression(out falseNode);
					if (condNode != null) 
					{
					  condNode.FalseExpression = falseNode; 
					  Terminate(condNode);
					}
					
				}
			} else SynErr(149);
		} else SynErr(150);
		if (exprNode != null) Terminate(exprNode); 
	}

	void Type(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else if (la.kind == 80) {
			Get();
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else SynErr(151);
		if (la.kind == 110) {
			Get();
			typeNode.NullableToken = t; 
		}
		PointerOrArray(typeNode);
		Terminate(typeNode); 
	}

	void FormalParameterList(FormalParameterListNode parsNode) {
		FormalParameterNode node; 
		FormalParameterTag(out node);
		if (parsNode != null && node != null) parsNode.Items.Add(node); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			FormalParameterTag(out node);
			if (parsNode != null && node != null) 
			 parsNode.Items.Add(separator, node); 
			
		}
	}

	void Block(out BlockStatementNode blockNode) {
		StatementNode stmtNode;
		blockNode = null;
		
		Expect(96);
		blockNode = new BlockStatementNode(t); 
		SetCommentOwner(blockNode);
		
		while (StartOf(16)) {
			Statement(out stmtNode);
			if (stmtNode != null) blockNode.Statements.Add(stmtNode); 
		}
		Expect(111);
		Terminate(blockNode); 
	}

	void ConstMemberDeclaration(out MemberDeclarationNode memNode) {
		memNode = null; 
		Expect(17);
		var constNode = new ConstDeclarationNode(t);
		SetCommentOwner(constNode);
		memNode = constNode;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeNode);
		memNode.TypeName = typeNode; 
		ConstTagNode tagNode;
		
		SingleConstMember(out tagNode);
		constNode.ConstTags.Add(tagNode); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			SingleConstMember(out tagNode);
			constNode.ConstTags.Add(separator, tagNode); 
		}
		Expect(114);
		Terminate(memNode); 
	}

	void EventDeclaration(out MemberDeclarationNode memNode) {
		memNode = null; 
		TypeOrNamespaceNode typeNode; 
		Expect(26);
		var eventToken = t; 
		Type(out typeNode);
		if (IsFieldDecl()) {
			var fiNode = new FieldDeclarationNode(eventToken); 
			SetCommentOwner(fiNode);
			memNode = fiNode;
			fiNode.TypeName = typeNode;
			
			FieldMemberDeclarators(fiNode);
			Expect(114);
			Terminate(fiNode); 
		} else if (la.kind == 1) {
			var evpNode = new EventPropertyDeclarationNode(t);
			SetCommentOwner(evpNode);
			memNode = evpNode;
			
			TypeName(out typeNode);
			evpNode.TypeName = typeNode; 
			Expect(96);
			evpNode.OpenBrace = t; 
			EventAccessorDeclarations(evpNode);
			Expect(111);
			evpNode.CloseBrace = t;
			Terminate(evpNode);
			
		} else SynErr(152);
	}

	void ConstructorDeclaration(out MemberDeclarationNode memNode) {
		memNode = null; 
		Expect(1);
		var cstNode = new ConstructorDeclarationNode(t);
		SetCommentOwner(cstNode);
		memNode = cstNode;
		
		Expect(98);
		cstNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(cstNode.FormalParameters);
		}
		Expect(113);
		Terminate(cstNode.FormalParameters); 
		if (la.kind == 86) {
			Get();
			Token colonToken = t;
			ConstructorInitializerNode initializerNode = null;
			
			if (la.kind == 8) {
				Get();
				initializerNode = new BaseConstructorInitializerNode(colonToken);
				initializerNode.BaseOrThisToken = t;
				SetCommentOwner(initializerNode);
				
			} else if (la.kind == 68) {
				Get();
				initializerNode = new ThisConstructorInitializerNode(colonToken); 
				initializerNode.BaseOrThisToken = t;
				SetCommentOwner(initializerNode);
				
			} else SynErr(153);
			Expect(98);
			CurrentArgumentList(initializerNode.Arguments);
			Expect(113);
			Terminate(initializerNode);
			cstNode.Initializer = initializerNode;
			
		}
		if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			cstNode.Body = blockNode; 
		} else if (la.kind == 114) {
			Get();
			cstNode.ClosingSemicolon = t; 
		} else SynErr(154);
		Terminate(memNode); 
	}

	void MemberName(out TypeOrNamespaceNode resultNode) {
		resultNode = null;
		Token separator = null;
		Token identifier = null;
		TypeOrNamespaceNodeCollection argList = null;
		
		Expect(1);
		resultNode = new TypeOrNamespaceNode(t);
		SetCommentOwner(resultNode);
		identifier = t; 
		
		if (la.kind == 91) {
			Get();
			resultNode.QualifierToken = identifier;
			separator = t; 
			
			Expect(1);
			identifier = t; 
		}
		if (la.kind == _lt && IsPartOfMemberName()) {
			TypeArgumentList(out argList);
		}
		var tagNode = new TypeTagNode(identifier, argList);
		SetCommentOwner(tagNode);
		resultNode.TypeTags.Add(separator, tagNode); 
		
		while (la.kind == _dot && Peek(1).kind == _ident) {
			Expect(90);
			separator = t;
			argList = null;
			
			Expect(1);
			identifier = t; 
			if (la.kind == _lt && IsPartOfMemberName()) {
				TypeArgumentList(out argList);
			}
			tagNode = new TypeTagNode(identifier, argList);
			SetCommentOwner(tagNode);
			resultNode.TypeTags.Add(separator, tagNode); 
			
		}
		Terminate(resultNode); 
	}

	void MethodDeclaration(MethodDeclarationNode metNode) {
		if (la.kind == 100) {
			TypeParameterList(metNode);
		}
		Expect(98);
		metNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(metNode.FormalParameters);
		}
		Expect(113);
		Terminate(metNode.FormalParameters); 
		while (la.kind == 122) {
			TypeParameterConstraintNode constrNode; 
			TypeParameterConstraintsClause(out constrNode);
			metNode.TypeParameterConstraints.Add(constrNode); 
		}
		if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			metNode.Body = blockNode; 
		} else if (la.kind == 114) {
			Get();
			metNode.ClosingSemicolon = t; 
		} else SynErr(155);
		Terminate(metNode); 
	}

	void OperatorDeclaration(OperatorDeclarationNode opNode) {
		Expect(49);
		opNode.OperatorToken = t; 
		OverloadableOp(opNode);
		Expect(98);
		opNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(opNode.FormalParameters);
		}
		Expect(113);
		Terminate(opNode.FormalParameters); 
		if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			opNode.Body = blockNode; 
		} else if (la.kind == 114) {
			Get();
			opNode.ClosingSemicolon = t; 
		} else SynErr(156);
	}

	void FieldMemberDeclarators(FieldDeclarationNode fiNode) {
		FieldTagNode tagNode; 
		SingleFieldMember(out tagNode);
		fiNode.FieldTags.Add(tagNode); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			SingleFieldMember(out tagNode);
			fiNode.FieldTags.Add(separator, tagNode); 
		}
	}

	void PropertyDeclaration(PropertyDeclarationNodeBase propNode) {
		Expect(96);
		propNode.OpenBrace = t; 
		AccessorDeclarations(propNode);
		Expect(111);
		propNode.CloseBrace = t;
		Terminate(propNode);
		
	}

	void IndexerDeclaration(IndexerDeclarationNode indNode) {
		Expect(68);
		indNode.ThisToken = t; 
		Expect(97);
		indNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(indNode.FormalParameters);
		}
		Expect(112);
		Terminate(indNode.FormalParameters); 
		Expect(96);
		indNode.OpenBrace = t; 
		AccessorDeclarations(indNode);
		Expect(111);
		indNode.CloseBrace = t; 
	}

	void CastOperatorDeclaration(out MemberDeclarationNode memNode) {
		memNode = null; 
		if (la.kind == 37) {
			Get();
		} else if (la.kind == 27) {
			Get();
		} else SynErr(157);
		var copNode = new CastOperatorDeclarationNode(t); 
		SetCommentOwner(copNode);
		memNode = copNode;
		
		Expect(49);
		copNode.OperatorToken = t; 
		TypeOrNamespaceNode typeNode; 
		
		Type(out typeNode);
		copNode.TypeName = typeNode; 
		Expect(98);
		copNode.FormalParameters = new FormalParameterListNode(t); 
		if (StartOf(7)) {
			FormalParameterList(copNode.FormalParameters);
		}
		Expect(113);
		Terminate(copNode.FormalParameters); 
		if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			copNode.Body = blockNode; 
		} else if (la.kind == 114) {
			Get();
			copNode.ClosingSemicolon = t; 
		} else SynErr(158);
		Terminate(memNode); 
	}

	void SingleConstMember(out ConstTagNode tagNode) {
		Expect(1);
		tagNode = new ConstTagNode(t); 
		SetCommentOwner(tagNode);
		
		Expect(85);
		tagNode.EqualToken = t;
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		tagNode.Expression = exprNode;
		Terminate(tagNode);
		
	}

	void EventAccessorDeclarations(PropertyDeclarationNodeBase propNode) {
		var attrNodes = new AttributeDecorationNodeCollection(); 
		AttributeDecorations(attrNodes);
		var mod = new ModifierNodeCollection(); 
		ModifierList(mod);
		Expect(1);
		var accNode = new AccessorNode(t);
		SetCommentOwner(accNode);
		accNode.AttributeDecorations = attrNodes;
		accNode.Modifiers = mod;
		BlockStatementNode blockNode; 
		
		Block(out blockNode);
		accNode.Body = blockNode;
		Terminate(accNode);
		propNode.FirstAccessor = accNode;
		
		if (StartOf(17)) {
			attrNodes = new AttributeDecorationNodeCollection(); 
			AttributeDecorations(attrNodes);
			mod = new ModifierNodeCollection(); 
			ModifierList(mod);
			Expect(1);
			accNode = new AccessorNode(t);
			SetCommentOwner(accNode);
			accNode.AttributeDecorations = attrNodes;
			accNode.Modifiers = mod;
			
			Block(out blockNode);
			accNode.Body = blockNode;
			Terminate(accNode);
			propNode.SecondAccessor = accNode;
			
		}
	}

	void CurrentArgumentList(ArgumentNodeCollection argNodes) {
		if (StartOf(18)) {
			ArgumentNode argNode; 
			CurrentArgumentItem(out argNode);
			if (argNodes != null) argNodes.Add(argNode); 
			while (la.kind == 87) {
				Get();
				var separator = t; 
				CurrentArgumentItem(out argNode);
				if (argNodes != null) argNodes.Add(separator, argNode); 
			}
		}
	}

	void AccessorDeclarations(PropertyDeclarationNodeBase propNode) {
		var attrNodes = new AttributeDecorationNodeCollection(); 
		AttributeDecorations(attrNodes);
		var mod = new ModifierNodeCollection(); 
		ModifierList(mod);
		Expect(1);
		var accNode = new AccessorNode(t);
		SetCommentOwner(accNode);
		accNode.AttributeDecorations = attrNodes;
		accNode.Modifiers = mod;
		propNode.FirstAccessor = accNode;
		BlockStatementNode blockNode;
		
		if (la.kind == 96) {
			Block(out blockNode);
			accNode.Body = blockNode; 
		} else if (la.kind == 114) {
			Get();
			accNode.ClosingSemicolon = t; 
		} else SynErr(159);
		Terminate(accNode); 
		if (StartOf(17)) {
			attrNodes = new AttributeDecorationNodeCollection(); 
			AttributeDecorations(attrNodes);
			mod = new ModifierNodeCollection(); 
			ModifierList(mod);
			Expect(1);
			accNode = new AccessorNode(t);
			SetCommentOwner(accNode);
			accNode.AttributeDecorations = attrNodes;
			accNode.Modifiers = mod;
			propNode.SecondAccessor = accNode;
			
			if (la.kind == 96) {
				Block(out blockNode);
				accNode.Body = blockNode; 
			} else if (la.kind == 114) {
				Get();
				accNode.ClosingSemicolon = t; 
			} else SynErr(160);
			Terminate(accNode); 
		}
	}

	void OverloadableOp(OperatorDeclarationNode opNode) {
		if (la.kind == 93) {
			Get();
			opNode.KindToken = t;
			opNode.Kind = OverloadableOperatorType.GreaterThan;
			
			if (la.kind == 93) {
				if (la.pos > t.pos+1) Error("UNDEF", la, "no whitespace allowed in right shift operator"); 
				Get();
				opNode.SecondKindToken = t;
				opNode.Kind = OverloadableOperatorType.RightShift;
				
			}
		} else if (StartOf(19)) {
			switch (la.kind) {
			case 108: {
				Get();
				opNode.Kind = OverloadableOperatorType.Addition; 
				break;
			}
			case 102: {
				Get();
				opNode.Kind = OverloadableOperatorType.Subtraction; 
				break;
			}
			case 106: {
				Get();
				opNode.Kind = OverloadableOperatorType.Not; 
				break;
			}
			case 115: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseNot; 
				break;
			}
			case 95: {
				Get();
				opNode.Kind = OverloadableOperatorType.Increment; 
				break;
			}
			case 88: {
				Get();
				opNode.Kind = OverloadableOperatorType.Decrement; 
				break;
			}
			case 70: {
				Get();
				opNode.Kind = OverloadableOperatorType.True; 
				break;
			}
			case 29: {
				Get();
				opNode.Kind = OverloadableOperatorType.False; 
				break;
			}
			case 116: {
				Get();
				opNode.Kind = OverloadableOperatorType.Multiplication; 
				break;
			}
			case 132: {
				Get();
				opNode.Kind = OverloadableOperatorType.Division; 
				break;
			}
			case 133: {
				Get();
				opNode.Kind = OverloadableOperatorType.Modulo; 
				break;
			}
			case 83: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseAnd; 
				break;
			}
			case 129: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseOr; 
				break;
			}
			case 130: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseXor; 
				break;
			}
			case 101: {
				Get();
				opNode.Kind = OverloadableOperatorType.LeftShift; 
				break;
			}
			case 92: {
				Get();
				opNode.Kind = OverloadableOperatorType.Equal; 
				break;
			}
			case 105: {
				Get();
				opNode.Kind = OverloadableOperatorType.NotEqual; 
				break;
			}
			case 100: {
				Get();
				opNode.Kind = OverloadableOperatorType.LessThan; 
				break;
			}
			case 94: {
				Get();
				opNode.Kind = OverloadableOperatorType.GreaterThanOrEqual; 
				break;
			}
			case 131: {
				Get();
				opNode.Kind = OverloadableOperatorType.LessThanOrEqual; 
				break;
			}
			}
			opNode.KindToken = t; 
		} else SynErr(161);
	}

	void InterfaceMemberDeclaration(InterfaceDeclarationNode typeDecl) {
		var mod = new ModifierNodeCollection();
		var attrNodes = new AttributeDecorationNodeCollection();
		Token identifier;
		MemberDeclarationNode memNode = null;
		
		AttributeDecorations(attrNodes);
		ModifierList(mod);
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeNode);
			if (la.kind == 1) {
				Get();
				identifier = t; 
				if (la.kind == 98 || la.kind == 100) {
					var metNode = new MethodDeclarationNode(typeNode.StartToken);
					SetCommentOwner(metNode);
					metNode.TypeName = typeNode;
					metNode.MemberName = TypeOrNamespaceNode.CreateTypeNode(identifier);
					memNode = metNode;
					
					MethodDeclaration(metNode);
				} else if (la.kind == 96) {
					var propNode = new PropertyDeclarationNode(typeNode.StartToken);
					SetCommentOwner(propNode);
					propNode.TypeName = typeNode;
					propNode.MemberName = TypeOrNamespaceNode.CreateTypeNode(identifier);
					memNode = propNode;
					
					Get();
					propNode.OpenBrace = t; 
					InterfaceAccessors(propNode);
					Expect(111);
					propNode.CloseBrace = t; 
					Terminate(memNode);
					
				} else SynErr(162);
			} else if (la.kind == 68) {
				Get();
				var indNode = new IndexerDeclarationNode(typeNode.StartToken);
				SetCommentOwner(indNode);
				indNode.TypeName = typeNode;
				indNode.ThisToken = t;
				memNode = indNode;
				
				Expect(97);
				indNode.FormalParameters = new FormalParameterListNode(t); 
				if (StartOf(7)) {
					FormalParameterList(indNode.FormalParameters);
				}
				Expect(112);
				Terminate(indNode.FormalParameters); 
				Expect(96);
				indNode.OpenBrace = t; 
				InterfaceAccessors(indNode);
				Expect(111);
				indNode.CloseBrace = t; 
				Terminate(indNode);
				
			} else SynErr(163);
		} else if (la.kind == 26) {
			InterfaceEventDeclaration(out memNode);
		} else SynErr(164);
		if (memNode != null) 
		{
		  memNode.AttributeDecorations = attrNodes;
		  memNode.Modifiers = mod;
		  typeDecl.MemberDeclarations.Add(memNode);
		}
		
	}

	void InterfaceAccessors(PropertyDeclarationNodeBase propNode) {
		var attrNodes = new AttributeDecorationNodeCollection(); 
		AttributeDecorations(attrNodes);
		var mod = new ModifierNodeCollection(); 
		ModifierList(mod);
		Expect(1);
		var accNode = new AccessorNode(t);
		SetCommentOwner(accNode);
		accNode.AttributeDecorations = attrNodes;
		accNode.Modifiers = mod;
		propNode.FirstAccessor = accNode;
		
		Expect(114);
		accNode.ClosingSemicolon = t; 
		Terminate(accNode);
		
		if (StartOf(17)) {
			attrNodes = new AttributeDecorationNodeCollection(); 
			AttributeDecorations(attrNodes);
			mod = new ModifierNodeCollection(); 
			ModifierList(mod);
			Expect(1);
			accNode = new AccessorNode(t);
			SetCommentOwner(accNode);
			accNode.AttributeDecorations = attrNodes;
			accNode.Modifiers = mod;
			propNode.SecondAccessor = accNode;
			
			Expect(114);
			accNode.ClosingSemicolon = t; 
			Terminate(accNode);
			
		}
	}

	void InterfaceEventDeclaration(out MemberDeclarationNode memNode) {
		Expect(26);
		var ieNode = new InterfaceEventDeclarationNode(t);
		SetCommentOwner(ieNode);
		memNode = ieNode;
		
		TypeOrNamespaceNode typeNode; 
		Type(out typeNode);
		ieNode.TypeName = typeNode; 
		Expect(1);
		ieNode.IdentifierToken = t; 
		Expect(114);
		Terminate(memNode); 
	}

	void LocalVariableDeclaration(out LocalVariableNode varNode) {
		TypeOrNamespaceNode typeNode = null;
		varNode = null;
		
		if (IsVar()) {
			Expect(1);
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else if (StartOf(11)) {
			Type(out typeNode);
		} else SynErr(165);
		varNode = new LocalVariableNode(typeNode); 
		SetCommentOwner(varNode);
		LocalVariableTagNode varTagNode;
		
		LocalVariableDeclarator(out varTagNode);
		if (varNode != null) varNode.VariableTags.Add(varTagNode); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			LocalVariableDeclarator(out varTagNode);
			if (varNode != null) varNode.VariableTags.Add(separator, varTagNode); 
			
		}
		Terminate(varNode); 
	}

	void LocalVariableDeclarator(out LocalVariableTagNode varDeclNode) {
		Expect(1);
		varDeclNode = new LocalVariableTagNode(t); 
		SetCommentOwner(varDeclNode);
		
		if (la.kind == 85) {
			Get();
			var start = t; 
			if (StartOf(20)) {
				VariableInitializerNode varInitNode; 
				VariableInitializer(out varInitNode);
				varDeclNode.Initializer = varInitNode; 
			} else if (la.kind == 63) {
				Get();
				var stcInitNode = new StackAllocInitializerNode(start);
				SetCommentOwner(stcInitNode);
				varDeclNode.Initializer = stcInitNode;
				stcInitNode.StackAllocToken = t;
				
				TypeOrNamespaceNode typeNode; 
				Type(out typeNode);
				stcInitNode.TypeName = typeNode; 
				Expect(97);
				stcInitNode.OpenSquareToken = t;
				ExpressionNode exprNode; 
				
				Expression(out exprNode);
				stcInitNode.Expression = exprNode; 
				Expect(112);
				stcInitNode.CloseSquareToken = t;
				Terminate(stcInitNode);
				
			} else SynErr(166);
		}
		Terminate(varDeclNode); 
	}

	void VariableInitializer(out VariableInitializerNode initNode) {
		initNode = null; 
		ExpressionNode exprNode; 
		
		if (StartOf(13)) {
			Expression(out exprNode);
			var exprInitNode = new ExpressionInitializerNode(exprNode);
			SetCommentOwner(exprInitNode);
			initNode = exprInitNode;
			
		} else if (la.kind == 96) {
			ArrayInitializerNode arrInitNode; 
			ArrayInitializer(out arrInitNode);
			initNode = arrInitNode; 
		} else SynErr(167);
	}

	void ArrayInitializer(out ArrayInitializerNode initNode) {
		initNode = null; 
		Expect(96);
		initNode = new ArrayInitializerNode(t); 
		SetCommentOwner(initNode);
		
		if (StartOf(20)) {
			VariableInitializerNode varInitNode; 
			VariableInitializer(out varInitNode);
			var initItem = new ArrayItemInitializerNode(varInitNode);
			SetCommentOwner(initItem);
			initNode.Items.Add(initItem);
			
			while (NotFinalComma()) {
				Expect(87);
				initItem.Separator = t; 
				VariableInitializer(out varInitNode);
				initItem = new ArrayItemInitializerNode(varInitNode);
				SetCommentOwner(initItem);
				initNode.Items.Add(initItem);
				
			}
			if (la.kind == 87) {
				Get();
				initItem.Separator = t; 
			}
		}
		Expect(111);
		Terminate(initNode); 
	}

	void FormalParameterTag(out FormalParameterNode parNode) {
		var attrNodes = new AttributeDecorationNodeCollection();
		parNode = null;
		var modifier = FormalParameterModifier.In;
		Token start = null;
		
		AttributeDecorations(attrNodes);
		if (StartOf(21)) {
			if (la.kind == 57) {
				Get();
				modifier = FormalParameterModifier.Ref; 
			} else if (la.kind == 50) {
				Get();
				modifier = FormalParameterModifier.Out; 
			} else if (la.kind == 68) {
				Get();
				modifier = FormalParameterModifier.This; 
			} else {
				Get();
				modifier = FormalParameterModifier.Params; 
			}
			start = t; 
		}
		TypeOrNamespaceNode typeNode; 
		Type(out typeNode);
		if (start == null) start = typeNode.StartToken; 
		Expect(1);
		parNode = new FormalParameterNode(start);
		SetCommentOwner(parNode);
		parNode.Modifier = modifier;
		parNode.IdentifierToken = t;
		parNode.TypeName = typeNode;
		Terminate(parNode);
		
	}

	void CurrentArgumentItem(out ArgumentNode argNode) {
		ExpressionNode exprNode; 
		Token argKind = null;
		
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
			} else {
				Get();
			}
			argKind = t; 
		}
		Expression(out exprNode);
		argNode = new ArgumentNode(argKind == null ? 
		 (exprNode == null ? t : exprNode.StartToken) : argKind);
		SetCommentOwner(argNode);
		argNode.KindToken = argKind;
		argNode.Expression = exprNode;
		Terminate(argNode);
		
	}

	void Attributes(out AttributeDecorationNode attrNode) {
		AttributeNode attributeNode;
		Token separator = null;
		
		Expect(97);
		attrNode = new AttributeDecorationNode(t); 
		if (IsAttrTargSpec()) {
			if (la.kind == 1) {
				Get();
			} else if (StartOf(22)) {
				Keyword();
			} else SynErr(168);
			attrNode.TargetToken = t; 
			Expect(86);
			separator = t; 
		}
		Attribute(out attributeNode);
		attrNode.Attributes.Add(separator, attributeNode); 
		while (la.kind == _comma && Peek(1).kind != _rbrack) {
			Expect(87);
			separator = t; 
			Attribute(out attributeNode);
			attrNode.Attributes.Add(separator, attributeNode); 
		}
		if (la.kind == 87) {
			Get();
			attrNode.OrphanSeparator = t; 
		}
		Expect(112);
		Terminate(attrNode); 
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

	void AttributeArguments(AttributeNode argsNode) {
		Token identifier = null;
		Token equal = null;
		ExpressionNode exprNode;
		
		Expect(98);
		Start(argsNode.Arguments); 
		if (StartOf(13)) {
			if (IsAssignment()) {
				Expect(1);
				identifier = t; 
				Expect(85);
				equal = t; 
			}
			Expression(out exprNode);
			var newArg = new AttributeArgumentNode(identifier, equal, exprNode);
			SetCommentOwner(newArg);
			argsNode.Arguments.Add(newArg); 
			
			while (la.kind == 87) {
				Get();
				var separator = t; 
				if (IsAssignment()) {
					Expect(1);
					identifier = t; 
					Expect(85);
					equal = t; 
				} else if (StartOf(13)) {
				} else SynErr(170);
				Expression(out exprNode);
				newArg = new AttributeArgumentNode(identifier, equal, exprNode);
				SetCommentOwner(newArg);
				argsNode.Arguments.Add(separator, newArg); 
				
			}
		}
		Expect(113);
		Terminate(argsNode.Arguments); 
	}

	void PrimitiveType(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (StartOf(6)) {
			IntegralType(out typeNode);
		} else if (StartOf(23)) {
			if (la.kind == 32) {
				Get();
			} else if (la.kind == 23) {
				Get();
			} else if (la.kind == 19) {
				Get();
			} else {
				Get();
			}
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else SynErr(171);
	}

	void PointerOrArray(TypeOrNamespaceNode typeNode) {
		while (IsPointerOrDims()) {
			if (la.kind == 116) {
				Get();
				var ptNode = new PointerModifierNode(t);
				typeNode.TypeModifiers.Add(ptNode); 
				SetCommentOwner(ptNode);
				
			} else if (la.kind == 97) {
				Get();
				var arrNode = new ArrayModifierNode(t); 
				SetCommentOwner(arrNode);
				
				while (la.kind == 87) {
					Get();
					arrNode.AddSeparator(t); 
				}
				Expect(112);
				Terminate(arrNode); 
			} else SynErr(172);
		}
	}

	void NonArrayType(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else SynErr(173);
		if (la.kind == 110) {
			Get();
			typeNode.NullableToken = t; 
		}
		if (la.kind == 116) {
			Get();
			var ptNode = new PointerModifierNode(t);
			typeNode.TypeModifiers.Add(ptNode); 
			SetCommentOwner(ptNode);
			
		}
		Terminate(typeNode); 
	}

	void TypeInRelExpr(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else if (la.kind == 80) {
			Get();
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else SynErr(174);
		if (IsNullableTypeMark()) {
			Expect(110);
			typeNode.NullableToken = t; 
		}
		PointerOrArray(typeNode);
		Terminate(typeNode); 
	}

	void PredefinedType(out TypeOrNamespaceNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
			} else {
				Get();
			}
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else SynErr(175);
	}

	void TypeArgumentList(out TypeOrNamespaceNodeCollection argList) {
		argList = null; 
		Expect(100);
		argList = new TypeOrNamespaceNodeCollection(); 
		SetCommentOwner(argList);
		Start(argList);
		
		if (StartOf(11)) {
			TypeOrNamespaceNode typeNode; 
			Type(out typeNode);
			argList.Add(typeNode); 
		}
		while (la.kind == 87) {
			Get();
			var separator = t; 
			if (StartOf(11)) {
				TypeOrNamespaceNode typeNode; 
				Type(out typeNode);
				argList.Add(separator, typeNode); 
			}
		}
		Expect(93);
		Terminate(argList); 
	}

	void Statement(out StatementNode stmtNode) {
		stmtNode = null; 
		if (la.kind == _ident && Peek(1).kind == _colon) {
			Token identifier; 
			Expect(1);
			identifier = t; 
			Expect(86);
			var label = new LabelNode(identifier, t); 
			SetCommentOwner(label);
			
			Statement(out stmtNode);
			if (stmtNode != null) stmtNode.Labels.AddLabel(label); 
		} else if (la.kind == 17) {
			ConstStatement(out stmtNode);
		} else if (IsLocalVarDecl()) {
			LocalVariableNode varNode; 
			LocalVariableDeclaration(out varNode);
			var varDecl = new VariableDeclarationStatementNode(varNode.StartToken);
			SetCommentOwner(varDecl);
			stmtNode = varDecl;
			varDecl.Declaration = varNode;
			
			Expect(114);
			Terminate(varDecl); 
		} else if (StartOf(24)) {
			EmbeddedStatement(out stmtNode);
		} else SynErr(176);
	}

	void ConstStatement(out StatementNode stmtNode) {
		ExpressionNode exprNode; 
		Expect(17);
		var csNode = new ConstStatementNode(t);
		SetCommentOwner(csNode);
		stmtNode = csNode;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeNode);
		csNode.TypeName = typeNode; 
		Expect(1);
		var cmTag = new ConstTagNode(t); 
		SetCommentOwner(cmTag);
		
		Expect(85);
		cmTag.EqualToken = t; 
		Expression(out exprNode);
		cmTag.Expression = exprNode;
		Terminate(cmTag);
		csNode.ConstTags.Add(cmTag);
		
		while (la.kind == 87) {
			Get();
			var separator = t; 
			Expect(1);
			var cmcTag = new ConstTagNode(t); 
			SetCommentOwner(cmcTag);
			
			Expect(85);
			cmcTag.EqualToken = t; 
			Expression(out exprNode);
			cmcTag.Expression = exprNode;
			Terminate(cmcTag);
			csNode.ConstTags.Add(separator, cmTag);
			
		}
		Expect(114);
		Terminate(csNode); 
	}

	void EmbeddedStatement(out StatementNode stmtNode) {
		stmtNode = null; 
		if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			stmtNode = blockNode; 
		} else if (la.kind == 114) {
			EmptyStatement(out stmtNode);
		} else if (la.kind == 15) {
			CheckedBlock(out stmtNode);
		} else if (la.kind == 75) {
			UncheckedBlock(out stmtNode);
		} else if (la.kind == 76) {
			UnsafeBlock(out stmtNode);
		} else if (StartOf(13)) {
			ExpressionNode exprNode; 
			var eNode = new ExpressionStatementNode(la); 
			SetCommentOwner(eNode);
			
			StatementExpression(out exprNode);
			eNode.Expression = exprNode; 
			Expect(114);
			stmtNode = eNode;
			Terminate(eNode); 
			
		} else if (la.kind == 36) {
			IfStatement(out stmtNode);
		} else if (la.kind == 67) {
			SwitchStatement(out stmtNode);
		} else if (la.kind == 82) {
			WhileStatement(out stmtNode);
		} else if (la.kind == 22) {
			DoWhileStatement(out stmtNode);
		} else if (la.kind == 33) {
			ForStatement(out stmtNode);
		} else if (la.kind == 34) {
			ForEachStatement(out stmtNode);
		} else if (la.kind == 10) {
			BreakStatement(out stmtNode);
		} else if (la.kind == 18) {
			ContinueStatement(out stmtNode);
		} else if (la.kind == 35) {
			GotoStatement(out stmtNode);
		} else if (la.kind == 58) {
			ReturnStatement(out stmtNode);
		} else if (la.kind == 69) {
			ThrowStatement(out stmtNode);
		} else if (la.kind == 71) {
			TryFinallyBlock(out stmtNode);
		} else if (la.kind == 43) {
			LockStatement(out stmtNode);
		} else if (la.kind == 78) {
			UsingStatement(out stmtNode);
		} else if (la.kind == 121) {
			Get();
			if (la.kind == 58) {
				YieldReturnStatement(out stmtNode);
			} else if (la.kind == 10) {
				YieldBreakStatement(out stmtNode);
			} else SynErr(177);
		} else if (la.kind == 31) {
			FixedStatement(out stmtNode);
		} else SynErr(178);
	}

	void EmptyStatement(out StatementNode stmtNode) {
		Expect(114);
		stmtNode = new EmptyStatementNode(t); 
		SetCommentOwner(stmtNode);
		
	}

	void CheckedBlock(out StatementNode stmtNode) {
		Expect(15);
		var start = t; 
		BlockStatementNode blockNode;
		
		Block(out blockNode);
		stmtNode = new CheckedStatementNode(t, blockNode); 
		SetCommentOwner(stmtNode);
		
	}

	void UncheckedBlock(out StatementNode stmtNode) {
		Expect(75);
		var start = t; 
		BlockStatementNode blockNode;
		
		Block(out blockNode);
		stmtNode = new UncheckedStatementNode(t, blockNode); 
		SetCommentOwner(stmtNode);
		
	}

	void UnsafeBlock(out StatementNode stmtNode) {
		Expect(76);
		var start = t; 
		BlockStatementNode blockNode;
		
		Block(out blockNode);
		stmtNode = new UnsafeStatementNode(t, blockNode); 
		SetCommentOwner(stmtNode);
		
	}

	void StatementExpression(out ExpressionNode exprNode) {
		bool isAssignment = assnStartOp[la.kind] || IsTypeCast(); 
		exprNode = null;
		
		ExpressionNode unaryNode; 
		Unary(out unaryNode);
		exprNode = unaryNode; 
		if (StartOf(25)) {
			AssignmentOperatorNode asgnNode; 
			AssignmentOperator(out asgnNode);
			asgnNode.LeftOperand = unaryNode;
			exprNode = asgnNode;
			ExpressionNode rightNode; 
			
			Expression(out rightNode);
			asgnNode.RightOperand = rightNode; 
		} else if (la.kind == 87 || la.kind == 113 || la.kind == 114) {
			if (isAssignment) Error("UNDEF", la, "error in assignment."); 
		} else SynErr(179);
	}

	void IfStatement(out StatementNode stmtNode) {
		Expect(36);
		var ifNode = new IfStatementNode(t);
		SetCommentOwner(ifNode);
		stmtNode = ifNode;
		
		Expect(98);
		ifNode.OpenParenthesis = t;
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		ifNode.Condition = exprNode; 
		Expect(113);
		ifNode.CloseParenthesis = t;
		StatementNode thenBranchNode; 
		StatementNode elseBranchNode; 
		
		EmbeddedStatement(out thenBranchNode);
		ifNode.ThenStatement = thenBranchNode; 
		if (la.kind == 24) {
			Get();
			ifNode.ElseToken = t; 
			EmbeddedStatement(out elseBranchNode);
			ifNode.ElseStatement = elseBranchNode; 
		}
		Terminate(stmtNode); 
	}

	void SwitchStatement(out StatementNode stmtNode) {
		Expect(67);
		var swcNode = new SwitchStatementNode(t);
		SetCommentOwner(swcNode);
		stmtNode = swcNode; 
		
		Expect(98);
		swcNode.OpenParenthesis = t; 
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		swcNode.Expression = exprNode; 
		Expect(113);
		swcNode.CloseParenthesis = t; 
		Expect(96);
		swcNode.SwitchSections.StartToken = t; 
		while (la.kind == 12 || la.kind == 20) {
			SwitchSection(swcNode);
		}
		Expect(111);
		Terminate(swcNode.SwitchSections); 
		Terminate(stmtNode);
		
	}

	void WhileStatement(out StatementNode stmtNode) {
		Expect(82);
		var whNode = new WhileStatementNode(t); 
		SetCommentOwner(whNode);
		stmtNode = whNode;
		
		Expect(98);
		whNode.OpenParenthesis = t;
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		Expect(113);
		whNode.CloseParenthesis = t;
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		whNode.Statement = bodyNode;
		Terminate(whNode);
		
	}

	void DoWhileStatement(out StatementNode stmtNode) {
		Expect(22);
		var dwhNode = new DoWhileStatementNode(t); 
		SetCommentOwner(dwhNode);
		stmtNode = dwhNode;
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		dwhNode.Statement = bodyNode; 
		Expect(82);
		dwhNode.WhileToken = t; 
		Expect(98);
		ExpressionNode exprNode;
		dwhNode.OpenParenthesis = t; 
		
		Expression(out exprNode);
		dwhNode.Condition = exprNode; 
		Expect(113);
		dwhNode.CloseParenthesis = t; 
		Expect(114);
		Terminate(stmtNode); 
	}

	void ForStatement(out StatementNode stmtNode) {
		Expect(33);
		var forNode = new ForStatementNode(t);
		SetCommentOwner(forNode);
		stmtNode = forNode;
		
		Expect(98);
		forNode.OpenParenthesis = t; 
		if (StartOf(26)) {
			ForInitializer(forNode);
		}
		Expect(114);
		forNode.InitSeparator = t; 
		if (StartOf(13)) {
			ExpressionNode exprNode; 
			Expression(out exprNode);
		}
		Expect(114);
		forNode.ConditionSeparator = t; 
		if (StartOf(13)) {
			ForIterator(forNode);
		}
		Expect(113);
		forNode.CloseParenthesis = t; 
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		forNode.Statement = bodyNode;
		Terminate(forNode); 
		
	}

	void ForEachStatement(out StatementNode stmtNode) {
		Expect(34);
		var feNode = new ForeachStatementNode(t); 
		SetCommentOwner(feNode);
		stmtNode = feNode;
		
		Expect(98);
		feNode.OpenParenthesis = t; 
		TypeOrNamespaceNode typeNode = null; 
		
		if (IsVar()) {
			Expect(1);
			typeNode = TypeOrNamespaceNode.CreateTypeNode(t); 
		} else if (StartOf(11)) {
			Type(out typeNode);
		} else SynErr(180);
		feNode.TypeName = typeNode; 
		Expect(1);
		feNode.IdentifierToken = t; 
		Expect(38);
		feNode.InToken = t; 
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		feNode.Collection = exprNode; 
		Expect(113);
		feNode.CloseParenthesis = t; 
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		feNode.Statement = bodyNode;
		Terminate(stmtNode); 
		
	}

	void BreakStatement(out StatementNode stmtNode) {
		Expect(10);
		stmtNode = new BreakStatementNode(t); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void ContinueStatement(out StatementNode stmtNode) {
		Expect(18);
		stmtNode = new ContinueStatementNode(t); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void GotoStatement(out StatementNode stmtNode) {
		Expect(35);
		var gotoNode = new GotoStatementNode(t); 
		SetCommentOwner(gotoNode);
		
		if (la.kind == 1) {
			Get();
			gotoNode.IdentifierToken = t; 
		} else if (la.kind == 12) {
			Get();
			gotoNode.IdentifierToken = t;
			ExpressionNode exprNode; 
			
			Expression(out exprNode);
			gotoNode.Expression = exprNode; 
		} else if (la.kind == 20) {
			Get();
			gotoNode.IdentifierToken = t; 
		} else SynErr(181);
		stmtNode = gotoNode; 
		Expect(114);
		Terminate(stmtNode); 
	}

	void ReturnStatement(out StatementNode stmtNode) {
		ExpressionNode exprNode = null; 
		Expect(58);
		var start = t; 
		if (StartOf(13)) {
			Expression(out exprNode);
		}
		stmtNode = new ReturnStatementNode(t, exprNode); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void ThrowStatement(out StatementNode stmtNode) {
		ExpressionNode exprNode = null; 
		Expect(69);
		var start = t; 
		if (StartOf(13)) {
			Expression(out exprNode);
		}
		stmtNode = new ThrowStatementNode(t, exprNode); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void TryFinallyBlock(out StatementNode stmtNode) {
		Expect(71);
		var tryNode = new TryStatementNode(t);
		SetCommentOwner(tryNode);
		stmtNode = tryNode;
		BlockStatementNode blockNode; 
		
		Block(out blockNode);
		tryNode.TryBlock = blockNode; 
		if (la.kind == 13) {
			CatchClauses(tryNode);
			if (la.kind == 30) {
				Get();
				tryNode.FinallyToken = t; 
				Block(out blockNode);
				tryNode.FinallyBlock = blockNode; 
			}
		} else if (la.kind == 30) {
			Get();
			tryNode.FinallyToken = t; 
			Block(out blockNode);
			tryNode.FinallyBlock = blockNode; 
		} else SynErr(182);
	}

	void LockStatement(out StatementNode stmtNode) {
		Expect(43);
		var lckNode = new LockStatementNode(t); 
		SetCommentOwner(lckNode);
		stmtNode = lckNode;
		
		Expect(98);
		lckNode.OpenParenthesis = t;
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		lckNode.Expression = exprNode; 
		Expect(113);
		lckNode.CloseParenthesis = t;
		StatementNode bodyNode;;
		
		EmbeddedStatement(out bodyNode);
		lckNode.Statement = bodyNode;
		Terminate(stmtNode);
		
	}

	void UsingStatement(out StatementNode stmtNode) {
		Expect(78);
		var usNode = new UsingStatementNode(t); 
		SetCommentOwner(usNode);
		stmtNode = usNode;
		
		Expect(98);
		usNode.OpenParenthesis = t; 
		if (IsLocalVarDecl()) {
			LocalVariableNode varNode; 
			LocalVariableDeclaration(out varNode);
			usNode.Initializer = varNode; 
		} else if (StartOf(13)) {
			ExpressionNode exprNode; 
			Expression(out exprNode);
			usNode.Expression = exprNode; 
		} else SynErr(183);
		Expect(113);
		usNode.CloseParenthesis = t;   
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		usNode.Statement = bodyNode;
		Terminate(stmtNode); 
		
	}

	void YieldReturnStatement(out StatementNode stmtNode) {
		ExpressionNode exprNode; 
		Expect(58);
		var start = t; 
		Expression(out exprNode);
		stmtNode = new YieldReturnStatementNode(t, exprNode); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void YieldBreakStatement(out StatementNode stmtNode) {
		Expect(10);
		stmtNode = new YieldBreakStatementNode(t); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void FixedStatement(out StatementNode stmtNode) {
		Expect(31);
		var fixNode = new FixedStatementNode(t);
		SetCommentOwner(fixNode);
		stmtNode = fixNode;
		
		Expect(98);
		fixNode.OpenParenthesis = t; 
		TypeOrNamespaceNode typeNode; 
		
		Type(out typeNode);
		fixNode.TypeName = typeNode; 
		Expect(1);
		var fiNode = new FixedInitializerNode(t); 
		SetCommentOwner(fiNode);
		
		Expect(85);
		fiNode.EqualToken = t; 
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		fiNode.Expression = exprNode; 
		Terminate(fiNode);
		fixNode.Initializers.Add(fiNode);
		
		while (la.kind == 87) {
			Get();
			var separator = t; 
			Expect(1);
			fiNode = new FixedInitializerNode(t); 
			SetCommentOwner(fiNode);
			
			Expect(85);
			fiNode.EqualToken = t; 
			Expression(out exprNode);
			fiNode.Expression = exprNode; 
			Terminate(fiNode);
			fixNode.Initializers.Add(separator, fiNode);
			
		}
		Expect(113);
		fixNode.CloseParenthesis = t;
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		fixNode.Statement = bodyNode;
		Terminate(fixNode); 
		
	}

	void SwitchSection(SwitchStatementNode swcNode) {
		var ssNode = new SwitchSectionNode(la); 
		SetCommentOwner(ssNode);
		SwitchLabelNode slNode;
		
		SwitchLabel(out slNode);
		ssNode.Labels.Add(slNode); 
		while (la.kind == _case || (la.kind == _default && Peek(1).kind == _colon)) {
			SwitchLabel(out slNode);
			ssNode.Labels.Add(slNode); 
		}
		StatementNode stmtNode; 
		Statement(out stmtNode);
		ssNode.Statements.Add(stmtNode); 
		while (IsNoSwitchLabelOrRBrace()) {
			Statement(out stmtNode);
			ssNode.Statements.Add(stmtNode); 
		}
		swcNode.SwitchSections.Add(ssNode); 
	}

	void ForInitializer(ForStatementNode forNode) {
		if (IsLocalVarDecl()) {
			LocalVariableNode varNode; 
			LocalVariableDeclaration(out varNode);
			forNode.Initializer = varNode; 
		} else if (StartOf(13)) {
			ExpressionNode exprNode; 
			StatementExpression(out exprNode);
			if (exprNode != null) forNode.Initializers.Add(exprNode); 
			while (la.kind == 87) {
				Get();
				var separator = t; 
				StatementExpression(out exprNode);
				if (exprNode != null) forNode.Initializers.Add(separator, exprNode); 
			}
		} else SynErr(184);
	}

	void ForIterator(ForStatementNode forNode) {
		ExpressionNode exprNode; 
		StatementExpression(out exprNode);
		if (exprNode != null) forNode.Iterators.Add(exprNode); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			StatementExpression(out exprNode);
			if (exprNode != null) forNode.Iterators.Add(separator, exprNode); 
		}
	}

	void CatchClauses(TryStatementNode tryNode) {
		Expect(13);
		var ccNode = new CatchClauseNode(t);
		SetCommentOwner(ccNode);
		BlockStatementNode blockNode;
		
		if (la.kind == 96) {
			Block(out blockNode);
			ccNode.Block = blockNode; 
			Terminate(ccNode);
			tryNode.CatchClauses.Add(ccNode);
			
		} else if (la.kind == 98) {
			Get();
			ccNode.OpenParenthesis = t; 
			TypeOrNamespaceNode typeNode;
			
			ClassType(out typeNode);
			ccNode.TypeName = typeNode; 
			if (la.kind == 1) {
				Get();
				ccNode.IdentifierToken = t; 
			}
			Expect(113);
			ccNode.CloseParenthesis = t; 
			Block(out blockNode);
			ccNode.Block = blockNode; 
			Terminate(ccNode);
			tryNode.CatchClauses.Add(ccNode);
			
			if (la.kind == 13) {
				CatchClauses(tryNode);
			}
		} else SynErr(185);
	}

	void Unary(out ExpressionNode exprNode) {
		exprNode = null;  
		UnaryOperatorNode unaryOp = null;
		
		if (unaryHead[la.kind] || IsTypeCast()) {
			switch (la.kind) {
			case 108: {
				Get();
				unaryOp = new UnaryPlusOperatorNode(t); 
				break;
			}
			case 102: {
				Get();
				unaryOp = new UnaryMinusOperatorNode(t);
				break;
			}
			case 106: {
				Get();
				unaryOp = new UnaryNotOperatorNode(t); 
				break;
			}
			case 115: {
				Get();
				unaryOp = new BitwiseNotOperatorNode(t); 
				break;
			}
			case 95: {
				Get();
				unaryOp = new PreIncrementOperatorNode(t); 
				break;
			}
			case 88: {
				Get();
				unaryOp = new PreDecrementOperatorNode(t); 
				break;
			}
			case 116: {
				Get();
				unaryOp = new PointerOperatorNode(t); 
				break;
			}
			case 83: {
				Get();
				unaryOp = new ReferenceOperatorNode(t); 
				break;
			}
			case 98: {
				Get();
				TypeOrNamespaceNode typeNode; 
				var tcNode = new TypecastOperatorNode(t);
				unaryOp = tcNode;
				
				Type(out typeNode);
				tcNode.TypeName = typeNode; 
				Expect(113);
				Terminate(tcNode); 
				break;
			}
			default: SynErr(186); break;
			}
			SetCommentOwner(unaryOp); 
			ExpressionNode unaryNode; 
			Unary(out unaryNode);
			if (unaryOp == null) exprNode = unaryNode;
			else
			{
			  unaryOp.Operand = unaryNode;
			  exprNode = unaryOp;
			}
			Terminate(unaryOp);
			
		} else if (StartOf(27)) {
			Primary(out exprNode);
		} else SynErr(187);
	}

	void AssignmentOperator(out AssignmentOperatorNode opNode) {
		AssignmentOperatorType op = AssignmentOperatorType.SimpleAssignment;
		Token start;
		Token second = null;
		
		start = la; 
		switch (la.kind) {
		case 85: {
			Get();
			op = AssignmentOperatorType.SimpleAssignment; 
			break;
		}
		case 109: {
			Get();
			op = AssignmentOperatorType.AdditionAssignment; 
			break;
		}
		case 103: {
			Get();
			op = AssignmentOperatorType.SubtractionAssignment; 
			break;
		}
		case 117: {
			Get();
			op = AssignmentOperatorType.MultiplicationAssignment; 
			break;
		}
		case 89: {
			Get();
			op = AssignmentOperatorType.DivisionAssignment; 
			break;
		}
		case 104: {
			Get();
			op = AssignmentOperatorType.ModuloAssignment; 
			break;
		}
		case 84: {
			Get();
			op = AssignmentOperatorType.LogicalAndAssignment; 
			break;
		}
		case 107: {
			Get();
			op = AssignmentOperatorType.LogicalOrAssignment; 
			break;
		}
		case 118: {
			Get();
			op = AssignmentOperatorType.LogicalXorAssignment; 
			break;
		}
		case 99: {
			Get();
			op = AssignmentOperatorType.LeftShiftAssignment; 
			break;
		}
		case 93: {
			Get();
			int pos = t.pos; 
			Expect(94);
			if (pos+1 < t.pos) Error("UNDEF", la, "no whitespace allowed in right shift assignment");
			op = AssignmentOperatorType.RightShiftAssignment;
			second = t;
			
			break;
		}
		default: SynErr(188); break;
		}
		opNode = new AssignmentOperatorNode(start, second, op);
		SetCommentOwner(opNode); 
		
	}

	void SwitchLabel(out SwitchLabelNode slNode) {
		slNode = new SwitchLabelNode(la); 
		SetCommentOwner(slNode);
		
		if (la.kind == 12) {
			Get();
			ExpressionNode exprNode; 
			Expression(out exprNode);
			slNode.Expression = exprNode; 
		} else if (la.kind == 20) {
			Get();
		} else SynErr(189);
		Expect(86);
		Terminate(slNode); 
	}

	void LambdaFunctionSignature(LambdaExpressionNode lambdaNode) {
		if (la.kind == _ident) {
			Expect(1);
			var fpNode = new FormalParameterNode(t);
			SetCommentOwner(fpNode);
			fpNode.IdentifierToken = t;
			Terminate(fpNode);
			lambdaNode.FormalParameters.Add(fpNode);
			
		} else if (la.kind == 98) {
			Get();
			lambdaNode.OpenParenthesis = t;
			FormalParameterNode parNode; 
			
			if (IsExplicitLambdaParameter(la)) {
				ExplicitLambdaParameter(out parNode);
				lambdaNode.FormalParameters.Add(parNode); 
				while (la.kind == 87) {
					Get();
					var separator = t; 
					ExplicitLambdaParameter(out parNode);
					lambdaNode.FormalParameters.Add(separator, parNode); 
				}
			} else if (la.kind != _rpar) {
				Expect(1);
				parNode = new FormalParameterNode(t);
				SetCommentOwner(parNode);
				parNode.IdentifierToken = t;
				Terminate(parNode);
				lambdaNode.FormalParameters.Add(parNode);
				
				while (la.kind == 87) {
					Get();
					var separator = t; 
					Expect(1);
					parNode = new FormalParameterNode(t);
					SetCommentOwner(parNode);
					parNode.IdentifierToken = t;
					Terminate(parNode);
					lambdaNode.FormalParameters.Add(separator, parNode);
					
				}
			} else if (la.kind == 113) {
			} else SynErr(190);
			Expect(113);
			lambdaNode.OpenParenthesis = t; 
		} else SynErr(191);
	}

	void ExplicitLambdaParameter(out FormalParameterNode fpNode) {
		fpNode = new FormalParameterNode(la); 
		SetCommentOwner(fpNode);
		
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				fpNode.Modifier = FormalParameterModifier.Ref; 
			} else {
				Get();
				fpNode.Modifier = FormalParameterModifier.Out; 
			}
		}
		TypeOrNamespaceNode typeNode; 
		Type(out typeNode);
		fpNode.TypeName = typeNode; 
		Expect(1);
		fpNode.IdentifierToken = t; 
		Terminate(fpNode);
		
	}

	void LambdaFunctionBody(LambdaExpressionNode lambdaNode) {
		if (StartOf(13)) {
			ExpressionNode exprNode; 
			Expression(out exprNode);
			lambdaNode.Expression = exprNode; 
		} else if (la.kind == 96) {
			BlockStatementNode blockNode; 
			Block(out blockNode);
			lambdaNode.Block = blockNode; 
		} else SynErr(192);
	}

	void FromClause(out FromClauseNode fromNode) {
		Expect(1);
		fromNode = new FromClauseNode(t); 
		SetCommentOwner(fromNode);
		var typeToken = la;
		
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeOrNamespaceNode typeNode; 
			Type(out typeNode);
			fromNode.TypeName = typeNode; 
		} else if (la.kind == 1) {
		} else SynErr(193);
		Expect(1);
		fromNode.IdentifierToken = t; 
		Expect(38);
		fromNode.InToken = t; 
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		fromNode.Expression = exprNode;
		Terminate(fromNode);
		
	}

	void QueryBody(out QueryBodyNode bodyNode) {
		bodyNode = new QueryBodyNode(la); 
		SetCommentOwner(bodyNode);
		
		while (la.kind == 1 || la.kind == 122) {
			QueryBodyClause(bodyNode);
		}
	}

	void QueryBodyClause(QueryBodyNode bodyNode) {
		if (la.kind == _ident && la.val == "from") {
			FromClauseNode fromNode; 
			FromClause(out fromNode);
			bodyNode.BodyClauses.Add(fromNode); 
		} else if (la.kind == _ident && la.val == "let") {
			LetClauseNode letNode; 
			LetClause(out letNode);
			bodyNode.BodyClauses.Add(letNode); 
		} else if (la.kind == _ident && la.val == "join") {
			JoinClauseNode joinNode; 
			JoinClause(out joinNode);
			bodyNode.BodyClauses.Add(joinNode); 
		} else if (la.kind == _ident && la.val == "orderby") {
			OrderByClauseNode orderNode; 
			OrderByClause(out orderNode);
			bodyNode.BodyClauses.Add(orderNode); 
		} else if (la.kind == _ident && la.val == "into") {
			IntoClauseNode intoNode; 
			IntoClause(out intoNode);
			bodyNode.BodyClauses.Add(intoNode); 
		} else if (la.kind == _ident && la.val == "select") {
			SelectClauseNode selectNode; 
			SelectClause(out selectNode);
			bodyNode.BodyClauses.Add(selectNode); 
		} else if (la.kind == _ident && la.val == "group") {
			GroupByClauseNode groupNode; 
			GroupClause(out groupNode);
			bodyNode.BodyClauses.Add(groupNode); 
		} else if (la.kind == 122) {
			WhereClauseNode whereNode; 
			WhereClause(out whereNode);
			bodyNode.BodyClauses.Add(whereNode); 
		} else if (la.kind == 1) {
			Get();
			Error("SYNERR", t, "invalid identifier in query expression", null); 
		} else SynErr(194);
	}

	void LetClause(out LetClauseNode letNode) {
		ExpressionNode exprNode; 
		Expect(1);
		letNode = new LetClauseNode(t); 
		SetCommentOwner(letNode);
		
		Expect(1);
		letNode.IdentifierToken = t; 
		Expect(85);
		letNode.EqualToken = t; 
		Expression(out exprNode);
		letNode.Expression = exprNode; 
		Terminate(letNode);
		
	}

	void JoinClause(out JoinClauseNode joinNode) {
		ExpressionNode exprNode; 
		Expect(1);
		joinNode = new JoinClauseNode(t);
		SetCommentOwner(joinNode);
		Token typeToken = la; 
		
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeOrNamespaceNode typeNode; 
			Type(out typeNode);
			joinNode.TypeName = typeNode; 
		} else if (la.kind == 1) {
		} else SynErr(195);
		Expect(1);
		joinNode.IdentifierToken = t; 
		Expect(38);
		joinNode.InToken = t; 
		Expression(out exprNode);
		joinNode.InExpression = exprNode; 
		Expect(1);
		joinNode.OnToken = t; 
		Expression(out exprNode);
		joinNode.OnExpression = exprNode; 
		Expect(1);
		joinNode.EqualsToken = t; 
		Expression(out exprNode);
		joinNode.EqualsExpression = exprNode; 
		if (la.kind == 1) {
			if (la.kind == _ident && la.val == "into") {
				Expect(1);
				joinNode.IntoToken = t; 
				Expect(1);
				joinNode.IntoIdentifierToken = t; 
			} else {
			}
		}
		Terminate(joinNode); 
	}

	void OrderByClause(out OrderByClauseNode obNode) {
		OrderingClauseNode ordNode; 
		Expect(1);
		obNode = new OrderByClauseNode(t); 
		SetCommentOwner(obNode);
		
		OrderingClause(out ordNode);
		obNode.Orderings.Add(ordNode); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			OrderingClause(out ordNode);
			obNode.Orderings.Add(separator, ordNode); 
		}
		Terminate(obNode); 
	}

	void IntoClause(out IntoClauseNode intoNode) {
		Expect(1);
		intoNode = new IntoClauseNode(t); 
		SetCommentOwner(intoNode);
		
		Expect(1);
		intoNode.IdentifierToken = t; 
		Terminate(intoNode);
		
	}

	void SelectClause(out SelectClauseNode selNode) {
		Expect(1);
		selNode = new SelectClauseNode(t);
		SetCommentOwner(selNode);
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		selNode.Expression = exprNode;
		Terminate(selNode);
		
	}

	void GroupClause(out GroupByClauseNode groupNode) {
		groupNode = null; 
		ExpressionNode exprNode; 
		Expect(1);
		groupNode = new GroupByClauseNode(t); 
		SetCommentOwner(groupNode);
		
		Expression(out exprNode);
		groupNode.GroupExpression = exprNode; 
		Expect(125);
		groupNode.ByToken = t; 
		Expression(out exprNode);
		groupNode.ByExpression = exprNode; 
		Terminate(groupNode);
		
	}

	void WhereClause(out WhereClauseNode whereNode) {
		ExpressionNode exprNode; 
		Expect(122);
		whereNode = new WhereClauseNode(t); 
		SetCommentOwner(whereNode);
		
		Expression(out exprNode);
		whereNode.Expression = exprNode; 
		Terminate(whereNode);
		
	}

	void OrderingClause(out OrderingClauseNode ordNode) {
		ordNode = new OrderingClauseNode(la);
		SetCommentOwner(ordNode);
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		ordNode.Expression = exprNode; 
		if (la.kind == 123 || la.kind == 124) {
			if (la.kind == 123) {
				Get();
			} else {
				Get();
			}
			ordNode.Direction = t; 
		}
	}

	void NullCoalescingExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		OrExpr(out exprNode);
		while (la.kind == 126) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.NullCoalescing);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			OrExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void OrExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		AndExpr(out exprNode);
		while (la.kind == 127) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.ConditionalOr);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			AndExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void AndExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		BitOrExpr(out exprNode);
		while (la.kind == 128) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.ConditionalAnd);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			BitOrExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitOrExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		BitXorExpr(out exprNode);
		while (la.kind == 129) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.LogicalOr);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			BitXorExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitXorExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		BitAndExpr(out exprNode);
		while (la.kind == 130) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.LogicalXor);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			BitAndExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitAndExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		EqlExpr(out exprNode);
		while (la.kind == 83) {
			Get();
			var opNode = new BinaryOperatorNode(t, BinaryOperatorType.LogicalAnd);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			EqlExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void EqlExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		RelExpr(out exprNode);
		BinaryOperatorNode opNode = null; 
		while (la.kind == 92 || la.kind == 105) {
			if (la.kind == 105) {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.NotEquals); 
			} else {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Equals); 
			}
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNodeBase rgNode; 
			RelExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void RelExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null; 
		ShiftExpr(out exprNode);
		BinaryOperatorNode opNode = null; 
		while (StartOf(28)) {
			if (StartOf(29)) {
				if (la.kind == 100) {
					Get();
					opNode = new BinaryOperatorNode(t, BinaryOperatorType.LessThan); 
				} else if (la.kind == 93) {
					Get();
					opNode = new BinaryOperatorNode(t, BinaryOperatorType.GreaterThan); 
				} else if (la.kind == 131) {
					Get();
					opNode = new BinaryOperatorNode(t, BinaryOperatorType.LessThanOrEqual); 
				} else if (la.kind == 94) {
					Get();
					opNode = new BinaryOperatorNode(t, BinaryOperatorType.GreaterThanOrEqual); 
				} else SynErr(196);
				SetCommentOwner(opNode);
				opNode.LeftOperand = exprNode;
				ExpressionNode unaryNode;
				
				Unary(out unaryNode);
				BinaryOperatorNodeBase rgNode; 
				ShiftExpr(out rgNode);
				BindBinaryOperator(opNode, unaryNode, rgNode);
				exprNode = opNode;
				
			} else {
				TypeTestingOperatorNode typeTestingNode = null; 
				if (la.kind == 42) {
					Get();
					typeTestingNode = new TypeTestingOperatorNode(t, TypeTestingOperatorType.Is); 
				} else if (la.kind == 7) {
					Get();
					typeTestingNode = new TypeTestingOperatorNode(t, TypeTestingOperatorType.As); 
				} else SynErr(197);
				SetCommentOwner(typeTestingNode);
				TypeOrNamespaceNode typeNode; 
				
				TypeInRelExpr(out typeNode);
				typeTestingNode.RightOperand = typeNode;
				typeTestingNode.LeftOperand = exprNode;
				SetCommentOwner(typeTestingNode.RightOperand);
				exprNode = typeTestingNode;
				
			}
		}
		Terminate(exprNode); 
	}

	void ShiftExpr(out BinaryOperatorNodeBase exprNode) {
		exprNode = null;
		Token start;
		BinaryOperatorNode addExpr;
		
		AddExpr(out addExpr);
		exprNode = addExpr;
		BinaryOperatorNode opNode = null; 
		
		while (IsShift()) {
			if (la.kind == 101) {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.LeftShift); 
			} else if (la.kind == 93) {
				Get();
				start = t; 
				Expect(93);
				opNode = new BinaryOperatorNode(start, t, BinaryOperatorType.RightShift); 
			} else SynErr(198);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNode rgNode; 
			AddExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void AddExpr(out BinaryOperatorNode exprNode) {
		exprNode = null; 
		MulExpr(out exprNode);
		BinaryOperatorNode opNode = null; 
		while (la.kind == 102 || la.kind == 108) {
			if (la.kind == 108) {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Addition); 
			} else {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Subtraction); 
			}
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryOperatorNode rgNode; 
			MulExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void MulExpr(out BinaryOperatorNode exprNode) {
		exprNode = null;
		BinaryOperatorNode opNode = null;
		
		while (la.kind == 116 || la.kind == 132 || la.kind == 133) {
			if (la.kind == 116) {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Multiplication); 
			} else if (la.kind == 132) {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Division); 
			} else {
				Get();
				opNode = new BinaryOperatorNode(t, BinaryOperatorType.Modulo); 
			}
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			opNode.RightOperand = unaryNode;
			exprNode = opNode;
			Terminate(opNode);
			
		}
	}

	void Primary(out ExpressionNode exprNode) {
		ExpressionNode innerNode = null;
		exprNode = null;
		
		if (StartOf(30)) {
			Literal(out innerNode);
		} else if (la.kind == 98) {
			Get();
			var pExprNode = new ParenthesizedExpressionNode(t); 
			SetCommentOwner(pExprNode);
			
			Expression(out innerNode);
			pExprNode.Expression = innerNode;
			innerNode = pExprNode;
			
			Expect(113);
			Terminate(pExprNode); 
		} else if (StartOf(31)) {
			PredefinedTypeMemberAccess(out innerNode);
		} else if (IsQualifiedAliasMember()) {
			QualifiedAliasMemberAccess(out innerNode);
		} else if (la.kind == 1) {
			SimpleNameNode sn = null; 
			SimpleName(out sn);
			innerNode = sn; 
		} else if (la.kind == 68) {
			Get();
			innerNode = new ThisAccessNode(t); 
			SetCommentOwner(innerNode);
			
		} else if (la.kind == 8) {
			Get();
			if (la.kind == 90) {
				BaseMemberAccessNode baseNode = null; 
				BaseMemberAccess(out baseNode);
				innerNode = baseNode; 
			} else if (la.kind == 97) {
				BaseElementAccessNode baseNode = null; 
				BaseElementAccess(out baseNode);
				innerNode = baseNode; 
			} else SynErr(199);
		} else if (la.kind == 46) {
			NewOperator(out innerNode);
		} else if (la.kind == 72) {
			TypeOfOperator(out innerNode);
		} else if (la.kind == 15) {
			CheckedOperator(out innerNode);
		} else if (la.kind == 75) {
			UncheckedOperator(out innerNode);
		} else if (la.kind == 20) {
			DefaultOperator(out innerNode);
		} else if (la.kind == 21) {
			AnonymousDelegate(out innerNode);
		} else if (la.kind == 62) {
			SizeOfOperator(out innerNode);
		} else SynErr(200);
		var curExprNode = innerNode; 
		while (StartOf(32)) {
			switch (la.kind) {
			case 95: {
				Get();
				var incNode = new PostIncrementOperatorNode(t);
				SetCommentOwner(incNode);
				incNode.Operand = curExprNode;
				curExprNode = incNode;
				
				break;
			}
			case 88: {
				Get();
				var decNode = new PostDecrementOperatorNode(t);
				SetCommentOwner(decNode);
				decNode.Operand = curExprNode;
				curExprNode = decNode;
				
				break;
			}
			case 134: {
				Get();
				SimpleNameNode snNode;
				var pointerNode = new PointerMemberAccessOperatorNode(t, innerNode);
				SetCommentOwner(pointerNode);
				
				SimpleName(out snNode);
				pointerNode.MemberName = snNode;
				curExprNode = pointerNode;
				
				break;
			}
			case 90: {
				Get();
				SimpleNameNode snNode;
				var maNode = new PrimaryMemberAccessOperatorNode(t, innerNode);
				SetCommentOwner(maNode);
				
				SimpleName(out snNode);
				maNode.MemberName = snNode;
				curExprNode = maNode;
				
				break;
			}
			case 98: {
				Get();
				var invNode = new InvocationOperatorNode(t, innerNode);
				SetCommentOwner(invNode);
				
				CurrentArgumentList(invNode.Arguments);
				Expect(113);
				Terminate(invNode);
				curExprNode = invNode;
				
				break;
			}
			case 97: {
				Get();
				var elementAccess = new ElementAccessNode(t, innerNode);
				SetCommentOwner(elementAccess);
				
				ArrayIndexer(elementAccess.Expressions);
				Expect(112);
				Terminate(elementAccess); 
				curExprNode = elementAccess;
				
				break;
			}
			}
		}
		exprNode = curExprNode; 
	}

	void Literal(out ExpressionNode valNode) {
		valNode = null; 
		switch (la.kind) {
		case 2: {
			Get();
			valNode = IntegerLiteralNode.Create(t); 
			break;
		}
		case 3: {
			Get();
			valNode = RealLiteralNode.Create(t); 
			break;
		}
		case 4: {
			Get();
			valNode = new CharLiteralNode(t); 
			break;
		}
		case 5: {
			Get();
			valNode = new StringLiteralNode(t); 
			break;
		}
		case 70: {
			Get();
			valNode = new TrueLiteralNode(t); 
			break;
		}
		case 29: {
			Get();
			valNode = new FalseLiteralNode(t); 
			break;
		}
		case 47: {
			Get();
			valNode = new NullLiteralNode(t); 
			break;
		}
		default: SynErr(201); break;
		}
		SetCommentOwner(valNode); 
	}

	void PredefinedTypeMemberAccess(out ExpressionNode exprNode) {
		exprNode = null; 
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
		default: SynErr(202); break;
		}
		var typeNode = TypeOrNamespaceNode.CreateTypeNode(t);
		var predefMemAccessNode = new PredefinedTypeMemberAccessNode(t, typeNode);
		SetCommentOwner(predefMemAccessNode);
		
		Expect(90);
		SimpleNameNode nameNode = new SimpleNameNode(t);
		predefMemAccessNode.MemberName = nameNode;
		nameNode.SeparatorToken = t; 
		
		Expect(1);
		nameNode.IdentifierToken = t;
		TypeOrNamespaceNodeCollection argList;
		
		if (IsGeneric()) {
			TypeArgumentList(out argList);
			nameNode.Arguments = argList; 
		}
		exprNode = predefMemAccessNode;
		Terminate(exprNode);
		
	}

	void QualifiedAliasMemberAccess(out ExpressionNode exprNode) {
		exprNode = null; 
		Expect(1);
		var qam = new QualifiedAliasMemberNode(t);
		
		Expect(91);
		qam.QualifierSeparatorToken = t;
		
		Expect(1);
		qam.IdentifierToken = t; 
		Terminate(qam);
		
		if (la.kind == 100) {
			TypeOrNamespaceNodeCollection argList; 
			
			TypeArgumentList(out argList);
			qam.Arguments = argList; 
			Terminate(qam);
			
		}
		Expect(90);
		Token separatorToken = t;
		
		Expect(1);
		SimpleNameNode simpleNameNode = new SimpleNameNode(t);
		simpleNameNode.SeparatorToken = separatorToken;
		Terminate(simpleNameNode);
		
		    exprNode = new QualifiedAliasMemberAccessNode(t, qam, simpleNameNode); 
		    SetCommentOwner(exprNode);
		
		    Terminate(exprNode); 
		  
	}

	void SimpleName(out SimpleNameNode sn) {
		sn = null; 
		Expect(1);
		sn = new SimpleNameNode(t); 
		SetCommentOwner(sn);
		TypeOrNamespaceNodeCollection argList; 
		
		if (IsGeneric()) {
			TypeArgumentList(out argList);
			sn.Arguments = argList; 
		}
		Terminate(sn); 
	}

	void BaseMemberAccess(out BaseMemberAccessNode exprNode) {
		Expect(90);
		Token separatorToken = t;
		exprNode = new BaseMemberAccessNode(t); 
		SetCommentOwner(exprNode);
		
		Expect(1);
		var simpleNameNode = new SimpleNameNode(t);
		SetCommentOwner(simpleNameNode);
		simpleNameNode.SeparatorToken = separatorToken;
		Terminate(simpleNameNode);
		
		    exprNode.MemberName = simpleNameNode; 
		    exprNode.Terminate(t); 
		  
	}

	void BaseElementAccess(out BaseElementAccessNode exprNode) {
		Expect(97);
		Token separatorToken = t;
		exprNode = new BaseElementAccessNode(t);
		SetCommentOwner(exprNode);
		
		ArrayIndexer(exprNode.Expressions);
		Expect(112);
		exprNode.Terminate(t); 
	}

	void NewOperator(out ExpressionNode exprNode) {
		exprNode = null; 
		Expect(46);
		var newToken = t; 
		if (la.kind == 96) {
			var anonNode = new NewOperatorWithAnonymousTypeNode(t); 
			SetCommentOwner(anonNode);
			
			AnonymousObjectInitializer(anonNode);
			exprNode = anonNode; 
		} else if (StartOf(33)) {
			TypeOrNamespaceNode typeNode; 
			NonArrayType(out typeNode);
			NewOperatorWithType(newToken, typeNode, out exprNode);
		} else if (la.kind == 97) {
			var impArrNode = new NewOperatorWithArrayNode(t); 
			SetCommentOwner(impArrNode);
			
			ImplicitArrayCreation(impArrNode);
			exprNode = impArrNode; 
		} else SynErr(203);
		Terminate(exprNode); 
	}

	void TypeOfOperator(out ExpressionNode exprNode) {
		Expect(72);
		var topNode = new TypeofOperatorNode(t);
		SetCommentOwner(topNode);
		exprNode = topNode;
		
		Expect(98);
		topNode.OpenParenthesis = t;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeNode);
		topNode.TypeName = typeNode; 
		Expect(113);
		topNode.CloseParenthesis = t;
		Terminate(exprNode);
		
	}

	void CheckedOperator(out ExpressionNode exprNode) {
		Expect(15);
		var copNode = new CheckedOperatorNode(t);
		SetCommentOwner(copNode);
		exprNode = copNode;
		
		Expect(98);
		copNode.OpenParenthesis = t;
		ExpressionNode innerNode;
		
		Expression(out innerNode);
		copNode.Expression = innerNode; 
		Expect(113);
		copNode.CloseParenthesis = t;
		Terminate(exprNode);
		
	}

	void UncheckedOperator(out ExpressionNode exprNode) {
		Expect(75);
		var uopNode = new UncheckedOperatorNode(t);
		SetCommentOwner(uopNode);
		exprNode = uopNode;
		
		Expect(98);
		uopNode.OpenParenthesis = t;
		ExpressionNode innerNode;
		
		Expression(out innerNode);
		uopNode.Expression = innerNode; 
		Expect(113);
		uopNode.CloseParenthesis = t;
		Terminate(exprNode);
		
	}

	void DefaultOperator(out ExpressionNode exprNode) {
		Expect(20);
		var defNode = new DefaultOperatorNode(t);
		SetCommentOwner(defNode);
		exprNode = defNode;
		
		Expect(98);
		defNode.OpenParenthesis = t;
		ExpressionNode primNode;
		
		Primary(out primNode);
		defNode.Expression = primNode; 
		Expect(113);
		defNode.CloseParenthesis = t;
		Terminate(defNode);
		
	}

	void AnonymousDelegate(out ExpressionNode exprNode) {
		Expect(21);
		var adNode = new AnonymousDelegateNode(t);
		SetCommentOwner(adNode);
		exprNode = adNode;
		
		if (la.kind == 98) {
			Get();
			var parsNode = new FormalParameterListNode(t); 
			SetCommentOwner(parsNode);
			adNode.ParameterList = parsNode;
			
			if (StartOf(34)) {
				FormalParameterNode parNode; 
				AnonymousMethodParameter(out parNode);
				parsNode.Items.Add(parNode); 
				while (la.kind == 87) {
					Get();
					var separator = t; 
					AnonymousMethodParameter(out parNode);
					parsNode.Items.Add(separator, parNode); 
				}
			}
			Expect(113);
			Terminate(parsNode); 
		}
		BlockStatementNode blockNode; 
		Block(out blockNode);
		Terminate(exprNode); 
	}

	void SizeOfOperator(out ExpressionNode exprNode) {
		Expect(62);
		var sopNode = new SizeofOperatorNode(t);
		SetCommentOwner(sopNode);
		exprNode = sopNode;
		
		Expect(98);
		sopNode.OpenParenthesis = t;
		TypeOrNamespaceNode typeNode;
		
		Type(out typeNode);
		sopNode.TypeName = typeNode; 
		Expect(113);
		sopNode.CloseParenthesis = t;
		Terminate(exprNode);
		
	}

	void ArrayIndexer(ExpressionNodeCollection exprList) {
		ExpressionNode exprNode; 
		Token separator = null;
		
		Expression(out exprNode);
		if (exprList != null)
		{
		  exprList.Add(exprNode);
		}
		
		while (la.kind == 87) {
			Get();
			separator = t; 
			Expression(out exprNode);
			if (exprList != null)
			{
			  exprList.Add(separator, exprNode);
			}
			
		}
	}

	void AnonymousObjectInitializer(NewOperatorWithAnonymousTypeNode anonNode) {
		Expect(96);
		anonNode.OpenBrace = t; 
		MemberDeclaratorList(anonNode);
		if (la.kind == 87) {
			Get();
			anonNode.OrphanComma = t; 
		}
		Expect(111);
		anonNode.CloseBrace = t; 
		Terminate(anonNode);
		
	}

	void NewOperatorWithType(Token newToken, TypeOrNamespaceNode typeNode, out ExpressionNode exprNode) {
		exprNode = null; 
		ObjectOrCollectionInitializerNode initNode; 
		if (la.kind == 98) {
			var scNode = new NewOperatorWithConstructorNode(newToken); 
			SetCommentOwner(scNode);
			scNode.TypeName = typeNode;
			exprNode = scNode;
			
			Get();
			var invNode = new InvocationOperatorNode(t, scNode);
			SetCommentOwner(invNode);
			
			CurrentArgumentList(invNode.Arguments);
			Expect(113);
			Terminate(invNode);
			scNode.Initializer = invNode;
			
			if (la.kind == 96) {
				ObjectOrCollectionInitializer(out initNode);
				scNode.ObjectInitializer = initNode; 
			}
			Terminate(scNode); 
		} else if (la.kind == 96) {
			var scNode = new NewOperatorWithConstructorNode(newToken); 
			SetCommentOwner(scNode);
			scNode.TypeName = typeNode;
			exprNode = scNode;
			
			ObjectOrCollectionInitializer(out initNode);
			scNode.ObjectInitializer = initNode; 
			Terminate(scNode);
			
		} else if (IsDims()) {
			var newOpNode = new NewOperatorWithArrayNode(t); 
			SetCommentOwner(newOpNode);
			newOpNode.TypeName = typeNode;
			exprNode = newOpNode;
			
			ImplicitArrayCreation(newOpNode);
			Terminate(newOpNode); 
		} else if (la.kind == 97) {
			var newOpNode = new NewOperatorWithArrayNode(newToken); 
			SetCommentOwner(newOpNode);
			newOpNode.TypeName = typeNode;
			exprNode = newOpNode;
			ExpressionNode initExprNode; 
			
			Get();
			newOpNode.SizedDimensions.StartToken = t; 
			Expression(out initExprNode);
			newOpNode.SizedDimensions.Add(initExprNode); 
			while (la.kind == 87) {
				Get();
				var separator = t; 
				Expression(out initExprNode);
				newOpNode.SizedDimensions.Add(separator, initExprNode); 
			}
			Expect(112);
			Terminate(newOpNode.SizedDimensions); 
			while (IsDims()) {
				Expect(97);
				newOpNode.OpenSquareBracket = t; 
				while (la.kind == 87) {
					Get();
					newOpNode.Commas.Add(t); 
				}
				Expect(112);
				newOpNode.CloseSquareBracket = t; 
			}
			if (la.kind == 96) {
				ArrayInitializerNode arrInitNode; 
				ArrayInitializer(out arrInitNode);
				newOpNode.Initializer = arrInitNode; 
			}
			Terminate(newOpNode);
			exprNode = newOpNode;
			
		} else SynErr(204);
	}

	void ImplicitArrayCreation(NewOperatorWithArrayNode impArrNode) {
		Expect(97);
		impArrNode.OpenSquareBracket = t; 
		while (la.kind == 87) {
			Get();
			impArrNode.Commas.Add(t); 
		}
		Expect(112);
		impArrNode.CloseSquareBracket = t; 
		if (la.kind == 96) {
			ArrayInitializerNode initNode; 
			ArrayInitializer(out initNode);
			impArrNode.Initializer = initNode; 
		}
		Terminate(impArrNode); 
	}

	void MemberDeclaratorList(NewOperatorWithAnonymousTypeNode initNode) {
		MemberDeclaratorNode mdNode; 
		MemberDeclarator(out mdNode);
		initNode.Declarators.Add(mdNode); 
		while (NotFinalComma()) {
			Expect(87);
			var separator = t; 
			MemberDeclarator(out mdNode);
			initNode.Declarators.Add(separator, mdNode); 
		}
	}

	void MemberDeclarator(out MemberDeclaratorNode mdNode) {
		ExpressionNode exprNode;
		mdNode = null;
		
		if (IsMemberInitializer()) {
			Expect(1);
			Token start = t; 
			mdNode = new MemberDeclaratorNode(t);
			SetCommentOwner(mdNode);
			mdNode.Kind = MemberDeclaratorNode.DeclaratorKind.Expression;
			
			Expect(85);
			mdNode.EqualToken = t; 
			Expression(out exprNode);
			mdNode.Expression = exprNode; 
		} else if (StartOf(27)) {
			Token start = la; 
			ExpressionNode primNode;
			
			Primary(out primNode);
			mdNode = new MemberDeclaratorNode(primNode == null ? t : primNode.StartToken);
			SetCommentOwner(mdNode);
			mdNode.Kind = MemberDeclaratorNode.DeclaratorKind.SimpleName;
			mdNode.Expression = primNode;
			
		} else if (StartOf(31)) {
			TypeOrNamespaceNode typeNode; 
			PredefinedType(out typeNode);
			mdNode = new MemberDeclaratorNode(typeNode.StartToken);
			SetCommentOwner(mdNode);
			mdNode.Kind = MemberDeclaratorNode.DeclaratorKind.MemberAccess;
			mdNode.TypeName = typeNode;
			
			Expect(90);
			mdNode.DotSeparator = t; 
			Expect(1);
			mdNode.IdentifierToken = t; 
		} else SynErr(205);
		Terminate(mdNode); 
	}

	void ObjectOrCollectionInitializer(out ObjectOrCollectionInitializerNode oiNode) {
		Expect(96);
		oiNode = new ObjectOrCollectionInitializerNode(t); 
		SetCommentOwner(oiNode);
		
		if (IsEmptyMemberInitializer()) {
			Expect(111);
			Terminate(oiNode); 
		} else if (IsMemberInitializer()) {
			MemberInitializerList(oiNode);
			if (la.kind == 87) {
				Get();
				oiNode.OrphanSeparator = t; 
			}
			Expect(111);
			Terminate(oiNode); 
		} else if (StartOf(20)) {
			ElementInitializerList(oiNode);
			if (la.kind == 87) {
				Get();
				oiNode.OrphanSeparator = t; 
			}
			Expect(111);
			Terminate(oiNode); 
		} else SynErr(206);
	}

	void MemberInitializerList(ObjectOrCollectionInitializerNode ocNode) {
		MemberInitializerNode miNode; 
		MemberInitializer(out miNode);
		ocNode.MemberInitializers.Add(miNode); 
		while (NotFinalComma()) {
			Expect(87);
			var separator = t; 
			MemberInitializer(out miNode);
			ocNode.MemberInitializers.Add(separator, miNode); 
		}
	}

	void ElementInitializerList(ObjectOrCollectionInitializerNode ocNode) {
		ElementInitializerNode eiNode; 
		ElementInitializer(out eiNode);
		ocNode.ElementInitializers.Add(eiNode); 
		while (NotFinalComma()) {
			Expect(87);
			var separator = t; 
			ElementInitializer(out eiNode);
			ocNode.ElementInitializers.Add(separator, eiNode); 
		}
	}

	void ElementInitializer(out ElementInitializerNode eiNode) {
		eiNode = new ElementInitializerNode(la);
		SetCommentOwner(eiNode);
		ExpressionNode exprNode; 
		
		if (IsValueInitializer()) {
			Expression(out exprNode);
			eiNode.Expression = exprNode; 
		} else if (la.kind == 96) {
			Get();
			eiNode.ExpressionList.StartToken = t; 
			Expression(out exprNode);
			eiNode.ExpressionList.Add(exprNode); 
			while (la.kind == 87) {
				Get();
				var separator = t; 
				Expression(out exprNode);
				eiNode.ExpressionList.Add(separator, exprNode); 
			}
			Expect(111);
			Terminate(eiNode.ExpressionList); 
		} else SynErr(207);
		Terminate(eiNode);
	}

	void MemberInitializer(out MemberInitializerNode miNode) {
		Expect(1);
		miNode = new MemberInitializerNode(t); 
		SetCommentOwner(miNode);
		
		Expect(85);
		miNode.EqualToken = t; 
		if (IsValueInitializer()) {
			ExpressionNode exprNode; 
			Expression(out exprNode);
			miNode.Expression = exprNode; 
		} else if (la.kind == 96) {
			ObjectOrCollectionInitializerNode initNode; 
			ObjectOrCollectionInitializer(out initNode);
			miNode.Initializer = initNode; 
		} else SynErr(208);
		Terminate(miNode); 
	}

	void AnonymousMethodParameter(out FormalParameterNode parNode) {
		var modifier = FormalParameterModifier.In; 
		Token start = null;
		parNode = null;
		
		if (la.kind == 50 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				modifier = FormalParameterModifier.Ref; 
			} else {
				Get();
				modifier = FormalParameterModifier.Out; 
			}
			start = t; 
		}
		TypeOrNamespaceNode typeNode; 
		Type(out typeNode);
		if (start == null) start = typeNode.StartToken; 
		Expect(1);
		parNode = new FormalParameterNode(start);
		SetCommentOwner(parNode);
		parNode.Modifier = modifier;
		parNode.IdentifierToken = t;
		parNode.TypeName = typeNode;
		Terminate(parNode);
		
	}

	void SingleFieldMember(out FieldTagNode fiNode) {
		Expect(1);
		fiNode = new FieldTagNode(t); 
		SetCommentOwner(fiNode);
		
		if (la.kind == 85) {
			Get();
			fiNode.EqualToken = t;
			VariableInitializerNode varInitNode; 
			
			VariableInitializer(out varInitNode);
			fiNode.Initializer = varInitNode; 
		}
		Terminate(fiNode); 
	}

	void TypeParameter(out AttributeDecorationNodeCollection attrNodes, out Token identifier) {
		attrNodes = new AttributeDecorationNodeCollection(); 
		AttributeDecorations(attrNodes);
		Expect(1);
		identifier = t; 
	}

	void TypeParameterConstraintTag(out TypeParameterConstraintTagNode tag) {
		tag = null; 
		if (la.kind == 16) {
			Get();
			tag = new TypeParameterConstraintTagNode(t); 
		} else if (la.kind == 66) {
			Get();
			tag = new TypeParameterConstraintTagNode(t); 
		} else if (la.kind == 46) {
			Get();
			var start = t; 
			Expect(98);
			var openPar = t; 
			Expect(113);
			tag = new TypeParameterConstraintTagNode(start, openPar, t); 
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			TypeOrNamespaceNode typeNode; 
			ClassType(out typeNode);
			tag = new TypeParameterConstraintTagNode(typeNode); 
		} else SynErr(209);
		SetCommentOwner(tag); 
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
		CS3();

      Expect(0);
      if (_RegionStack.Count > 0)
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
		{T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,T, x,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,x,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, T,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, T,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, x,x,x,x, T,x,x,x, x,T,x,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,T,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, x,x,x,x, T,T,T,x, x,x,x,x, T,T,T,x, x,T,x,x, T,x,T,T, T,T,T,x, T,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,x,x, x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,T,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, T,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,T, x,x,x,x, T,T,T,x, x,T,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,x,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,x,x, x,T,x,x, x,x,x,T, x,x,x,T, T,x,x,T, x,T,x,x, x,x,x,x, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, T,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
		{x,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,T, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x}

	  };

    #endregion
    
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
			case 119: s = "larrow expected"; break;
			case 120: s = "\"partial\" expected"; break;
			case 121: s = "\"yield\" expected"; break;
			case 122: s = "\"where\" expected"; break;
			case 123: s = "\"ascending\" expected"; break;
			case 124: s = "\"descending\" expected"; break;
			case 125: s = "\"by\" expected"; break;
			case 126: s = "\"??\" expected"; break;
			case 127: s = "\"||\" expected"; break;
			case 128: s = "\"&&\" expected"; break;
			case 129: s = "\"|\" expected"; break;
			case 130: s = "\"^\" expected"; break;
			case 131: s = "\"<=\" expected"; break;
			case 132: s = "\"/\" expected"; break;
			case 133: s = "\"%\" expected"; break;
			case 134: s = "\"->\" expected"; break;
			case 135: s = "??? expected"; break;
			case 136: s = "invalid NamespaceMemberDeclaration"; break;
			case 137: s = "invalid TypeDeclaration"; break;
			case 138: s = "invalid TypeDeclaration"; break;
			case 139: s = "invalid EnumDeclaration"; break;
			case 140: s = "invalid ClassType"; break;
			case 141: s = "invalid ClassMemberDeclaration"; break;
			case 142: s = "invalid ClassMemberDeclaration"; break;
			case 143: s = "invalid StructMemberDeclaration"; break;
			case 144: s = "invalid StructMemberDeclaration"; break;
			case 145: s = "invalid StructMemberDeclaration"; break;
			case 146: s = "invalid IntegralType"; break;
			case 147: s = "this symbol not expected in EnumBody"; break;
			case 148: s = "this symbol not expected in EnumBody"; break;
			case 149: s = "invalid Expression"; break;
			case 150: s = "invalid Expression"; break;
			case 151: s = "invalid Type"; break;
			case 152: s = "invalid EventDeclaration"; break;
			case 153: s = "invalid ConstructorDeclaration"; break;
			case 154: s = "invalid ConstructorDeclaration"; break;
			case 155: s = "invalid MethodDeclaration"; break;
			case 156: s = "invalid OperatorDeclaration"; break;
			case 157: s = "invalid CastOperatorDeclaration"; break;
			case 158: s = "invalid CastOperatorDeclaration"; break;
			case 159: s = "invalid AccessorDeclarations"; break;
			case 160: s = "invalid AccessorDeclarations"; break;
			case 161: s = "invalid OverloadableOp"; break;
			case 162: s = "invalid InterfaceMemberDeclaration"; break;
			case 163: s = "invalid InterfaceMemberDeclaration"; break;
			case 164: s = "invalid InterfaceMemberDeclaration"; break;
			case 165: s = "invalid LocalVariableDeclaration"; break;
			case 166: s = "invalid LocalVariableDeclarator"; break;
			case 167: s = "invalid VariableInitializer"; break;
			case 168: s = "invalid Attributes"; break;
			case 169: s = "invalid Keyword"; break;
			case 170: s = "invalid AttributeArguments"; break;
			case 171: s = "invalid PrimitiveType"; break;
			case 172: s = "invalid PointerOrArray"; break;
			case 173: s = "invalid NonArrayType"; break;
			case 174: s = "invalid TypeInRelExpr"; break;
			case 175: s = "invalid PredefinedType"; break;
			case 176: s = "invalid Statement"; break;
			case 177: s = "invalid EmbeddedStatement"; break;
			case 178: s = "invalid EmbeddedStatement"; break;
			case 179: s = "invalid StatementExpression"; break;
			case 180: s = "invalid ForEachStatement"; break;
			case 181: s = "invalid GotoStatement"; break;
			case 182: s = "invalid TryFinallyBlock"; break;
			case 183: s = "invalid UsingStatement"; break;
			case 184: s = "invalid ForInitializer"; break;
			case 185: s = "invalid CatchClauses"; break;
			case 186: s = "invalid Unary"; break;
			case 187: s = "invalid Unary"; break;
			case 188: s = "invalid AssignmentOperator"; break;
			case 189: s = "invalid SwitchLabel"; break;
			case 190: s = "invalid LambdaFunctionSignature"; break;
			case 191: s = "invalid LambdaFunctionSignature"; break;
			case 192: s = "invalid LambdaFunctionBody"; break;
			case 193: s = "invalid FromClause"; break;
			case 194: s = "invalid QueryBodyClause"; break;
			case 195: s = "invalid JoinClause"; break;
			case 196: s = "invalid RelExpr"; break;
			case 197: s = "invalid RelExpr"; break;
			case 198: s = "invalid ShiftExpr"; break;
			case 199: s = "invalid Primary"; break;
			case 200: s = "invalid Primary"; break;
			case 201: s = "invalid Literal"; break;
			case 202: s = "invalid PredefinedTypeMemberAccess"; break;
			case 203: s = "invalid NewOperator"; break;
			case 204: s = "invalid NewOperatorWithType"; break;
			case 205: s = "invalid MemberDeclarator"; break;
			case 206: s = "invalid ObjectOrCollectionInitializer"; break;
			case 207: s = "invalid ElementInitializer"; break;
			case 208: s = "invalid MemberInitializer"; break;
			case 209: s = "invalid TypeParameterConstraintTag"; break;

  			  default: s = "error " + n; break;
	  	  }
        Error("SYNERR", la, s, null);
	  	}
		  errDist = 0;
	  }

	  #endregion
  }

#pragma warning restore 1591

}