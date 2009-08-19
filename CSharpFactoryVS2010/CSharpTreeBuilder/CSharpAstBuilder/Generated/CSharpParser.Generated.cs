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
	public const int maxT = 129;
	public const int _ppDefine = 130;
	public const int _ppUndef = 131;
	public const int _ppIf = 132;
	public const int _ppElif = 133;
	public const int _ppElse = 134;
	public const int _ppEndif = 135;
	public const int _ppLine = 136;
	public const int _ppError = 137;
	public const int _ppWarning = 138;
	public const int _ppPragma = 139;
	public const int _ppRegion = 140;
	public const int _ppEndReg = 141;
	public const int _cBlockCom = 142;
	public const int _cLineCom = 143;

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
				if (la.kind == 130) {
				AddConditionalDirective(la); 
				}
				if (la.kind == 131) {
				RemoveConditionalDirective(la); 
				}
				if (la.kind == 132) {
				IfPragma(la); 
				}
				if (la.kind == 133) {
				ElifPragma(la); 
				}
				if (la.kind == 134) {
				ElsePragma(la); 
				}
				if (la.kind == 135) {
				EndifPragma(la); 
				}
				if (la.kind == 136) {
				LinePragma(la); 
				}
				if (la.kind == 137) {
				ErrorPragma(la); 
				}
				if (la.kind == 138) {
				WarningPragma(la); 
				}
				if (la.kind == 139) {
				PragmaPragma(la); 
				}
				if (la.kind == 140) {
				RegionPragma(la); 
				}
				if (la.kind == 141) {
				EndRegionPragma(la); 
				}
				if (la.kind == 142) {
				HandleBlockComment(la); 
				}
				if (la.kind == 143) {
				HandleLineComment(la); 
				}

			  la = t;
      }
    }
	
	  #region Parser methods generated by CoCo
	  
	void CS3() {
		CompilationUnitNode.SetStartToken(la); 
		
		while (IsExternAliasDirective()) {
			ExternAliasDirective(CompilationUnitNode);
		}
		while (la.kind == 78) {
			UsingDirective(CompilationUnitNode);
		}
		while (IsGlobalAttrTarget()) {
			GlobalAttributes();
		}
		while (StartOf(1)) {
			NamespaceMemberDeclaration(CompilationUnitNode);
		}
		Terminate(CompilationUnitNode); 
		
	}

	void ExternAliasDirective(NamespaceScopeNode parentNode) {
		Expect(28);
		SignRealToken(); 
		var eaNode = New<ExternAliasNode>(t);
		
		Expect(1);
		if (t.val != "alias") Error1003(t, "alias"); 
		eaNode.AliasToken = t;
		
		Expect(1);
		eaNode.IdentifierToken = t; 
		Expect(114);
		Terminate(eaNode); 
		parentNode.ExternAliasNodes.Add(eaNode);
		
	}

	void UsingDirective(NamespaceScopeNode parentNode) {
		Token alias = null;
		Token eq = null;
		NamespaceOrTypeNameNode nsNode = null;
		
		Expect(78);
		SignRealToken();
		Token start = t;
		
		if (IsAssignment()) {
			Expect(1);
			alias = t; 
			Expect(85);
			eq = t; 
		}
		NamespaceOrTypeName(out nsNode);
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
		CompilationUnitNode.GlobalAttributes.Add(globAttrNode);
		
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
			
		} else SynErr(130);
	}

	void NamespaceOrTypeName(out NamespaceOrTypeNameNode resultNode) {
		resultNode = null;
		Token separator = null;
		Token identifier = null;
		TypeNodeCollection argList = null;
		
		Expect(1);
		resultNode = new NamespaceOrTypeNameNode(t);
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
		NamespaceOrTypeNameNode nsNode = null;
		
		NamespaceOrTypeName(out nsNode);
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
			if (la.kind == 1) {
				Get();
				partialToken = t; 
			}
			if (la.kind == 16) {
				ClassDeclaration(out typeDecl);
			} else if (la.kind == 66) {
				StructDeclaration(out typeDecl);
			} else if (la.kind == 40) {
				InterfaceDeclaration(out typeDecl);
			} else SynErr(131);
		} else if (la.kind == 25) {
			EnumDeclaration(out typeDecl);
		} else if (la.kind == 21) {
			DelegateDeclaration(out typeDecl);
		} else SynErr(132);
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
		while (la.kind == 1) {
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
		while (la.kind == 1) {
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
		while (la.kind == 1) {
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
			TypeNode typeNode = null; 
			if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
				ClassType(out typeNode);
			} else if (StartOf(6)) {
				IntegralType(out typeNode);
			} else SynErr(133);
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
		TypeNode typeNode;
		
		Type(out typeNode);
		Expect(1);
		var ddNode = new DelegateDeclarationNode(start, t);
		SetCommentOwner(ddNode);
		typeDecl = ddNode;
		ddNode.Type = typeNode;
		
		if (la.kind == 100) {
			TypeParameterList(typeDecl);
		}
		Expect(98);
		if (StartOf(7)) {
			FormalParameterList(ddNode.FormalParameters);
		}
		Expect(113);
		Terminate(ddNode.FormalParameters); 
		while (la.kind == 1) {
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
		TypeNode typeNode; 
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
		constrNode = null;
		
		Expect(1);
		if (t.val != "where") Error("SYNERR", t, "'where' token is expected", null);
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

	void ClassType(out TypeNode typeNode) {
		typeNode = null; 
		if (la.kind == 1) {
			NamespaceOrTypeNameNode namespaceOrTypeName;
			
			NamespaceOrTypeName(out namespaceOrTypeName);
			typeNode = new TypeNode(namespaceOrTypeName.StartToken);
			typeNode.TypeName = namespaceOrTypeName;
			typeNode.Terminate(t);
			
		} else if (la.kind == 48 || la.kind == 65) {
			if (la.kind == 48) {
				Get();
			} else {
				Get();
			}
			typeNode = TypeNode.CreateTypeNode(t); 
		} else SynErr(134);
	}

	void ClassMemberDeclaration(AttributeDecorationNodeCollection attrNodes, ModifierNodeCollection mod,
TypeDeclarationNode typeDecl, out MemberDeclarationNode memNode) {
		memNode = null; 
		if (StartOf(9)) {
			StructMemberDeclaration(attrNodes, mod, typeDecl, out memNode);
		} else if (la.kind == 115) {
			Get();
			var finNode = new DestructorDeclarationNode(t);
			SetCommentOwner(finNode);
			memNode = finNode;
			
			Expect(1);
			finNode.IdentifierToken = t; 
			Expect(98);
			Expect(113);
			Terminate(finNode.FormalParameters); 
			if (la.kind == 96) {
				BlockStatementNode blockNode; 
				Block(out blockNode);
				finNode.Body = blockNode; 
			} else if (la.kind == 114) {
				Get();
				finNode.ClosingSemicolon = t; 
			} else SynErr(135);
			Terminate(memNode); 
		} else SynErr(136);
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
			Expect(1);
			var partialToken = t; 
			TypeNode typeNode; 
			
			Type(out typeNode);
			NamespaceOrTypeNameNode nameNode; 
			MemberName(out nameNode);
			var metNode = new MethodDeclarationNode(partialToken);
			SetCommentOwner(metNode);
			metNode.Type = typeNode;
			metNode.MemberName = nameNode;
			memNode = metNode;
			
			MethodDeclaration(metNode);
		} else if (StartOf(11)) {
			TypeNode typeNode; 
			Type(out typeNode);
			if (la.kind == 49) {
				var opNode = new OperatorDeclarationNode(typeNode.StartToken); 
				SetCommentOwner(opNode);
				opNode.Type = typeNode;
				memNode = opNode;
				
				OperatorDeclaration(opNode);
			} else if (IsFieldDecl()) {
				var fiNode = new FieldDeclarationNode(typeNode.StartToken); 
				SetCommentOwner(fiNode);
				fiNode.Type = typeNode;
				memNode = fiNode;
				
				FieldMemberDeclarators(fiNode);
				Expect(114);
				Terminate(memNode); 
			} else if (la.kind == 1) {
				NamespaceOrTypeNameNode nameNode; 
				MemberName(out nameNode);
				if (la.kind == 96) {
					var propNode = new PropertyDeclarationNode(typeNode.StartToken);
					SetCommentOwner(propNode);
					propNode.Type = typeNode; 
					propNode.MemberName = nameNode;
					memNode = propNode;
					
					PropertyDeclaration(propNode);
				} else if (la.kind == 90) {
					var indNode = new IndexerDeclarationNode(typeNode.StartToken);
					SetCommentOwner(indNode);
					indNode.Type = typeNode;
					indNode.MemberName = nameNode;
					memNode = indNode;
					
					Get();
					indNode.MemberNameSeparator = t; 
					IndexerDeclaration(indNode);
				} else if (la.kind == 98 || la.kind == 100) {
					var metNode = new MethodDeclarationNode(typeNode.StartToken);
					SetCommentOwner(metNode);
					metNode.Type = typeNode;
					metNode.MemberName = nameNode;
					memNode = metNode;
					
					MethodDeclaration(metNode);
				} else SynErr(137);
			} else if (la.kind == 68) {
				var indNode = new IndexerDeclarationNode(typeNode.StartToken);
				SetCommentOwner(indNode);
				indNode.Type = typeNode;
				memNode = indNode;
				
				IndexerDeclaration(indNode);
			} else SynErr(138);
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
			
		} else SynErr(139);
	}

	void IntegralType(out TypeNode typeNode) {
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
		default: SynErr(140); break;
		}
		typeNode = TypeNode.CreateTypeNode(t); 
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
				while (!(la.kind == 0 || la.kind == 1 || la.kind == 97)) {SynErr(141); Get();}
				EnumMemberDeclaration(out valNode);
				typeDecl.Values.Add(separator, valNode); 
			}
			if (la.kind == 87) {
				Get();
				typeDecl.OrphanSeparator = t; 
			}
		}
		while (!(la.kind == 0 || la.kind == 111)) {SynErr(142); Get();}
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
			qNode.QueryBody = bodyNode;
			
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
				AssignmentExpressionNode asgnNode; 
				AssignmentOp(out asgnNode);
				ExpressionNode rightExprNode; 
				Expression(out rightExprNode);
				asgnNode.RightOperand = rightExprNode;
				asgnNode.LeftOperand = leftExprNode;
				exprNode = asgnNode;
				
			} else if (StartOf(14)) {
				BinaryExpressionNodeBase ncNode; 
				NullCoalescingExpr(out ncNode);
				if (ncNode == null)
				{
				  exprNode = leftExprNode;
				}
				else
				{
				  ncNode.LeftmostExpressionWithMissingLeftOperand.LeftOperand = leftExprNode;
				  exprNode = ncNode;
				}
				
				if (la.kind == 110) {
					Get();
					var condNode = new ConditionalExpressionNode(exprNode);
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
			} else SynErr(143);
		} else SynErr(144);
		if (exprNode != null) Terminate(exprNode); 
	}

	void Type(out TypeNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else if (la.kind == 80) {
			Get();
			typeNode = TypeNode.CreateTypeNode(t); 
		} else SynErr(145);
		if (la.kind == 110) {
			Get();
			typeNode.NullableToken = t; 
		}
		PointerOrArray(typeNode);
		Terminate(typeNode); 
	}

	void FormalParameterList(FormalParameterNodeCollection parsNode) {
		FormalParameterNode node; 
		FormalParameterTag(out node);
		if (parsNode != null && node != null) parsNode.Add(node); 
		while (la.kind == 87) {
			Get();
			var separator = t; 
			FormalParameterTag(out node);
			if (parsNode != null && node != null) 
			 parsNode.Add(separator, node); 
			
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
		TypeNode typeNode;
		
		Type(out typeNode);
		memNode.Type = typeNode; 
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
		TypeNode typeNode; 
		Expect(26);
		var eventToken = t; 
		Type(out typeNode);
		if (IsFieldDecl()) {
			var fiNode = new FieldDeclarationNode(eventToken); 
			SetCommentOwner(fiNode);
			memNode = fiNode;
			fiNode.Type = typeNode;
			
			FieldMemberDeclarators(fiNode);
			Expect(114);
			Terminate(fiNode); 
		} else if (la.kind == 1) {
			var evpNode = new EventPropertyDeclarationNode(t);
			SetCommentOwner(evpNode);
			memNode = evpNode;
			evpNode.Type = typeNode;
			NamespaceOrTypeNameNode memberName = null;
			
			MemberName(out memberName);
			evpNode.MemberName = memberName; 
			Expect(96);
			evpNode.OpenBrace = t; 
			EventAccessorDeclarations(evpNode);
			Expect(111);
			evpNode.CloseBrace = t;
			Terminate(evpNode);
			
		} else SynErr(146);
	}

	void ConstructorDeclaration(out MemberDeclarationNode memNode) {
		memNode = null; 
		Expect(1);
		var cstNode = new ConstructorDeclarationNode(t);
		SetCommentOwner(cstNode);
		memNode = cstNode;
		
		Expect(98);
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
				
			} else SynErr(147);
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
		} else SynErr(148);
		Terminate(memNode); 
	}

	void MemberName(out NamespaceOrTypeNameNode resultNode) {
		resultNode = null;
		Token separator = null;
		Token identifier = null;
		TypeNodeCollection argList = null;
		
		Expect(1);
		resultNode = new NamespaceOrTypeNameNode(t);
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
		if (StartOf(7)) {
			FormalParameterList(metNode.FormalParameters);
		}
		Expect(113);
		Terminate(metNode.FormalParameters); 
		while (la.kind == 1) {
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
		} else SynErr(149);
		Terminate(metNode); 
	}

	void OperatorDeclaration(OperatorDeclarationNode opNode) {
		Expect(49);
		opNode.OperatorToken = t; 
		OverloadableOp(opNode);
		Expect(98);
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
		} else SynErr(150);
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
		} else SynErr(151);
		var copNode = new CastOperatorDeclarationNode(t); 
		SetCommentOwner(copNode);
		memNode = copNode;
		
		Expect(49);
		copNode.OperatorToken = t; 
		TypeNode typeNode; 
		
		Type(out typeNode);
		copNode.Type = typeNode; 
		Expect(98);
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
		} else SynErr(152);
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
		} else SynErr(153);
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
			} else SynErr(154);
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
			case 126: {
				Get();
				opNode.Kind = OverloadableOperatorType.Division; 
				break;
			}
			case 127: {
				Get();
				opNode.Kind = OverloadableOperatorType.Modulo; 
				break;
			}
			case 83: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseAnd; 
				break;
			}
			case 123: {
				Get();
				opNode.Kind = OverloadableOperatorType.BitwiseOr; 
				break;
			}
			case 124: {
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
			case 125: {
				Get();
				opNode.Kind = OverloadableOperatorType.LessThanOrEqual; 
				break;
			}
			}
			opNode.KindToken = t; 
		} else SynErr(155);
	}

	void InterfaceMemberDeclaration(InterfaceDeclarationNode typeDecl) {
		var mod = new ModifierNodeCollection();
		var attrNodes = new AttributeDecorationNodeCollection();
		Token identifier;
		MemberDeclarationNode memNode = null;
		
		AttributeDecorations(attrNodes);
		ModifierList(mod);
		if (StartOf(11)) {
			TypeNode typeNode; 
			Type(out typeNode);
			if (la.kind == 1) {
				Get();
				identifier = t; 
				if (la.kind == 98 || la.kind == 100) {
					var metNode = new MethodDeclarationNode(typeNode.StartToken);
					SetCommentOwner(metNode);
					metNode.Type = typeNode;
					metNode.MemberName = NamespaceOrTypeNameNode.CreateSimpleName(identifier);
					memNode = metNode;
					
					MethodDeclaration(metNode);
				} else if (la.kind == 96) {
					var propNode = new PropertyDeclarationNode(typeNode.StartToken);
					SetCommentOwner(propNode);
					propNode.Type = typeNode;
					propNode.MemberName = NamespaceOrTypeNameNode.CreateSimpleName(identifier);
					memNode = propNode;
					
					Get();
					propNode.OpenBrace = t; 
					InterfaceAccessors(propNode);
					Expect(111);
					propNode.CloseBrace = t; 
					Terminate(memNode);
					
				} else SynErr(156);
			} else if (la.kind == 68) {
				Get();
				var indNode = new IndexerDeclarationNode(typeNode.StartToken);
				SetCommentOwner(indNode);
				indNode.Type = typeNode;
				indNode.ThisToken = t;
				memNode = indNode;
				
				Expect(97);
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
				
			} else SynErr(157);
		} else if (la.kind == 26) {
			InterfaceEventDeclaration(out memNode);
		} else SynErr(158);
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
		
		TypeNode typeNode; 
		Type(out typeNode);
		ieNode.Type = typeNode; 
		Expect(1);
		ieNode.IdentifierToken = t; 
		Expect(114);
		Terminate(memNode); 
	}

	void LocalVariableDeclaration(out LocalVariableNode varNode) {
		TypeNode typeNode = null;
		varNode = null;
		
		if (IsVar()) {
			Expect(1);
			typeNode = TypeNode.CreateTypeNode(t); 
		} else if (StartOf(11)) {
			Type(out typeNode);
		} else SynErr(159);
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
				
				TypeNode typeNode; 
				Type(out typeNode);
				stcInitNode.Type = typeNode; 
				Expect(97);
				stcInitNode.OpenSquareToken = t;
				ExpressionNode exprNode; 
				
				Expression(out exprNode);
				stcInitNode.Expression = exprNode; 
				Expect(112);
				stcInitNode.CloseSquareToken = t;
				Terminate(stcInitNode);
				
			} else SynErr(160);
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
		} else SynErr(161);
	}

	void ArrayInitializer(out ArrayInitializerNode initNode) {
		initNode = null; 
		Expect(96);
		initNode = new ArrayInitializerNode(t); 
		SetCommentOwner(initNode);
		
		if (StartOf(20)) {
			VariableInitializerNode varInitNode; 
			VariableInitializer(out varInitNode);
			initNode.VariableInitializers.Add(varInitNode);
			
			while (NotFinalComma()) {
				Expect(87);
				var separator = t; 
				VariableInitializer(out varInitNode);
				varInitNode.SeparatorToken = t; 
				initNode.VariableInitializers.Add(varInitNode);
				
			}
			if (la.kind == 87) {
				Get();
				initNode.OrphanComma = t; 
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
		TypeNode typeNode; 
		Type(out typeNode);
		if (start == null) start = typeNode.StartToken; 
		Expect(1);
		parNode = new FormalParameterNode(start);
		SetCommentOwner(parNode);
		parNode.AttributeDecorations = attrNodes;
		parNode.Modifier = modifier;
		parNode.IdentifierToken = t;
		parNode.Type = typeNode;
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
			} else SynErr(162);
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
		default: SynErr(163); break;
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
				} else SynErr(164);
				Expression(out exprNode);
				newArg = new AttributeArgumentNode(identifier, equal, exprNode);
				SetCommentOwner(newArg);
				argsNode.Arguments.Add(separator, newArg); 
				
			}
		}
		Expect(113);
		Terminate(argsNode.Arguments); 
	}

	void PrimitiveType(out TypeNode typeNode) {
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
			typeNode = TypeNode.CreateTypeNode(t); 
		} else SynErr(165);
	}

	void PointerOrArray(TypeNode typeNode) {
		while (IsPointerOrDims()) {
			if (la.kind == 116) {
				Get();
				typeNode.PointerTokens.Add(t); 
			} else if (la.kind == 97) {
				Get();
				var rankSpecifier = new RankSpecifierNode(t);
				typeNode.RankSpecifiers.Add(rankSpecifier);
				SetCommentOwner(rankSpecifier);
				
				while (la.kind == 87) {
					Get();
					rankSpecifier.Commas.Add(t); 
				}
				Expect(112);
				rankSpecifier.CloseSquareBracket = t;
				Terminate(rankSpecifier); 
				
			} else SynErr(166);
		}
	}

	void NonArrayType(out TypeNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else SynErr(167);
		if (la.kind == 110) {
			Get();
			typeNode.NullableToken = t; 
		}
		while (la.kind == 116) {
			Get();
			typeNode.PointerTokens.Add(t); 
			
		}
		Terminate(typeNode); 
	}

	void TypeInRelExpr(out TypeNode typeNode) {
		typeNode = null; 
		if (StartOf(15)) {
			PrimitiveType(out typeNode);
		} else if (la.kind == 1 || la.kind == 48 || la.kind == 65) {
			ClassType(out typeNode);
		} else if (la.kind == 80) {
			Get();
			typeNode = TypeNode.CreateTypeNode(t); 
		} else SynErr(168);
		if (IsNullableTypeMark()) {
			Expect(110);
			typeNode.NullableToken = t; 
		}
		PointerOrArray(typeNode);
		Terminate(typeNode); 
	}

	void TypeArgumentList(out TypeNodeCollection argList) {
		argList = null; 
		Expect(100);
		argList = new TypeNodeCollection(); 
		SetCommentOwner(argList);
		Start(argList);
		
		   TypeNode typeNode = null;
		
		if (StartOf(11)) {
			Type(out typeNode);
		}
		if (typeNode==null) { typeNode = TypeNode.CreateEmptyTypeNode(null); }
		argList.Add(typeNode); 
		
		while (la.kind == 87) {
			Get();
			typeNode = null;
			
			if (StartOf(11)) {
				Type(out typeNode);
			}
			if (typeNode==null) { typeNode = TypeNode.CreateEmptyTypeNode(t); }
			argList.Add(typeNode); 
			
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
		} else SynErr(169);
	}

	void ConstStatement(out StatementNode stmtNode) {
		ExpressionNode exprNode; 
		Expect(17);
		var csNode = new ConstStatementNode(t);
		SetCommentOwner(csNode);
		stmtNode = csNode;
		TypeNode typeNode;
		
		Type(out typeNode);
		csNode.Type = typeNode; 
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
		if (la.kind == _ident && la.val == "yield") {
			Expect(1);
			if (la.kind == 58) {
				YieldReturnStatement(out stmtNode);
			} else if (la.kind == 10) {
				YieldBreakStatement(out stmtNode);
			} else SynErr(170);
		} else if (la.kind == 96) {
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
		} else if (la.kind == 31) {
			FixedStatement(out stmtNode);
		} else SynErr(171);
	}

	void YieldReturnStatement(out StatementNode stmtNode) {
		var start = t;
		ExpressionNode exprNode; 
		
		Expect(58);
		var returnToken = t; 
		Expression(out exprNode);
		stmtNode = new YieldReturnStatementNode(t, returnToken, exprNode); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
	}

	void YieldBreakStatement(out StatementNode stmtNode) {
		var start = t; 
		Expect(10);
		var breakToken = t; 
		stmtNode = new YieldBreakStatementNode(t, breakToken); 
		SetCommentOwner(stmtNode);
		
		Expect(114);
		Terminate(stmtNode); 
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
			AssignmentExpressionNode asgnNode; 
			AssignmentOp(out asgnNode);
			asgnNode.LeftOperand = unaryNode;
			exprNode = asgnNode;
			ExpressionNode rightNode; 
			
			Expression(out rightNode);
			asgnNode.RightOperand = rightNode; 
		} else if (la.kind == 87 || la.kind == 113 || la.kind == 114) {
			if (isAssignment) Error("UNDEF", la, "error in assignment."); 
		} else SynErr(172);
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
		whNode.Condition = exprNode; 
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
			forNode.Condition = exprNode; 
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
		TypeNode typeNode = null; 
		
		if (IsVar()) {
			Expect(1);
			typeNode = TypeNode.CreateTypeNode(t); 
		} else if (StartOf(11)) {
			Type(out typeNode);
		} else SynErr(173);
		feNode.Type = typeNode; 
		Expect(1);
		feNode.IdentifierToken = t; 
		Expect(38);
		feNode.InToken = t; 
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		feNode.CollectionExpression = exprNode; 
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
		} else SynErr(174);
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
		} else SynErr(175);
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
		} else SynErr(176);
		Expect(113);
		usNode.CloseParenthesis = t;   
		StatementNode bodyNode; 
		
		EmbeddedStatement(out bodyNode);
		usNode.Statement = bodyNode;
		Terminate(stmtNode); 
		
	}

	void FixedStatement(out StatementNode stmtNode) {
		Expect(31);
		var fixNode = new FixedStatementNode(t);
		SetCommentOwner(fixNode);
		stmtNode = fixNode;
		
		Expect(98);
		fixNode.OpenParenthesis = t; 
		TypeNode typeNode; 
		
		Type(out typeNode);
		fixNode.Type = typeNode; 
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
		} else SynErr(177);
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
			TypeNode typeNode;
			
			ClassType(out typeNode);
			ccNode.Type = typeNode; 
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
		} else SynErr(178);
	}

	void Unary(out ExpressionNode exprNode) {
		exprNode = null;  
		UnaryExpressionNodeBase unaryOp = null;
		
		if (unaryHead[la.kind] || IsTypeCast()) {
			switch (la.kind) {
			case 108: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.Identity); 
				break;
			}
			case 102: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.Negation);
				break;
			}
			case 106: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.LogicalNegation); 
				break;
			}
			case 115: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.BitwiseNegation); 
				break;
			}
			case 95: {
				Get();
				unaryOp = new PreIncrementExpressionNode(t); 
				break;
			}
			case 88: {
				Get();
				unaryOp = new PreDecrementExpressionNode(t); 
				break;
			}
			case 116: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.PointerIndirection); 
				break;
			}
			case 83: {
				Get();
				unaryOp = new UnaryOperatorExpressionNode(t, UnaryOperator.AddressOf); 
				break;
			}
			case 98: {
				Get();
				TypeNode typeNode; 
				var tcNode = new CastExpressionNode(t);
				unaryOp = tcNode;
				
				Type(out typeNode);
				tcNode.Type = typeNode; 
				Expect(113);
				Terminate(tcNode); 
				break;
			}
			default: SynErr(179); break;
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
		} else SynErr(180);
	}

	void AssignmentOp(out AssignmentExpressionNode opNode) {
		AssignmentOperator op = AssignmentOperator.SimpleAssignment;
		Token start;
		Token second = null;
		
		start = la; 
		switch (la.kind) {
		case 85: {
			Get();
			op = AssignmentOperator.SimpleAssignment; 
			break;
		}
		case 109: {
			Get();
			op = AssignmentOperator.AdditionAssignment; 
			break;
		}
		case 103: {
			Get();
			op = AssignmentOperator.SubtractionAssignment; 
			break;
		}
		case 117: {
			Get();
			op = AssignmentOperator.MultiplicationAssignment; 
			break;
		}
		case 89: {
			Get();
			op = AssignmentOperator.DivisionAssignment; 
			break;
		}
		case 104: {
			Get();
			op = AssignmentOperator.ModuloAssignment; 
			break;
		}
		case 84: {
			Get();
			op = AssignmentOperator.LogicalAndAssignment; 
			break;
		}
		case 107: {
			Get();
			op = AssignmentOperator.LogicalOrAssignment; 
			break;
		}
		case 118: {
			Get();
			op = AssignmentOperator.LogicalXorAssignment; 
			break;
		}
		case 99: {
			Get();
			op = AssignmentOperator.LeftShiftAssignment; 
			break;
		}
		case 93: {
			Get();
			int pos = t.pos; 
			Expect(94);
			if (pos+1 < t.pos) Error("UNDEF", la, "no whitespace allowed in right shift assignment");
			op = AssignmentOperator.RightShiftAssignment;
			second = t;
			
			break;
		}
		default: SynErr(181); break;
		}
		opNode = new AssignmentExpressionNode(start, second, op);
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
		} else SynErr(182);
		Expect(86);
		Terminate(slNode); 
	}

	void LambdaFunctionSignature(LambdaExpressionNode lambdaNode) {
		if (la.kind == 1) {
			Get();
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
			} else if (la.kind == 1) {
				Get();
				parNode = new FormalParameterNode(t);
				SetCommentOwner(parNode);
				parNode.Type = TypeNode.CreateEmptyTypeNode(null);
				parNode.IdentifierToken = t;
				Terminate(parNode);
				lambdaNode.FormalParameters.Add(parNode);
				
				while (la.kind == 87) {
					Get();
					var separator = t; 
					Expect(1);
					parNode = new FormalParameterNode(t);
					SetCommentOwner(parNode);
					parNode.Type = TypeNode.CreateEmptyTypeNode(null);
					parNode.IdentifierToken = t;
					Terminate(parNode);
					lambdaNode.FormalParameters.Add(separator, parNode);
					
				}
			} else if (la.kind == 113) {
			} else SynErr(183);
			Expect(113);
			lambdaNode.OpenParenthesis = t; 
		} else SynErr(184);
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
		TypeNode typeNode; 
		Type(out typeNode);
		fpNode.Type = typeNode; 
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
		} else SynErr(185);
	}

	void FromClause(out FromClauseNode fromNode) {
		Expect(1);
		if (t.val!="from") Error("SYNERR", t, "Expected 'from' but found '{0}'.", t.val);
		fromNode = new FromClauseNode(t); 
		SetCommentOwner(fromNode);
		var typeToken = la;
		
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeNode typeNode; 
			Type(out typeNode);
			fromNode.Type = typeNode; 
		}
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
		
		while (la.kind == _ident && (la.val == "from" || la.val == "let" || la.val == "where" || la.val == "join" || la.val == "orderby")) {
			QueryBodyClauseNode bodyClauseNode = null; 
			QueryBodyClause(out bodyClauseNode);
			bodyNode.BodyClauses.Add(bodyClauseNode); 
		}
		if (la.kind == _ident && la.val == "select") {
			SelectClauseNode selectNode; 
			SelectClause(out selectNode);
			bodyNode.SelectClause = selectNode; 
		} else if (la.kind == 1) {
			GroupClauseNode groupNode; 
			GroupClause(out groupNode);
			bodyNode.GroupClause = groupNode; 
		} else SynErr(186);
		if (la.kind == _ident && la.val == "into") {
			QueryContinuationNode intoNode; 
			QueryContinuation(out intoNode);
			bodyNode.QueryContinuation = intoNode; 
		}
	}

	void QueryBodyClause(out QueryBodyClauseNode bodyClauseNode) {
		bodyClauseNode=null; 
		if (la.kind == _ident && la.val == "from") {
			FromClauseNode fromNode; 
			FromClause(out fromNode);
			bodyClauseNode = fromNode; 
		} else if (la.kind == _ident && la.val == "let") {
			LetClauseNode letNode; 
			LetClause(out letNode);
			bodyClauseNode = letNode; 
		} else if (la.kind == _ident && la.val == "where") {
			WhereClauseNode whereNode; 
			WhereClause(out whereNode);
			bodyClauseNode = whereNode; 
		} else if (la.kind == _ident && la.val == "join") {
			JoinClauseNode joinNode; 
			JoinClause(out joinNode);
			bodyClauseNode = joinNode; 
		} else if (la.kind == 1) {
			OrderByClauseNode orderNode; 
			OrderByClause(out orderNode);
			bodyClauseNode = orderNode; 
		} else SynErr(187);
	}

	void SelectClause(out SelectClauseNode selNode) {
		Expect(1);
		if (t.val!="select") Error("SYNERR", t, "Expected 'select' but found '{0}'.", t.val);
		selNode = new SelectClauseNode(t);
		SetCommentOwner(selNode);
		ExpressionNode exprNode; 
		
		Expression(out exprNode);
		selNode.Expression = exprNode;
		Terminate(selNode);
		
	}

	void GroupClause(out GroupClauseNode groupNode) {
		groupNode = null; 
		ExpressionNode exprNode; 
		Expect(1);
		if (t.val!="group") Error("SYNERR", t, "Expected 'group' but found '{0}'.", t.val);
		groupNode = new GroupClauseNode(t); 
		SetCommentOwner(groupNode);
		
		Expression(out exprNode);
		groupNode.GroupExpression = exprNode; 
		Expect(1);
		if (t.val!="by") Error("SYNERR", t, "Expected 'by' but found '{0}'.", t.val);
		groupNode.ByToken = t; 
		
		Expression(out exprNode);
		groupNode.ByExpression = exprNode; 
		Terminate(groupNode);
		
	}

	void QueryContinuation(out QueryContinuationNode intoNode) {
		Expect(1);
		if (t.val!="into") Error("SYNERR", t, "Expected 'into' but found '{0}'.", t.val);
		intoNode = new QueryContinuationNode(t); 
		SetCommentOwner(intoNode);
		
		Expect(1);
		intoNode.IdentifierToken = t; 
		Terminate(intoNode);
		QueryBodyNode bodyNode;
		
		QueryBody(out bodyNode);
		intoNode.QueryBody = bodyNode;
		
	}

	void LetClause(out LetClauseNode letNode) {
		ExpressionNode exprNode; 
		Expect(1);
		if (t.val!="let") Error("SYNERR", t, "Expected 'let' but found '{0}'.", t.val);
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

	void WhereClause(out WhereClauseNode whereNode) {
		ExpressionNode exprNode; 
		Expect(1);
		if (t.val!="where") Error("SYNERR", t, "Expected 'where' but found '{0}'.", t.val);
		whereNode = new WhereClauseNode(t); 
		SetCommentOwner(whereNode);
		
		Expression(out exprNode);
		whereNode.Expression = exprNode; 
		Terminate(whereNode);
		
	}

	void JoinClause(out JoinClauseNode joinNode) {
		ExpressionNode exprNode; 
		Expect(1);
		if (t.val!="join") Error("SYNERR", t, "Expected 'join' but found '{0}'.", t.val);
		joinNode = new JoinClauseNode(t);
		SetCommentOwner(joinNode);
		Token typeToken = la; 
		
		if (IsType(ref typeToken) && typeToken.val != "in") {
			TypeNode typeNode; 
			Type(out typeNode);
			joinNode.Type = typeNode; 
		}
		Expect(1);
		joinNode.IdentifierToken = t; 
		Expect(38);
		joinNode.InToken = t; 
		Expression(out exprNode);
		joinNode.InExpression = exprNode; 
		Expect(1);
		if (t.val!="on") Error("SYNERR", t, "Expected 'on' but found '{0}'.", t.val);
		joinNode.OnToken = t; 
		
		Expression(out exprNode);
		joinNode.OnExpression = exprNode; 
		Expect(1);
		if (t.val!="equals") Error("SYNERR", t, "Expected 'equals' but found '{0}'.", t.val);
		joinNode.EqualsToken = t; 
		
		Expression(out exprNode);
		joinNode.EqualsExpression = exprNode; 
		if (la.kind == _ident && la.val == "into") {
			var joinIntoNode = new JoinIntoClauseNode(joinNode.StartToken);
			joinIntoNode.Comment = joinNode.Comment;
			joinIntoNode.Type = joinNode.Type;
			joinIntoNode.IdentifierToken = joinNode.IdentifierToken;
			joinIntoNode.InToken = joinNode.InToken;
			joinIntoNode.InExpression = joinNode.InExpression;
			joinIntoNode.OnToken = joinNode.OnToken;
			joinIntoNode.OnExpression = joinNode.OnExpression;
			joinIntoNode.EqualsToken = joinNode.EqualsToken;
			joinIntoNode.EqualsExpression = joinNode.EqualsExpression;
			
			Expect(1);
			if (t.val!="into") Error("SYNERR", t, "Expected 'into' but found '{0}'.", t.val);
			joinIntoNode.IntoToken = t; 
			
			Expect(1);
			joinIntoNode.IntoIdentifierToken = t; 
			joinNode = joinIntoNode;
			
		}
		Terminate(joinNode); 
	}

	void OrderByClause(out OrderByClauseNode obNode) {
		OrderingClauseNode ordNode; 
		Expect(1);
		if (t.val!="orderby") Error("SYNERR", t, "Expected 'orderby' but found '{0}'.", t.val);
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

	void OrderingClause(out OrderingClauseNode ordNode) {
		ordNode = new OrderingClauseNode(la);
		SetCommentOwner(ordNode);
		ExpressionNode exprNode;
		
		Expression(out exprNode);
		ordNode.Expression = exprNode; 
		if (la.kind == _ident && (la.val == "ascending" || la.val == "descending")) {
			Expect(1);
			ordNode.Direction = t; 
		}
	}

	void NullCoalescingExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		OrExpr(out exprNode);
		while (la.kind == 120) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.NullCoalescing);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			OrExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void OrExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		AndExpr(out exprNode);
		while (la.kind == 121) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.ConditionalOr);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			AndExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void AndExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		BitOrExpr(out exprNode);
		while (la.kind == 122) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.ConditionalAnd);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			BitOrExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitOrExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		BitXorExpr(out exprNode);
		while (la.kind == 123) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.LogicalOr);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			BitXorExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitXorExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		BitAndExpr(out exprNode);
		while (la.kind == 124) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.LogicalXor);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			BitAndExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void BitAndExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		EqlExpr(out exprNode);
		while (la.kind == 83) {
			Get();
			var opNode = new BinaryExpressionNode(t, BinaryOperator.LogicalAnd);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			EqlExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void EqlExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		RelExpr(out exprNode);
		BinaryExpressionNode opNode = null; 
		while (la.kind == 92 || la.kind == 105) {
			if (la.kind == 105) {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.NotEquals); 
			} else {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Equals); 
			}
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNodeBase rgNode; 
			RelExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void RelExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null; 
		ShiftExpr(out exprNode);
		BinaryExpressionNode opNode = null; 
		while (StartOf(28)) {
			if (StartOf(29)) {
				if (la.kind == 100) {
					Get();
					opNode = new BinaryExpressionNode(t, BinaryOperator.LessThan); 
				} else if (la.kind == 93) {
					Get();
					opNode = new BinaryExpressionNode(t, BinaryOperator.GreaterThan); 
				} else if (la.kind == 125) {
					Get();
					opNode = new BinaryExpressionNode(t, BinaryOperator.LessThanOrEqual); 
				} else if (la.kind == 94) {
					Get();
					opNode = new BinaryExpressionNode(t, BinaryOperator.GreaterThanOrEqual); 
				} else SynErr(188);
				SetCommentOwner(opNode);
				opNode.LeftOperand = exprNode;
				ExpressionNode unaryNode;
				
				Unary(out unaryNode);
				BinaryExpressionNodeBase rgNode; 
				ShiftExpr(out rgNode);
				BindBinaryOperator(opNode, unaryNode, rgNode);
				exprNode = opNode;
				
			} else {
				TypeTestingExpressionNode typeTestingNode = null; 
				if (la.kind == 42) {
					Get();
					typeTestingNode = new TypeTestingExpressionNode(t, TypeTestingOperator.Is); 
				} else if (la.kind == 7) {
					Get();
					typeTestingNode = new TypeTestingExpressionNode(t, TypeTestingOperator.As); 
				} else SynErr(189);
				SetCommentOwner(typeTestingNode);
				TypeNode typeNode; 
				
				TypeInRelExpr(out typeNode);
				typeTestingNode.RightOperand = typeNode;
				typeTestingNode.LeftOperand = exprNode;
				SetCommentOwner(typeTestingNode.RightOperand);
				exprNode = typeTestingNode;
				
			}
		}
		Terminate(exprNode); 
	}

	void ShiftExpr(out BinaryExpressionNodeBase exprNode) {
		exprNode = null;
		Token start;
		BinaryExpressionNode addExpr;
		
		AddExpr(out addExpr);
		exprNode = addExpr;
		BinaryExpressionNode opNode = null; 
		
		while (IsShift()) {
			if (la.kind == 101) {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.LeftShift); 
			} else if (la.kind == 93) {
				Get();
				start = t; 
				Expect(93);
				opNode = new BinaryExpressionNode(start, t, BinaryOperator.RightShift); 
			} else SynErr(190);
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNode rgNode; 
			AddExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void AddExpr(out BinaryExpressionNode exprNode) {
		exprNode = null; 
		MulExpr(out exprNode);
		BinaryExpressionNode opNode = null; 
		while (la.kind == 102 || la.kind == 108) {
			if (la.kind == 108) {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Addition); 
			} else {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Subtraction); 
			}
			SetCommentOwner(opNode);
			opNode.LeftOperand = exprNode;
			ExpressionNode unaryNode;
			
			Unary(out unaryNode);
			BinaryExpressionNode rgNode; 
			MulExpr(out rgNode);
			BindBinaryOperator(opNode, unaryNode, rgNode);
			exprNode = opNode;
			
		}
	}

	void MulExpr(out BinaryExpressionNode exprNode) {
		exprNode = null;
		BinaryExpressionNode opNode = null;
		
		while (la.kind == 116 || la.kind == 126 || la.kind == 127) {
			if (la.kind == 116) {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Multiplication); 
			} else if (la.kind == 126) {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Division); 
			} else {
				Get();
				opNode = new BinaryExpressionNode(t, BinaryOperator.Modulo); 
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
		exprNode = null;
		
		if (StartOf(30)) {
			Literal(out exprNode);
		} else if (la.kind == 98) {
			Get();
			var pExprNode = new ParenthesizedExpressionNode(t); 
			SetCommentOwner(pExprNode);
			
			Expression(out exprNode);
			pExprNode.Expression = exprNode;
			exprNode = pExprNode;
			
			Expect(113);
			Terminate(pExprNode); 
		} else if (StartOf(31)) {
			PredefinedTypeMemberAccess(out exprNode);
		} else if (IsQualifiedAliasMember()) {
			QualifiedAliasMemberAccess(out exprNode);
		} else if (la.kind == 1) {
			SimpleNameNode sn = null; 
			SimpleName(out sn);
			exprNode = sn; 
		} else if (la.kind == 68) {
			Get();
			exprNode = new ThisAccessNode(t); 
			SetCommentOwner(exprNode);
			
		} else if (la.kind == 8) {
			Get();
			if (la.kind == 90) {
				BaseMemberAccessNode baseNode = null; 
				BaseMemberAccess(out baseNode);
				exprNode = baseNode; 
			} else if (la.kind == 97) {
				BaseElementAccessNode baseNode = null; 
				BaseElementAccess(out baseNode);
				exprNode = baseNode; 
			} else SynErr(191);
		} else if (la.kind == 46) {
			NewOperator(out exprNode);
		} else if (la.kind == 72) {
			TypeOfOperator(out exprNode);
		} else if (la.kind == 15) {
			CheckedOperator(out exprNode);
		} else if (la.kind == 75) {
			UncheckedOperator(out exprNode);
		} else if (la.kind == 20) {
			DefaultOperator(out exprNode);
		} else if (la.kind == 21) {
			AnonymousDelegate(out exprNode);
		} else if (la.kind == 62) {
			SizeOfOperator(out exprNode);
		} else SynErr(192);
		while (StartOf(32)) {
			switch (la.kind) {
			case 95: {
				Get();
				var incNode = new PostIncrementExpressionNode(t);
				SetCommentOwner(incNode);
				incNode.Operand = exprNode;
				exprNode = incNode;
				
				break;
			}
			case 88: {
				Get();
				var decNode = new PostDecrementExpressionNode(t);
				SetCommentOwner(decNode);
				decNode.Operand = exprNode;
				exprNode = decNode;
				
				break;
			}
			case 128: {
				Get();
				SimpleNameNode snNode;
				var pointerNode = new PointerMemberAccessNode(t, exprNode);
				SetCommentOwner(pointerNode);
				
				SimpleName(out snNode);
				pointerNode.MemberName = snNode;
				exprNode = pointerNode;
				
				break;
			}
			case 90: {
				Get();
				SimpleNameNode snNode;
				var maNode = new PrimaryExpressionMemberAccessNode(t, exprNode);
				SetCommentOwner(maNode);
				
				SimpleName(out snNode);
				maNode.MemberName = snNode;
				exprNode = maNode;
				
				break;
			}
			case 98: {
				Get();
				var invNode = new InvocationExpressionNode(t, exprNode);
				SetCommentOwner(invNode);
				
				CurrentArgumentList(invNode.Arguments);
				Expect(113);
				Terminate(invNode);
				exprNode = invNode;
				
				break;
			}
			case 97: {
				Get();
				var elementAccess = new ElementAccessNode(t, exprNode);
				SetCommentOwner(elementAccess);
				
				ArrayIndexer(elementAccess.Expressions);
				Expect(112);
				Terminate(elementAccess); 
				exprNode = elementAccess;
				
				break;
			}
			}
		}
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
		default: SynErr(193); break;
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
		default: SynErr(194); break;
		}
		var typeName = NamespaceOrTypeNameNode.CreateSimpleName(t);
		var predefMemAccessNode = new PredefinedTypeMemberAccessNode(t, typeName);
		SetCommentOwner(predefMemAccessNode);
		
		Expect(90);
		SimpleNameNode nameNode = new SimpleNameNode(t);
		predefMemAccessNode.MemberName = nameNode;
		nameNode.SeparatorToken = t; 
		
		Expect(1);
		nameNode.IdentifierToken = t;
		TypeNodeCollection argList;
		
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
			TypeNodeCollection argList; 
			
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
		TypeNodeCollection argList; 
		
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
			var anonNode = new AnonymousObjectCreationExpressionNode(t); 
			SetCommentOwner(anonNode);
			
			AnonymousObjectInitializer(anonNode);
			exprNode = anonNode; 
		} else if (StartOf(33)) {
			TypeNode typeNode; 
			NonArrayType(out typeNode);
			NewOperatorWithType(newToken, typeNode, out exprNode);
		} else if (la.kind == 97) {
			var impArrNode = new ArrayCreationExpressionNode(t); 
			SetCommentOwner(impArrNode);
			
			ImplicitArrayCreation(impArrNode);
			exprNode = impArrNode; 
		} else SynErr(195);
		Terminate(exprNode); 
	}

	void TypeOfOperator(out ExpressionNode exprNode) {
		Expect(72);
		var topNode = new TypeofExpressionNode(t);
		SetCommentOwner(topNode);
		exprNode = topNode;
		
		Expect(98);
		topNode.OpenParenthesis = t;
		TypeNode typeNode;
		
		Type(out typeNode);
		topNode.Type = typeNode; 
		Expect(113);
		topNode.CloseParenthesis = t;
		Terminate(exprNode);
		
	}

	void CheckedOperator(out ExpressionNode exprNode) {
		Expect(15);
		var copNode = new CheckedExpressionNode(t);
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
		var uopNode = new UncheckedExpressionNode(t);
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
		var defNode = new DefaultValueExpressionNode(t);
		SetCommentOwner(defNode);
		exprNode = defNode;
		
		Expect(98);
		defNode.OpenParenthesis = t;
		TypeNode typeNode;
		
		Type(out typeNode);
		defNode.Type = typeNode; 
		Expect(113);
		defNode.CloseParenthesis = t;
		Terminate(defNode);
		
	}

	void AnonymousDelegate(out ExpressionNode exprNode) {
		Expect(21);
		var adNode = new AnonymousMethodExpressionNode(t);
		SetCommentOwner(adNode);
		exprNode = adNode;
		
		if (la.kind == 98) {
			Get();
			if (StartOf(34)) {
				FormalParameterNode parNode; 
				AnonymousMethodParameter(out parNode);
				adNode.FormalParameters.Add(parNode); 
				while (la.kind == 87) {
					Get();
					var separator = t; 
					AnonymousMethodParameter(out parNode);
					adNode.FormalParameters.Add(separator, parNode); 
				}
			}
			Expect(113);
		}
		BlockStatementNode blockNode; 
		Block(out blockNode);
		adNode.Body = blockNode;
		Terminate(exprNode); 
		
	}

	void SizeOfOperator(out ExpressionNode exprNode) {
		Expect(62);
		var sopNode = new SizeofExpressionNode(t);
		SetCommentOwner(sopNode);
		exprNode = sopNode;
		
		Expect(98);
		sopNode.OpenParenthesis = t;
		TypeNode typeNode;
		
		Type(out typeNode);
		sopNode.Type = typeNode; 
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

	void AnonymousObjectInitializer(AnonymousObjectCreationExpressionNode anonNode) {
		Expect(96);
		anonNode.OpenBrace = t; 
		if (StartOf(13)) {
			MemberDeclaratorList(anonNode);
			if (la.kind == 87) {
				Get();
				anonNode.OrphanComma = t; 
			}
		}
		Expect(111);
		anonNode.CloseBrace = t; 
		Terminate(anonNode);
		
	}

	void NewOperatorWithType(Token newToken, TypeNode typeNode, out ExpressionNode exprNode) {
		exprNode = null; 
		if (la.kind == 98) {
			var objectCreationNode = new ObjectCreationExpressionNode(newToken); 
			SetCommentOwner(objectCreationNode);
			objectCreationNode.Type = typeNode;
			exprNode = objectCreationNode;
			
			Get();
			objectCreationNode.OpenParenthesis = t; 
			CurrentArgumentList(objectCreationNode.Arguments);
			Expect(113);
			objectCreationNode.CloseParenthesis = t; 
			if (la.kind == 96) {
				ObjectOrCollectionInitializerNode initNode; 
				ObjectOrCollectionInitializer(out initNode);
				objectCreationNode.ObjectOrCollectionInitializer = initNode; 
			}
			Terminate(objectCreationNode); 
		} else if (la.kind == 96) {
			var objectCreationNode = new ObjectCreationExpressionNode(newToken); 
			SetCommentOwner(objectCreationNode);
			objectCreationNode.Type = typeNode;
			exprNode = objectCreationNode;
			ObjectOrCollectionInitializerNode initNode; 
			
			ObjectOrCollectionInitializer(out initNode);
			objectCreationNode.ObjectOrCollectionInitializer = initNode; 
			Terminate(objectCreationNode);
			
		} else if (IsDims()) {
			var arrayCreationNode = new ArrayCreationExpressionNode(t); 
			SetCommentOwner(arrayCreationNode);
			arrayCreationNode.Type = typeNode;
			exprNode = arrayCreationNode;
			
			RankSpecifiers(arrayCreationNode);
			ArrayInitializerNode arrInitNode; 
			ArrayInitializer(out arrInitNode);
			arrayCreationNode.Initializer = arrInitNode;
			Terminate(arrayCreationNode); 
			
		} else if (la.kind == 97) {
			var arrayCreationNode = new ArrayCreationExpressionNode(newToken); 
			SetCommentOwner(arrayCreationNode);
			arrayCreationNode.Type = typeNode;
			exprNode = arrayCreationNode;
			ExpressionNode initExprNode; 
			
			Get();
			arrayCreationNode.ArraySizeSpecifier.OpenSquareBracket = t; 
			Expression(out initExprNode);
			arrayCreationNode.ArraySizeSpecifier.Expressions.Add(initExprNode); 
			while (la.kind == 87) {
				Get();
				arrayCreationNode.ArraySizeSpecifier.Commas.Add(t);
				var separator = t; 
				
				Expression(out initExprNode);
				arrayCreationNode.ArraySizeSpecifier.Expressions.Add(separator, initExprNode); 
			}
			Expect(112);
			arrayCreationNode.ArraySizeSpecifier.CloseSquareBracket = t;
			Terminate(arrayCreationNode.ArraySizeSpecifier); 
			
			while (IsDims()) {
				Expect(97);
				var rankSpecifier = new RankSpecifierNode(t);
				arrayCreationNode.RankSpecifiers.Add(rankSpecifier);
				
				while (la.kind == 87) {
					Get();
					rankSpecifier.Commas.Add(t); 
				}
				Expect(112);
				rankSpecifier.CloseSquareBracket = t;
				Terminate(rankSpecifier);
				
			}
			if (la.kind == 96) {
				ArrayInitializerNode arrInitNode; 
				ArrayInitializer(out arrInitNode);
				arrayCreationNode.Initializer = arrInitNode; 
			}
			Terminate(arrayCreationNode);
			exprNode = arrayCreationNode;
			
		} else SynErr(196);
	}

	void ImplicitArrayCreation(ArrayCreationExpressionNode impArrNode) {
		Expect(97);
		var rankSpecifier = new RankSpecifierNode(t);
		impArrNode.RankSpecifiers.Add(rankSpecifier);
		
		while (la.kind == 87) {
			Get();
			rankSpecifier.Commas.Add(t); 
		}
		Expect(112);
		rankSpecifier.CloseSquareBracket = t;
		Terminate(rankSpecifier);
		ArrayInitializerNode initNode; 
		
		ArrayInitializer(out initNode);
		impArrNode.Initializer = initNode;
		Terminate(impArrNode); 
		
	}

	void MemberDeclaratorList(AnonymousObjectCreationExpressionNode anonNode) {
		MemberDeclaratorNode mdNode; 
		MemberDeclarator(out mdNode);
		anonNode.Declarators.Add(mdNode); 
		while (NotFinalComma()) {
			Expect(87);
			var separator = t; 
			MemberDeclarator(out mdNode);
			anonNode.Declarators.Add(separator, mdNode); 
		}
	}

	void MemberDeclarator(out MemberDeclaratorNode mdNode) {
		mdNode = null; 
		ExpressionNode exprNode = null; 
		Token identToken = null;
		Token equalToken = null;
		
		if (IsAssignment()) {
			Expect(1);
			identToken = t; 
			Expect(85);
			equalToken = t; 
		}
		Expression(out exprNode);
		mdNode = CreateMemberDeclarator(identToken, equalToken, exprNode);
		SetCommentOwner(mdNode);
		Terminate(mdNode); 
		
	}

	void ObjectOrCollectionInitializer(out ObjectOrCollectionInitializerNode oiNode) {
		Expect(96);
		oiNode = new ObjectOrCollectionInitializerNode(t); 
		SetCommentOwner(oiNode);
		
		if (la.kind == 111) {
			Get();
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
		} else SynErr(197);
	}

	void RankSpecifiers(ArrayCreationExpressionNode arrayCreationNode) {
		Expect(97);
		var rankSpecifier = new RankSpecifierNode(t);
		arrayCreationNode.RankSpecifiers.Add(rankSpecifier);
		
		while (la.kind == 87) {
			Get();
			rankSpecifier.Commas.Add(t); 
		}
		Expect(112);
		rankSpecifier.CloseSquareBracket = t;
		Terminate(rankSpecifier);
		
		while (la.kind == 97) {
			Get();
			rankSpecifier = new RankSpecifierNode(t);
			arrayCreationNode.RankSpecifiers.Add(rankSpecifier);
			
			while (la.kind == 87) {
				Get();
				rankSpecifier.Commas.Add(t); 
			}
			Expect(112);
			rankSpecifier.CloseSquareBracket = t;
			Terminate(rankSpecifier);
			
		}
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
		
		if (StartOf(13)) {
			var savedToken = la; 
			Expression(out exprNode);
			if (exprNode is AssignmentExpressionNode) 
			{ 
			  Error0747(savedToken); 
			}
			else
			{
			  eiNode.NonAssignmentExpression = exprNode; 
			}
			
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
		} else SynErr(198);
		Terminate(eiNode);
	}

	void MemberInitializer(out MemberInitializerNode miNode) {
		Expect(1);
		miNode = new MemberInitializerNode(t); 
		SetCommentOwner(miNode);
		
		Expect(85);
		miNode.EqualToken = t; 
		if (StartOf(13)) {
			ExpressionNode exprNode; 
			Expression(out exprNode);
			miNode.Expression = exprNode; 
		} else if (la.kind == 96) {
			ObjectOrCollectionInitializerNode initNode; 
			ObjectOrCollectionInitializer(out initNode);
			miNode.Initializer = initNode; 
		} else SynErr(199);
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
		TypeNode typeNode; 
		Type(out typeNode);
		if (start == null) start = typeNode.StartToken; 
		Expect(1);
		parNode = new FormalParameterNode(start);
		SetCommentOwner(parNode);
		parNode.Modifier = modifier;
		parNode.IdentifierToken = t;
		parNode.Type = typeNode;
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
			TypeNode typeNode; 
			ClassType(out typeNode);
			tag = new TypeParameterConstraintTagNode(typeNode); 
		} else SynErr(200);
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
		{T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,T,x, T,x,x,x, T,x,x,x, x,x,x,T, x,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,x,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, T,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, T,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, x,x,x,x, T,x,x,x, x,T,x,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,T,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,T,x,T, x,x,T,x, T,T,x,T, x,T,x,T, x,T,T,T, T,x,x,x, T,x,x,x, x,T,x,T, T,T,x,x, T,x,T,x, T,x,x,T, x,T,T,T, T,x,x,T, T,T,x,x, T,T,T,x, x,x,x,x, x,T,T,x, T,T,x,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, x,x,x,x, T,T,T,x, x,x,x,x, T,T,T,x, x,T,x,x, T,x,T,T, T,T,T,x, T,x,x,x, T,T,T,T, T,T,T,T, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,T,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, T,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,T, x,T,T,T, T,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,T,x, x,x,x,x, x,T,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,T, x,x,x,x, T,T,T,x, x,T,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,T, T,T,T,T, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,T,T, x,x,T,T, x,x,T,T, T,T,T,T, x,x,x,x, x,T,x,T, T,T,T,T, T,x,x,T, x,x,x,T, T,x,T,T, T,x,x,x, x,x,x,x, x,x,T,T, x,T,T,x, x,T,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,T,T, x,x,x,x, T,x,x,x, x,x,x,T, T,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,T,x,x, x,T,x,x, x,x,x,T, x,x,x,T, T,x,x,T, x,T,x,x, x,x,x,x, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, T,x,x,T, x,x,x,x, T,x,x,x, x,x,x,T, x,x,T,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,x,x, T,T,x,T, x,x,T,T, x,x,x,T, T,T,x,T, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,T,T, T,x,x,x, x,x,x,x, x,x,x,T, x,T,T,x, x,T,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x},
		{x,x,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,T, x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,T, x,x,T,x, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, T,x,x,x, T,x,T,x, x,x,x,x, x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,T,T,x, x,T,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x}

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
			case 120: s = "\"??\" expected"; break;
			case 121: s = "\"||\" expected"; break;
			case 122: s = "\"&&\" expected"; break;
			case 123: s = "\"|\" expected"; break;
			case 124: s = "\"^\" expected"; break;
			case 125: s = "\"<=\" expected"; break;
			case 126: s = "\"/\" expected"; break;
			case 127: s = "\"%\" expected"; break;
			case 128: s = "\"->\" expected"; break;
			case 129: s = "??? expected"; break;
			case 130: s = "invalid NamespaceMemberDeclaration"; break;
			case 131: s = "invalid TypeDeclaration"; break;
			case 132: s = "invalid TypeDeclaration"; break;
			case 133: s = "invalid EnumDeclaration"; break;
			case 134: s = "invalid ClassType"; break;
			case 135: s = "invalid ClassMemberDeclaration"; break;
			case 136: s = "invalid ClassMemberDeclaration"; break;
			case 137: s = "invalid StructMemberDeclaration"; break;
			case 138: s = "invalid StructMemberDeclaration"; break;
			case 139: s = "invalid StructMemberDeclaration"; break;
			case 140: s = "invalid IntegralType"; break;
			case 141: s = "this symbol not expected in EnumBody"; break;
			case 142: s = "this symbol not expected in EnumBody"; break;
			case 143: s = "invalid Expression"; break;
			case 144: s = "invalid Expression"; break;
			case 145: s = "invalid Type"; break;
			case 146: s = "invalid EventDeclaration"; break;
			case 147: s = "invalid ConstructorDeclaration"; break;
			case 148: s = "invalid ConstructorDeclaration"; break;
			case 149: s = "invalid MethodDeclaration"; break;
			case 150: s = "invalid OperatorDeclaration"; break;
			case 151: s = "invalid CastOperatorDeclaration"; break;
			case 152: s = "invalid CastOperatorDeclaration"; break;
			case 153: s = "invalid AccessorDeclarations"; break;
			case 154: s = "invalid AccessorDeclarations"; break;
			case 155: s = "invalid OverloadableOp"; break;
			case 156: s = "invalid InterfaceMemberDeclaration"; break;
			case 157: s = "invalid InterfaceMemberDeclaration"; break;
			case 158: s = "invalid InterfaceMemberDeclaration"; break;
			case 159: s = "invalid LocalVariableDeclaration"; break;
			case 160: s = "invalid LocalVariableDeclarator"; break;
			case 161: s = "invalid VariableInitializer"; break;
			case 162: s = "invalid Attributes"; break;
			case 163: s = "invalid Keyword"; break;
			case 164: s = "invalid AttributeArguments"; break;
			case 165: s = "invalid PrimitiveType"; break;
			case 166: s = "invalid PointerOrArray"; break;
			case 167: s = "invalid NonArrayType"; break;
			case 168: s = "invalid TypeInRelExpr"; break;
			case 169: s = "invalid Statement"; break;
			case 170: s = "invalid EmbeddedStatement"; break;
			case 171: s = "invalid EmbeddedStatement"; break;
			case 172: s = "invalid StatementExpression"; break;
			case 173: s = "invalid ForEachStatement"; break;
			case 174: s = "invalid GotoStatement"; break;
			case 175: s = "invalid TryFinallyBlock"; break;
			case 176: s = "invalid UsingStatement"; break;
			case 177: s = "invalid ForInitializer"; break;
			case 178: s = "invalid CatchClauses"; break;
			case 179: s = "invalid Unary"; break;
			case 180: s = "invalid Unary"; break;
			case 181: s = "invalid AssignmentOp"; break;
			case 182: s = "invalid SwitchLabel"; break;
			case 183: s = "invalid LambdaFunctionSignature"; break;
			case 184: s = "invalid LambdaFunctionSignature"; break;
			case 185: s = "invalid LambdaFunctionBody"; break;
			case 186: s = "invalid QueryBody"; break;
			case 187: s = "invalid QueryBodyClause"; break;
			case 188: s = "invalid RelExpr"; break;
			case 189: s = "invalid RelExpr"; break;
			case 190: s = "invalid ShiftExpr"; break;
			case 191: s = "invalid Primary"; break;
			case 192: s = "invalid Primary"; break;
			case 193: s = "invalid Literal"; break;
			case 194: s = "invalid PredefinedTypeMemberAccess"; break;
			case 195: s = "invalid NewOperator"; break;
			case 196: s = "invalid NewOperatorWithType"; break;
			case 197: s = "invalid ObjectOrCollectionInitializer"; break;
			case 198: s = "invalid ElementInitializer"; break;
			case 199: s = "invalid MemberInitializer"; break;
			case 200: s = "invalid TypeParameterConstraintTag"; break;

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