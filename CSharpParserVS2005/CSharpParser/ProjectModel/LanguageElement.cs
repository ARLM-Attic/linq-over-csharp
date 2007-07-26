using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is the common base class of all language element descriptors.
  /// </summary>
  // ==================================================================================
  public abstract class LanguageElement
  {
    #region Private fields

    private readonly Token _Token;
    private readonly CSharpSyntaxParser _Parser;
    private string _Name;
    private CommentInfo _Comment;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token)
    {
      _Token = token;
      _Name = token.val;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token, CSharpSyntaxParser parser)
    {
      _Token = token;
      _Name = token.val;
      _Parser = parser;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token and name.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="name">Language element name</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token, string name)
    {
      _Token = token;
      _Name = name;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string Name
    {
      get { return _Name; }
      set { _Name = value; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartLine
    {
      get { return _Token.line; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return _Token.col; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartPosition
    {
      get { return _Token.pos; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CSharpSyntaxParser Parser
    {
      get { return _Parser; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the starting token of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Token Token
    {
      get { return _Token; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the comment belonging to this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfo Comment
    {
      get { return _Comment; }
      set { _Comment = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element has a comment or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasComment
    {
      get { return _Comment == null; }
    }

    #endregion
  }
}
