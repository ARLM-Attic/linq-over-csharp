using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a property member declaration.
  /// </summary>
  // ==================================================================================
  public class PropertyDeclaration : MemberDeclaration
  {
    #region Private fields

    private AccessorDeclaration _Getter;
    private AccessorDeclaration _Setter;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new property member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public PropertyDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a getter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasGetter
    {
      get { return _Getter != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a setter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSetter
    {
      get { return _Setter != null; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "get" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Getter
    {
      get { return _Getter; }
      set { _Getter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "set" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Setter
    {
      get { return _Setter; }
      set { _Setter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method signature reserved by the "get" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ReservedGetSignature
    {
      get
      {
        return string.Format("get_{0}()", SimpleName);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method signature reserved by the "set" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ReservedSetSignature
    {
      get
      {
        string typeName = ResultingType.RightMostPart.IsResolvedToType
                            ? ResultingType.RightMostPart.ResolvingType.FullName
                            : (ResultingType.RightMostPart.IsResolvedToTypeParameter
                                 ? ResultingType.RightMostPart.ResolvingTypeParameter.Name
                                 : "");
        return string.Format("set_{0}({1})", SimpleName, typeName);
      }
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
      if (_Getter != null)
      {
        _Getter.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      if (_Setter != null)
      {
        _Setter.ResolveTypeReferences(contextType, declarationScope, parameterScope);
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
      CheckMethodModifiers();

      // --- Check for the reserved method names (get_<Property>, set_<Property>)
      TypeReference type = ResultingType.RightMostPart;
      if (IsValid && (type.IsResolvedToType || type.IsResolvedToTypeParameter))
      {
        MethodDeclaration method;
        if (DeclaringType.Methods.TryGetValue(ReservedGetSignature, out method))
          Parser.Error0111(method.Token, ParametrizedName, method.Signature);
        if (DeclaringType.Methods.TryGetValue(ReservedSetSignature, out method))
          Parser.Error0111(method.Token, ParametrizedName, method.Signature);
      }

      // --- Check accessor modifiers
      if (_Getter != null) _Getter.CheckSemantics();
      if (_Setter != null) _Setter.CheckSemantics();

      // --- Check accessibility modifiers
      bool bothAccessorsExists = _Getter != null && _Setter != null;
      bool getHasModifier = _Getter != null && !_Getter.HasDefaultVisibility;
      bool setHasModifier = _Setter != null && !_Setter.HasDefaultVisibility;
      if (IsOverride)
      {
        // --- For a property or indexer that includes an override modifier, the 
        // --- accessor-modifier of an accessor shall match the accessor-modifier, 
        // --- if any, of the accessor being overridden.
      }
      else
      {
        // --- For a property or indexer that has no override modifier, an accessor-modifier 
        // --- is permitted only if the property or indexer has both a get and set accessor, 
        // --- and then is permitted only on one of those accessors.
        if (getHasModifier && !bothAccessorsExists)
          Parser.Error0276(_Getter.Token, QualifiedName);
        else if (setHasModifier && !bothAccessorsExists)
          Parser.Error0276(_Setter.Token, QualifiedName);
        else if (getHasModifier && setHasModifier)
          Parser.Error0274(Token, QualifiedName);
      }

      // --- The accessor-modifier shall declare an accessibility that is strictly more 
      // --- restrictive than the declared accessibility of the property or indexer itself.
      AccessorDeclaration toCheck = null;
      if (getHasModifier && !setHasModifier) toCheck = _Getter;
      if (setHasModifier && !getHasModifier) toCheck = _Setter;
      if (!DeclaringType.IsInterface && toCheck != null)
      {
        bool moreRestrictive = false;
        switch (Visibility)
        {
          case Visibility.Public:
            moreRestrictive = toCheck.Visibility != Visibility.Public;
            break;
          case Visibility.ProtectedInternal:
            moreRestrictive = toCheck.Visibility == Visibility.Internal ||
                              toCheck.Visibility == Visibility.Protected ||
                              toCheck.Visibility == Visibility.Private;
            break;
          case Visibility.Internal:
          case Visibility.Protected:
            moreRestrictive = toCheck.Visibility == Visibility.Private;
            break;
        }
        if (!moreRestrictive)
        {
          Parser.Error0273(toCheck.Token, toCheck.QualifiedName, QualifiedName);
        }
      }

      // --- No more checks, if the resulting type is not resolved.
      if (!ResultingType.RightMostPart.IsResolvedToType) return;

      // --- Property cannot return void
      if (ResultingType.RightMostPart.ResolvingType.TypeObject == typeof(void))
      {
        Parser.Error0547(Token, Name);
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
  public class PropertyDeclarationCollection :
    RestrictedIndexedCollection<PropertyDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">PropertyDeclaration item.</param>
    /// <returns>
    /// Name of the property declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(PropertyDeclaration item)
    {
      return item.Signature;
    }
  }
}
