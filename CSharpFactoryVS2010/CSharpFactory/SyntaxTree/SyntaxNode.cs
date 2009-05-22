// ================================================================================================
// SyntaxNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is the common root for all syntax element nodes belonging to a source 
  /// file node.
  /// </summary>
  // ================================================================================================
  public abstract class SyntaxNode
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a syntax node descriptor.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNode(Token start)
    {
      StartToken = start;
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int StartLine
    {
      get { return StartToken.line; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending line number of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int EndLine
    {
      get { return TerminatingToken.line; }  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return StartToken.col; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending column number of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int EndColumn
    {
      get { return TerminatingToken.col + TerminatingToken.val.Length - 1; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int StartPosition
    {
      get { return StartToken.pos; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int EndPosition
    {
      get { return TerminatingToken.pos + TerminatingToken.val.Length - 1; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the starting token of this language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token StartToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating token of this language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token TerminatingToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element is valid in its context.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsValid { get; private set; }

    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is valid in its context.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void Validate()
    {
      IsValid = true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the validity of this language element.
    /// </summary>
    /// <param name="isValid">True, if the language element is valid.</param>
    // ----------------------------------------------------------------------------------------------
    public void Validate(bool isValid)
    {
      IsValid = isValid;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is invalid in its context.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void Invalidate()
    {
      IsValid = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Marks the termination of this language element.
    /// </summary>
    /// <param name="token">Terminating token</param>
    // ----------------------------------------------------------------------------------------------
    public void Terminate(Token token)
    {
      TerminatingToken = token;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public virtual OutputSegment GetOutputSegment()
    {
      return null;
    }

    #endregion

    #region Overridden members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return StartToken.val;
    }

    #endregion
  }
}