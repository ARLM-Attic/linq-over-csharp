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
    private bool _IsValid;
    private CommentInfo _Comment;
    private Token _TerminatingToken;

    /// <summary>Language element related to this comment.</summary>
    private LanguageElement _RelatedElement;

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
      : this (token, contextElement.Parser)
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
      _TerminatingToken = token;
      _Name = token.val;
      _Parser = parser;
      _IsValid = true;
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
    /// Gets the simple (short) name of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string SimpleName
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
    /// Gets the full name (fully qualified name) of the language element.
    /// </summary>
    /// <remarks>
    /// The default implementation retrieves the name but it can be overridden
    /// by derived language element types.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual string FullName
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the element as the CLR uses it after compilation
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example, for generic types the name is like: List`1, Dictionary`2
    /// </para>
    /// <para>
    /// The default implementation retrieves the name but it can be overridden
    /// by derived language element types.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual string ClrName
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the element where the actual type parameters are used.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default implementation retrieves the name but it can be overridden
    /// by derived language element types.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual string ParametrizedName
    {
      get { return _Name; }
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
    /// Gets the ending line number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndLine
    {
      get { return _TerminatingToken.line; }  
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
    /// Gets the ending column number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndColumn
    {
      get { return _TerminatingToken.col + _TerminatingToken.val.Length - 1; }
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
    /// Gets the ending position of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndPosition
    {
      get { return _TerminatingToken.pos + _TerminatingToken.val.Length - 1; }
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
    /// Gets the terminating token of this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Token TerminatingToken
    {
      get { return _TerminatingToken; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the comment belonging to this language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CommentInfo Comment
    {
      get { return _Comment; }
      set
      {
        _Comment = value;
        if (value == null) return;

        _Comment.RelatedElement = this;

        // --- Check documentation comments
        ISupportsDocumentationComment commentedElement = this as ISupportsDocumentationComment;
        if (commentedElement == null)
        {
          // --- Documentation comment is probably not allowed here
          if (value.HasDocumentation)
          {
            Parser.CompilationUnit.AddCommentToFix(value);
          }
        }
      }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this language element is valid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValid
    {
      get { return _IsValid; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    /// <remarks>By default, it is the full name of the member.</remarks>
    // --------------------------------------------------------------------------------
    public virtual string Signature
    {
      get { return FullName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the language element related to this element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement RelatedElement
    {
      get { return _RelatedElement; }
      set { _RelatedElement = value; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is valid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Validate()
    {
      _IsValid = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the validity of this language element.
    /// </summary>
    /// <param name="isValid">True, if the language element is valid.</param>
    // --------------------------------------------------------------------------------
    public void Validate(bool isValid)
    {
      _IsValid = isValid;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this language element is invalid in its context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Invalidate()
    {
      _IsValid = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Marks the termination of this language element.
    /// </summary>
    /// <param name="token">Terminating token</param>
    // --------------------------------------------------------------------------------
    public void Terminate(Token token)
    {
      _TerminatingToken = token;
    }

    #endregion

    #region Overridden members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // --------------------------------------------------------------------------------
    public override string ToString()
    {
      return FullName;
    }

    #endregion
  }
}
