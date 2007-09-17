using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents a type parameter used in generic constructions.
  /// </summary>
  // ==================================================================================
  public sealed class TypeParameter : AttributedElement
  {
    #region Private fields

    private TypeParameterConstraint _Constraint;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type parameter according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public TypeParameter(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the constraint related to this type parameter
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameterConstraint Constraint
    {
      get { return _Constraint; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Assigns a constraint to this type parameter.
    /// </summary>
    /// <param name="constraint">Constraint of this type parameter</param>
    // --------------------------------------------------------------------------------
    public void AssignConstraint(TypeParameterConstraint constraint)
    {
      _Constraint = constraint;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of type parameters.
  /// </summary>
  // ==================================================================================
  public class TypeParameterCollection : RestrictedIndexedCollection<TypeParameter>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used for indexing.
    /// </summary>
    /// <param name="item">TypeParameter item.</param>
    /// <returns>
    /// Name of the type parameter.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(TypeParameter item)
    {
      return item.Name;
    }
  }
}
