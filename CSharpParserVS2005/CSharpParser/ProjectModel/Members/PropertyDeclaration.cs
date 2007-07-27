using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a property member declaration.
  /// </summary>
  // ==================================================================================
  public class PropertyDeclaration : MemberDeclaration
  {
    #region Private fields

    private AccessorDeclaration _Getter;
    private AccessorDeclaration _Setter;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new property member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public PropertyDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a getter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasGetter
    {
      get { return _Getter != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates if this property has a setter or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasSetter
    {
      get { return _Setter != null; }
    }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "get" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Getter
    {
      get { return _Getter; }
      set { _Getter = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "set" accessor
    /// </summary>
    // --------------------------------------------------------------------------------
    public AccessorDeclaration Setter
    {
      get { return _Setter; }
      set { _Setter = value; }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_Getter != null)
      {
        _Getter.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_Setter != null)
      {
        _Setter.ResolveTypeReferences(contextType, contextInstance);
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
  public class PropertyDeclarationCollection :
    RestrictedIndexedCollection<PropertyDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">PropertyDeclaration item.</param>
    /// <returns>
    /// Name of the property declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(PropertyDeclaration item)
    {
      return item.Signature;
    }
  }
}
