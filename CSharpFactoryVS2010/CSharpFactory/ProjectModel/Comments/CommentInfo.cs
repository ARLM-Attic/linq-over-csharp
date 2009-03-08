using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpSyntaxParser=CSharpFactory.ParserFiles.CSharpSyntaxParser;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class a C# comment.
  /// </summary>
  // ==================================================================================
  public abstract class CommentInfo: LanguageElement
  {
    #region Private fields

    /// <summary>Start line position of this comment.</summary>
    protected int _StartLine;
    /// <summary>Start column position of this comment.</summary>
    protected int _StartColumn;
    /// <summary>Ending line position of this comment.</summary>
    protected int _EndLine;
    /// <summary>Ending column position of this comment.</summary>
    protected int _EndColumn;
    /// <summary>Text information within this comment.</summary>
    protected string _Text;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    protected CommentInfo(ParserFiles.Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text): base(token, parser)
    {
      _EndLine = endLine;
      _EndColumn = endColumn;
      _Text = text;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the line where #endregion directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public new int EndLine
    {
      get { return _EndLine; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the column where #endregion directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public new int EndColumn
    {
      get { return _EndColumn; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text of the comment excluding the comment mark tokens.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string Text
    {
      get { return _Text; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this comment has a documentation comment within
    /// its body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool HasDocumentation
    {
      get { return false; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class a C# line comment.
  /// </summary>
  // ==================================================================================
  public sealed class LineComment : CommentInfo
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public LineComment(ParserFiles.Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text)
      : 
      base(token, parser, endLine, endColumn, text)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class a C# block comment.
  /// </summary>
  // ==================================================================================
  public sealed class BlockComment : CommentInfo
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public BlockComment(ParserFiles.Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text)
      : base(token, parser, endLine, endColumn, text)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a C# XML comment line.
  /// </summary>
  // ==================================================================================
  public sealed class XmlCommentLine : CommentInfo
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public XmlCommentLine(ParserFiles.Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text)
      : base(token, parser, endLine, endColumn, text)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this comment has a documentation comment within
    /// its body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool HasDocumentation
    {
      get { return true; }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a C# XML block comment.
  /// </summary>
  // ==================================================================================
  public sealed class XmlBlockComment : CommentInfo
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="token">Start token of the comment.</param>
    /// <param name="parser">Parser used by the comment</param>
    /// <param name="endLine">Ending line.</param>
    /// <param name="endColumn">Ending column.</param>
    /// <param name="text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public XmlBlockComment(Token token, CSharpSyntaxParser parser, int endLine,
      int endColumn, string text)
      : base(token, parser, endLine, endColumn, text)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this comment has a documentation comment within
    /// its body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool HasDocumentation
    {
      get { return true; }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of comment information.
  /// </summary>
  // ==================================================================================
  public class CommentInfoCollection : ImmutableCollection<CommentInfo>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this block has a documentation comment within
    /// its body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasDocumentation
    {
      get
      {
        foreach (CommentInfo comment in this)
          if (comment.HasDocumentation) return true;
        return false;
      }
    }
  }
}