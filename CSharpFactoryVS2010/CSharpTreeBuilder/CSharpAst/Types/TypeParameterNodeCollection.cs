// ================================================================================================
// TypeParameterNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a collection of <see cref="TypeParameterNode"/> instances.
  /// </summary>
  /// <remarks>
  /// This class is used to hold type parameters belonging to a type or member
  /// definition.
  /// </remarks>
  // ================================================================================================
  public class TypeParameterNodeCollection : ImmutableCollection<TypeParameterNode>
  {
  }
}