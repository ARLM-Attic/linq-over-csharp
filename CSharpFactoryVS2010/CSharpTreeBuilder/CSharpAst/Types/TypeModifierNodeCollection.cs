// ================================================================================================
// TypeModifierNodeCollection.cs
//
// Created: 2009.05.31, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Represents a collection of <see cref="TypeModifierNode"/> instances.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>TypeModifierNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>TypeModifierNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeModifierNodeCollection: 
    SyntaxNodeCollection<TypeModifierNode, TypeOrNamespaceNode>
  {
  }
}