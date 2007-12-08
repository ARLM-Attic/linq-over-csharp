using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstract list initializer.
  /// </summary>
  // ==================================================================================
  public abstract class ListInitializer : Initializer
  {
    #region Private fields

    private readonly List<Initializer> _Initializers = new List<Initializer>();
    
    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new array initializer instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public ListInitializer(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of initializers.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Initializer> Initializers
    {
      get { return _Initializers; }
    }

    #endregion

    #region Public methods

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
                                               ITypeDeclarationScope declarationScope,
                                               ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      foreach (Initializer init in _Initializers)
      {
        init.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}