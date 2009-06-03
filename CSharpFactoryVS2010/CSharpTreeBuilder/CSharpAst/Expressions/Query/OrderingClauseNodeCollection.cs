// ================================================================================================
// OrderingClauseNodeCollection.cs
//
// Created: 2009.06.03, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>
  ///     This class represents a collection of <see cref="OrderingClauseNode"/>
  ///     instances.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>OrderingClauseNode</em> }</para>
  /// 	</blockquote>
  /// 	<para dir="ltr">Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>OrderingClauseNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class OrderingClauseNodeCollection : 
    SyntaxNodeCollection<OrderingClauseNode, OrderByClauseNode>
  {
  }
}