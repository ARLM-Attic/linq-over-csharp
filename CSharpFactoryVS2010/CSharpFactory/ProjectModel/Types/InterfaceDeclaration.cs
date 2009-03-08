using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# interface declaration
  /// </summary>
  // ==================================================================================
  public sealed class InterfaceDeclaration: TypeDeclaration
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new interface declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    public InterfaceDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType)
      : base(token, parser, declaringType)
    {
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
      return new InterfaceDeclaration(Token, Parser, DeclaringType);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if type declaration matches with the declaration rules.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckTypeDeclaration()
    {
      base.CheckTypeDeclaration();
      CheckUnallowedNonClassModifiers();
    }

    #endregion
  }
}