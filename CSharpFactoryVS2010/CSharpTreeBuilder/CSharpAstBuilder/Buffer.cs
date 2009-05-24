// ================================================================================================
// Buffer.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.IO;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
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
    // ReSharper disable InconsistentNaming
    public const int EOF = char.MaxValue + 1;
    // ReSharper restore InconsistentNaming
    
    private const int MaxBufferLength = 256 * 1024; // 256KB

    #endregion

    #region Private fields

    private readonly byte[] _Buffer;      // input buffer
    private int _StartPosition;           // position of first byte in buffer relative to input stream
    private int _BufferLength;            // length of buffer
    private readonly int _FileLength;     // length of input stream
    private int _Position;                // current position in buffer
    private Stream _Stream;               // input stream (seekable)
    private readonly bool _IsUserStream;  // was the stream opened by the user?

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

}