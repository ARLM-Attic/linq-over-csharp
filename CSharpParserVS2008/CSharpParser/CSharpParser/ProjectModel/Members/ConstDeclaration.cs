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

      TypeReference typeRef = ResultingType.Tail;
      if (typeRef == null) return;

      // --- Go further only if field type has been resolved
      if (!typeRef.IsResolvedToType && !typeRef.IsResolvedToTypeParameter)
      {
        Invalidate();
      }

      // --- In case if invalidity we finish the check.
      if (!IsValid) return;

      ITypeAbstraction constType = typeRef.TypeInstance;

      // --- Resulting type must be one of the followings:
      // --- A reference type or an enum;
      if (constType.IsClass || constType.IsEnum) return;

      // --- The type byte, sbyte, short, ushort, int, uint, long, ulong, char, float, 
      // --- double, decimal, string or bool;
      if (TypeBase.IsSame(constType, typeof(byte)) ||
          TypeBase.IsSame(constType, typeof(sbyte)) ||
          TypeBase.IsSame(constType, typeof(short)) ||
          TypeBase.IsSame(constType, typeof(ushort)) ||
          TypeBase.IsSame(constType, typeof(int)) ||
          TypeBase.IsSame(constType, typeof(uint)) ||
          TypeBase.IsSame(constType, typeof(long)) ||
          TypeBase.IsSame(constType, typeof(ulong)) ||
          TypeBase.IsSame(constType, typeof(char)) ||
          TypeBase.IsSame(constType, typeof(float)) ||
          TypeBase.IsSame(constType, typeof(double)) ||
          TypeBase.IsSame(constType, typeof(decimal)) ||
          TypeBase.IsSame(constType, typeof(string)) ||
          TypeBase.IsSame(constType, typeof(bool)))
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
