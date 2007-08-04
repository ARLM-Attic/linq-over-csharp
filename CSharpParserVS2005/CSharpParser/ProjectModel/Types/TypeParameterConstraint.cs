using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Collections;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the usable parameter constraint types.
  /// </summary>
  // ==================================================================================
  public enum ParameterConstraintType
  {
    Class,
    Struct,
    Type
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a type parameter constaint used within the context of a
  /// generic type or member definition.
  /// </summary>
  // ==================================================================================
  public sealed class TypeParameterConstraint : LanguageElement, IResolutionRequired
  {
    #region Private fields

    private ParameterConstraintType _ParameterType;
    private readonly List<TypeReference> _Constraints = new List<TypeReference>();
    private bool _HasNew;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new parameter constraint declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser used by this language element.</param>
    // --------------------------------------------------------------------------------
    public TypeParameterConstraint(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of parameter constraint.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ParameterConstraintType ParameterType
    {
      get { return _ParameterType; }
      set { _ParameterType = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if the parameter has a new() constraint.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasNew
    {
      get { return _HasNew; }
      set { _HasNew = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type references of the constraint.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeReference> Constraints
    {
      get { return _Constraints; }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references in this namespace fragment.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      foreach (TypeReference typeReference in _Constraints)
      {
        typeReference.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a list of type parameter constraint that can be indexed 
  /// by the constraint parameter name.
  /// </summary>
  // ==================================================================================
  public class TypeParameterConstraintCollection : 
    RestrictedIndexedCollection<TypeParameterConstraint>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">Namespace item.</param>
    /// <returns>
    /// Full name of the namespace.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeParameterConstraint item)
    {
      return item.Name;
    }
  }
}
