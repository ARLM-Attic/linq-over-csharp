// ================================================================================================
// ScopeNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of syntax nodes sharing the same logical scope.
  /// </summary>
  /// <remarks>
  /// For example, namespaces and type declarations in a file share the same scope.
  /// </remarks>
  // ================================================================================================
  public class ScopeNodeCollection : SyntaxNodeCollection<ISyntaxNode, NamespaceScopeNode>
  {
  }
}