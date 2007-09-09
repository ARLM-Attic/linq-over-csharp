using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a base type on the ancestor list of a type.
  /// </summary>
  // ==================================================================================
  public class TypeReference : LanguageElement, IUsesResolutionContext, ITypeCharacteristics
  {
    #region Private fields

    private bool _IsGlobal;
    private TypeReference _PrefixType;
    private TypeReference _SubType;
    private TypeKind _Kind;
    private readonly TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();
    private ResolutionTarget _Target;
    private ResolutionNodeBase _ResolvingNode;
    private ITypeCharacteristics _ResolvingType;
    private TypeParameter _ResolvingTypeParameter;
    private NamespaceHierarchy _ResolvingHierarchy;

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
    // --------------------------------------------------------------------------------
    public TypeReference(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
      _Kind = TypeKind.simple;
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
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type reference instance and immediatelly resolves it to the 
    /// specified type.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    /// <param name="type">Type this reference is resolved to.</param>
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
    public NamespaceHierarchy ResolvingHierarchy
    {
      get { return _ResolvingHierarchy; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolving type of this name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics ResolvingType
    {
      get { return _ResolvingType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter resolving this name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameter ResolvingTypeParameter
    {
      get { return _ResolvingTypeParameter; }
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
        return (!IsResolved || _ResolvingNode == null)
                 ? Name
                 : _ResolvingNode.FullName;
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
    public bool IsGlobal
    {
      get { return _IsGlobal; }
      set { _IsGlobal = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the prefix type of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference PrefixType
    {
      get { return _PrefixType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the direct subtype of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference SubType
    {
      get { return _SubType; }
      set 
      { 
        _SubType = value;
        if (value != null) _SubType._PrefixType = this;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the prefix type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string PrefixTypeFullName
    {
      get
      {
        return _PrefixType == null
                 ? null
                 : (String.IsNullOrEmpty(_PrefixType.PrefixTypeFullName)
                      ? _PrefixType.Name
                      : _PrefixType.PrefixTypeFullName + "." + PrefixType.Name);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeKind Kind
    {
      get { return _Kind; }
      set { _Kind = value; }
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
    public ResolutionNodeBase ResolvingNode
    {
      get { return _ResolvingNode; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type arguments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReferenceCollection Arguments
    {
      get { return _TypeArguments; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this reference has a subtype or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSubType
    {
      get { return _SubType != null; }
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
        if (_TypeArguments.Count > 0)
        {
          StringBuilder sb = new StringBuilder(100);
          sb.Append(base.Name);
          sb.Append('<');
          if (_TypeArguments.Count > 1) 
            sb.Append(String.Empty.PadRight(_TypeArguments.Count - 1, ','));
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
        if (_TypeArguments.Count > 0)
        {
          return base.Name + "`" + _TypeArguments.Count;
        }
        else
        {
          return base.Name;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the element where the actual type parameters are used.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
    {
      get
      {
        if (_TypeArguments.Count > 0)
        {
          StringBuilder sb = new StringBuilder(100);
          sb.Append(base.Name);
          sb.Append('<');
          bool firstParam = true;
          foreach (TypeReference paramType in _TypeArguments)
          {
            if (!firstParam) sb.Append(", ");
            sb.Append(paramType.ParametrizedName);
            firstParam = false;
          }
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
    /// Gets the namespace of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Namespace
    {
      get { return ResolvingType.Namespace; }
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
      get { return ResolvingType.DeclaringUnit; }
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
      get { return ResolvingType.BaseType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type that declares the current nested type.
    /// </summary>
    /// <remarks>
    /// If there is no declaring type, null should be returned.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics DeclaringType
    {
      get { return ResolvingType.DeclaringType; }
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
        if (HasSubType)
        {
          return string.Format("{0}{1}{2}", TypeName, IsGlobal ? "::" : ".", _SubType.FullName);
        }
        else
        {
          return TypeName;
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type encompasses or refers to 
    /// another type; that is, whether the current Type is an array, a pointer, or is 
    /// passed by reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasElementType
    {
      get { return ResolvingType.HasElementType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is abstract and must be overridden.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return ResolvingType.IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArray
    {
      get { return ResolvingType.IsArray; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a class; that is, not a value 
    /// type or interface.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsClass
    {
      get { return ResolvingType.IsClass; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents an enumeration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEnum
    {
      get { return ResolvingType.IsEnum; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current type is a generic type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericType
    {
      get { return ResolvingType.IsGenericType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type represents a generic type 
    /// definition, from which other generic types can be constructed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsGenericTypeDefinition
    {
      get { return ResolvingType.IsGenericTypeDefinition; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int TypeParameterCount
    {
      get { return _ResolvingType.TypeParameterCount; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an interface; that is, not a 
    /// class or a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInterface
    {
      get { return ResolvingType.IsInterface; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the current Type object represents a type 
    /// whose definition is nested inside the definition of another type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNested
    {
      get { return ResolvingType.IsNested; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and visible only within 
    /// its own assembly.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedAssembly
    {
      get { return ResolvingType.IsNestedAssembly; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is nested and declared private.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPrivate
    {
      get { return ResolvingType.IsNestedPrivate; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a class is nested and declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNestedPublic
    {
      get { return ResolvingType.IsNestedPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is not declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNotPublic
    {
      get { return ResolvingType.IsNotPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPointer
    {
      get { return ResolvingType.IsPointer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is one of the primitive types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPrimitive
    {
      get { return ResolvingType.IsPrimitive; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared public.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsPublic
    {
      get { return ResolvingType.IsPublic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is declared sealed.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return ResolvingType.IsSealed; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a value type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsValueType
    {
      get { return ResolvingType.IsValueType; }
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
      get { return ResolvingType.IsVisible; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a MemberTypes value indicating that this member is a type or a nested 
    /// type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MemberTypes MemberType
    {
      get { return ResolvingType.MemberType; }
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
        if(_Kind == TypeKind.array) return Name + "[]";
        else if (_Kind == TypeKind.pointer) return Name + "*";
        else return Name;
      }  
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is a void type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsVoid
    {
      get { return _Kind == TypeKind.@void; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the name of the rightmost part of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string RightmostName
    {
      get
      {
        TypeReference currentPart = this;
        while (currentPart.HasSubType) currentPart = currentPart.SubType;
        return currentPart.Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rightmost part of ths type reference instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference RightMostPart
    {
      get
      {
        TypeReference currentPart = this;
        while (currentPart.HasSubType) currentPart = currentPart.SubType;
        return currentPart;        
      }
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
    public void ResolveTypeReferences(ResolutionContext contextType, 
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
      _ResolvingType = new NetBinaryType(type);
      _ResolvingNode = null;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
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
      _ResolvingNode = node;
      _ResolvingType = node.Resolver;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
    }


    public void ResolveToNamespaceHierarchy(NamespaceHierarchy hierarchy)
    {
      if (_Target == ResolutionTarget.Unresolved)
      {
        Parser.CompilationUnit.ResolutionCounter++;
        Parser.CompilationUnit.ResolvedToHierarchy++;
      }
      _Target = ResolutionTarget.NamespaceHierarchy;
      _ResolvingNode = null;
      _ResolvingType = null;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
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
      _ResolvingNode = node;
      _ResolvingType = null;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
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
      _ResolvingNode = null;
      _ResolvingType = null;
      _ResolvingTypeParameter = param;
      _ResolvingHierarchy = null;
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
      _ResolvingNode = null;
      _ResolvingType = null;
      _ResolvingTypeParameter = param;
      _ResolvingHierarchy = null;
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
      _ResolvingNode = null;
      _ResolvingType = null;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs this type reference is unresolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Unresolve()
    {
      _Target = ResolutionTarget.Unresolved;
      _ResolvingNode = null;
      _ResolvingType = null;
      _ResolvingTypeParameter = null;
      _ResolvingHierarchy = null;
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
          current = current.SubType;
        }
      }
    }

    #endregion
  }

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
