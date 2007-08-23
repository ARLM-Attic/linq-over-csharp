using System;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an argument of an attribute declaration.
  /// </summary>
  // ==================================================================================
  public sealed class AttributeArgument : LanguageElement, IUsesResolutionContext
  {
    #region Private fields

    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new argument.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public AttributeArgument(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the argument.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this is a named attribute argument.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasNamedArgument
    {
      get { return !String.IsNullOrEmpty(Name); }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}
