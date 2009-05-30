// ================================================================================================
// ISyntaxNode.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This interface defines the responsibilities of a node in the syntax tree.</summary>
  /// <remarks>
  ///     This interface is primarily implemented by <see cref="SyntaxNode"/> and
  ///     <see cref="SyntaxNodeCollection&lt;TNode&gt;"/> but can be implemented by any
  ///     other types to mimic syntax node behavior.
  /// </remarks>
  // ================================================================================================
  public interface ISyntaxNode
  {
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
  }
}