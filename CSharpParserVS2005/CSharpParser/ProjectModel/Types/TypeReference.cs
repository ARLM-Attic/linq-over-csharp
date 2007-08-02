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
  /// This type represents a base type on the ancestor list of a type.
  /// </summary>
  // ==================================================================================
  public class TypeReference : LanguageElement, IResolutionRequired
  {
    #region Private fields

    private bool _IsGlobal;
    private TypeReference _SubType;
    private TypeKind _Kind;
    private readonly TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();
    private ResolutionInfo _ResolutionInfo;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type reference instance.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public TypeReference(Token token) : base(token)
    {
      _Kind = TypeKind.simple;
      Name = token.val;
      _ResolutionInfo = new ResolutionInfo();

    #if DIAGNOSTICS
      _Locations.Add(new TypeReferenceLocation(this, CompilationUnit.CurrentLocation));
    #endif
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new type reference instance and immediatelly resolves it to the 
    /// specified type.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="type">Type this reference is resolved to.</param>
    // --------------------------------------------------------------------------------
    public TypeReference(Token token, Type type)
      : this(token)
    {
      ResolveToType(type);
    }

    #endregion

    #region Public properties

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
    /// Gets or sets the direct subtype of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference SubType
    {
      get { return _SubType; }
      set { _SubType = value; }
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
    /// Overrides the name property to use generic arguments
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
          bool firstParam = true;
          foreach (TypeReference paramType in _TypeArguments)
          {
            if (!firstParam) sb.Append(", ");
            sb.Append(paramType.Name);
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
    /// Gets the full name of this type reference.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FullName
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
    /// Gets the information about the type resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionInfo ResolutionInfo
    {
      get { return _ResolutionInfo; }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this type reference
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      // --- Resolve the type argument types
      foreach (TypeReference typeReference in _TypeArguments)
      {
        typeReference.ResolveTypeReferences(contextType, contextInstance);
      }

      // --- Do not resolve the type reference again
      if (_ResolutionInfo.IsResolved) return;

      switch (contextType)
      {
        case ResolutionContext.SourceFile:
          ResolveTypeInSourceFile(contextInstance);
          break;
        case ResolutionContext.Namespace:
          ResolveTypeInNamespace(contextInstance);
          break;
        case ResolutionContext.TypeDeclaration:
          ResolveTypeInTypeDeclaration(contextInstance);
          break;
        case ResolutionContext.MethodDeclaration:
          ResolveTypeInMethodDeclaration(contextInstance);
          break;
        case ResolutionContext.AccessorDeclaration:
          ResolveTypeInAccessorDeclaration(contextInstance);
          break;
        default:
          throw new InvalidOperationException("Invalid resolution context detected.");
      }

      // --- Do not resolve the type reference again
      if (_ResolutionInfo.IsResolved) return;

      _ResolutionInfo.Add(
        new ResolutionItem(ResolutionTarget.Type, ResolutionMode.RuntimeType, this));
      _ResolutionCounter++;

      if (_SubType != null)
      {
        _SubType.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves this type reference to the specified system type.
    /// </summary>
    /// <param name="type">System type.</param>
    // --------------------------------------------------------------------------------
    public void ResolveToType(Type type)
    {
      _ResolutionInfo.Add(
        new ResolutionItem(ResolutionTarget.Type, ResolutionMode.RuntimeType, type));
      _ResolutionCounter++;
      _ResolvedToSystemType++;
    }

    #endregion

    #region Private type resolution methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type within a source file.
    /// </summary>
    /// <param name="instance">Source file instance.</param>
    // --------------------------------------------------------------------------------
    private void ResolveTypeInSourceFile(IResolutionRequired instance)
    {
      SourceFile file = instance as SourceFile;
      if (file == null)
      {
        throw new InvalidOperationException("Source file context expected.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type within a namespace declaration.
    /// </summary>
    /// <param name="instance">Namespace declaration instance.</param>
    // --------------------------------------------------------------------------------
    private void ResolveTypeInNamespace(IResolutionRequired instance)
    {
      NamespaceFragment nameSpace = instance as NamespaceFragment;
      if (nameSpace == null)
      {
        throw new InvalidOperationException("Namspace context expected.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type within a type declaration.
    /// </summary>
    /// <param name="instance">Type declaration instance.</param>
    // --------------------------------------------------------------------------------
    private void ResolveTypeInTypeDeclaration(IResolutionRequired instance)
    {
      TypeDeclaration typeDecl = instance as TypeDeclaration;
      if (typeDecl == null)
      {
        throw new InvalidOperationException("Type declaration context expected.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type within an accessor declaration.
    /// </summary>
    /// <param name="instance">Accessor declaration instance.</param>
    // --------------------------------------------------------------------------------
    private void ResolveTypeInAccessorDeclaration(IResolutionRequired instance)
    {
      AccessorDeclaration accDecl = instance as AccessorDeclaration;
      if (accDecl == null)
      {
        throw new InvalidOperationException("Accessor declaration context expected.");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the type within a method declaration.
    /// </summary>
    /// <param name="instance">Method declaration instance.</param>
    // --------------------------------------------------------------------------------
    private void ResolveTypeInMethodDeclaration(IResolutionRequired instance)
    {
      MethodDeclaration methodDecl = instance as MethodDeclaration;
      if (methodDecl == null)
      {
        throw new InvalidOperationException("Method declaration context expected.");
      }
    }

    #endregion

    #region Diagnostics region

    #if DIAGNOSTICS

    private static int _ResolutionCounter;
    private static int _ResolvedToSystemType;
    private static readonly List<TypeReferenceLocation> _Locations = 
      new List<TypeReferenceLocation>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static int ResolutionCounter
    {
      get { return _ResolutionCounter; }
      set { _ResolutionCounter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the count of references resolved to system types.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static int ResolvedToSystemType
    {
      get { return _ResolvedToSystemType; }
      set { _ResolvedToSystemType = value; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the location of type references.
    /// </summary>
    // --------------------------------------------------------------------------------
    public static List<TypeReferenceLocation> Locations
    {
      get { return _Locations; }
    }
#endif
    
    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a collection of type references.
  /// </summary>
  // ==================================================================================
  public sealed class TypeReferenceCollection : RestrictedCollection<TypeReference>
  { }

  #region Diagnostics

  #if DIAGNOSTICS

  public class TypeReferenceLocation
  {
    private readonly SourceFile _File;
    private readonly TypeReference _Reference;
    
    public TypeReferenceLocation(TypeReference reference, SourceFile file)
    {
      _Reference = reference;
      _File = file;
    }

    public SourceFile File
    {
      get { return _File; }
    }

    public TypeReference Reference
    {
      get { return _Reference; }
    }
  }

  #endif
  
  #endregion
}
