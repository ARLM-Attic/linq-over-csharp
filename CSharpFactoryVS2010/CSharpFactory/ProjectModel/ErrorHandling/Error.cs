using System;
using CSharpFactory.Collections;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a compilation (syntax or semantics) error.
  /// </summary>
  // ==================================================================================
  public sealed class Error
  {
    #region Private fields

    private readonly string _Code;
    private readonly string _Description;
    private readonly string _File;
    private readonly int _Line;
    private readonly int _Column;
    private readonly int _Posititon;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="line">Line number</param>
    /// <param name="column">Column number</param>
    /// <param name="position">File position number</param>
    /// <param name="file">File that caused the error.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    public Error(string code, int line, int column, int position, string file, 
      string description):
      this(code, line, column, position, file, description, null)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="line">Line number</param>
    /// <param name="column">Column number</param>
    /// <param name="position">File position number</param>
    /// <param name="file">File that caused the error.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    public Error(string code, int line, int column, int position, string file, 
      string description, params object[] parameters)
    {
      _Code = code;
      _Line = line;
      _Column = column;
      _Posititon = position;
      if (parameters == null)
      {
        _Description = description;
      }
      else
      {
        _Description = String.Format(description, parameters);
      }
      _File = file;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the code of the error.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Code
    {
      get { return _Code; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the detailed description of the error.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Description
    {
      get { return _Description; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the file that caused the error.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string File
    {
      get { return _File; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the error line within the file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Line
    {
      get { return _Line; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the error column within the line.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Column
    {
      get { return _Column; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the error position within the file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Position
    {
      get { return _Posititon; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents list of errors.
  /// </summary>
  // ==================================================================================
  public sealed class ErrorCollection : ImmutableCollection<Error>
  {
  }
}
