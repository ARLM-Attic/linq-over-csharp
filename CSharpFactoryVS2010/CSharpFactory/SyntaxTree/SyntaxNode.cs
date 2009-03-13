// ================================================================================================
// SyntaxNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is the common root for all syntax element nodes belonging to a source 
  /// file node.
  /// </summary>
  // ================================================================================================
  public abstract class SyntaxNode
  {
    #region Private fields

    private readonly Token _Token;
    private bool _IsValid;
    private Token _TerminatingToken;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token. The element uses the specifed element as a context element.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    // --------------------------------------------------------------------------------
    protected SyntaxNode(Token token)
    {
      _Token = token;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the language element.
    /// </summary>
    /// <remarks>
    /// This name can be overridden.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual string Name { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartLine
    {
      get { return _Token.line; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending line number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndLine
    {
      get { return _TerminatingToken.line; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return _Token.col; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending column number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndColumn
    {
      get { return _TerminatingToken.col + _TerminatingToken.val.Length - 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartPosition
    {
      get { return _Token.pos; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending position of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndPosition
    {
      get { return _TerminatingToken.pos + _TerminatingToken.val.Length - 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the starting token of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Token Token
    {
      get { return _Token; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating token of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Token TerminatingToken
    {
      get { return _TerminatingToken; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element is valid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValid
    {
      get { return _IsValid; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is valid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Validate()
    {
      _IsValid = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the validity of this language element.
    /// </summary>
    /// <param name="isValid">True, if the language element is valid.</param>
    // --------------------------------------------------------------------------------
    public void Validate(bool isValid)
    {
      _IsValid = isValid;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is invalid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Invalidate()
    {
      _IsValid = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Marks the termination of this language element.
    /// </summary>
    /// <param name="token">Terminating token</param>
    // --------------------------------------------------------------------------------
    public void Terminate(Token token)
    {
      _TerminatingToken = token;
    }

    #endregion

    #region Overridden members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // --------------------------------------------------------------------------------
    public override string ToString()
    {
      return Name;
    }

    #endregion
  }
}