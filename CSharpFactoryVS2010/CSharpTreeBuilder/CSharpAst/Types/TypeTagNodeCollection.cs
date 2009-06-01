// ================================================================================================
// TypeTagNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a collection of type tag nodes.</summary>
  /// <remarks>
  /// 	<para>
  ///         For example, if we have the following type name: "System.Text.Encoding", this
  ///         is represented by three <see cref="TypeTagNode"/> instances for "System",
  ///         "Text" and "Encoding", respectively.
  ///     </para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>TypeTagNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>TypeTagNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeTagNodeCollection : SyntaxNodeCollection<TypeTagNode, TypeOrNamespaceNode>
  {
  }
}