using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is the common base class of all language elements having attributes.
  /// </summary>
  // ==================================================================================
  public abstract class AttributedElement : LanguageElement, IUsesResolutionContext
  {
    #region Private fields

    private AttributeCollection _Attributes = new AttributeCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    protected AttributedElement(Token token, CSharpSyntaxParser parser)
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
    protected AttributedElement(Token token, CSharpSyntaxParser parser, string name)
      : base(token, parser, name)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or the collection of attributes belonging to this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public AttributeCollection Attributes
    {
      get { return _Attributes; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets or the collection of attributes belonging to this type declaration.
    /// </summary>
    /// <param name="attrs">Attributes to assign</param>
    // --------------------------------------------------------------------------------
    public void AssignAttributes(AttributeCollection attrs)
    {
      if (attrs == null)
      {
        (_Attributes as IList<AttributeDeclaration>).Clear();
      }
      else
      {
        _Attributes = attrs;
      }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public virtual void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      // --- Resolve attributes
      foreach (AttributeDeclaration attr in Attributes)
      {
        attr.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}