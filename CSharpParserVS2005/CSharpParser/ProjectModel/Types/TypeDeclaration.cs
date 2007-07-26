using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.ParserFiles;
using CSharpParser.Collections;

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
  public abstract class TypeDeclaration: AttributedElement, ITypeParameterOwner
  {
    #region Private fields

    private Visibility _Visibility;
    private bool _DefaultVisibility;
    private NamespaceFragment _Namespace;
    private readonly TypeReferenceCollection _BaseTypes = new TypeReferenceCollection();
    private TypeDeclaration _DeclaringType;
    private bool _IsNew;
    private bool _IsUnsafe;
    private readonly TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private readonly TypeParameterConstraintCollection _ParameterConstraints =
      new TypeParameterConstraintCollection();
    private readonly TypeDeclarationCollection _NestedTypes = new TypeDeclarationCollection();

    //ers of the type
    private readonly MemberDeclarationCollection _Members = new MemberDeclarationCollection();
    private readonly ConstDeclarationCollection _Consts = new ConstDeclarationCollection();
    private readonly ConstructorDeclarationCollection _Constructors = new ConstructorDeclarationCollection();
    private readonly EventPropertyDeclarationCollection _EventProperties = 
      new EventPropertyDeclarationCollection();
    private readonly PropertyDeclarationCollection _Properties = new PropertyDeclarationCollection();
    private readonly IndexerDeclarationCollection _Indexers = new IndexerDeclarationCollection();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    protected TypeDeclaration(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
      _Members.BeforeAdd += BeforeAddMembers;
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
    public NamespaceFragment Namespace
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
    public TypeParameterConstraintCollection ParameterConstraints
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
    public TypeReferenceCollection BaseTypes
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
    public override string Name
    {
      get
      {
        if (IsGenericType)
        {
          return String.Format("{0}`{1}", base.Name, _TypeParameters.Count);
        }
        return base.Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
    {
      get
      {
        return IsNestedType 
          ? _DeclaringType.FullName + "+" + Name 
          : Name;
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
          sb.Append(base.Name);
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
    public TypeDeclaration DeclaringType
    {
      get { return _DeclaringType; }
      set
      {
        _DeclaringType = value;
        _DeclaringType.NestedTypes.Add(this);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of nested types belonging to this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclarationCollection NestedTypes
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
      get { return _DeclaringType != null; }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of constant declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public ConstDeclarationCollection Consts
    {
      get { return _Consts; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of constructor declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public ConstructorDeclarationCollection Constructors
    {
      get { return _Constructors; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of event property declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public EventPropertyDeclarationCollection EventProperties
    {
      get { return _EventProperties; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of indexer declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public IndexerDeclarationCollection Indexers
    {
      get { return _Indexers; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of property declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public PropertyDeclarationCollection Properties
    {
      get { return _Properties; }
    }

    #endregion

    #region Public methods

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
    public MemberDeclarationCollection Members
    {
      get { return _Members; }
    }

    #endregion

    #region Public iterators

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

    #endregion

    #region ITypeParameterOwner members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter to the method declaration
    /// </summary>
    /// <param name="parameter">Type parameter</param>
    // --------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameter parameter)
    {
      try
      {
        _TypeParameters.Add(parameter);
      }
      catch (ArgumentException)
      {
        Parser.Error("CS0692", parameter.Token, 
          String.Format("Duplicate type parameter '{0}'", parameter.Name));
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter constraint to the type declaration
    /// </summary>
    /// <param name="constraint">Type parameter constraint</param>
    // --------------------------------------------------------------------------------
    public void AddTypeParameterConstraint(TypeParameterConstraint constraint)
    {
      try
      {
        _ParameterConstraints.Add(constraint);
      }
      catch (ArgumentException)
      {
        Parser.Error("CS0409", constraint.Token,
          String.Format("A constraint clause has already been specified for type " + 
          "parameter '{0}'. All of the constraints for a type parameter must be " +
          "specified in a single where clause.", constraint.Name));
      }
    }

    #endregion

    #region Private members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if a member with the specified name already exists or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    void BeforeAddMembers(object sender, ItemedCancelEventArgs<MemberDeclaration> e)
    {
      // --- Check, if member is a constant
      ConstDeclaration constDecl = e.Item as ConstDeclaration;
      if (constDecl != null)
      {
        if (_Consts.Contains(constDecl))
        {
          Parser.CompilationUnit.ErrorHandler.Error("CS0102", e.Item.Token,
                                                    String.Format(
                                                      "The type '{0}' already contains a definition for '{1}'", Name,
                                                      e.Item.Signature));
          e.Cancel = true;
          return;
        }
        else _Consts.Add(constDecl);
      }

      // --- Check, if member is a constructor
      ConstructorDeclaration ctorDecl = e.Item as ConstructorDeclaration;
      if (ctorDecl != null)
      {
        if (_Constructors.Contains(ctorDecl))
        {
          Parser.CompilationUnit.ErrorHandler.Error("CS0111", e.Item.Token,
                                                    String.Format(
                                                      "Type '{0}' already defines a member called '{1}' with the same parameter types",
                                                      Name, e.Item.Signature));
          e.Cancel = true;
          return;
        }
        else _Constructors.Add(ctorDecl);
      }

      // --- Check, if member is an event property
      EventPropertyDeclaration evPropDecl = e.Item as EventPropertyDeclaration;
      if (evPropDecl != null)
      {
        if (_EventProperties.Contains(evPropDecl))
        {
          Parser.CompilationUnit.ErrorHandler.Error("CS0102", e.Item.Token,
                                                    String.Format(
                                                      "The type '{0}' already contains a definition for '{1}'", Name,
                                                      e.Item.Signature));
          e.Cancel = true;
          return;
        }
        else _EventProperties.Add(evPropDecl);
      }

      // --- Check, if member is an indexer property
      IndexerDeclaration indDecl = e.Item as IndexerDeclaration;
      if (indDecl != null)
      {
        if (_Indexers.Contains(indDecl))
        {
          Parser.CompilationUnit.ErrorHandler.Error("CS0111", e.Item.Token,
            String.Format("Type '{0}' already defines a member called '{1}' with the same parameter types",
                                                      Name, e.Item.Signature));
          e.Cancel = true;
          return;
        }
        else _Indexers.Add(indDecl);
      }

      // --- Check, if member is a property
      PropertyDeclaration propDecl = e.Item as PropertyDeclaration;
      if (propDecl != null)
      {
        if (_Properties.Contains(propDecl))
        {
          Parser.CompilationUnit.ErrorHandler.Error("CS0102", e.Item.Token,
                                                    String.Format(
                                                      "The type '{0}' already contains a definition for '{1}'", Name,
                                                      e.Item.Signature));
          e.Cancel = true;
          return;
        }
        else _Properties.Add(propDecl);
      }

      // --- Check for finalizer
      FinalizerDeclaration finDecl = e.Item as FinalizerDeclaration;
      ClassDeclaration thisClass = this as ClassDeclaration;
      if (finDecl != null && thisClass != null)
      {
        if (thisClass.HasAlreadyFinalizer(finDecl))
        {
          e.Cancel = true;
          return;
        }
      }

      // --- Add member to the other members
      if (_Members.Contains(e.Item))
      {
        Parser.CompilationUnit.ErrorHandler.Error("CS0101",
          e.Item.Token,
          String.Format("The namespace '{0}' already contains a definition for '{1}'",
          Name, e.Item.Signature));
        e.Cancel = true;
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
  public class TypeDeclarationCollection : RestrictedIndexedCollection<TypeDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">TypeDeclaration item.</param>
    /// <returns>
    /// Full name of the type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeDeclaration item)
    {
      return (item.Namespace == null ? String.Empty : item.Namespace.FullName) + item.FullName;
    }
  }
}
