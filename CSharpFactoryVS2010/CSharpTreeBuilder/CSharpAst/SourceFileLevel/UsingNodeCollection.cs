// ================================================================================================
// UsingNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a collection of using directive nodes.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>UsingNamespaceNode</em> | <em>UsingAliasNode</em>: Collection
  ///         item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class UsingNodeCollection : SyntaxNodeCollection<UsingNamespaceNode, NamespaceScopeNode>
  {
  }
}