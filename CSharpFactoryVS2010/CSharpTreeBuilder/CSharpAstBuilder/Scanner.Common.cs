// ================================================================================================
// Scanner.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.IO;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpAstBuilder 
{
  // ================================================================================================
  /// <summary>
  /// This class represents the scanner.
  /// </summary>
  // ================================================================================================
  public partial class Scanner
  {
    #region Constant declarations

    const char EOL = '\n';
    const int eofSym = 0; /* pdt */

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
        throw new ParseFileIOException("Cannot open file " + fileName);
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
    /// Gets the current position.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public int Position
    {
      get { return pos; }
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

    #region Operations

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the scanner.
    /// </summary>
    //---------------------------------------------------------------------------------------------
    void Init()
    {
      _SkipMode = false;
      _EOFFound = false;

      pos = -1; 
      line = 1; 
      col = 0;
      oldEols = 0;

      // --- At this point we reached the beginning of the first line.
      RaiseNewLineReached(line);

      // --- Start scanning the stream
      NextCh();
      if (ch == 0xEF)
      { // check optional byte order mark for UTF-8
        NextCh(); int ch1 = ch;
        NextCh(); int ch2 = ch;
        if (ch1 != 0xBB || ch2 != 0xBF)
        {
          throw new ParseFileIOException(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
        }
        buffer = new UTF8Buffer(buffer); col = 0;
        NextCh();
      }
  		pt = tokens = new Token();  // first token is a dummy
	  }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Get the next token (possibly a token already seen during peeking) 
    /// </summary>
    /// <returns>The next token.</returns>
    //---------------------------------------------------------------------------------------------
    public Token Scan()
    {
      if (tokens.Next == null) tokens.Next = NextToken();
      pt = tokens = tokens.Next;
      return tokens;
    }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Peek for the next token, ignore pragmas 
    /// </summary>
    /// <returns>The next token.</returns>
    //---------------------------------------------------------------------------------------------
    public Token Peek()
    {
      if (pt.Next == null)
      {
        do
        {
          pt = pt.Next = NextToken();
        } while (pt.kind > maxT); // skip pragmas
      }
      else
      {
        do
        {
          pt = pt.Next;
        } while (pt.kind > maxT);
      }
      return pt;
    }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Peek for the next token, but does not ignore pragmas 
    /// </summary>
    /// <returns>The next token.</returns>
    //---------------------------------------------------------------------------------------------
    public Token PeekWithPragma()
    {
      pt = pt.Next ?? (pt.Next = NextToken());
      return pt;
    }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Make sure that peeking starts at the current scan position 
    /// </summary>
    //---------------------------------------------------------------------------------------------
    public void ResetPeek()
    {
       pt = tokens;
    }

    #endregion

    #region Scanner events

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Occurs when the scanner reaches a new line.
    /// </summary>
    //---------------------------------------------------------------------------------------------
    public event EventHandler<NewLineReachedEventArgs> NewLineReached;

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Raises the "new line reached" event.
    /// </summary>
    /// <param name="line">The line number reached.</param>
    //---------------------------------------------------------------------------------------------
    private void RaiseNewLineReached(int line)
    {
      if (NewLineReached != null)
        NewLineReached.Invoke(this, new NewLineReachedEventArgs(line));
    }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Occurs when the scanner scannes a new token.
    /// </summary>
    //---------------------------------------------------------------------------------------------
    public event EventHandler<TokenScannedEventArgs> TokenScanned;

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Raises the "token scanned" event.
    /// </summary>
    /// <param name="token">The token scanned.</param>
    //---------------------------------------------------------------------------------------------
    private void RaiseTokenScanned(Token token)
    {
      if (TokenScanned != null)  
        TokenScanned.Invoke(this, new TokenScannedEventArgs(token));
    }

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Occurs when the scanner scannes whitespaces.
    /// </summary>
    //---------------------------------------------------------------------------------------------
    public event EventHandler<WhitespaceScannedEventArgs> WhitespaceScanned;

    //---------------------------------------------------------------------------------------------
    /// <summary>
    /// Raises the "whitespace scanned" event .
    /// </summary>
    /// <param name="whitespace">The whitespace.</param>
    //---------------------------------------------------------------------------------------------
    private void RaiseWhitespaceScanned(string whitespace)
    {
      if (WhitespaceScanned != null)
        WhitespaceScanned.Invoke(this, new WhitespaceScannedEventArgs(whitespace));
    }

    #endregion
  }
}