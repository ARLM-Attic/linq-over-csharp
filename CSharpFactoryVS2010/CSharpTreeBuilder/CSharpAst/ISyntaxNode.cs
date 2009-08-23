// ================================================================================================
// ISyntaxNode.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This interface defines the responsibilities of a node in the syntax tree.</summary>
  /// <remarks>
  ///     This interface is primarily implemented by <see cref="SyntaxNode{TNode}"/> and
  ///     <see cref="SyntaxNodeCollection{TNode, TParent}"/> but can be implemented by any
  ///     other types to mimic syntax node behavior.
  /// </remarks>
  // ================================================================================================
  public interface ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit node owning this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    CompilationUnitNode Owner { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the owner compilation unit of this syntax node.
    /// </summary>
    /// <param name="owner">The owner.</param>
    // ----------------------------------------------------------------------------------------------
    void SetOwner(CompilationUnitNode owner);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent node of this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISyntaxNode Parent { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional leading separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token SeparatorToken { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Line numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int StartLine { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending line number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Line numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int EndLine { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the syntax node.
    /// </summary>
    /// <remarks>
    /// Column numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int StartColumn { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending column number of the syntax element.
    /// </summary>
    /// <remarks>
    /// Column numbers start at 1.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    int EndColumn { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int StartPosition { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending position of the language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    int EndPosition { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the starting token of this language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token StartToken { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating token of this language element.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token TerminatingToken { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start symbol of this syntax node
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISymbolReference StartSymbol { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the start symbol according to the specified token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    void SetStartSymbol(Token token);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the terminating symbol of this syntax node
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISymbolReference TerminatingSymbol { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    OutputSegment GetOutputSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Terminates this syntax node with the specified token.
    /// </summary>
    /// <param name="t">The t.</param>
    // ----------------------------------------------------------------------------------------------
    void Terminate(Token t);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the comment related to the syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ICommentNode Comment { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of semantic entities created from this syntax node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    List<SemanticEntity> SemanticEntities { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit node for this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    CompilationUnitNode CompilationUnitNode { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the SourcePoint of the start position of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SourcePoint SourcePoint { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    void AcceptVisitor(ISyntaxNodeVisitor visitor);
  }
}