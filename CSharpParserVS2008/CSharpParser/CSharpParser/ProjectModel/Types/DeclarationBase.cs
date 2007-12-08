using System.Collections.Generic;
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
          if (comment == null) _DocComment = DocumentationComment.EmptyComment;
          else _DocComment = new DocumentationComment(comment);
        }
        return _DocComment;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the members of of this language element that are also can be documented.
    /// </summary>
    /// <remarks>
    /// This implementation does not return any documentable members.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual IEnumerable<ISupportsDocumentationComment> DocumentableMembers
    {
      get { yield break; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the documentation comment belonging to the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void ProcessDocumentationComment()
    {
      // --- Check for missing documentation comment
      if (DocumentationComment.IsEmpty)
      {
        // REFACTOR: Refactor this later
        TypeDeclaration typeDef = this as TypeDeclaration;
        MemberDeclaration memberDef = this as MemberDeclaration;
        if (typeDef != null && typeDef.Visibility != Visibility.Private ||
          memberDef != null && memberDef.Visibility != Visibility.Private)
        {
          Parser.Warning1591(Token, FullName);
        }
        return;
      }

      // --- Check XML formatting
      if (!DocumentationComment.IsWellFormedXml)
      {
        Parser.Warning1570(DocumentationComment.OriginalComment.Token,
          FullName, DocumentationComment.BadlyFormedReason);
        return;
      }

      // --- Check for missing or duplicate "param" tags
      if (DocumentationComment.Parameters.Count > 0)
      {
        foreach (FormalParameter param in FormalParameters)
        {
          int count = 0;
          foreach (ParamTag paramTag in DocumentationComment.Parameters)
          {
            if (param.Name.Equals(paramTag.Name)) count++;
            if (count > 1)
              Parser.Warning1571(param.Token, Signature, param.Name);
          }
          if (count == 0)
            Parser.Warning1573(param.Token, Signature, param.Name);
        }

        // --- Check for "param" tags without matching formal parameters
        foreach (ParamTag paramTag in DocumentationComment.Parameters)
        {
          if (!string.IsNullOrEmpty(paramTag.Name))
          {
            bool found = false;
            foreach (FormalParameter param in FormalParameters)
            {
              if (param.Name.Equals(paramTag.Name))
              {
                found = true;
                break;
              }
            }
            if (!found)
              Parser.Warning1572(Token, Signature, paramTag.Name);
          }
        }
      }

      // --- Check for missing or duplicate "typeparam" tags
      if (DocumentationComment.TypeParameters.Count > 0)
      {
        foreach (TypeParameter param in TypeParameters)
        {
          int count = 0;
          foreach (TypeParamTag paramTag in DocumentationComment.TypeParameters)
          {
            if (param.Name.Equals(paramTag.Name)) count++;
            if (count > 1)
              Parser.Warning1710(param.Token, Signature, param.Name);
          }
          if (count == 0)
            Parser.Warning1712(param.Token, Signature, param.Name);
        }

        // --- Check for "param" tags without matching formal parameters
        foreach (TypeParamTag paramTag in DocumentationComment.TypeParameters)
        {
          if (!string.IsNullOrEmpty(paramTag.Name))
          {
            bool found = false;
            foreach (TypeParameter param in TypeParameters)
            {
              if (param.Name.Equals(paramTag.Name))
              {
                found = true;
                break;
              }
            }
            if (!found)
              Parser.Warning1711(Token, Signature, paramTag.Name);
          }
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of formal parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual FormalParameterCollection FormalParameters
    {
      get { return new FormalParameterCollection(); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual TypeParameterCollection TypeParameters
    {
      get { return new TypeParameterCollection(); }
    }

    #endregion
  }
}
