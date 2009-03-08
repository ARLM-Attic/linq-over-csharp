using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstact initializer.
  /// </summary>
  // ==================================================================================
  public abstract class Initializer : LanguageElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    protected Initializer(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public virtual void ResolveTypeReferences(ResolutionContext contextType,
      ITypeDeclarationScope declarationScope,
      ITypeParameterScope parameterScope)
    {
    }

    #endregion

  }
}
