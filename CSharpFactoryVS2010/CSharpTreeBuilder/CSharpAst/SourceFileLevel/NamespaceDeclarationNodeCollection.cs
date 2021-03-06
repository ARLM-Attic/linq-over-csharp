// ================================================================================================
// NamespaceDeclarationNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents a collection of namespace declaration nodes.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>NamespaceDeclarationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>NamespaceDeclarationNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class NamespaceDeclarationNodeCollection :
    SyntaxNodeCollection<NamespaceDeclarationNode, NamespaceScopeNode>
  {
  }
}