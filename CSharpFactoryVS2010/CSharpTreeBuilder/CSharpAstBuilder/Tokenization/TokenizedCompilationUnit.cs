// ================================================================================================
// TokenizedCompilationUnit.cs
//
// Created: 2009.06.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the tokenized form of a compilation unit (source file)
  /// </summary>
  // ================================================================================================
  public class TokenizedCompilationUnit
  {
    #region Constant values

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents the default capacity of the token stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const int DefaultCapacity = 1024;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The extent size of the token stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const int ExtentSize = 1024;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The token code representing that a white space is literally written to the stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const byte LiteralWhiteSpace = 220;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The token code representing the first frequent whitespace 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const byte FirstFrequentWhiteSpaceCode = 221;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The token code representing the last frequent whitespace 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const byte LastFrequentWhiteSpaceCode = 236;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The token code representing the end of line 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public const byte EndOfLineCode = 237;

    #endregion

    #region Private fields

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Byte array storing the token stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private byte[] _TokenStream;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Current position in the token stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private int _CurrentPosition;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The starting position of the last token that is written to the stream. -1 means no token
    /// has been written to the stream yet.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private int _LastSymbolPosition;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Holds a list of literals. This array can be used to look up the literal by its index.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly List<string> _LiteralsByIndex = new List<string>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Holds a list of literals. This array can be used to look up the literal index by its value.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly Dictionary<string, int> _LiteralsByValue = new Dictionary<string, int>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Keeps track of line positions within token stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly List<LinePosition> _LinePositions = new List<LinePosition>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Byte codes for most frequently used whitespaces
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private readonly Dictionary<string, byte> _WhiteSpaceByteCodes =
      new Dictionary<string, byte>
        {
          { " ", 221},
          { "  ", 222 },
          { "   ", 223 },
          { "    ", 224 },
          { "     ", 225 },
          { "      ", 226 },
          { "       ", 227 },
          { "        ", 228 },
          { "         ", 229 },
          { "          ", 230 },
          { "           ", 231 },
          { "            ", 232 },
          { "             ", 233 },
          { "              ", 234 },
          { "               ", 235 },
          { "                ", 236 },
        };
    #endregion

    #region Nested types

    // ================================================================================================
    /// <summary>
    /// This structure is used to keep the starting position of a line in the token buffer.
    /// </summary>
    // ================================================================================================
    public struct LinePosition
    {
      /// <summary>Line number</summary>
      public int Line;

      /// <summary>Line position in the stream</summary>
      public int Position;
    }

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenizedCompilationUnit"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity of the token stream.</param>
    // ----------------------------------------------------------------------------------------------
    public TokenizedCompilationUnit(int capacity)
    {
      _TokenStream = new byte[capacity];
      _CurrentPosition = 0;
      _LastSymbolPosition = -1;
      _LinePositions.Add(new LinePosition { Line = 1, Position = 0 });
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenizedCompilationUnit"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TokenizedCompilationUnit()
      : this(DefaultCapacity)
    {
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current position of the token stream.
    /// </summary>
    /// <value>The current position.</value>
    // ----------------------------------------------------------------------------------------------
    public int CurrentPosition
    {
      get { return _CurrentPosition; }
    }

    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified token to the stream.
    /// </summary>
    /// <param name="token">The token to write.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTokenToStream(Token token)
    {
      AddSymbolToStream(token, TokenWriter);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes an "end of line mark" to the stream
    /// </summary>
    /// <param name="line">Line number.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddEndOfLineStream(int line)
    {
      AddSymbolToStream(line, EndOfLineWriter);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes whitespace symbols to the stream
    /// </summary>
    /// <param name="whitespace">Whitespace symbols.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddWhitespaceToStream(string whitespace)
    {
      AddSymbolToStream(whitespace, WhitespaceWriter);
    }

    #endregion

    #region Helper methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a symbol to stream.
    /// </summary>
    /// <typeparam name="T">Type of the symbol to write to the stream</typeparam>
    /// <param name="symbol">The symbol to write to the stream.</param>
    /// <param name="symbolWriter">Action writing the symbol body.</param>
    // ----------------------------------------------------------------------------------------------
    private void AddSymbolToStream<T>(T symbol, Action<T, int> symbolWriter)
    {
      // --- Remember the starting position
      var startPosition = _CurrentPosition;

      // --- Write out the distance from the previous symbols's start position. Use zero in case
      // --- of the first token.
      WriteNumber(_LastSymbolPosition == -1 ? 0 : startPosition - _LastSymbolPosition);

      // --- Write the symbol body to the stream
      symbolWriter(symbol, startPosition);

      // --- Store the start position of this symbol
      _LastSymbolPosition = startPosition;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified token to the stream.
    /// </summary>
    /// <param name="token">The token to write.</param>
    /// <param name="startPosition">The start position of this token.</param>
    // ----------------------------------------------------------------------------------------------
    private void TokenWriter(Token token, int startPosition)
    {
      // --- Add the token type (as a single byte) to the stream
      WriteByte((byte) token.Kind);
      // --- If token is a literal value, write out the related literal's index
      if (token.IsLiteral) WriteNumber(GetLiteralIndex(token.Value));
      // --- If token is a pragma, write out the pragma text
      else if (token.IsPragma) WriteText(token.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an "end of line" byte code to the stream.
    /// </summary>
    /// <param name="line">Line number.</param>
    /// <param name="startPosition">The start position of this "end of line" mark.</param>
    // ----------------------------------------------------------------------------------------------
    private void EndOfLineWriter(int line, int startPosition)
    {
      // --- The end of the first line is not written to the stream.
      if (line <= 1) return;
      _LinePositions.Add(new LinePosition { Line = line, Position = startPosition });
      WriteByte(EndOfLineCode);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified whitespace to stream.
    /// </summary>
    /// <param name="whitespace">The whitespace string.</param>
    /// <param name="startPosition">The start position of this whitespace.</param>
    // ----------------------------------------------------------------------------------------------
    private void WhitespaceWriter(string whitespace, int startPosition)
    {
      byte wsCode;
      if (_WhiteSpaceByteCodes.TryGetValue(whitespace, out wsCode))
        WriteByte(wsCode);
      else
        WriteNumber(GetLiteralIndex(whitespace));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resizes the token stream to the specified new size.
    /// </summary>
    /// <param name="newSize">The new size.</param>
    // ----------------------------------------------------------------------------------------------
    private void ResizeTokenStream(int newSize)
    {
      if (newSize < _CurrentPosition)
      {
        throw new InvalidOperationException("Shrinking token buffer size under its length.");
      }
      var newStream = new byte[newSize];
      _TokenStream.CopyTo(newStream, 0);
      _TokenStream = newStream;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes a byte value to the token stream.
    /// </summary>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteByte(byte value)
    {
      if (_CurrentPosition >= _TokenStream.Length)
      {
        ResizeTokenStream(_TokenStream.Length + ExtentSize);
      }
      _TokenStream[_CurrentPosition++] = value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes a 32-bit number with prefixed compression to the stream.
    /// </summary>
    /// <param name="number">The number to be written to the stream.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteNumber(int number)
    {
      if (number >= 0 && number < 0x80)
      {
        // --- Write as a single byte with no prefix
        WriteByte((byte)number);
      }
      else if (number >= 0x80 && number < 0x8000)
      {
        // --- Write as a 16 bit unsigned value with binary prefix "1"
        WriteByte((byte) ((number & 0xff00 >> 8) | 0x80));
        WriteByte((byte)(number & 0xff));
      }
      else if (number >= 0x8000 && number < 0x40000000)
      {
        // --- Write as a 32 bit unsigned value with binary prefix "11"
        WriteByte((byte)((number & 0xff00000 >> 24) | 0xc0));
        WriteByte((byte)(number & 0xff0000 >> 16));
        WriteByte((byte)(number & 0xff00 >> 8));
        WriteByte((byte)(number & 0xff));
      }
      else
      {
        throw new InvalidOperationException("Number to write to the token stream is out of range.");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes out the text to the token stream.
    /// </summary>
    /// <param name="text">The text.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteText(string text)
    {
      if (text == null) throw new ArgumentNullException("text");
      WriteNumber(text.Length);
      foreach (var c in text)
      {
        WriteNumber(c);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the index of the literal.
    /// </summary>
    /// <param name="literal">The literal.</param>
    /// <returns>The index of the literal.</returns>
    // ----------------------------------------------------------------------------------------------
    private int GetLiteralIndex(string literal)
    {
      int index;
      if (!_LiteralsByValue.TryGetValue(literal, out index))
      {
        index = _LiteralsByIndex.Count;
        _LiteralsByIndex.Add(literal);
        _LiteralsByValue.Add(literal, index);
      }
      return index;
    }

    public void Seek(int position)
    {
      throw new NotImplementedException();  
    }

    public LinePosition FindLineNumber(int position)
    {
      throw new NotImplementedException();
    }

    public int SeekToLineStart(int position)
    {
      var result = FindLineNumber(position);
      Seek(result.Position);
      return result.Line;
    }

    public byte ReadByte()
    {
      throw new NotImplementedException();
    }

    public int ReadNumber()
    {
      throw new NotImplementedException();
    }

    public string ReadString()
    {
      throw new NotImplementedException();
    }

    public string ReadLiteral()
    {
      return ResolveLiteralByIndex(ReadNumber());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a literal by its index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>
    /// The value of the literal.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    private string ResolveLiteralByIndex(int index)
    {
      if (index < 0 || index >= _LiteralsByIndex.Count)
      {
        throw new InvalidOperationException("Literal index is out of the expected range.");
      }
      return _LiteralsByIndex[index];
    }

    #endregion
  }
}