using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a field member declaration.
  /// </summary>
  // ==================================================================================
  public class FieldDeclaration : MemberDeclaration
  {
    #region Private fields

    private Initializer _Initializer;
    private bool _IsEvent;
    private VariableInfo _VariableInfo;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new field member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public FieldDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field has an initializer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return _Initializer != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this field is an event field or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsEvent
    {
      get { return _IsEvent; }
      set { _IsEvent = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer of this field.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer
    {
      get { return _Initializer; }
      set { _Initializer = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the variable information about this field.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableInfo VariableInfo
    {
      get { return _VariableInfo; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// This method sets the variable category according to the field modifiers.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void AfterSetModifiers()
    {
      // TODO: Make a distinction between class and struct types. See Section 12.1.2
      VariableCategory varCat = _IsStatic
        ? VariableCategory.Static
        : VariableCategory.Instance;
      _VariableInfo = new VariableInfo(varCat, _IsStatic, Token);
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_Initializer != null)
      {
        _Initializer.ResolveTypeReferences(contextType, contextInstance);
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
  public class FieldDeclarationCollection : RestrictedIndexedCollection<FieldDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">FieldDeclaration item.</param>
    /// <returns>
    /// Name of the field declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(FieldDeclaration item)
    {
      return item.Name;
    }
  }
}
