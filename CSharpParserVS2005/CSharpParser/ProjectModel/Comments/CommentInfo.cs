using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class a C# comment.
  /// </summary>
  // ==================================================================================
  public abstract class CommentInfo: LanguageElement
  {
    #region Private fields

    protected int _StartLine;
    protected int _StartColumn;
    protected int _EndLine;
    protected int _EndColumn;
    protected string _Text;
    private LanguageElement _RelatedElement;

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
    protected CommentInfo(Token token, CSharpSyntaxParser parser, int endLine, 
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
    public int EndLine
    {
      get { return _EndLine; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the column where #endregion directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndColumn
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
    /// Gets or sets the language element related to this comment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement RelatedElement
    {
      get { return _RelatedElement; }
      set { _RelatedElement = value; }
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
    public LineComment(Token token, CSharpSyntaxParser parser, int endLine, 
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
    public BlockComment(Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text)
      : base(token, parser, endLine, endColumn, text)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a C# XML comment.
  /// </summary>
  // ==================================================================================
  public sealed class XmlComment : CommentInfo
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
    public XmlComment(Token token, CSharpSyntaxParser parser, int endLine, 
      int endColumn, string text)
      : base(token, parser, endLine, endColumn, text)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of comment information.
  /// </summary>
  // ==================================================================================
  public class CommentInfoCollection : RestrictedCollection<CommentInfo>
  {
  }
}