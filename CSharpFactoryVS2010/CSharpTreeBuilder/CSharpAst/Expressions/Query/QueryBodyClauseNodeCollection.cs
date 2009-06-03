// ================================================================================================
// QueryBodyClauseNodeCollection.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast.Query
{
  // ================================================================================================
  /// <summary>
  ///     This class represent a collection of <see cref="QueryBodyClauseNode"/>
  ///     instances.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>QueryBodyClauseNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>QueryBodyClauseNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class QueryBodyClauseNodeCollection : 
    SyntaxNodeCollection<QueryBodyClauseNode, QueryExpressionNode>
  {
  }
}