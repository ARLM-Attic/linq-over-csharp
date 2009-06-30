// ================================================================================================
// CSharpSymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents symbols specific to the C# programming language.
  /// </summary>
  // ================================================================================================
  public struct CSharpSymbol : 
    IPositionedSymbol, 
    ISymbolReference,
    IEquatable<CSharpSymbol>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents an empty C# symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol Empty = new CSharpSymbol(-1, null);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbol"/> class.
    /// </summary>
    /// <param name="kind">The kind.</param>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbol(int kind, string value) : this()
    {
      Kind = kind;
      Value = value;
      Row = -1;
      Column = -1;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbol"/> struct.
    /// </summary>
    /// <param name="kind">The kind.</param>
    /// <param name="value">The value.</param>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbol(int kind, string value, int row, int column)
      : this()
    {
      Value = value;
      Kind = kind;
      Row = row;
      Column = column;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a flag indicating whether this instance is whitespace (including "end of line" symbol).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is whitespace; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsWhitespace
    {
      get 
      {
        return Kind == CSharpSymbolStream.LiteralWhiteSpace ||
               Kind == CSharpSymbolStream.EndOfLineCode ||
               (
                 Kind >= CSharpSymbolStream.FirstFrequentWhiteSpaceCode &&
                 Kind <= CSharpSymbolStream.LastFrequentWhiteSpaceCode
               );
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an "end of line" symbol.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is an "end of line" symbol; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsEndOfLine
    {
      get { return Kind == CSharpSymbolStream.EndOfLineCode; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a keyword.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is keyword; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasConstantValue
    {
      get { return Kind >= CSharpParser._abstract && Kind <= CSharpParser._larrow; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsIdentifier
    {
      get { return Kind == CSharpParser._ident; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a literal (integer, real, char, string or
    /// an identifier).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a literal; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsLiteral
    {
      get { return IsIdentifier || Kind >= CSharpParser._intCon && Kind <= CSharpParser._stringCon; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is apragma.
    /// </summary>
    /// <value><c>true</c> if this instance is pragma; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool IsPragma
    {
      get { return Kind >= CSharpParser._ppDefine && Kind <= CSharpParser._ppEndReg; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a comment.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is a comment; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsComment
    {
      get { return Kind >= CSharpParser._cBlockCom && Kind <= CSharpParser._cLineCom; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string value of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Value { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Kind { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an empty symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return Kind == -1 && Value == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new identifier symbol.
    /// </summary>
    /// <param name="value">Symbol value.</param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateIdentifier(string value)
    {
      return new CSharpSymbol(CSharpParser._ident, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new integer literal symbol.
    /// </summary>
    /// <param name="value">Symbol value.</param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateIntegerLiteral(string value)
    {
      return new CSharpSymbol(CSharpParser._intCon, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new real literal symbol.
    /// </summary>
    /// <param name="value">Symbol value.</param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateRealLiteral(string value)
    {
      return new CSharpSymbol(CSharpParser._realCon, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new character literal symbol.
    /// </summary>
    /// <param name="value">Symbol value.</param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateCharLiteral(string value)
    {
      return new CSharpSymbol(CSharpParser._charCon, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new string literal symbol.
    /// </summary>
    /// <param name="value">Symbol value.</param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateStringLiteral(string value)
    {
      return new CSharpSymbol(CSharpParser._charCon, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "#define" pragma symbol.
    /// </summary>
    /// <param name="value">Symbol value without the "#define" word.
    /// </param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateDefinePragma(string value)
    {
      return new CSharpSymbol(CSharpParser._ppDefine, "#define " + value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "#undef" pragma symbol.
    /// </summary>
    /// <param name="value">Symbol value without the "#undef" word.
    /// </param>
    /// <returns>
    /// The newly created symbol.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static CSharpSymbol CreateUndefPragma(string value)
    {
      return new CSharpSymbol(CSharpParser._ppDefine, "#undef " + value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; 
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.NullReferenceException">
    /// The <paramref name="obj"/> parameter is null.
    /// </exception>
    // ----------------------------------------------------------------------------------------------
    public override bool Equals(object obj)
    {
      if (obj is CSharpSymbol)
      {
        var otherSymbol = (CSharpSymbol)obj;
        return Kind == otherSymbol.Kind && Value == otherSymbol.Value;
      }
      return false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other"/> parameter; 
    /// otherwise, false.
    /// </returns>
    /// <param name="other">An object to compare with this object.</param>
    // ----------------------------------------------------------------------------------------------
    public bool Equals(CSharpSymbol other)
    {
      return Kind == other.Kind && Value == other.Value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return Value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures 
    /// like a hash table. 
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override int GetHashCode()
    {
      return Kind.GetHashCode() ^ Value.GetHashCode();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the row position of the symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Row { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column position of the symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Column { get; private set; }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a symbol from the specified compilation unit.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit node.</param>
    /// <returns>
    /// Symbol information obtained from the specified compilation unit.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    ISymbol ISymbolReference.GetSymbol(CompilationUnitNode compilationUnitNode)
    {
      return new CSharpSymbol(Kind, Value);
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a positioned symbol from the specified compilation unit.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit node.</param>
    /// <returns>
    /// Positioned symbol information obtained from the specified compilation unit.
    /// </returns>
    // --------------------------------------------------------------------------------------------
    IPositionedSymbol ISymbolReference.GetPositionedSymbol(CompilationUnitNode compilationUnitNode)
    {
      return new CSharpSymbol(Kind, Value, Row, Column);
    }
  }
}