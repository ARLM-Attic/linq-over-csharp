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
    private LanguageElement _ContextElement;
    private readonly CSharpSyntaxParser _Parser;
    private string _Name;
    private CommentInfo _Comment;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token. The element uses the specifed element as a context element.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="contextElement">Contex of this language element.</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token, LanguageElement contextElement)
      :
      this (token, contextElement.Parser)
    {
      _ContextElement = contextElement;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance creating this element.</param>
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
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="name">Name of the element.</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token, CSharpSyntaxParser parser, string name):
      this(token, parser)
    {
      _Name = name;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the short name of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ShortName
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the language element.
    /// </summary>
    /// <remarks>
    /// This name can be overridden.
    /// </remarks>
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the context of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement ContextElement
    {
      get { return _ContextElement; }
      internal set { _ContextElement = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element has a context or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasContextElement
    {
      get { return _ContextElement == null; }
    }

    #endregion
  }
}
