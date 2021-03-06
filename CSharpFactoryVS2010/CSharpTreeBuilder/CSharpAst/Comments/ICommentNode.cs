// ================================================================================================
// ICommentNode.cs
//
// Created: 2009.06.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the common behavior of a comment entity.
  /// </summary>
  // ================================================================================================
  public interface ICommentNode: ISyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has documentation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool HasDocumentation { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text represented by the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    string Text { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node that owns the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISyntaxNode OwnerNode { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token that owns the comment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token OwnerToken { get; set; }
  }
}