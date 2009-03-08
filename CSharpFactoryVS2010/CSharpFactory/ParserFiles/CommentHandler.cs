using CSharpFactory.ProjectModel;

namespace CSharpFactory.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This class encapsulates the functionality required to handle comments.
  /// </summary>
  // ==================================================================================
  internal sealed class CommentHandler
  {
    #region Private fields

    private readonly CSharpSyntaxParser _Parser;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of comment handler that works together with the
    /// specified CSharpSyntaxParser instance.
    /// </summary>
    /// <param name="parser">Parser instance.</param>
    // --------------------------------------------------------------------------------
    public CommentHandler(CSharpSyntaxParser parser)
    {
      _Parser = parser;
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses the specified line comment.
    /// </summary>
    /// <param name="commentToken">Pragma token holding the comment.</param>
    // --------------------------------------------------------------------------------
    public void HandleLineComment(Token commentToken)
    {
      CommentInfo comment;
      string text = commentToken.val.Substring(2);
      if (text.EndsWith("\r")) text = text.Substring(0, text.Length - 1);
      if (text.StartsWith("/"))
      {
        text = text.Substring(1);
        comment = new XmlCommentLine(
          commentToken,
          _Parser,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      else
      {
        comment = new LineComment(
          commentToken,
          _Parser,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      ProcessComment(commentToken, comment);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses the specified block comment.
    /// </summary>
    /// <param name="commentToken">Pragma token holding the comment.</param>
    // --------------------------------------------------------------------------------
    public void HandleBlockComment(Token commentToken)
    {
      CommentInfo comment;
      string text = commentToken.val.Substring(2);
      if (text.EndsWith("*/")) text = text.Substring(0, text.Length - 2);
      if (text.StartsWith("*"))
      {
        text = text.Substring(1);
        comment = new XmlBlockComment(
          commentToken,
          _Parser,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      else
      {
        comment = new BlockComment(
          commentToken,
          _Parser,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      ProcessComment(commentToken, comment);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Processes the specified comment.
    /// </summary>
    /// <param name="commentToken"></param>
    /// <param name="comment"></param>
    /// <remarks>
    /// If the comment is the first token in the line, adds this comment to the ones
    /// already collected into a MultiCommentBlock. Later when a language element is 
    /// used, the comment is assigned to the appropriate element.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ProcessComment(Token commentToken, CommentInfo comment)
    {
      _Parser.File.AddComment(comment);

      // --- Check, if the comment is the first token in this line
      if (_Parser.CheckTokenIsFirstInLine(commentToken))
      {
        // --- This token is the first in this line, so attach it to the orphan comment.
        MultiCommentBlock multiBlock = _Parser.OrphanComment as MultiCommentBlock;
        if (_Parser.OrphanComment == null)
        {
          // --- This is the first part of the orphan comment.
          _Parser.OrphanComment = comment;
        }
        else
        {
          if (multiBlock == null)
          {
            // --- The orphan comment should be converted to a multi-comment block.
            multiBlock = new MultiCommentBlock(_Parser.OrphanComment);
            _Parser.OrphanComment = multiBlock;
          }
          multiBlock.AttachComment(comment);
        }
      }
      else
      {
        // --- This comment is an end-of-line comment, so attach it to the previous token.
        if (commentToken.prev != null)
        {
          comment.RelatedElement = new TokenElement(commentToken.prev, _Parser);
        }
      }
    }

    #endregion
  }
}
