using CSharpParser.Collections;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class a C# comment.
  /// </summary>
  // ==================================================================================
  public abstract class CommentInfo
  {
    #region Private fields

    private readonly int _StartLine;
    private readonly int _StartColumn;
    private readonly int _EndLine;
    private readonly int _EndColumn;
    private readonly string _Text;
    private LanguageElement _RelatedElement;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new comment instance.
    /// </summary>
    /// <param name="_StartLine">Starting line.</param>
    /// <param name="_StartColumn">Starting column.</param>
    /// <param name="_EndLine">Ending line.</param>
    /// <param name="_EndColumn">Ending column.</param>
    /// <param name="_Text">Comment text.</param>
    // --------------------------------------------------------------------------------
    protected CommentInfo(int _StartLine, int _StartColumn, int _EndLine, int _EndColumn, 
      string _Text)
    {
      this._StartLine = _StartLine;
      this._StartColumn = _StartColumn;
      this._EndLine = _EndLine;
      this._EndColumn = _EndColumn;
      this._Text = _Text;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the line where #region directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartLine
    {
      get { return _StartLine; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the column where #region directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return _StartColumn; }
    }

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
    public string Text
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
    /// <param name="_StartLine">Starting line.</param>
    /// <param name="_StartColumn">Starting column.</param>
    /// <param name="_EndLine">Ending line.</param>
    /// <param name="_EndColumn">Ending column.</param>
    /// <param name="_Text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public LineComment(int _StartLine, int _StartColumn, int _EndLine, int _EndColumn, 
      string _Text) : base(_StartLine, _StartColumn, _EndLine, _EndColumn, _Text)
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
    /// <param name="_StartLine">Starting line.</param>
    /// <param name="_StartColumn">Starting column.</param>
    /// <param name="_EndLine">Ending line.</param>
    /// <param name="_EndColumn">Ending column.</param>
    /// <param name="_Text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public BlockComment(int _StartLine, int _StartColumn, int _EndLine, int _EndColumn, 
      string _Text) : base(_StartLine, _StartColumn, _EndLine, _EndColumn, _Text)
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
    /// <param name="_StartLine">Starting line.</param>
    /// <param name="_StartColumn">Starting column.</param>
    /// <param name="_EndLine">Ending line.</param>
    /// <param name="_EndColumn">Ending column.</param>
    /// <param name="_Text">Comment text.</param>
    // --------------------------------------------------------------------------------
    public XmlComment(int _StartLine, int _StartColumn, int _EndLine, int _EndColumn, 
      string _Text) : base(_StartLine, _StartColumn, _EndLine, _EndColumn, _Text)
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