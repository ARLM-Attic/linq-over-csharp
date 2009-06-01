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
  /// 	<para>For example, namespaces and type declarations in a file share the same
  ///     scope.</para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>NamspaceDeclarationNode</em> | <em>TypeDeclarationNode</em>
  ///         }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>NamspaceDeclarationNode</em> | <em>TypeDeclarationNode</em>:
  ///         Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class ScopeNodeCollection : SyntaxNodeCollection<ISyntaxNode, NamespaceScopeNode>
  {
  }
}