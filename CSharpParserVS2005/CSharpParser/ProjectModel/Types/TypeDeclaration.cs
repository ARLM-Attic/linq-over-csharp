using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the visibility options a type declaration can have.
  /// </summary>
  // ==================================================================================
  public enum Visibility
  {
    Default = 0,
    Private,
    Protected,
    Public,
    Internal
  }

  // ==================================================================================
  /// <summary>
  /// This abstract type represents a type declaration.
  /// </summary>
  // ==================================================================================
  public abstract class TypeDeclaration: AttributedElement
  {
    #region Private fields

    private Visibility _Visibility;
    private bool _DefaultVisibility;
    private Namespace _Namespace;
    private TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private List<TypeParameterConstraint> _ParameterConstraints =
      new List<TypeParameterConstraint>();
    private List<TypeReference> _BaseTypes = new List<TypeReference>();
    private bool _IsNew;
    private bool _IsUnsafe;
    private TypeDeclaration _ParentType;
    private List<TypeDeclaration> _NestedTypes = new List<TypeDeclaration>();
    private List<MemberDeclaration> _Members = new List<MemberDeclaration>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected TypeDeclaration(Token token)
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
    /// Gets or sets the namespace of the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Namespace Namespace
    {
      get { return _Namespace; }
      set { _Namespace = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters belonging to this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameterCollection TypeParameters
    {
      get { return _TypeParameters; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter constraints belonging to this type
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeParameterConstraint> ParameterConstraints
    {
      get { return _ParameterConstraints; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is a generic type or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return _TypeParameters.Count > 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type elements belonging to this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeReference> BaseTypes
    {
      get { return _BaseTypes; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has a base type or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBaseType
    {
      get { return _BaseTypes.Count > 0; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Overrides the name property to use generic notation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get
      {
        string normalName = IsNestedType 
          ? _ParentType.FullName + "+" + Name 
          : Name;
        if (IsGenericType)
        {
          return String.Format("{0}´{1}", normalName, _TypeParameters.Count);
        }
        else
        {
          return normalName;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name property using generic type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ParametrizedName
    {
      get
      {
        if (IsGenericType)
        {
          StringBuilder sb = new StringBuilder(100);
          sb.Append(Name);
          sb.Append('<');
          bool firstParam = true;
          foreach (TypeParameter param in _TypeParameters)
          {
            if (!firstParam) sb.Append(", ");
            sb.Append(param.Name);
            firstParam = false;
          }
          sb.Append('>');
          return sb.ToString();
        }
        else
        {
          return Name;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is a class or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return this is ClassDeclaration; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is a struct or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStruct
    {
      get { return this is StructDeclaration; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an interface or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return this is InterfaceDeclaration; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has a 'new' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNew
    {
      get { return _IsNew; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has an 'unsafe' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnsafe
    {
      get { return _IsUnsafe; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this type declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration ParentType
    {
      get { return _ParentType; }
      set
      {
        _ParentType = value;
        _ParentType.NestedTypes.Add(this);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of nested types belonging to this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeDeclaration> NestedTypes
    {
      get { return _NestedTypes; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is a nested type or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedType
    {
      get { return _ParentType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has nested types or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasNestedTypes
    {
      get { return _NestedTypes.Count > 0; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets or the collection of attributes belonging to this type declaration.
    /// </summary>
    /// <param name="pars">Expression parameters to assign.</param>
    // --------------------------------------------------------------------------------
    public void AssignTypeParameters(TypeParameterCollection pars)
    {
      if (pars == null)
      {
        _TypeParameters.Clear();
      }
      else
      {
        _TypeParameters = pars;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the properties using the modifiers.
    /// </summary>
    /// <param name="mod">Modifier enumeration value.</param>
    // --------------------------------------------------------------------------------
    public virtual void SetModifiers(Modifier mod)
    {
      // --- Set visibility
      _Visibility = Visibility.Default;
      if ((mod & Modifier.@private) != 0) _Visibility = Visibility.Private;
      else if ((mod & Modifier.@protected) != 0) _Visibility = Visibility.Protected;
      else if ((mod & Modifier.@public) != 0) _Visibility = Visibility.Public;
      else if ((mod & Modifier.@internal) != 0) _Visibility = Visibility.Internal;
      _DefaultVisibility = _Visibility == Visibility.Default;

      // --- New and unsafe
      _IsNew = (mod & Modifier.@new) != 0;
      _IsUnsafe = (mod & Modifier.@unsafe) != 0;
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
    /// Gets the string representing the modifiers of this type definition.
    /// </summary>
    /// <returns>
    /// String representation of modifiers.
    /// </returns>
    // --------------------------------------------------------------------------------
    public virtual string GetModifiersText()
    {
      StringBuilder sb = new StringBuilder(40);
      if (!_DefaultVisibility) sb.Append(_Visibility.ToString().ToLower());
      if (_IsNew)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("new");
      }
      if (_IsUnsafe)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("unsafe");
      }
      return sb.ToString();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the members belonging to this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<MemberDeclaration> Members
    {
      get { return _Members; }
    }

    #endregion

    #region Public iterators

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets all constant members within the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<ConstDeclaration> Consts
    {
      get
      {
        foreach (MemberDeclaration member in _Members)
        {
          if (member is ConstDeclaration) yield return member as ConstDeclaration;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets all field members within the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<FieldDeclaration> Fields
    {
      get
      {
        foreach (MemberDeclaration member in _Members)
        {
          if (member is FieldDeclaration) yield return member as FieldDeclaration;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets all event members within the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<FieldDeclaration> Events
    {
      get
      {
        foreach (MemberDeclaration member in _Members)
        {
          FieldDeclaration fi = member as FieldDeclaration;
          if (fi != null && fi.IsEvent) yield return fi;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets all constructor members within the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<ConstructorDeclaration> Constructors
    {
      get
      {
        foreach (MemberDeclaration member in _Members)
        {
          if (member is ConstructorDeclaration) yield return member as ConstructorDeclaration;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets all property members within the type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<PropertyDeclaration> Properties
    {
      get
      {
        foreach (MemberDeclaration member in _Members)
        {
          if (member is PropertyDeclaration) yield return member as PropertyDeclaration;
        }
      }
    }

    #endregion
  }
}
