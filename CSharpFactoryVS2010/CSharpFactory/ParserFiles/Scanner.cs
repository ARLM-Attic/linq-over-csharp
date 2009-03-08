
using System;
using System.IO;
using System.Collections.Generic;

namespace CSharpFactory.ParserFiles {

  // ==================================================================================
  /// <summary>
  /// This class defines a Token used by the scanner and the parser.
  /// </summary>
  /// <remarks>
  /// This class defines public fields that are against the convention not using public
  /// fields. Because wrapping this fields into properties would affect the performance
  /// we live together to violating the convention.
  /// </remarks>
  // ==================================================================================
  public class Token
  {
    /// <summary>Kind of token.</summary>
    public int kind;

    /// <summary>Token position in the source text (starting at 0).</summary>
    public int pos;

    /// <summary>Token column (starting at 0).</summary>
    public int col;

    /// <summary>Token line (starting at 1).</summary>
    public int line;

    /// <summary>Token value.</summary>
    public string val;

    /// <summary>Points to the next token.</summary>
    private Token _next;

    /// <summary>Points to the previous token.</summary>
    private Token _prev;

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Token next
    {
      get { return _next; }
      set
      {
        _next = value;
        if (value != null) _next._prev = this;
      }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the previous token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Token prev
    {
      get { return _prev; }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a memory buffer that keeps a part of the file under
  /// scanning and parsing in the memory.
  /// </summary>
  // ==================================================================================
  public class Buffer
  {
    #region Constant values

    /// <summary>This constant represents the end of file token.</summary>
    public const int EOF = char.MaxValue + 1;
    private const int MaxBufferLength = 256 * 1024; // 256KB

    #endregion

    #region Private fields

    private readonly byte[] _Buffer;          // input buffer
    private int _StartPosition;                 // position of first byte in buffer relative to input stream
    private int _BufferLength;                   // length of buffer
    private readonly int _FileLength;         // length of input stream
    private int _Position;                      // current position in buffer
    private Stream _Stream;                // input stream (seekable)
    private readonly bool _IsUserStream;   // was the stream opened by the user?

    #endregion

    #region Lifecycle methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new buffer instance.
    /// </summary>
    /// <param name="s">Stream of the buffer.</param>
    /// <param name="isUserStream">
    /// Flag indicating if this is a user stream (lifecyle of stream is managed by 
    /// the user).
    /// </param>
    //-----------------------------------------------------------------------------------
    public Buffer(Stream s, bool isUserStream)
    {
      _Stream = s; 
      _IsUserStream = isUserStream;
      _FileLength = _BufferLength = (int)s.Length;
      if (_Stream.CanSeek && _BufferLength > MaxBufferLength) _BufferLength = MaxBufferLength;
      _Buffer = new byte[_BufferLength];
      _StartPosition = Int32.MaxValue; // nothing in the buffer so far
      Pos = 0; // setup buffer to position 0 (start)
      if (_BufferLength == _FileLength) Close();
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance based on an existing Buffer instance.
    /// </summary>
    /// <param name="b">Base buffer.</param>
    //-----------------------------------------------------------------------------------
    protected Buffer(Buffer b)
    {
      _Buffer = b._Buffer;
      _StartPosition = b._StartPosition;
      _BufferLength = b._BufferLength;
      _FileLength = b._FileLength;
      _Position = b._Position;
      _Stream = b._Stream;
      b._Stream = null;
      _IsUserStream = b._IsUserStream;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Closes the buffer on finalization.
    /// </summary>
    //-----------------------------------------------------------------------------------
    ~Buffer() { Close(); }

    #endregion

    #region Public methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Closes the stream.
    /// </summary>
    //-----------------------------------------------------------------------------------
    protected void Close()
    {
      if (!_IsUserStream && _Stream != null)
      {
        _Stream.Close();
        _Stream = null;
      }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Reads the next value from the buffer and returns it as an integer. Moves the
    /// position forward.
    /// </summary>
    /// <returns>
    /// Next value in the buffer. If the file end is reached, an EOF value is returned.
    /// </returns>
    /// <remarks>
    /// If the end of the buffer reached, the next chunk of the file is read into the 
    /// buffer. If the file end is reached, an EOF value is returned.
    /// </remarks>
    //-----------------------------------------------------------------------------------
    public virtual int Read()
    {
      if (_Position < _BufferLength)
      {
        return _Buffer[_Position++];
      }
      if (Pos < _FileLength)
      {
        Pos = Pos; // shift buffer start to Pos
        return _Buffer[_Position++];
      }
      return EOF;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Peeks the next value in the buffer and returns it as an integer, does not move
    /// the current position.
    /// </summary>
    /// <returns>
    /// Next value in the buffer. If the file end is reached, an EOF value is returned.
    /// </returns>
    //-----------------------------------------------------------------------------------
    public int Peek()
    {
      int curPos = Pos;
      int ch = Read();
      Pos = curPos;
      return ch;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets a string from the specified current buffer position range.
    /// </summary>
    /// <param name="beg">Beginning position of the string.</param>
    /// <param name="end">Next position after the end of the string.</param>
    /// <returns></returns>
    //-----------------------------------------------------------------------------------
    public string GetString(int beg, int end)
    {
      var len = end - beg;
      var buf = new char[len];
      var oldPos = Pos;
      Pos = beg;
      for (var i = 0; i < len; i++) buf[i] = (char)Read();
      Pos = oldPos;
      return new String(buf);
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the position of the buffer.
    /// </summary>
    /// <remarks>
    /// If the position is set to a file position that is not in the buffer, 
    /// automatically loads in the corresponding chunk of the file.
    /// </remarks>
    //-----------------------------------------------------------------------------------
    public int Pos
    {
      get { return _Position + _StartPosition; }
      set
      {
        if (value < 0) value = 0;
        else if (value > _FileLength) value = _FileLength;
        if (value >= _StartPosition && value < _StartPosition + _BufferLength)
        { // already in buffer
          _Position = value - _StartPosition;
        }
        else if (_Stream != null)
        { // must be swapped in
          _Stream.Seek(value, SeekOrigin.Begin);
          _BufferLength = _Stream.Read(_Buffer, 0, _Buffer.Length);
          _StartPosition = value; _Position = 0;
        }
        else
        {
          _Position = _FileLength - _StartPosition; // make Pos return _FileLength
        }
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a memory buffer that uses UTF8 encoded files.
  /// </summary>
  // ==================================================================================
  public class UTF8Buffer : Buffer
  {
    #region Lifecycle methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UTF8Buffer instance based on a Buffer instance.
    /// </summary>
    /// <param name="b">Base Buffer instance</param>
    //-----------------------------------------------------------------------------------
    public UTF8Buffer(Buffer b) : base(b) { }

    #endregion

    #region Overridden methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Reads the next value from the UTF8Buffer.
    /// </summary>
    /// <returns>Value read.</returns>
    //-----------------------------------------------------------------------------------
    public override int Read()
    {
      int ch;
      do
      {
        ch = base.Read();
        // until we find a uft8 start (0xxxxxxx or 11xxxxxx)
      } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
      if (ch < 128 || ch == EOF)
      {
        // nothing to do, first 127 chars are the same in ascii and utf8
        // 0xxxxxxx or end of file character
      }
      else if ((ch & 0xF0) == 0xF0)
      {
        // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
        int c1 = ch & 0x07; ch = base.Read();
        int c2 = ch & 0x3F; ch = base.Read();
        int c3 = ch & 0x3F; ch = base.Read();
        int c4 = ch & 0x3F;
        ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
      }
      else if ((ch & 0xE0) == 0xE0)
      {
        // 1110xxxx 10xxxxxx 10xxxxxx
        int c1 = ch & 0x0F; ch = base.Read();
        int c2 = ch & 0x3F; ch = base.Read();
        int c3 = ch & 0x3F;
        ch = (((c1 << 6) | c2) << 6) | c3;
      }
      else if ((ch & 0xC0) == 0xC0)
      {
        // 110xxxxx 10xxxxxx
        int c1 = ch & 0x1F; ch = base.Read();
        int c2 = ch & 0x3F;
        ch = (c1 << 6) | c2;
      }
      return ch;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents the scanner.
  /// </summary>
  // ==================================================================================
  public class Scanner
  {
    #region Constant declarations

    const char EOL = '\n';
    const int eofSym = 0; /* pdt */

    #endregion

    #region Constant declarations generated by CoCo

    const int maxT = 145;
    const int noSym = 145;

    #endregion

    #region Private fields

    Buffer buffer;    // scanner buffer
    Token t;          // current token
    int ch;           // current input character
    int pos;          // byte position of current character
    int col;          // column number of current character
    int line;         // line number of current character
    int oldEols;      // EOLs that appeared in a comment;
    static readonly Dictionary<int, int> start; // maps first token character to start state
    Token tokens;     // list of tokens already peeked (first token is a dummy)
    Token pt;         // current peek token
    char[] tval = new char[128]; // text of current token
    int tlen;         // length of current token

    /// <summary>Signs that scanner should skip over block comment start.</summary>
    private bool _SkipMode;

    /// <summary>Indicates that an EOF found.</summary>
    private bool _EOFFound;

    #endregion

	static Scanner() {
		start = new Dictionary<int, int>(128);
    for (int i = 65; i <= 90; ++i) start[i] = 1;
    for (int i = 95; i <= 95; ++i) start[i] = 1;
    for (int i = 97; i <= 122; ++i) start[i] = 1;
    for (int i = 170; i <= 170; ++i) start[i] = 1;
    for (int i = 181; i <= 181; ++i) start[i] = 1;
    for (int i = 186; i <= 186; ++i) start[i] = 1;
    for (int i = 192; i <= 214; ++i) start[i] = 1;
    for (int i = 216; i <= 246; ++i) start[i] = 1;
    for (int i = 248; i <= 255; ++i) start[i] = 1;
    for (int i = 49; i <= 57; ++i) start[i] = 171;
    start[92] = 15; 
    start[64] = 172; 
    start[48] = 173; 
    start[46] = 174; 
    start[39] = 44; 
    start[34] = 61; 
    start[38] = 209; 
    start[61] = 175; 
    start[58] = 176; 
    start[44] = 79; 
    start[45] = 210; 
    start[47] = 211; 
    start[62] = 177; 
    start[43] = 178; 
    start[123] = 86; 
    start[91] = 87; 
    start[40] = 88; 
    start[60] = 212; 
    start[37] = 213; 
    start[33] = 179; 
    start[124] = 214; 
    start[63] = 215; 
    start[125] = 95; 
    start[93] = 96; 
    start[41] = 97; 
    start[59] = 98; 
    start[126] = 99; 
    start[42] = 180; 
    start[94] = 216; 
    start[35] = 181; 
    start[Buffer.EOF] = -1;

	}
	
    #region Lifecycle methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a scanner instance based on the specified file.
    /// </summary>
    /// <param name="fileName">File to be scanned.</param>
    //-----------------------------------------------------------------------------------
    public Scanner(string fileName)
    {
      try
      {
        Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        buffer = new Buffer(stream, false);
        Init();
      }
      catch (IOException)
      {
        throw new FatalError("Cannot open file " + fileName);
      }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a scanner instance based on the specified stream.
    /// </summary>
    /// <param name="s">Stream to scan.</param>
    //-----------------------------------------------------------------------------------
    public Scanner(Stream s)
    {
      buffer = new Buffer(s, true);
      Init();
    }

    #endregion

    #region Public properties

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the current skipmode
    /// </summary>
    //-----------------------------------------------------------------------------------
    public bool SkipMode
    {
      get { return _SkipMode; }
      set { _SkipMode = value; }
    } 
    
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current character
    /// </summary>
    //-----------------------------------------------------------------------------------
    public int CurrentChar
    {
      get { return ch; }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the buffer object
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Buffer Buffer
    {
      get { return buffer; }
    }

    #endregion

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the scanner.
    /// </summary>
    //-----------------------------------------------------------------------------------
    void Init()
    {
      _SkipMode = false;
      _EOFFound = false;

      pos = -1; line = 1; col = 0;
      oldEols = 0;
      NextCh();
      if (ch == 0xEF)
      { // check optional byte order mark for UTF-8
        NextCh(); int ch1 = ch;
        NextCh(); int ch2 = ch;
        if (ch1 != 0xBB || ch2 != 0xBF)
        {
          throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
        }
        buffer = new UTF8Buffer(buffer); col = 0;
        NextCh();
      }
  		pt = tokens = new Token();  // first token is a dummy
	  }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next character from the stream.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public void NextCh()
    {
      if (oldEols > 0) { ch = EOL; oldEols--; }
      else
      {
        pos = buffer.Pos;
        ch = buffer.Read(); col++;
        if (!_EOFFound && ch == Buffer.EOF)
        {
          _EOFFound = true;
          ch = EOL;
        }
        // replace isolated '\r' by '\n' in order to make
        // eol handling uniform across Windows, Unix and Mac
        if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
        if (ch == EOL)
        {
          line++; 
          col = 0;
        }
      }

  	}

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a character to the currently scanned token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    void AddCh()
    {
      if (tlen >= tval.Length)
      {
        var newBuf = new char[2 * tval.Length];
        Array.Copy(tval, 0, newBuf, 0, tval.Length);
        tval = newBuf;
      }
      tval[tlen++] = (char) ch;
  		NextCh();
	  }



    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a character to the currently scanned token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    void CheckLiteral()
    {
      switch (t.val)
      {
        case "abstract": t.kind = 6; break;
        case "as": t.kind = 7; break;
        case "base": t.kind = 8; break;
        case "bool": t.kind = 9; break;
        case "break": t.kind = 10; break;
        case "byte": t.kind = 11; break;
        case "case": t.kind = 12; break;
        case "catch": t.kind = 13; break;
        case "char": t.kind = 14; break;
        case "checked": t.kind = 15; break;
        case "class": t.kind = 16; break;
        case "const": t.kind = 17; break;
        case "continue": t.kind = 18; break;
        case "decimal": t.kind = 19; break;
        case "default": t.kind = 20; break;
        case "delegate": t.kind = 21; break;
        case "do": t.kind = 22; break;
        case "double": t.kind = 23; break;
        case "else": t.kind = 24; break;
        case "enum": t.kind = 25; break;
        case "event": t.kind = 26; break;
        case "explicit": t.kind = 27; break;
        case "extern": t.kind = 28; break;
        case "false": t.kind = 29; break;
        case "finally": t.kind = 30; break;
        case "fixed": t.kind = 31; break;
        case "float": t.kind = 32; break;
        case "for": t.kind = 33; break;
        case "foreach": t.kind = 34; break;
        case "goto": t.kind = 35; break;
        case "if": t.kind = 36; break;
        case "implicit": t.kind = 37; break;
        case "in": t.kind = 38; break;
        case "int": t.kind = 39; break;
        case "interface": t.kind = 40; break;
        case "internal": t.kind = 41; break;
        case "is": t.kind = 42; break;
        case "lock": t.kind = 43; break;
        case "long": t.kind = 44; break;
        case "namespace": t.kind = 45; break;
        case "new": t.kind = 46; break;
        case "null": t.kind = 47; break;
        case "object": t.kind = 48; break;
        case "operator": t.kind = 49; break;
        case "out": t.kind = 50; break;
        case "override": t.kind = 51; break;
        case "params": t.kind = 52; break;
        case "private": t.kind = 53; break;
        case "protected": t.kind = 54; break;
        case "public": t.kind = 55; break;
        case "readonly": t.kind = 56; break;
        case "ref": t.kind = 57; break;
        case "return": t.kind = 58; break;
        case "sbyte": t.kind = 59; break;
        case "sealed": t.kind = 60; break;
        case "short": t.kind = 61; break;
        case "sizeof": t.kind = 62; break;
        case "stackalloc": t.kind = 63; break;
        case "static": t.kind = 64; break;
        case "string": t.kind = 65; break;
        case "struct": t.kind = 66; break;
        case "switch": t.kind = 67; break;
        case "this": t.kind = 68; break;
        case "throw": t.kind = 69; break;
        case "true": t.kind = 70; break;
        case "try": t.kind = 71; break;
        case "typeof": t.kind = 72; break;
        case "uint": t.kind = 73; break;
        case "ulong": t.kind = 74; break;
        case "unchecked": t.kind = 75; break;
        case "unsafe": t.kind = 76; break;
        case "ushort": t.kind = 77; break;
        case "using": t.kind = 78; break;
        case "var": t.kind = 79; break;
        case "virtual": t.kind = 80; break;
        case "void": t.kind = 81; break;
        case "volatile": t.kind = 82; break;
        case "while": t.kind = 83; break;
        case "partial": t.kind = 121; break;
        case "yield": t.kind = 122; break;
        case "from": t.kind = 123; break;
        case "let": t.kind = 124; break;
        case "where": t.kind = 125; break;
        case "join": t.kind = 126; break;
        case "on": t.kind = 127; break;
        case "equals": t.kind = 128; break;
        case "into": t.kind = 129; break;
        case "orderby": t.kind = 130; break;
        case "ascending": t.kind = 131; break;
        case "descending": t.kind = 132; break;
        case "select": t.kind = 133; break;
        case "group": t.kind = 134; break;
        case "by": t.kind = 135; break;
        default: break;
      }
  	}

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Adds a character to the currently scanned token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    Token NextToken()
    {
		  while (ch == ' ' ||
      ch >= 9 && ch <= 10 || ch == 13
		  ) NextCh();

      var apx = 0;
      // Handle skip mode
      if (ch != Buffer.EOF && _SkipMode)
      {
        // Skip "/*" block comment begin token
        if (ch == '/') NextCh();
        if (ch != Buffer.EOF && ch == '*') NextCh();
      }
      t = new Token {pos = pos, col = col, line = line};
      int state;
      try { state = start[ch]; }
      catch (KeyNotFoundException) { state = 0; }
      tlen = 0; AddCh();

      switch (state)
      {
        case -1: { t.kind = eofSym; break; } // NextCh already done
        case 0: { t.kind = noSym; break; }   // NextCh already done
        case 1:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 160 || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255)
          { AddCh(); goto case 1; }
          if (ch == 92)
          { AddCh(); goto case 2; }
          t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;
        case 2:
          if (ch == 'u')
          { AddCh(); goto case 3; }
          if (ch == 'U')
          { AddCh(); goto case 7; }
          t.kind = noSym; break;
        case 3:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 4; }
          t.kind = noSym; break;
        case 4:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 5; }
          t.kind = noSym; break;
        case 5:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 6; }
          t.kind = noSym; break;
        case 6:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 1; }
          t.kind = noSym; break;
        case 7:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 8; }
          t.kind = noSym; break;
        case 8:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 9; }
          t.kind = noSym; break;
        case 9:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 10; }
          t.kind = noSym; break;
        case 10:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 11; }
          t.kind = noSym; break;
        case 11:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 12; }
          t.kind = noSym; break;
        case 12:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 13; }
          t.kind = noSym; break;
        case 13:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 14; }
          t.kind = noSym; break;
        case 14:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 1; }
          t.kind = noSym; break;
        case 15:
          if (ch == 'u')
          { AddCh(); goto case 16; }
          if (ch == 'U')
          { AddCh(); goto case 20; }
          t.kind = noSym; break;
        case 16:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 17; }
          t.kind = noSym; break;
        case 17:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 18; }
          t.kind = noSym; break;
        case 18:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 19; }
          t.kind = noSym; break;
        case 19:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 1; }
          t.kind = noSym; break;
        case 20:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 21; }
          t.kind = noSym; break;
        case 21:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 22; }
          t.kind = noSym; break;
        case 22:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 23; }
          t.kind = noSym; break;
        case 23:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 24; }
          t.kind = noSym; break;
        case 24:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 25; }
          t.kind = noSym; break;
        case 25:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 26; }
          t.kind = noSym; break;
        case 26:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 27; }
          t.kind = noSym; break;
        case 27:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 1; }
          t.kind = noSym; break;
        case 28:
          tlen -= apx;
          buffer.Pos = t.pos;
          NextCh();
          line = t.line;
          col = t.col;
          for (var i = 0; i < tlen; i++) NextCh();
          t.kind = 2; break;
        case 29:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 30; }
          t.kind = noSym; break;
        case 30:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 30; }
          if (ch == 'U')
          { AddCh(); goto case 186; }
          if (ch == 'u')
          { AddCh(); goto case 187; }
          if (ch == 'L')
          { AddCh(); goto case 188; }
          if (ch == 'l')
          { AddCh(); goto case 189; }
          t.kind = 2; break;
        case 31:
          t.kind = 2; break;
        case 32:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 32; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          if (ch == 'E' || ch == 'e')
          { AddCh(); goto case 33; }
          t.kind = 3; break;
        case 33:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 35; }
          if (ch == '+' || ch == '-')
          { AddCh(); goto case 34; }
          t.kind = noSym; break;
        case 34:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 35; }
          t.kind = noSym; break;
        case 35:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 35; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          t.kind = 3; break;
        case 36:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 36; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          if (ch == 'E' || ch == 'e')
          { AddCh(); goto case 37; }
          t.kind = 3; break;
        case 37:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 39; }
          if (ch == '+' || ch == '-')
          { AddCh(); goto case 38; }
          t.kind = noSym; break;
        case 38:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 39; }
          t.kind = noSym; break;
        case 39:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 39; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          t.kind = 3; break;
        case 40:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 42; }
          if (ch == '+' || ch == '-')
          { AddCh(); goto case 41; }
          t.kind = noSym; break;
        case 41:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 42; }
          t.kind = noSym; break;
        case 42:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 42; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          t.kind = 3; break;
        case 43:
          t.kind = 3; break;
        case 44:
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']' && ch <= 65535)
          { AddCh(); goto case 45; }
          if (ch == 92)
          { AddCh(); goto case 190; }
          t.kind = noSym; break;
        case 45:
          if (ch == 39)
          { AddCh(); goto case 60; }
          t.kind = noSym; break;
        case 46:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 47; }
          t.kind = noSym; break;
        case 47:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 191; }
          if (ch == 39)
          { AddCh(); goto case 60; }
          t.kind = noSym; break;
        case 48:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 49; }
          t.kind = noSym; break;
        case 49:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 50; }
          t.kind = noSym; break;
        case 50:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 51; }
          t.kind = noSym; break;
        case 51:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 45; }
          t.kind = noSym; break;
        case 52:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 53; }
          t.kind = noSym; break;
        case 53:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 54; }
          t.kind = noSym; break;
        case 54:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 55; }
          t.kind = noSym; break;
        case 55:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 56; }
          t.kind = noSym; break;
        case 56:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 57; }
          t.kind = noSym; break;
        case 57:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 58; }
          t.kind = noSym; break;
        case 58:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 59; }
          t.kind = noSym; break;
        case 59:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 45; }
          t.kind = noSym; break;
        case 60:
          t.kind = 4; break;
        case 61:
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535)
          { AddCh(); goto case 61; }
          if (ch == '"')
          { AddCh(); goto case 77; }
          if (ch == 92)
          { AddCh(); goto case 193; }
          t.kind = noSym; break;
        case 62:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 63; }
          t.kind = noSym; break;
        case 63:
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535)
          { AddCh(); goto case 61; }
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 194; }
          if (ch == '"')
          { AddCh(); goto case 77; }
          if (ch == 92)
          { AddCh(); goto case 193; }
          t.kind = noSym; break;
        case 64:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 65; }
          t.kind = noSym; break;
        case 65:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 66; }
          t.kind = noSym; break;
        case 66:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 67; }
          t.kind = noSym; break;
        case 67:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 61; }
          t.kind = noSym; break;
        case 68:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 69; }
          t.kind = noSym; break;
        case 69:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 70; }
          t.kind = noSym; break;
        case 70:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 71; }
          t.kind = noSym; break;
        case 71:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 72; }
          t.kind = noSym; break;
        case 72:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 73; }
          t.kind = noSym; break;
        case 73:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 74; }
          t.kind = noSym; break;
        case 74:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 75; }
          t.kind = noSym; break;
        case 75:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 61; }
          t.kind = noSym; break;
        case 76:
          if (ch <= '!' || ch >= '#' && ch <= 65535)
          { AddCh(); goto case 76; }
          if (ch == '"')
          { AddCh(); goto case 196; }
          t.kind = noSym; break;
        case 77:
          t.kind = 5; break;
        case 78:
          t.kind = 85; break;
        case 79:
          t.kind = 88; break;
        case 80:
          t.kind = 89; break;
        case 81:
          t.kind = 90; break;
        case 82:
          t.kind = 92; break;
        case 83:
          t.kind = 93; break;
        case 84:
          t.kind = 95; break;
        case 85:
          t.kind = 96; break;
        case 86:
          t.kind = 97; break;
        case 87:
          t.kind = 98; break;
        case 88:
          t.kind = 99; break;
        case 89:
          t.kind = 100; break;
        case 90:
          t.kind = 104; break;
        case 91:
          t.kind = 105; break;
        case 92:
          t.kind = 106; break;
        case 93:
          t.kind = 108; break;
        case 94:
          t.kind = 110; break;
        case 95:
          t.kind = 112; break;
        case 96:
          t.kind = 113; break;
        case 97:
          t.kind = 114; break;
        case 98:
          t.kind = 115; break;
        case 99:
          t.kind = 116; break;
        case 100:
          t.kind = 118; break;
        case 101:
          t.kind = 119; break;
        case 102:
          t.kind = 120; break;
        case 103:
          if (ch == 'e')
          { AddCh(); goto case 104; }
          t.kind = noSym; break;
        case 104:
          if (ch == 'f')
          { AddCh(); goto case 105; }
          t.kind = noSym; break;
        case 105:
          if (ch == 'i')
          { AddCh(); goto case 106; }
          t.kind = noSym; break;
        case 106:
          if (ch == 'n')
          { AddCh(); goto case 107; }
          t.kind = noSym; break;
        case 107:
          if (ch == 'e')
          { AddCh(); goto case 108; }
          t.kind = noSym; break;
        case 108:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 109; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 108; }
          t.kind = noSym; break;
        case 109:
          t.kind = 146; break;
        case 110:
          if (ch == 'n')
          { AddCh(); goto case 111; }
          t.kind = noSym; break;
        case 111:
          if (ch == 'd')
          { AddCh(); goto case 112; }
          t.kind = noSym; break;
        case 112:
          if (ch == 'e')
          { AddCh(); goto case 113; }
          t.kind = noSym; break;
        case 113:
          if (ch == 'f')
          { AddCh(); goto case 114; }
          t.kind = noSym; break;
        case 114:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 115; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 114; }
          t.kind = noSym; break;
        case 115:
          t.kind = 147; break;
        case 116:
          if (ch == 'f')
          { AddCh(); goto case 117; }
          t.kind = noSym; break;
        case 117:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 118; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 117; }
          t.kind = noSym; break;
        case 118:
          t.kind = 148; break;
        case 119:
          if (ch == 'f')
          { AddCh(); goto case 120; }
          t.kind = noSym; break;
        case 120:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 121; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 120; }
          t.kind = noSym; break;
        case 121:
          t.kind = 149; break;
        case 122:
          if (ch == 'e')
          { AddCh(); goto case 123; }
          t.kind = noSym; break;
        case 123:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 124; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 123; }
          t.kind = noSym; break;
        case 124:
          t.kind = 150; break;
        case 125:
          if (ch == 'f')
          { AddCh(); goto case 126; }
          t.kind = noSym; break;
        case 126:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 127; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 126; }
          t.kind = noSym; break;
        case 127:
          t.kind = 151; break;
        case 128:
          if (ch == 'i')
          { AddCh(); goto case 129; }
          t.kind = noSym; break;
        case 129:
          if (ch == 'n')
          { AddCh(); goto case 130; }
          t.kind = noSym; break;
        case 130:
          if (ch == 'e')
          { AddCh(); goto case 131; }
          t.kind = noSym; break;
        case 131:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 132; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 131; }
          t.kind = noSym; break;
        case 132:
          t.kind = 152; break;
        case 133:
          if (ch == 'r')
          { AddCh(); goto case 134; }
          t.kind = noSym; break;
        case 134:
          if (ch == 'o')
          { AddCh(); goto case 135; }
          t.kind = noSym; break;
        case 135:
          if (ch == 'r')
          { AddCh(); goto case 136; }
          t.kind = noSym; break;
        case 136:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 137; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 136; }
          t.kind = noSym; break;
        case 137:
          t.kind = 153; break;
        case 138:
          if (ch == 'a')
          { AddCh(); goto case 139; }
          t.kind = noSym; break;
        case 139:
          if (ch == 'r')
          { AddCh(); goto case 140; }
          t.kind = noSym; break;
        case 140:
          if (ch == 'n')
          { AddCh(); goto case 141; }
          t.kind = noSym; break;
        case 141:
          if (ch == 'i')
          { AddCh(); goto case 142; }
          t.kind = noSym; break;
        case 142:
          if (ch == 'n')
          { AddCh(); goto case 143; }
          t.kind = noSym; break;
        case 143:
          if (ch == 'g')
          { AddCh(); goto case 144; }
          t.kind = noSym; break;
        case 144:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 145; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 144; }
          t.kind = noSym; break;
        case 145:
          t.kind = 154; break;
        case 146:
          if (ch == 'r')
          { AddCh(); goto case 147; }
          t.kind = noSym; break;
        case 147:
          if (ch == 'a')
          { AddCh(); goto case 148; }
          t.kind = noSym; break;
        case 148:
          if (ch == 'g')
          { AddCh(); goto case 149; }
          t.kind = noSym; break;
        case 149:
          if (ch == 'm')
          { AddCh(); goto case 150; }
          t.kind = noSym; break;
        case 150:
          if (ch == 'a')
          { AddCh(); goto case 151; }
          t.kind = noSym; break;
        case 151:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 152; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 151; }
          t.kind = noSym; break;
        case 152:
          t.kind = 155; break;
        case 153:
          if (ch == 'e')
          { AddCh(); goto case 154; }
          t.kind = noSym; break;
        case 154:
          if (ch == 'g')
          { AddCh(); goto case 155; }
          t.kind = noSym; break;
        case 155:
          if (ch == 'i')
          { AddCh(); goto case 156; }
          t.kind = noSym; break;
        case 156:
          if (ch == 'o')
          { AddCh(); goto case 157; }
          t.kind = noSym; break;
        case 157:
          if (ch == 'n')
          { AddCh(); goto case 158; }
          t.kind = noSym; break;
        case 158:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 159; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 158; }
          t.kind = noSym; break;
        case 159:
          t.kind = 156; break;
        case 160:
          if (ch == 'e')
          { AddCh(); goto case 161; }
          t.kind = noSym; break;
        case 161:
          if (ch == 'g')
          { AddCh(); goto case 162; }
          t.kind = noSym; break;
        case 162:
          if (ch == 'i')
          { AddCh(); goto case 163; }
          t.kind = noSym; break;
        case 163:
          if (ch == 'o')
          { AddCh(); goto case 164; }
          t.kind = noSym; break;
        case 164:
          if (ch == 'n')
          { AddCh(); goto case 165; }
          t.kind = noSym; break;
        case 165:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 166; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 165; }
          t.kind = noSym; break;
        case 166:
          t.kind = 157; break;
        case 167:
          if (ch <= ')' || ch >= '+' && ch <= 65535)
          { AddCh(); goto case 167; }
          if (ch == '*')
          { AddCh(); goto case 197; }
          t.kind = noSym; break;
        case 168:
          t.kind = 158; break;
        case 169:
          if (ch == 10 || ch == 13)
          { AddCh(); goto case 170; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= 65535)
          { AddCh(); goto case 169; }
          t.kind = noSym; break;
        case 170:
          t.kind = 159; break;
        case 171:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 171; }
          if (ch == 'U')
          { AddCh(); goto case 182; }
          if (ch == 'u')
          { AddCh(); goto case 183; }
          if (ch == 'L')
          { AddCh(); goto case 184; }
          if (ch == 'l')
          { AddCh(); goto case 185; }
          if (ch == '.')
          { apx++; AddCh(); goto case 198; }
          if (ch == 'E' || ch == 'e')
          { AddCh(); goto case 40; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          t.kind = 2; break;
        case 172:
          if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255)
          { AddCh(); goto case 1; }
          if (ch == 92)
          { AddCh(); goto case 15; }
          if (ch == '"')
          { AddCh(); goto case 76; }
          t.kind = noSym; break;
        case 173:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 171; }
          if (ch == 'U')
          { AddCh(); goto case 182; }
          if (ch == 'u')
          { AddCh(); goto case 183; }
          if (ch == 'L')
          { AddCh(); goto case 184; }
          if (ch == 'l')
          { AddCh(); goto case 185; }
          if (ch == '.')
          { apx++; AddCh(); goto case 198; }
          if (ch == 'X' || ch == 'x')
          { AddCh(); goto case 29; }
          if (ch == 'E' || ch == 'e')
          { AddCh(); goto case 40; }
          if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm')
          { AddCh(); goto case 43; }
          t.kind = 2; break;
        case 174:
          if (ch >= '0' && ch <= '9')
          { AddCh(); goto case 32; }
          t.kind = 91; break;
        case 175:
          if (ch == '=')
          { AddCh(); goto case 83; }
          if (ch == '>')
          { AddCh(); goto case 102; }
          t.kind = 86; break;
        case 176:
          if (ch == ':')
          { AddCh(); goto case 82; }
          t.kind = 87; break;
        case 177:
          if (ch == '=')
          { AddCh(); goto case 84; }
          t.kind = 94; break;
        case 178:
          if (ch == '+')
          { AddCh(); goto case 85; }
          if (ch == '=')
          { AddCh(); goto case 94; }
          t.kind = 109; break;
        case 179:
          if (ch == '=')
          { AddCh(); goto case 92; }
          t.kind = 107; break;
        case 180:
          if (ch == '=')
          { AddCh(); goto case 100; }
          t.kind = 117; break;
        case 181:
          if (ch == 9 || ch >= 11 && ch <= 12 || ch == ' ')
          { AddCh(); goto case 181; }
          if (ch == 'd')
          { AddCh(); goto case 103; }
          if (ch == 'u')
          { AddCh(); goto case 110; }
          if (ch == 'i')
          { AddCh(); goto case 116; }
          if (ch == 'e')
          { AddCh(); goto case 200; }
          if (ch == 'l')
          { AddCh(); goto case 128; }
          if (ch == 'w')
          { AddCh(); goto case 138; }
          if (ch == 'p')
          { AddCh(); goto case 146; }
          if (ch == 'r')
          { AddCh(); goto case 153; }
          t.kind = noSym; break;
        case 182:
          if (ch == 'L' || ch == 'l')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 183:
          if (ch == 'L' || ch == 'l')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 184:
          if (ch == 'U' || ch == 'u')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 185:
          if (ch == 'U' || ch == 'u')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 186:
          if (ch == 'L' || ch == 'l')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 187:
          if (ch == 'L' || ch == 'l')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 188:
          if (ch == 'U' || ch == 'u')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 189:
          if (ch == 'U' || ch == 'u')
          { AddCh(); goto case 31; }
          t.kind = 2; break;
        case 190:
          if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch >= 'a' && ch <= 'b' || ch == 'n' || ch == 'r' || ch == 't')
          { AddCh(); goto case 45; }
          if (ch == 'f' || ch == 'v')
          { AddCh(); goto case 45; }
          if (ch == 'x')
          { AddCh(); goto case 46; }
          if (ch == 'u')
          { AddCh(); goto case 48; }
          if (ch == 'U')
          { AddCh(); goto case 52; }
          t.kind = noSym; break;
        case 191:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 192; }
          if (ch == 39)
          { AddCh(); goto case 60; }
          t.kind = noSym; break;
        case 192:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 45; }
          if (ch == 39)
          { AddCh(); goto case 60; }
          t.kind = noSym; break;
        case 193:
          if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch >= 'a' && ch <= 'b' || ch == 'n' || ch == 'r' || ch == 't')
          { AddCh(); goto case 61; }
          if (ch == 'f' || ch == 'v')
          { AddCh(); goto case 61; }
          if (ch == 'x')
          { AddCh(); goto case 62; }
          if (ch == 'u')
          { AddCh(); goto case 64; }
          if (ch == 'U')
          { AddCh(); goto case 68; }
          t.kind = noSym; break;
        case 194:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 195; }
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '/' || ch >= ':' && ch <= '@' || ch >= 'G' && ch <= '[' || ch >= ']' && ch <= '`' || ch >= 'g' && ch <= 65535)
          { AddCh(); goto case 61; }
          if (ch == '"')
          { AddCh(); goto case 77; }
          if (ch == 92)
          { AddCh(); goto case 193; }
          t.kind = noSym; break;
        case 195:
          if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535)
          { AddCh(); goto case 61; }
          if (ch == '"')
          { AddCh(); goto case 77; }
          if (ch == 92)
          { AddCh(); goto case 193; }
          t.kind = noSym; break;
        case 196:
          if (ch == '"')
          { AddCh(); goto case 76; }
          t.kind = 5; break;
        case 197:
          if (ch <= ')' || ch >= '+' && ch <= '.' || ch >= '0' && ch <= 65535)
          { AddCh(); goto case 167; }
          if (ch == '/')
          { AddCh(); goto case 168; }
          if (ch == '*')
          { AddCh(); goto case 197; }
          t.kind = noSym; break;
        case 198:
          if (ch <= '/' || ch >= ':' && ch <= 65535)
          { apx++; AddCh(); goto case 28; }
          if (ch >= '0' && ch <= '9')
          { apx = 0;AddCh(); goto case 36; }
          t.kind = noSym; break;
        case 199:
          if (ch == '=')
          { AddCh(); goto case 89; }
          t.kind = 102; break;
        case 200:
          if (ch == 'l')
          { AddCh(); goto case 201; }
          if (ch == 'n')
          { AddCh(); goto case 202; }
          if (ch == 'r')
          { AddCh(); goto case 133; }
          t.kind = noSym; break;
        case 201:
          if (ch == 'i')
          { AddCh(); goto case 119; }
          if (ch == 's')
          { AddCh(); goto case 122; }
          t.kind = noSym; break;
        case 202:
          if (ch == 'd')
          { AddCh(); goto case 203; }
          t.kind = noSym; break;
        case 203:
          if (ch == 'i')
          { AddCh(); goto case 125; }
          if (ch == 'r')
          { AddCh(); goto case 160; }
          t.kind = noSym; break;
        case 204:
          t.kind = 136; break;
        case 205:
          t.kind = 137; break;
        case 206:
          t.kind = 138; break;
        case 207:
          t.kind = 141; break;
        case 208:
          t.kind = 144; break;
        case 209:
          if (ch == '=')
          { AddCh(); goto case 78; }
          if (ch == '&')
          { AddCh(); goto case 206; }
          t.kind = 84; break;
        case 210:
          if (ch == '-')
          { AddCh(); goto case 80; }
          if (ch == '=')
          { AddCh(); goto case 90; }
          if (ch == '>')
          { AddCh(); goto case 208; }
          t.kind = 103; break;
        case 211:
          if (ch == '=')
          { AddCh(); goto case 81; }
          if (ch == '*')
          { AddCh(); goto case 167; }
          if (ch == '/')
          { AddCh(); goto case 169; }
          t.kind = 142; break;
        case 212:
          if (ch == '<')
          { AddCh(); goto case 199; }
          if (ch == '=')
          { AddCh(); goto case 207; }
          t.kind = 101; break;
        case 213:
          if (ch == '=')
          { AddCh(); goto case 91; }
          t.kind = 143; break;
        case 214:
          if (ch == '=')
          { AddCh(); goto case 93; }
          if (ch == '|')
          { AddCh(); goto case 205; }
          t.kind = 139; break;
        case 215:
          if (ch == '?')
          { AddCh(); goto case 204; }
          t.kind = 111; break;
        case 216:
          if (ch == '=')
          { AddCh(); goto case 101; }
          t.kind = 140; break;

  		}
	  	t.val = new String(tval, 0, tlen);
		  return t;
	  }
	
    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Get the next token (possibly a token already seen during peeking) 
    /// </summary>
    /// <returns>The next token.</returns>
    //-----------------------------------------------------------------------------------
    public Token Scan()
    {
      if (tokens.next == null)
      {
        return NextToken();
      }
      pt = tokens = tokens.next;
      return tokens;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Peek for the next token, ignore pragmas 
    /// </summary>
    /// <returns>The next token.</returns>
    //-----------------------------------------------------------------------------------
    public Token Peek()
    {
      if (pt.next == null)
      {
        do
        {
          pt = pt.next = NextToken();
        } while (pt.kind > maxT); // skip pragmas
      }
      else
      {
        do
        {
          pt = pt.next;
        } while (pt.kind > maxT);
      }
      return pt;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Peek for the next token, but does not ignore pragmas 
    /// </summary>
    /// <returns>The next token.</returns>
    //-----------------------------------------------------------------------------------
    public Token PeekWithPragma()
    {
      pt = pt.next ?? (pt.next = NextToken());
      return pt;
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Make sure that peeking starts at the current scan position 
    /// </summary>
    //-----------------------------------------------------------------------------------
    public void ResetPeek()
    {
       pt = tokens;
    }
  }

}