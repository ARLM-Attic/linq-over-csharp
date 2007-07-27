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
  public sealed class TypeReference : LanguageElement, IResolutionRequired
  {
    #region Private fields

    private bool _IsGlobal;
    private TypeReference _SubType;
    private TypeKind _Kind;
    private readonly TypeReferenceCollection _TypeArguments = new TypeReferenceCollection();
    private ResolutionResult _ResolutionResult;
    private ResolutionTarget _ResolutionTarget;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new base type element.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public TypeReference(Token token)
      : base(token)
    {
      _Kind = TypeKind.simple;
      Name = token.val;
      _ResolutionResult = ResolutionResult.Unresolved;
      _ResolutionTarget = ResolutionTarget.Unresolved;
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
    /// Gets the flag indicating if the type has been resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _ResolutionResult != ResolutionResult.Unresolved; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the type has been successfully resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool SuccessfullyResolved
    {
      get { return IsResolved && _ResolutionTarget != ResolutionTarget.Ambiguous; }
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

      // TODO: Resolve this type
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a collection of type references.
  /// </summary>
  // ==================================================================================
  public sealed class TypeReferenceCollection : RestrictedCollection<TypeReference>
  {}
}
