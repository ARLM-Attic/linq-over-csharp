// ================================================================================================
// QueryExpressionNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a C# 3.0 query expression.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>FromClauseNode</em>
  /// 			<em>QueryBodyNode</em></para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  /// 			<em>FromClauseNode</em>: <see cref="FromClause"/><br/>
  /// 			<em>QueryBodyNode</em>: <see cref="QueryBody"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class QueryExpressionNode : ExpressionNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public QueryExpressionNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets from clause of this query expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FromClauseNode FromClause { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the query body.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public QueryBodyNode QueryBody { get; internal set; }
  }
}