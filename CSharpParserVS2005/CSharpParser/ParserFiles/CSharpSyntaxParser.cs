using System;
using CSharpParser.ProjectModel;

namespace CSharpParser.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This part of the CSharpSyntaxParser class adds extensions to the CoCo/R
  /// generated parser.
  /// </summary>
  // ==================================================================================
  public partial class CSharpSyntaxParser
  {
    #region Fields used by the parser

    /// <summary>Token ID for #if preprocessor directive.</summary>
    public const int ppIfKind = _ppIf;
    /// <summary>Token ID for #elif preprocessor directive.</summary>
    public const int ppElifKind = _ppElif;
    /// <summary>Token ID for #else preprocessor directive.</summary>
    public const int ppElseKind = _ppElse;
    /// <summary>Token ID for #endif preprocessor directive.</summary>
    public const int ppEndifKind = _ppEndif;
    /// <summary>Token ID for EOF.</summary>
    public const int EOFKind = _EOF;

    #endregion

    #region Project Model extension fields

    private CompilationUnit _CompilationUnit;
    private SourceFile _File;
    private LanguageElement _CurrentElement;
    private CommentInfo _OrphanComment;
    private readonly PragmaHandler _PragmaHandler;
    private readonly CommentHandler _CommentHandler;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this parser using the specified scanner, compilation
    /// uint and file.
    /// </summary>
    /// <param name="scanner">The scanner used to scan tokens.</param>
    /// <param name="unit">Compilation unit using this parser instance.</param>
    /// <param name="file">File used by this parser instance.</param>
    // --------------------------------------------------------------------------------
    public CSharpSyntaxParser(Scanner scanner, CompilationUnit unit, SourceFile file)
    {
      _Scanner = scanner;
      _CompilationUnit = unit;
      _File = file;
      _PragmaHandler = new PragmaHandler(this);
      _CommentHandler = new CommentHandler(this);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the instance representing the compilation unit being parsed
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit CompilationUnit
    {
      get { return _CompilationUnit; }
      set { _CompilationUnit = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the instance representing the file being parsed
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile File
    {
      get { return _File; }
      set { _File = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the languauge element that is currently processed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement CurrentElement
    {
      get { return _CurrentElement; }
      set
      {
        _CurrentElement = value;
        if (_OrphanComment != null && _OrphanComment.Token.pos < _CurrentElement.Token.pos)
        {
          // --- The orphan comment can be added to this language element.
          _CurrentElement.Comment = _OrphanComment;
          _OrphanComment = null;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the last comment that is not assigned to any language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfo OrphanComment
    {
      get { return _OrphanComment; }
      set { _OrphanComment = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the scanner used by this parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Scanner Scanner
    {
      get { return _Scanner; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the pragma handler used by this parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    internal PragmaHandler PragmaHandler
    {
      get { return _PragmaHandler; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the comment handler used by this parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    internal CommentHandler CommentHandler
    {
      get { return _CommentHandler; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the lookahead token.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Token Lookahead
    {
      get { return la; }
    }

    #endregion

    #region Resolvers

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token are: ident "="
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsAssignment()
    {
      return la.kind == _ident && Peek(1).kind == _assgn;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token is not a final comma in a list.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool NotFinalComma()
    {
      int peek = Peek(1).kind;
      return la.kind == _comma && peek != _rbrace && peek != _rbrack;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks whether the next sequence of tokens is a qualident and returns the 
    /// qualident string.
    /// </summary>
    /// <param name="pt">Peek token</param>
    /// <param name="qualident">Qualident string</param>
    /// <returns>
    /// True, if the lookahead indicates a qualident; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsQualident(ref Token pt, out string qualident)
    {
      qualident = "";
      if (pt.kind == _ident)
      {
        qualident = pt.val;
        pt = _Scanner.Peek();
        while (pt.kind == _dot)
        {
          pt = _Scanner.Peek();
          if (pt.kind != _ident) return false;
          qualident += "." + pt.val;
          pt = _Scanner.Peek();
        }
        return true;
      }
      else return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the current type represents a generic type.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsGeneric()
    {
      _Scanner.ResetPeek();
      Token pt = la;
      if (!IsTypeArgumentList(ref pt))
      {
        return false;
      }
      return typArgLstFol[pt.kind];
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a type argument list.
    /// </summary>
    /// <param name="pt">Token following the type argument list.</param>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsTypeArgumentList(ref Token pt)
    {
      if (pt.kind == _lt)
      {
        pt = _Scanner.Peek();
        while (true)
        {
          if (!IsType(ref pt))
          {
            return false;
          }
          if (pt.kind == _gt)
          {
            // list recognized
            pt = _Scanner.Peek();
            break;
          }
          else if (pt.kind == _comma)
          {
            // another argument
            pt = _Scanner.Peek();
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a type.
    /// </summary>
    /// <param name="pt">Token following the type.</param>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsType(ref Token pt)
    {
      if (typeKW[pt.kind])
      {
        pt = _Scanner.Peek();
      }
      else if (pt.kind == _void)
      {
        pt = _Scanner.Peek();
        if (pt.kind != _times)
        {
          return false;
        }
        pt = _Scanner.Peek();
      }
      else if (pt.kind == _ident)
      {
        pt = _Scanner.Peek();
        if (pt.kind == _dblcolon || pt.kind == _dot)
        {
          // either namespace alias qualifier "::" or first
          // part of the qualident
          pt = _Scanner.Peek();
          String dummyId;
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
        pt = _Scanner.Peek();
      }
      return SkipPointerOrDims(ref pt);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a local variable 
    /// declaration: Type ident.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    /// <remarks>
    /// Type can be void*
    /// </remarks>
    // --------------------------------------------------------------------------------
    bool IsLocalVarDecl()
    {
      Token pt = la;
      _Scanner.ResetPeek();
      return IsType(ref pt) && pt.kind == _ident;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent dimensions:
    /// "[" ("," | "]")
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsDims()
    {
      int peek = Peek(1).kind;
      return la.kind == _lbrack && (peek == _comma || peek == _rbrack);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent pointer or dimensions:
    /// "*" | "[" ("," | "]")
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsPointerOrDims()
    {
      return la.kind == _times || IsDims();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to skip pointer or dimension tokens.
    /// </summary>
    /// <param name="pt">The current token after skip.</param>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool SkipPointerOrDims(ref Token pt)
    {
      for (; ; )
      {
        if (pt.kind == _lbrack)
        {
          do pt = _Scanner.Peek();
          while (pt.kind == _comma);
          if (pt.kind != _rbrack) return false;
        }
        else if (pt.kind != _times) break;
        pt = _Scanner.Peek();
      }
      return true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if there is an attribute target specifier:
    /// (ident | keyword) ":"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsAttrTargSpec()
    {
      return (la.kind == _ident || keyword[la.kind]) && Peek(1).kind == _colon;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a field declaration:
    /// ident ("," | "=" | ";")
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsFieldDecl()
    {
      int peek = Peek(1).kind;
      return la.kind == _ident &&
             (peek == _comma || peek == _assgn || peek == _scolon);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a type cast.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsTypeCast()
    {
      if (la.kind != _lpar) { return false; }
      if (IsSimpleTypeCast()) { return true; }
      return GuessTypeCast();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a type cast keyword:
    /// "(" typeKW ")"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsSimpleTypeCast()
    {
      // assert: la.kind == _lpar
      _Scanner.ResetPeek();
      Token pt1 = _Scanner.Peek();
      Token pt2 = _Scanner.Peek();
      return typeKW[pt1.kind] &&
              (pt2.kind == _rpar ||
              (pt2.kind == _question && _Scanner.Peek().kind == _rpar));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a type cast by guess:
    /// "(" Type ")" castFollower
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool GuessTypeCast()
    {
      // assert: la.kind == _lpar
      _Scanner.ResetPeek();
      Token pt = _Scanner.Peek();
      if (!IsType(ref pt))
      {
        return false;
      }
      if (pt.kind != _rpar)
      {
        return false;
      }
      pt = _Scanner.Peek();
      return castFollower[pt.kind];
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent a global assembly target:
    /// "[" "assembly"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsGlobalAttrTarget()
    {
      Token pt = Peek(1);
      return la.kind == _lbrack && pt.kind == _ident && ("assembly".Equals(pt.val) || "module".Equals(pt.val));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens represent an extern directive:
    /// "extern"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsExternAliasDirective()
    {
      return la.kind == _extern;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens no switch case or right backet.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsNoSwitchLabelOrRBrace()
    {
      return (la.kind != _case && la.kind != _default && la.kind != _rbrace) ||
             (la.kind == _default && Peek(1).kind != _colon);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token defines a shift operator.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
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
      _Scanner.ResetPeek();
      Token pt = la;
      if (!IsTypeArgumentList(ref pt))
      {
        return false;
      }
      return pt.kind != _lpar;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token are: ident "="
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsNullableTypeMark()
    {
      int peek = Peek(1).kind;
      return la.kind == _question &&
             (
               peek == _and ||
               peek == _andassgn ||
               peek == _assgn ||
               peek == _colon ||
               peek == _comma ||
               peek == _divassgn ||
               peek == _dot ||
               peek == _eq ||
               peek == _gt ||
               peek == _gteq ||
               peek == _lbrack ||
               peek == _lpar ||
               peek == _lshassgn ||
               peek == _lt ||
               peek == _ltlt ||
               peek == _minusassgn ||
               peek == _modassgn ||
               peek == _neq ||
               peek == _orassgn ||
               peek == _plusassgn ||
               peek == _question ||
               peek == _rbrace ||
               peek == _rbrack ||
               peek == _rpar ||
               peek == _scolon ||
               peek == _timesassgn ||
               peek == _xorassgn
             );
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token are: ident "="
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsMemberInitializer()
    {
      return (la.kind == _ident && Peek(1).kind == _assgn);
    }


    #endregion
  }
}