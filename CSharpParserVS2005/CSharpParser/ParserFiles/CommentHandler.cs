using CSharpParser.ProjectModel;

namespace CSharpParser.ParserFiles
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
      if (text.StartsWith("/"))
      {
        text = text.Substring(1);
        comment = new XmlComment(
          commentToken.line,
          commentToken.col,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      else
      {
        comment = new LineComment(
          commentToken.line,
          commentToken.col,
          commentToken.line,
          commentToken.col + commentToken.val.Length,
          text);
      }
      _Parser.File.AddComment(comment);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Parses the specified block comment.
    /// </summary>
    /// <param name="commentToken">Pragma token holding the comment.</param>
    // --------------------------------------------------------------------------------
    public void HandleBlockComment(Token commentToken)
    {
      
    }

    #endregion

  }
}
