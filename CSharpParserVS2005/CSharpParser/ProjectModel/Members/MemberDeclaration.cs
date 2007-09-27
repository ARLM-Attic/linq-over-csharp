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
  public abstract class MemberDeclaration : AttributedElement
  {
    #region Private fields

    // --- Holds the declared modifiers, even not allowed for the type
    protected Modifier _DeclaredModifier;

    // --- Holds the information about the explicitly declared visibility.
    protected Visibility _DeclaredVisibility;

    // --- Modifier flags used by this declaration. True value indicates the modifier
    // --- is used.
    protected bool _IsNew;
    protected bool _IsUnsafe;
    protected bool _IsStatic;
    protected bool _IsVirtual;
    protected bool _IsSealed;
    protected bool _IsOverride;
    protected bool _IsAbstract;
    protected bool _IsExtern;
    protected bool _IsReadOnly;
    protected bool _IsVolatile;

    // --- Fields describing the main characteristics of the member
    private readonly TypeDeclaration _DeclaringType;
    private TypeReference _ResultingType;
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
    /// Gets or sets the visibility of the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Visibility DeclaredVisibility
    {
      get { return _DeclaredVisibility; }
      set { _DeclaredVisibility = value; }
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
    /// Gets the flag indicating if the type has its default visibility (no visibility
    /// modifiers has been used for the class declaration).
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasDefaultVisibility
    {
      get { return _DeclaredVisibility == Visibility.Default; }
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
        Name = value.RightmostName;
        value.RightMostPart.ResolveToName();
        if (value.HasSubType)
        {
          _ExplicitName = value;
          value.RightMostPart.PrefixType.SubType = null;
        }
        else
        {
          _ExplicitName = null;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "new" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNew
    {
      get { return _IsNew; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "unsafe" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnsafe
    {
      get { return _IsUnsafe; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "static" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "virtual" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVirtual
    {
      get { return _IsVirtual; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "sealed" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return _IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "override" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsOverride
    {
      get { return _IsOverride; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "abstract" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return _IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this property has the "extern" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsExtern
    {
      get { return _IsExtern; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has the "readonly" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsReadOnly
    {
      get { return _IsReadOnly; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has the "volatile" modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVolatile
    {
      get { return _IsVolatile; }
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
    public string QualifiedName
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
      else if (_DeclaringType.IsInterface || _DeclaringType.IsEnum)
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics of the specified member.
    /// </summary>
    /// <remarks>
    /// This implementation only checks, if the type of the member is at least as
    /// accessible as the member itself.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual void CheckSemantics()
    {
      if (_ResultingType == null) return;
      TypeReference typeRef = _ResultingType.RightMostPart;
      
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
      AccessibilityDomain typeDomain = new AccessibilityDomain(typeRef.ResolvingType);
      if (!typeDomain.IsAtLeastAsAccessibleAs(memberDomain))
      {
        // --- Field type is less accessible than the field itself
        Parser.Error0052(Token, QualifiedName, ResultingType.FullName);
        Invalidate();
      }
    }
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
