using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a reference to a type or namespace name part.
  /// </summary>
  /// <remarks>
  /// <para>
  /// A TypeReference instance represents a whole type, type parameter or namespace or
  /// a part of a compound type or namespace. For example, "int", "System" or "T" can
  /// be instances of a TypeReference. 
  /// </para>
  /// <para>
  /// Compound names cover more TypeReference instances that are bidirectionally 
  /// linked to each other through the Prefix and Suffix properties. Form example,
  /// "System.IO.BinaryReader" is represented by three TypeReference instances:
  /// "System" -> "IO" -> "BinaryReader".
  /// </para>
  /// </remarks>
  // ==================================================================================
  public class TypeReference : TypeBase, 
    IUsesResolutionContext, 
    ITypeAbstraction
  {
    #region Private fields

    /// <summary>Closed with the "::" global scope operator?</summary>
    private bool _IsGlobalScope;

    /// <summary>Prefix type reference (backward link)</summary>
    private TypeReference _Prefix;

    /// <summary>Suffix type reference (forward link)</summary>
    private TypeReference _Suffix;

    /// <summary>Resolved to void?</summary>
    private bool _IsVoid;

    /// <summary>Declared as nullable with "?" operator?</summary>
    private bool _IsNullable;

    /// <summary>Pointer and/or array modifiers attached to the type reference</summary>
    private readonly List<TypeModifier> _TypeModifiers = new List<TypeModifier>();

    /// <summary>Arguments attached to generic types</summary>
    private readonly TypeReferenceCollection _Arguments = new TypeReferenceCollection();

    /// <summary>Target of the resolution</summary>
    private ResolutionTarget _Target;

    /// <summary>Type/Type parameter representing this reference</summary>
    private ITypeAbstraction _TypeInstance;

    /// <summary>Node in the resolution tree that resolved this reference.</summary>
    private ResolutionNodeBase _ResolverNode;

    /// <summary>Hierarchy representing this reference</summary>
    private NamespaceHierarchy _ResolverHierarchy;

    /// <summary>Indicatesif constructed type has already been built</summary>
    private bool _ConstructedTypeBuilt;

    // --- Type reference representing an empty type used in generic parameters
    private static TypeReference _EmptyType;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type reference instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <remarks>
    /// This constructor is used for type references that cannot be resolved at the 
    /// time of their creation.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeReference(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
      Name = token.val;
      Parser.CompilationUnit.Locations.
        Add(new TypeReferenceLocation(this, parser.CompilationUnit.CurrentFile));
      if (_EmptyType == null)
      {
        _EmptyType = this;
        _EmptyType = new TypeReference(token, parser);
        _EmptyType.Name = "<EmptyType>";
        _EmptyType.ResolveToName();
      }
      _ConstructedTypeBuilt = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type reference instance and immediatelly resolves it to the 
    /// specified type.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="type">Type this reference is resolved to.</param>
    /// <remarks>
    /// This constructor is used for type references that can be immediatelly resolved 
    /// at the time of their creation.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeReference(Token token, CSharpSyntaxParser parser, Type type)
      : this(token, parser)
    {
      ResolveToType(type);
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type modifiers belonging to this reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    internal List<TypeModifier> TypeModifiers
    {
      get { return _TypeModifiers; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the static instance representing an empty type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static TypeReference EmptyType
    {
      get { return _EmptyType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolving hierarchy of this name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy ResolverHierarchy
    {
      get { return _ResolverHierarchy; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolving type of this name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction TypeInstance
    {
      get
      {
        if (!IsResolvedToType)
          throw new InvalidOperationException(
            "This reference is not resolved to a type or type parameter.");
        return _TypeInstance;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter resolving this name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameter ResolvingTypeParameter
    {
      get { return _TypeInstance as TypeParameter; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name full name this type is resolved to.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string ResolvingName
    {
      get
      {
        return (!IsResolved || _ResolverNode == null)
                 ? Name
                 : _ResolverNode.FullName;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type reference is resolved or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _Target != ResolutionTarget.Unresolved; }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type reference is resolved to a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToType
    {
      get { return _Target == ResolutionTarget.Type; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type reference is resolved to a type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToTypeParameter
    {
      get
      {
        return _Target == ResolutionTarget.TypeParameter ||
          _Target == ResolutionTarget.MethodTypeParameter;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type reference is resolved to a namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToNamespace
    {
      get { return _Target == ResolutionTarget.Namespace; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if subtype is referenced from "global::".
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGlobalScope
    {
      get { return _IsGlobalScope; }
      set { _IsGlobalScope = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the prefix type of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Prefix
    {
      get { return _Prefix; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the direct suffix of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Suffix
    {
      get { return _Suffix; }
      set 
      { 
        _Suffix = value;
        if (value != null) _Suffix._Prefix = this;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolution target.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionTarget Target
    {
      get { return _Target; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolving node of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase ResolverNode
    {
      get { return _ResolverNode; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type arguments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReferenceCollection Arguments
    {
      get { return _Arguments; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this reference has a suffix or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSuffix
    {
      get { return _Suffix != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the generic name of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get
      {
        if (_Arguments.Count > 0)
        {
          StringBuilder sb = new StringBuilder(100);
          sb.Append(base.Name);
          sb.Append('<');
          if (_Arguments.Count > 1) 
            sb.Append(String.Empty.PadRight(_Arguments.Count - 1, ','));
          sb.Append('>');
          return sb.ToString();
        }
        else
        {
          return base.Name;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the element as the CLR uses it after compilation
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ClrName
    {
      get
      {
        if (_Arguments.Count > 0)
        {
          return base.Name + "`" + _Arguments.Count;
        }
        else
        {
          return base.Name;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace of the type after it has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Namespace
    {
      get
      {
        return TypeInstance.Namespace;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if a type is open or not.
    /// </summary>
    /// <remarks>
    /// A type is open, if directly or indireclty references to a type parametes.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override bool IsOpenType
    {
      get { return IsResolvedToTypeParameter; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public override int GetArrayRank()
    {
      return TypeInstance.GetArrayRank();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element type of this type.
    /// </summary>
    /// <returns>
    /// Element type for a pointer, reference or array; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction GetElementType()
    {
      return TypeInstance.GetElementType();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets an enumerable representing the generic type arguments.
    /// </summary>
    /// <returns>Enumerable representing the types.</returns>
    // --------------------------------------------------------------------------------
    protected override IEnumerable<ITypeAbstraction> GetArguments()
    {
      foreach (TypeReference typeRef in _Arguments) yield return typeRef;
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the types directly nested into this type.
    /// </summary>
    /// <returns>
    /// Dictionary of nested types keyed by the CLR names of the nested types. Empty
    /// dictionary is retrieved if there is no nested type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override Dictionary<string, ITypeAbstraction> GetNestedTypes()
    {
      return TypeInstance.GetNestedTypes();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of an enum type.
    /// </summary>
    /// <remarks>
    /// Throws an exception, if the underlying type is not an enum type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction GetUnderlyingEnumType()
    {
      return TypeInstance.GetUnderlyingEnumType();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference unit where the type is defined.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override ReferencedUnit DeclaringUnit
    {
      get { return TypeInstance.DeclaringUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsUnmanagedType
    {
      get { return TypeInstance.IsUnmanagedType; }
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
    public override ITypeAbstraction BaseType
    {
      get { return TypeInstance.BaseType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction DeclaringType
    {
      get { return TypeInstance.DeclaringType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string FullName
    {
      get
      {
        if (HasSuffix)
          return string.Format("{0}{1}{2}", TypeName, IsGlobalScope 
            ? "::" 
            : ".", _Suffix.FullName);
        else
          return TypeName;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool HasElementType
    {
      get { return TypeInstance.HasElementType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsAbstract
    {
      get { return TypeInstance.IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsArray
    {
      get { return _TypeModifiers.Count > 0 && _TypeModifiers[0] is ArrayModifier; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsClass
    {
      get { return TypeInstance.IsClass; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsEnum
    {
      get { return TypeInstance.IsEnum; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsGenericType
    {
      get { return TypeInstance.IsGenericType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsGenericTypeDefinition
    {
      get { return TypeInstance.IsGenericTypeDefinition; }
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
    public override ITypeAbstraction MakeArrayType(int rank)
    {
      return TypeInstance.MakeArrayType(rank);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Makes a pointer type from the current type with the specified rank.
    /// </summary>
    /// <returns>
    /// Pointer type created from this type.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override ITypeAbstraction MakePointerType()
    {
      return TypeInstance.MakePointerType();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override int TypeParameterCount
    {
      get
      {
        if (_TypeInstance == null) return 0;
        return _TypeInstance.TypeParameterCount;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsInterface
    {
      get { return TypeInstance.IsInterface; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsNested
    {
      get { return TypeInstance.IsNested; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsNotPublic
    {
      get { return TypeInstance.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPointer
    {
      get { return _TypeModifiers.Count > 0 && _TypeModifiers[0] is PointerModifier; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPublic
    {
      get { return TypeInstance.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsSealed
    {
      get { return TypeInstance.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared static.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsStatic
    {
      get { return TypeInstance.IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return TypeInstance.IsValueType; }
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
    public override bool IsVisible
    {
      get { return TypeInstance.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets type name of this type reference (Name modified with the array or 
    /// pointer modifier).
    /// </summary>
    // --------------------------------------------------------------------------------
    public string TypeName
    {
      get
      {
        string pointerMods = String.Empty;
        string arrayMods = string.Empty;

        foreach (TypeModifier typeMod in _TypeModifiers)
        {
          ArrayModifier array = typeMod as ArrayModifier;
          PointerModifier pointer = typeMod as PointerModifier;
          if (pointer != null && arrayMods.Length == 0) pointerMods += "*";
          else if (array != null)
          {
            arrayMods = "[" + String.Empty.PadLeft(array.Rank-1, ',') + "]" + arrayMods;
          }
        }
        return Name + pointerMods + arrayMods;
      }  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a nullable type or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNullable
    {
      get { return _IsNullable; }
      internal set { _IsNullable = value;}
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a void type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVoid
    {
      get { return _IsVoid; }
      internal set { _IsVoid = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rightmost part of ths type reference instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Tail
    {
      get
      {
        TypeReference currentPart = this;
        while (currentPart.HasSuffix) currentPart = currentPart.Suffix;
        return currentPart;        
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the name of the rightmost part of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string TailName
    {
      get { return Tail.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if tail is resolved to a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool TailIsType
    {
      get { return Tail.IsResolvedToType;  }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if tail is resolved to a type parameter.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool TailIsTypeParameter
    {
      get { return Tail.IsResolvedToTypeParameter; }
    }

    #endregion

    #region IUsesResolutionContext implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      // --- Do not resolve the type reference more than once
      if (IsResolved) return;

      // --- Resolve the 'void' type
      if (IsVoid)
      {
        ResolveToType(typeof(void));
        return;
      }

      // --- Resolve named types
      NamespaceOrTypeResolver resolver = new NamespaceOrTypeResolver(Parser);
      NamespaceOrTypeResolverInfo info =
        resolver.Resolve(this, contextType, declarationScope, parameterScope);

      if (info.IsResolved)
      {
        // --- We expect a type and not a namespace.
        if (info.Target == ResolutionTarget.Namespace)
        {
          Parser.Error0118(Token, FullName, "namespace", "type");
          Invalidate();
          return;
        }
      }
      Validate(info.IsResolved);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to the specified system type.
    /// </summary>
    /// <param name="type">System type.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToType(Type type)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToSystemType++;
      }
      _Target = ResolutionTarget.Type;
      _TypeInstance = new NetBinaryType(type);
      _ResolverNode = null;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to the specified abstract type.
    /// </summary>
    /// <param name="type">System type.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToType(ITypeAbstraction type)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToSystemType++;
      }
      _Target = ResolutionTarget.Type;
      _TypeInstance = type;
      TypeDeclaration typeDecl = type as TypeDeclaration;
      _ResolverNode = (typeDecl != null) ? null : typeDecl.ResolverNode;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to the specified source-declared type.
    /// </summary>
    /// <param name="node">Node representing the type.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToType(TypeResolutionNode node)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToSourceType++;
      }
      _Target = ResolutionTarget.Type;
      _ResolverNode = node;
      _TypeInstance = node.Resolver;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to the specified namespace hierarchy.
    /// </summary>
    /// <param name="hierarchy">Namespace hierarchy.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToNamespaceHierarchy(NamespaceHierarchy hierarchy)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToHierarchy++;
      }
      _Target = ResolutionTarget.NamespaceHierarchy;
      _ResolverNode = null;
      _TypeInstance = null;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to a namespace.
    /// </summary>
    /// <param name="node">Node representing the namespace.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToNamespace(ResolutionNodeBase node)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToNamespace++;
      }
      _Target = ResolutionTarget.Namespace;
      _ResolverNode = node;
      _TypeInstance = null;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to a generic type parameter
    /// </summary>
    /// <param name="param">Type parameter this name is resolved to.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToTypeParameter(TypeParameter param)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToNamespace++;
      }
      _Target = ResolutionTarget.TypeParameter;
      _ResolverNode = null;
      _TypeInstance = param;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to a generic method type parameter
    /// </summary>
    /// <param name="param">Type parameter this name is resolved to.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToMethodTypeParameter(TypeParameter param)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToNamespace++;
      }
      _Target = ResolutionTarget.MethodTypeParameter;
      _ResolverNode = null;
      _TypeInstance = param;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to a simple name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void ResolveToName()
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToName++;
      }
      _Target = ResolutionTarget.Name;
      _ResolverNode = null;
      _TypeInstance = null;
      _ResolverHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this type reference is unresolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Unresolve()
    {
      _Target = ResolutionTarget.Unresolved;
      _ResolverNode = null;
      _TypeInstance = null;
      _ResolverHierarchy = null;
    }

    #endregion

    #region Constructed type handling

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Builds up a constructed type defined by the type modifiers.
    /// </summary>
    // --------------------------------------------------------------------------------
    internal void BuildConstructedType()
    {
      if (_ConstructedTypeBuilt) return;
      if (_TypeInstance == null) return;

      _ConstructedTypeBuilt = true;
      
      // --- Create the nullable type
      if (IsNullable)
      {
        _TypeInstance = new NullableType(_TypeInstance);
      }

      // --- Create pointer types
      ITypeAbstraction type = _TypeInstance;
      int ptrIndex = 0;
      while (ptrIndex < _TypeModifiers.Count && _TypeModifiers[ptrIndex] is PointerModifier)
      {
        if (!type.IsUnmanagedType)
        {
          Parser.Error0208(Token, type.FullName);
          return;
        }
        type = type.MakePointerType();
        ptrIndex++;
      }

      // --- Create array types from right to left
      int arrIndex = _TypeModifiers.Count - 1;
      while (arrIndex >= ptrIndex)
      {
        ArrayModifier array = _TypeModifiers[arrIndex] as ArrayModifier;
        if (array == null)
        {
          // TODO: Sign error!
        }
        else
        {
          type = type.MakeArrayType(array.Rank);
        }
        arrIndex--;
      }
      _TypeInstance = type;
    }

    #endregion

    #region Iterators

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterates through the parts of the name represented by this instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public IEnumerable<TypeReference> NameParts
    {
      get
      {
        TypeReference current = this;
        while (current != null)
        {
          yield return this;
          current = current.Suffix;
        }
      }
    }

    #endregion
  }

  #region TypeModifier and related classes

  // ==================================================================================
  /// <summary>
  /// This abstract class represents a common type modifier (array, pointer, etc.)
  /// </summary>
  // ==================================================================================
  public abstract class TypeModifier { }

  // ==================================================================================
  /// <summary>
  /// This class represents a pointer modifier
  /// </summary>
  // ==================================================================================
  public sealed class PointerModifier : TypeModifier { }

  public sealed class ArrayModifier : TypeModifier
  {
    private readonly int _Rank;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an array modifier with the specified rank.
    /// </summary>
    /// <param name="rank">Rank of the array.</param>
    // --------------------------------------------------------------------------------
    public ArrayModifier(int rank)
    {
      _Rank = rank;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Rank
    {
      get { return _Rank; }
    }
  }

  #endregion

  // ==================================================================================
  /// <summary>
  /// This type represents a collection of type references.
  /// </summary>
  // ==================================================================================
  public sealed class TypeReferenceCollection : RestrictedCollection<TypeReference>
  { }

  #region TypeReferenceLocation class

  // ==================================================================================
  /// <summary>
  /// This type represents a location for a type reference.
  /// </summary>
  // ==================================================================================
  public class TypeReferenceLocation
  {
    #region Private fields

    private readonly SourceFile _File;
    private readonly TypeReference _Reference;

    #endregion

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new location instance
    /// </summary>
    /// <param name="reference">Type reference</param>
    /// <param name="file">File containing the type reference.</param>
    // --------------------------------------------------------------------------------
    public TypeReferenceLocation(TypeReference reference, SourceFile file)
    {
      _Reference = reference;
      _File = file;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file declaring the type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile File
    {
      get { return _File; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type reference represented by this location
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Reference
    {
      get { return _Reference; }
    }
  }
  
  #endregion
}
