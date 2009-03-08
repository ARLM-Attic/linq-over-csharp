using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of a language element handling
  /// type parameters.
  /// </summary>
  // ==================================================================================
  public interface ITypeParameterOwner
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter to the language element.
    /// </summary>
    /// <param name="parameter">Type parameter to add.</param>
    // --------------------------------------------------------------------------------
    void AddTypeParameter(TypeParameter parameter);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter constraint to the type declaration
    /// </summary>
    /// <param name="constraint">Type parameter constraint</param>
    // --------------------------------------------------------------------------------
    void AddTypeParameterConstraint(TypeParameterConstraint constraint);
  }
}

