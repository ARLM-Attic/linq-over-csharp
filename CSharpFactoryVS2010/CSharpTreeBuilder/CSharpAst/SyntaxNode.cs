// ================================================================================================
// SyntaxNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  ///     This abstract class is the common base class for all syntax nodes belonging to a
  ///     syntax tree of a compilation unit. The root object object of the syntax tree is
  ///     represented by a <see cref="CompilationUnitNode"/> instance.
  /// </summary>
  /// <remarks>
  /// 	<para>
  ///         This node represents a logical unit of tokens composing a syntax node, like a
  ///         using directive, a type declaration, a member declaration a statement, and so
  ///         on. Any syntax node can encapsulate other syntax nodes, or even 
  ///         <see cref="SyntaxNodeCollection{TNode, TParent}"/> instances. Syntax nodes can 
  ///         have zero or more parents that have a type of <typeparamref name="TParent"/>. You
  ///         can access this node through the <see cref="ParentNode"/> property.
  ///     </para>
  /// </remarks>
  /// <typeparam name="TParent">
  /// Type of the parent syntax node, must implement <see cref="ISyntaxNode"/>.
  /// </typeparam>
  // ================================================================================================
  public abstract class SyntaxNode<TParent> : ISyntaxNode
    where TParent: class, ISyntaxNode
  {
    // --- Backing fields
    private CompilationUnitNode _Owner;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxNode&lt;TParent&gt;"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNode()
      : this(null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a syntax node descriptor.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected SyntaxNode(Token start)
    {
      SemanticEntities = new List<SemanticEntity>();

      StartToken = start;
      TerminatingToken = start;
      Validate();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit node owning this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNode Owner
    {
      get { return _Owner; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the owner compilation unit of this syntax node.
    /// </summary>
    /// <param name="owner">The owner.</param>
    // ----------------------------------------------------------------------------------------------
    void ISyntaxNode.SetOwner(CompilationUnitNode owner)
    {
      _Owner = owner;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent node of this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISyntaxNode ISyntaxNode.Parent
    {
      get { return ParentNode; }
      set { ParentNode = value as TParent; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent node of this syntax node.
    /// </summary>
    /// <value>The parent node.</value>
    /// <example>
    /// Parent is optional. If an item is in a syntax node collections, this property
    /// should point to the parent of the collection and not to the collection itself.
    /// </example>
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
    public Token StartToken { get; protected set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating token of this language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token TerminatingToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start symbol of this syntax node
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISymbolReference StartSymbol { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the start symbol according to the specified token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    public void SetStartSymbol(Token token)
    {
      if (token == null) throw new ArgumentNullException("token");
      StartToken = token;
      StartSymbol = token.BoundToStream
        ? new CSharpSymbolReference(token.TokenizedStreamPosition) as ISymbolReference
        : new CSharpSymbol(token.Kind, token.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating symbol of this syntax node
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ISymbolReference TerminatingSymbol { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element is valid in its context.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsValid { get; private set; }

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
    /// <summary>Sets the token terminating this syntax node.</summary>
    /// <param name="token">Terminating token</param>
    // ----------------------------------------------------------------------------------------------
    public void Terminate(Token token)
    {
      if (token == null) throw new ArgumentNullException("token");
      TerminatingToken = token;
      TerminatingSymbol = new CSharpSymbolReference(token.TokenizedStreamPosition);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the comment related to the syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ICommentNode Comment { get; set; }

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
      return new OutputSegment(SeparatorToken, StartToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return StartToken == null 
        ? GetType().ToString()
        : StartToken.Value;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of semantic entities created from this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<SemanticEntity> SemanticEntities { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit node for this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNode CompilationUnitNode
    {
      get
      {
        ISyntaxNode node = this;
        while (node != null && !(node is CompilationUnitNode))
        {
          node = node.Parent;
        }
        return node as CompilationUnitNode;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the SourcePoint of the start position of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourcePoint SourcePoint
    {
      get
      {
        return new SourcePoint(CompilationUnitNode, StartPosition);
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      throw new ApplicationException(string.Format("SyntaxNode.AcceptVisitor called on type: {0}",GetType()));
    }

    #endregion
  }
}