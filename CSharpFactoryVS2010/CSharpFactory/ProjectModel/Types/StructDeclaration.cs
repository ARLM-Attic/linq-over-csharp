using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# class declaration
  /// </summary>
  // ==================================================================================
  public sealed class StructDeclaration: TypeDeclaration
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new struct declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    public StructDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType) : base(token, parser, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    /// <remarks>
    /// This method returns true, as structs are always implicitly sealed.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsSealed
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the default base type of this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction DefaultBaseType
    {
      get { return NetBinaryType.ValueType; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the properties using the modifiers.
    /// </summary>
    /// <param name="mod">Modifier enumeration value.</param>
    // --------------------------------------------------------------------------------
    public override void SetModifiers(Modifier mod)
    {
      base.SetModifiers(mod);

      // --- Structure members can have only private, public and internal modifiers.
      // --- protected or protected internal modifiers are not allowed.
      if (IsNested && DeclaringType is StructDeclaration)
      {
        if (_DeclaredVisibility == Visibility.Protected ||
            _DeclaredVisibility == Visibility.ProtectedInternal)
        {
          Parser.Error0106(Token, "protected");
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the type of this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected override TypeDeclaration CreateNewPart()
    {
      return new StructDeclaration(Token, Parser, DeclaringType);
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