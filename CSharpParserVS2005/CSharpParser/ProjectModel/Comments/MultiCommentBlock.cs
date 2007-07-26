using System.Text;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a comment block compoesd from multiple line comments.
  /// </summary>
  // ==================================================================================
  public sealed class MultiCommentBlock : CommentInfo
  {
    #region Private fields

    private readonly CommentInfoCollection _Comments = new CommentInfoCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public MultiCommentBlock(Token token, int endLine, int endColumn, string text)
      : 
      base(token, endLine, endColumn, text)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new multi-block comment and initializes with the specified comment.
    /// </summary>
    /// <param name="comment">Comment used to initialize this block.</param>
    // --------------------------------------------------------------------------------
    public MultiCommentBlock(CommentInfo comment)
      :
      base (comment.Token, comment.EndLine, comment.EndColumn, comment.Text)
    {
      AttachComment(comment);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the comments involved in this multi-comment block.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfoCollection Comments
    {
      get { return _Comments; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text of the comment excluding the comment mark tokens.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Text
    {
      get
      {
        StringBuilder sb = new StringBuilder(1024);
        foreach (CommentInfo ci in _Comments)
        {
          if (sb.Length > 0) sb.Append("/r");
          sb.Append(ci.Text);
        }
        return sb.ToString();
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Attaches a new comment to this comment block.
    /// </summary>
    /// <param name="comment">Comment to attach to the block.</param>
    /// <remarks>
    /// The end line and columns are modified according to the newly attached comment.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void AttachComment(CommentInfo comment)
    {
      _Comments.Add(comment);
      if (comment.StartLine > _EndLine)
      {
        _EndLine = comment.EndLine;
        _EndColumn = comment.EndColumn;
      }
    }

    #endregion
  }
}