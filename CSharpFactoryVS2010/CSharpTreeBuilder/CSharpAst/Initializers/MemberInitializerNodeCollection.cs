// ================================================================================================
// MemberInitializerNodeCollection.cs
//
// Created: 2009.06.08, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  ///     This class represents a collection of <see cref="MemberInitializerNode"/>
  ///     instances.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>MemberInitializerNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>MemberInitializerNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class MemberInitializerNodeCollection : 
    SyntaxNodeCollection<MemberInitializerNode, ObjectOrCollectionInitializerNode>
  {
  }
}