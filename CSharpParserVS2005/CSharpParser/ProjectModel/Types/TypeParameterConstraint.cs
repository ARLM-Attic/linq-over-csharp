using System.Collections.Generic;
using CSharpParser.ParserFiles;

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
  public sealed class TypeParameterConstraint : LanguageElement
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
    // --------------------------------------------------------------------------------
    public TypeParameterConstraint(Token token)
      : base(token)
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

  }
}
