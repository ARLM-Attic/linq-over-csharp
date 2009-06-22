// ================================================================================================
// CSharpParser.Comments.cs
//
// Created: 2009.06.17, by Istvan Novak (DeepDiver)
// ================================================================================================

using System;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Comment handler parts of the AST Builder
  /// </summary>
  // ================================================================================================
  public partial class CSharpParser
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the current orphan comment that has not been assigned to any syntax node or
    /// token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private ICommentNode OrphanComment { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the comment owner candidate.
    /// </summary>
    /// <value>The comment owner candidate.</value>
    // ----------------------------------------------------------------------------------------------
    private ISyntaxNode _CommentOwnerCandidate;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// This method is responsible to handle line comments.
    /// </summary>
    /// <param name="token">The token representing the line comment.</param>
    // ----------------------------------------------------------------------------------------------
    private void HandleLineComment(Token token)
    {
      ProcessComment(new LineCommentNode(token));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// This method is responsible to handle (potentially multi-line) block comments.
    /// </summary>
    /// <param name="token">The token representing the block comment.</param>
    // ----------------------------------------------------------------------------------------------
    private void HandleBlockComment(Token token)
    {
      ProcessComment(new BlockCommentNode(token));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Processes the specified comment.
    /// </summary>
    /// <param name="comment">The comment to process.</param>
    // ----------------------------------------------------------------------------------------------
    private void ProcessComment(ICommentNode comment)
    {
      // --- Check, if the comment is the first token in this line
      if (CheckTokenIsFirstInLine(comment.StartToken))
      {
        // --- This token is the first in this line, so attach it to the orphan comment.
        var multiBlock = OrphanComment as MultiCommentNode;
        if (OrphanComment == null)
        {
          // --- This is the first part of the orphan comment.
          OrphanComment = comment;
        }
        else
        {
          if (multiBlock == null)
          {
            // --- The orphan comment should be converted to a multi-comment block.
            multiBlock = new MultiCommentNode {OrphanComment};
            OrphanComment = multiBlock;
          }
          multiBlock.Add(comment);
        }
      }
      else
      {
        // --- This comment is an end-of-line comment, so attach it to the previous token.
        if (comment.StartToken.Previous != null)
        {
          comment.OwnerToken = comment.StartToken.Previous;
          comment.StartToken.Previous.TrailingComment = comment;
        }
        else
        {
          throw new InvalidOperationException("Token with <null> previous token found.");
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the syntax node that is the potential owner of the current orphan comment.
    /// </summary>
    /// <param name="commentOwner">The comment owner.</param>
    // ----------------------------------------------------------------------------------------------
    private void SetCommentOwner(ISyntaxNode commentOwner)
    {
      if (commentOwner == null) return;
      _CommentOwnerCandidate = commentOwner;
      if (OrphanComment != null && OrphanComment.StartToken.pos < _CommentOwnerCandidate.StartToken.pos)
      {
        // --- The orphan comment can be added to this language element.
        OrphanComment.OwnerNode = _CommentOwnerCandidate;
        _CommentOwnerCandidate.Comment = OrphanComment;
        OrphanComment = null;
      }
    }
  }
}