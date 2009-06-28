// ================================================================================================
// CSharpSymbolStream.cs
//
// Created: 2009.06.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the compilation unit as a stream of symbols.
  /// </summary>
  // ================================================================================================
  public class CSharpSymbolStream
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
    /// Byte array storing the symbol stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private byte[] _SymbolStream;

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Counts the consecutive EOL marks
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private int _EolCounter;

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
    /// Initializes a new instance of the <see cref="CSharpSymbolStream"/> class.
    /// </summary>
    /// <param name="capacity">The initial capacity of the token stream.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbolStream(int capacity)
    {
      _SymbolStream = new byte[capacity];
      CurrentPosition = 0;
      _EolCounter = 0;
      Frozen = false;
      _LastSymbolPosition = -1;
      _LinePositions.Add(new LinePosition { Line = 1, Position = 0 });
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSymbolStream"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbolStream()
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
    public int CurrentPosition { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="CSharpSymbolStream"/> is frozen.
    /// </summary>
    /// <value><c>true</c> if frozen; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool Frozen { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the length of the stream.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Length { get; private set; }

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
      _EolCounter = 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes an "end of line mark" to the stream
    /// </summary>
    /// <param name="line">Line number.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddEndOfLineToStream(int line)
    {
      AddSymbolToStream(line, EndOfLineWriter);
      _EolCounter++;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes whitespace symbols to the stream
    /// </summary>
    /// <param name="whitespace">Whitespace symbols.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddWhitespaceToStream(string whitespace)
    {
      if (_EolCounter == 0)
      {
        AddSymbolToStream(whitespace, WhitespaceWriter);
        return;
      }
      while (whitespace.StartsWith("\r\n") && _EolCounter > 0)
      {
        whitespace = whitespace.Substring(2);
        _EolCounter--;
      }
      if (string.IsNullOrEmpty(whitespace)) return;
      AddSymbolToStream(whitespace, WhitespaceWriter);
      _EolCounter = 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Freezes the stream.
    /// </summary>
    /// <remarks>
    /// After the stream is frozen, only seek and read operations are allowed.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public void FreezeStream()
    {
      Length = CurrentPosition + 1;
      ResizeTokenStream(Length);
      Frozen = true;
    }

    #endregion

    #region Stream writer methods

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
      if (Frozen)
      {
        throw new InvalidOperationException("Cannot write to a frozen symbol stream.");
      }

      // --- Remember the starting position
      var startPosition = CurrentPosition;

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
      if (token.IsLiteral || token.IsPragma) WriteNumber(GetLiteralIndex(token.Value));
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
      {
        WriteByte(LiteralWhiteSpace);
        WriteNumber(GetLiteralIndex(whitespace));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes a byte value to the token stream.
    /// </summary>
    /// <param name="value">The value.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteByte(byte value)
    {
      if (CurrentPosition >= _SymbolStream.Length)
      {
        ResizeTokenStream(_SymbolStream.Length + ExtentSize);
      }
      _SymbolStream[CurrentPosition++] = value;
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
      else if (number >= 0x80 && number < 0x4000)
      {
        // --- Write as a 16 bit unsigned value with binary prefix "1"
        WriteByte((byte) ((number & 0xff00 >> 8) | 0x80));
        WriteByte((byte)(number & 0xff));
      }
      else if (number >= 0x4000 && number < 0x40000000)
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

    #endregion

    #region Stream reader methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current position is the end of the stream.
    /// </summary>
    /// <value><c>true</c> if this is the end of stream; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool Eos
    {
      get { return CurrentPosition >= _SymbolStream.Length - 1; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sests the current position to the specified one.
    /// </summary>
    /// <param name="position">The position.</param>
    // ----------------------------------------------------------------------------------------------
    public void Seek(int position)
    {
      if (!Frozen)
      {
        throw new InvalidOperationException("Cannot seek in an unfrozen symbol stream.");
      }
      CheckStreamPosition(position);
      CurrentPosition = position;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the line number and retrieves the related line information.
    /// </summary>
    /// <param name="position">The position.</param>
    /// <returns>
    /// Line information (line number and stream position) of the specified line.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public LinePosition FindLineNumber(int position)
    {
      var index = FindLinePosition(0, _LinePositions.Count - 1, position);
      if (index < 0) index = 0;
      return _LinePositions[index];
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds the line position in the position array between the specified indexes.
    /// </summary>
    /// <param name="lowerIndex">Lower index.</param>
    /// <param name="upperIndex">Upper index.</param>
    /// <param name="position">The position to find.</param>
    /// <returns>Index of the line position or -1, if position not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private int FindLinePosition(int lowerIndex, int upperIndex, int position)
    {
      if (upperIndex - lowerIndex < 5)
      {
        // --- Linear search for small arrays
        for (var i = upperIndex; i >= lowerIndex; i--)
        {
          if (_LinePositions[i].Position <= position) return i;
        }
        return -1;
      }

      // --- Binary search
      var middleIndex = lowerIndex + (upperIndex - lowerIndex)/2;
      return _LinePositions[middleIndex].Position <= position 
        ? FindLinePosition(middleIndex, upperIndex, position) 
        : FindLinePosition(lowerIndex, middleIndex - 1, position);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the current position to the beginning of the line represented by the input position.
    /// </summary>
    /// <param name="position">Position within a line.</param>
    /// <returns>Start line number.</returns>
    // ----------------------------------------------------------------------------------------------
    public int SeekToLineStart(int position)
    {
      var result = FindLineNumber(position);
      Seek(result.Position);
      return result.Line;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the row and column info of the symbol at the current position.
    /// </summary>
    /// <param name="position">The position to search for.</param>
    /// <param name="row">The row position.</param>
    /// <param name="column">The column position.</param>
    // ----------------------------------------------------------------------------------------------
    public void GetRowAndColumnInfo(int position, out int row, out int column)
    {
      var oldPos = CurrentPosition;
      try
      {
        var linePos = FindLineNumber(position);
        // --- Now we have the line number.
        row = linePos.Line;

        // --- Search the column position
        column = 1;
        Seek(linePos.Position);
        while (position > CurrentPosition)
        {
          column += ReadSymbol().Value.Length;
        }
        return;
      }
      finally
      {
        CurrentPosition = oldPos;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the row and column info of the token at the specified position.
    /// </summary>
    /// <param name="row">The row position.</param>
    /// <param name="column">The column position.</param>
    // ----------------------------------------------------------------------------------------------
    public void GetRowAndColumnInfo(out int row, out int column)
    {
      GetRowAndColumnInfo(CurrentPosition, out row, out column);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reads the symbol at the current position.
    /// </summary>
    /// <returns>
    /// Null, if we are at the end of the symbol stream.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbol ReadSymbol()
    {
      if (Eos) return CSharpSymbol.Empty;
      // --- Skip the "back pointer"
      ReadNumber();
      if (Eos) return CSharpSymbol.Empty;
      // --- Read the symbol
      var symbol = new CSharpSymbol(ReadByte(), null);
      var value = string.Empty;
      if (symbol.IsWhitespace)
      {
        if (!symbol.IsEndOfLine)
        {
          // --- Handle whitespace
          value =
            symbol.Kind >= FirstFrequentWhiteSpaceCode && symbol.Kind <= LastFrequentWhiteSpaceCode
              ? value.PadRight(symbol.Kind - FirstFrequentWhiteSpaceCode + 1, ' ')
              : ReadLiteral();
        }
      }
      else if (symbol.IsLiteral || symbol.IsPragma)
      {
        // --- Handle literals and pragmas
        value = ReadLiteral();
      }
      else
      {
        value = SymbolHelper.GetSymbolName(symbol.Kind);
      }
      return new CSharpSymbol(symbol.Kind, value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reads the symbol from the specified position.
    /// </summary>
    /// <returns>
    /// Null, if we are at the end of the symbol stream.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public CSharpSymbol ReadSymbol(int position)
    {
      Seek(position);
      return ReadSymbol();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Moves back to the previous symbol.
    /// </summary>
    /// <returns>
    /// True, if movement was successful; false, if we are at the beginning of the token stream.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public bool MoveBack()
    {
      if (CurrentPosition <= 0) return false;
      var startPosition = CurrentPosition;
      var backJump = ReadNumber();
      Seek(startPosition - backJump);
      return true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reads a byte from the current position and advances the stream with one position.
    /// </summary>
    /// <returns>Byte read from the stream.</returns>
    // ----------------------------------------------------------------------------------------------
    private byte ReadByte()
    {
      CheckStreamPosition(CurrentPosition);
      return _SymbolStream[CurrentPosition++];
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reads a number from the stream. Uses the prefixes to recognize the number of bytes used to
    /// store the number in the stream and advances the position accordingly.
    /// </summary>
    /// <returns>Byte read from the stream.</returns>
    // ----------------------------------------------------------------------------------------------
    private int ReadNumber()
    {
      var startByte = ReadByte();
      switch (startByte >> 6)
      {
        case 0:
        case 1:
          // --- Single byte
          return startByte;
        case 2:
          // --- Two consecutive bytes
          return (startByte & 0x3f) << 8 + ReadByte();
        default:
          // -- Four consecutive bytes
          return (startByte & 0x3f) << 24 + ReadByte() << 16 + ReadByte() << 8 + ReadByte();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Reads a literal from the stream and advances the position accordingly.
    /// </summary>
    /// <returns>Byte read from the stream.</returns>
    // ----------------------------------------------------------------------------------------------
    public string ReadLiteral()
    {
      return ResolveLiteralByIndex(ReadNumber());
    }

    #endregion

    #region Helper methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resizes the token stream to the specified new size.
    /// </summary>
    /// <param name="newSize">The new size.</param>
    // ----------------------------------------------------------------------------------------------
    private void ResizeTokenStream(int newSize)
    {
      if (newSize < CurrentPosition)
      {
        throw new InvalidOperationException("Shrinking symbol stream size under its length.");
      }
      if (newSize <= _SymbolStream.Length) return;
      var newStream = new byte[newSize];
      _SymbolStream = newStream;
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the stream position is valid. Throws an exception if not.
    /// </summary>
    /// <param name="position">The position.</param>
    // ----------------------------------------------------------------------------------------------
    private void CheckStreamPosition(int position)
    {
      if (position < 0 || position >= _SymbolStream.Length)
      {
        throw new InvalidOperationException("Seek position is out of the symbol stream.");
      }
    }

    #endregion

    #region Diagnostic methods

    public override string ToString()
    {
      var sb = new StringBuilder();
      return sb.ToString();
    }

    #endregion
  }
}