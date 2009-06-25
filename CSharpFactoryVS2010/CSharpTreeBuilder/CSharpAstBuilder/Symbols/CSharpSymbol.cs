// ================================================================================================
// CSharpSymbol.cs
//
// Created: 2009.06.20, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents symbols specific to the C# programming language.
  /// </summary>
  // ================================================================================================
  public class CSharpSymbol : Symbol
  {
    #region Private fields

    private readonly int _Kind;
    private readonly string _Value;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbol"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbol()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbol"/> class.
    /// </summary>
    /// <param name="kind">The kind.</param>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    protected CSharpSymbol(int kind, string value)
    {
      _Kind = kind;
      _Value = value;
    }

    #endregion

    #region ISymbol implementation

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a flag indicating whether this instance is whitespace (including "end of line" symbol).
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is whitespace; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public override bool IsWhitespace
    {
      get 
      {
        return Kind == SymbolStream.LiteralWhiteSpace ||
               Kind == SymbolStream.EndOfLineCode ||
               (
                 Kind >= SymbolStream.FirstFrequentWhiteSpaceCode &&
                 Kind <= SymbolStream.LastFrequentWhiteSpaceCode
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
    public override bool IsEndOfLine
    {
      get { return Kind == SymbolStream.EndOfLineCode; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is a keyword.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is keyword; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public override bool HasConstantValue
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
    public override bool IsIdentifier
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
    public override bool IsLiteral
    {
      get { return Kind >= CSharpParser._intCon && Kind <= CSharpParser._stringCon; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is apragma.
    /// </summary>
    /// <value><c>true</c> if this instance is pragma; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public override bool IsPragma
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
    public override bool IsComment
    {
      get { return Kind >= CSharpParser._cBlockCom && Kind <= CSharpParser._cLineCom; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string value of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string Value { get { return _Value; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of a symbol.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override int Kind { get { return _Kind; } }

    #endregion

    #region Factory methods

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

    #endregion
  }
}