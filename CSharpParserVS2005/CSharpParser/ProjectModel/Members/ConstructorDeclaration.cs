using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a constructor declaration.
  /// </summary>
  // ==================================================================================
  public sealed class ConstructorDeclaration : MethodDeclaration
  {
    #region Private fields

    private bool _HasBase;
    private bool _HasThis;
    private readonly ArgumentList _BaseArguments = new ArgumentList();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructor declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public ConstructorDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
      Name = ".ctor";
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constructor has the "base" initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBase
    {
      get { return _HasBase; }
      set { _HasBase = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this constructor has the "this" initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasThis
    {
      get { return _HasThis; }
      set { _HasThis = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base arguments of the constructor
    /// </summary>
    // --------------------------------------------------------------------------------
    public ArgumentList BaseArguments
    {
      get { return _BaseArguments; }
    }

    #endregion

    #region IUsesResolutionContext implementation

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
      base.ResolveTypeReferences(ResolutionContext.MethodDeclaration, declarationScope, this);
      foreach (Argument arg in _BaseArguments)
      {
        arg.ResolveTypeReferences(ResolutionContext.MethodDeclaration, declarationScope, this);
      }
    }

    #endregion
  
    #region Semantic checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics for the specified field declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckSemantics()
    {
      CheckGeneralMemberSemantics();

      // --- "new" modifier is not allowed on a constructor declaration.
      if (IsNew)
      {
        Parser.Error0106(Token, "new");
        Invalidate();
      }

      // --- "readonly" modifier is not allowed on a constructor declaration.
      if (IsReadOnly)
      {
        Parser.Error0106(Token, "readonly");
        Invalidate();
      }

      // --- "volatile" modifier is not allowed on a constructor declaration.
      if (IsVolatile)
      {
        Parser.Error0106(Token, "volatile");
        Invalidate();
      }

      // --- "virtual" modifier is not allowed on a constructor declaration.
      if (IsVirtual)
      {
        Parser.Error0106(Token, "virtual");
        Invalidate();
      }

      // --- "sealed" modifier is not allowed on a constructor declaration.
      if (IsSealed)
      {
        Parser.Error0106(Token, "sealed");
        Invalidate();
      }
    
      // --- "override" modifier is not allowed on a constructor declaration.
      if (IsOverride)
      {
        Parser.Error0106(Token, "override");
        Invalidate();
      }

      // --- "abstract" modifier is not allowed on a constructor declaration.
      if (IsAbstract)
      {
        Parser.Error0106(Token, "abstract");
        Invalidate();
      }

      // --- Check static constructor semantics
      if (IsStatic)
      {
        // --- Static constructor must be parameterless
        if (FormalParameters.Count > 0)
        {
          Parser.Error0132(Token, QualifiedName);
          Invalidate();
        }

        // --- Access modifiers are notallowed an static constructors
        if (!HasDefaultVisibility)
        {
          Parser.Error0515(Token, QualifiedName);
          Invalidate();
        }
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of type declarations that can be indexed by the
  /// full name of the type.
  /// </summary>
  // ==================================================================================
  public class ConstructorDeclarationCollection : 
    RestrictedIndexedCollection<ConstructorDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">ConstructorDeclaration item.</param>
    /// <returns>
    /// Signature of the constructor declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(ConstructorDeclaration item)
    {
      return item.Signature;
    }
  }
}
