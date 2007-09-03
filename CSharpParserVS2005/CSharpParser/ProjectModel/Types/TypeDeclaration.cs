using System;
using System.Reflection;
using System.Text;
using CSharpParser.ParserFiles;
using CSharpParser.Collections;
using CSharpParser.Semantics;

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
    Internal,
    ProtectedInternal
  }

  // ==================================================================================
  /// <summary>
  /// This abstract type represents a type declaration.
  /// </summary>
  // ==================================================================================
  public abstract class TypeDeclaration: AttributedElement, 
    ITypeParameterOwner,
    ITypeCharacteristics,
    ITypeParameterScope,
    IResolutionContext
  {
    #region Private fields

    protected Visibility _DeclaredVisibility;
    protected bool _DefaultVisibility;
    private NamespaceFragment _EnclosingNamespace;
    private readonly TypeReferenceCollection _BaseTypes = new TypeReferenceCollection();
    private TypeDeclaration _DeclaringType;
    private bool _IsNew;
    private bool _IsUnsafe;
    private bool _IsAbstract;
    private bool _IsSealed;
    private readonly TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private readonly TypeParameterConstraintCollection _ParameterConstraints =
      new TypeParameterConstraintCollection();
    private readonly TypeDeclarationCollection _NestedTypes = new TypeDeclarationCollection();

    // --- Members of the type
    private readonly MemberDeclarationCollection _Members = new MemberDeclarationCollection();
    private readonly ConstDeclarationCollection _Consts = new ConstDeclarationCollection();
    private readonly ConstructorDeclarationCollection _Constructors = new ConstructorDeclarationCollection();
    private readonly EventPropertyDeclarationCollection _EventProperties = 
      new EventPropertyDeclarationCollection();
    private readonly PropertyDeclarationCollection _Properties = new PropertyDeclarationCollection();
    private readonly IndexerDeclarationCollection _Indexers = new IndexerDeclarationCollection();
    private readonly MethodDeclarationCollection _Methods = new MethodDeclarationCollection();
    private readonly OperatorDeclarationCollection _Operators = new OperatorDeclarationCollection();
    private readonly CastOperatorDeclarationCollection _CastOperators = 
      new CastOperatorDeclarationCollection();
    private readonly FieldDeclarationCollection _Fields = new FieldDeclarationCollection();

    // --- Fields used by ITypeCharacteristics
    private ITypeCharacteristics _BaseType;

    // --- Fields used for semantics check
    private TypeResolutionNode _ResolverNode;

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
    /// Gets or sets the declared visibility of the type declaration.
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
      get { return _DefaultVisibility ? Visibility.Internal : _DeclaredVisibility; }  
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
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return _TypeParameters.Count > 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return _TypeParameters.Count; }
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
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    string ITypeCharacteristics.Namespace
    {
      get { return _EnclosingNamespace.FullName; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is .NET runtime type or not
    /// </summary>
    /// <remarks>Always returns false.</remarks>
    // --------------------------------------------------------------------------------
    public bool IsRuntimeType
    {
      get { return false; }
    }

    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ReferencedUnit DeclaringUnit
    {
      get { return Parser.CompilationUnit.ThisUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of this type.
    /// </summary>
    /// <remarks>
    /// If there is no explicit base type for this type, a corresponding reference to
    /// System.Object should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics BaseType
    {
      get
      {
        if (_BaseTypes.Count != 0)
        {
          if (_BaseType == null)
          {
            _BaseType = NetBinaryType.Object;
          }
        }
        else
        {
          if (_BaseType == null)
          {
            // TODO: Change it to a type reference
            throw new NotImplementedException();
          }
        }
        return _BaseType;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    ITypeCharacteristics ITypeCharacteristics.DeclaringType
    {
      get { return _DeclaringType; }
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
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    /// <remarks>
    /// Always returns false, because a type declaration never has an element type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return _IsAbstract; }
    }

    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { throw new NotImplementedException(); }
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

    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { return this is EnumDeclaration; }
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
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return _DeclaringType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedAssembly
    {
      get { return IsNested && _DeclaredVisibility == ProjectModel.Visibility.Internal; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPrivate
    {
      get { return IsNested && _DeclaredVisibility == ProjectModel.Visibility.Private; }
    }

    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return IsNested && _DeclaredVisibility == ProjectModel.Visibility.Public; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return (_DeclaredVisibility & ProjectModel.Visibility.Public) != 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    /// <remarks>
    /// Always returns false.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    /// <remarks>
    /// Always returns false.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsPrimitive
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return (_DeclaredVisibility & ProjectModel.Visibility.Public) == 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual bool IsSealed
    {
      get { return _IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { return IsStruct || IsEnum; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type can be accessed by code outside the 
    /// assembly.
    /// </summary>
    /// <value>
    /// true if the current Type is a public type or a public nested type such that 
    /// all the enclosing types are public; otherwise, false.
    /// </value>
    /// <remarks>
    /// Use this property to determine whether a type is part of the public 
    /// interface of a component assembly.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool IsVisible
    {
      get
      {
        return (!IsNested && IsPublic) || (IsNestedPublic && DeclaringType.IsVisible);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a MemberTypes value indicating that this member is a type or a nested 
    /// type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MemberTypes MemberType
    {
      get 
      {
        return IsNested
                 ? MemberTypes.TypeInfo | MemberTypes.NestedType
                 : MemberTypes.TypeInfo;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name of the current member.
    /// </summary>
    /// <remarks>The simple name does not contain any adornements.</remarks>
    // --------------------------------------------------------------------------------
    public string SimpleName
    {
      get { return ShortName; }
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
        if (_DeclaringType == value) return;
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of method declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public MethodDeclarationCollection Methods
    {
      get { return _Methods; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of operator declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public OperatorDeclarationCollection Operators
    {
      get { return _Operators; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of cast operator declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public CastOperatorDeclarationCollection CastOperators
    {
      get { return _CastOperators; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of field operator declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public FieldDeclarationCollection Fields
    {
      get { return _Fields; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver node of this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode ResolverNode
    {
      get { return _ResolverNode; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolver during the semantic parsing.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void SetTypeResolvers()
    {
      ResolutionNodeBase resolverNode;
      if (IsNested)
      {
        // --- Use the resolver of the declaring type
        resolverNode = _DeclaringType.ResolverNode;
      }
      else if (_EnclosingNamespace == null)
      {
        // --- This is a global type, use the resolver of the compilation unit
        resolverNode = Parser.CompilationUnit.SourceResolutionTree;
      }
      else
      {
        // --- This is a type within a namespace
        resolverNode = _EnclosingNamespace.ResolverNode;
      }

      // --- Register the type for the resolver node
      if (resolverNode != null)
      {
        if ((_ResolverNode = resolverNode.CreateType(this)) == null)
        {
          Parser.Error0101(Token, Name, Name);
        }

        // --- Set the resolver for the nested types
        foreach (TypeDeclaration nested in _NestedTypes)
        {
          nested.SetTypeResolvers();
        }
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
      _DefaultVisibility = _DeclaredVisibility == Visibility.Default;

      // --- New and unsafe
      _IsNew = (mod & Modifier.@new) != 0;
      _IsUnsafe = (mod & Modifier.@unsafe) != 0;
      _IsAbstract = (mod & Modifier.@abstract) != 0;
      _IsSealed = (mod & Modifier.@sealed) != 0;

      // --- Types declared within a namespace but out of a type declaration can only
      // --- be public or internal
      if (!IsNested && Visibility != Visibility.Public &&
        Visibility != Visibility.Internal)
      {
        Parser.Error1527(Token);
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
      if (!_DefaultVisibility) sb.Append(_DeclaredVisibility.ToString().ToLower());
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object carrying detailed information about this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public object TypeObject
    {
      get { return this; }
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
        Parser.Error0692(parameter.Token, parameter.Name);
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
        Parser.Error0409(constraint.Token, constraint.Name);
      }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);

      // --- Resolve base types
      foreach (TypeReference typeReference in _BaseTypes)
      {
        typeReference.ResolveTypeReferences(ResolutionContext.TypeDeclaration, this);
      }

      // --- Resolve type argument constraints
      foreach (TypeParameterConstraint constraint in _ParameterConstraints)
      {
        constraint.ResolveTypeReferences(ResolutionContext.TypeDeclaration, this);
      }

      // --- Resolve nested type definitions
      foreach (TypeDeclaration nestedType in _NestedTypes)
      {
        nestedType.ResolveTypeReferences(ResolutionContext.TypeDeclaration, this);
      }

      // --- Resolve types in members
      foreach (MemberDeclaration member in _Members)
      {
        member.ResolveTypeReferences(ResolutionContext.TypeDeclaration, this);
      }
    }

    #endregion

    #region IResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile EnclosingSourceFile
    {
      get { throw new NotSupportedException("TypeDeclaration has no enclosing source file."); }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment EnclosingNamespace
    {
      get { return _EnclosingNamespace; }
      set { _EnclosingNamespace = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration EnclosingType
    {
      get { return this; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the method declaration enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MethodDeclaration EnclosingMethod
    {
      get { throw new NotSupportedException("TypeDeclaration has no enclosing method."); }
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
          Parser.Error0102(e.Item.Token, Name, e.Item.Signature);
          e.Cancel = true;
          return;
        }
        else _Consts.Add(constDecl);
      }

      // --- Check, if member is a field
      FieldDeclaration fieldDecl = e.Item as FieldDeclaration;
      if (fieldDecl != null)
      {
        if (_Fields.Contains(fieldDecl))
        {
          Parser.Error0102(e.Item.Token, Name, e.Item.Signature);
          e.Cancel = true;
          return;
        }
        else _Fields.Add(fieldDecl);
      }

      // --- Check, if member is a constructor
      ConstructorDeclaration ctorDecl = e.Item as ConstructorDeclaration;
      if (ctorDecl != null)
      {
        if (_Constructors.Contains(ctorDecl))
        {
          Parser.Error0111(e.Item.Token, Name, e.Item.Signature);
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
          Parser.Error0102(e.Item.Token, Name, e.Item.Signature);
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
          Parser.Error0111(e.Item.Token, Name, e.Item.Signature);
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
          Parser.Error0102(e.Item.Token, Name, e.Item.Signature);
          e.Cancel = true;
          return;
        }
        else _Properties.Add(propDecl);
      }

      // --- Check, if member is an operator declaration
      OperatorDeclaration operDecl = e.Item as OperatorDeclaration;
      if (operDecl != null)
      {
        if (_Operators.Contains(operDecl))
        {
          Parser.Error0111(e.Item.Token, Name, e.Item.Signature);
          e.Cancel = true;
          return;
        }
        else _Operators.Add(operDecl);
      }

      // --- Check, if member is a cast operator declaration
      CastOperatorDeclaration castDecl = e.Item as CastOperatorDeclaration;
      if (castDecl != null)
      {
        if (_CastOperators.Contains(castDecl))
        {
          Parser.Error0557(e.Item.Token, Name);
          e.Cancel = true;
          return;
        }
        else _CastOperators.Add(castDecl);
      }

      // --- Check, if member is a method
      MethodDeclaration methodDecl = e.Item as MethodDeclaration;
      if (methodDecl != null)
      {
        if (_Methods.Contains(methodDecl))
        {
          Parser.Error0111(e.Item.Token, Name, e.Item.Signature);
          e.Cancel = true;
          return;
        }
        else _Methods.Add(methodDecl);
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
        Parser.Error0101(e.Item.Token, Name, e.Item.Signature);
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
      return (item.EnclosingNamespace == null 
        ? String.Empty 
        : item.EnclosingNamespace.FullName) 
        + item.FullName;
    }
  }
}
