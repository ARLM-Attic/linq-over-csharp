using System;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

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

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_ExceptionType != null)
      {
        _ExceptionType.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}