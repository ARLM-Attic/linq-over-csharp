// ================================================================================================
// WhereClauseNode.cs
//
// Created: 2009.06.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents the "where" caluse of a query expression.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>where</strong>" <em>ExpressionNode</em></para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>where</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>ExpressionNode</em>: <see cref="Expression"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class WhereClauseNode : QueryBodyClauseNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="WhereClauseNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public WhereClauseNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expression of the "where" clause.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Expression { get; internal set; }
  }
}