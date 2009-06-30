// ================================================================================================
// CSharpParser.Resolvers.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Resolvers used by the parser
  /// </summary>
  // ================================================================================================
  public partial class CSharpParser
  {
    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token are: ident "="
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    bool IsAssignment()
    {
      return Lookahead.kind == _ident && Peek(1).kind == _assgn;
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
      return Lookahead.kind == _comma && peek != _rbrace && peek != _rbrack;
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
        pt = Scanner.Peek();
        while (pt.kind == _dot)
        {
          pt = Scanner.Peek();
          if (pt.kind != _ident) return false;
          qualident += "." + pt.val;
          pt = Scanner.Peek();
        }
        return true;
      }
      return false;
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
      Scanner.ResetPeek();
      Token pt = Lookahead;
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
        pt = Scanner.Peek();
        while (true)
        {
          if (!IsType(ref pt))
          {
            return false;
          }
          if (pt.kind == _gt)
          {
            // list recognized
            pt = Scanner.Peek();
            break;
          }
          if (pt.kind == _comma)
          {
            // another argument
            pt = Scanner.Peek();
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
        pt = Scanner.Peek();
      }
      else if (pt.kind == _void)
      {
        pt = Scanner.Peek();
        if (pt.kind != _times)
        {
          return false;
        }
        pt = Scanner.Peek();
      }
      else if (pt.kind == _ident)
      {
        pt = Scanner.Peek();
        if (pt.kind == _dblcolon || pt.kind == _dot)
        {
          // either namespace alias qualifier "::" or first
          // part of the qualident
          pt = Scanner.Peek();
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
        pt = Scanner.Peek();
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
      Token pt = Lookahead;
      Scanner.ResetPeek();
      return (IsType(ref pt) || pt.val == "var") && pt.kind == _ident;
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
      return Lookahead.kind == _lbrack && (peek == _comma || peek == _rbrack);
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
      return Lookahead.kind == _times || IsDims();
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
          do pt = Scanner.Peek();
          while (pt.kind == _comma);
          if (pt.kind != _rbrack) return false;
        }
        else if (pt.kind != _times) break;
        pt = Scanner.Peek();
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
      return (Lookahead.kind == _ident || keyword[Lookahead.kind]) && Peek(1).kind == _colon;
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
      return Lookahead.kind == _ident &&
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
      if (Lookahead.kind != _lpar) { return false; }
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
      Scanner.ResetPeek();
      Token pt1 = Scanner.Peek();
      Token pt2 = Scanner.Peek();
      return typeKW[pt1.kind] &&
              (pt2.kind == _rpar ||
              (pt2.kind == _question && Scanner.Peek().kind == _rpar));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token is "var"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsVar()
    {
      if (Lookahead.kind != _ident || Lookahead.val != "var") return false;
      var pt = Peek(1);
      return pt.val != "." && pt.val != "::";
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
      Scanner.ResetPeek();
      Token pt = Scanner.Peek();
      if (!IsType(ref pt))
      {
        return false;
      }
      if (pt.kind != _rpar)
      {
        return false;
      }
      pt = Scanner.Peek();
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
      return Lookahead.kind == _lbrack && pt.kind == _ident && ("assembly".Equals(pt.val) || "module".Equals(pt.val));
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
      return Lookahead.kind == _extern;
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
      return (Lookahead.kind != _case && Lookahead.kind != _default && Lookahead.kind != _rbrace) ||
             (Lookahead.kind == _default && Peek(1).kind != _colon);
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
      return (Lookahead.kind == _ltlt) ||
             (Lookahead.kind == _gt &&
               pt.kind == _gt &&
               (Lookahead.pos + Lookahead.val.Length == pt.pos)
             );
    }

    // true: TypeArgumentListNode followed by anything but "("
    bool IsPartOfMemberName()
    {
      Scanner.ResetPeek();
      Token pt = Lookahead;
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
      return Lookahead.kind == _question &&
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
    /// Lookahead method to check if the next tokens are: ident "="
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsMemberInitializer()
    {
      return (Lookahead.kind == _ident && Peek(1).kind == _assgn);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token is: ident "}"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsEmptyMemberInitializer()
    {
      return (Lookahead.kind == _rbrace);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token is: ident "{"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsValueInitializer()
    {
      return (Lookahead.kind != _lbrace);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens are: "partial" "void"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsPartialMethod()
    {
      return (Lookahead.val == "partial" && Peek(1).kind == _void);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens are start tokens of a lambda
    /// expression or not.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsLambda()
    {
      Scanner.ResetPeek();
      if (Lookahead.kind == _ident)
      {
        // --- ident =>
        return Scanner.Peek().kind == _larrow;
      }
      if (Lookahead.kind == _lpar)
      {
        if (IsTypeCast()) return false;
        if (IsExplicitLambdaParameter()) return true;

        Scanner.ResetPeek();
        Token pt1 = Scanner.Peek();
        Token pt2 = Scanner.Peek();
        Token pt3 = Scanner.Peek();

        // --- () =>
        if (pt1.kind == _rpar && pt2.kind == _larrow) return true;
        // --- "(out ident" || "(ref ident" 
        if ((pt1.kind == _out || pt1.kind == _ref) && pt2.kind == _ident) return true;
        // --- (ident) =>
        if (pt1.kind == _ident && pt2.kind == _rpar && pt3.kind == _larrow) return true;
        // --- ( ident "," | "::" | "." | "<"
        if (pt1.kind == _ident && pt2.kind == _comma) return true;
        return false;
      }
      return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens are start tokens of an explicit 
    /// lambda expression parameter.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsExplicitLambdaParameter()
    {
      Scanner.ResetPeek();
      Token pt1 = Scanner.Peek();
      return IsExplicitLambdaParameter(pt1);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens are start tokens of an explicit 
    /// lambda expression parameter.
    /// </summary>
    /// <param name="tok">First token to check</param>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsExplicitLambdaParameter(Token tok)
    {
      Token token = tok;
      if (token.kind == _ref || token.kind == _out) return true;
      if (IsType(ref token))
      {
        if (token.kind == _ident)
        {
          Token identFollow = Scanner.Peek();
          if (identFollow.kind == _comma || identFollow.kind == _rpar) return true;
        }
      }
      return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next tokens are start tokens of an query
    /// expression.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsQueryExpression()
    {
      Scanner.ResetPeek();
      Token pt1 = Scanner.Peek();
      Token pt2 = Scanner.Peek();

      return Lookahead.kind == _ident && Lookahead.val == "from"
             && (pt1.kind == _ident || typeKW[pt1.kind])
             && pt2.val != ";"
             && pt2.val != ","
             && pt2.val != "=";
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token are: ident "::"
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsQualifiedAliasMember()
    {
      return la.kind == _ident && Peek(1).kind == _dblcolon;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Lookahead method to check if the next token is the "where" identifier.
    /// </summary>
    /// <returns>
    /// True, if lookahed resulted with the expected result; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool IsWhere()
    {
      return la.kind == _ident && la.val == "where";
    }
  }
}