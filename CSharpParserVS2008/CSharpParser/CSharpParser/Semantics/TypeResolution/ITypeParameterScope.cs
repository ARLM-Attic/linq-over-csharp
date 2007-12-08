using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of a scope defining type parameters
  /// </summary>
  /// <remarks>
  /// A type parameter scope is a a type declaration and a method declartation, since
  /// these can declara generic type parameters and constraints.
  /// </remarks>
  // ==================================================================================
  public interface ITypeParameterScope
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameters belonging to this scope.
    /// </summary>
    // --------------------------------------------------------------------------------
    TypeParameterCollection TypeParameters { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter constraints belonging to this scope.
    /// </summary>
    // --------------------------------------------------------------------------------
    List<TypeParameterConstraint> ParameterConstraints { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent type of this parameter scope.
    /// </summary>
    // --------------------------------------------------------------------------------
    TypeDeclaration DeclaringType { get; }
  }
}
