using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "const" member declaration.
  /// </summary>
  // ==================================================================================
  public sealed class ConstDeclaration : MemberDeclaration
  {
    #region Private fields

    private Expression _Expression;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "const" member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public ConstDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression defining this constant value.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Expression Expression
    {
      get { return _Expression; }
      set { _Expression = value; }
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
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      if (_Expression != null)
      {
        _Expression.ResolveTypeReferences(contextType, declarationScope, parameterScope);
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

      // --- "abstract" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@abstract) != 0)
      {
        Parser.Error0681(Token);
        Invalidate();
      }

      // --- "readonly" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@readonly) != 0)
      {
        Parser.Error0106(Token, "readonly");
        Invalidate();
      }

      // --- "volatile" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@volatile) != 0)
      {
        Parser.Error0106(Token, "volatile");
        Invalidate();
      }

      // --- "virtual" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@virtual) != 0)
      {
        Parser.Error0106(Token, "virtual");
        Invalidate();
      }

      // --- "sealed" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@sealed) != 0)
      {
        Parser.Error0106(Token, "sealed");
        Invalidate();
      }

      // --- "override" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@override) != 0)
      {
        Parser.Error0106(Token, "override");
        Invalidate();
      }

      // --- "extern" is not allowed on constants
      if ((_DeclaredModifier & Modifier.@extern) != 0)
      {
        Parser.Error0106(Token, "extern");
        Invalidate();
      }

      TypeReference typeRef = ResultingType.RightMostPart;
      if (typeRef == null) return;

      // --- Go further only if field type has been resolved
      if (!typeRef.IsResolvedToType && !typeRef.IsResolvedToTypeParameter)
      {
        Invalidate();
      }

      // --- In case if invalidity we finish the check.
      if (!IsValid) return;

      ITypeCharacteristics constType = typeRef.ResolvingType;

      // --- Resulting type must be one of the followings:
      // --- A reference type or an enum;
      if (constType.IsClass || constType.IsEnum) return;

      // --- The type byte, sbyte, short, ushort, int, uint, long, ulong, char, float, 
      // --- double, decimal, string or bool;
      if (constType.TypeObject.Equals(typeof(byte)) ||
          constType.TypeObject.Equals(typeof(sbyte)) ||
          constType.TypeObject.Equals(typeof(short)) ||
          constType.TypeObject.Equals(typeof(ushort)) ||
          constType.TypeObject.Equals(typeof(int)) ||
          constType.TypeObject.Equals(typeof(uint)) ||
          constType.TypeObject.Equals(typeof(long)) ||
          constType.TypeObject.Equals(typeof(ulong)) ||
          constType.TypeObject.Equals(typeof(char)) ||
          constType.TypeObject.Equals(typeof(float)) ||
          constType.TypeObject.Equals(typeof(double)) ||
          constType.TypeObject.Equals(typeof(decimal)) ||
          constType.TypeObject.Equals(typeof(string)) ||
          constType.TypeObject.Equals(typeof(bool)))
        return;

      // --- Const has an invalid type
      Parser.Error0283(Token, constType.FullName);
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of type declarations that can be indexed by the
  /// full name of the type.
  /// </summary>
  // ==================================================================================
  public class ConstDeclarationCollection : RestrictedIndexedCollection<ConstDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">ConstDeclaration item.</param>
    /// <returns>
    /// Name of the const declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(ConstDeclaration item)
    {
      return item.Name;
    }
  }
}
