using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a catch clause in a try..catch..finally statement.
  /// </summary>
  // ==================================================================================
  public sealed class CatchClause : BlockStatement
  {
    #region Private fields

    private TypeReference _ExceptionType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "fixed" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public CatchClause(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of exception in this clause.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ExceptionType
    {
      get { return _ExceptionType; }
      set { _ExceptionType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this clause has an exception specified.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasExceptionType
    {
      get { return _ExceptionType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this clause has an exception instance name
    /// specified.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasName
    {
      get { return !String.IsNullOrEmpty(Name); }
    }

    #endregion
  }
}