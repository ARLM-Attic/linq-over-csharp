// ================================================================================================
// ITypeParameterHolder.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This interface represents syntax tree nodes that can hold type parameters
  /// </summary>
  // ================================================================================================
  public interface ITypeParameterHolder
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type parameters.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type parameters; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool HasTypeParameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the open sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token OpenSign { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the open sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    void SetOpenSign(Token token);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the close sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token CloseSign { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the close sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    void SetCloseSign(Token token);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    /// <value>The type parameters.</value>
    // ----------------------------------------------------------------------------------------------
    ImmutableCollection<TypeParameterNode> TypeParameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter constraints.
    /// </summary>
    /// <value>The type parameter constraints.</value>
    // ----------------------------------------------------------------------------------------------
    TypeParameterConstraintNodeCollection TypeParameterConstraints { get; }
  }
}