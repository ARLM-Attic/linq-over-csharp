using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a destructor declaration.
  /// </summary>
  // ==================================================================================
  public sealed class FinalizerDeclaration : MethodDeclaration
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new destructor declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public FinalizerDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the finalizer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get { return "~" + base.Name; }
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

      // --- "new" modifier is not allowed on a destructor declaration.
      if (IsNew)
      {
        Parser.Error0106(Token, "new");
        Invalidate();
      }

      // --- "readonly" modifier is not allowed on a destructor declaration.
      if (IsReadOnly)
      {
        Parser.Error0106(Token, "readonly");
        Invalidate();
      }

      // --- "volatile" modifier is not allowed on a destructor declaration.
      if (IsVolatile)
      {
        Parser.Error0106(Token, "volatile");
        Invalidate();
      }

      // --- "virtual" modifier is not allowed on a destructor declaration.
      if (IsVirtual)
      {
        Parser.Error0106(Token, "virtual");
        Invalidate();
      }

      // --- "sealed" modifier is not allowed on a destructor declaration.
      if (IsSealed)
      {
        Parser.Error0106(Token, "sealed");
        Invalidate();
      }

      // --- "override" modifier is not allowed on a destructor declaration.
      if (IsOverride)
      {
        Parser.Error0106(Token, "override");
        Invalidate();
      }

      // --- "abstract" modifier is not allowed on a destructor declaration.
      if (IsAbstract)
      {
        Parser.Error0106(Token, "abstract");
        Invalidate();
      }

      // --- "static" modifier is not allowed on a destructor declaration.
      if (IsStatic)
      {
        Parser.Error0106(Token, "static");
        Invalidate();
      }

      // --- Access modifiers are not allowed
      if (!HasDefaultVisibility)
      {
        Parser.Error0106(Token, DeclaredVisibility.ToString().ToLower());
        Invalidate();
      }
    }

    #endregion

  }
}