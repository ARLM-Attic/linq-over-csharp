using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a type member declaration.
  /// </summary>
  // ==================================================================================
  public abstract class MemberDeclaration : DeclarationBase
  {
    #region Private fields

    // --- Fields describing the main characteristics of the member
    private readonly TypeDeclaration _DeclaringType;
    private TypeReference _ResultingType;

    /// <summary>
    /// Explicit name of this member (when member is an explicit interface member declaration).
    /// </summary>
    protected TypeReference _ExplicitName;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    protected MemberDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType.Parser)
    {
      _DeclaringType = declaringType;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaring this member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration DeclaringType
    {
      get { return _DeclaringType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the effective visibility of the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Visibility Visibility
    {
      get {
        return HasDefaultVisibility
                 ? (_DeclaringType.IsClass || _DeclaringType.IsStruct
                      ? Visibility.Private
                      : Visibility.Public)
                 : _DeclaredVisibility;
        }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resulting type of this member declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference ResultingType
    {
      get { return _ResultingType; }
      set { _ResultingType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the explicit property name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual TypeReference ExplicitName
    {
      get { return _ExplicitName; }
      set
      {
        _ExplicitName = value;
        Name = value.TailName;
        value.Tail.ResolveToName();
        if (value.HasSuffix)
        {
          _ExplicitName = value;
          value.Tail.Prefix.Suffix = null;
        }
        else
        {
          _ExplicitName = null;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string FullName
    {
      get { return _ExplicitName == null 
        ? Name 
        : _ExplicitName.FullName + "." + Name;  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualified name of this member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string QualifiedName
    {
      get { return _DeclaringType.Name + "." + FullName; }
    }

    #endregion

    #region Public methods 

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the properties using the modifiers.
    /// </summary>
    /// <param name="mod">Modifier enumeration value.</param>
    // --------------------------------------------------------------------------------
    public void SetModifiers(Modifier mod)
    {
      // --- Store modifiers for later check
      _DeclaredModifier = mod;

      // --- Set visibility
      Modifier visOnly = mod & Modifier.VisibilityAccessors;
      if (visOnly == 0) _DeclaredVisibility = Visibility.Default;
      else if (visOnly == Modifier.@private) _DeclaredVisibility = Visibility.Private;
      else if (visOnly == Modifier.@protected) _DeclaredVisibility = Visibility.Protected;
      else if (visOnly == Modifier.@public) _DeclaredVisibility = Visibility.Public;
      else if (visOnly == Modifier.@internal) _DeclaredVisibility = Visibility.Internal;
      else if (visOnly == Modifier.ProtectedInternal) _DeclaredVisibility = Visibility.ProtectedInternal;
      else
      {
        // --- An invalid combination of access modifiers is used
        Parser.Error0107(Token);
      }

      // --- Set other modifiers
      _IsNew = (mod & Modifier.@new) != 0;
      _IsUnsafe = (mod & Modifier.@unsafe) != 0;
      _IsStatic = (mod & Modifier.@static) != 0;
      _IsVirtual = (mod & Modifier.@virtual) != 0;
      _IsSealed = (mod & Modifier.@sealed) != 0;
      _IsOverride = (mod & Modifier.@override) != 0;
      _IsAbstract = (mod & Modifier.@abstract) != 0;
      _IsExtern = (mod & Modifier.@extern) != 0;
      _IsReadOnly = (mod & Modifier.@readonly) != 0;
      _IsVolatile = (mod & Modifier.@volatile) != 0;
      AfterSetModifiers();

      // --- Check visibility regulations of members
      if (_DeclaringType is StructDeclaration)
      {
        // --- Structure members can have only private, public and internal modifiers.
        // --- protected or protected internal modifiers are not allowed.
        if (_DeclaredVisibility == Visibility.Protected ||
            _DeclaredVisibility == Visibility.ProtectedInternal)
        {
          Parser.Error0106(Token, "protected");
        }
      }
      else if (!(this is AccessorDeclaration) &&
        _DeclaringType.IsInterface || _DeclaringType.IsEnum)
      {
        // --- Interface and enum members cannot have visibility modifiers.
        if (!HasDefaultVisibility)
        {
          Parser.Error0106(Token, _DeclaredVisibility.ToString().ToLower());
        }
      }
    }

    #endregion

    #region Virtual properties and methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    /// <remarks>By default, it is the full name of the member.</remarks>
    // --------------------------------------------------------------------------------
    public virtual string Signature
    {
      get { return FullName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This method is to override by derived classes to be able to responde the member
    /// modifiers set.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void AfterSetModifiers()
    {
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

      // --- Resolve the return type
      if (_ResultingType != null)
      {
        _ResultingType.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }

      // --- Resolve the explicit name of the member
      if (_ExplicitName != null)
      {
        _ExplicitName.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion

    #region Semantic checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics of the specified member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void CheckSemantics()
    {
      CheckGeneralMemberSemantics();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the general semantics of this member.
    /// </summary>
    /// <remarks>
    /// This implementation only checks, if the type of the member is at least as
    /// accessible as the member itself.
    /// Derived types should use this method to check the general semantics and then
    /// they can check member-specific semantics.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void CheckGeneralMemberSemantics()
    {
      if (_ResultingType == null) return;
      TypeReference typeRef = _ResultingType.Tail;

      // --- Only those members are checked that have a non-void resolved resulting type.
      if (typeRef == null || !typeRef.IsResolved || typeRef.IsVoid) return;

      // --- If types are resolved to type parameters, no accessibility checks are 
      // --- performed.
      if (typeRef.IsResolvedToTypeParameter) return;

      // --- This member should have been resolved to a type and not to something else
      if (!typeRef.IsResolvedToType)
      {
        Invalidate();
        return;
      }

      // --- Check, if the type of the member is at least as accessible as the field.
      AccessibilityDomain memberDomain = new AccessibilityDomain(DeclaringType, Visibility);
      AccessibilityDomain typeDomain = new AccessibilityDomain(typeRef.TypeInstance);
      if (!typeDomain.IsAtLeastAsAccessibleAs(memberDomain))
      {
        // --- Field type is less accessible than the field itself
        Parser.Error0052(Token, QualifiedName, ResultingType.FullName);
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if modifier combinations are valid according to the method rules.
    /// </summary>
    /// <remarks>
    /// This method is used to check not only the method but property semantics.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void CheckMethodModifiers()
    {
      // --- Static methods cannot be abstract, override or virtual at the same time.
      if (IsStatic && (IsAbstract || IsOverride || IsVirtual))
      {
        Parser.Error0112(Token, QualifiedName);
        Invalidate();
      }

      // --- Either virtual or override modifier is allowed.
      if (IsVirtual && IsOverride)
      {
        Parser.Error0113(Token, QualifiedName);
        Invalidate();
      }

      // --- Abstract method cannot be virtual
      if (IsAbstract && IsVirtual)
      {
        Parser.Error0503(Token, QualifiedName);
        Invalidate();
      }

      // --- Abstract method cannot be sealed
      if (IsAbstract && IsSealed)
      {
        Parser.Error0502(Token, QualifiedName);
        Invalidate();
      }

      // --- Abstract method cannot be extern
      if (IsAbstract && IsExtern)
      {
        Parser.Error0180(Token, QualifiedName);
        Invalidate();
      }

      // --- Only override classes can be sealed
      if (IsSealed && !IsOverride)
      {
        Parser.Error0238(Token, QualifiedName);
        Invalidate();
      }

      // --- Private methods must not have abstract, virtual or override modifiers.
      if (Visibility == Visibility.Private &&
        (IsAbstract || IsVirtual || IsOverride))
      {
        Parser.Error0621(Token, QualifiedName);
        Invalidate();
      }
    }

    #endregion

    #region Protected semantic check methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "extern" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void ExternNotAllowed()
    {
      if (IsExtern)
      {
        Parser.Error0106(Token, "extern");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "override" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void OverrideNotAllowed()
    {
      if (IsOverride)
      {
        Parser.Error0106(Token, "override");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "static" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void StaticNotAllowed()
    {
      if (IsStatic)
      {
        Parser.Error0106(Token, "static");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "new" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void NewNotAllowed()
    {
      if (IsNew)
      {
        Parser.Error0106(Token, "new");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "readonly" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void ReadOnlyNotAllowed()
    {
      if (IsReadOnly)
      {
        Parser.Error0106(Token, "readonly");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "volatile" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void VolatileNotAllowed()
    {
      if (IsVolatile)
      {
        Parser.Error0106(Token, "volatile");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "virtual" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void VirtualNotAllowed()
    {
      if (IsVirtual)
      {
        Parser.Error0106(Token, "virtual");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "sealed" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0106 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void SealedNotAllowed()
    {
      if (IsSealed)
      {
        Parser.Error0106(Token, "sealed");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "abstract" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0681 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void AbstractNotAllowed()
    {
      if (IsAbstract)
      {
        Parser.Error0106(Token, "abstract");
        Invalidate();
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the unallowed "abstract" modifier is used for this member.
    /// </summary>
    /// <remarks>
    /// Raises a CS0681 if modifier found, the member is invalidated.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected void AbstractNotAllowedWith0681()
    {
      if (IsAbstract)
      {
        Parser.Error0681(Token);
        Invalidate();
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of member declarations that can be indexed by the
  /// signature of the type.
  /// </summary>
  // ==================================================================================
  public sealed class MemberDeclarationCollection : 
    RestrictedIndexedCollection<MemberDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">TypeDeclaration item.</param>
    /// <returns>
    /// Signature of the member declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(MemberDeclaration item)
    {
      return item.Signature;
    }
  }
}
