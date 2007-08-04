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
  public class FieldDeclaration : MemberDeclaration, IVariableInfo
  {
    #region Private fields

    private Initializer _Initializer;
    private bool _IsEvent;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new field member declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public FieldDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
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
    /// Defines the category of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public VariableCategory Category
    {
      get { return _IsStatic ? VariableCategory.Static : VariableCategory.Instance; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsInitiallyAssigned
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int DeclarationPosition
    {
      get { return Token.pos; }
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
