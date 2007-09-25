using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a field member declaration.
  /// </summary>
  // ==================================================================================
  public class FieldDeclaration : MemberDeclaration, IVariableInfo
  {
    #region Private fields

    private Initializer _Initializer;
    private bool _IsEvent;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new field member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public FieldDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has an initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return _Initializer != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field is an event field or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEvent
    {
      get { return _IsEvent; }
      set { _IsEvent = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of this field.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer
    {
      get { return _Initializer; }
      set { _Initializer = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the category of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableCategory Category
    {
      get { return _IsStatic ? VariableCategory.Static : VariableCategory.Instance; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInitiallyAssigned
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int DeclarationPosition
    {
      get { return Token.pos; }
    }

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
      if (_Initializer != null)
      {
        _Initializer.ResolveTypeReferences(contextType, declarationScope, parameterScope);
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
      base.CheckSemantics();

      // --- "extern" is not allowed on fields
      if ((_DeclaredModifier & Modifier.@extern) != 0)
      {
        Parser.Error0106(Token, "extern");
        Invalidate();        
      }

      // --- "override" is not allowed on fields
      if ((_DeclaredModifier & Modifier.@override) != 0)
      {
        Parser.Error0106(Token, "override");
        Invalidate();
      }

      // --- "abstract" is not allowed on fields
      if ((_DeclaredModifier & Modifier.@abstract) != 0)
      {
        Parser.Error0681(Token);
        Invalidate();
      }

      // --- Field can be "readonly" or "volatile" but not both
      if ((_DeclaredModifier & Modifier.@readonly) != 0 &&
        (_DeclaredModifier & Modifier.@volatile) != 0)
      {
        Parser.Error0678(Token, QualifiedName);
        Invalidate();
      }

      // --- Go further only if field type has been resolved
      if (!ResultingType.IsResolvedToType && !ResultingType.IsResolvedToTypeParameter)
      {
        Invalidate();
      }

      // --- In case if invalidity we finish the check.
      if (!IsValid) return;

      // --- Check the resulting type of volatile fields
      if (IsVolatile)
      {
        // --- A type-parameter that is known to be a reference type;
        if (ResultingType.IsResolvedToTypeParameter)
        {
          TypeParameter param = ResultingType.ResolvingTypeParameter;
          if (param.HasConstraint && param.Constraint.HasPrimary) 
          {
            if (param.Constraint.Primary.IsClass ||
            (param.Constraint.Primary.IsType && 
            param.Constraint.Primary.Type.RightMostPart.IsClass))
              return;
          }

          // --- Type parameter is unconstrained or not a reference type.
          Parser.Error0677(ResultingType.Token, QualifiedName, ResultingType.FullName);
          return;
        }

        ITypeCharacteristics fieldType = ResultingType.ResolvingType;

        // --- Resulting type must be one of the followings:
        // --- A reference type;
        if (fieldType.IsClass) return;

        // --- The type byte, sbyte, short, ushort, int, uint, char, float, or bool;
        if (fieldType.TypeObject.Equals(typeof(byte)) ||
            fieldType.TypeObject.Equals(typeof(sbyte)) ||
            fieldType.TypeObject.Equals(typeof(short)) ||
            fieldType.TypeObject.Equals(typeof(ushort)) ||            
            fieldType.TypeObject.Equals(typeof(int)) ||
            fieldType.TypeObject.Equals(typeof(uint)) ||
            fieldType.TypeObject.Equals(typeof(char)) ||
            fieldType.TypeObject.Equals(typeof(float)) ||
            fieldType.TypeObject.Equals(typeof(bool)))
          return;

        // --- An enum-type having an enum base type of byte, sbyte, short, 
        // --- ushort, int, or uint.
        if (fieldType.IsEnum)
        {
          ITypeCharacteristics ulType = fieldType.GetUnderlyingEnumType();
        if (ulType.TypeObject.Equals(typeof(byte)) ||
            ulType.TypeObject.Equals(typeof(sbyte)) ||
            ulType.TypeObject.Equals(typeof(short)) ||
            ulType.TypeObject.Equals(typeof(ushort)) ||            
            ulType.TypeObject.Equals(typeof(int)) ||
            ulType.TypeObject.Equals(typeof(uint)))
          return;
        }

        // --- Volatile filed is an unallowed type
        Parser.Error0677(ResultingType.Token, QualifiedName, ResultingType.FullName);
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
  public class FieldDeclarationCollection : RestrictedIndexedCollection<FieldDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">FieldDeclaration item.</param>
    /// <returns>
    /// Name of the field declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(FieldDeclaration item)
    {
      return item.Name;
    }
  }
}
