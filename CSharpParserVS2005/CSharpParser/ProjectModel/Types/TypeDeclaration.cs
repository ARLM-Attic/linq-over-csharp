using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.ParserFiles;
using CSharpParser.Collections;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
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

    // --- Holds the declared modifiers, even not allowed fot the type
    protected Modifier _DeclaredModifier;

    // --- Holds the information about the explicitly declared visibility.
    protected Visibility _DeclaredVisibility;

    // --- Modifier flags used by this declaration. True value indicates the modifier
    // --- is used.
    private bool _IsAbstract;
    private bool _IsNew;
    private bool _IsPartial;
    private bool _IsSealed;
    private bool _IsStatic;
    private bool _IsUnsafe;

    // --- Fields to decribe the type declaration scope
    private SourceFile _EnclosingSourceFile;
    private NamespaceFragment _EnclosingNamespace;

    // --- Fields describing the main characteristics of the type
    private TypeDeclaration _DeclaringType;
    private ITypeCharacteristics _BaseType;
    private TypeReference _BaseTypeReference;
    private readonly List<TypeReference> _InterfaceList = new List<TypeReference>();
    private readonly TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private readonly List<TypeParameterConstraint> _ParameterConstraints =
      new List<TypeParameterConstraint>();

    // --- Fields describing partial type specific information
    private TypeDeclaration _ConsolidatedDeclaration;
    private readonly List<TypeDeclaration> _Parts = new List<TypeDeclaration>();

    // --- Types declared within this type declaration
    private readonly List<TypeDeclaration> _NestedTypes = new List<TypeDeclaration>();

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

    // --- Node in the resolution tree that unambigously resolves this type
    private TypeResolutionNode _ResolverNode;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    /// <param name="declaringType">
    /// Type that declares this type. Null, if this type has no declaring type.
    /// </param>
    // --------------------------------------------------------------------------------
    protected TypeDeclaration(Token token, CSharpSyntaxParser parser, 
      TypeDeclaration declaringType)
      : base(token, parser)
    {
      _DeclaringType = declaringType;
      _Members.BeforeAdd += BeforeAddMembers;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declared visibility of the type declaration.
    /// </summary>
    /// <remarks>
    /// Declared visibility is the one that is explicitly declared in the source code.
    /// The effective visibility can be accessed by the Visibility property.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Visibility DeclaredVisibility
    {
      get { return _DeclaredVisibility; }
      set { _DeclaredVisibility = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the effective visibility of this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Visibility Visibility
    {
      get
      {
        return HasDefaultVisibility
          ? (IsNested ? Visibility.Private : Visibility.Internal)
          : _DeclaredVisibility;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get
      {
        return _IsAbstract;
      }
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
    /// Gets or sets the flag indicating if this type declaration has the partial 
    /// modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPartial
    {
      get { return _IsPartial; }
      set { _IsPartial = value; }
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
    /// Gets the flag indicating if this type has a 'static' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _IsStatic; }
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
    /// Gets the number of parts this type defines.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int PartCount
    {
      get { return _Parts.Count == 0 ? 1 : _Parts.Count; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the partitions belonging to this type declaration.
    /// </summary>
    /// <remarks>
    /// Only partial types have partitions. Each partition is a physical fragment of
    /// the partial type declaration.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public List<TypeDeclaration> Parts
    {
      get { return _Parts; }
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
    /// Makes an array type from the current type with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of array type to be created</param>
    /// <returns>
    /// Array type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakeArrayType(int rank)
    {
      return new ArrayType(this, rank);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics MakePointerType()
    {
      return new PointerType(this);
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
    public List<TypeReference> InterfaceList
    {
      get { return _InterfaceList; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has a base type or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBaseType
    {
      get { return _BaseType != null; }  
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
        return (IsGenericType) 
          ? String.Format("{0}`{1}", base.Name, _TypeParameters.Count)
          : base.Name;
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
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public int GetArrayRank()
    {
      throw new ArgumentException("Type is not an array.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics GetElementType()
    {
      return null;
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

    // --------------------------------------------------------------------------------
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
      get { return _BaseType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type reference representing the base type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference BaseTypeReference
    {
      get { return _BaseTypeReference; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the default base type of this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual ITypeCharacteristics DefaultBaseType
    {
      get { return NetBinaryType.Object; }
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
    public override string FullName
    {
      get
      {
        return IsNested 
          ? _DeclaringType.FullName + "+" + Name 
          : (_EnclosingNamespace == null ? Name : _EnclosingNamespace + "." + Name);
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
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name property using generic type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
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
      get { return IsNested && Visibility == Visibility.Private; }
    }

    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return IsNested && IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return Visibility != Visibility.Public; }
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
      get { return Visibility == Visibility.Public; }
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
        return (!IsNested && (IsPublic || Visibility == Visibility.Internal)) || 
          (IsNestedPublic && DeclaringType.IsVisible);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration that consolidates information about this partial
    /// type fragment.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration ConsolidatedDeclaration
    {
      get { return _ConsolidatedDeclaration; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this type declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration DeclaringType
    {
      get { return _DeclaringType; }
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

      // --- New and unsafe
      _IsNew = (mod & Modifier.@new) != 0;
      _IsUnsafe = (mod & Modifier.@unsafe) != 0;
      _IsAbstract = (mod & Modifier.@abstract) != 0;
      _IsSealed = (mod & Modifier.@sealed) != 0;
      _IsStatic = (mod & Modifier.@static) != 0;

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
      get { return _DeclaredVisibility == Visibility.Default; }
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Setsthe source file of this type declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SetSourceFile(SourceFile file)
    {
      _EnclosingSourceFile = file;
      foreach (TypeDeclaration nested in _NestedTypes)
        nested.SetSourceFile(file);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Setsthe source file of this type declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SetNamespace(NamespaceFragment ns)
    {
      _EnclosingNamespace = ns;
      foreach (TypeDeclaration nested in _NestedTypes)
        nested.SetNamespace(ns);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clones this type declaration into a new instance.
    /// </summary>
    /// <returns>
    /// The new cloned instance.
    /// </returns>
    // --------------------------------------------------------------------------------
    public virtual TypeDeclaration CloneToPart()
    {
      // --- Create the new type declaration instance
      TypeDeclaration clone = CreateNewPart();
      clone._ConsolidatedDeclaration = this;

      // --- Copy the properties of this instance to the new instance
      clone.ContextElement = ContextElement;
      clone.Comment = Comment;
      clone.Validate(); 
      clone._DeclaredVisibility = _DeclaredVisibility;
      clone._IsAbstract = _IsAbstract;
      clone._IsNew = _IsNew;
      clone._IsPartial = _IsPartial;
      clone._IsSealed = _IsSealed;
      clone._IsStatic = _IsStatic;
      clone._IsUnsafe = _IsUnsafe;
      clone._EnclosingSourceFile = _EnclosingSourceFile;
      clone._EnclosingNamespace = _EnclosingNamespace;
      clone._DeclaringType = _DeclaringType;
      clone._BaseType = _BaseType;
      clone._BaseTypeReference = _BaseTypeReference;
      foreach (AttributeDeclaration attr in Attributes) clone.Attributes.Add(attr);
      foreach (TypeReference type in _InterfaceList) clone._InterfaceList.Add(type);
      foreach (TypeParameter par in _TypeParameters) clone._TypeParameters.Add(par);
      foreach (TypeParameterConstraint con in _ParameterConstraints) 
        clone._ParameterConstraints.Add(con);
      foreach (TypeDeclaration type in _NestedTypes) clone._NestedTypes.Add(type);
      foreach (MemberDeclaration member in _Members) clone._Members.Add(member);
      clone._ResolverNode = _ResolverNode;

      // --- Retrieve this new instance
      return clone;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the type of this declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected abstract TypeDeclaration CreateNewPart();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Consolidates partial type fragments of this type definition.
    /// </summary>
    /// <remarks>
    /// This consolidates affects only the type declaration part (visibility,
    /// modifiers, base types and interfaces) but no members.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void ConsolidateDeclarationParts()
    {
      // --- Non-partial types do not need consolidation
      if (PartCount == 1) return;

      // --- Create a list of interface names used
      List<string> interfaces = new List<string>();
      foreach (TypeReference intf in _InterfaceList)
        if (intf.IsValid) interfaces.Add(intf.ResolvingType.FullName);

      Attributes.Clear();
      foreach (TypeDeclaration part in _Parts)
      {
        // --- Do not consolidate invalid parts
        if (!part.IsValid) continue;

        // --- Step 1: Partitions must have default accessibility or share the same
        // --- access specifier.
        if (!part.HasDefaultVisibility) _DeclaredVisibility = part._DeclaredVisibility;

        // --- Step 2: If one or more parts include the abstract modifier, the type is 
        // --- abstract, otherwise concrete.
        if (part.IsAbstract) _IsAbstract = true;

        // --- Step 3: If one or more parts include the sealed modifier, the type is 
        // --- sealed, otherwise open.
        if (part.IsSealed) _IsSealed = true;

        // --- Step 4: If one or more parts include the static modifier, the type is 
        // --- static, otherwise open.
        if (part.IsStatic) _IsStatic = true;

        // --- Step 5: a partial class declaration includes a base class specification, 
        // --- that base class specification shall reference the same type as all other 
        // --- parts of that partial type that include a base class specification.
        if (part.HasBaseType)
        {
          if (HasBaseType && _BaseType.FullName != part.BaseType.FullName)
          {
            Parser.Error0263(part.BaseTypeReference.Token, Name);
            part.Invalidate();
          }
          else
          {
            _BaseType = part.BaseType;
            _BaseTypeReference = part.BaseTypeReference;
          }
        }

        // --- Step 6: The set of interfaces for a type declared in multiple partitions
        // --- is the union of the interfaces specified on each part. A particular 
        // --- interface can only be named once on each part, but multiple parts can 
        // --- name the same base interface(s).
        foreach (TypeReference intf in part.InterfaceList)
        {
          if (!intf.IsValid) continue;
          if (!interfaces.Contains(intf.ResolvingType.FullName))
          {
            interfaces.Add(intf.ResolvingType.FullName);
            _InterfaceList.Add(intf);
          }
        }

        // --- Step 7: The attributes of a type declared in multiple partitions are determined 
        // --- by combining, in an unspecified order, the attributes of each of its parts. 
        // --- If the same attribute is placed on multiple parts, it is equivalent to 
        // --- specifying that attribute multiple times on the type.
        foreach (AttributeDeclaration attr in part.Attributes)
        {
          Attributes.Add(attr);
        }

      }

      // --- Step 8: When the unsafe modifier is used on a partial type declaration, 
      // --- only that particular part is considered an unsafe context.
      _IsUnsafe = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if there are any unallowed modifiers on this type declaration.
    /// </summary>
    /// <remarks>
    /// Derived classes should check other unallowed modifiers related to their
    /// classification.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void CheckUnallowedModifiers()
    {
      // --- "extern" is not allowed on types
      if ((_DeclaredModifier & Modifier.@extern) != 0)
        Parser.Error0106(Token, "extern");

      // --- "readonly" is not allowed on types
      if ((_DeclaredModifier & Modifier.@readonly) != 0)
        Parser.Error0106(Token, "readonly");

      // --- "new" modifier is allowed only on nested types.
      if (IsNew && !IsNested) Parser.Error1530(Token);

      CheckVolatileAndOverride();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if there are any unallowed modifiers on this non-class type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CheckUnallowedNonClassModifiers()
    {
      // --- "abstract", "static" and "sealed" is not allowed on non-class types
      if (IsAbstract) Parser.Error0106(Token, "abstract");
      if (IsStatic) Parser.Error0106(Token, "static");
      if ((_DeclaredModifier & Modifier.@sealed) != 0) Parser.Error0106(Token, "sealed");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if unallowed "volatile" and "override" modifiers are used on this 
    /// type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    protected virtual void CheckVolatileAndOverride()
    {
      // --- "volatile" is not allowed on types
      if ((_DeclaredModifier & Modifier.@volatile) != 0) Parser.Error0116(Token);

      // --- "override" is not allowed on types
      if ((_DeclaredModifier & Modifier.@volatile) != 0) Parser.Error0116(Token);
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
      TypeParameter param;
      
      // --- Type parameter must exist for the constraint
      if (!_TypeParameters.TryGetValue(constraint.Name, out param))
      {
        Parser.Error0699(constraint.Token, ParametrizedName, constraint.Name);
      }
      else
      {
        // --- One type parameter can have only one constraint.
        if (param.Constraint != null)
        {
          Parser.Error0409(constraint.Token, constraint.Name);
        }
        else param.AssignConstraint(constraint);
      }
      _ParameterConstraints.Add(constraint);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a nested type declaration to this type.
    /// </summary>
    /// <param name="type">Type to add</param>
    /// <remarks>
    /// Type declaration is added to the type container of the compilation unit.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void AddTypeDeclaration(TypeDeclaration type)
    {
      if (type == null) return;
      _NestedTypes.Add(type);
      Parser.CompilationUnit.AddTypeDeclaration(type);
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    /// <remarks>
    /// Base types are not resolved here, sinsce thay are resolved in an earlier phase
    /// of semantical parsing.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(ResolutionContext.TypeDeclaration, declarationScope, this);

      // --- Resolve type argument constraints
      foreach (TypeParameterConstraint constraint in _ParameterConstraints)
      {
        constraint.ResolveTypeReferences(ResolutionContext.TypeDeclaration, declarationScope, this);
      }

      // --- Resolve nested type definitions
      foreach (TypeDeclaration nestedType in _NestedTypes)
      {
        nestedType.ResolveTypeReferences(ResolutionContext.TypeDeclaration, declarationScope, this);
      }

      // --- Resolve types in members
      foreach (MemberDeclaration member in _Members)
      {
        member.ResolveTypeReferences(ResolutionContext.TypeDeclaration, declarationScope, this);
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
      get { return _EnclosingSourceFile; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace enclosing this resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceFragment EnclosingNamespace
    {
      get { return _EnclosingNamespace; }
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

    #region Iterators

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterates through the type declarations this type declaration directly 
    /// depends on.
    /// </summary>
    /// <remarks>
    /// In the spirit of S17.1.2.1, a type direclty depnds on its direct base types
    /// and its direct enclosing type (if type is a nested type).
    /// </remarks>
    // --------------------------------------------------------------------------------
    public IEnumerable<ITypeCharacteristics> DirectlyDependsOn
    {
      get
      {
        // --- Return direct base types
        if (_BaseType != null) yield return _BaseType;
        foreach (TypeReference baseType in InterfaceList)
        {
          if (baseType.RightMostPart.IsValid)
          {
            yield return baseType.RightMostPart.ResolvingType;
          }
        }
        // --- Return directly enclosing type
        if (_DeclaringType != null) yield return _DeclaringType;
      }
    }

    #endregion

    #region Methods related to base type checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the base declaration of this type declaration conforms with the
    /// C# language specification.
    /// </summary>
    /// <remarks>
    /// Validates or invalidates the base class or interfaces of the type and the
    /// type declaration itself. Raises the appropriate compiler messages.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void CheckBaseTypeSemantics()
    {
      SeparateBaseClassAndInterfaces();
      if (HasBaseType)
        _BaseTypeReference.Validate(CheckBaseType());
      CheckBaseInterfaces();
      Validate();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Separates the base class and interfaces of this type declaration
    /// </summary>
    // --------------------------------------------------------------------------------
    public void SeparateBaseClassAndInterfaces()
    {
      if (_InterfaceList.Count > 0 && _InterfaceList[0].RightMostPart.IsResolvedToType &&
        !_InterfaceList[0].RightMostPart.ResolvingType.IsInterface)
      {
        // --- The first element on the interface list is a class type.
        _BaseTypeReference = _InterfaceList[0];
        _BaseType = _InterfaceList[0].RightMostPart.ResolvingType;
        _InterfaceList.RemoveAt(0);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the type specified as a base type can be the base type of this 
    /// declaration.
    /// </summary>
    /// <returns>
    /// True, if the base type is correct; otherwise, false.
    /// </returns>
    /// <remarks>
    /// Raises the appropriate compiler error messages.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool CheckBaseType()
    {
      // --- No base type: it is OK
      if (_BaseType == null) return true;
      bool isOk = CheckBaseTypeAccessibility(_BaseTypeReference.RightMostPart);

      // --- Checks for base types of classes
      if (IsClass)
      {
        // --- Base types of classes cannot be special classes
        if (_BaseType.TypeObject.Equals(typeof(Array)) ||
         _BaseType.TypeObject.Equals(typeof(Delegate)) ||
         _BaseType.TypeObject.Equals(typeof(Enum)) ||
         _BaseType.TypeObject.Equals(typeof(ValueType))
        )
        {
          Parser.Error0644(_BaseTypeReference.Token, Name, _BaseType.FullName);
          return false;
        }

        // --- Base types of classes cannot be sealed types
        if (_BaseType.IsSealed)
        {
          Parser.Error0509(_BaseTypeReference.Token, Name, _BaseType.FullName);
          return false;
        }

        // --- Base types cannot be static
        if (_BaseType.IsStatic)
        {
          Parser.Error0709(_BaseTypeReference.Token, Name, _BaseType.FullName);
          return false;
        }
      }
      // --- Checks for base types of structs
      else if (IsStruct)
      {
        // --- Only interface types allowed on interface list.
        if (!_BaseType.IsInterface)
        {
          Parser.Error0527(_BaseTypeReference.Token, _BaseType.FullName);
          return false;
        }
      }
      // --- Checks for base types of enums
      else if (IsEnum)
      {
        // --- Only integral types are allowed in form of keywords.
        if (_BaseTypeReference.HasSubType ||
          (
            _BaseTypeReference.Name != "sbyte" &&
            _BaseTypeReference.Name != "byte" &&
            _BaseTypeReference.Name != "short" &&
            _BaseTypeReference.Name != "ushort" &&
            _BaseTypeReference.Name != "int" &&
            _BaseTypeReference.Name != "uint" &&
            _BaseTypeReference.Name != "long" &&
            _BaseTypeReference.Name != "ulong"
          ))
        {
          Parser.Error1008(_BaseTypeReference.Token);
          return false;
        }
      }
      // --- Check base types of interfaces
      else if (IsInterface)
      {
        // --- Only interface types allowed on interface list.
        Parser.Error0527(_BaseTypeReference.Token, _BaseType.FullName);
        return false;
      }

      return isOk;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the intrfaces specified are correct interfaces of this declaration.
    /// </summary>
    /// <returns>
    /// True, if the base interface list is correct; otherwise, false.
    /// </returns>
    /// <remarks>
    /// Raises the appropriate compiler error messages.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool CheckBaseInterfaces()
    {
      bool allIsOk = true;
      List<string> baseNames = new List<string>();
      TypeReference classToName = _BaseTypeReference;
      foreach (TypeReference type in _InterfaceList)
      {
        bool isOk = true;
        if (type.RightMostPart.IsResolved)
        {
          if (IsTypeParamOrNamespace(type)) isOk = false;
          else
          {
            // --- Only interfaces are allowed on the interface list
            if (!type.RightMostPart.ResolvingType.IsInterface)
            {
              if (IsClass)
              {
                if (classToName == null)
                {
                  Parser.Error1722(type.Token, type.Name);
                  classToName = type;
                }
                else
                {
                  Parser.Error1721(type.Token, type.Name, classToName.FullName, type.FullName);
                }
              }
              else Parser.Error0527(type.Token, type.FullName);
              isOk = false;
            }
            else
            {
              // --- Interfaces can be only once on the list
              if (baseNames.Contains(type.RightMostPart.ResolvingType.FullName))
              {
                Parser.Error0528(type.Token, type.FullName);
                isOk = false;
              }
              else baseNames.Add(type.RightMostPart.ResolvingType.FullName);
            }
          }
        }
        type.Validate(isOk);
        allIsOk &= isOk;
      }
      return allIsOk;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the specified type is resolved to a type parameter or a namespace.
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>
    /// True, if the type is resolved as a type parameter or a namespace.
    /// </returns>
    /// <remarks>
    /// Raises the appropriate compiler error messages.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool IsTypeParamOrNamespace(TypeReference type)
    {
      TypeReference rightMostPart = type.RightMostPart;

      if (rightMostPart.Target == ResolutionTarget.TypeParameter)
      {
        Parser.Error0689(type.Token, type.ResolvingTypeParameter.Name);
        return true;
      }
      if (rightMostPart.Target == ResolutionTarget.Namespace)
      {
        Parser.Error0118(type.Token, type.FullName, "namespace", "type");
        return true;
      }
      return false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the base type is accessible in the context of the specified type 
    /// declaration.
    /// </summary>
    /// <param name="baseType">Base type to check</param>
    /// <returns>
    /// True, if base type is accessible; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool CheckBaseTypeAccessibility(TypeReference baseType)
    {
      // --- Base interfaces can have any accessibility
      if (baseType.IsInterface) return true;

      // --- Is the base type a referenced type?
      TypeDeclaration baseDecl = baseType.ResolvingType as TypeDeclaration;

      if (baseDecl != null)
      {
        // --- This type has been declared in the source code
        if (
             (IsPublic && baseDecl.IsNotPublic) ||
             (Visibility == Visibility.Protected &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Protected
               )
             ) ||
             (Visibility == Visibility.Internal &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Protected
               )
             ) ||
             (Visibility == Visibility.ProtectedInternal &&
               (
                 baseDecl.Visibility == Visibility.Private ||
                 baseDecl.Visibility == Visibility.Internal ||
                 baseDecl.Visibility == Visibility.Protected
               )
             )
           )
        {
          Parser.Error0060(Token, baseType.FullName, Name);
          return false;
        }
      }
      else
      {
        // --- This type is referenced from an assembly, can be used only if it is
        // --- visible from outside.
        return baseType.ResolvingType.IsVisible;
      }
      return true;
    }

    #endregion

    #region Methods to check type declaration

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if type declaration matches with the declaration rules.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void CheckTypeDeclaration()
    {
      CheckUnallowedModifiers();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if constraint declarations of generic types are OK.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CheckConstraintDeclarations()
    {
      // --- Only generic types have constraints
      if (!IsGenericTypeDefinition) return;

      // --- Check each constraint declaration individually
      foreach (TypeParameterConstraint constraint in _ParameterConstraints)
      {
        // --- Separate constraints to primary, secondary and constructor parts
        constraint.SeparateConstraintElements();
        constraint.CheckClassConstraint();
        constraint.CheckNewWithStruct();

        // --- Go through the secondary elements and check for unallowed constraint types
        List<string> names = new List<string>();
        foreach (ConstraintElement element in constraint.SecondaryConstraints)
        {
          constraint.CheckSecondaryElement(element);

          // --- Check for interface constraint duplication 
          if (element.IsType && element.Type.RightMostPart.IsResolvedToType)
          {
            ITypeCharacteristics resolvingType = element.Type.RightMostPart.ResolvingType;
            if (resolvingType.IsInterface)
            {
              // --- Check for constraint duplication 
              if (names.Contains(resolvingType.FullName))
              {
                Parser.Error0405(element.Token, resolvingType.FullName, constraint.Name);
                element.Invalidate();
              }
              else names.Add(resolvingType.FullName);
            }
          }

          // --- Check for type parameter uniqueness
          if (element.IsType && element.Type.IsResolvedToTypeParameter)
          {
            if (names.Contains(element.Type.Name))
            {
              Parser.Error0405(element.Token, element.Type.Name, constraint.Name);
              element.Invalidate();
            }
            else names.Add(element.Type.Name);
          }
        }
      }

      // --- Check type parameter dependency rules
      foreach (TypeParameter typeParameter in _TypeParameters)
      {
        typeParameter.CheckDependency(_TypeParameters);
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

  #region TypeDeclarationCollection class

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
    /// Fully qualified name of the type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeDeclaration item)
    {
      return item.FullName;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// Adds the specified item to the collection.
    /// </summary>
    /// <param name="item">Item to add to the collection</param>
    // ----------------------------------------------------------------------------------
    public override void Add(TypeDeclaration item)
    {
      TypeDeclaration basePartition;
      if (TryGetValue(GetKeyOfItem(item), out basePartition))
      {
        // --- This item is already in the collection. Add it as a partition
        // --- In a later phase we shall check for duplication.
        if (basePartition.Parts.Count == 0)
        {
          basePartition.Parts.Add(basePartition.CloneToPart());
        }
        basePartition.Parts.Add(item);
      }
      else
      {
        base.Add(item);
      }
    }
  }

  #endregion

  #region Visibility enum

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

  #endregion
}
