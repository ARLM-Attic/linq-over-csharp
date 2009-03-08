using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# class declaration
  /// </summary>
  // ==================================================================================
  public sealed class ClassDeclaration: TypeDeclaration
  {
    #region Private fields

    private FinalizerDeclaration _Finalizer;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new class declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    public ClassDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType) : base(token, parser, declaringType)
    {
    } 

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the finalizer of this type declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public FinalizerDeclaration Finalizer
    {
      get { return _Finalizer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the type has a finalizer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasFinalizer
    {
      get { return _Finalizer != null; }
    }

    #endregion

    #region Internal methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this class has already a finalizer or not.
    /// </summary>
    /// <param name="finalizer">Finalizer declared in the class.</param>
    /// <remarks>
    /// Raises a compilation error, if the class already has a finalizer.
    /// </remarks>
    // --------------------------------------------------------------------------------
    internal bool HasAlreadyFinalizer(FinalizerDeclaration finalizer)
    {
      if (HasFinalizer)
      {
        Parser.Error0111(finalizer.Token, Name, finalizer.Signature);
        return true;
      }
      _Finalizer = finalizer;
      return false;
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the type of this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected override TypeDeclaration CreateNewPart()
    {
      return new ClassDeclaration(Token, Parser, DeclaringType);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clones this type declaration into a new instance.
    /// </summary>
    /// <returns>
    /// The new cloned instance.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override TypeDeclaration CloneToPart()
    {
      ClassDeclaration clone = base.CloneToPart() as ClassDeclaration;
      clone._Finalizer = _Finalizer;
      return clone;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if type declaration matches with the declaration rules.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckTypeDeclaration()
    {
      base.CheckTypeDeclaration();

      // --- Abstract classes cannot be sealed or static
      if (IsAbstract && (IsStatic || IsSealed)) Parser.Error0418(Token, Name);

      // --- Check static classes
      if (IsStatic)
      {
        // --- Static classes cannot be sealed
        if (IsSealed) Parser.Error0441(Token, Name);

        // --- Static clases must derive from System.Object
        if (BaseType != null && !BaseType.Equals(NetBinaryType.Object))
        {
          Parser.Error0713(Token, Name, BaseType.Name);
        }

        // --- Static classes must not have interface list
        if (InterfaceList.Count > 0)
        {
          Parser.Error0714(InterfaceList[0].Token, Name);
        }
      }
    }

    #endregion
  }
}
