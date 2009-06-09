// ================================================================================================
// InitializerExpressionNodeCollection.cs
//
// Created: 2009.06.08, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  ///     This class represents a collection of <see cref="ExpressionNode"/> instances
  ///     used in an element initializer.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>ExpressionNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>ExpressionNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class InitializerExpressionNodeCollection : 
    SyntaxNodeCollection<ExpressionNode, ElementInitializerNode>
  {
  }
}