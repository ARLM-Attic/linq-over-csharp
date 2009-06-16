
using System;
using System.IO;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpAstBuilder.PPExpressions {

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

    /// <summary>Tokens are kept in linked list.</summary>
    public Token next; 
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
    private const int MaxBufferLength = 4 * 1024; // 4KB

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
      else if (Pos < _FileLength)
      {
        Pos = Pos; // shift buffer start to Pos
        return _Buffer[_Position++];
      }
      else
      {
        return EOF;
      }
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
      int len = end - beg;
      char[] buf = new char[len];
      int oldPos = Pos;
      Pos = beg;
      for (int i = 0; i < len; i++) buf[i] = (char)Read();
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
  /// This class represents the scanner for preprocessor expressions.
  /// </summary>
  // ==================================================================================
  public class PPScanner
  {
    #region Constant declarations

    const char EOL = '\n';
    const int eofSym = 0; /* pdt */

    #endregion

    #region Constant declarations generated by CoCo

    const int maxT = 11;
    const int noSym = 11;

    #endregion
    
    #region Private fields

    Buffer buffer;    // scanner buffer
    Token t;          // current token
    int ch;           // current input character
    int pos;          // byte position of current character
    int col;          // column number of current character
    int line;         // line number of current character
    int oldEols;      // EOLs that appeared in a comment;
    Dictionary<int, int> start; // maps first token character to start state
    Token tokens;     // list of tokens already peeked (first token is a dummy)
    Token pt;         // current peek token
    char[] tval = new char[128]; // text of current token
    int tlen;         // length of current token

    #endregion

    #region Lifecycle methods

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a scanner instance based on the specified stream.
    /// </summary>
    /// <param name="s">Stream to scan.</param>
    //-----------------------------------------------------------------------------------
    public PPScanner(Stream s)
    {
      buffer = new Buffer(s, true);
      Init();
    }

    #endregion
	
    #region Public properties

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
      start = new Dictionary<int, int>(128);
    for (int i = 65; i <= 90; ++i) start[i] = 2;
    for (int i = 95; i <= 95; ++i) start[i] = 2;
    for (int i = 97; i <= 122; ++i) start[i] = 2;
    for (int i = 170; i <= 170; ++i) start[i] = 2;
    for (int i = 181; i <= 181; ++i) start[i] = 2;
    for (int i = 186; i <= 186; ++i) start[i] = 2;
    for (int i = 192; i <= 214; ++i) start[i] = 2;
    for (int i = 216; i <= 246; ++i) start[i] = 2;
    for (int i = 248; i <= 255; ++i) start[i] = 2;
    start[92] = 16; 
    start[64] = 1; 
    start[38] = 29; 
    start[61] = 31; 
    start[40] = 33; 
    start[33] = 38; 
    start[124] = 35; 
    start[41] = 37; 
    start[Buffer.EOF] = -1;

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
        // replace isolated '\r' by '\n' in order to make
        // eol handling uniform across Windows, Unix and Mac
        if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
        if (ch == EOL) { line++; col = 0; }
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
        char[] newBuf = new char[2 * tval.Length];
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
        case "false": t.kind = 2; break;
        case "true": t.kind = 3; break;
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

      t = new Token();
      t.pos = pos; t.col = col; t.line = line;
      int state;
      try { state = start[ch]; }
      catch (KeyNotFoundException) { state = 0; }
      tlen = 0; AddCh();

      switch (state)
      {
        case -1: { t.kind = eofSym; break; } // NextCh already done
        case 0: { t.kind = noSym; break; }   // NextCh already done
        case 1:
          if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255)
          { AddCh(); goto case 2; }
          if (ch == 92)
          { AddCh(); goto case 16; }
          t.kind = noSym; break;
        case 2:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z' || ch == 160 || ch == 170 || ch == 181 || ch == 186 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 246 || ch >= 248 && ch <= 255)
          { AddCh(); goto case 2; }
          if (ch == 92)
          { AddCh(); goto case 3; }
          t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;
        case 3:
          if (ch == 'u')
          { AddCh(); goto case 4; }
          if (ch == 'U')
          { AddCh(); goto case 8; }
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
          { AddCh(); goto case 7; }
          t.kind = noSym; break;
        case 7:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 2; }
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
          { AddCh(); goto case 15; }
          t.kind = noSym; break;
        case 15:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 2; }
          t.kind = noSym; break;
        case 16:
          if (ch == 'u')
          { AddCh(); goto case 17; }
          if (ch == 'U')
          { AddCh(); goto case 21; }
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
          { AddCh(); goto case 20; }
          t.kind = noSym; break;
        case 20:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 2; }
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
          { AddCh(); goto case 28; }
          t.kind = noSym; break;
        case 28:
          if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f')
          { AddCh(); goto case 2; }
          t.kind = noSym; break;
        case 29:
          if (ch == '&')
          { AddCh(); goto case 30; }
          t.kind = noSym; break;
        case 30:
          t.kind = 4; break;
        case 31:
          if (ch == '=')
          { AddCh(); goto case 32; }
          t.kind = noSym; break;
        case 32:
          t.kind = 5; break;
        case 33:
          t.kind = 6; break;
        case 34:
          t.kind = 7; break;
        case 35:
          if (ch == '|')
          { AddCh(); goto case 36; }
          t.kind = noSym; break;
        case 36:
          t.kind = 9; break;
        case 37:
          t.kind = 10; break;
        case 38:
          if (ch == '=')
          { AddCh(); goto case 34; }
          t.kind = 8; break;

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
      else
      {
        pt = tokens = tokens.next;
        return tokens;
      }
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
    /// Make sure that peeking starts at the current scan position 
    /// </summary>
    //-----------------------------------------------------------------------------------
    public void ResetPeek()
    {
       pt = tokens;
    }
  }

}