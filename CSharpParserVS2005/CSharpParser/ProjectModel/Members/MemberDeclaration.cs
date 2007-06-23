using CSharpParser.Collections;
using CSharpParser.ParserFiles;

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

    private Visibility _Visibility;
    private bool _DefaultVisibility;
    private TypeReference _ResultingType;
    private TypeReference _ExplicitName;
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

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected MemberDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the visibility of the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Visibility Visibility
    {
      get { return _Visibility; }
      set { _Visibility = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the type has its default visibility (no visibility
    /// modifiers has been used for the class declaration).
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasDefaultVisibility
    {
      get { return _DefaultVisibility; }
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
    public TypeReference ExplicitName
    {
      get { return _ExplicitName; }
      set
      {
        _ExplicitName = value;
        Name = _ExplicitName.RightmostName;
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
    public string FullName
    {
      get { return _ExplicitName == null ? Name : _ExplicitName.FullName;  }
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
      // --- Set visibility
      _Visibility = Visibility.Default;
      if ((mod & Modifier.@private) != 0) _Visibility = Visibility.Private;
      else if ((mod & Modifier.@protected) != 0) _Visibility = Visibility.Protected;
      else if ((mod & Modifier.@public) != 0) _Visibility = Visibility.Public;
      else if ((mod & Modifier.@internal) != 0) _Visibility = Visibility.Internal;
      _DefaultVisibility = _Visibility == Visibility.Default;

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
    }

    #endregion

    #region Virtual methods

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
