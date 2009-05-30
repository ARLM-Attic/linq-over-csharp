// ================================================================================================
// SyntaxNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be a base class for all collections that themselves
  /// represent a syntax node. An instance of this collection must be a syntax node.
  /// </summary>
  /// <remarks>
  ///     This class implements the <see cref="ISyntaxNode"/> interface so that a
  ///     collection of other syntax node could behave as a single syntax node having a start
  ///     token and a terminating token. For example, the list of current method parameters
  ///     is represented by class derived from this class. The start token is the opening
  ///     parenthesis, the terminating token is the closing paranthesis, and items of the
  ///     collection are the current method parameters.
  /// </remarks>
  /// <typeparam name="TNode">The type of the node, that must be an <see cref="ISyntaxNode"/>.</typeparam>
  // ================================================================================================
  public abstract class SyntaxNodeCollection<TNode, TParent> : ImmutableCollection<TNode>, ISyntaxNode
    where TNode: ISyntaxNode
    where TParent: ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent node of this syntax node.
    /// </summary>
    /// <value>The parent node.</value>
    // ----------------------------------------------------------------------------------------------
    public TParent ParentNode { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional leading separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Line numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int StartLine
    {
      get { return StartToken == null ? -1 : StartToken.Line; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending line number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Line numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int EndLine
    {
      get { return TerminatingToken == null ? -1 : TerminatingToken.Line; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Column numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return StartToken == null ? -1 : StartToken.Column; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending column number of the syntax element.
    /// </summary>
    /// <remarks>
    /// Column numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public int EndColumn
    {
      get 
      { 
        return TerminatingToken == null 
          ? -1 
          : TerminatingToken.Column + TerminatingToken.Value.Length - 1; 
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int StartPosition
    {
      get { return StartToken == null ? -1 : StartToken.Position; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int EndPosition
    {
      get
      {
        return TerminatingToken == null 
          ? -1 
          : TerminatingToken.Position + TerminatingToken.Value.Length - 1;
      }
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
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public virtual OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        Count > 0 && this[0].StartToken != StartToken ? StartToken : null,
        new List<TNode>(this),
        Count > 0 && this[Count - 1].TerminatingToken != TerminatingToken ? TerminatingToken : null
        );
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new item to the collection.
    /// </summary>
    /// <remarks>
    ///     When the first item is added to the collection and no start token is set, the
    ///     collection's start token is automatically set to the start token of the that item.
    ///     The terminating token is set to the terminating token of the last item in the
    ///     collection. Because of this design, set the terminating token of any items before
    ///     adding them to the collection. If you cannot guarante that the terminating token of
    ///     an item is set before adding it to the collection, use the <see cref="Terminate"/> method to explicitly set the terminating token of the
    ///     collection.
    /// </remarks>
    /// <param name="item">Item to add to the collection.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Add(TNode item)
    {
      base.Add(item);
      if (Count == 0 && StartToken == null) StartToken = item.StartToken;
      TerminatingToken = item.TerminatingToken;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified item to the collection and sets its separator to the specified token.
    /// </summary>
    /// <param name="separator">The separator token.</param>
    /// <param name="item">Item to add to the collection.</param>
    // ----------------------------------------------------------------------------------------------
    public void Add(Token separator, TNode item)
    {
      item.SeparatorToken = separator;
      Add(item);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Marks the termination of this language element.
    /// </summary>
    /// <param name="token">Terminating token</param>
    // ----------------------------------------------------------------------------------------------
    internal void Start(Token token)
    {
      StartToken = token;
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
  }
}