// ================================================================================================
// NamespaceDeclarationNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a collection of namespace declaration nodes.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceDeclarationNodeCollection :
    ImmutableCollection<NamespaceDeclarationNode>
  {
  }
}