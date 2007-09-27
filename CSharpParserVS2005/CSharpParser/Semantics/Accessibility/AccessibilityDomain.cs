using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents an accessibility value of a type or a type member.
  /// </summary>
  /// <remarks>
  /// Accessibility domains can be compared to determine if one equals with the other,
  /// or one is a superset of an other or they are disjoint.
  /// </remarks>
  // ==================================================================================
  public sealed class AccessibilityDomain
  {
    #region Public types

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This type defines the possible scope types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public enum ProgramScope
    {
      /// <summary>Access domain is empty. The type cannot be accessed.</summary>
      None,

      /// <summary>Type or member can be accessed only from the enclosing type.</summary>
      Private,

      /// <summary>
      /// Type can be accessed from within its declaring type and from inheritors 
      /// within the declaration type.
      /// </summary>
      PrivatePlusInheritors,
      
      /// <summary>
      /// Type can be accessed from within the compilation unit but not out from
      /// the compilation.
      /// </summary>
      Internal,

      /// <summary>
      /// Type can be accessed from within its declaring type and from inheritors in
      /// any compilation unit.
      /// </summary>
      PublicInheritors,

      /// <summary>
      /// Type can be accessed from within the compilation unit and out from the 
      /// compilation by deriving types.
      /// </summary>
      InternalPlusInheritors,

      /// <summary>
      /// Type can be accessed without any restriction.
      /// </summary>
      Public
    }

    #endregion

    #region Private fields

    private ProgramScope _Scope;
    private ITypeCharacteristics _ScopingType;
    private readonly ITypeCharacteristics _OwnerType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an accessibility domain instance for a member of the specified type, 
    /// where member has the specified visibility.
    /// </summary>
    /// <param name="parentType">Parent type instance.</param>
    /// <param name="memberVisibility">Visibility of the member.</param>
    // --------------------------------------------------------------------------------
    public AccessibilityDomain(ITypeCharacteristics parentType, Visibility memberVisibility)
    {
      AccessibilityDomain parent = new AccessibilityDomain(parentType);
      Combine(parent, memberVisibility);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an accessibility domain instance for the specified type.
    /// </summary>
    /// <param name="type">Type instance.</param>
    // --------------------------------------------------------------------------------
    public AccessibilityDomain(ITypeCharacteristics type)
    {
      _OwnerType = type;
      TypeDeclaration typeDecl = 
        TypeDeclaration.GetRootElementType(type) as TypeDeclaration;
      if (typeDecl == null)
      {
        // --- Type comes from a referenced assembly
        _Scope = type.IsVisible ? ProgramScope.Public : ProgramScope.None;
        _ScopingType = null;
      }
      else
      {
        // --- Type comes from the source declaration
        if (typeDecl.IsNested)
        {
          AccessibilityDomain parent = new AccessibilityDomain(typeDecl.DeclaringType);
          Combine(parent, typeDecl.Visibility);
        }
        else
        {
          // --- Top level type
          _Scope = typeDecl.IsPublic ? ProgramScope.Public : ProgramScope.Internal;
          _ScopingType = null;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an accessibility domain instance internally.
    /// </summary>
    /// <param name="scope">Scope of the domain.</param>
    /// <param name="scopingType">Scoping type of the domain.</param>
    // --------------------------------------------------------------------------------
    private AccessibilityDomain(ProgramScope scope, ITypeCharacteristics scopingType)
    {
      _Scope = scope;
      _ScopingType = scopingType;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the scope of this accessibility domain.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ProgramScope Scope
    {
      get { return _Scope; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the scoping typeof this accessibility domain.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics ScopingType
    {
      get { return _ScopingType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type owning this accessibility domain.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics OwnerType
    {
      get { return _OwnerType; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the type represented by this accessibility domain is at least as
    /// accessible as the type represented by the other domain.
    /// </summary>
    /// <param name="other">Other accessibility domain.</param>
    /// <returns>
    /// True, if this type is at least as accessible as the other; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This accessibility domain must be a superset of the other domain to be 
    /// "at least as accessible".
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsAtLeastAsAccessibleAs(AccessibilityDomain other)
    {
      if (_Scope == ProgramScope.PublicInheritors && other._Scope == ProgramScope.Internal)
        return false;
      if (_Scope == ProgramScope.Public || _Scope > other._Scope) return true;
      if (other._Scope > _Scope) return false;

      // --- At this point scopes are equal.
      if (_Scope == ProgramScope.None || _Scope == ProgramScope.Public ||
        _Scope == ProgramScope.Internal)
      {
        // --- Scoping type does not require more restriction, this accessibility
        // --- domain equals with the other.
        return true;
      }

      // --- If the other scoping type is a derived type of this domain's type,
      // --- this is "at least as accessible", otherwise, not.
      return TypeDeclaration.IsSameOrInheritsFrom(other._ScopingType, _ScopingType);
    }

    #endregion

    #region Private methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the properties of this accessibility domain by combining the specified
    /// parent accessibility domain with the provided member visibility.
    /// </summary>
    /// <param name="parent">Parent access domain.</param>
    /// <param name="memberVisibility">Visibility of the member.</param>
    /// <remarks>
    /// This method assumes that the parent access domain is a domain of this type 
    /// or a nested type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void Combine(AccessibilityDomain parent, Visibility memberVisibility)
    {
      switch (memberVisibility)
      {
        case Visibility.Private:
          // --- We keep the type private
          parent._Scope = ProgramScope.Private;
          parent._ScopingType = parent._OwnerType;
          break;

        case Visibility.Protected:
          // --- Intersect the parent domain with the parent type + inheritors
          parent.Intersect(
            new AccessibilityDomain(ProgramScope.PublicInheritors, parent._OwnerType));
          break;

        case Visibility.ProtectedInternal:
          // --- Intersect the parent domain with the parent type + inheritors + 
          // --- compilation unit.
          parent.Intersect(
            new AccessibilityDomain(ProgramScope.InternalPlusInheritors, parent._OwnerType));
          break;

        case Visibility.Internal:
          // --- Intersect the parent domain with the compilation unit.
          parent.Intersect(new AccessibilityDomain(ProgramScope.Internal, null));
          break;

        case Visibility.Public:
          // --- We keep the parent scope
          break;
      }

      // --- Copy back parent properties
      _Scope = parent._Scope;
      _ScopingType = parent._ScopingType;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Intersects this accessibility domain with the specified domain.
    /// </summary>
    /// <param name="other">Domain to intersect this domain with.</param>
    /// <remarks>
    /// This method assumes that the other accessibility domain is a domain of a 
    /// nested type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void Intersect(AccessibilityDomain other)
    {
      // --- If the current scope includes inheritors, we need some more intersection
      // --- operation later
      bool includesInheritors = _Scope == ProgramScope.InternalPlusInheritors ||
                                _Scope == ProgramScope.PublicInheritors ||
                                _Scope == ProgramScope.PrivatePlusInheritors;

      // --- Intersect the program scope to the smaller one.
      if (other._Scope < _Scope) _Scope = other._Scope;
      if (_Scope == ProgramScope.Internal && other._Scope == ProgramScope.PublicInheritors)
        _Scope = ProgramScope.PrivatePlusInheritors;
      if (_Scope == ProgramScope.None || _Scope == ProgramScope.Public ||
        _Scope == ProgramScope.Internal)
      {
        // --- Scoping type does not require more restriction.
        _ScopingType = null;
        return;
      }

      if (_Scope == ProgramScope.Private || !includesInheritors)
      {
        // --- Scoping type is the nested type
        _ScopingType = other._ScopingType;
      }
      else
      {
        // --- We must combine the inheritors of the owner type with the inheritors
        // --- of the scoping type.
        if (TypeDeclaration.IsSameOrInheritsFrom(other._ScopingType, _ScopingType))
        {
          // --- Nested type derives from the declaring type
          _ScopingType = other._ScopingType;
        }
        else
        {
          // --- Inheritors are not in the intersection, omit inheritors.
          _ScopingType = other._ScopingType;
          if (_Scope == ProgramScope.InternalPlusInheritors)
            _Scope = ProgramScope.Internal;
          else
            _Scope = ProgramScope.Private;
        }
      }
    }

    #endregion
  }
}
