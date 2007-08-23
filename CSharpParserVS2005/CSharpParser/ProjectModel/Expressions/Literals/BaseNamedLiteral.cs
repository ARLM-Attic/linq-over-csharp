using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a "base" literal.
  /// </summary>
  // ==================================================================================
  public class BaseNamedLiteral : BaseLiteral
  {
    #region Private fields

    private readonly TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();
    
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "base" literal.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public BaseNamedLiteral(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type arguments of the primitive method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReferenceCollection TypeArguments
    {
      get { return _TypeArguments; }
    }
    
#endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      foreach (TypeReference arg in _TypeArguments)
      {
        arg.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}