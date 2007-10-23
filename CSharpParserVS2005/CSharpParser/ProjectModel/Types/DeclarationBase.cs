using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract is the common base class of type and member declarations.
  /// </summary>
  // ==================================================================================
  public abstract class DeclarationBase : AttributedElement,
    ISupportsDocumentationComment
  {
    #region Private and protected fields

    /// <summary>Holds the declared modifiers.</summary>
    protected Modifier _DeclaredModifier;

    /// <summary>Holds the information about the explicitly declared visibility.</summary>
    protected Visibility _DeclaredVisibility;

    /// <summary>Member has the "new" modifier.</summary>
    protected bool _IsNew;

    /// <summary>Member has the "unsafe" modifier.</summary>
    protected bool _IsUnsafe;

    /// <summary>Member has the "static" modifier.</summary>
    protected bool _IsStatic;

    /// <summary>Member has the "virtual" modifier.</summary>
    protected bool _IsVirtual;

    /// <summary>Member has the "sealed" modifier.</summary>
    protected bool _IsSealed;

    /// <summary>Member has the "override" modifier.</summary>
    protected bool _IsOverride;

    /// <summary>Member has the "abstract" modifier.</summary>
    protected bool _IsAbstract;

    /// <summary>Member has the "extern" modifier.</summary>
    protected bool _IsExtern;

    /// <summary>Member has the "readonly" modifier.</summary>
    protected bool _IsReadOnly;

    /// <summary>Member has the "volatiole" modifier.</summary>
    protected bool _IsVolatile;

    // --- Documentation related information
    private DocumentationComment _DocComment;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a declaration element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    protected DeclarationBase(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
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
    protected DeclarationBase(Token token, CSharpSyntaxParser parser, string name)
      : base(token, parser, name)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declared visibility of the type declaration.
    /// </summary>
    /// <remarks>
    /// Declared visibility is the one that is explicitly declared in the source code.
    /// The effective visibility can be accessed by the Visibility property.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Visibility DeclaredVisibility
    {
      get { return _DeclaredVisibility; }
      set { _DeclaredVisibility = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the type has its default visibility (no visibility
    /// modifiers has been used for the class declaration).
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasDefaultVisibility
    {
      get { return _DeclaredVisibility == Visibility.Default; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "new" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNew
    {
      get { return _IsNew; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "unsafe" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnsafe
    {
      get { return _IsUnsafe; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "static" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsStatic
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "virtual" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVirtual
    {
      get { return _IsVirtual; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "sealed" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsSealed
    {
      get { return _IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "override" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsOverride
    {
      get { return _IsOverride; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "abstract" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsAbstract
    {
      get { return _IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "extern" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsExtern
    {
      get { return _IsExtern; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has the "readonly" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsReadOnly
    {
      get { return _IsReadOnly; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has the "volatile" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVolatile
    {
      get { return _IsVolatile; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the documentation comment belonging to the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public DocumentationComment DocumentationComment
    {
      get
      {
        if (_DocComment == null)
        {
          // --- Either the declaration or its first attribute holds the comment
          CommentInfo comment = Comment;
          if (comment == null && Attributes.Count > 0)
            comment = Attributes[0].Comment;

          // --- Extract the document information from the comment.
          if (comment == null) _DocComment = new DocumentationComment();
          else _DocComment = new DocumentationComment(comment);
        }
        return _DocComment;
      }
    }

    #endregion
  }
}
