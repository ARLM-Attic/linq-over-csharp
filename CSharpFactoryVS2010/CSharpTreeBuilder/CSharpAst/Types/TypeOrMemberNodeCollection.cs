// ================================================================================================
// TypeOrMemberNodeCollection.cs
//
// Created: 2009.06.01, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This collection represents the type or member declarations belonging to a type
  /// declaration.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>TypeOrMemberDeclarationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>TypeOrMemberDeclarationNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeOrMemberNodeCollection : SyntaxNodeCollection
    <TypeOrMemberDeclarationNode, TypeDeclarationNode>
  {
  }
}